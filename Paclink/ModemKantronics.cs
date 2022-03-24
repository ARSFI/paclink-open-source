using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{

    // This class handles all Kantronics TNCs for both Packet and Pactor connections...
    public class ModemKantronics : IModem
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ModemKantronics()
        {
            this.RetryConnect += ClientKantronics_RetryConnect;
            // The class is instantiated to begin a channel connection...
            dttLastActivity = DateTime.Now;
            Globals.queStatusDisplay.Enqueue("Starting");
            Globals.queStatusDisplay.Enqueue("Starting");
            Globals.blnManualAbort = false;
            objHostPort = new KantronicsHostPort(ref Globals.stcSelectedChannel);
            Globals.blnChannelActive = true;
        } // New

        // Enumerations
        private LinkStates enmState = LinkStates.Undefined;

        // Date/Time
        private DateTime dttLastActivity;
        private DateTime dttLastBreakSent; // Time last break command was sent

        // Strings
        private string[] aryConnectScriptLines;
        private string strScriptResponse;

        // Boolean
        private bool blnInScript;   // Set while processing a connection script
        private bool blnClosed;     // Set when the channel is closed
        private bool blnAutomaticConnect;
        private bool blnSendingID;

        // Objects and Classes
        private KantronicsHostPort objHostPort; // Reference to the host port
        private ProtocolInitial objProtocol;    // Reference to the protocol handler

        // Events
        private event RetryConnectEventHandler RetryConnect;

        private delegate void RetryConnectEventHandler();

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

        public bool Close()
        {
            // Subroutine to close the channel and put TNC into known state...
            // Always call this method before the instance goes out of scope to
            // insure the channel is never left in Pactor mode...

            if (Globals.blnManualAbort == true)
            {
                objHostPort.LinkCommand('X');
                Thread.Sleep(5000);
                enmState = LinkStates.Disconnected;
            }

            if (blnClosed)
                return true;
            blnClosed = true;
            Globals.queChannelDisplay.Enqueue("G*** Closing " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
            if (Globals.objRadioControl != null) // Shut down the radio control and free the serial port
            {
                Globals.objRadioControl.Close();
                Thread.Sleep(Globals.intComCloseTime);
                Globals.objRadioControl = null;
            }

            if (objProtocol is object)
                objProtocol.CloseProtocol();
            var dttTimeout = DateTime.Now;
            while (!objHostPort.SendComplete)
            {
                if (dttTimeout.AddSeconds(60) < DateTime.Now)
                    break;
                Poll();
                Thread.Sleep(50);
            }

            dttTimeout = DateTime.Now;
            while (enmState != LinkStates.Disconnected)
            {
                if (dttTimeout.AddSeconds(60) < DateTime.Now)
                    break;
                Poll();
                Thread.Sleep(50);
            }

            objHostPort.Close();
            Thread.Sleep(Globals.intComCloseTime);
            Globals.queStatusDisplay.Enqueue("Idle");
            Globals.queRateDisplay.Enqueue("------");
            Globals.blnChannelActive = false;
            Globals.ObjSelectedModem = null;
            return true;
        } // Close 

        public void Abort()
        {
            // Forcably aborts a connection in progress...
            // This is used for any abnormal disconnect

            if (enmState != LinkStates.Connected)
            {
                Close();
                return;
            }

            if (objHostPort is object && objHostPort.IsOpen)
            {
                if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    Disconnect();
                }
                else
                {
                    Close();
                }
            }

            objHostPort.Abort();
        } // Abort 

        public bool Connect(bool blnAutomatic)
        {
            // Handles the initial TNC initialization and connection to a remote site...

            blnAutomaticConnect = blnAutomatic;
            dttLastActivity = DateTime.Now;
            Globals.queChannelDisplay.Enqueue("G*** Initializing TNC " + Globals.stcSelectedChannel.TNCType + " on " + Globals.stcSelectedChannel.TNCSerialPort + " with " + Globals.stcSelectedChannel.TNCConfigurationFile);
            if (objHostPort.Open() == false)
                return false;
            if (objHostPort.IsOpen == false)
            {
                return false;
            }

            // Separating out this test for Packet and Pactor allows using some 3x3 MARS calls which are 
            // Legal for KAM+ Pactor but illegal for Packet. Rev 2.0.66.0
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
            {
                objHostPort.HostCommand("MYCALL " + Globals.SiteCallsign);
                if (string.IsNullOrEmpty(objHostPort.WaitOnHostCommandResponse("MYCALL")))
                {
                    Globals.queChannelDisplay.Enqueue("R*** Configuration of the TNC failed");
                    enmState = LinkStates.Disconnected;
                    return false;
                }
            }
            else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
            {
                objHostPort.HostCommand("MYPTCALL " + Globals.SiteCallsign);
                if (string.IsNullOrEmpty(objHostPort.WaitOnHostCommandResponse("MYPTCALL")))
                {
                    Globals.queChannelDisplay.Enqueue("R*** Configuration of the TNC failed");
                    enmState = LinkStates.Disconnected;
                    return false;
                }
            }

            enmState = LinkStates.Initialized;
            if (Globals.stcSelectedChannel.RDOControl == "Serial") // now will handle both Packet and Pactor
            {
                if (Globals.objRadioControl != null)
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

            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
            {
                if (Globals.objRadioControl != null)
                    Globals.objRadioControl.SetParameters(ref Globals.stcSelectedChannel);
                if (string.IsNullOrEmpty(Globals.stcSelectedChannel.TNCScript))
                {
                    objHostPort.HostCommand("C " + Globals.stcSelectedChannel.RemoteCallsign);
                    Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign);
                    Globals.queStatusDisplay.Enqueue("Connecting");
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
            else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
            {
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
                        enmState = LinkStates.Disconnected;
                        Globals.dlgPactorConnect.Close();
                        Globals.dlgPactorConnect = null;
                        Close();
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

                // Make sure the TNC is in packet mode and disconnected before starting
                // a Pactor call...
                objHostPort.LinkCommand('X');

                // Start a Pactor call...
                objHostPort.HostCommand("PACTOR " + Globals.stcSelectedChannel.RemoteCallsign);
                Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign);
                Globals.queStatusDisplay.Enqueue("Connecting");
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
            // Sends byte stream data to the TNC...

            if (bytData.Length > 0)
            {
                dttLastActivity = DateTime.Now;
                objHostPort.DataToSend(bytData);
            }
        } // DataToSend (bytes) 

        public void Disconnect()
        {
            objHostPort.HostCommand("D");
            objHostPort.WaitOnHostCommandResponse("D");
            enmState = LinkStates.Disconnected;
        } // Disconnect 

        public void Poll()
        {
            if (objHostPort is object)
            {
                if (objHostPort.IsOpen)
                    objHostPort.Poll();
                do
                {
                    // Get unsolicited status reports...
                    string strStatusReport = objHostPort.StatusReports;
                    if (string.IsNullOrEmpty(strStatusReport))
                        break;
                    if (blnInScript)
                        strScriptResponse += strStatusReport;
                    Globals.queChannelDisplay.Enqueue("P" + strStatusReport.Trim());
                    if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PacketTNC)
                    {
                        if (strStatusReport.IndexOf("*** CONNECTED") != -1)
                        {
                            enmState = LinkStates.Connected;
                            objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                        }

                        if (strStatusReport.IndexOf("*** DISCONNECTED") != -1)
                        {
                            enmState = LinkStates.Disconnected;
                        }
                    }
                    else if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC)
                    {
                        if (strStatusReport.IndexOf("LINKED TO") != -1)
                        {
                            enmState = LinkStates.Connected;
                            objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                        }

                        if (strStatusReport.IndexOf("STANDBY") != -1)
                        {
                            if (blnSendingID)
                            {
                                blnSendingID = false;
                                enmState = LinkStates.Disconnected;
                                return;
                            }
                            else
                            {
                                SendIdentification();
                            }
                        }
                    }

                    strStatusReport = "";
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

                        dttLastActivity = DateTime.Now;
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
            // Function to initiate the connect script...
            try
            {
                blnInScript = true;
                if (string.IsNullOrEmpty(aryConnectScriptLines[0]))
                {
                    objHostPort.HostCommand("C " + Globals.stcSelectedChannel.RemoteCallsign);
                    Globals.queChannelDisplay.Enqueue("G*** Calling " + Globals.stcSelectedChannel.RemoteCallsign);
                    return true;
                }

                strScriptResponse = ""; // clear the script response string on start RMedits: Jan 17, 2008
                objHostPort.HostCommand(aryConnectScriptLines[0]);
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

                Globals.queChannelDisplay.Enqueue("G #End Script");
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

        private bool WaitOnScriptResponse(string strTarget)
        {
            var dttTimeout = DateTime.Now.AddSeconds(Globals.stcSelectedChannel.TNCScriptTimeout);
            do
            {
                if (dttTimeout < DateTime.Now)
                {
                    Globals.queChannelDisplay.Enqueue("G #Connect Script Timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC") + " - disconnecting");
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
                {
                    // Jan 17, 2008
                    // RM edits:
                    // Here we have to trim the strScriptResponse of everything before the match
                    // this should work and will pass the match and remainder of the response to the protocol
                    // on the last script line  
                    int intMatchStart = strScriptResponse.ToUpper().IndexOf(strTarget);
                    strScriptResponse = strScriptResponse.Substring(intMatchStart);
                    return true;
                }

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
            if (Globals.stcSelectedChannel.ChannelType == ChannelMode.PactorTNC & Globals.stcSelectedChannel.PactorId & blnSendingID == false)
            {
                Globals.queChannelDisplay.Enqueue("G*** Sending station identification ...");
                blnSendingID = true;

                // Set the TNC to transmitting FEC...
                objHostPort.LinkCommand('T');

                // Allow one second for the TNC to begin FEC transmission...
                var dttTimer = DateTime.Now.AddMilliseconds(1000);
                do
                {
                    Poll();
                    if (dttTimer < DateTime.Now)
                        break;
                    Thread.Sleep(100);
                }
                while (true);

                // Send the site callsign...
                DataToSend("DE " + Globals.SiteCallsign);

                // Give the TNC the time it takes to send the callsign...
                dttTimer = DateTime.Now.AddSeconds(5);
                do
                {
                    Poll();
                    if (dttTimer < DateTime.Now)
                        break;
                    Thread.Sleep(100);
                }
                while (true);
                objHostPort.LinkCommand('E');
            }
            else
            {
                enmState = LinkStates.Disconnected;
            }
        } // SendIdentification

        private void ClientKantronics_RetryConnect()
        {
            Thread.Sleep(500);
            Connect(false);
        } // ClientKantronics_RetryConnect
    }

    // This class implements the Kantronics host mode protocol...
    public class KantronicsHostPort
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Queue DataBlockIn = Queue.Synchronized(new Queue());
        private const byte Command = 0x43;
        private const byte Data = 0x44;
        private const byte Echo = 0x45;
        private const byte Report = 0x53;
        private const byte Link = 0x49;
        private const byte FEND = 0xC0;
        private const byte FESC = 0xDB;
        private const byte TFEND = 0xDC;
        private const byte TFESC = 0xDD;

        // Objects
        private object objPollOutgoingLock = new object();

        // Enumerations
        private LinkDirection enmLinkDirection;

        // Queues
        public Queue DataBlocksReceived = Queue.Synchronized(new Queue());
        private Queue queCommandToSend = Queue.Synchronized(new Queue());
        private Queue queDataBlockOut = Queue.Synchronized(new Queue());
        private Queue queLinkCommand = Queue.Synchronized(new Queue());
        private Queue queStatusReports = Queue.Synchronized(new Queue());
        private Queue quePortInput = Queue.Synchronized(new Queue());

        // Structures and Objects
        private ChannelProperties stcChannel;
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
                if (_objSerial != null)
                {
                    _objSerial.DataReceived -= objSerial_DataReceived;
                }

                _objSerial = value;
                if (_objSerial != null)
                {
                    _objSerial.DataReceived += objSerial_DataReceived;
                }
            }
        }

        // Arrays
        private byte[] bytStatusRequest = new byte[6];
        private byte[] bytCommandHeader = new byte[4];
        private byte[] bytDataHeader = new byte[4];
        private byte[] bytFEND = new byte[] { 0xC0 };
        private byte[] bytExitHostMode = new byte[] { 0xC0, 0x71, 0xC0 };
        private byte[] bytExitKissMode = new byte[] { 0xC0, 0xFF, 0xC0 };

        // Bytes and Integers
        private byte bytChannel;
        private int intAvailableBufferOut;
        private int intTNCPort;

        // Date/Time...
        private DateTime dttIdleTimeout;

        // Strings
        private string strCommandToSend;
        private string strCommandResponse;
        private string strStatusResponse;
        private string strAPSFile;

        // Booleans
        private bool blnFENDEscape;
        private bool blnFESCEscape;
        private bool blnHostMode;
        private bool blnSending;
        private bool blnRequestStatus;
        private bool blnUnconfirmedPackets;
        private bool blnClosing;
        private bool blnAbort;

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
                    return queStatusReports.Dequeue().ToString();
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
                return !(queDataBlockOut.Count > 0 | blnUnconfirmedPackets);
            }
        } // SendComplete

        public int OutboundQueueCount
        {
            get
            {
                return queDataBlockOut.Count;
            }
        } // OutboundQueueCount

        public bool IsISS
        {
            get
            {
                return blnSending;
            }
        }

        public KantronicsHostPort(ref ChannelProperties stcNewChannel)
        {
            stcChannel = stcNewChannel;
            Globals.ResetProgressBar();
        }

        public bool Open()
        {
            if (objSerial == null)
            {
                intTNCPort = stcChannel.TNCPort;
                strAPSFile = stcChannel.TNCConfigurationFile;
                if (File.Exists(strAPSFile) == false)
                {
                    Globals.queChannelDisplay.Enqueue("R*** Configuration file not found");
                    return false;
                }

                if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    bytChannel = 0x41; // "A"
                }
                else if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    bytChannel = 0x30; // "0"
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("R*** Channel configuration error");
                    return false;
                }

                // Preset header byte arrays...
                bytStatusRequest[0] = FEND;
                bytStatusRequest[1] = 0x43;
                var switchExpr = intTNCPort;
                switch (switchExpr)
                {
                    case 0:
                        {
                            bytStatusRequest[2] = 0x30;
                            break;
                        }

                    case 1:
                        {
                            bytStatusRequest[2] = 0x31;
                            break;
                        }

                    case 2:
                        {
                            bytStatusRequest[2] = 0x32;
                            break;
                        }
                }

                bytStatusRequest[3] = 0x30;
                bytStatusRequest[4] = 0x53;
                bytStatusRequest[5] = FEND;
                bytCommandHeader[0] = FEND;
                bytCommandHeader[1] = 0x43;
                var switchExpr1 = intTNCPort;
                switch (switchExpr1)
                {
                    case 0:
                        {
                            bytCommandHeader[2] = 0x30;
                            break;
                        }

                    case 1:
                        {
                            bytCommandHeader[2] = 0x31;
                            break;
                        }

                    case 2:
                        {
                            bytCommandHeader[2] = 0x32;
                            break;
                        }
                }

                bytCommandHeader[3] = bytChannel;
                bytDataHeader[0] = FEND;
                bytDataHeader[1] = 0x44;
                var switchExpr2 = intTNCPort;
                switch (switchExpr2)
                {
                    case 0:
                        {
                            bytDataHeader[2] = 0x30;
                            break;
                        }

                    case 1:
                        {
                            bytDataHeader[2] = 0x31;
                            break;
                        }

                    case 2:
                        {
                            bytDataHeader[2] = 0x32;
                            break;
                        }
                }

                bytDataHeader[3] = bytChannel;

                // Open the serial port...
                try
                {
                    objSerial = new SerialPort();
                    objSerial.DataReceived += DataReceivedEvent;
                    objSerial.PortName = stcChannel.TNCSerialPort;
                    objSerial.ReceivedBytesThreshold = 1;
                    objSerial.BaudRate = Convert.ToInt32(stcChannel.TNCBaudRate);
                    objSerial.DataBits = 8;
                    objSerial.StopBits = StopBits.One;
                    objSerial.Parity = Parity.None;
                    objSerial.Handshake = Handshake.None;
                    objSerial.RtsEnable = true;
                    objSerial.DtrEnable = true;
                    try
                    {
                        objSerial.Open();
                    }
                    catch
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Failed to open serial port on " + stcChannel.TNCSerialPort + ". Port may be in use by another application.");
                        Log.Error("[KantronicsHostPort.Open] Failed to open serial port on " + stcChannel.TNCSerialPort);
                        return false;
                    }

                    if (objSerial.IsOpen == false)
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Failed to open serial port on " + stcChannel.TNCSerialPort + ". Port may be in use by another application.");
                        Log.Error("[PortKantronicsHost.New A] Failed to open serial port on " + stcChannel.TNCSerialPort);
                        return false;
                    }
                    else
                    {
                        Globals.queChannelDisplay.Enqueue("G*** Serial port " + stcChannel.TNCSerialPort + " opened");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("[ModemKantronics.OpenSerialPortHostMode B] " + ex.Message);
                    return false;
                }

                return ConfigureTNC();
            }

            return default;
        } // New

        public void Close()
        {
            if (objSerial is object)
            {
                if (objSerial.IsOpen)
                {
                    objSerial.WriteTimeout = 1500;
                    objSerial.Write(bytExitHostMode, 0, 3);
                    Thread.Sleep(100);
                }

                objSerial.Close();
                Thread.Sleep(Globals.intComCloseTime);
                // objSerial.Dispose()
                objSerial = null;
            }
        } // Close

        public void Abort()
        {
            blnAbort = true;
        } // Abort

        public void LinkCommand(char chrCommand)
        {
            // Posts a single character Pactor link command...

            if (chrCommand == 'T')
                enmLinkDirection = LinkDirection.Sending;
            queLinkCommand.Enqueue(chrCommand);
            PollOutgoing();
        } // LinkCommand

        public void HostCommand(string strCommand)
        {
            strCommandResponse = "";
            strCommandToSend = strCommand;
        } // HostCommand

        public void DataToSend(string strData)
        {
            var objEncoder = new ASCIIEncoding();
            DataToSend(Globals.GetBytes(strData));
        } // DataToSend (String)

        public void DataToSend(byte[] bytData)
        {
            int intIndex;

            // Chop inbound data into 128 byte blocks...
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

            // Stuff escape sequences into individual 128 byt blocks for embedded FEND and FESC bytes...
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
                    if (bytSingle == FEND)
                    {
                        bytBuffer[intPosition] = 0xDB;
                        intPosition += 1;
                        bytBuffer[intPosition] = 0xDC;
                        intPosition += 1;
                    }
                    else if (bytSingle == FESC)
                    {
                        bytBuffer[intPosition] = 0xDB;
                        intPosition += 1;
                        bytBuffer[intPosition] = 0xDD;
                        intPosition += 1;
                    }
                    else
                    {
                        bytBuffer[intPosition] = bytSingle;
                        intPosition += 1;
                    }
                }

                Array.Resize(ref bytBuffer, intPosition);
                queDataBlockOut.Enqueue(bytBuffer);
            }
        } // DataToSend (Byte())

        public void SendingFECMode()
        {
            enmLinkDirection = LinkDirection.Sending;
        }

        public string WaitOnHostCommandResponse(string strMatch, int intTimeoutSeconds = 2)
        {
            var dttTimeout = DateTime.Now;
            bool blnResult = false;
            do
            {
                if (dttTimeout.AddSeconds(intTimeoutSeconds) > DateTime.Now)
                {
                    if (!string.IsNullOrEmpty(strCommandResponse))
                    {
                        if (strCommandResponse.IndexOf(strMatch) != -1 || strCommandResponse == "OK")
                        {
                            return strCommandResponse;
                        }
                    }
                }
                else
                {
                    return "";
                }

                Thread.Sleep(50);
                if (blnClosing | Globals.blnProgramClosing | blnAbort)
                    return "OK";
                Poll();
            }
            while (true);
            return "";
        } // WaitOnCommandResponse

        private bool WaitOnNonHostCommandResponse(string strMatch, int intTimeoutSeconds = 2)
        {
            var dttTimeout = DateTime.Now;
            bool blnResult = false;
            do
            {
                if (dttTimeout.AddSeconds(intTimeoutSeconds) > DateTime.Now)
                {
                    if (!string.IsNullOrEmpty(strCommandResponse))
                    {
                        if (strCommandResponse.IndexOf("cmd:") != -1)
                        {
                            strCommandResponse = "";
                            return true;
                        }

                        if (strCommandResponse.IndexOf("CALLSIGN=>") != -1)
                        {
                            strCommandResponse = "";
                            objSerial.Write(Globals.SiteCallsign + Globals.CR);
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }

                Thread.Sleep(50);
                if (blnClosing | Globals.blnProgramClosing | blnAbort)
                    return true;
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
                {
                    return true;
                }

                Thread.Sleep(60);
                if (blnClosing | Globals.blnProgramClosing)
                    return true;
                Poll();
            }
            while (true);
            return false;
        } // WaitOnHostMode

        private void DataReceivedEvent(object s, SerialDataReceivedEventArgs e)
        {
            // Reads all pending bytes in the serial port and places them byte arrays on
            // the serial port input queue...

            int intBytesToRead = objSerial.BytesToRead;
            int intBytesRead;
            var bytInputBuffer = new byte[intBytesToRead];
            intBytesRead = objSerial.Read(bytInputBuffer, 0, intBytesToRead);
            if (intBytesRead != intBytesToRead)
            {
                Log.Error("[HostPortKantronics.DataReceivedEvent] Bytes read does not match bytes to read");
            }

            quePortInput.Enqueue(bytInputBuffer);
        } // DataReceivedEvent

        public void Poll()
        {
            // Polls for incoming and outgoing data flow...
            ProcessPortInput();
            PollOutgoing();
        } // ChannelPoll
          // Takes each partial host mode block from the serial port input queue and forms
          // finished blocks. Finished blocks are processed by the ProcessReceivedBlock
          // method...

        private byte[] bytBlockBuffer = new byte[1000];
        private int intBlockPointer = 0;
        private byte bytPrevious = 0;

        private void ProcessPortInput()
        {
            // Takes each partial host mode block from the serial port input queue and forms
            // finished blocks. Finished blocks are processed by the ProcessReceivedBlock
            // method...
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
                    if (blnHostMode == false)
                    {
                        // Non-host mode processing...
                        strCommandResponse += ((char)bytSingle).ToString();

                        // A double FEND indicates entry into host mode...
                        if (bytSingle == FEND & bytPrevious == FEND)
                        {
                            blnHostMode = true;
                        }
                    }
                    else
                    {
                        bytBlockBuffer[intBlockPointer] = bytSingle;
                        if (bytSingle == FEND)
                        {
                            var switchExpr = intBlockPointer;
                            switch (switchExpr)
                            {
                                case 0:
                                    {
                                        break;
                                    }
                                // Looking good - do nothing...
                                case 1:
                                    {
                                        // Reset block pointer to 0...
                                        intBlockPointer = 0;
                                        break;
                                    }

                                case 2:
                                    {
                                        // This has to be an error...
                                        bytBlockBuffer[intBlockPointer] = 0;
                                        break;
                                    }

                                case 3:
                                    {
                                        // This also has to an error...
                                        bytBlockBuffer[intBlockPointer] = 0;
                                        break;
                                    }

                                default:
                                    {
                                        // This ends the block...
                                        bytBlockBuffer[intBlockPointer] = bytSingle;

                                        // Process the completed host mode block...
                                        this.ProcessReceivedFrame(ref bytBlockBuffer, intBlockPointer);
                                        intBlockPointer = -1;
                                        break;
                                    }
                            }
                        }
                        else if (intBlockPointer == 0)
                        {
                            // Wait for an FEND to start a new block...
                            continue;
                        }
                        else
                        {
                            // Add the received byte to the block buffer...
                            bytBlockBuffer[intBlockPointer] = bytSingle;
                        }

                        intBlockPointer += 1;
                    }

                    bytPrevious = bytSingle;
                }
            }
        } // ProcessPortInput

        private void ProcessReceivedFrame(ref byte[] bytHostFrame, int intHostFrameUpperBound)
        {
            var switchExpr = bytHostFrame[1];
            switch (switchExpr)
            {
                case Command: // "C" - Responding to an explicit TNC command
                    {
                        string strResponse = Globals.GetString(bytHostFrame, 4, intHostFrameUpperBound - 1);
                        if (strResponse.StartsWith("FREE BYTES"))
                        {
                            ProcessStatusReports(strResponse);
                        }
                        else
                        {
                            if (strResponse.StartsWith("HBAUD"))
                            {
                                var strTokens = strResponse.Substring(5).Trim().Split('/');
                                if (strTokens.Length == 1)
                                {
                                    Globals.queRateDisplay.Enqueue(strTokens[0]);
                                }
                                else if (strTokens.Length == 2)
                                {
                                    if (stcChannel.TNCType == "KAM/+" | stcChannel.TNCType == "KAMXL")
                                    {
                                        Globals.queRateDisplay.Enqueue(strTokens[1]);
                                    }
                                    else if (stcChannel.TNCType == "KPC9612" | stcChannel.TNCType == "KPC9612+" | stcChannel.TNCType == "KPC4")

                                    {
                                        Globals.queRateDisplay.Enqueue(strTokens[stcChannel.TNCPort - 1]);
                                    }
                                    else
                                    {
                                        Globals.queRateDisplay.Enqueue(strTokens[0]);
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(strResponse))
                                strResponse = "OK";
                            strCommandResponse = strResponse;
                        }

                        break;
                    }

                case Data: // "D" - Data received from the radio channel
                    {
                        // Framing bytes and escape sequences removed...
                        DataBlocksReceived.Enqueue(RemoveEscapes(ref bytHostFrame, intHostFrameUpperBound));
                        break;
                    }

                case Echo: // "E" - Echoed data as sent
                    {
                        // Data echoed as sent...
                        if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                        {
                            Globals.UpdateProgressBar(intHostFrameUpperBound - 4);
                        }

                        break;
                    }

                case Report:  // "S" - An unsolicitated report (Example: "*** DISCONNECTED")
                    {
                        queStatusReports.Enqueue(Globals.GetString(bytHostFrame, 4, intHostFrameUpperBound - 1));
                        break;
                    }

                case Link:    // "I" - Pactor link state reports
                    {
                        ProcessLinkReports(ref bytHostFrame);
                        break;
                    }
            }
        } // ProcessReceivedBlock 

        private byte[] RemoveEscapes(ref byte[] bytHostFrame, int intHostFrameUpperBound)
        {
            // Removes hast framing bytes and escape sequences from the host from and
            // return a byte array with the original data only...

            var bytDataBlock = new byte[intHostFrameUpperBound + 1];
            int intDataBlockPosition = 0;
            int intHostFramePosition;
            var loopTo = intHostFrameUpperBound - 1;
            for (intHostFramePosition = 4; intHostFramePosition <= loopTo; intHostFramePosition++)
            {
                byte bytSingle = bytHostFrame[intHostFramePosition];
                if (bytHostFrame[intHostFramePosition - 1] == FESC)
                {
                    if (bytSingle == TFEND)
                    {
                        bytDataBlock[intDataBlockPosition - 1] = FEND;
                    }
                    else if (bytSingle == TFESC)
                    {
                    }
                    // Do nothing...
                    else
                    {
                        // Copy a byte to bytDataBlock...
                        bytDataBlock[intDataBlockPosition] = bytSingle;
                        intDataBlockPosition += 1;
                    }
                }
                else
                {
                    // Copy a byte to bytDataBlock...
                    bytDataBlock[intDataBlockPosition] = bytSingle;
                    intDataBlockPosition += 1;
                }
            }

            Array.Resize(ref bytDataBlock, intDataBlockPosition);
            return bytDataBlock;
        } // RemoveEscapes

        private DateTime dttStatusRequestTimer = DateTime.Now;

        private void PollOutgoing()
        {
            lock (objPollOutgoingLock)
            {
                try
                {
                    if (blnHostMode & objSerial is object && objSerial.IsOpen)
                    {
                        if (dttStatusRequestTimer < DateTime.Now)
                        {
                            dttStatusRequestTimer = DateTime.Now.AddMilliseconds((double)1500);
                            objSerial.Write(bytStatusRequest, 0, 6);
                        }

                        if (intAvailableBufferOut > 300)
                        {
                            if (queLinkCommand.Count > 0)
                            {
                                // Sends a single character link command. This is give priority over
                                // TNC commands and pending data...
                                char chrCommand = ' ';
                                try
                                {
                                    chrCommand = Convert.ToChar(queLinkCommand.Dequeue());
                                }
                                catch (Exception ex)
                                {
                                    Log.Error("KantronicsHostPort.PollOutgoing: " + ex.Message);
                                }

                                objSerial.Write(bytFEND, 0, 1);
                                objSerial.Write(chrCommand.ToString());
                                objSerial.Write(bytFEND, 0, 1);
                            }
                            else if (!string.IsNullOrEmpty(strCommandToSend))
                            {
                                // Sends a TNC command. This is given priority over pending data...
                                blnUnconfirmedPackets = true;
                                objSerial.Write(bytCommandHeader, 0, 4);
                                objSerial.Write(strCommandToSend);
                                objSerial.Write(bytFEND, 0, 1);
                                intAvailableBufferOut -= strCommandToSend.Length + 5;
                                strCommandToSend = "";
                            }
                            else if (enmLinkDirection == LinkDirection.Sending | stcChannel.ChannelType != ChannelMode.PactorTNC)
                            {
                                while (queDataBlockOut.Count > 0 & intAvailableBufferOut > 128)
                                {
                                    // Sends a block of pending data...
                                    blnUnconfirmedPackets = true;
                                    byte[] bytDataToSend;
                                    try
                                    {
                                        bytDataToSend = (byte[])queDataBlockOut.Dequeue();
                                    }
                                    catch
                                    {
                                        break;
                                    }

                                    objSerial.Write(bytDataHeader, 0, 4);
                                    objSerial.Write(bytDataToSend, 0, bytDataToSend.Length);
                                    objSerial.Write(bytFEND, 0, 1);
                                    intAvailableBufferOut -= 128;
                                    if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                                        Globals.UpdateProgressBar(bytDataToSend.Length);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("KantronicsHostPort.PollOutgoing: " + ex.Message);
                }
            }
        } // PollOutgoing

        private int intBufferOffset = 0;

          // Process the status report from the TNC...
        private void ProcessStatusReports(string strStatus)
        {
            if (enmLinkDirection == LinkDirection.Receiving)
            {
                blnSending = false;
                Globals.queStateDisplay.Enqueue("Pactor Receiving  " + Globals.ProgressBarStatus());
            }
            else if (enmLinkDirection == LinkDirection.Sending)
            {
                blnSending = true;
                Globals.queStateDisplay.Enqueue("Pactor Sending " + Globals.ProgressBarStatus());
            }

            var strLines = strStatus.Split('\r');
            if (strLines.Length >= 2)
            {
                if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    string strReport = strLines[strLines.GetUpperBound(0) - 1];
                    Globals.queStateDisplay.Enqueue(strReport + "  " + Globals.ProgressBarStatus());
                    if (strReport.IndexOf("DISCONNECTED") != -1)
                    {
                        enmLinkDirection = LinkDirection.Disconnected;
                        Globals.queStateDisplay.Enqueue("");
                    }
                }
            }

            if (strLines[1].IndexOf("#") == -1)
                blnUnconfirmedPackets = false;
            else
                blnUnconfirmedPackets = true;
            string strSeparators = " /" + Globals.CRLF;
            var strTokens = strLines[0].Split(strSeparators.ToCharArray());

            // Update the available outbound buffer available...
            if (strTokens.Length >= 3)
            {
                int intBuffer = 0;
                if (int.TryParse(strTokens[strTokens.GetUpperBound(0)], out intBuffer))
                {
                    if (intBufferOffset < intBuffer - 1024)
                    {
                        intBufferOffset = intBuffer - 1024;
                    }

                    intAvailableBufferOut = intBuffer - intBufferOffset;
                }
            }
        } // ProcessStatus

        private void ProcessLinkReports(ref byte[] bytFrame)
        {
            var switchExpr = bytFrame[3];
            switch (switchExpr)
            {
                case 0x30:
                    {
                        enmLinkDirection = LinkDirection.Receiving;
                        Globals.queStateDisplay.Enqueue("Pactor Receiving  " + Globals.ProgressBarStatus());
                        break;
                    }

                case 0x31:
                    {
                        enmLinkDirection = LinkDirection.Sending;
                        Globals.queStateDisplay.Enqueue("Pactor Sending");
                        break;
                    }
            }
        } // ProcessLinkReports

        private bool ConfigureTNC()
        {
            // Configures the TNC for operation with Paclink...

            // Check if full configuration is required...
            if (blnClosing || Globals.blnProgramClosing || blnAbort)
                return false;
            bool blnFastStart = false;
            if (stcChannel.TNCConfigureOnFirstUseOnly | Globals.blnPactorDialogResuming == true)
            {
                if (Globals.cllFastStart.Contains(stcChannel.ChannelName))
                {
                    blnFastStart = true;
                }
            }

            // Test for cmd: response...
            if (blnClosing || Globals.blnProgramClosing || blnAbort)
                return false;
            strCommandResponse = "";
            objSerial.Write(Globals.CR);
            if (!WaitOnNonHostCommandResponse("cmd:"))
            {
                if (!RecoverTNC())
                {
                    Globals.queChannelDisplay.Enqueue("R*** Could not find or recover the " + stcChannel.TNCType);
                    Log.Error("[PortKantronicsHost.New C] Could not find or recover the TNC");
                    return false;
                }
            }

            // Set to host mode...
            if (blnClosing || Globals.blnProgramClosing || blnAbort)
                return false;
            strCommandResponse = "";
            objSerial.Write("INTFACE HOST" + Globals.CR);
            if (!WaitOnNonHostCommandResponse("cmd:"))
            {
                Globals.queChannelDisplay.Enqueue("R*** INTFACE HOST command failed");
                Log.Error("[PortKantronicsHost.New D] INTFACE HOST command failed");
                return false;
            }

            if (blnClosing || Globals.blnProgramClosing || blnAbort)
                return false;
            strCommandResponse = "";
            objSerial.Write("RESET" + Globals.CR);
            if (!WaitOnHostMode())
            {
                Globals.queChannelDisplay.Enqueue("R*** RESET command failed");
                Log.Error("[PortKantronicsHost.New B] RESET command failed");
                return false;
            }

            // Get configuration parameters...
            if (blnFastStart == false)
            {
                Globals.queChannelDisplay.Enqueue("G*** Configuring the TNC - Please wait...");
                var strConfiguration = default(string);
                if (File.Exists(strAPSFile))
                {
                    // Read the configuration files...
                    strConfiguration = File.ReadAllText(strAPSFile);
                }

                try
                {
                    var objStringReader = new StringReader(strConfiguration);
                    do
                    {
                        if (blnClosing || Globals.blnProgramClosing || blnAbort)
                            return false;
                        string strLine = objStringReader.ReadLine();
                        if (string.IsNullOrEmpty(strLine))
                            break;
                        if (strLine.IndexOf("MAXUSERS") != -1)
                            continue;
                        var strCommand = strLine.Split(';');
                        if (!string.IsNullOrEmpty(strCommand[0].Trim()))
                        {
                            var strTokens = strCommand[0].Trim().Split(' ');
                            HostCommand(strCommand[0]);
                            objSerial.Write(bytCommandHeader, 0, 4);
                            objSerial.Write(strCommandToSend);
                            objSerial.Write(bytFEND, 0, 1);
                            strCommandToSend = "";
                            Thread.Sleep(100);
                            // If WaitOnHostCommandResponse(strTokens(0).ToUpper) = "" Then
                            // Log.Error("[PortKantronicsHost.ConfigureTNC] '" & strCommand(0) & "' failed")
                            // End If
                        }

                        Thread.Sleep(1);
                        Application.DoEvents();
                        if (blnClosing | Globals.blnProgramClosing)
                            break;
                    }
                    while (true);
                }
                catch (Exception ex)
                {
                    Log.Error("[KantronicsHostPort.ConfigureTNC] " + ex.Message);
                }

                HostCommand("MAXUSERS 1");
                WaitOnHostCommandResponse("MAX", 6);
                if (blnClosing || Globals.blnProgramClosing || blnAbort)
                {
                    return false;
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("G*** TNC Configuration complete");
                }
            }

            if (stcChannel.TNCType == "KAM/+")
            {
                if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    HostCommand("PORT VHF");
                    WaitOnHostCommandResponse("PORT");
                    stcChannel.TNCPort = 2;
                    intTNCPort = 2;
                }
                else if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    HostCommand("PORT HF");
                    WaitOnHostCommandResponse("PORT");
                    stcChannel.TNCPort = 1;
                    intTNCPort = 1;
                    HostCommand("SPACE 4000");
                    WaitOnHostCommandResponse("SPACE");
                    HostCommand("MARK " + (Convert.ToInt32(stcChannel.AudioToneCenter) - 100).ToString());
                    WaitOnHostCommandResponse("MARK");
                    HostCommand("SPACE " + (Convert.ToInt32(stcChannel.AudioToneCenter) + 100).ToString());
                    WaitOnHostCommandResponse("SPACE");
                    HostCommand("SHIFT MODEM");
                    WaitOnHostCommandResponse("SHIFT");
                }
            }
            else if (stcChannel.TNCType == "KAMXL")
            {
                HostCommand("PORT " + stcChannel.TNCPort.ToString());
                WaitOnHostCommandResponse("PORT");
                if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    HostCommand("SPACE 4000");
                    WaitOnHostCommandResponse("SPACE");
                    HostCommand("MARK " + (Convert.ToInt32(stcChannel.AudioToneCenter) - 100).ToString());
                    WaitOnHostCommandResponse("MARK");
                    HostCommand("SPACE " + (Convert.ToInt32(stcChannel.AudioToneCenter) + 100).ToString());
                    WaitOnHostCommandResponse("SPACE");
                }
            }
            else if (stcChannel.TNCType == "KAM98")
            {
                if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    stcChannel.TNCPort = 2;
                    intTNCPort = 2;
                    HostCommand("SPACE 4000");
                    WaitOnHostCommandResponse("SPACE");
                    HostCommand("MARK 1200"); // Changed by RM from 1300 rev 2.0.61.0 Feb 15, 2006
                    WaitOnHostCommandResponse("MARK");
                    HostCommand("SPACE 2100");
                    WaitOnHostCommandResponse("SPACE");
                }
                else if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    stcChannel.TNCPort = 1;
                    intTNCPort = 1;
                    HostCommand("SPACE 4000");
                    WaitOnHostCommandResponse("SPACE");
                    HostCommand("MARK " + (Convert.ToInt32(stcChannel.AudioToneCenter) - 100).ToString());
                    WaitOnHostCommandResponse("MARK");
                    HostCommand("SPACE " + (Convert.ToInt32(stcChannel.AudioToneCenter) + 100).ToString());
                    WaitOnHostCommandResponse("SPACE");
                }
            }

            // Causes the data rate label to be set...
            if (stcChannel.ChannelType == ChannelMode.PacketTNC)
            {
                HostCommand("HB");
                WaitOnHostCommandResponse("HB");
            }
            else if (stcChannel.ChannelType == ChannelMode.PactorTNC)
            {
                Globals.queRateDisplay.Enqueue("200 Baud");
            }

            // Indicate that the named channel has been configured at least once...
            if (Globals.cllFastStart.Contains(stcChannel.ChannelName) == false)
            {
                Globals.cllFastStart.Add(stcChannel.ChannelName);
            }

            return true;
        } // ConfigureTNC

        private bool RecoverTNC()
        {
            strCommandResponse = "";
            objSerial.Write("*");
            WaitOnNonHostCommandResponse("cmd:");
            objSerial.Write(Convert.ToString('\u0003'));
            objSerial.Write(Globals.CR);
            if (WaitOnNonHostCommandResponse("cmd:") == true)
            {
                objSerial.Write("INTFACE TERMINAL" + Globals.CR); // insures not in NEW USER mode
                return true;
            }

            // Try exiting host mode...
            objSerial.Write(bytExitHostMode, 0, bytExitHostMode.Length);
            Thread.Sleep(1000);
            objSerial.Write(Globals.CR);
            if (WaitOnNonHostCommandResponse("cmd:", 2) == true)
            {
                objSerial.Write("INTFACE TERMINAL" + Globals.CR); // insures not in NEW USER mode
                return true;
            }

            // Try exiting kiss mode...
            objSerial.Write(bytExitKissMode, 0, bytExitKissMode.Length);
            Thread.Sleep(1000);
            objSerial.Write(Globals.CR);
            if (WaitOnNonHostCommandResponse("cmd:", 2) == true)
            {
                objSerial.Write("INTFACE TERMINAL" + Globals.CR); // insures not in NEW USER mode
                return true;
            }

            // last resort try the auto baud
            for (int i = 1; i <= 8; i++) // should cycle through about once per second
            {
                objSerial.Write("*");
                Thread.Sleep(200);
                objSerial.Write(Globals.CR);
                if (WaitOnNonHostCommandResponse("cmd:", 1) == true)
                {
                    objSerial.Write("INTFACE TERMINAL" + Globals.CR); // insures not in NEW USER mode
                    return true;
                }
            }

            Globals.queChannelDisplay.Enqueue("R*** Could not find or recover the " + stcChannel.TNCType);
            Globals.queChannelDisplay.Enqueue("R***     Check connections");
            Globals.queChannelDisplay.Enqueue("R***     Check TNC baud rate");
            Globals.queChannelDisplay.Enqueue("R***     Try recycling TNC power");
            return false;
        } // RecoverTNC

        private void objSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
        }
    }
}