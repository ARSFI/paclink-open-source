using System;
using System.Collections;
using System.IO;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using NLog;
using TNCKissInterface;

namespace Paclink
{
    public class ModemNativeKiss : IModem
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        // Enumerations
        private LinkStates enmState = LinkStates.Undefined;

        // Date/Time
        private DateTime dttLastActivity;
        private DateTime dttLastScript;
        private DateTime dttStartConnect;

        // Integer
        private int intConnectScriptPtr = -1;     // Pointer to the active connect script line, -1 disables scripting
        private int intBytesSentCount;            // Holds last bytes sent count
        private int intTXDelay = 50; // the delay from PTT to data start in 10's of ms
        private int intPersistance = 128;
        private int intSlottime = 7;
        private int intMaxFrameSize = 128;
        private int intMaxFrames = 4;
        private int intACKTimer = 3;
        private int intMaxRetry = 10;
        private int intPollThresh = 30;
        private int intBytesConfirmedSent = 0; // report of total bytes confirmed delivered to remote station. 
                                               // used to update progress bar.


        // Queues
        private Queue queTNCData = Queue.Synchronized(new Queue());

        // Bytes
        private byte[] bytExitKiss = new byte[] { 0xC0, 0xFF, 0xC0 }; // KISS Exit sequence

        // Strings
        private string strCommandReply;               // String reply from a command
        private string[] ConnectScript;               // An array of connection script entries
        private string strStatus;                     // Holds plain language status
        private string strScriptResponse;
        private string strAPSFile;
        private string[] strKissStart;       // The KISS start sequence of commands
        private string strEscapeChars = "";  // Optional escape characters for the KISS mode
        private string strBaudRate = "1200 Baud";

        // Boolean
        private bool blnInScript;   // Set while processing a connection script
        private bool blnClosed;     // Set when the channel is closed
        private bool blnFullDuplex = false;
        private bool blnExitKiss = true; // flag to enable kiss exit sequence on close

        // Objects and classes 
        private ProtocolInitial objProtocol;          // Instance of the message protocol handler
        private SerialPort _objSerial;                 // the serial port to communicate to the KISS TNC

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


                    // Read the bytes available to a synchronous queue
                    _objSerial.DataReceived -= objSerial_DataReceived;
                }

                _objSerial = value;
                if (_objSerial != null)
                {
                    _objSerial.DataReceived += objSerial_DataReceived;
                }
            }
        }

        private COMPort objKissComPort;
        private TNCChannel objKissChannel;
        private DataLinkProvider objKissDLProvider;
        private Connection.ConnectionParameterBuf objConParam;
        private Connection objCon = null;

        // Threads
        private Thread ResponderThread;
        private Thread MonitorThread;
        private Thread ReaderThread;

        // Events

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

        public ModemNativeKiss()
        {
            // The class is instantiated to begin a channel connection...
            dttLastActivity = DateTime.Now;
            Globals.queStatusDisplay.Enqueue("Starting");
            Globals.queStatusDisplay.Enqueue("Starting");
            Globals.blnManualAbort = false;
            Globals.ResetProgressBar();
            intBytesConfirmedSent = 0;
            if (Open())
            {
                Globals.blnChannelActive = true;
            }
            else
            {
                Globals.blnChannelActive = false;
                if (objSerial != null)
                {
                    objSerial.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    objSerial.Dispose();
                }

                Globals.ObjSelectedModem = null;
            }
        } // New

        // Function to open the KISS TNC 
        private bool Open()
        {
            bool OpenRet = default;
            OpenRet = true;
            string strConfiguration;
            bool blnInKissStartSequence = false;
            var strLine = default(string);
            bool blnOnAir1200 = false;
            bool blnOnAir9600 = false;
            strAPSFile = Globals.stcSelectedChannel.TNCConfigurationFile;
            if (File.Exists(strAPSFile) == false)
            {
                Globals.queChannelDisplay.Enqueue("R*** .aps Configuration file not found...");
                OpenRet = false;
            }
            else
            {
                try
                {
                    strConfiguration = File.ReadAllText(strAPSFile);
                    var objStringReader = new StringReader(strConfiguration);
                    do
                    {
                        try
                        {
                            strLine = objStringReader.ReadLine();
                            if (string.IsNullOrEmpty(strLine))
                                break;
                            var strCommand = strLine.Split(';');
                            if (!string.IsNullOrEmpty(strCommand[0].Trim()))
                            {
                                var strTokens = strCommand[0].Trim().Split(' ');
                                var switchExpr = strTokens[0].Trim().ToUpper();
                                switch (switchExpr)
                                {
                                    // These key words define the 1200 and  9600 baud segments
                                    case "BEGIN1200B":
                                        {
                                            blnOnAir1200 = true;
                                            blnOnAir9600 = false;
                                            break;
                                        }

                                    case "BEGIN9600B":
                                        {
                                            blnOnAir9600 = true;
                                            blnOnAir1200 = false;
                                            break;
                                        }

                                    case "END1200B":
                                        {
                                            blnOnAir9600 = false;
                                            blnOnAir1200 = false;
                                            if (Globals.stcSelectedChannel.TNCOnAirBaud == 1200)
                                                break;
                                            break;
                                        }

                                    case "END9600B":
                                        {
                                            blnOnAir9600 = false;
                                            blnOnAir1200 = false;
                                            if (Globals.stcSelectedChannel.TNCOnAirBaud == 9600)
                                                break;
                                            break;
                                        }
                                }

                                if (blnOnAir1200 & Globals.stcSelectedChannel.TNCOnAirBaud == 1200 | blnOnAir9600 & Globals.stcSelectedChannel.TNCOnAirBaud == 9600)
                                {
                                    var switchExpr1 = strTokens[0].Trim().ToUpper();
                                    switch (switchExpr1)
                                    {
                                        case "KISSSTART":
                                            {
                                                strKissStart = new string[0];
                                                blnInKissStartSequence = true;
                                                break;
                                            }

                                        case "KISSEND":
                                            {
                                                blnInKissStartSequence = false;
                                                break;
                                            }

                                        case "TXDELAY":
                                            {
                                                intTXDelay = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "PERSISTANCE":
                                            {
                                                intPersistance = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "SLOTTIME":
                                            {
                                                intSlottime = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "FULLDUPLEX":
                                            {
                                                blnFullDuplex = Convert.ToBoolean(strTokens[1]);
                                                break;
                                            }

                                        case "MAXFRAMESIZE":
                                            {
                                                intMaxFrameSize = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "MAXFRAMES":
                                            {
                                                intMaxFrames = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "ACKTIMER":
                                            {
                                                intACKTimer = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "MAXRETRY":
                                            {
                                                intMaxRetry = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "POLLTHRESH":
                                            {
                                                intPollThresh = Convert.ToInt32(strTokens[1]);
                                                break;
                                            }

                                        case "KISSEXIT":
                                            {
                                                blnExitKiss = Convert.ToBoolean(strTokens[1]);
                                                break;
                                            }

                                        case "ESCAPE":
                                            {
                                                strEscapeChars = strTokens[1].Trim();
                                                break;
                                            }

                                        default:
                                            {
                                                if (blnInKissStartSequence)
                                                {
                                                    Array.Resize(ref strKissStart, strKissStart.Length + 1);
                                                    strKissStart[strKissStart.Length - 1] = strCommand[0].Trim().ToUpper();
                                                }

                                                break;
                                            }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("[ModemNativeKiss.Open] " + strLine + Globals.CR + ex.Message);
                        }
                    }
                    while (true);
                }
                catch (Exception ex)
                {
                    Log.Error("[ModemNativeKiss.Open] " + ex.Message);
                }
            }

            if (objSerial != null)
            {
                objSerial.Close();
                Thread.Sleep(Globals.intComCloseTime);
                objSerial.Dispose();
                objSerial = null;
            }
            // Open the serial port...
            try
            {
                objSerial = new SerialPort();
                objSerial.PortName = Globals.stcSelectedChannel.TNCSerialPort;
                objSerial.ReceivedBytesThreshold = 1;
                objSerial.BaudRate = Convert.ToInt32(Globals.stcSelectedChannel.TNCBaudRate);
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
                    Globals.queChannelDisplay.Enqueue("R*** Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort + ". Port may be in use by another application.");
                    Log.Error("[ClinetNativeKISS.Open] Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort);
                    return false;
                }

                if (objSerial.IsOpen == false)
                {
                    Globals.queChannelDisplay.Enqueue("R*** Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort + ". Port may be in use by another application.");
                    Log.Error("[ModemNativeKiss.Open] Failed to open serial port on " + Globals.stcSelectedChannel.TNCSerialPort);
                    return false;
                }
                else
                {
                    Globals.queChannelDisplay.Enqueue("G*** Serial port " + Globals.stcSelectedChannel.TNCSerialPort + " opened");
                }
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.Open] " + ex.Message);
                return false;
            }

            OpenRet = InitializeKISSTnc(); // initialize the TNC to KISS mode

            if (OpenRet == false)
            {
                Log.Error("[ClientNativeKiss.Open] Failed to Initialize KISS TNC");
                enmState = LinkStates.LinkFailed;

                if (objSerial != null)
                {
                    objSerial.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    objSerial.Dispose();
                    objSerial = null;
                }

                return false;
            }
            else
            {
                // All OK so set up Peter's Native KISS DLL
                try
                {
                    objKissComPort = new COMPort(objSerial); // Globals.stcSelectedChannel.TNCSerialPort, Convert.ToInt32(Globals.stcSelectedChannel.TNCBaudRate));
                    // set up the optional escape characters
                    if (!string.IsNullOrEmpty(strEscapeChars))
                        objKissComPort.escapedCharList = strEscapeChars; // Escape characters from .aps file
                    objKissChannel = objKissComPort.CreateChannel(0); // Create TNC Channel 0
                    objKissDLProvider = objKissChannel.CreateProvider(Globals.SiteCallsign); // Create a DLProvider for the Site Call sign 
                                                                                             // set up the KISS parameters from default values or as updated from the .aps file
                    objConParam = new Connection.ConnectionParameterBuf();
                    objConParam.ackTimer = 1000 * intACKTimer; // convert from Sec to ms.
                    objConParam.maxRetry = intMaxRetry;
                    objConParam.maxIFrame = intMaxFrameSize;
                    objConParam.maxWindowSize = intMaxFrames;
                    objConParam.pollThresh = 1000 * intPollThresh; // convert from Sec to ms
                    objKissChannel.SetPersistence(intPersistance);
                    objKissChannel.SetSlotTime(intSlottime);
                    objKissChannel.SetTXDelay(intTXDelay);
                    objKissChannel.SetTXFullDuplex(Convert.ToInt32(blnFullDuplex));
                    enmState = LinkStates.Initialized;
                }
                catch (Exception ex)
                {
                    Log.Error("[ClientNativeKiss.Open] Call to DLL Error: " + ex.ToString());
                    return false;
                }
            }
            // Extended logging added for debugging
            if (File.Exists(Globals.SiteRootDirectory + @"Log\NativeKISSax25PktLog.log"))
            {
                File.Delete(Globals.SiteRootDirectory + @"Log\NativeKISSax25PktLog.log");
            }

            Support.pktLogFile = Globals.SiteRootDirectory + @"Log\NativeKISSax25PktLog.log";
            objKissChannel.packetMonitorEnable = TNCChannel.PacketMonitorType.LogFile;
            return OpenRet;
        } // Open

        // Function to initialize the TNC and set to KISS mode
        private bool InitializeKISSTnc()
        {
            try
            {
                // send exit KISS sequence in case TNC was left in KISS mode
                objSerial.Write(bytExitKiss, 0, bytExitKiss.Length);
                Thread.Sleep(200);
                // Send return to get cmd: prompt
                objSerial.Write(Globals.CR);
                Thread.Sleep(200);
                var switchExpr = Globals.stcSelectedChannel.TNCType;
                switch (switchExpr)
                {
                    case "TM-D710 int":
                    case "TM-D72": // TODO: this needs verification or correction for the D700
                        {
                            // if TM-Dxx Kenwood internal use TC sequence
                            objSerial.Write("TC 1" + Globals.CR); // force the interface to Display unit
                            Thread.Sleep(100);
                            int intHz = Globals.KHzToHz(Globals.stcSelectedChannel.RDOCenterFrequency);
                            // TODO: These codes need verification for D700,  OK for D710
                            if (intHz <= 199000000) // A Band
                            {
                                objSerial.Write("TN 2,0" + Globals.CR); // Enable the TNC to use A band and go to TNC interface
                            }
                            else
                            {
                                objSerial.Write("TN 2,1" + Globals.CR);
                            } // Enable the TNC to use B band and go to TNC interface

                            break;
                        }

                    case "TM-D700 int":
                    case "TH-D7 int":
                        {
                            objSerial.Write("TC 1" + Globals.CR); // force the interface to Display unit
                            Thread.Sleep(100);
                            objSerial.Write("TNC 2" + Globals.CR); // select the TNC Packet mode
                            Thread.Sleep(100);
                            int intHz = Globals.KHzToHz(Globals.stcSelectedChannel.RDOCenterFrequency);
                            // TODO: These codes need verification for D700, D7
                            if (intHz <= 199000000) // A Band
                            {
                                objSerial.Write("BC 0,0" + Globals.CR); // Set A band as asctive band
                                objSerial.Write("DTB 0" + Globals.CR); // Enable the TNC to use A band
                            }
                            else
                            {
                                objSerial.Write("BC 1,1" + Globals.CR); // Set B band as asctive band
                                objSerial.Write("DTB 1" + Globals.CR);
                            } // Enable the TNC to use B band

                            break;
                        }
                }

                Thread.Sleep(500);
                queTNCData.Clear();
                strCommandReply = "";
                objSerial.Write(Globals.CR);

                // Send KISSSTART sequence
                foreach (string cmd in strKissStart)
                {
                    if (cmd.IndexOf("HBAUD ") != -1)
                    {
                        strBaudRate = cmd.Substring(cmd.IndexOf("HBAUD ") + 5).Trim() + " baud";
                    }

                    objSerial.Write(cmd + Globals.CR);
                    Thread.Sleep(200);
                }

                Globals.queChannelDisplay.Enqueue("G*** KISS TNC " + Globals.stcSelectedChannel.TNCType + " opened");
                // Parse the connection script, if any, into the ConnectScript string array...
                if (!string.IsNullOrEmpty(Globals.stcSelectedChannel.TNCScript))
                {
                    ConnectScript = Globals.stcSelectedChannel.TNCScript.Replace(Globals.LF, "").Split(Convert.ToChar(Globals.CR));
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.InitializeKISSTnc] " + ex.Message);
                return false;
            }
        }

        public bool Close()
        {
            // Subroutine to close the channel and put TNC into known state...
            // Always call this method before the instance goes out of scope
            if (blnClosed)
                return true;
            try
            {
                if (Globals.blnManualAbort == true)
                {
                    enmState = LinkStates.Disconnected;
                }

                blnClosed = true;
                Globals.queChannelDisplay.Enqueue("G*** Closing Packet Channel " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
                if (Globals.objRadioControl != null) // Shut down the radio control and free the serial port
                {
                    Globals.objRadioControl.Close();
                    Thread.Sleep(Globals.intComCloseTime);
                    Globals.objRadioControl = null;
                }

                if (objProtocol is object)
                    objProtocol.CloseProtocol();
                var dttTimeout = DateTime.Now;
                dttTimeout = DateTime.Now;
                while (!(enmState == LinkStates.Disconnected | enmState == LinkStates.LinkFailed))
                {
                    if (dttTimeout.AddSeconds(10) < DateTime.Now)
                        break;
                }

                if (objCon != null)
                {
                    objCon.Close();
                }

                if (objKissDLProvider != null)
                {
                    objKissDLProvider.RemoveConnection(objCon);
                    objKissDLProvider.Close();
                }

                if (blnExitKiss & objKissChannel != null)
                {
                    objKissChannel.SetKissModeOff();
                    Thread.Sleep(200);
                }

                if (objKissChannel != null)
                    objKissChannel.Close();
                if (objKissComPort != null)
                    objKissComPort.Close();
                Globals.queStatusDisplay.Enqueue("Idle");
                Globals.queRateDisplay.Enqueue("------");
                Globals.blnChannelActive = false;
                Globals.ObjSelectedModem = null;
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.Close]: " + ex.ToString());
            }

            return default;
        } // Close 

        public void Abort()
        {
            try
            {
                Globals.queChannelDisplay.Enqueue("R*** Aborting channel " + Globals.stcSelectedChannel.ChannelName);
                if (objCon != null)
                {
                    objCon.DisconnectNoWait();
                    Thread.Sleep(1000);
                    objCon.DisconnectNoWait();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.Abort] : " + ex.ToString());
            }

            enmState = LinkStates.LinkFailed;
            Close();
        } // Abort 

        public bool Connect(bool blnAutomatic)
        {
            // Handles new connection...
            // startup the appropriate threads in the Native KISS DLL
            if (blnClosed)
                return false;
            string strVia = "";
            string strTarget = Globals.stcSelectedChannel.RemoteCallsign;
            if (enmState != LinkStates.Initialized)
                return false;
            try
            {
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
                        else
                        {
                            Globals.queChannelDisplay.Enqueue("R*** Failure setting Radio control for radio " + Globals.stcSelectedChannel.RDOModel);
                            return false;
                        }

                        if (!Globals.objRadioControl.InitializeSerialPort(ref Globals.stcSelectedChannel))
                        {
                            Globals.queChannelDisplay.Enqueue("R*** Failure initializing Radio Control");
                            Log.Error("[ModemNativeKiss.Connect A] Failure initializing Radio Control");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[ClientNativeKiss.Connect B] " + ex.Message);
            }

            try
            {
                if (Globals.objRadioControl != null)
                    Globals.objRadioControl.SetParameters(ref Globals.stcSelectedChannel);
                if (ConnectScript != null && ConnectScript.Length > 0)
                {
                    if (StartScript(ref strVia, ref strTarget)) // Activates scripting, modifies sVia and sTarget
                    {
                        Globals.stcSelectedChannel.RemoteCallsign = strTarget;
                        if (string.IsNullOrEmpty(strVia))
                        {
                            enmState = LinkStates.Connecting;
                            objCon = objKissDLProvider.CreateConnection(objConParam);
                            objCon.ConnectNoWait(Globals.stcSelectedChannel.RemoteCallsign);
                        }
                        else
                        {
                            var aryVias = strVia.Split(',');
                            var switchExpr = aryVias.Length;
                            switch (switchExpr)
                            {
                                case 1:
                                    {
                                        enmState = LinkStates.Connecting;
                                        objCon = objKissDLProvider.CreateConnection(objConParam);
                                        objCon.ConnectNoWait(Globals.stcSelectedChannel.RemoteCallsign, aryVias[0].Trim());
                                        break;
                                    }

                                case 2:
                                    {
                                        enmState = LinkStates.Connecting;
                                        objCon = objKissDLProvider.CreateConnection(objConParam);
                                        objCon.ConnectNoWait(Globals.stcSelectedChannel.RemoteCallsign, aryVias[0].Trim(), aryVias[1].Trim());
                                        break;
                                    }

                                default:
                                    {
                                        Globals.queChannelDisplay.Enqueue("R*** Script Error - Max of 2 vias using Native KISS!");
                                        enmState = LinkStates.LinkFailed;
                                        return false;
                                    }
                            }
                        }
                        // Start connection timing
                        dttStartConnect = DateTime.Now;
                    }
                    else
                    {
                        Globals.queChannelDisplay.Enqueue("R*** Script Error - ending connection");
                        enmState = LinkStates.LinkFailed;
                        return false;
                    }
                }
                else
                {
                    // start Connection Timing...
                    dttStartConnect = DateTime.Now;
                    Globals.queRateDisplay.Enqueue("Linking");
                    Globals.queChannelDisplay.Enqueue("G*** Starting Packet connection to " + Globals.stcSelectedChannel.RemoteCallsign + " on port " + Globals.stcSelectedChannel.TNCPort.ToString());
                    // Start KISS connection here
                    objCon = objKissDLProvider.CreateConnection(objConParam);
                    objCon.ConnectNoWait(Globals.stcSelectedChannel.RemoteCallsign);
                    enmState = LinkStates.Connecting;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.Connect C] " + ex.Message);
                enmState = LinkStates.LinkFailed;
                return false;
            }
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
            if (blnClosed)
                return;
            try
            {
                if (bytData.Length > 0 & objCon != null)
                {
                    if (objCon.connectState == Connection.ConnectionState.Connected)
                    {
                        dttLastActivity = DateTime.Now;
                        objCon.Send(bytData);
                        intBytesConfirmedSent += 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.DataToSend(Byte())]: " + ex.ToString());
            }
        } // DataToSend (bytes) 

        public void Disconnect()
        {
            try
            {
                if (objCon != null)
                    objCon.DisconnectNoWait();
            }
            catch (Exception ex)
            {
                Log.Error("[ModemNativeKiss.Disconnect] :  " + ex.ToString());
            }
        } // Disconnect 

        private int intTicks = 0;
        private TNCKissInterface.Connection.ConnectionState enmKissConState = TNCKissInterface.Connection.ConnectionState.Disconnected;

        // Sub to poll status, bytes received, and timeouts
        public void Poll()
        {
            if (blnClosed)
                return;
            intTicks += 1;
            if (intTicks >= 10)
            {
                intTicks = 0;
            }

            if (objCon == null | blnClosed)
                return;
            if (intTicks == 0)
            {
                // poll for status
                if (enmKissConState != objCon.connectState)
                {
                    enmKissConState = objCon.connectState; // ' Get unsolicited status reports...
                    switch (enmKissConState)
                    {
                        case Connection.ConnectionState.Connected:
                            {
                                if (enmState != LinkStates.Connected & objProtocol == null)
                                {
                                    Globals.queChannelDisplay.Enqueue("P*** " + "CONNECTED");
                                    Globals.queRateDisplay.Enqueue(strBaudRate);
                                    enmState = LinkStates.Connected;
                                    objProtocol = new ProtocolInitial(this, ref Globals.stcSelectedChannel);
                                }

                                break;
                            }

                        case Connection.ConnectionState.ConnectPending:
                            {
                                Globals.queChannelDisplay.Enqueue("P*** " + "CONNECT PENDING");
                                break;
                            }

                        case Connection.ConnectionState.Disconnected:
                            {
                                if (enmState == LinkStates.Connecting)
                                {
                                    Globals.queChannelDisplay.Enqueue("P*** " + "DISCONNECTED");
                                    Globals.queChannelDisplay.Enqueue("R*** " + "Connect failure with " + Globals.stcSelectedChannel.RemoteCallsign);
                                    enmState = LinkStates.LinkFailed;
                                }
                                else if (!(enmState == LinkStates.Disconnected | enmState == LinkStates.LinkFailed))
                                {
                                    Globals.queChannelDisplay.Enqueue("P*** " + "DISCONNECTED");
                                    enmState = LinkStates.Disconnected;
                                    return;
                                }

                                break;
                            }

                        case Connection.ConnectionState.DisconnectPending:
                            {
                                Globals.queChannelDisplay.Enqueue("P*** " + "DISCONNECT PENDING");
                                break;
                            }
                    }
                }
                // check for timeouts approx once per second
                if (blnInScript & DateTime.Now.Subtract(dttLastScript).TotalSeconds > Globals.stcSelectedChannel.TNCScriptTimeout)
                {
                    Globals.queChannelDisplay.Enqueue("R*** Script Timeout...aborting script");
                    Abort();
                }
                else if (enmState == LinkStates.Connecting & DateTime.Now.Subtract(dttStartConnect).TotalSeconds > 60)
                {
                    Globals.queChannelDisplay.Enqueue("R*** 60 second connect timeout...");
                    Abort();
                }
            }

            if (objCon.isConnected & enmState == LinkStates.Connected & !blnClosed)
            {
                try
                {
                    // Check for received data...
                    var bytRcvBuff = new byte[8192];
                    int intBytesAvail;
                    intBytesAvail = objCon.Recv(bytRcvBuff, false);
                    if (intBytesAvail > 0)
                    {
                        Array.Resize(ref bytRcvBuff, intBytesAvail);
                        dttLastActivity = DateTime.Now;
                        if (blnInScript)
                        {
                            // Display received data immediately and post data to the script response string...
                            string strResponse = Globals.GetString(bytRcvBuff);
                            Globals.queChannelDisplay.Enqueue("X" + strResponse);
                            strScriptResponse += strResponse.ToUpper();
                            dttLastScript = DateTime.Now;
                            SequenceScript();
                        }
                        else if (objProtocol is object)
                        {
                            // Send data to the protocol handler...
                            objProtocol.ChannelInput(ref bytRcvBuff);
                        }
                    }

                    if (intTicks == 0) // this is done only approx once per second
                    {
                        // poll for confirmed data sent to update progress bar
                        if (intBytesConfirmedSent != objCon.bytesSent)
                        {
                            int intNewBytes = objCon.bytesSent - intBytesConfirmedSent;
                            intBytesConfirmedSent = objCon.bytesSent;
                            if (intNewBytes > 0)
                            {
                                Globals.UpdateProgressBar(intNewBytes);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("[ModemNativeKiss.Poll] : " + ex.ToString());
                }
            }
        } // Poll 

        // Function to sequence the connect script...
        private bool SequenceScript()
        {
            if (intConnectScriptPtr > ConnectScript.Length - 1)
            {
                blnInScript = false;
                Globals.queChannelDisplay.Enqueue(Globals.CR);
                Globals.queChannelDisplay.Enqueue("G #End of Script File...");
                if (objProtocol != null)
                    objProtocol.ChannelInput(strScriptResponse);
                return true;
            }

            try
            {
                int intPtr = strScriptResponse.IndexOf(ConnectScript[intConnectScriptPtr]);
                if (intPtr == -1)
                    return false;
                Globals.queChannelDisplay.Enqueue(Globals.CR);
                Globals.queChannelDisplay.Enqueue("G     #Script(" + (1 + intConnectScriptPtr).ToString() + "):" + ConnectScript[intConnectScriptPtr]);
                if (ConnectScript.Length == intConnectScriptPtr + 1)
                {
                    Globals.queChannelDisplay.Enqueue("G #End Script");
                    if (objProtocol != null)
                        objProtocol.ChannelInput(strScriptResponse);
                    strScriptResponse = "";
                    blnInScript = false;
                    return true;
                }
                else if (ConnectScript.Length < intConnectScriptPtr + 1)
                {
                    Globals.queChannelDisplay.Enqueue("G #End of Script File...");
                    if (objProtocol != null)
                        objProtocol.ChannelInput(strScriptResponse);
                    strScriptResponse = "";
                    blnInScript = false;
                    return true;
                }

                strScriptResponse = strScriptResponse.Substring(intPtr + ConnectScript[intConnectScriptPtr].Length);
                Globals.queChannelDisplay.Enqueue("G     #Script(" + (2 + intConnectScriptPtr).ToString() + "):" + ConnectScript[intConnectScriptPtr + 1]);
                DataToSend(ConnectScript[intConnectScriptPtr + 1] + Globals.CR);
                intConnectScriptPtr += 2;
                if (intConnectScriptPtr > ConnectScript.Length - 1)
                {
                    blnInScript = false;
                    Globals.queChannelDisplay.Enqueue("G #End of Script File...");
                    if (objProtocol != null)
                        objProtocol.ChannelInput(strScriptResponse);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("[ClientNativeKiss.SequenceScript] Err: " + ex.Message);
                Globals.queChannelDisplay.Enqueue("R*** Connection script error");
                return false;
            }

            return default;
        } // ConnectionScript

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

        // Function to start the connection script
        private bool StartScript(ref string strVia, ref string strTarget)
        {
            bool StartScriptRet = default;
            // Starts the connection script. Returns True if OK False if script or processing error...

            try
            {
                StartScriptRet = true;
                strVia = "";
                if (ConnectScript == null)
                {
                    intConnectScriptPtr = -1; // This sets the script pointer to signal inactive
                    blnInScript = false;
                    return true; // No script. Do not update strTarget.
                }
                else
                {
                    int intPt;
                    string strTemp;
                    string strTok;
                    dttLastScript = DateTime.Now; // initialize the script timing
                    strTemp = " " + ConnectScript[0].ToUpper().Trim();

                    // This strips off any leading V or Via (case insensitive) and skips over any syntax "Connect via"
                    intPt = strTemp.IndexOf(" V ");
                    if (intPt != -1)
                    {
                        strVia = " " + strTemp.Substring(intPt + 2).Trim();
                    }
                    else
                    {
                        intPt = strTemp.IndexOf(" VIA ");
                        if (intPt != -1)
                        {
                            strVia = strTemp.Substring(intPt + 4).Trim();
                        }
                    }

                    if (ConnectScript.Length == 1)
                    {
                        Globals.queChannelDisplay.Enqueue("G*** Requesting connection to " + Globals.stcSelectedChannel.RemoteCallsign + " via " + strVia + " at " + Globals.Timestamp());
                        // Simple via connect, just a single line (not a true script)
                        intConnectScriptPtr = -1; // Set script pointer to inactive, strVia is updated, strTarget is unchanged
                        blnInScript = false;
                        return true;
                    }
                    else
                    {
                        // True script processing here (indicated by at least two Connection script lines)
                        intConnectScriptPtr = 1; // Initialize ptr to first script line response (also signals the script is active)
                        strScriptResponse = ""; // clear the script response string on start 
                        blnInScript = true;
                        dttLastActivity = DateTime.Now;
                        dttLastScript = DateTime.Now;
                        strTok = GetConnectTarget(ConnectScript[0]);
                        if (!string.IsNullOrEmpty(strTok))
                        {
                            strTarget = strTok; // sTarget is updated 
                            Globals.queChannelDisplay.Enqueue("G #Begin Connection Script");
                            Globals.queChannelDisplay.Enqueue("G     #Script(1): " + ConnectScript[0]);
                        }
                        else
                        {
                            StartScriptRet = false;
                            blnInScript = false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return StartScriptRet;
        } // RunScript

        private string GetConnectTarget(string Script)
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
                if (intPt != -1)
                {
                    strTemp = strTemp.Substring(intPt + 7).Trim();
                }
                else
                {
                    return "";
                }
            }

            return strTemp.Split(' ')[0].Trim();
        } // GetConnectTarget

        private void objSerial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int intBytesToRead = objSerial.BytesToRead;
                var bytBuffer = new byte[intBytesToRead];
                objSerial.Read(bytBuffer, 0, intBytesToRead);
                queTNCData.Enqueue(bytBuffer);
            }
            catch (Exception ex)
            {
                Log.Error("[ClientNativeKiss.objSerial_DataReceived] Err: " + ex.Message);
            }
        }
    }
}