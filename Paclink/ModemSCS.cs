using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{
    public class ModemSCS : IModem
    {
        // This Class handles all PTC II TNCs (PTCII, IIe, IIex, IIpro, IIusb, DR-7800) for both Packet and Pactor connections
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private struct ConnectedCall
        {
            public string Callsign;                    // Callsign of the connected call
            public int ConnectionID;               // The connection ID (unique)
            public bool PendingDisconnect;          // Flag to indicate a pending disconnect on this Connection 
            public int Port;                       // Port number of the connected call
            public DateTime StartDisconnect;               // Time disconnect countdown started to delay disconnect 
        }

        // Integer
        private int intConnectScriptPtr = -1;     // Pointer to the active connect script line, -1 disables scripting
        private int intScriptTimer = -1;          // Used to time out a stuck connection script
        private int intConnectTimer;              // For Pactor connect timeouts
        private int intActivityTimer;             // For activity timeouts
        private int intBytesSentCount;            // Holds last bytes sent count

        // Bytes
        private byte bytLink;                         // Connected link state
        private byte bytMode;                         // 0 Standby, &H20 ARQ, &H40 FEC, &H70 Busy
        private byte bytDirection;                    // True for ISS else False
        private bool blnSendingID;                 // Set while an FEC ID is being sent
        private static bool blnDoFECId;            // Set at end of a successful link or unsuccessful selcal

        // Strings
        private string strCommandReply;               // String reply from a command
        private string[] ConnectScript;               // An array of connection script entries
        private string strStatus;                     // Holds plain language status

        // Boolean
        private bool blnHostMode;                  // Indicates TNC in host mode
        private bool blnCommandReply;              // Flag indicating reply received TNC was a command
        private bool blnAutomaticConnect;          // Flag used to indicate if connection was initiated automatically
        private bool blnNoIdentification;          // Flag set to inhibit FEC Id
        private bool blnWaitingForManualConnect;
        private bool blnNormalDisconnect;
        private bool blnIDSending;                 // Flag to indicate FEC ID is being sent
        private bool blnStandby;                   // Flag used to signal standby status used in FEC ID
        private bool blnDisconnectProcessed;       // Flag used to keep from processing multiple disconnects

        // Objects and classes 
        private ProtocolInitial objProtocol;          // Instance of the message protocol handler
        private ConnectedCall stcConnectedCall;       // Details of a connected link
        private SCSHostPort _objHostPort;   // Instance of the SCS host mode port

        private SCSHostPort objHostPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objHostPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_objHostPort != null)
                {
                    _objHostPort.OnActivity -= OnLinkActivity;
                    _objHostPort.OnPTCControl -= OnPTCControl;
                    _objHostPort.OnPTCData -= OnPTCData;
                    _objHostPort.OnPTCError -= OnPTCError;
                    _objHostPort.OnPTCStatusReport -= OnPTCStatusReport;
                }

                _objHostPort = value;
                if (_objHostPort != null)
                {
                    _objHostPort.OnActivity += OnLinkActivity;
                    _objHostPort.OnPTCControl += OnPTCControl;
                    _objHostPort.OnPTCData += OnPTCData;
                    _objHostPort.OnPTCError += OnPTCError;
                    _objHostPort.OnPTCStatusReport += OnPTCStatusReport;
                }
            }
        }

        // Enums and Structures
        private LinkStates enmState = LinkStates.Undefined;

        public void Abort()
        {
            // Closes the channel immediately...

            if (enmState != LinkStates.Connected)
            {
                Close();
                return;
            }

            blnNoIdentification = true;
            if (objHostPort is object)
            {
                SendCommand("D", 31); // Normal disconnect
                Thread.Sleep(2000);
                objHostPort.Close();
                Thread.Sleep(Globals.intComCloseTime);
                objHostPort = null;
            }

            enmState = LinkStates.LinkFailed;
            Close();
        } // Abort
          // Closes the channel and put TNC into known state.
          // Always call this method before the instance goes out of scope...

        private bool blnClosing = false;

        public bool Close()
        {
            if (blnClosing)
                return true;
            blnClosing = true;
            if (!(enmState == LinkStates.Disconnected | enmState == LinkStates.LinkFailed | enmState == LinkStates.NoSerialPort))
            {
                ImmediateDisconnect();
            }

            try
            {
                Globals.queChannelDisplay.Enqueue("G*** Closing " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                if (Globals.objRadioControl is object) // Shut down the radio control and free the serial port
                {
                    Globals.objRadioControl.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    Globals.objRadioControl = null;
                }

                if (objHostPort is object && objHostPort.HostState == SCSHostPort.HostModeState.HostMode)
                {
                    // Wait for all serial data to be sent before closing serial port...
                    var dttStartClose = DateTime.Now;
                    while (DateTime.Now.Subtract(dttStartClose).TotalMilliseconds < 500 & blnSendingID)
                        Thread.Sleep(100);
                    objHostPort.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    objHostPort = null;
                }

                if (objProtocol is object)
                {
                    objProtocol.CloseProtocol();
                    objProtocol = null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("[PTCIIClient.Close] " + ex.Message);
            }

            if (objHostPort != null)
            {
                objHostPort.Close();
                Thread.Sleep(Globals.intComCloseTime);
                objHostPort = null;
            }

            Globals.queStateDisplay.Enqueue("");
            Globals.queStatusDisplay.Enqueue("Idle");
            Globals.queRateDisplay.Enqueue("------");
            blnClosing = false;
            Globals.blnChannelActive = false;
            return true;
        } // Close 

        public bool Connect(bool blnAutomatic)
        {
            // Handles new connection...

            if (Globals.blnPactorDialogResuming == false)
                Globals.queChannelDisplay.Enqueue(Globals.CLEAR);
            string strVia = "";
            string strCenterFreq;
            int intIndex;
            string longPathStr = "%";
            if (Globals.stcSelectedChannel.PactorUseLongPath)
            {
                longPathStr = "!";
            }
            string strTarget = Globals.stcSelectedChannel.RemoteCallsign;
            blnAutomaticConnect = blnAutomatic;
            if (enmState != LinkStates.Initialized)
            {
                try
                {
                    if (OpenSerialPort() == false)
                    {
                        enmState = LinkStates.NoSerialPort;
                        return false;
                    }
                }
                catch
                {
                    enmState = LinkStates.NoSerialPort;
                    return false;
                }

                try
                {
                    if (!InitializeTheTNC())
                    {
                        enmState = LinkStates.LinkFailed;
                        Close();
                        return false;
                    }
                    else
                    {
                        enmState = LinkStates.Initialized;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("[PTCIIClient.Connect B] " + ex.Message);
                }
            }

            try
            {
                // Modified to work for Pactor and Packet...
                if (Globals.stcSelectedChannel.RDOControl == "Serial" | Globals.stcSelectedChannel.RDOControl == "Via PTCII")
                {
                    Globals.ObjScsModem = this;
                    if (Globals.objRadioControl == null)
                    {
                        if (Globals.stcSelectedChannel.RDOModel.StartsWith("Kenwood"))
                        {
                            Globals.objRadioControl = new RadioKenwood();
                        }
                        else if (Globals.stcSelectedChannel.RDOModel.StartsWith("Icom"))
                        {
                            Globals.objRadioControl = new RadioIcom();
                        }
                        else if (Globals.stcSelectedChannel.RDOModel.StartsWith("Yaesu"))
                        {
                            Globals.objRadioControl = new RadioYaesu();
                        }
                        else if (Globals.stcSelectedChannel.RDOControl == "Serial" & Globals.stcSelectedChannel.RDOModel.StartsWith("Micom"))
                        {
                            Globals.objRadioControl = new RadioMicom();
                        }
                        else
                        {
                            blnNoIdentification = true;
                            Globals.queChannelDisplay.Enqueue("R*** Failure setting Radio control for radio " + Globals.stcSelectedChannel.RDOModel);
                            return false;
                        }

                        if (!Globals.objRadioControl.InitializeSerialPort(ref Globals.stcSelectedChannel))
                        {
                            blnNoIdentification = true;
                            Globals.queChannelDisplay.Enqueue("R*** Failure initializing Radio Control");
                            Log.Error("[PTCIIClient.Connect C] Failure initializing Radio Control");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[PTCIIClient.Connect D] " + ex.Message);
            }

            try
            {
                if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    SendCommand("#ST 0", 31);
                    // note the followig is needed to set the packet call sign ....the PAC MYCALL does not appear to work correctly
                    // SendCommand("#MYCALL " & SiteCallsign, 31)
                    // SendCommand("#PAC MYCALL " & SiteCallsign, 31)
                    SendCommand("I" + Globals.stcSelectedChannel.TNCPort.ToString() + ":" + Globals.SiteCallsign, Globals.stcSelectedChannel.TNCPort);
                }
                else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    SendCommand("#ST 2", 31);
                    string strPactorCallsign = Globals.SiteCallsign;
                    if (Globals.blnForceHFRouting)
                    {
                        if (Globals.SiteCallsign.Length == 7 & Globals.SiteCallsign.Contains("-") == false)
                        {
                            // Add 'T' as 8th character without a dash
                            strPactorCallsign += "T";
                        }
                        else
                        {
                            // Add "-T"
                            strPactorCallsign += "-T";
                        }
                    }

                    SendCommand("#MYCALL " + strPactorCallsign, 31);
                    SendCommand("#FSKA " + Globals.stcSelectedChannel.TNCFSKLevel.ToString(), 31);
                    SendCommand("#PSKA " + Globals.stcSelectedChannel.TNCPSKLevel.ToString(), 31);
                }
            }
            catch (Exception ex)
            {
                Log.Error("[PTCIIClient.Connect E] " + ex.Message);
            }

            var strTemp = default(string);
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC) // This handle packet connections
            {
                try
                {
                    if (Globals.objRadioControl != null)
                        Globals.objRadioControl.SetParameters(ref Globals.stcSelectedChannel);
                    if (ConnectScript != null && ConnectScript.Length > 0) // TODO: needs testing
                    {
                        if (RunScript(ref strVia, ref strTarget)) // Activates scripting, modifies sVia and sTarget
                        {
                            stcConnectedCall.Callsign = strTarget;
                            strTemp = "C " + Globals.stcSelectedChannel.TNCPort.ToString() + ":" + strTarget + strVia;
                            // Set for 60 second timeout...
                            intConnectTimer = 60;
                        }
                        else
                        {
                            Globals.queChannelDisplay.Enqueue("R*** Script Error - ending connection");
                            Globals.blnChannelActive = false;
                            enmState = LinkStates.LinkFailed;
                            Globals.ObjSelectedModem = null;
                            return false;
                        }
                    }
                    else
                    {
                        // Set for 60 second timeout...
                        intConnectTimer = 60;
                        Globals.queRateDisplay.Enqueue("Linking");
                        Globals.queChannelDisplay.Enqueue("G*** Starting Packet connection to " + Globals.stcSelectedChannel.RemoteCallsign + " on port " + Globals.stcSelectedChannel.TNCPort.ToString());
                        strTemp = "C " + Globals.stcSelectedChannel.TNCPort.ToString() + ":" + Globals.stcSelectedChannel.RemoteCallsign;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("[PTCIIClient.Connect F] " + ex.Message);
                }
            }
            else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
            {
                try
                {
                    strCenterFreq = Globals.ExtractFreq(Globals.stcSelectedChannel.RDOCenterFrequency);
                    intIndex = strCenterFreq.IndexOf('(');
                    if (intIndex > 0)
                    {
                        strCenterFreq = strCenterFreq.Substring(0, intIndex - 1);
                    }

                    float tmpVal = 0.0F;
                    if (!float.TryParse(strCenterFreq, out tmpVal) | string.IsNullOrEmpty(Globals.stcSelectedChannel.RemoteCallsign.Trim()) | !blnAutomaticConnect)
                    {

                        // This handles manual pactor connections or unspecified automatic channels...
                        if (Globals.dlgPactorConnect != null)
                        {
                            Globals.dlgPactorConnect.CloseWindow();
                            Globals.dlgPactorConnect = null;
                        }

                        blnWaitingForManualConnect = true;
                        DialogPactorConnectViewModel vm = null;
                        if (Globals.dlgPactorConnect == null)
                        {
                            if (!string.IsNullOrEmpty(Globals.stcEditedSelectedChannel.RemoteCallsign))
                            {
                                vm = new DialogPactorConnectViewModel(this, ref Globals.stcEditedSelectedChannel);
                            }
                            else
                            {
                                vm = new DialogPactorConnectViewModel(this, ref Globals.stcSelectedChannel);
                            }
                        }

                        Globals.dlgPactorConnect = (IPactorConnectWindow)UserInterfaceFactory.GetUiSystem().CreateForm(AvailableForms.PactorConnect, vm);

                        blnWaitingForManualConnect = false;
                        if (Globals.dlgPactorConnect.ShowModal() == UiDialogResult.Cancel)
                        {
                            UserInterfaceFactory.GetUiSystem().GetMainForm().RefreshWindow();
                            blnNoIdentification = true;
                            if (objProtocol != null)
                            {
                                objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                                objProtocol = null;
                            }

                            Globals.dlgPactorConnect.CloseWindow();
                            Globals.dlgPactorConnect = null;
                            blnNoIdentification = true;
                            enmState = LinkStates.LinkFailed;
                            return false;
                        }

                        // This updates the channel parameters...
                        Globals.dlgPactorConnect.UpdateChannelProperties();
                        Globals.dlgPactorConnect.CloseWindow();
                        Globals.dlgPactorConnect = null;
                        if (!Globals.blnPactorDialogResuming)
                            Globals.stcEditedSelectedChannel = default;
                    }
                    else
                    {
                        // Set the radio parameters for an automatic channel...
                        if (Globals.objRadioControl != null)
                            Globals.objRadioControl.SetParameters(ref Globals.stcSelectedChannel);
                        if (Globals.stcSelectedChannel.TNCBusyHold)
                        {
                            // The following waits up to 30 sec for a 6 second clear channel before starting an auto forward...
                            blnWaitingForManualConnect = true;
                            Globals.queChannelDisplay.Enqueue("G*** Waiting for clear channel condition...");
                            var intClearChannelMonitor = default(int);
                            var dttClearChannelWait = DateTime.Now.AddSeconds(30);
                            do
                            {
                                if (dttClearChannelWait < DateTime.Now)
                                    break;
                                Thread.Sleep(100);
                                Poll();
                                if (strStatus != "Channel Busy")
                                    intClearChannelMonitor += 1;
                                if (intClearChannelMonitor > 60)
                                    break;
                            }
                            while (true);
                            blnWaitingForManualConnect = false;
                            if (intClearChannelMonitor <= 60)
                            {
                                Globals.queChannelDisplay.Enqueue("R*** Channel busy - autoforwarding ended");
                                blnNoIdentification = true;
                                enmState = LinkStates.LinkFailed;
                                return false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("[PTCIIClient.Connect G] " + ex.Message);
                }

                try
                {
                    Globals.queRateDisplay.Enqueue("Linking");
                    Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign + " on " + Globals.ExtractFreq(Globals.stcSelectedChannel.RDOCenterFrequency) + " KHz");
                    strTemp = "C " + longPathStr + Globals.stcSelectedChannel.RemoteCallsign;
                    // intConnectTimer = CInt(Math.Max(15, 7 * stcChannel.FrequenciesScanned)) ' Set for 15 seconds min or 7 sec/freq
                    intConnectTimer = 60;
                    objHostPort.blnISS = true;  // Precondition to ISS 
                    blnIDSending = false;
                    blnNoIdentification = false;
                    blnDisconnectProcessed = false;
                }
                catch (Exception ex)
                {
                    Log.Error("[PTCIIClient.Connect H] " + ex.Message);
                }
            }

            try
            {
                SendCommand(strTemp, Globals.stcSelectedChannel.TNCPort);
                blnNormalDisconnect = false;
                blnDisconnectProcessed = false;
                intActivityTimer = 0;
                enmState = LinkStates.Connecting;
                SendCommand("%B", Globals.stcSelectedChannel.TNCPort);
                SendCommand("M N", Globals.stcSelectedChannel.TNCPort);
            }
            catch (Exception ex)
            {
                Log.Error("[PTCIIClient.Connect J] " + ex.Message);
            }

            return true;
        } // Connect 

        private string ConnectTarget(string Script)
        {
            // Extracts the target call from the connect script...

            string strTemp;
            int intPt;
            strTemp = Script.Trim().ToUpper();
            intPt = strTemp.IndexOf("C ");
            if (intPt != -1)
            {
                strTemp = strTemp.Substring(intPt + 1).Trim();
            }
            else
            {
                intPt = strTemp.IndexOf("CONNECT ");
                if (intPt != 0)
                {
                    strTemp = strTemp.Substring(intPt + 7).Trim();
                }
                else
                {
                    return "";
                }
            }

            return strTemp.Split(' ')[0].Trim();
        } // ConnectTarget

        public void DataToSend(string sData)
        {
            // Adds data to the outbound queue...

            var objEncoder = new ASCIIEncoding();
            DataToSend(Globals.GetBytes(sData));
        } // DataToSend (String) 

        public void DataToSend(byte[] bytData)
        {
            // Adds data to the outbound queue.

            intActivityTimer = 0;
            try
            {
                var switchExpr = Globals.stcSelectedChannel.TNCPort;
                switch (switchExpr)
                {
                    case 31: // Pactor
                        {
                            // Reset the confirmed data count...  
                            SendCommand("%T", Globals.stcSelectedChannel.TNCPort);
                            Poll();
                            if (bytData.Length > 0)
                            {
                                objHostPort.DataToSend(bytData, Globals.stcSelectedChannel.TNCPort);
                                objHostPort.Poll();
                            } // Packet

                            break;
                        }

                    default:
                        {
                            if (bytData.Length > 0)
                            {
                                objHostPort.DataToSend(bytData, Globals.stcSelectedChannel.TNCPort);
                                objHostPort.Poll();
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[PTCIIClient.SendData] Port=" + Globals.stcSelectedChannel.TNCPort.ToString() + " - " + ex.Message);
            }
        }  // DataToSend (Byte()) 

        public void Disconnect()
        {
            // Invokes a channel disconnect...

            if (stcConnectedCall.PendingDisconnect | enmState == LinkStates.Connecting)
            {
                ImmediateDisconnect();
                if (objProtocol != null)
                {
                    objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                    objProtocol = null;
                    enmState = LinkStates.Disconnected;
                }
                else
                {
                    enmState = LinkStates.LinkFailed;
                }
            }
            else
            {
                stcConnectedCall.PendingDisconnect = true;
                stcConnectedCall.StartDisconnect = DateTime.Now;
                SendCommand("D", Globals.stcSelectedChannel.TNCPort);
            }

            Globals.queProgressDisplay.Enqueue(0);
        } // Disconnect 

        private bool EndScript(string strText)
        {
            // Tests for any script bailouts...

            var strEndText = new string[] { "DISCONNECTED", "TIMEOUT", "EXCEEDED", "FAILURE", "BUSY" };
            for (int intIndex = 0, loopTo = (strEndText.Length - 1); intIndex <= loopTo; intIndex++)
            {
                if (strText.ToUpper().IndexOf(strEndText[intIndex]) != -1)
                    return true;
            }

            return false;
        } // EndScript

        private bool ImmediateDisconnect()
        {
            // Function to send an immediate disconnect command to the PTC II in host mode...

            // Returns True if sucessful, False otherwise
            var dttStart = DateTime.Now;
            blnCommandReply = false;
            strCommandReply = "";
            try
            {
                if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    SendCommand("#DD", Globals.stcSelectedChannel.TNCPort);
                }
                else // Two packet disconnects for packet mode
                {
                    SendCommand("D", Globals.stcSelectedChannel.TNCPort);
                    Thread.Sleep(100);
                    SendCommand("D", Globals.stcSelectedChannel.TNCPort);
                }
            }
            catch
            {
            }

            // Wait for reply?
            while (DateTime.Now.Subtract(dttStart).TotalMilliseconds < 1200 & !blnCommandReply)
            {
                if (objHostPort != null)
                    objHostPort.Poll();
                Thread.Sleep(50);
            }

            return true;
        } // ImmediateDisconnect

        private bool InitializeTheTNC()
        {
            // Function to initialize the PTC II TNC...

            if (Globals.blnPactorDialogResuming == false)
            {
                Globals.queChannelDisplay.Enqueue("G*** Initializing the " + Globals.stcSelectedChannel.TNCType + " TNC");
            }

            // Parse the connection script, if any, into the ConnectScript string array...
            if (!string.IsNullOrEmpty(Globals.stcSelectedChannel.TNCScript))
            {
                ConnectScript = Globals.stcSelectedChannel.TNCScript.Replace(Globals.LF, "").Split(Convert.ToChar(Globals.CR));
            }

            if (objHostPort.Startup())
            {
                if (Globals.blnPactorDialogResuming == false)
                {
                    Globals.queChannelDisplay.Enqueue("G*** TNC " + Globals.stcSelectedChannel.TNCType + " on serial port " + Globals.stcSelectedChannel.TNCSerialPort + " initialized OK");
                }

                blnHostMode = true;
                if (blnDoFECId)
                {
                    blnDoFECId = false;
                    SendIdentification();
                }

                return true;
            }
            else
            {
                Globals.queChannelDisplay.Enqueue("R*** TNC " + Globals.stcSelectedChannel.TNCType + " on serial port " + Globals.stcSelectedChannel.TNCSerialPort + " initialization failed");
                return false;
            }
        } // Initialize

        public ModemSCS()
        {
            Globals.blnChannelActive = true;
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                Globals.queStateDisplay.Enqueue("Packet");
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
                Globals.blnPactorDialogResume = !Globals.stcSelectedChannel.EnableAutoforward;
        } // New

        public bool NormalDisconnect
        {
            get
            {
                return blnNormalDisconnect;
            }

            set
            {
                blnNormalDisconnect = value;
            }
        } // NormalDisconnect

        private void OneSecondEvent()
        {
            // One second event for script timing, Pactor connect, timeouts.
            // Called by ClientPTCII.Poll()...

            if (!blnWaitingForManualConnect)
                intActivityTimer += 1;
            if (intActivityTimer > 60 * Globals.stcSelectedChannel.TNCTimeout)
            {
                Globals.queChannelDisplay.Enqueue("R*** " + Globals.stcSelectedChannel.TNCTimeout.ToString() + " minute activity timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC"));
                ImmediateDisconnect();
                var dttStartDisconnect = DateTime.Now;
                while (DateTime.Now.Subtract(dttStartDisconnect).TotalMilliseconds < 2000)
                {
                    Thread.Sleep(100);
                    objHostPort.Poll();
                }

                // The following allows the Pactor manual user to retry a connect with a different PMBO/frequency
                if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & !blnAutomaticConnect & !blnNormalDisconnect & enmState != LinkStates.Initialized)
                {
                    enmState = LinkStates.Initialized; // This allows bypassing the initialization 
                    Globals.queRateDisplay.Enqueue("------");
                    return;
                }
                // All other timeout attempts take this path
                if (objProtocol != null)
                {
                    objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                    objProtocol = null;
                }

                enmState = LinkStates.LinkFailed;
                return;
            }

            if (intConnectScriptPtr != -1)
            {
                intScriptTimer += 1;
                if (intScriptTimer > Globals.stcSelectedChannel.TNCScriptTimeout)
                {
                    intConnectScriptPtr = -1;
                    Globals.queChannelDisplay.Enqueue("R #Connect Script Timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC") + " - disconnecting");
                    ImmediateDisconnect();
                    enmState = LinkStates.LinkFailed;
                }
            }
            else if (intConnectTimer > 0)
            {
                intConnectTimer -= 1;
                if (intConnectTimer <= 0)
                {
                    intConnectTimer = 0;

                    // Connect Timeout condition
                    Globals.queChannelDisplay.Enqueue("R*** Connection timeout");
                    blnStandby = false; // Preset standby flag false
                    ImmediateDisconnect();
                    if (objProtocol != null)
                    {
                        objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                        objProtocol = null;
                    }

                    enmState = LinkStates.LinkFailed;
                    Globals.blnStartingChannel = true;
                    return;
                }
            }
            else if (stcConnectedCall.PendingDisconnect & DateTime.Now.Subtract(stcConnectedCall.StartDisconnect).TotalSeconds > 10)
            {
                ImmediateDisconnect();
                Thread.Sleep(500);
                if (blnNormalDisconnect)
                {
                    enmState = LinkStates.Disconnected;
                }
                else
                {
                    enmState = LinkStates.LinkFailed;
                }
            }

            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & objHostPort.blnISS & objProtocol is object)
            {
                // Request bytes sent report...
                SendCommand("%T", Globals.stcSelectedChannel.TNCPort);
            }
            else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
            {
                SendCommand("L", Globals.stcSelectedChannel.TNCPort);
            }
        } // OneSecondEvent

        private void OnLinkActivity()
        {
            intActivityTimer = 0;
        } // OnLinkActivity

        private void OnPTCControl(string strData)
        {
            // Receives control command responses from the SCS host port...

            if (string.IsNullOrEmpty(strData))
                return;

            // Process reply from L request for channel information.
            string strReport = "";
            var strTokens = strData.Split(' ');
            if (strTokens.Length == 6)
            {
                // -- Response to L command --
                // v0 v1 v2 v3 v4 v5
                // v0 = Number of link status messages not yet displayed
                // v1 = Number of receive frames not yet displayed
                // v2 = Number of send frames not yet transmitted
                // v3 = Number of transmitted frames not yet acknowledged
                // v4 = Number of tries on current operation
                // v5 = Link state
                strTokens[5] = strTokens[5].Substring(0, strTokens[5].Length - 1);
                int tmpVal = 0;
                if (int.TryParse(strTokens[5], out tmpVal))
                {
                    switch (tmpVal)
                    {
                        case 0:
                            {
                                strReport = "Packet - Disconnected";
                                break;
                            }

                        case 1:
                            {
                                strReport = "Packet - Linking";
                                break;
                            }

                        case 2:
                            {
                                strReport = "Packet - Frame Reject";
                                break;
                            }

                        case 3:
                            {
                                strReport = "Packet - Disconnect Request";
                                break;
                            }

                        case 4:
                            {
                                strReport = "Packet - Information Transfer";
                                break;
                            }

                        case 5:
                            {
                                strReport = "Packet - Reject Frame Sent";
                                break;
                            }

                        case 6:
                            {
                                strReport = "Packet - Waiting Acknowledgement";
                                break;
                            }

                        case 7:
                            {
                                strReport = "Packet - Device Busy";
                                break;
                            }

                        case 8:
                            {
                                strReport = "Packet - Remote Device Busy";
                                break;
                            }

                        case 9:
                            {
                                strReport = "Packet - Both Devices Busy";
                                break;
                            }

                        case 10:
                            {
                                strReport = "Packet - Waiting Acknowledgement and Device Busy";
                                break;
                            }

                        case 11:
                            {
                                strReport = "Packet - Waiting Acknowledgement and Remote Busy";
                                break;
                            }

                        case 12:
                            {
                                strReport = "Packet - Waiting Acknowledgement and Both Devices Busy";
                                break;
                            }

                        case 13:
                            {
                                strReport = "Packet - Reject Frame Send and Device Busy";
                                break;
                            }

                        case 14:
                            {
                                strReport = "Packet - Reject Frame Send and Remote Busy";
                                break;
                            }

                        case 15:
                            {
                                strReport = "Packet - Reject Frame Send and Both Devices Busy";
                                break;
                            }
                    }

                    // Update the count of pending frames in the TNC.
                    if (int.TryParse(strTokens[2], out tmpVal))
                        objHostPort.intTNCFramesPending = tmpVal;
                    return;
                }
            }

            // Process reply from a %T request for Pactor bytes sent...
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & enmState == LinkStates.Connected)
            {
                int tmpVal = 0;
                if (int.TryParse(strData, out tmpVal))
                {
                    int intNewBytesSentCount = tmpVal;
                    if (intNewBytesSentCount > 0)
                    {
                        if (intBytesSentCount > intNewBytesSentCount)
                            intBytesSentCount = intNewBytesSentCount;
                        Globals.UpdateProgressBar(intNewBytesSentCount - intBytesSentCount);
                        if (intNewBytesSentCount - intBytesSentCount > 0)
                        {
                            intActivityTimer = 0; // Reset the inactivity counter if bytes flowing
                            intBytesSentCount = intNewBytesSentCount;
                            objHostPort.intTNCBytesSent = intNewBytesSentCount;
                        }
                    }

                    return;
                }
            }

            // See if this is a response to a L command to check link state.
            else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
            {
                if (!string.IsNullOrEmpty(strReport))
                {
                    // Include byte transfer counts if available...
                    Globals.queStateDisplay.Enqueue(strReport + "  " + Globals.ProgressBarStatus());
                    return;
                }
            }

            var intChannel = default(int);
            int intPtr1 = strData.IndexOf("(");
            int intPtr2 = strData.IndexOf(")");
            if (intPtr1 != -1 & intPtr2 > intPtr1)
            {
                try
                {
                    intChannel = Convert.ToInt32(strData.Substring(intPtr1 + 1, intPtr2 - (intPtr1 + 1)));
                }
                catch
                {
                }
            }

            if (strData.StartsWith("*** "))
                Globals.queChannelDisplay.Enqueue("P" + strData);
            if (strData.IndexOf("*** Host Mode") != -1)
            {
                blnHostMode = true;
            }
            else if (strData.IndexOf("*** PTC Fault") != -1 | strData.IndexOf("*** Serial Port Fault") != -1)
            {
                enmState = LinkStates.LinkFailed;
            }
            else if (strData.IndexOf("CONNECTED to") != -1)
            {
                strData = strData.Replace("(31)", "").Trim();
                Globals.queChannelDisplay.Enqueue("P" + strData + "  @ " + Globals.TimestampEx());
                stcConnectedCall.Callsign = strData.Substring(12 + strData.IndexOf("CONNECTED to")).Trim();
                intConnectTimer = 0; // Reset the ConnectCtr
                if (intChannel == 31) // A Pactor connect
                {
                    enmState = LinkStates.Connected;
                    intActivityTimer = 0;
                    intBytesSentCount = 0;
                    objHostPort.intTNCBytesSent = 0;
                    objHostPort.intTNCBytesPosted = 0;
                    objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                }
                else if (intChannel != 0) // A packet connect connection ID will be in intChannel
                {
                    try
                    {
                        var switchExpr1 = Globals.stcSelectedChannel.TNCType;
                        switch (switchExpr1)
                        {
                            case "PTC-II":
                            case "PTC-IIpro":
                                {
                                    stcConnectedCall.Port = Convert.ToInt32(stcConnectedCall.Callsign.Substring(0, 1));
                                    stcConnectedCall.Callsign = stcConnectedCall.Callsign.Substring(2).Trim();
                                    break;
                                }

                            case "PTC-IIe":
                            case "PTC-IIex":
                            case "PTC-IIusb":
                                {
                                    stcConnectedCall.Port = 1;
                                    break;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("[PTCIIClient.OnPTCControl] " + ex.Message);
                        return;
                    }

                    enmState = LinkStates.Connected;
                    objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                }
                else
                {
                    return;
                }
            }
            else if (strData.IndexOf("DISCONNECTED") != -1 | strData.IndexOf("LINK FAIL") != -1)
            {
                strData = strData.Replace("(31)", "").Trim();
                if (!blnDisconnectProcessed)
                {
                    blnDisconnectProcessed = true;
                    Globals.queChannelDisplay.Enqueue("P" + strData + "  @ " + Globals.TimestampEx());
                    if (objProtocol != null & Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                    {
                        // A connection was established which created objProtocol...
                        objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                        objProtocol = null;
                        if (strData.IndexOf("LINK FAIL") != -1)
                        {
                            enmState = LinkStates.LinkFailed;
                        }
                        else
                        {
                            enmState = LinkStates.Disconnected;
                        }
                    }
                    else if (objProtocol != null & Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & (blnAutomaticConnect | blnNormalDisconnect))
                    {
                        // A connection was established which created objProtocol
                        // SendIdentification()
                        blnDoFECId = true;
                        objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                        objProtocol = null;
                        if (strData.IndexOf("LINK FAIL") != -1)
                        {
                            enmState = LinkStates.LinkFailed;
                        }
                        else
                        {
                            enmState = LinkStates.Disconnected;
                        }
                    }
                    else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & !blnAutomaticConnect & !blnNormalDisconnect & enmState != LinkStates.Initialized)
                    {
                        // Not automatic so allow selecting another RMS and or frequency...
                        enmState = LinkStates.Initialized; // This allows bypassing the Initialization
                                                            // SendIdentification()
                        blnDoFECId = true;
                        if (objProtocol is object)
                        {
                            objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                            objProtocol = null;
                        }

                        Globals.queRateDisplay.Enqueue("------");
                        enmState = LinkStates.Disconnected;
                        return;
                    }
                    else
                    {
                        enmState = LinkStates.LinkFailed;
                    }
                }
                else
                {
                    return;
                } // Disconnect already processed

                if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & !blnAutomaticConnect & enmState != LinkStates.Initialized & Globals.blnPactorDialogResume)
                {
                    // Not automatic so allow selecting another RMS Frequency...
                    Globals.queRateDisplay.Enqueue("------");
                }
            }
            else if (strData.IndexOf("Current port settings") != -1)
            {
                if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    if (strData.IndexOf("9600") != -1)
                    {
                        Globals.queRateDisplay.Enqueue("9600 Baud");
                    }
                    else
                    {
                        Globals.queRateDisplay.Enqueue("1200 Baud");
                    }
                }
            }
            else
            {
                strCommandReply = strData;
                blnCommandReply = true;
            }
        } // OnPTCControl

        private void OnPTCData(byte[] bytData)
        {
            // Receives channel data from the SCS host port...

            var bytPureData = new byte[bytData.Length - 4 + 1];
            Array.Copy(bytData, 3, bytPureData, 0, bytPureData.Length);
            intActivityTimer = 0; // Reset the Inbound counter with each data frame
            if (intConnectScriptPtr != -1) // Still sequencing the connecction script
            {
                if (!SequenceScript(Globals.GetString(bytPureData), stcConnectedCall.Callsign))
                    return;
            }

            if (objProtocol is object)
                objProtocol.ChannelInput(ref bytPureData);
        } // OnPTCData

        private void OnPTCError(string strText)
        {
            Globals.queChannelDisplay.Enqueue("P*** " + strText);
            Log.Error("[PTCIIClient.OnPTCError] " + strText);
        } // OnPTCError

        private bool blnConnected = false;

        private void OnPTCStatusReport(byte[] bytStatus)
        {
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
            {
                if (bytStatus.Length == 4 | bytStatus.Length == 7)
                {
                    // Decode the basic status byte...
                    bytLink = (byte)(bytStatus[3] & Convert.ToByte(0x7));
                    bytMode = (byte)(bytStatus[3] & Convert.ToByte(0x70));
                    var switchExpr = bytMode;
                    switch (switchExpr)
                    {
                        case 0:
                            {
                                strStatus = "Standby ";
                                blnStandby = true;
                                if (Globals.dlgPactorConnect is object)
                                    Globals.dlgPactorConnect.ChannelBusy = false;
                                break;
                            }

                        case 0x20:
                            {
                                strStatus = "Pactor ARQ ";
                                blnStandby = false;
                                break;
                            }

                        case 0x40:
                            {
                                strStatus = "Pactor FEC ";
                                blnStandby = false;
                                break;
                            }

                        case 0x70:
                            {
                                blnStandby = false;
                                strStatus = "Channel Busy ";
                                if (Globals.dlgPactorConnect is object)
                                    Globals.dlgPactorConnect.ChannelBusy = true;
                                break;
                            }

                        default:
                            {
                                strStatus = "Packet ";
                                break;
                            }
                    }

                    bytDirection = (byte)(bytStatus[3] & Convert.ToByte(0x8));
                    if ((int)bytDirection > 0)
                        objHostPort.blnISS = true;
                    else
                        objHostPort.blnISS = false;
                    if ((int)bytMode == 0x20)
                    {
                        if ((int)bytDirection > 0)
                            strStatus += "Sending ";
                        else
                            strStatus += "Receiving ";
                        var switchExpr1 = bytLink;
                        switch (switchExpr1)
                        {
                            case 0:
                            case 1:
                                {
                                    strStatus += "Repeating ";
                                    break;
                                }

                            case 2:
                                {
                                    strStatus += "Traffic ";
                                    break;
                                }

                            case 3:
                                {
                                    strStatus += "Idle ";
                                    break;
                                }

                            case 4:
                                {
                                    strStatus += "Over ";
                                    break;
                                }

                            case 5:
                                {
                                    strStatus += "Phasing ";
                                    break;
                                }

                            case 6:
                                {
                                    strStatus += "Linking ";
                                    break;
                                }

                            case 7:
                                {
                                    strStatus += "Calling ";
                                    break;
                                }
                        }
                    }
                }

                if (bytStatus.Length == 7)
                {
                    // Decode the extended status report...
                    if ((int)bytStatus[4] == 0)
                    {
                        if (blnConnected)
                        {
                            blnConnected = false;
                        }

                        objHostPort.blnISS = false;
                        if (stcConnectedCall.PendingDisconnect)
                        {
                            if (objProtocol != null)
                            {
                                objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                                objProtocol = null;
                                enmState = LinkStates.Disconnected;
                            }
                            else
                            {
                                enmState = LinkStates.LinkFailed;
                            }
                        }
                    }
                    else
                    {
                        var switchExpr2 = bytStatus[4];
                        switch (switchExpr2)
                        {
                            case 1:
                                {
                                    var switchExpr3 = bytStatus[5];
                                    switch (switchExpr3)
                                    {
                                        case 0:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 100");
                                                break;
                                            }

                                        case 1:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 200");
                                                break;
                                            }
                                    }

                                    break;
                                }

                            case 2:
                                {
                                    var switchExpr4 = bytStatus[5];
                                    switch (switchExpr4)
                                    {
                                        case 0:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 100");
                                                break;
                                            }

                                        case 1:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 200");
                                                break;
                                            }

                                        case 2:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 400");
                                                break;
                                            }

                                        case 3:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 800");
                                                break;
                                            }

                                        default:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 800");
                                                break;
                                            }
                                    }

                                    break;
                                }

                            case 3:
                                {
                                    var switchExpr5 = bytStatus[5];
                                    switch (switchExpr5)
                                    {
                                        case 0:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 200");
                                                break;
                                            }

                                        case 1:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 800");
                                                break;
                                            }

                                        case 2:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 1400");
                                                break;
                                            }

                                        case 3:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 2800");
                                                break;
                                            }

                                        case 4:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 3200");
                                                break;
                                            }

                                        case 5:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 3600");
                                                break;
                                            }

                                        default:
                                            {
                                                Globals.queRateDisplay.Enqueue("P" + bytStatus[4].ToString() + " - 3600");
                                                break;
                                            }
                                    }

                                    break;
                                }
                        }

                        if (enmState != LinkStates.Connected & (int)bytStatus[4] != 0)
                        {
                            enmState = LinkStates.Connected;
                            intActivityTimer = 0;
                            objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                        }
                    }

                    if ((int)bytStatus[6] != 0x80)
                    {
                        int intSign;
                        if ((int)bytStatus[6] >= 0x80)
                            intSign = -1;
                        else
                            intSign = 1;
                        int intOffset = (int)bytStatus[6] & 0x7F;
                        if (intSign == -1)
                            intOffset = (int)(intOffset | 0xFFFFFF80);
                        strStatus += "Offset: " + intOffset.ToString() + " Hz";
                    }
                }

                Globals.queStateDisplay.Enqueue(strStatus + "  " + Globals.ProgressBarStatus());
            }
        } // OnPTCStatusReport

        public bool OpenSerialPort()
        {
            // Instantiate a new SCSHostPort...
            try
            {
                if (objHostPort != null)
                {
                    objHostPort.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    objHostPort = null;
                }

                objHostPort = new SCSHostPort();

                // Open the host port's serial port...
                if (!objHostPort.Open())
                {
                    // If blnPactorDialogResuming = False Then
                    Globals.queChannelDisplay.Enqueue("R*** TNC " + Globals.stcSelectedChannel.TNCType + " initialization failed on serial port " + Globals.stcSelectedChannel.TNCSerialPort);
                    Globals.queChannelDisplay.Enqueue("R*** Serial port may be in use by another application");
                    // End If
                    objHostPort = null;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("[Client.OpenSerialPort] " + ex.Message);
                objHostPort = null;
                return false;
            }
        } // OpenSerialPort
          // Called from Main at 100 millisecond intervals...

        private int intCalls = 0;
        public void Poll()
        {
            intCalls += 1;
            if (intCalls >= 10)
            {
                intCalls = 0;
            }

            if (objHostPort is object)
            {
                if (Globals.dlgPactorConnect is object)
                {
                    if (intCalls == 0)
                        OneSecondEvent();
                }
                else if (intCalls == 0)
                {
                    // Request status here...
                    objHostPort.PostQuery();
                }

                objHostPort.Poll();
            }
        } // Poll 

        private bool RunScript(ref string strVia, ref string strTarget)
        {
            bool RunScriptRet = default;
            // Starts the connection script. Returns True if OK False if script or processing error...

            try
            {
                RunScriptRet = true;
                intScriptTimer = 0; // initialize the script timer
                strVia = "";
                if (ConnectScript == null)
                {
                    intConnectScriptPtr = -1; // This sets the script pointer to signal inactive
                    return true; // No script. Do not update strTarget.
                }
                else
                {
                    int intPt;
                    string strTemp;
                    string strTok;
                    strTemp = " " + ConnectScript[0].ToUpper().Trim();

                    // This strips off any leading V or Via (case insensitive) and skips over any syntax "Connect via"
                    intPt = strTemp.IndexOf(" V ");
                    if (intPt != -1)
                    {
                        strVia = " " + strTemp.Substring(intPt + 2).Trim().Replace(",", " "); // ' For PTC II replace "," via delimiters with space
                        strVia = strVia.Replace("  ", " "); // remove any double spaces
                    }
                    else
                    {
                        intPt = strTemp.IndexOf(" VIA ");
                        if (intPt != -1)
                        {
                            strVia = " " + strTemp.Substring(intPt + 4).Trim().Replace(",", " "); // ' For PTC II replace "," via delimiters with space
                            strVia = strVia.Replace("  ", " "); // remove any double spaces
                        }
                    }

                    if ((ConnectScript.Length - 1) == 0)
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Requesting connection to " + Globals.stcSelectedChannel.RemoteCallsign + " via " + strVia + " at " + Globals.Timestamp());

                        // Simple via connect, just a single line (not a true script)
                        intConnectScriptPtr = -1; // Set script pointer to inactive, strVia is updated, strTarget is unchanged
                        return true;
                    }
                    else
                    {
                        // True script processing here (indicated by at least two Connection script lines)
                        intConnectScriptPtr = 0; // Initialize ptr to first script line (signals the script is active)
                        intActivityTimer = 0;
                        strTok = ConnectTarget(ConnectScript[0]);
                        if (!string.IsNullOrEmpty(strTok))
                        {
                            strTarget = strTok; // sTarget is updated 
                            Globals.queChannelDisplay.Enqueue("G #Begin Connection Script");
                            Globals.queChannelDisplay.Enqueue("G     #Script(" + (intConnectScriptPtr + 1).ToString() + "): " + ConnectScript[intConnectScriptPtr]);
                        }
                        else
                        {
                            RunScriptRet = false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return RunScriptRet;
        } // RunScript

        private void SendCommand(byte[] bytCommand, int intChannel)
        {
            // Send a command to the PTC via the control channel...

            if (objHostPort is object && objHostPort.HostState == SCSHostPort.HostModeState.HostMode)
            {
                blnCommandReply = false;
                try
                {
                    objHostPort.SendHostCommandPacket(bytCommand, Convert.ToByte(intChannel));
                    objHostPort.Poll();
                    var dttStart = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Log.Error("[PTCHostMode.SendCommand(Byte)] " + ex.Message);
                }
            }
        }  // SendCommand (Byte)

        private void SendCommand(string strCommand, int intChannel)
        {
            // Send a command to the PTC via the control channel...

            if (objHostPort is object && objHostPort.HostState == SCSHostPort.HostModeState.HostMode)
            {
                blnCommandReply = false;
                try
                {
                    objHostPort.SendHostCommandPacket(strCommand, Convert.ToByte(intChannel));
                    objHostPort.Poll();
                    var dttStart = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Log.Error("[PTCHostMode.SendCommand(String)] " + ex.Message + " Command:" + intChannel.ToString() + "/" + strCommand);
                }
            }
        }  // SendCommand (String)

        public void SendRadioCommand(byte[] bytCommand)
        {
            // Function to send radio command via PTC II...
            SendCommand(bytCommand, 31);
        } // SendRadioCommand (Byte)

        public void SendRadioCommand(string strCommand)
        {
            // Function to send radio command via PTC II...
            SendCommand(strCommand, 31);
        } // SendRadioCommand (String)

        private void SendIdentification()
        {
            stcConnectedCall.PendingDisconnect = false;
            if (blnSendingID | Globals.stcSelectedChannel.ChannelType != ChannelMode.PactorTNC | !Globals.stcSelectedChannel.PactorId | blnNoIdentification | blnIDSending)
                return; // Prevents sending ID twice
            blnSendingID = true;
            if (objHostPort is object && objHostPort.HostState == SCSHostPort.HostModeState.HostMode)
            {
                // Sends station callsign in Pactor I FEC...
                SendCommand("#CL", 31);     // Clear the transmit buffer
                SendCommand("#U *1", 31);   // Repeat 1 time
                SendCommand("#U 1", 31);    // Use FSK 100 baud mode
                Thread.Sleep(1000);
                DataToSend(" DE " + Globals.SiteCallsign + " ");

                // Wait five seconds...
                var intTicks = default(int);
                do
                {
                    intTicks += 1;
                    if (intTicks > 30)
                        break;
                    objHostPort.Poll();
                    Thread.Sleep(100);
                }
                while (true);

                // End FEC transmission...
                SendCommand("#DD", 31);
                intTicks = 0;
                do
                {
                    intTicks += 1;
                    if (intTicks > 10)
                        break;
                    objHostPort.Poll();
                    Thread.Sleep(100);
                }
                while (true);
            }

            blnSendingID = false;
        } // SendIdentification

        private bool SequenceScript(string Text, string From)
        {
            bool SequenceScriptRet = default;
            // Sequences the connect script. Returns True if scripting is completed, False otherwise...

            var strDataToSend = default(string);
            bool blnTextFound;
            var switchExpr = intConnectScriptPtr;
            switch (switchExpr)
            {
                case -1:  // No scripting
                    {
                        if (Text.StartsWith("***"))
                        {
                            Globals.queChannelDisplay.Enqueue("P" + Text);
                        }
                        else
                        {
                            Globals.queChannelDisplay.Enqueue("X" + Text);
                        }

                        enmState = LinkStates.Connected;
                        objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                        return true;  // Script Ptr should always be even
                    }

                default:
                    {
                        Globals.queChannelDisplay.Enqueue("X" + Text);
                        if (EndScript(Text)) // Check for aborted script
                        {
                            Globals.queChannelDisplay.Enqueue("G #Script stopped: " + Text);
                            intConnectScriptPtr = -1; // Terminate the scripting
                            ImmediateDisconnect();
                            return false;
                        }
                        else if (ConnectScript.Length > intConnectScriptPtr + 1)
                        {
                            blnTextFound = Text.ToUpper().Contains(ConnectScript[intConnectScriptPtr + 1].Trim().ToUpper());
                            if (blnTextFound & intConnectScriptPtr + 2 < ConnectScript.Length)
                            {
                                intScriptTimer = 0; // Reset the script timer
                                Globals.queChannelDisplay.Enqueue("G     #Script(" + (intConnectScriptPtr + 2).ToString() + "): " + ConnectScript[intConnectScriptPtr + 1]);
                                intConnectScriptPtr += 2;
                                strDataToSend = ConnectScript[intConnectScriptPtr];
                                Globals.queChannelDisplay.Enqueue("G     #Script(" + (intConnectScriptPtr + 1).ToString() + "): " + strDataToSend);
                            }
                            else if (blnTextFound)
                            {
                                intScriptTimer = 0;
                                Globals.queChannelDisplay.Enqueue("G     #Script(" + (intConnectScriptPtr + 2).ToString() + "): " + ConnectScript[intConnectScriptPtr + 1]);
                                Globals.queChannelDisplay.Enqueue("G #End Script");
                                enmState = LinkStates.Connected;
                                intConnectScriptPtr = -1; // Terminate the scripting
                                objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                                SequenceScriptRet = true;
                            }
                        }
                        else // Must be an odd number of scrip lines 
                        {
                            Globals.queChannelDisplay.Enqueue("G #Script terminated (end of script file): " + Text);
                            enmState = LinkStates.Connected;
                            intConnectScriptPtr = -1; // Terminate the scripting
                            objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                            SequenceScriptRet = true;
                        }

                        break;
                    }
            }

            if (!string.IsNullOrEmpty(strDataToSend))
            {
                byte[] bytData;
                var objEncoder = new ASCIIEncoding();
                bytData = Globals.GetBytes(strDataToSend + Globals.CR);

                // Send the data here...
                DataToSend(bytData);
            }

            return SequenceScriptRet;
        } // SequenceScript

        public LinkStates State
        {
            get
            {
                return enmState;
            }
        } // State 
    }

    public class SCSHostPort
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public enum HostModeState
        {
            HostModeClosed,
            PTCStart,
            PTCInitializing,
            PTCFault,
            HostMode,
            SerialPortFault
        }

        // Function declarations...
        // Declare Function UPDCRC Lib "VBSUPPORT.DLL" (ByVal value As Byte, ByVal CRC As Integer) As Integer

        // Events...
        public event OnPTCErrorEventHandler OnPTCError;

        public delegate void OnPTCErrorEventHandler(string strData);

        public event OnPTCStatusReportEventHandler OnPTCStatusReport;

        public delegate void OnPTCStatusReportEventHandler(byte[] bytStatus);

        public event OnPTCControlEventHandler OnPTCControl;

        public delegate void OnPTCControlEventHandler(string strData);

        public event OnPTCDataEventHandler OnPTCData;

        public delegate void OnPTCDataEventHandler(byte[] bytData);

        public event OnPTCRadioEventHandler OnPTCRadio;

        public delegate void OnPTCRadioEventHandler(byte[] bytData);

        public event OnActivityEventHandler OnActivity;

        public delegate void OnActivityEventHandler();

        // Queues...
        private Queue quePortInput = Queue.Synchronized(new Queue()); // Holds raw byte received from serial port
        private Queue queDataOutbound = Queue.Synchronized(new Queue());      // Holds complete data host frames queued for the serial port
        private Queue queCommandOutbound = Queue.Synchronized(new Queue());   // Holds complete command host frames queued for the serial port

        // Structures and enumerations...
        private HostModeState enmHostState;     // Holds the current host port state     

        // Byte arrays...
        private byte[] bytSOH = new byte[] { 0x1 }; // Used in attempt to recover the TNC
        private byte[] bytExitCRCExtendedHostMode = new byte[] { 0xAA, 0xAA, 0x1F, 0xC1, 0x5, 0x4A, 0x48, 0x4F, 0x53, 0x54, 0x30, 0x54, 0xFA };
        private byte[] bytRepeatRequest = new byte[] { 0xAA, 0xAA, 0xAA, 0x55 };

        // Strings and string builders
        private StringBuilder sbdResponse = new StringBuilder();

        // Integers
        public int intTNCFramesPending;
        public int intTNCBytesSent;
        public int intTNCBytesPosted;

        // Booleans...
        public bool blnISS;
        private bool blnAcknowledged;  // Set when a frame from the TNC is successfully decoded
        private bool blnCmdSeen;       // Set when a "cmd:" is received from the TNC (while not in host mode)
        private bool blnAsteriskSeen;   // Set when a "*" is received from the TNC (while not in host mode) indicating TNC emulation mode 2
        private bool blnPacSeen;       // Set when a "pac" is received from the TNC (while not in host mode)
        private bool blnHostSync;      // Set when "INVALID" is received from the TNC (while not in host mode)
        private bool blnRepeatRequest; // Set when a repeat request frame is received from the TNC
        private bool blnClose;         // Set when host mode is ended

        // Objects and classes...
        private SerialPort _objSerial; // Serial port to the TNC

        private SerialPort objSerial
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objSerial;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_objSerial != null)
                {
                    _objSerial.DataReceived -= OnDataReceived;
                }

                _objSerial = value;
                if (_objSerial != null)
                {
                    _objSerial.DataReceived += OnDataReceived;
                }
            }
        }

        public void Abort()
        {
            SendHostCommandPacket("%C", 31);
            queDataOutbound.Clear();
        } // Abort

        private void AddCRC(ref byte[] bytPendingFrame)
        {
            // Calculates and sets the CRC values into the last two bytes of the
            // bytPendingFrame...
            try
            {
                int intCRC = 0xFFFF;
                for (int intIndex = 2, loopTo = bytPendingFrame.Length - 3; intIndex <= loopTo; intIndex++)
                    intCRC = Crc.UpdCrc(bytPendingFrame[intIndex], intCRC);
                intCRC = ~intCRC;
                bytPendingFrame[bytPendingFrame.Length - 2] = Convert.ToByte(intCRC & 0xFF);
                bytPendingFrame[bytPendingFrame.Length - 1] = Convert.ToByte((intCRC & 0xFF00) / 256);
            }
            catch (Exception ex)
            {
                Log.Error("[SCSHostPort.AddCRC] " + ex.Message);
            }
        } // AddCRC

        private void AddStuffingBytes(ref byte[] bytPendingFrame)
        {
            // Begin byte stuffing at location 2 (skip 170, 170 header)
            // This insures two sequential bytes of 170 identifies Start of header

            var bytNewFrame = new byte[1001];
            int intPosition = 2;
            var blnAAFound = default(bool);
            int intIndex;
            var loopTo = bytPendingFrame.Length - 1;
            for (intIndex = 2; intIndex <= loopTo; intIndex++)
            {
                if (bytPendingFrame[intIndex] != 0xAA) // No change use exisiting data...
                {
                    bytNewFrame[intPosition] = bytPendingFrame[intIndex];
                    intPosition += 1;
                }
                else // Add a 0 byte stuff after the &HAA to insure never two adjacent &HAA bytes... 
                {
                    blnAAFound = true;
                    bytNewFrame[intPosition] = bytPendingFrame[intIndex];
                    bytNewFrame[intPosition + 1] = 0;
                    intPosition += 2;
                }
            }

            if (blnAAFound == false)
            {
                return;
            }
            else
            {
                bytPendingFrame = new byte[intPosition];
                bytNewFrame[0] = 0xAA;
                bytNewFrame[1] = 0xAA;
                Array.Copy(bytNewFrame, bytPendingFrame, intPosition);
            }
        } // AddStuffingBytes

        private bool CheckCRC(ref byte[] bytFrame, int intUpperbound)
        {
            // Checks the CRC on a received host mode data frame...

            int intCRC = 0xFFFF;
            for (int intIndex = 2, loopTo = intUpperbound - 2; intIndex <= loopTo; intIndex++)
                intCRC = Crc.UpdCrc(bytFrame[intIndex], intCRC);
            intCRC = ~intCRC;
            if (bytFrame[intUpperbound - 1] != Convert.ToByte(intCRC & 0xFF))
                return false;
            if (bytFrame[intUpperbound] != Convert.ToByte((intCRC & 0xFF00) / 256))
                return false;
            return true;
        } // CheckCRC

        public void Close()
        {
            if (!blnClose)
            {
                if (enmHostState != HostModeState.HostModeClosed)
                {
                    enmHostState = HostModeState.HostModeClosed;
                    queDataOutbound.Clear();
                    queCommandOutbound.Clear();
                    ExitHostMode();
                }

                blnClose = true;
                try
                {
                    if (objSerial is object)
                    {
                        if (objSerial.IsOpen)
                            objSerial.Write("QUIT" + Globals.CR);
                        Thread.Sleep(200);

                        // Release the serial port...
                        objSerial.DiscardInBuffer();
                        objSerial.DiscardOutBuffer();
                        if (objSerial.IsOpen)
                        {
                            objSerial.Close();
                            Thread.Sleep(200);
                        }
                        // objSerial.Dispose()
                        objSerial = null;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("[SCSHostPort.Close] " + ex.Message);
                }
            }
        } // Close

        public HostModeState HostState
        {
            // Returns the current state of the program and controller...
            get
            {
                return enmHostState;
            }
        } // HostState

        public bool OutboundPending
        {
            get
            {
                return queDataOutbound.Count > 0;
            }
        } // OutboundPending

        public void DataToSend(byte[] bytDataToSend, int intChannel)
        {
            // Accepts data to be sent to the indicated channel. If the data is longer than
            // 128 bytes then is is broken into 128 byte packets before posting to the outbound
            // data queue. The channel number (intChannel) may be in the range 1 to 31 (&H1 to 
            // &H1F). Pactor is always channel 31 (&H1F)...

            if (bytDataToSend.Length < 1)
                return; // Don't send a null packet
            if (bytDataToSend.Length <= 128)
            {
                PostDataPacket(bytDataToSend, intChannel);
            }
            else
            {
                int intNumberOfFullPackets = bytDataToSend.Length / 128;
                int intRemainderPacket = bytDataToSend.Length % 128;
                int intIndex;
                var loopTo = intNumberOfFullPackets - 1;
                for (intIndex = 0; intIndex <= loopTo; intIndex++)
                {
                    var bytBuffer = new byte[128];
                    Array.Copy(bytDataToSend, intIndex * 128, bytBuffer, 0, 128);
                    PostDataPacket(bytBuffer, intChannel);
                }

                if (intRemainderPacket != 0)
                {
                    var bytRemainder = new byte[intRemainderPacket];
                    Array.Copy(bytDataToSend, intNumberOfFullPackets * 128, bytRemainder, 0, intRemainderPacket);
                    PostDataPacket(bytRemainder, intChannel);
                }
            }
        } // SendData 

        public void ExitHostMode()
        {
            enmHostState = HostModeState.HostModeClosed;
            queDataOutbound.Clear();
            queCommandOutbound.Clear();
            var bytExitHostMode = new byte[] { 0xAA, 0xAA, 0x1F, 0x1, 0x5, 0x4A, 0x48, 0x4F, 0x53, 0x54, 0x30, 0x0, 0x0 };
            if (objSerial is object)
            {
                queCommandOutbound.Enqueue(bytExitHostMode);
                SendNextFrame();
                Thread.Sleep(200);
            }

            Thread.Sleep(2000);
        }

        private bool IsTNCCommandResponse(char chrCommand = '\r')
        {
            // Returns true if the TNC responses with either "cmd:" or "PAC" or "*" following a
            // CR (default) or other specified command...

            blnCmdSeen = false;
            blnPacSeen = false;
            blnAsteriskSeen = false; // used to detect case where TNC is in Emulation mode TNC 2
            blnHostSync = false;
            sbdResponse.Length = 0;
            try
            {
                objSerial.Write(Convert.ToString(chrCommand));
            }
            catch
            {
            }

            var dttTimer = DateTime.Now.AddMilliseconds(2000);
            do
            {
                if (blnCmdSeen | blnPacSeen | blnAsteriskSeen)
                    return true;
                if (dttTimer < DateTime.Now)
                    return false;
                Thread.Sleep(100);
                Poll();
                if (blnClose | Globals.blnProgramClosing)
                    return false;
            }
            while (true);
        } // IsTNCCommandResponse

        private void NewState(HostModeState emnNewHostState)
        {
            // Called for any PTC controller state change...

            if (emnNewHostState != enmHostState)
            {
                enmHostState = emnNewHostState;
                var switchExpr = enmHostState;
                switch (switchExpr)
                {
                    case HostModeState.PTCStart:
                        {
                            break;
                        }
                    // RaiseEvent OnPTCControl("*** PTC Standby")
                    case HostModeState.PTCInitializing:
                        {
                            break;
                        }
                    // RaiseEvent OnPTCControl("*** PTC Initializing")
                    case HostModeState.HostMode:
                        {
                            break;
                        }
                    // RaiseEvent OnPTCControl("*** Host Mode")
                    case HostModeState.PTCFault:
                        {
                            OnPTCControl?.Invoke("*** PTC Fault");
                            break;
                        }

                    case HostModeState.SerialPortFault:
                        {
                            OnPTCControl?.Invoke("*** Serial Port Fault");
                            break;
                        }

                    case HostModeState.HostModeClosed:
                        {
                            break;
                        }
                }
            }
        } // NewState

        private void OnDataReceived(object s, SerialDataReceivedEventArgs e)
        {
            // Reads all pending bytes in the serial port and places them byte arrays on
            // the serial port input queue...

            var intBytesToRead = default(int);
            try
            {
                intBytesToRead = objSerial.BytesToRead;
            }
            catch
            {
            }

            if (intBytesToRead > 0)
            {
                int intBytesRead;
                var bytInputBuffer = new byte[intBytesToRead];
                intBytesRead = objSerial.Read(bytInputBuffer, 0, intBytesToRead);
                if (intBytesRead != intBytesToRead)
                {
                    Log.Error("[SCSHostPort.DataReceivedEvent] Bytes read does not match bytes to read");
                }

                ProcessDataReceived(bytInputBuffer);
                // Try to send another packet immediately.
                // This queues some pending packets in the TNC that are ready for transmission.
                if (CanAcceptDataPacket())
                {
                    PollOutgoing();
                }
            }
        } // OnDataReceived 

        public bool CanAcceptDataPacket()
        {
            // Check to see if the TNC can accept another packet.
            // We want a reasonable number of pending packets in the TNC, but we don't want to overrun it.
            // The "L" channel status command provides information about the number of pending packets.
            int intMaxPendingPackets;
            if (Globals.stcSelectedChannel.TNCType == "PTC DR-7800")
            {
                intMaxPendingPackets = 20;
            }
            else
            {
                intMaxPendingPackets = 5;
            }

            if (blnAcknowledged & intTNCFramesPending < intMaxPendingPackets)
            {
                return true;
            }

            return false;
        }

        public bool Open()
        {
            // Opens the serial port and calls Setup() to configure the TNC...

            try
            {
                objSerial = new SerialPort();
                objSerial.DataReceived += OnDataReceived;
                objSerial.PortName = Globals.stcSelectedChannel.TNCSerialPort;
                // objSerial.WriteTimeout = 1000
                objSerial.ReceivedBytesThreshold = 1;
                objSerial.BaudRate = Convert.ToInt32(Globals.stcSelectedChannel.TNCBaudRate);
                objSerial.DataBits = 8;
                objSerial.StopBits = StopBits.One;
                objSerial.Parity = Parity.None;
                objSerial.Handshake = Handshake.None; // Handshake.RequestToSend
                objSerial.RtsEnable = true;
                objSerial.DtrEnable = true;
                objSerial.DiscardNull = false;
                objSerial.Open();
                objSerial.DiscardInBuffer();
                objSerial.DiscardOutBuffer();
                if (!objSerial.IsOpen)
                {
                    NewState(HostModeState.SerialPortFault);
                    Globals.queChannelDisplay.Enqueue("R*** " + Globals.stcSelectedChannel.TNCSerialPort + " failed to open");
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        } // Open

        public void Poll()
        {
            if (blnClose == false)
            {
                if (enmHostState == HostModeState.HostMode)
                {
                    PollOutgoing();
                }
            }
        } // Poll

        private DateTime dttReplyTimer = DateTime.MinValue;
        private int intPendingPoll = 0;
        private void PollOutgoing()
        {
            if (dttReplyTimer == DateTime.MinValue)
            {
                dttReplyTimer = DateTime.Now;
            }

            // 
            // Send in the "L" command every 4th time to update the number of unsent packets in the TNC.
            // 
            intPendingPoll = (intPendingPoll + 1) % 4;
            if (intPendingPoll == 0)         // 
            {
                SendHostCommandPacket("L", Convert.ToByte(Globals.stcSelectedChannel.TNCPort));
                return;
            }

            if (blnRepeatRequest)
            {
                // Repeat the last frame if requested by the TNC...
                blnRepeatRequest = false;
                SendNextFrame(true);
            }
            else if (blnAcknowledged == false)
            {
                // Repeat the last frame if no acknowledgement received within one second...
                if (dttReplyTimer < DateTime.Now)
                {
                    dttReplyTimer = DateTime.Now.AddMilliseconds(1000);
                    SendNextFrame(true);
                }
            }
            else
            {
                // Send the next frame in the queue if any are pending else send a 
                // query command...
                dttReplyTimer = DateTime.Now.AddMilliseconds(1000);
                if (queDataOutbound.Count > 0 | queCommandOutbound.Count > 0)
                {
                    SendNextFrame();
                }
                else
                {
                    PostQuery();
                }
            }
        } // PollOutgoing

        private void PostDataPacket(byte[] bytData, int intChannel)
        {
            // Posts an outbound data packet to the outbound queue. The packet is
            // formatted into host mode null terminated frame...

            if (enmHostState == HostModeState.HostMode)
            {
                var bytFrame = new byte[bytData.Length + 6 + 1];
                bytFrame[0] = 0xAA;
                bytFrame[1] = 0xAA;
                bytFrame[2] = Convert.ToByte(intChannel);
                bytFrame[3] = 0;
                bytFrame[4] = Convert.ToByte(bytData.Length - 1);
                for (int intIndex = 0, loopTo = bytFrame[4]; intIndex <= loopTo; intIndex++)
                    bytFrame[intIndex + 5] = bytData[intIndex];
                queDataOutbound.Enqueue(bytFrame);
            }
            else
            {
                Log.Error("[SCSHostPort.SendDataPacket] Not in host mode! State=" + enmHostState.ToString());
            }
        } // SendDataPacket 

        private bool PostPactorInboundNullTerminatedData(ref byte[] bytInboundHostModePacket)
        {
            // Receives a null terminated byte array and posts the data (with the
            // host mode wrapper removed) to the Pactor control TCP/IP connection...

            var bytDataIn = new byte[301];
            int intIndex;
            for (intIndex = 0; intIndex <= 300; intIndex++)
            {
                if (bytInboundHostModePacket[intIndex + 2] == 0)
                    break;
                bytDataIn[intIndex] = bytInboundHostModePacket[intIndex + 2];
            }

            Array.Resize(ref bytDataIn, intIndex);
            try
            {
                OnPTCControl?.Invoke(Globals.GetString(bytDataIn) + Globals.CR);
            }
            catch
            {
            }

            blnAcknowledged = true;
            return default;
        } // PostPactorInboundNullTerminatedData

        private DateTime dttTimer = DateTime.MinValue;

        public void PostQuery()
        {
            if (dttTimer == DateTime.MinValue)
            {
                dttTimer = DateTime.Now.AddMilliseconds(1000);
            }

            if (dttTimer < DateTime.Now)
            {
                dttTimer = DateTime.Now.AddMilliseconds((double)1000);
                var bytG3Query = new byte[] { 0xAA, 0xAA, 0xFE, 0x1, 0x1, 0x47, 0x33, 0x0, 0x0 };
                queCommandOutbound.Enqueue(bytG3Query);
            }
            else
            {
                var bytGQuery = new byte[] { 0xAA, 0xAA, 0xFF, 0x1, 0x0, 0x47, 0x0, 0x0 };
                queCommandOutbound.Enqueue(bytGQuery);
            }
        } // PostQuery

        private bool PostRadioResponseInboundData(ref byte[] bytInboundHostModePacket)
        {
            // Receives a byte count inbound host mode packet and posts the data (with the
            // host mode wrapper removed) to the Pactor control TCP/IP connection...

            var bytDataIn = new byte[301];
            int intIndex;
            for (intIndex = 0; intIndex <= 300; intIndex++)
            {
                if (bytInboundHostModePacket[intIndex + 2] == 0)
                    break;
                bytDataIn[intIndex] = bytInboundHostModePacket[intIndex + 2];
            }

            Array.Resize(ref bytDataIn, intIndex);
            OnPTCRadio?.Invoke(bytDataIn);
            blnAcknowledged = true;
            return default;
        } // PostRadioResponseInboundData

        // Assembles the received raw data into complete
        // CRC extended host mode frames...
        private byte[] bytFrame = new byte[1000];
        private int intPosition = 0;
        private int intUpperBound = 0;
        private bool blnNullTerminate = false;
        private bool blnRemoveStuffing = false;

        private void ProcessDataReceived(byte[] bytReceivedBytes)
        {
            if (enmHostState != HostModeState.HostMode)
            {
                ProcessNonHostInput(ref bytReceivedBytes);
            }
            else
            {
                foreach (byte bytSingle in bytReceivedBytes)
                {
                    // Remove stuffing bytes...
                    if (intPosition > 2)
                    {
                        if (blnRemoveStuffing & (int)bytSingle == 0)
                        {
                            blnRemoveStuffing = false;
                            continue;
                        }

                        blnRemoveStuffing = false;
                        if ((int)bytSingle == 0xAA)
                        {
                            blnRemoveStuffing = true;
                        }
                    }

                    // Build new frame...
                    var switchExpr = intPosition;
                    switch (switchExpr)
                    {
                        case 0:
                            {
                                // First framing byte...
                                if ((int)bytSingle == 0xAA)
                                {
                                    bytFrame[intPosition] = bytSingle;
                                    intPosition += 1;
                                    intUpperBound = 0;
                                    blnNullTerminate = false;
                                    // blnAcknowledged = False
                                }

                                blnRemoveStuffing = false;
                                break;
                            }

                        case 1:
                            {
                                // Second framing byte...
                                if ((int)bytSingle == 0xAA)
                                {
                                    bytFrame[intPosition] = bytSingle;
                                    intPosition += 1;
                                }
                                else
                                {
                                    intPosition = 0;
                                }

                                blnRemoveStuffing = false;
                                break;
                            }

                        case 2:
                            {
                                // Channel indicator byte...
                                bytFrame[intPosition] = bytSingle;
                                intPosition += 1;
                                break;
                            }

                        case 3:
                            {
                                // Payload type indicator byte...
                                if ((int)bytSingle == 0)
                                {
                                    intUpperBound = 5;
                                    blnNullTerminate = true;
                                }
                                else if ((int)bytSingle <= 5)
                                {
                                    blnNullTerminate = true;
                                }

                                if ((int)bytFrame[2] == 0xFF)
                                {
                                    if ((int)bytSingle == 0x0)
                                        intUpperBound = intPosition + 2;
                                }

                                if ((int)bytFrame[2] == 0xAA & (int)bytSingle == 0x55)
                                {
                                    blnRepeatRequest = true;
                                    intPosition = 0;
                                    intUpperBound = 0;
                                }
                                else
                                {
                                    bytFrame[intPosition] = bytSingle;
                                    intPosition += 1;
                                }

                                break;
                            }

                        case 4:
                            {
                                // Payload byte count or start of payload if zero terminated...
                                bytFrame[intPosition] = bytSingle;
                                if (blnNullTerminate)
                                {
                                    if ((int)bytSingle == 0x0)
                                        intUpperBound = intPosition + 2;
                                }
                                else
                                {
                                    intUpperBound = (int)bytSingle + 7;
                                }

                                intPosition += 1;
                                break;
                            }

                        case 5:
                            {
                                // Payload or first CRC...
                                if (blnNullTerminate)
                                {
                                    if ((int)bytSingle == 0x0)
                                        intUpperBound = intPosition + 2;
                                }

                                bytFrame[intPosition] = bytSingle;
                                if (intPosition >= intUpperBound & intUpperBound != 0)
                                {
                                    if (this.CheckCRC(ref bytFrame, intPosition))
                                    {
                                        this.ProcessReceivedFrame(ref bytFrame, intUpperBound);
                                        blnAcknowledged = true;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objSerial.Write(bytRepeatRequest, 0, 4);
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    intPosition = 0;
                                    intUpperBound = 0;
                                }
                                else
                                {
                                    intPosition += 1;
                                }

                                break;
                            }

                        case 6:
                            {
                                // Payload or second CRC...
                                if (blnNullTerminate & intUpperBound == 0)
                                {
                                    if ((int)bytSingle == 0x0)
                                        intUpperBound = intPosition + 2;
                                }

                                bytFrame[intPosition] = bytSingle;
                                if (intPosition >= intUpperBound & intUpperBound != 0)
                                {
                                    if (this.CheckCRC(ref bytFrame, intPosition))
                                    {
                                        this.ProcessReceivedFrame(ref bytFrame, intUpperBound);
                                        blnAcknowledged = true;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objSerial.Write(bytRepeatRequest, 0, 4);
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    intPosition = 0;
                                    intUpperBound = 0;
                                }
                                else
                                {
                                    intPosition += 1;
                                }

                                break;
                            }

                        default:
                            {
                                // Payload and CRC bytes to intUpperBound...
                                if (blnNullTerminate & intUpperBound == 0)
                                {
                                    if ((int)bytSingle == 0x0)
                                        intUpperBound = intPosition + 2;
                                }

                                bytFrame[intPosition] = bytSingle;
                                if (intPosition >= intUpperBound & intUpperBound != 0)
                                {
                                    if (this.CheckCRC(ref bytFrame, intUpperBound))
                                    {
                                        this.ProcessReceivedFrame(ref bytFrame, intUpperBound);
                                        blnAcknowledged = true;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            objSerial.Write(bytRepeatRequest, 0, 4);
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    intPosition = 0;
                                    intUpperBound = 0;
                                }
                                else
                                {
                                    intPosition += 1;
                                }

                                break;
                            }
                    }
                }
            }
        } // ProcessDataRececeived

        private void ProcessNonHostInput(ref byte[] bytData)
        {
            // Processes data from the TNC when not in host mode...
            var switchExpr = enmHostState;
            switch (switchExpr)
            {
                case HostModeState.PTCInitializing:
                    {
                        StartupResponses(bytData);
                        break;
                    }
            }
        } // ProcessNonHostInput

        private void ProcessReceivedFrame(ref byte[] bytFrame, int intUpperBound)
        {
            // Processes a host frame received from the TNC... 

            OnActivity?.Invoke();

            // Remove the frame leaving the channel number and payload...
            var bytPayload = new byte[intUpperBound - 4 + 1];
            Array.Copy(bytFrame, 2, bytPayload, 0, intUpperBound - 3);
            var switchExpr = bytPayload[0]; // Select payload type
            switch (switchExpr)
            {
                case 0xFF:
                    {
                        if (bytPayload[2] != 0)
                        {
                            int intChannel = bytPayload[2] - 1;
                            SendHostCommandPacket("G3", Convert.ToByte(intChannel));
                        }

                        break;
                    }

                case 0xFE: // Status report...
                    {
                        OnPTCStatusReport?.Invoke(bytPayload);
                        break;
                    }

                case 0xFD: // From the radio...
                    {
                        OnPTCRadio?.Invoke(bytPayload); // Command responses and channel data...
                        break;
                    }

                default:
                    {
                        var switchExpr1 = bytPayload[1];
                        switch (switchExpr1)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 6:
                                {
                                    OnPTCControl?.Invoke(Globals.GetString(bytPayload, 2) + Globals.CR);
                                    break;
                                }

                            case 7:
                                {
                                    OnPTCData?.Invoke(bytPayload);
                                    break;
                                }
                        }

                        break;
                    }
            }
        } // ProcessReceivedFrame

        public void SendHostCommandPacket(byte[] bytCommand, byte intChannel)
        {
            // Puts a host mode packet in the outbound queue for transmission to the controller...

            if (enmHostState == HostModeState.HostMode)
            {
                var bytFrame = new byte[bytCommand.Length + 6 + 1];
                bytFrame[0] = 0xAA;
                bytFrame[1] = 0xAA;
                bytFrame[2] = intChannel;
                bytFrame[3] = 1;
                bytFrame[4] = Convert.ToByte(bytCommand.Length - 1);
                for (int intIndex = 5, loopTo = bytFrame[4] + 5; intIndex <= loopTo; intIndex++)
                    bytFrame[intIndex] = bytCommand[intIndex - 5];
                queCommandOutbound.Enqueue(bytFrame);
            }
            else
            {
                Log.Error("[SCSHostPort.SendHostPacket] Not in Host mode! State=" + enmHostState.ToString());
            }
        } // SendHostPacket (Byte())

        public void SendHostCommandPacket(string strCommand, byte intChannel)
        {
            // Puts a host mode packet in the outbound queue for transmission to the controller...

            if (enmHostState == HostModeState.HostMode)
            {
                var bytFrame = new byte[strCommand.Length + 6 + 1];
                bytFrame[0] = 0xAA;
                bytFrame[1] = 0xAA;
                bytFrame[2] = intChannel;
                bytFrame[3] = 1;
                bytFrame[4] = Convert.ToByte(strCommand.Length - 1);
                for (int intIndex = 5, loopTo = bytFrame[4] + 5; intIndex <= loopTo; intIndex++)
                    bytFrame[intIndex] = Convert.ToByte(Globals.Asc(strCommand[intIndex - 5]));
                queCommandOutbound.Enqueue(bytFrame);
            }
            else
            {
                Log.Error("[SCSHostPort.SendHostPacket] Not in host mode - State=" + enmHostState.ToString() + " Command: " + strCommand);
            }
        } // SendHostPacket (String)

        private bool blnAlternate = true;
        private byte[] bytPendingFrame = null;
        private int intRepeatCount = 0;

        private void SendNextFrame(bool blnRepeatLast = false)
        {
            if (blnRepeatLast == false)
            {
                intRepeatCount = 0;
                if (queCommandOutbound.Count > 0)
                {
                    // Send a command packet
                    try
                    {
                        bytPendingFrame = (byte[])queCommandOutbound.Dequeue();
                    }
                    catch
                    {
                        return;
                    }
                }
                // Send a data packet
                else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    // Packet mode
                    if (intTNCFramesPending >= 5)
                    {
                        // 
                        // Decrease from 7 to 5 to reduce the nuber of uacked buffers in the TNC.
                        // If we have more than 5 outstanding buffers already sent, stop sending until the TNC
                        // catches up.
                        // 
                        return;
                    }
                    else
                    {
                        try
                        {
                            bytPendingFrame = (byte[])queDataOutbound.Dequeue();
                        }
                        catch
                        {
                            return;
                        }

                        Globals.UpdateProgressBar(bytPendingFrame.Length - 6);
                    }
                }
                else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    // Pactor mode
                    if (intTNCBytesPosted - intTNCBytesSent > 1200)
                        SendHostCommandPacket("%T", 31);
                    PostQuery();
                    int intMaxPendingPackets;
                    if (Globals.stcSelectedChannel.TNCType == "PTC DR-7800")
                    {
                        intMaxPendingPackets = 25;
                    }
                    else
                    {
                        intMaxPendingPackets = 7;
                    }

                    if (intTNCFramesPending < intMaxPendingPackets)
                    {
                        // PHS If (intTNCBytesPosted - intTNCBytesSent) < 2400 Then
                        try
                        {
                            bytPendingFrame = (byte[])queDataOutbound.Dequeue();
                        }
                        catch
                        {
                            return;
                        }

                        intTNCBytesPosted += bytPendingFrame.Length - 7;
                    }
                    else
                    {
                        return;
                    }
                }

                blnAcknowledged = false;

                // Add the alternate frame bit...
                if ((int)bytPendingFrame[3] >= 0x80)
                    blnAlternate = true;
                if (blnAlternate == true)
                    bytPendingFrame[3] = (byte)(bytPendingFrame[3] | Convert.ToByte(0x80));
                blnAlternate = !blnAlternate;

                // Fill in the CRC bytes...
                this.AddCRC(ref bytPendingFrame);

                // Add any required stuffing bytes...
                this.AddStuffingBytes(ref bytPendingFrame);
                try
                {
                    objSerial.Write(bytPendingFrame, 0, bytPendingFrame.Length);
                }
                catch
                {
                }

                intTNCFramesPending += 1;    // PHS count another pending outgoing frame (will be updated by L command response)
            }
            else
            {
                // Repeat the last command
                blnAcknowledged = false;
                intRepeatCount += 1;
                if (intRepeatCount < 10)
                {
                    try
                    {
                        objSerial.Write(bytPendingFrame, 0, bytPendingFrame.Length);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("R*** No PTC Host mode frame acknowledge after 10 repeats...");
                    NewState(HostModeState.PTCFault);
                }
            }
        } // SendNextFrame

        private void StartCRCExtendedHostMode()
        {
            // Enter CRC extended host mode...
            try
            {
                objSerial.Write("JHOST4" + Globals.CR);
            }
            catch
            {
            }

            Thread.Sleep(100);
            NewState(HostModeState.HostMode);

            // Jump start the polling process...
            var bytInitialPoll = new byte[] { 0xAA, 0xAA, 0xFF, 0xC1, 0x0, 0x47, 0xF1, 0x5F };
            queCommandOutbound.Enqueue(bytInitialPoll);
            SendNextFrame();
        } // StartCRCExtendedHostMode

        public bool Startup()
        {
            // Called to handle the initialization and startup proceduce for the PTC controller.

            NewState(HostModeState.PTCInitializing);
            DateTime dttTimer;

            // Test if the channel is set up for a fast start...
            bool blnFastStart = false;
            if (Globals.stcSelectedChannel.TNCConfigureOnFirstUseOnly == true | Globals.blnPactorDialogResuming == true)
            {
                if (Globals.cllFastStart.Contains(Globals.stcSelectedChannel.ChannelName))
                {
                    if (Globals.stcSelectedChannel.TNCType != "PTC DR-7800") // PHS: DR-7800 requires full initialization
                    {
                        blnFastStart = true;
                    }
                }
            }

            try
            {
                // Test for a normal terminal mode command response...
                if (IsTNCCommandResponse() == false && IsTNCCommandResponse('\u001b') == false)
                {
                    // Try and exit host mode strings first in case PTC left in host mode....
                    if (blnClose | Globals.blnProgramClosing)
                        return false;
                    try
                    {
                        objSerial.Write(bytExitCRCExtendedHostMode, 0, bytExitCRCExtendedHostMode.Length);
                    }
                    catch
                    {
                    }

                    Thread.Sleep(1000);
                    if (blnClose | Globals.blnProgramClosing)
                        return false;
                    if (IsTNCCommandResponse() == false)
                    {
                        var bytExitHostMode = new byte[] { 0xAA, 0xAA, 0x1F, 0x1, 0x5, 0x4A, 0x48, 0x4F, 0x53, 0x54, 0x30, 0x0, 0x0 };
                        try
                        {
                            objSerial.Write(bytExitHostMode, 0, bytExitHostMode.Length);
                        }
                        catch
                        {
                        }

                        Thread.Sleep(1000);
                        if (blnClose | Globals.blnProgramClosing)
                            return false;
                        if (IsTNCCommandResponse() == false)
                        {
                            // try clearing KISS mode 
                            var bytClearKiss = new byte[] { 0xC0, 0xFF, 0xC0 };
                            try
                            {
                                objSerial.Write(bytClearKiss, 0, bytClearKiss.Length);
                            }
                            catch
                            {
                            }

                            Thread.Sleep(1000);
                            if (blnClose | Globals.blnProgramClosing)
                                return false;
                            if (IsTNCCommandResponse() == false)
                            {
                                Globals.queChannelDisplay.Enqueue("R*** Could not recover the " + Globals.stcSelectedChannel.TNCType + " ... Try power recycle.");
                                Log.Error("[SCSHstPort.Startup] Could not recover the " + Globals.stcSelectedChannel.TNCType);
                                return false;
                            }
                        }
                    }
                }

                if (blnClose | Globals.blnProgramClosing)
                    return false;
                // May have been left in packet mode...
                if (blnPacSeen)
                {
                    blnCmdSeen = false;
                    blnAsteriskSeen = false;
                    // Resets packet mode if necessary necessary...
                    dttTimer = DateTime.Now;
                    try
                    {
                        sbdResponse.Length = 0;
                        objSerial.Write("QUIT" + Globals.CR);
                    }
                    catch
                    {
                    }

                    while (DateTime.Now.Subtract(dttTimer).TotalMilliseconds < 1000)
                    {
                        Thread.Sleep(100);
                        Poll();
                        if (blnClose | Globals.blnProgramClosing)
                            return false;
                        if (blnCmdSeen | blnAsteriskSeen)
                            break;
                    }
                }

                if (blnAsteriskSeen) // TNC was left in emulation mode TNC 2 by some other program
                {
                    blnCmdSeen = false;
                    dttTimer = DateTime.Now;
                    sbdResponse.Length = 0;
                    objSerial.Write("TNC 0" + Globals.CR); // Put in normal default emulation mode TNC 0
                    while (DateTime.Now.Subtract(dttTimer).TotalMilliseconds < 1000)
                    {
                        Thread.Sleep(100);
                        Poll();
                        if (blnClose | Globals.blnProgramClosing)
                            return false;
                        if (blnCmdSeen)
                            break;
                    }

                    if (!blnCmdSeen)
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Could not recover the TNC from Emulation mode 2");
                        Log.Error("[SCSHstPort.Startup] Could not recover the " + Globals.stcSelectedChannel.TNCType + " from Emulation mode 2");
                        return false;
                    }
                }
                // This should clear any connections . . . does not reset parameters...
                blnCmdSeen = false;
                try
                {
                    sbdResponse.Length = 0;
                    if (Globals.stcSelectedChannel.TNCType == "PTC DR-7800")
                    {
                        objSerial.Write("RESTART" + Globals.CR);   // PHS DR-7800 works better with Restart than Reset.
                    }
                    else
                    {
                        objSerial.Write("RESET" + Globals.CR);
                    } // Reset is OK with other types of modems.
                }
                catch
                {
                }

                dttTimer = DateTime.Now;
                while (DateTime.Now.Subtract(dttTimer).TotalMilliseconds < 3000) // Wait 3 seconds for RESET
                {
                    Thread.Sleep(100);
                    Poll();
                    if (blnClose | Globals.blnProgramClosing)
                        return false;
                    if (blnCmdSeen)
                        break;
                }

                // Attempt to recover the TNC if no "cmd:" has been seen...
                if (!blnCmdSeen)
                {
                    Log.Error("[SCSHostPort.Startup] No valid response from " + Globals.stcSelectedChannel.TNCType + " after RESET command");
                    for (int intIndex = 1; intIndex <= 256; intIndex++)
                    {
                        try
                        {
                            sbdResponse.Length = 0;
                            objSerial.Write(bytSOH, 0, bytSOH.Length);
                        }
                        catch
                        {
                        }

                        dttTimer = DateTime.Now.AddMilliseconds(2000);
                        do
                        {
                            if (dttTimer < DateTime.Now)
                                break;
                            Thread.Sleep(100);
                            Poll();
                            if (blnClose | Globals.blnProgramClosing)
                                return false;
                            if (blnHostSync)
                                break;
                        }
                        while (true);
                    }

                    Log.Error("[SCSHostPort.Startup] " + Globals.stcSelectedChannel.TNCType + " Host Sync: " + blnHostSync.ToString());
                    try
                    {
                        sbdResponse.Length = 0;
                        objSerial.Write(bytExitCRCExtendedHostMode, 0, bytExitCRCExtendedHostMode.Length);
                    }
                    catch
                    {
                    }

                    Thread.Sleep(500);
                    blnCmdSeen = false;
                    IsTNCCommandResponse();
                    if (blnClose | Globals.blnProgramClosing)
                        return default;
                }

                // Read and send the .aps initialization file if blnFastStart is not set...
                if (blnFastStart == false)
                {
                    if (blnCmdSeen)
                    {

                        // Read configuration commands from the configuration file...
                        var strLines = File.ReadAllLines(Globals.stcSelectedChannel.TNCConfigurationFile);
                        foreach (string strLine in strLines)
                        {
                            string tmpStrLine = strLine.Trim();
                            if (!tmpStrLine.StartsWith(";")) // leading ";" comments out line
                            {
                                if (tmpStrLine.IndexOf(";") != -1)
                                {
                                    tmpStrLine = tmpStrLine.Substring(0, tmpStrLine.IndexOf(";")).Trim(); // strip off comments
                                }

                                if (tmpStrLine.Length > 0)
                                {
                                    dttTimer = DateTime.Now;
                                    blnCmdSeen = false;
                                    try
                                    {
                                        sbdResponse.Length = 0;
                                        objSerial.Write(tmpStrLine + Globals.CR);
                                    }
                                    catch
                                    {
                                    }

                                    Thread.Sleep(10);
                                    Poll();
                                }
                            }

                            if (blnClose | Globals.blnProgramClosing)
                                return false;
                        }
                    }
                }
                // Force the MYCALL while in cmd mode
                string strPactorCallsign = Globals.SiteCallsign;
                if (Globals.blnForceHFRouting)
                {
                    if (Globals.SiteCallsign.Length == 7 & Globals.SiteCallsign.Contains("-") == false)
                    {
                        // Add 'T' as 8th character without a dash
                        strPactorCallsign += "T";
                    }
                    else
                    {
                        // Add "-T"
                        strPactorCallsign += "-T";
                    }
                }

                objSerial.Write("MYCALL " + strPactorCallsign + Globals.CR);
                Thread.Sleep(50);
                objSerial.Write("PAC MYCALL " + strPactorCallsign + Globals.CR);
                Thread.Sleep(50);

                // Set the TNC to host mode...
                StartCRCExtendedHostMode();
                if (Globals.cllFastStart.Contains(Globals.stcSelectedChannel.ChannelName) == false)
                    Globals.cllFastStart.Add(Globals.stcSelectedChannel.ChannelName);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("[SCSHostPort.Startup] " + ex.Message);
                NewState(HostModeState.PTCFault);
                Globals.queChannelDisplay.Enqueue("R*** Error during " + Globals.stcSelectedChannel.TNCType + " startup... See error log");
                return false;
            }
        } // Startup

        private void StartupResponses(byte[] bytFromPTC)
        {
            // Looks for confirmations to startup commands...

            foreach (byte bytSingle in bytFromPTC)
            {
                if (bytSingle != 0)
                {
                    // sbdCmdResponse.Append(Chr(bytSingle))
                    sbdResponse.Append((char)bytSingle);
                }

                if (sbdResponse.ToString().EndsWith("cmd: "))
                {
                    blnCmdSeen = true;
                    sbdResponse.Length = 0;
                }
                else if (sbdResponse.ToString() == "* ") // this handles case if TNC is in emulation mode TNC 2
                {
                    blnAsteriskSeen = true;
                    sbdResponse.Length = 0;
                }
                else if (sbdResponse.ToString().EndsWith("pac: "))
                {
                    blnPacSeen = true;
                    sbdResponse.Length = 0;
                }
                else if (sbdResponse.ToString().IndexOf("INVALID") >= 0)
                {
                    blnHostSync = true;
                    sbdResponse.Length = 0;
                }
            }
        } // StartupResponses
    }
}