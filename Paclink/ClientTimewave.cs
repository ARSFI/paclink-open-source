using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;

namespace Paclink
{

    // This class handles Timewave (PK) TNCs for both Packet and Pactor connections...
    public class ModemTimewave : IModem
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        // Structures and enumerations
        private LinkStates enmState = LinkStates.Undefined;
        private ConnectionOrigin enmConnectionStatus;

        // Integers
        private int intTotalBytesSent;      // Holds count of total bytes sent - used for progress bar

        // Strings
        private string strTargetCallsign;
        private string[] aryConnectScriptLines;
        private string strScriptResponse;

        // Boolean
        private bool blnSendingID;           // Flag used to insure ID only sent once per connection
        private bool blnInScript;
        private bool blnClosed;
        private bool blnAbort;
        private bool blnDisconnectRequested;
        private bool blnAutomaticConnect;

        // Queues 


        // Date/Time
        private DateTime dttDisconnect;

        // Objects and Classes
        private WA8DEDHostPort objHostPort;
        private ProtocolInitial objProtocol;

        public LinkStates State
        {
            get
            {
                return enmState;
            }
        } // State

        public bool NormalDisconnect
        {
            get
            {
                // Not used in this context...
                return true;
            }

            set
            {
                // Not used in this context...
            }
        } // NormalDisconnect

        public ModemTimewave()
        {
            // The class is instantiated to begin a channel connection...
            Globals.queStatusDisplay.Enqueue("Starting");
            objHostPort = new WA8DEDHostPort();
            Globals.blnChannelActive = true;
        } // New

        public bool Close()
        {
            // Subroutine to close the channel and put TNC into known state...
            // Always call this method before the instance goes out of scope to
            // insure the channel is never left in Pactor mode...

            if (!blnClosed)
            {
                blnClosed = true;
                Globals.queChannelDisplay.Enqueue("G*** Closing " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());

                // If not already closed force a quick TNC channel disconnect...
                if (enmState != LinkStates.Disconnected)
                {
                    if (objHostPort is object && objHostPort.IsOpen)
                    {
                        if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
                        {
                            objHostPort.HostCommand("DI");
                            objHostPort.WaitOnHostCommandResponse("DI");
                            objHostPort.HostCommand("DI");
                            objHostPort.WaitOnHostCommandResponse("DI");
                        }
                        else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
                        {
                            objHostPort.HostCommand("PA");
                            objHostPort.WaitOnHostCommandResponse("PA");
                        }

                        if (enmState == LinkStates.Connected)
                            enmState = LinkStates.Disconnected;
                    }
                }

                if (Globals.objRadioControl is object) // Shut down the radio control and free the serial port
                {
                    Globals.objRadioControl.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    Globals.objRadioControl = null;
                }

                if (objHostPort is object)
                {
                    if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC & objHostPort.IsOpen)
                    {
                        // Returns the TNC to packet mode...
                        objHostPort.HostCommand("PA");
                    }

                    objHostPort.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                }

                if (objProtocol is object)
                    objProtocol.CloseProtocol();
                objProtocol = null;
                Globals.queStatusDisplay.Enqueue("Idle");
                Globals.queRateDisplay.Enqueue("------");
                Globals.ResetProgressBar();
                Globals.blnChannelActive = false;
                Globals.ObjSelectedModem = null;
                return true;
            }

            return default;
        } // Close 

        public void Abort()
        {
            if (objHostPort is object && objHostPort.IsOpen)
            {
                if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
                {
                    objHostPort.HostCommand("DI");
                    objHostPort.WaitOnHostCommandResponse("DI");
                }
                else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
                {
                    objHostPort.HostCommand("PA");
                    objHostPort.WaitOnHostCommandResponse("PA");
                }

                if (enmState == LinkStates.Connected)
                    enmState = LinkStates.Disconnected;
            }
            // Else
            // enmState = LinkStates.LinkFailed
            // End If
            else
            {
                enmState = LinkStates.Disconnected;
            }

            if (objHostPort is object)
                objHostPort.Abort();
            blnAbort = true;
        } // Abort 

        public bool Connect(bool blnAutomatic)
        {
            // Handles the initial TNC initialization and connection to a remote site...

            blnAutomaticConnect = blnAutomatic;
            Globals.queChannelDisplay.Enqueue("G*** Initializing TNC " + Globals.stcSelectedChannel.TNCType + " on " + Globals.stcSelectedChannel.TNCSerialPort + " with " + Globals.stcSelectedChannel.TNCConfigurationFile);
            objHostPort.Open();
            if (objHostPort.IsOpen == false)
            {
                return false;
            }

            enmState = LinkStates.Initialized;
            // This now handles control for both Packet and Pactor
            if (Globals.stcSelectedChannel.RDOControl == "Serial")
            {
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
                    else if (Globals.stcSelectedChannel.RDOModel.StartsWith("Micom"))
                    {
                        Globals.objRadioControl = new RadioMicom();
                    }
                    else
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Failure setting Radio control for radio " + Globals.stcSelectedChannel.RDOModel);
                        return false;
                    }

                    if (!Globals.objRadioControl.InitializeSerialPort(ref Globals.stcSelectedChannel))
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Failure initializing Radio Control");
                        Log.Error("[KantronicsClient.Connect] Failure initializing Radio Control");
                        return false;
                    }
                }
            }

            if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
            {
                if (Globals.objRadioControl != null)
                    Globals.objRadioControl.SetParameters(ref Globals.stcSelectedChannel);
                // Set TNC to packet mode...
                objHostPort.HostCommand("PA");
                if (string.IsNullOrEmpty(objHostPort.WaitOnHostCommandResponse("PA")))
                {
                    Globals.queChannelDisplay.Enqueue("R*** Configuration of the TNC failed A");
                    enmState = LinkStates.Disconnected;
                    return false;
                }

                if (string.IsNullOrEmpty(Globals.stcSelectedChannel.TNCScript))
                {
                    objHostPort.HostCommand("CO" + Globals.stcSelectedChannel.RemoteCallsign);
                    Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign);
                }
                else
                {
                    aryConnectScriptLines = Globals.stcSelectedChannel.TNCScript.Replace(Globals.LF, "").ToUpper().Split(Convert.ToChar(Globals.CR));
                    blnInScript = true;
                    if (!ConnectionScript())
                    {
                        blnInScript = false;
                        return false;
                    }

                    blnInScript = false;
                }
            }
            else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
            {
                // Set TNC to Pactor mode...
                objHostPort.HostCommand("ML" + Globals.SiteCallsign); // Set MyTCall 
                objHostPort.HostCommand("PT"); // Set packet mode
                if (string.IsNullOrEmpty(objHostPort.WaitOnHostCommandResponse("PT")))
                {
                    Globals.queChannelDisplay.Enqueue("R*** Configuration of the TNC failed B");
                    enmState = LinkStates.Disconnected;
                    return false;
                }
                // This handles manual Pactor connections or unspecified automatic channels...
                float tmpVal = 0.0F;
                if (!float.TryParse(Globals.ExtractFreq(ref Globals.stcSelectedChannel.RDOCenterFrequency), out tmpVal) || string.IsNullOrEmpty(Globals.stcSelectedChannel.RemoteCallsign.Trim()) || !blnAutomatic)
                {
                    if (Globals.dlgPactorConnect is object)
                    {
                        Globals.dlgPactorConnect.Close();
                        Globals.dlgPactorConnect = null;
                    }

                    if (!string.IsNullOrEmpty(Globals.stcEditedSelectedChannel.RemoteCallsign))
                    {
                        Globals.dlgPactorConnect = new DialogPactorConnect(this, ref Globals.stcEditedSelectedChannel);
                    }
                    else
                    {
                        Globals.dlgPactorConnect = new DialogPactorConnect(this, ref Globals.stcSelectedChannel);
                    }

                    Globals.dlgPactorConnect.ShowDialog();
                    if (Globals.dlgPactorConnect.DialogResult == DialogResult.Cancel)
                    {
                        if (objProtocol != null)
                        {
                            objProtocol.LinkStateChange(ConnectionOrigin.Disconnected);
                            objProtocol = null;
                        }

                        if (enmState == LinkStates.Connected)
                            enmState = LinkStates.Disconnected;
                        Globals.dlgPactorConnect.Close();
                        Globals.dlgPactorConnect = null;
                        return false;
                    }

                    // This updates the channel parameters...
                    Globals.dlgPactorConnect.UpdateChannelProperties(ref Globals.stcSelectedChannel);
                    Globals.dlgPactorConnect.Close();
                    Globals.dlgPactorConnect = null;
                    if (!Globals.blnPactorDialogResuming)
                        Globals.stcEditedSelectedChannel = default;
                }
                else if (Globals.objRadioControl != null)
                    Globals.objRadioControl.SetParameters(ref Globals.stcSelectedChannel);

                // Start a Pactor call...
                objHostPort.HostCommand("Mf" + Globals.SiteCallsign); // Set MyPTCall
                objHostPort.HostCommand("PG" + Globals.stcSelectedChannel.RemoteCallsign);
                Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign);
            }
            else
            {
                return false;
            }

            return true;
        } // Connect 

        public void SendRadioCommand(byte[] bytCommand)
        {
            // Function to send radio command via PTC II (not used in this class)
        } // SendRadioCommand 

        public void SendRadioCommand(string strCommand)
        {
            // Function to send radio command via PTC II (not used in this class)
        } // SendRadioCommand 

        public void DataToSend(string strData)
        {
            // Sends string data to the TNC...

            var objEncoder = new ASCIIEncoding();
            DataToSend(Globals.GetBytes(strData));
        } // DataToSend (string) 

        public void DataToSend(byte[] bytData)
        {
            // Sends byte stream data to the TNC. May be called with 0 
            // length data to cause turnover in Pactor...

            if (bytData.Length > 0)
            {
                objHostPort.DataToSend(bytData);
            }
        } // DataToSend (bytes) 

        public void Disconnect()
        {
            // Handle channel disconnect...
            if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
            {
                objHostPort.HostCommand("DI");
                objHostPort.WaitOnHostCommandResponse("DI");
            }
            else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
            {
                // objHostPort.HostCommand("RC")
                // objHostPort.WaitOnHostCommandResponse("RC")
            }

            blnDisconnectRequested = true;
            dttDisconnect = DateTime.Now.AddSeconds(2);
        } // Disconnect 

        public void Poll()
        {
            if (blnDisconnectRequested)
            {
                if (dttDisconnect < DateTime.Now)
                    enmState = LinkStates.Disconnected;
            }

            if (objHostPort is object)
            {
                if (objHostPort.IsOpen)
                    objHostPort.Poll();
                do
                {
                    // Get unsolicited status reports...
                    string strStatusReport = objHostPort.StatusReports.ToUpper();
                    if (string.IsNullOrEmpty(strStatusReport))
                        break;
                    if (blnInScript)
                        strScriptResponse += strStatusReport;
                    Globals.queChannelDisplay.Enqueue("P*** " + strStatusReport.Trim());
                    if (strStatusReport.IndexOf("DISCONNECTED") != -1)
                    {
                        SendIdentification();
                        enmState = LinkStates.Disconnected;
                    }
                    else if (strStatusReport.IndexOf("CONNECTED") != -1)
                    {
                        enmState = LinkStates.Connected;
                        objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                    }
                    else if (strStatusReport.IndexOf("TIMEOUT") != -1)
                    {
                        SendIdentification();
                        enmState = LinkStates.Disconnected;
                    }
                    else if (strStatusReport.IndexOf("RETRY") != -1)
                    {
                        enmState = LinkStates.Disconnected;
                    }
                }
                while (true);
                do
                {
                    // Get received data...
                    if (objHostPort.DataBlocksReceived.Count > 0)
                    {
                        byte[] bytData;
                        try
                        {
                            bytData = (byte[])objHostPort.DataBlocksReceived.Dequeue();
                        }
                        catch
                        {
                            break;
                        }

                        if (blnInScript)
                        {
                            // Display received data immediately and post data to the script response string...
                            string strResponse = Globals.GetString(bytData);
                            Globals.queChannelDisplay.Enqueue("X" + strResponse);
                            strScriptResponse += strResponse;
                        }
                        else if (objProtocol is object)
                        {
                            // Send data to the protocol handler...
                            objProtocol.ChannelInput(ref bytData);
                        }
                        else
                        {
                            // Dump any received data if the obuProtocol is not yet instantiated...
                            while (objHostPort.DataBlocksReceived.Count > 0)
                            {
                                try
                                {
                                    objHostPort.DataBlocksReceived.Dequeue();
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                while (true);
            }
        } // Poll 

        private bool ConnectionScript()
        {
            // Processes the connection script...
            try
            {
                blnInScript = true;
                if (string.IsNullOrEmpty(aryConnectScriptLines[0]))
                {
                    objHostPort.HostCommand("CO" + Globals.stcSelectedChannel.RemoteCallsign);
                    Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign);
                    return true;
                }

                string strFirstScriptLine = FormatFirstScriptLine(aryConnectScriptLines[0]);
                objHostPort.HostCommand(strFirstScriptLine);
                Globals.queChannelDisplay.Enqueue("G #Begin Connection Script");
                Globals.queChannelDisplay.Enqueue("G     #Script(0):" + aryConnectScriptLines[0]);
                if (aryConnectScriptLines.Length > 1)
                {
                    if (string.IsNullOrEmpty(aryConnectScriptLines[1]))
                        return true;
                    if (WaitOnScriptResponse(aryConnectScriptLines[1]) == false)
                    {
                        return false;
                    }
                    else
                    {
                        Globals.queChannelDisplay.Enqueue("G     #Script(1):" + aryConnectScriptLines[1]);
                    }
                }
                else
                {
                    return true;
                }

                for (int intIndex = 2, loopTo = aryConnectScriptLines.GetUpperBound(0) - 1; intIndex <= loopTo; intIndex += 2)
                {
                    if (string.IsNullOrEmpty(aryConnectScriptLines[intIndex]))
                        return true;
                    if (string.IsNullOrEmpty(aryConnectScriptLines[intIndex + 1]))
                        return false;
                    objHostPort.DataToSend(aryConnectScriptLines[intIndex] + Globals.CR);
                    Globals.queChannelDisplay.Enqueue("G     #Script(" + intIndex.ToString() + "):" + aryConnectScriptLines[intIndex] + Globals.CRLF);
                    if (WaitOnScriptResponse(aryConnectScriptLines[intIndex + 1]) == false)
                        return false;
                    Globals.queChannelDisplay.Enqueue("G     #Script(" + (1 + intIndex).ToString() + "):" + aryConnectScriptLines[intIndex + 1]);
                }

                if (objProtocol != null)
                    objProtocol.ChannelInput(strScriptResponse);
                return true;
            }
            catch
            {
                Globals.queChannelDisplay.Enqueue("R*** Connection script error");
                return false;
            }
        } // ConnectionScript

        private string FormatFirstScriptLine(string strLine)
        {
            strLine = strLine.ToUpper().Replace(" V ", " VIA ");
            var chrDelimiter = new char[] { ' ' };
            var strTokens = strLine.Trim().ToUpper().Split(chrDelimiter, StringSplitOptions.RemoveEmptyEntries);
            if (strTokens[0] == "C" | strTokens[0].StartsWith("CONN"))
                strTokens[0] = "";
            string strResult = "";
            foreach (string strToken in strTokens)
                strResult += strToken + " ";
            return "CO" + strResult.Trim();
        }

        private bool WaitOnScriptResponse(string strTarget)
        {
            strScriptResponse = "";
            var dttTimeout = DateTime.Now.AddSeconds(Globals.stcSelectedChannel.TNCScriptTimeout);
            do
            {
                if (dttTimeout < DateTime.Now)
                {
                    Globals.queChannelDisplay.Enqueue("G #Connect Script Timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC") + " ... Disconnecting");
                    return false;
                }

                if (blnClosed | Globals.blnProgramClosing)
                    return false;
                if (EndScript(strScriptResponse)) // Check for aborted script
                {
                    Globals.queChannelDisplay.Enqueue("G #Script stopped: " + strScriptResponse);
                    return false;
                }

                if (strScriptResponse.ToUpper().IndexOf(strTarget) != -1)
                    return true;
                if (blnClosed | Globals.blnProgramClosing)
                    return true;
                Thread.Sleep(100);
                Poll();
            }
            while (true);
            return false;
        } // WaitOnScriptResponse

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

        private void SendIdentification()
        {
            // Send a station identification in Pactor FEC mode...

            // Must be a Pactor link and the ID option is set...
            if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC & Globals.stcSelectedChannel.PactorId & blnSendingID == false)
            {
                Globals.queChannelDisplay.Enqueue("G*** Sending station identification ...");
                blnSendingID = true;

                // Set the TNC to transmitting FEC...
                objHostPort.HostCommand("PD");
                objHostPort.WaitOnHostCommandResponse("PD");

                // Send the site callsign...
                DataToSend("DE " + Globals.SiteCallsign);

                // Give the TNC the time it takes to send the callsign...
                var dttTimer = DateTime.Now.AddSeconds(7);
                do
                {
                    Poll();
                    if (dttTimer < DateTime.Now)
                        break;
                    Thread.Sleep(100);
                }
                while (true);
                objHostPort.HostCommand("RC");
                objHostPort.WaitOnHostCommandResponse("RC");
                dttTimer = DateTime.Now.AddSeconds(2);
                do
                {
                    Poll();
                    if (dttTimer < DateTime.Now)
                        break;
                    Thread.Sleep(100);
                }
                while (true);
            }

            blnSendingID = false;
        } // SendIdentification
    }

    // This class implements the WA8DED host mode protocol...
    internal class WA8DEDHostPort
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private const byte Command = 0x4F;
        private const byte DataOut = 0x20;
        private const byte DataIn = 0x30;
        private const byte Echo = 0x2F;
        private const byte Report = 0x50;
        private const byte Link = 0x5F;
        private const byte Status = 0x40;
        private const byte SOH = 0x1;
        private const byte DLE = 0x10;
        private const byte ETB = 0x17;

        // Queues...
        public Queue DataBlocksReceived = Queue.Synchronized(new Queue());
        private Queue queDataBlockOut = Queue.Synchronized(new Queue());
        private Queue queStatusReports = Queue.Synchronized(new Queue());
        private Queue quePortInput = Queue.Synchronized(new Queue());

        // Structures and enumerations
        private LinkDirection enmLinkDirection;

        // Byte arrays
        private byte[] bytCommandHeader = new byte[] { SOH, Command };
        private byte[] bytDataHeader = new byte[] { SOH, DataOut };
        private byte[] bytETB = new byte[] { ETB };
        private byte[] bytPacketStatusRequest = new byte[] { SOH, Command, 0x43, 0x4F, ETB };
        private byte[] bytPactorStatusRequest = new byte[] { SOH, Command, 0x4F, 0x50, ETB };
        private byte[] bytExitHostMode = new byte[] { SOH, SOH, Command, 0x48, 0x4F, 0x4E, ETB };
        private byte[] bytExitKissMode = new byte[] { 0xC0, 0xFF, 0xC0 };

        // Characters
        private char chrPactorState;
        private char chrPactorRate;

        // Strings
        private string strCommandToSend;
        private string strCommandResponse;
        private string strAPSFile;

        // Booleans
        private bool blnHostMode;
        private bool blnRequestTurnover;
        private bool blnClearToSend;
        private bool blnPactorSending;
        private bool blnClosing;
        private bool blnAbort;

        // Integers
        private int intPacketsOutstanding;

        // Objects and classes
        private SerialPort _objSerial;

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
                _objSerial = value;
            }
        }

        private object objPollOutgoingLock = new object();

        public bool IsOpen
        {
            get
            {
                return blnHostMode;
            }
        } // IsOpen

        public string StatusReports
        {
            get
            {
                if (queStatusReports.Count > 0)
                {
                    return Convert.ToString(queStatusReports.Dequeue());
                }
                else
                {
                    return "";
                }
            }
        } // StatusReports

        public bool SendComplete
        {
            get
            {
                return !(queDataBlockOut.Count > 0); // Or blnUnconfirmedPackets)
            }
        } // SendComplete

        public int OutboundQueueCount
        {
            get
            {
                return queDataBlockOut.Count;
            }
        } // OutboundQueueCount

        public void Open()
        {
            Globals.ResetProgressBar();
            strAPSFile = Globals.stcSelectedChannel.TNCConfigurationFile;
            if (File.Exists(strAPSFile) == false)
            {
                Globals.queChannelDisplay.Enqueue("R*** Configuration file not found");
                return;
            }

            // Open the serial port...
            try
            {
                objSerial = new SerialPort();
                objSerial.DataReceived += DataReceivedEvent;
                objSerial.PortName = Globals.stcSelectedChannel.TNCSerialPort;
                objSerial.ReceivedBytesThreshold = 1;
                objSerial.BaudRate = Convert.ToInt32(Globals.stcSelectedChannel.TNCBaudRate);
                objSerial.DataBits = 8;
                objSerial.StopBits = StopBits.One;
                objSerial.Parity = Parity.None;
                objSerial.Handshake = Handshake.None;
                objSerial.RtsEnable = true;
                objSerial.DtrEnable = true;
                objSerial.Open();
                if (objSerial.IsOpen == false)
                {
                    Globals.queChannelDisplay.Enqueue("R*** Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort + ". Port may be in use by another application.");
                    Log.Error("[PortWA8DEDHost.New] Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort);
                    // objSerial.Dispose()
                    objSerial = null;
                    return;
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("G*** Serial port " + Globals.stcSelectedChannel.TNCSerialPort + " opened");
                }
            }
            catch (Exception ex)
            {
                Globals.queChannelDisplay.Enqueue("R*** Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort + ". Port may be in use by another application.");
                Log.Error("[PortWA8DEDHost.OpenSerialPortHostMode] " + ex.Message);
                // objSerial.Dispose()
                objSerial = null;
                return;
            }

            ConfigureTNC();
        } // New

        public void Close()
        {
            if (objSerial is object)
            {
                if (objSerial.IsOpen)
                {
                    objSerial.WriteTimeout = 1500;
                    objSerial.Write(bytExitHostMode, 0, bytExitHostMode.Length);
                    Thread.Sleep(100);
                }

                objSerial.Close();
                Thread.Sleep(Globals.intComCloseTime);
                // objSerial.Dispose()
                objSerial = null;
                Globals.queChannelDisplay.Enqueue("G*** Serial port closed");
            }
        } // Close

        public void Abort()
        {
            blnAbort = true;
        } // Abort

        public void HostCommand(string strCommand)
        {
            if (blnHostMode)
            {
                strCommandResponse = "";
                strCommandToSend = strCommand;
            }
        } // HostCommand

        public bool NonHostCommand(string strCommand)
        {
            if (blnAbort)
                return true;
            if (blnHostMode == false)
            {
                objSerial.Write(strCommand + Globals.CR);
                strCommandResponse = "";
                Poll();
                var intLoopCount = default(int);
                while (intLoopCount <= 5)
                {
                    Thread.Sleep(50);
                    intLoopCount += 1;
                    Poll();
                }

                strCommandResponse = "";
                return true;
            }
            else
            {
                return false;
            }
        } // NonHostCommand

        public void DataToSend(string strData, bool blnTurnover = false)
        {
            var objEncoder = new ASCIIEncoding();
            DataToSend(Globals.GetBytes(strData), blnTurnover);
        } // DataToSend (String)

        public void DataToSend(byte[] bytData, bool blnTurnover = false)
        {
            int intIndex;

            // Chop inbound data into 128 byte blocks (plus DLE escape sequences)...
            var queBlockBuffers = new Queue();
            int intWholeBlockCount = bytData.Length / 128;
            int intFractionalBlockSize = bytData.Length % 128;
            if (intWholeBlockCount > 0)
            {
                var loopTo = intWholeBlockCount - 1;
                for (intIndex = 0; intIndex <= loopTo; intIndex++)
                {
                    var bytBlock = new byte[128];
                    Array.Copy(bytData, intIndex * 128, bytBlock, 0, 128);
                    queBlockBuffers.Enqueue(bytBlock);
                }
            }

            if (intFractionalBlockSize > 0)
            {
                var bytPartialBlock = new byte[intFractionalBlockSize];
                Array.Copy(bytData, intWholeBlockCount * 128, bytPartialBlock, 0, intFractionalBlockSize);
                queBlockBuffers.Enqueue(bytPartialBlock);
            }

            // Stuff DLE escape sequences into individual 128 byte blocks...
            while (queBlockBuffers.Count > 0)
            {
                byte[] bytBlock;
                try
                {
                    bytBlock = (byte[])queBlockBuffers.Dequeue();
                }
                catch
                {
                    break;
                }

                var bytBuffer = new byte[bytBlock.Length * 2 + 1];
                int intPosition = 0;
                foreach (byte bytSingle in bytBlock)
                {
                    if (bytSingle == SOH)
                    {
                        bytBuffer[intPosition] = DLE;
                        intPosition += 1;
                        bytBuffer[intPosition] = SOH;
                        intPosition += 1;
                    }
                    else if (bytSingle == DLE)
                    {
                        bytBuffer[intPosition] = DLE;
                        intPosition += 1;
                        bytBuffer[intPosition] = DLE;
                        intPosition += 1;
                    }
                    else if (bytSingle == ETB)
                    {
                        bytBuffer[intPosition] = DLE;
                        intPosition += 1;
                        bytBuffer[intPosition] = ETB;
                        intPosition += 1;
                    }
                    else
                    {
                        bytBuffer[intPosition] = bytSingle;
                        intPosition += 1;
                    }
                }

                Array.Resize(ref bytBuffer, intPosition);

                // intProgressBarDenominator += bytBuffer.Length
                queDataBlockOut.Enqueue(bytBuffer);
            }

            if (blnTurnover)
                blnTurnover = true;
        } // DataToSend (Byte())

        public string WaitOnHostCommandResponse(string strMatch, int intTimeoutSeconds = 5)
        {
            var dttTimeout = DateTime.Now.AddSeconds(intTimeoutSeconds);
            if (strMatch == "PD")
                blnPactorSending = true;
            bool blnResult = false;
            do
            {
                if (dttTimeout < DateTime.Now)
                    break;
                if (!string.IsNullOrEmpty(strCommandResponse))
                {
                    if (strCommandResponse.StartsWith(strMatch) || strCommandResponse == "OK")
                    {
                        return strCommandResponse;
                    }
                    else if (strCommandResponse.StartsWith("HO$01"))
                    {
                        blnHostMode = true;
                        return "HO";
                    }
                }

                Application.DoEvents();
                Poll();
            }
            while (true);
            return "";
        } // WaitOnCommandResponse

        private bool WaitOnNonHostCommandResponse(int intTimeoutSeconds = 3)
        {
            if (blnAbort)
                return true;
            var dttTimeout = DateTime.Now;
            bool blnResult = false;
            do
            {
                if (dttTimeout.AddSeconds(intTimeoutSeconds) > DateTime.Now)
                {
                    if (!string.IsNullOrEmpty(strCommandResponse))
                    {
                        if (strCommandResponse.ToUpper().IndexOf("?WHAT") != -1 | strCommandResponse.ToUpper().IndexOf("?BAD") != -1)
                        {
                            break;
                        }
                        else if (strCommandResponse.ToLower().IndexOf("cmd:") != -1)
                        {
                            return true;
                            break;
                        }
                    }
                }
                else
                {
                    return false;
                }

                Thread.Sleep(50);
                Poll();
            }
            while (true);
            return false;
        } // WaitOnNonHostResponse

        private bool WaitOnHostMode()
        {
            var dttTimeout = DateTime.Now.AddSeconds(5);
            do
            {
                if (dttTimeout < DateTime.Now)
                    return false;
                if (blnHostMode)
                    return true;
                Thread.Sleep(50);
                Poll();
            }
            while (true);
            return false;
        } // WaitOnHostMode

        public void Poll()
        {
            if (blnAbort)
                queDataBlockOut.Clear();
            ProcessDataReceivedQueue();
            PollOutgoing();
        } // ChannelPoll

        private void DataReceivedEvent(object s, SerialDataReceivedEventArgs e)
        {
            // Reads all pending bytes in the serial port and places them byte arrays on
            // the serial port input queue...

            if (objSerial is object && objSerial.IsOpen)
            {
                int intBytesToRead = objSerial.BytesToRead;
                int intBytesRead;
                var bytInputBuffer = new byte[intBytesToRead];
                intBytesRead = objSerial.Read(bytInputBuffer, 0, intBytesToRead);
                if (intBytesRead != intBytesToRead)
                {
                    Log.Error("[HostWA8DEDHost.DataReceivedEvent] Bytes read does not match bytes to read");
                }

                quePortInput.Enqueue(bytInputBuffer);
            }
        } // DataReceivedEvent 
       
        // Takes each partial host mode block from the serial port input queue and forms
        // finished host frames. Finished frames are processed by the ProcessReceivedFrame
        // method...
        private byte[] bytBlockBuffer = new byte[1000];
        private int intBlockPointer = 0;
        private bool blnDLE = false;
        private void ProcessDataReceivedQueue()
        {
            while (quePortInput.Count > 0)
            {
                byte[] bytBuffer;
                try
                {
                    bytBuffer = (byte[])quePortInput.Dequeue();
                }
                catch
                {
                    break;
                }

                foreach (byte bytSingle in bytBuffer)
                {
                    // Non-host mode processing...
                    if (blnHostMode == false)
                    {
                        strCommandResponse += Convert.ToString((char)bytSingle);
                        continue;
                    }

                    // Set flag on receipt of DLE...
                    if (bytSingle == DLE & blnDLE == false)
                    {
                        blnDLE = true;
                        continue;
                    }

                    // Reset block pointer on receipt of new SOH...
                    if (bytSingle == SOH & blnDLE == false)
                    {
                        intBlockPointer = 0;
                        bytBlockBuffer[intBlockPointer] = bytSingle;
                        intBlockPointer += 1;
                        continue;
                    }

                    // Process the completed host mode block on receipt of ETB...
                    if (bytSingle == ETB & blnDLE == false)
                    {
                        bytBlockBuffer[intBlockPointer] = bytSingle;
                        this.ProcessReceivedFrame(ref bytBlockBuffer, intBlockPointer);
                        intBlockPointer = 0;
                        continue;
                    }

                    // Clear the DLE flag...
                    blnDLE = false;

                    // Do nothing with stray characters outside of host blocks...
                    if (intBlockPointer == 0)
                    {
                        // Wait for an SOH to start a new block...
                        continue;
                    }

                    // Add the received byte to the block buffer...
                    bytBlockBuffer[intBlockPointer] = bytSingle;
                    intBlockPointer += 1;
                }
            }
        } // ProcessPortInput

        private void ProcessReceivedFrame(ref byte[] bytHostFrame, int intHostFrameUpperBound)
        {
            // Processes full assembled host frames an routes the received data
            // to the appropriate function...

            var switchExpr = bytHostFrame[1];
            switch (switchExpr)
            {
                case Command: // Response to an explicit TNC command
                    {
                        ProcessCommandResponse(Globals.GetString(bytHostFrame, 2, intHostFrameUpperBound - 1));
                        break;
                    }

                case DataIn: // Data received from the radio channel
                    {
                        // Framing bytes removed...
                        if (!blnAbort)
                            DataBlocksReceived.Enqueue(RemoveFrame(ref bytHostFrame, intHostFrameUpperBound));
                        break;
                    }

                case Echo: // Pactor echoed data as sent
                    {
                        if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
                        {
                            Globals.UpdateProgressBar(intHostFrameUpperBound - 2);
                        }

                        break;
                    }

                case Report:
                    {
                        ProcessConnectionReports(Globals.GetString(bytHostFrame, 2, intHostFrameUpperBound - 1));
                        break;
                    }

                case Link:
                    {
                        ProcessLinkReports(Globals.GetString(bytHostFrame, 2, intHostFrameUpperBound - 1));
                        break;
                    }

                case Status:
                    {
                        ProcessStatusReports(Globals.GetString(bytHostFrame, 2, intHostFrameUpperBound - 1));
                        break;
                    }
            }
        } // ProcessReceivedBlock 

        private byte[] RemoveFrame(ref byte[] bytHostFrame, int intHostFrameUpperBound)
        {
            // Removes host framing bytes and returns a byte array with the original data only...
            int intIndex;
            var bytDataBlock = new byte[intHostFrameUpperBound - 3 + 1];
            var loopTo = intHostFrameUpperBound - 1;
            for (intIndex = 2; intIndex <= loopTo; intIndex++)
                bytDataBlock[intIndex - 2] = bytHostFrame[intIndex];
            return bytDataBlock;
        } // RemoveEscapes

        private DateTime dttStatusRequestTimer = DateTime.MinValue;
        private void PollOutgoing()
        {
            // Called by the ChannelPoll timer event at frequent intervals.
            // Polls the pending data queues and sends host framed data to the serial port...
            lock (objPollOutgoingLock)
            {
                try
                {
                    if (dttStatusRequestTimer == DateTime.MinValue) 
                    {
                        dttStatusRequestTimer = DateTime.Now.AddMilliseconds(500);
                    }

                    if (blnHostMode & objSerial is object && objSerial.IsOpen)
                    {
                        // Status requests sent approximately every 1-1/2 seconds...
                        if (dttStatusRequestTimer < DateTime.Now)
                        {
                            dttStatusRequestTimer = DateTime.Now.AddMilliseconds((double)500);
                            if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
                            {
                                objSerial.Write(bytPactorStatusRequest, 0, 5);
                            }
                            else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
                            {
                                objSerial.Write(bytPacketStatusRequest, 0, 5);
                            }
                        }

                        if (!string.IsNullOrEmpty(strCommandToSend))
                        {
                            // Sends a TNC command. This is given priority over pending data...
                            objSerial.Write(bytCommandHeader, 0, 2);
                            objSerial.Write(strCommandToSend);
                            objSerial.Write(bytETB, 0, 1);
                            strCommandToSend = "";
                        }
                        // Sends a block of pending data...
                        else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC & blnPactorSending & queDataBlockOut.Count > 0 | Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC & queDataBlockOut.Count > 0)


                        {
                            if (blnClearToSend)
                            {
                                byte[] bytDataToSend;
                                try
                                {
                                    bytDataToSend = (byte[])queDataBlockOut.Dequeue();
                                }
                                catch
                                {
                                    return;
                                }

                                objSerial.Write(bytDataHeader, 0, 2);
                                objSerial.Write(bytDataToSend, 0, bytDataToSend.Length);
                                objSerial.Write(bytETB, 0, 1);
                                blnClearToSend = false;
                                intPacketsOutstanding += 1;
                                if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
                                {
                                    // Frames contain 128 byte (or less). An data frame containing more than
                                    // 128 byte of data contain escape sequences that should not be counted...
                                    if (bytDataToSend.Length > 128)
                                    {
                                        Globals.UpdateProgressBar(128);
                                    }
                                    else
                                    {
                                        Globals.UpdateProgressBar(bytDataToSend.Length);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("WA8DEDHostPort.PollOutgoing: " + ex.Message);
                }
            }
        } // PollOutgoing

        private void ProcessCommandResponse(string strResponse)
        {
            strCommandResponse = strResponse;
            if (strResponse.StartsWith("OPPA") | strResponse.StartsWith("OPPt"))
            {
                Globals.queStateDisplay.Enqueue("");
            }
            else if (strResponse.StartsWith("OPPG"))
            {
                chrPactorState = strResponse[4];
                if (Convert.ToString(strResponse[5]) == "S")
                {
                    blnPactorSending = true;
                }
                else
                {
                    blnPactorSending = false;
                }

                chrPactorRate = strResponse[6];
                if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
                {
                    string strDirection;
                    if (blnPactorSending)
                    {
                        strDirection = "Sending ";
                    }
                    else
                    {
                        strDirection = "Receiving ";
                    }

                    string strRate;
                    if (chrPactorRate == '1')
                    {
                        strRate = "100 Baud";
                    }
                    else
                    {
                        strRate = "200 Baud";
                    }

                    Globals.queRateDisplay.Enqueue(strRate);
                    var strState = default(string);
                    var switchExpr = chrPactorState;
                    switch (switchExpr)
                    {
                        case '0':
                            {
                                Globals.queStateDisplay.Enqueue("Pactor Standby");
                                break;
                            }

                        case '1':
                            {
                                Globals.queStateDisplay.Enqueue("Pactor Phasing");
                                break;
                            }

                        case '2':
                            {
                                Globals.queStateDisplay.Enqueue("Pactor In change over");
                                break;
                            }

                        case '3':
                            {
                                Globals.queStateDisplay.Enqueue(strState + "Pactor Idle " + strDirection + " " + strRate + " " + Globals.ProgressBarStatus());
                                break;
                            }

                        case '4':
                            {
                                Globals.queStateDisplay.Enqueue(strState + "Pactor Traffic " + strDirection + " " + strRate + " " + Globals.ProgressBarStatus());
                                break;
                            }

                        case '5':
                            {
                                Globals.queStateDisplay.Enqueue(strState + "Pactor Error " + strDirection + " " + strRate + " " + Globals.ProgressBarStatus());
                                break;
                            }

                        case '6':
                            {
                                Globals.queStateDisplay.Enqueue(strState + "Pactor RQ " + strDirection + " " + strRate + " " + Globals.ProgressBarStatus());
                                break;
                            }
                    }
                }
            }
            else if (strResponse.StartsWith("HB"))
            {
                Globals.queRateDisplay.Enqueue(strResponse.Substring(2) + " Baud");
            }
        } // ProcessCommandResponse

        private void ProcessConnectionReports(string strStatus)
        {
            queStatusReports.Enqueue(strStatus);
        } // ProcessStatusReports

        private void ProcessLinkReports(string strLinkReport)
        {
            if (strLinkReport.StartsWith("XX"))
                blnClearToSend = true;
        } // ProcessLinkReports

        private void ProcessStatusReports(string strStatus)
        {
            intPacketsOutstanding = Globals.Asc(strStatus[4]) - 0x30;
            int intRetries = Globals.Asc(strStatus[5]) - 0x30;
            string strReport = "Outstanding packets: " + intPacketsOutstanding.ToString() + "   Retrys: " + intRetries.ToString() + "   " + strStatus.Substring(7);
            Globals.queStateDisplay.Enqueue(strReport + "  " + Globals.ProgressBarStatus());
        } // ProcessPollResponse

        private void ConfigureTNC()
        {
            // Configures the TNC for operation with Paclink...

            // Test if the channel is set up for a fast start...
            var blnFastStart = default(bool);
            if (Globals.stcSelectedChannel.TNCConfigureOnFirstUseOnly | Globals.blnPactorDialogResuming == true)
            {
                if (Globals.cllFastStart.Contains(Globals.stcSelectedChannel.ChannelName))
                {
                    blnFastStart = true;
                }
            }

            // Confirm that the TNC is alive. If not start a full configuration...
            if (blnFastStart == true)
            {
                objSerial.Write(Convert.ToString('\u0003'));
                if (WaitOnNonHostCommandResponse() == false)
                    blnFastStart = false;
            }

            if (blnFastStart == false)
            {
                if (RecoverTNC() == false)
                    return;
                if (Globals.stcSelectedChannel.TNCType.StartsWith("PK-88") == false)
                    NonHostCommand("EXPERT ON");

                // Get configuration parameters...
                Globals.queChannelDisplay.Enqueue("G*** Configuring the TNC - Please wait...");
                var strConfiguration = default(string);
                if (File.Exists(strAPSFile))
                {
                    // Read the configuration files...
                    strConfiguration = File.ReadAllText(strAPSFile);
                }

                var objStringReader = new StringReader(strConfiguration);
                do
                {
                    if (blnClosing || Globals.blnProgramClosing || blnAbort)
                        return;
                    string strLine = objStringReader.ReadLine();
                    if (string.IsNullOrEmpty(strLine))
                        break;
                    var strCommand = strLine.Split(';');
                    if (!string.IsNullOrEmpty(strCommand[0].Trim()))
                    {
                        var strTokens = strCommand[0].Trim().Split(' ');
                        objSerial.Write(strCommand[0].Trim() + Globals.CR);
                        strCommandResponse = "";
                        if (WaitOnNonHostCommandResponse() == false)
                        {
                            Log.Error("[PortWA8DEDHost.ConfigureTNC] '" + strCommand[0] + "' failed");
                        }
                    }
                }
                while (true);
                if (Globals.stcSelectedChannel.TNCType.StartsWith("PK-232"))
                {
                    NonHostCommand("EAS ON");
                    NonHostCommand("ARQTMO 30");
                }
                else if (Globals.stcSelectedChannel.TNCType.StartsWith("DSP-232"))
                {
                    NonHostCommand("QPT 27");
                    NonHostCommand("MODEM 27");
                    NonHostCommand("EAS ON");
                    NonHostCommand("ARQTMO 30");
                    NonHostCommand("PT");
                }

                NonHostCommand("MYCALL " + Globals.SiteCallsign);
                NonHostCommand("AWLEN 8");
                NonHostCommand("PARITY 0");
                NonHostCommand("8BITCONV ON");
                NonHostCommand("ALFDISP OFF");
                NonHostCommand("ALFPACK OFF");
                NonHostCommand("HPOLL OFF");
                Globals.queChannelDisplay.Enqueue("G*** TNC Configuration complete");
            }

            objSerial.Write("HOST ON" + Globals.CR);
            Thread.Sleep(200);
            // Assume host mode enabled to send the host mode query...
            blnHostMode = true;

            // If host mode query fails then sent host mode flag to False...
            HostCommand("HO");
            string strHCR = WaitOnHostCommandResponse("--", 7);
            if (strHCR != "HO")
                blnHostMode = false;
            if (!blnHostMode)
            {
                if (blnAbort == false)
                    Globals.queChannelDisplay.Enqueue("R*** TNC failed to enter host mode");
                Log.Error(Globals.stcSelectedChannel.TNCType + "Failed to enter host mode. Response to HO cmd: " + strHCR);
                return;
            }

            if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PacketTNC)
            {
                HostCommand("PA");
                WaitOnHostCommandResponse("PA");
                HostCommand("HB");
                WaitOnHostCommandResponse("HB");
            }
            else if (Globals.stcSelectedChannel.ChannelType == EChannelModes.PactorTNC)
            {
                HostCommand("PT");
                WaitOnHostCommandResponse("PT");
            }

            blnClearToSend = true;
            if (Globals.cllFastStart.Contains(Globals.stcSelectedChannel.ChannelName) == false)
            {
                Globals.cllFastStart.Add(Globals.stcSelectedChannel.ChannelName);
            }
        } // ConfigureTNC

        private bool RecoverTNC()
        {
            // Try and jumpstart the TNC...

            // Try initializing the data rate to the TNC...
            strCommandResponse = "";
            objSerial.Write("*");
            if (WaitOnNonHostCommandResponse() == true)
            {
                return true;
            }

            // Test for a cmd: response...
            strCommandResponse = "";
            objSerial.Write(Convert.ToString('\u0003'));
            if (WaitOnNonHostCommandResponse() == true)
            {
                return true;
            }

            // Try exiting host mode...
            objSerial.Write(bytExitHostMode, 0, bytExitHostMode.Length);
            if (WaitOnNonHostCommandResponse(2) == true)
            {
                return true;
            }

            // Try exiting kiss mode...
            objSerial.Write(bytExitKissMode, 0, bytExitKissMode.Length);
            if (WaitOnNonHostCommandResponse(2) == true)
            {
                return true;
            }

            // Last resort try the auto baud
            for (int i = 1; i <= 8; i++) // Should cycle through about once per second
            {
                objSerial.Write("*");
                Thread.Sleep(200);
                objSerial.Write(Globals.CR);
                if (WaitOnNonHostCommandResponse(1) == true)
                {
                    return true;
                }
            }

            // Test again for a cmd: response...
            objSerial.Write(Convert.ToString('\u0003'));
            if (WaitOnNonHostCommandResponse() == true)
            {
                return true;
            }

            Globals.queChannelDisplay.Enqueue("R*** Could not find or recover the " + Globals.stcSelectedChannel.TNCType + " ... check connections, try power recycle.");
            Globals.queChannelDisplay.Enqueue("R***     Check connections");
            Globals.queChannelDisplay.Enqueue("R***     Check TNC baud rate");
            Globals.queChannelDisplay.Enqueue("R***     Try recycling TNC power");
            return false;
        } // RecoverTNC
    }
}