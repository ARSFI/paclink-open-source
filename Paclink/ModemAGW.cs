using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{
    public class ModemAGW : IModem
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private DialogAgwEngineViewModel _dialogAgwEngine = new DialogAgwEngineViewModel();

        public ModemAGW()
        {
            Globals.blnChannelActive = true;
        } 

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private ProtocolInitial objProtocol;
        private DateTime dttStartDisconnect;
        private ConnectedCalls ConnectedCall; // holds the connected call structure
        private TcpClient _objTCPPort;

        private TcpClient objTCPPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objTCPPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _objTCPPort = value;
            }
        }

        private ASCIIEncoding objASCIIEncoding;
        private string[] ConnectScript = null;
        private bool blnLoggedIn;
        private bool blnBeaconsOK;
        private bool blnDisconnectPending;
        private bool blnPollEnabled;
        private bool blnNormalDisconnect;
        private int intConnectScriptPtr = -1; // A pointer to the current connect script line, -1 if inactive
        private int intScriptTimer = -1;
        private byte[] bytDataBuffer = new byte[0];
        private int intConnectTimer = -1;
        private int intTicks; // Holds the time ticks count 1 tick = 1 second
        private int intAGWPort;
        private string strVersion = "";

        private struct ConnectedCalls
        {
            public string Callsign; // callsign of the connected call
            public int Port;  // Port number of the connected call
            public string Stream; // Stream used for reporting "A-J"
            public int ActivityTimer;  // Holds count of seconds since last activity (send or receive) 
            public int FramesOutstanding; // Holds count of Frames outstanding for THIS connection 
            public int WaitDisconnect; // holds the number of seconds been waiting for disconnect
        }

        private List<OutboundAGWPacket> cllOutboundQueue = new List<OutboundAGWPacket>();

        private struct OutboundAGWPacket
        {
            public byte[] Data; // the completed packet with tcp frame formatting
            public int Port; // the identifier for the port
                             // Public ConID As Integer ' the ID 
            public bool IsDisconnect; // flags a disconnect packet
        }

        // Enums and Structures
        private LinkStates enmState = LinkStates.Undefined;
        private ConnectionOrigin enmConnectionStatus;

        // Queues
        private Queue quePortInput = Queue.Synchronized(new Queue());
        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
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
                return blnNormalDisconnect;
            }

            set
            {
                blnNormalDisconnect = value;
            }
        } // NormalDisconnect

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // Channel poll called from Main an about 100 millisecond intervals...

        private int intCalls = 0;

        // Process the poll every 10 calls to Poll()...
        public void Poll()
        {
            // Channel poll called from Main an about 100 millisecond intervals...
            intCalls += 1;
            if (intCalls >= 10)
            {
                intCalls = 0;
            }
            else
            {
                return;
            }

            // Process any input from TCP port
            ProcessPortInput();

            // Poll channel activity at about one second intervals...
            if (blnPollEnabled)
            {
                intTicks += 1;
                if (intConnectScriptPtr != -1)
                    intScriptTimer += 1;
                if (intConnectTimer != -1)
                    intConnectTimer += 1;
                try
                {
                    // Go throuth the outbound queue sending packets when # of outstanding packets < max outstanding
                    if (cllOutboundQueue.Count > 0)
                    {
                        int i = 0;
                        foreach (OutboundAGWPacket objQ in cllOutboundQueue)
                        {
                            if (objQ.IsDisconnect && ConnectedCall.FramesOutstanding == 0)
                            {
                                if (ConnectedCall.WaitDisconnect >= 2)
                                {
                                    try
                                    {
                                        objTCPPort.GetStream().Write(objQ.Data, 0, objQ.Data.Length);
                                    }
                                    catch
                                    {
                                    }

                                    cllOutboundQueue.RemoveAt(i); // when removed at i don't increment i...i now points to the next item in the collection
                                }
                                else
                                {
                                    ConnectedCall.WaitDisconnect += 1;
                                    i += 1;
                                } // advance to next packet in queue
                            }
                            else if (ConnectedCall.FramesOutstanding < Math.Max(1, Globals.stcSelectedChannel.AGWMaxFrames) && !objQ.IsDisconnect)
                            {
                                try
                                {
                                    objTCPPort.GetStream().Write(objQ.Data, 0, objQ.Data.Length);
                                    Globals.UpdateProgressBar(objQ.Data.Length - 36);
                                }
                                catch
                                {
                                }
                                // reset the activity timer for this connected call
                                ConnectedCall.ActivityTimer = 0;
                                ConnectedCall.WaitDisconnect = 0;
                                ConnectedCall.FramesOutstanding += 1;
                                cllOutboundQueue.RemoveAt(i); // When removed at i don't increment i...i now points to the next item in the collection
                            }
                            else
                            {
                                i += 1;
                            } // Nothing removed so increment i
                        }
                    }
                    // Increment and test activity timers
                    if (intConnectTimer > Globals.stcSelectedChannel.AGWScriptTimeout)
                    {
                        Globals.queChannelDisplay.Enqueue("R*** " + Globals.stcSelectedChannel.AGWScriptTimeout.ToString() + " second connection timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC"));
                        DirtyDisconnect();
                        enmState = LinkStates.LinkFailed;
                    }

                    ConnectedCall.ActivityTimer += 1;
                    if (ConnectedCall.ActivityTimer > 60 * Globals.stcSelectedChannel.AGWTimeout)
                    {
                        Globals.queChannelDisplay.Enqueue("R*** " + Globals.stcSelectedChannel.AGWTimeout.ToString() + " minute activity timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC"));
                        DirtyDisconnect();
                        enmState = LinkStates.LinkFailed;
                    }

                    if (intScriptTimer > Globals.stcSelectedChannel.AGWScriptTimeout)
                    {
                        Globals.queChannelDisplay.Enqueue("GConnect Script Timeout at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm UTC") + " # ... Disconnecting");
                        DirtyDisconnect();
                        enmState = LinkStates.LinkFailed;
                    }

                    if (blnDisconnectPending & DateTime.Now.Subtract(dttStartDisconnect).TotalSeconds > 30)
                    {
                        Disconnect();
                    }

                    AGWRequestFramesOutstanding(); // Request frames outstanding 
                }
                catch (Exception e)
                {
                    _log.Error("[ModemAGW.Poll] " + e.Message);
                }
            }
        }

        // Used to release the AGW resource by disconnecting
        private void Release()
        {
            blnPollEnabled = false;
            try
            {
                AGWUnRegisterCallsign(Globals.SiteCallsign);
                Thread.Sleep(1000);
                ProcessPortInput();
            }
            catch (Exception e)
            {
                UserInterfaceFactory.GetUiSystem().DisplayModalError("[Release] " + e.Message, "ModemAGW");
            }

            try
            {
                objTCPPort.Close();
                Thread.Sleep(200);
            }
            catch
            {
            }

            objTCPPort.Dispose();
        } // Release

        // Function to send radio command via PTCII (not used in this class)
        public void SendRadioCommand(byte[] bytCommand)
        {
        }

        // Function to send radio command via PTCII (not used in this class)
        public void SendRadioCommand(string strCommand)
        {
        }

        public void Abort()
        {
            if (enmState != LinkStates.Connected)
            {
                DirtyDisconnect();
                return;
            }

            Disconnect();
            enmState = LinkStates.LinkFailed;
        } // Abort *

        private bool Initialize()
        {
            intAGWPort = Convert.ToInt32(Globals.stcSelectedChannel.AGWPort.Substring(0, Globals.stcSelectedChannel.AGWPort.IndexOf(":")));
            blnDisconnectPending = false;
            if (!string.IsNullOrEmpty(Globals.stcSelectedChannel.AGWScript))
            {
                ConnectScript = Globals.stcSelectedChannel.AGWScript.Replace(Globals.LF, "").Split(Convert.ToChar(Globals.CR));
            }

            try
            {
                blnLoggedIn = false;
                blnPollEnabled = false;
                cllOutboundQueue = null;
                cllOutboundQueue = new List<OutboundAGWPacket>();
                ConnectedCall = default;
                ConnectedCall = new ConnectedCalls();

                if (objTCPPort != null)
                {
                    try
                    {
                        objTCPPort.Close();
                        var dttDisconStart = DateTime.Now;
                        while (DateTime.Now.Subtract(dttDisconStart).TotalSeconds < 10 & objTCPPort.Connected)
                            Thread.Sleep(100);
                    }
                    catch
                    {
                    }
                }

                objTCPPort = new TcpClient();

                objASCIIEncoding = new ASCIIEncoding();
                if (_dialogAgwEngine.AgwLocation == 1)
                {
                    // add start up of packet engine and login
                    PacketEngineRun();
                }

                bytDataBuffer = new byte[0]; // null out the data buffer
                Globals.queChannelDisplay.Enqueue("R*** Connecting to Packet Engine");
                //objTCPPort.ReceiveTimeout = 30;
                //objTCPPort.KeepAlive = true;
                objTCPPort.ConnectAsync(_dialogAgwEngine.AgwHost, _dialogAgwEngine.AgwTcpPort).ContinueWith(t =>
                {
                    OnConnected(objTCPPort);

                    byte[] buffer = new byte[1024];

                    Task<int> task = null;
                    try
                    {
                        task = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                        task.ContinueWith(t =>
                        {
                            if (t.Exception == null)
                            {
                                OnDataIn(buffer, t.Result);
                            }
                            else
                            {
                                OnError(t.Exception);
                            }
                        });
                        task.Wait(0);
                    }
                    catch (Exception e)
                    {
                        OnError(task.Exception);
                    }
                });
                var dttStart = DateTime.Now;
                while (DateTime.Now.Subtract(dttStart).TotalSeconds < 10 & !blnLoggedIn)
                {
                    Thread.Sleep(200);
                    ProcessPortInput();
                }

                blnPollEnabled = blnLoggedIn;
                return blnLoggedIn;
            }
            catch (Exception e)
            {
                _log.Error("[ModemAGW.Initialize] " + e.Message);
                return false;
            }
        }  // Initialize

        // Sub to Send String Data 
        public void DataToSend(string sData)
        {
            var objEncoder = new ASCIIEncoding();
            DataToSend(Globals.GetBytes(sData));
        } // DataToSend

        // Sub to Send byte Data or add data to the outbound Queue
        public void DataToSend(byte[] Data)
        {
            if (!blnLoggedIn)
                return;
            if (Data.Length == 0)
            {
                _log.Error("[Client.DataToSend] Data is empty array");
                return;
            }

            try
            {

                // Provide framing to Packet length and buffering for queued packets.
                MemoryStream ms = new MemoryStream(Data);
                while (ms.Position < ms.Length)
                {
                    var packet = new byte[Globals.stcSelectedChannel.AGWPacketLength];
                    var amountRead = ms.Read(packet, 0, Globals.stcSelectedChannel.AGWPacketLength);
                    if (amountRead < Globals.stcSelectedChannel.AGWPacketLength)
                    {
                        var tempBuffer = new byte[amountRead];
                        Array.Copy(packet, tempBuffer, amountRead);
                        packet = tempBuffer;
                    }

                    MemoryStream outputPacket = new MemoryStream();
                    outputPacket.Write(new byte[] { Convert.ToByte(ConnectedCall.Port - 1) });
                    outputPacket.Write(new byte[] { 0x00, 0x00, 0x00, 0x44 }); // 3 zeroes followed by 'D'.
                    outputPacket.Write(new byte[] { 0x00, 0xF0, 0x00 }); // AX.25 Information (0xF0) as we used 'C' to start the connection

                    var siteCallAsBytes = new byte[10];
                    objASCIIEncoding.GetBytes(Globals.SiteCallsign, 0, Math.Min(9, Globals.SiteCallsign.Length), siteCallAsBytes, 0);
                    outputPacket.Write(siteCallAsBytes);

                    var connectedCallAsBytes = new byte[10];
                    objASCIIEncoding.GetBytes(ConnectedCall.Callsign, 0, Math.Min(9, ConnectedCall.Callsign.Length), connectedCallAsBytes, 0);
                    outputPacket.Write(connectedCallAsBytes);

                    outputPacket.Write(ComputeLengthB(packet.Length));
                    outputPacket.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); // user field, not used.
                    outputPacket.Write(packet);

                    // Always Queue ....changed from before
                    var objQ = new OutboundAGWPacket();
                    objQ.Data = outputPacket.ToArray();
                    objQ.Port = ConnectedCall.Port;
                    objQ.IsDisconnect = false;
                    cllOutboundQueue.Add(objQ); // add the packet to the Outbound Queue
                }
            }
            catch (Exception e)
            {
                _log.Error("[ModemAGW.DataToSend] " + e.Message);
            }
        } // DataToSend

        // This creates a disconnect frame with a timestamp. The timestamp is used to delay sending the frame for at least 10 sec
        public void Disconnect()
        {
            try
            {
                var bytTemp = new byte[36];
                if (!blnLoggedIn | string.IsNullOrEmpty(ConnectedCall.Callsign))
                {
                    enmState = LinkStates.LinkFailed;
                    return;
                }

                if (!blnNormalDisconnect | intConnectScriptPtr != -1)
                {
                    DirtyDisconnect(); // use a dirty disconnect if partway through a script or Disconnect is pending
                    enmState = LinkStates.LinkFailed;
                }
                else // normal disconnect
                {
                    bytTemp[0] = Convert.ToByte(ConnectedCall.Port - 1);
                    bytTemp[6] = 0xF0; // AX.25 Information
                    objASCIIEncoding.GetBytes("d", 0, 1, bytTemp, 4);
                    objASCIIEncoding.GetBytes(Globals.SiteCallsign, 0, Globals.SiteCallsign.Length, bytTemp, 8);
                    objASCIIEncoding.GetBytes(ConnectedCall.Callsign, 0, ConnectedCall.Callsign.Length, bytTemp, 18);
                    var objD = new OutboundAGWPacket();
                    objD.Data = bytTemp;
                    objD.Port = ConnectedCall.Port;
                    objD.IsDisconnect = true;
                    dttStartDisconnect = DateTime.Now;
                    cllOutboundQueue.Add(objD); // add the packet to the Outbound Queue collection
                    blnDisconnectPending = true;
                    blnPollEnabled = true;
                }
            }
            catch (Exception e)
            {
                _log.Error("[ModemAGW.NormalDisconnect] " + e.Message);
            }
        } // NormalDisconnect

        // Function to compute Length value and return as 4 byte array for header
        private byte[] ComputeLengthB(long lLength)
        {
            byte[] ComputeLengthBRet = default;
            // Compute the 4 byte array based on the length
            // order is Lsbyte (0) to Msbyte (3)
            var bLength = new byte[4];
            int n;
            long lLen;
            long lQ;
            lLen = lLength;
            for (n = 1; n <= 4; n++)
            {
                lQ = lLen / Convert.ToInt32(Math.Pow(256, 4 - n)); // note integer divide 
                lLen = lLen - Convert.ToInt32(lQ * Math.Pow(256, 4 - n));
                bLength[4 - n] = Convert.ToByte(lQ);
            }

            ComputeLengthBRet = bLength;
            return ComputeLengthBRet;
        } // ComputeLengthB

        // Function to compute length based on 4 byte value in header
        private int ComputeLengthL(byte[] Buffer, int Index)
        {
            int ComputeLengthLRet = default;
            // Compute the length based on the 4 byte value LSB to MSB
            // normally index = 28 for data length
            int length;
            int n;
            length = 0;
            for (n = 0; n <= 3; n++)
                length = length + (int)(Buffer[n + Index] * Math.Pow(256, n));
            if (length > 1000)
                _log.Error("[ComputeLengthL] Length: " + length.ToString());
            ComputeLengthLRet = length;
            return ComputeLengthLRet;
        }  // ComputeLengthL

        // Function to Extract the Frame from the Buffer
        private int ExtractFrame(ref byte[] binBuffer, ref byte[] binFrame)
        {
            int intDataLength;
            // The following computes the 32 bit count from the Data length field  
            intDataLength = ComputeLengthL(binBuffer, 28);
            int intFramesize = intDataLength + 36;
            binFrame = new byte[intDataLength + 35 + 1];
            Array.Copy(binBuffer, 0, binFrame, 0, intFramesize);
            if (intFramesize < binBuffer.Length)
            {
                Array.Copy(binBuffer, intFramesize, binBuffer, 0, binBuffer.Length - intFramesize);
                Array.Resize(ref binBuffer, binBuffer.Length - intFramesize);
            }
            else
            {
                binBuffer = new byte[0];
            }

            return intFramesize - 36;
        } // ExtractBuffer

        // Function to regiser a callsign with the AGW Packet Engine
        private void AGWRegisterCallsign(string strCall)
        {
            var bytTemp = new byte[36];
            if (!objTCPPort.Connected)
                return;
            objASCIIEncoding.GetBytes("X", 0, 1, bytTemp, 4);
            objASCIIEncoding.GetBytes(strCall.ToUpper().Trim(), 0, strCall.Trim().Length, bytTemp, 8);
            try
            {
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);
            }
            catch (Exception e)
            {
                _log.Error("[AGWRegisterCallsign] " + e.Message);
            }
        } // AGWRegiserCallsign

        // Function to unregiser a callsign with the AGW Packet Engine
        private void AGWUnRegisterCallsign(string strCall)
        {
            var bytTemp = new byte[36];
            if (!objTCPPort.Connected)
                return;
            objASCIIEncoding.GetBytes("x", 0, 1, bytTemp, 4);
            objASCIIEncoding.GetBytes(strCall.ToUpper().Trim(), 0, strCall.Trim().Length, bytTemp, 8);
            try
            {
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);
            }
            catch (Exception e)
            {
                _log.Error("[AGWUnRegisterCallsign] " + e.Message);
            }
        } // AGWUnRegisterCallsign

        // Function to extract a callsign from a Frame
        private string ExtractCall(byte[] Frame, int Index)
        {
            // Extract clean Call from the Frame even if there are null values (Byte = 0)
            // Terminates the string on the first 0 value to avoid left over garbage in AGW frame.
            var B = new byte[10];
            int i;
            for (i = 0; i <= 9; i++)
            {
                if (Frame[i + Index] != 0)
                {
                    B[i] = Frame[i + Index];
                }
                else
                {
                    break;
                }
            }

            return Globals.GetString(B).ToUpper().Trim();
        }   // ExtractCall

        // Function to extract Text from a AGWPE frame
        private string ExtractText(byte[] Frame, int Index, int Length)
        {
            // Extract clean text from the Frame even if there are null values (Byte = 0)
            // Replaces all null values with Empty strings

            var B = new byte[Length];
            if (Length > 0)
            {
                Array.Copy(Frame, Index, B, 0, Length);
                return Globals.GetString(B).Trim();
            }
            else
            {
                return "";
            }
        }   // ExtractText

        // Function to do DirtyDisconnect
        private bool DirtyDisconnect()
        {
            // This sub used to insure a disconnect usually called by a timeout condition
            if (!blnLoggedIn)
                return false;
            try
            {
                if (ConnectedCall.Port > 0)
                {
                    var bytTemp = new byte[36];
                    bytTemp[0] = Convert.ToByte(ConnectedCall.Port - 1);
                    objASCIIEncoding.GetBytes("d", 0, 1, bytTemp, 4);
                    objASCIIEncoding.GetBytes(Globals.SiteCallsign, 0, Globals.SiteCallsign.Length, bytTemp, 8);
                    objASCIIEncoding.GetBytes(ConnectedCall.Callsign, 0, ConnectedCall.Callsign.Length, bytTemp, 18);
                    objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);
                    Thread.Sleep(1000);
                    objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length); // Send the disconnect twice to cause immediate disconnect
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _log.Error("[ModemAGW.DirtyDisconnect] " + e.Message);
                return false;
            }
        }  // DirtyDisconnect

        private bool blnClosing = false;

        // Subroutine to close the channel and put TNC into known state
        public bool Close()
        {
            bool CloseRet = default;
            // Always call this method before the instance goes out of scope...
            // Sequence for going from Host mode to Terminal Mode
            CloseRet = true;
            
            if (blnClosing)
                return true; // Too keep from reentrancy
            blnClosing = true;
            if (!(enmState == LinkStates.Disconnected | enmState == LinkStates.LinkFailed))
            {
                DirtyDisconnect();
            }

            Release(); // disconnect and release AGW resource
            var dttWait = DateTime.Now;
            while (DateTime.Now.Subtract(dttWait).TotalMilliseconds < 1000 & blnLoggedIn)
            {
                Thread.Sleep(100);
                ProcessPortInput();
            }

            Globals.queChannelDisplay.Enqueue("G*** Closing " + Globals.stcSelectedChannel.ChannelName + " at " + Globals.TimestampEx());
            if (objProtocol is object)
                objProtocol.CloseProtocol();
            blnClosing = false;
            Globals.queStatusDisplay.Enqueue("Idle");
            Globals.queRateDisplay.Enqueue("------");
            Globals.blnChannelActive = false;
            Globals.ObjSelectedModem = null;
            return CloseRet;
        } // Close

        // Sub to Append Incoming byte array to data buffer
        private void AppendBuffer(ref byte[] Buffer, byte[] Incoming)
        {
            int nOriginalSize;
            nOriginalSize = Buffer.Length;
            Array.Resize(ref Buffer, Buffer.Length + (Incoming.Length - 1) + 1);
            Array.Copy(Incoming, 0, Buffer, nOriginalSize, Incoming.Length);
        } // AppendBuffer

        // Request Frames Outstanding for all Connected Calls ("Y" frame)
        private void AGWRequestFramesOutstanding()
        {
            if (string.IsNullOrEmpty(ConnectedCall.Callsign))
                return;
            var bytTemp = new byte[36];
            bytTemp[0] = Convert.ToByte(intAGWPort - 1);
            objASCIIEncoding.GetBytes("Y", 0, 1, bytTemp, 4);
            objASCIIEncoding.GetBytes(ConnectedCall.Callsign, 0, ConnectedCall.Callsign.Length, bytTemp, 18);
            objASCIIEncoding.GetBytes(Globals.SiteCallsign, 0, Globals.SiteCallsign.Length, bytTemp, 8);
            try
            {
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);
            }
            catch (Exception e)
            {
                _log.Error("[AGWRequestFramesOutstanding] " + e.Message);
            }
        } // AGWRequestFramesOutstanding

        // Subroutine to test if the Packet engine is running and start if necessary.
        private void PacketEngineRun()
        {
            Process[] objAGWProcess;
            string strProcessName;
            objAGWProcess = Process.GetProcessesByName("Packet Engine Pro");
            if (objAGWProcess.Length != 0)
                return; // Packet Engine Pro is running
            objAGWProcess = Process.GetProcessesByName("AGW Packet Engine");
            if (objAGWProcess.Length != 0)
                return; // AGW Packet Engine is running
                        // Configure for Packet Engine Pro or AGW Packet Engine
            if (File.Exists(_dialogAgwEngine.AgwPath + "Packet Engine Pro.exe"))
            {
                strProcessName = "Packet Engine Pro";
            }
            else if (File.Exists(_dialogAgwEngine.AgwPath + "AGW Packet Engine.exe"))
            {
                strProcessName = "AGW Packet Engine";
            }
            else
            {
                Globals.queChannelDisplay.Enqueue("R*** AGWPE/PE Pro not found!");
                return;
            } // no way to determine process

            // Start the AGW Packet Engine or Packet Engine Pro
            var objProcess = new Process();
            var objProcessStartInfo = new ProcessStartInfo();
            objProcessStartInfo.WorkingDirectory = _dialogAgwEngine.AgwPath;
            objProcessStartInfo.UseShellExecute = true;
            objProcessStartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            objProcessStartInfo.FileName = _dialogAgwEngine.AgwPath + strProcessName + ".exe";
            objProcess.StartInfo = objProcessStartInfo;
            Globals.queChannelDisplay.Enqueue("R*** Starting " + strProcessName);
            objProcess.Start();
            var dtStart = DateTime.Now;
            while (DateTime.Now.Subtract(dtStart).TotalSeconds < 20) // wait up to 20 seconds
            {
                Thread.Sleep(1000);
                objAGWProcess = Process.GetProcessesByName(strProcessName);
                if (objAGWProcess.Length > 0)
                    break;
            }

            if (objAGWProcess.Length == 0)
            {
                Globals.queChannelDisplay.Enqueue("R*** Failure to start " + strProcessName);
            }
            else
            {
                Globals.queChannelDisplay.Enqueue("R" + strProcessName + "*** Running! Begin 10 sec wait to connect.");
                Thread.Sleep(10000);
            } // Wait an additional 10 sec for AGW to stabilize on some slow machines
        }   // PacketEngineRun

        ~ModemAGW()
        {
        }

        // Function to find the index of Value in a byte array
        public int IndexOf(int Value, byte[] Array, int Offset = 0)
        {
            if (Offset >= Array.Length)
                return -1;
            for (int i = Offset, loopTo = Array.Length - 1; i <= loopTo; i++)
            {
                if (Array[i] == Value)
                    return i;
            }

            return -1;
        }

        private void OnDataIn(object s, int bytesRead)
        {
            if (bytesRead > 0)
            {
                byte[] buffer = (byte[])s;
                byte[] newBuffer = new byte[bytesRead];
                Array.Copy(buffer, newBuffer, bytesRead);
                quePortInput.Enqueue(newBuffer);

                Task<int> t = null;
                try
                {
                    t = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                    t.ContinueWith(k =>
                    {
                        if (k.Exception == null)
                        {
                            OnDataIn(buffer, k.Result);
                        }
                        else
                        {
                            OnError(k.Exception);
                        }
                    });
                    t.Wait(0);
                }
                catch (Exception e)
                {
                    OnError(t.Exception);
                }
            }
            else
            {
                Disconnect();
            }
        } // OnDataIn


        private void OnError(Exception e)
        {
            // MainForm.UpdateChannelText("*** TCP DISCONNECT from PACKET ENGINE", "PURPLE")
            Globals.queChannelDisplay.Enqueue("R*** TCP DISCONNECT from PACKET ENGINE");
            blnLoggedIn = false;
        }

        private void OnConnected(object sender)
        {
            try
            {
                if (!string.IsNullOrEmpty(_dialogAgwEngine.AgwUserId)) // do a secure AGWPE login (needs to be verified)
                {
                    // MainForm.UpdateChannelText("*** Secure login to Packet Engine for user ID " & _dialogAgwEngine.AgwUserId)
                    Globals.queChannelDisplay.Enqueue("R*** Secure login to Packet Engine for user ID " + _dialogAgwEngine.AgwUserId);
                    var bTemp = new byte[546];
                    objASCIIEncoding.GetBytes("P", 0, 1, bTemp, 4); // login frame
                    Array.Copy(ComputeLengthB(255 + 255), 0, bTemp, 28, 4);
                    objASCIIEncoding.GetBytes(_dialogAgwEngine.AgwUserId, 0, _dialogAgwEngine.AgwUserId.Length, bTemp, 36);
                    objASCIIEncoding.GetBytes(_dialogAgwEngine.AgwPassword, 0, _dialogAgwEngine.AgwPassword.Length, bTemp, 36 + 255);
                    objTCPPort.GetStream().Write(bTemp, 0, bTemp.Length);
                    Thread.Sleep(500);
                }
                // after login call for registration first 
                Globals.queChannelDisplay.Enqueue("R*** Registering " + Globals.SiteCallsign + " with AGW Engine");
                AGWRegisterCallsign(Globals.SiteCallsign); // only one callsign may be registered for all ports!
            }
            catch (Exception e)
            {
                _log.Error("[ModemAGW.objTCPPort_OnReadyToSend] " + e.Message);
            }
        }

        // Subroutine to Initiate a Connect to the remote call 
        public bool Connect(bool blnAutomatic)
        {
            string strVia = "";
            var bytTemp = new byte[36];
            string strTarget = Globals.stcSelectedChannel.RemoteCallsign;
            if (!Initialize())
                return false;
            blnNormalDisconnect = false;
            if (RunScript(ref strVia, ref strTarget)) // Activates scripting, modifies sVia and sTarget
            {
                ConnectedCall.Callsign = strTarget;
                bytTemp[0] = Convert.ToByte(intAGWPort - 1);
                bytTemp[6] = 0xF0; // AX.25 Information
                objASCIIEncoding.GetBytes(Globals.SiteCallsign, 0, Globals.SiteCallsign.Length, bytTemp, 8);
                objASCIIEncoding.GetBytes(strTarget, 0, strTarget.Length, bytTemp, 18);
                if (string.IsNullOrEmpty(strVia))
                {
                    objASCIIEncoding.GetBytes("C", 0, 1, bytTemp, 4);
                }
                else
                {
                    objASCIIEncoding.GetBytes("v", 0, 1, bytTemp, 4);
                    var bytDigis = new byte[1]; // make one byte for the count
                    string[] strDigi;
                    strDigi = strVia.Split(',');
                    foreach (string Digi in strDigi)
                    {
                        Array.Resize(ref bytDigis, bytDigis.Length + 9 + 1); // make 10 bytes larger
                        var tmpDigi = Digi.Substring(0, Math.Min(Digi.Length, 10)).Trim();  // limit to 10 char max
                        objASCIIEncoding.GetBytes(tmpDigi, 0, tmpDigi.Length, bytDigis, bytDigis.Length - 10);
                        if (bytDigis.Length >= 71)
                            break;
                    }

                    bytDigis[0] = Convert.ToByte(bytDigis.Length / (double)10);
                    Array.Copy(ComputeLengthB(bytDigis.Length), 0, bytTemp, 28, 4);
                    AppendBuffer(ref bytTemp, bytDigis);
                }

                try
                {
                    Globals.queStatusDisplay.Enqueue("Connecting");
                    Globals.queChannelDisplay.Enqueue("R*** Connecting to " + strTarget);
                    intConnectTimer = 0;
                    objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);
                    enmState = LinkStates.Connecting;
                    return true;
                }
                catch (Exception e)
                {
                    _log.Error("[ModemAGW.Connect] " + e.Message);
                    return false;
                }
            }
            else
            {
                Globals.queChannelDisplay.Enqueue("R*** Script Error - connection ended");
                Globals.blnChannelActive = false;
                enmState = LinkStates.LinkFailed;
                Globals.ObjSelectedModem = null;
                return false;
            }
        }

        // Function to initiate the connect script
        private bool RunScript(ref string Via, ref string Target)
        {
            bool RunScriptRet = default;
            try
            {
                // Returns True if OK False if error in Script or processing Error
                // updates/sets Via and Target by reference
                RunScriptRet = true;
                intScriptTimer = 0; // initialize the script timer
                Via = ""; // initialize Via to none
                if (ConnectScript == null)
                {
                    intConnectScriptPtr = -1; // This sets the Script Pointer to signal Inactive
                    return RunScriptRet; // no script. Do not update sTarget. Via is empty
                }
                else
                {
                    int intPt;
                    string strTemp;
                    string strTok;
                    strTemp = " " + ConnectScript[0].ToUpper().Trim();
                    // This strips off any leading V or Via (case insensitive)
                    // and skips over any syntax "Connect via"
                    intPt = strTemp.IndexOf(" V ");
                    if (intPt != -1)
                    {
                        Via = strTemp.Substring(intPt + 2).Trim();
                    }
                    else
                    {
                        intPt = strTemp.IndexOf(" VIA ");
                        if (intPt != -1)
                        {
                            Via = strTemp.Substring(intPt + 4).Trim();
                        }
                    }

                    if ((ConnectScript.Length - 1) == 0)
                    {
                        // simple via connect, just a single line (not a true script)
                        intConnectScriptPtr = -1; // Set Script Pointer to Inactive sVia is updated, sTarget is unchanged
                        return RunScriptRet;
                    }
                    else
                    {
                        // true script processing here (indicated by at least two Connection script lines)
                        intConnectScriptPtr = 0; // Initialize ptr to first script line (signals the script is active)
                        strTok = ConnectTarget(ConnectScript[0]);
                        if (!string.IsNullOrEmpty(strTok))
                        {
                            Target = strTok; // sTarget is updated 
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
        }

        // Function to sequence the connect scrip
        private bool SequenceScript(string Text, string From)
        {
            bool SequenceScriptRet = default;
            // Returns True if scripting is completed, False otherwise
            var strDataToSend = default(string);
            bool blnTextFound;
            var switchExpr = intConnectScriptPtr;
            switch (switchExpr)
            {
                case -1:  // No scripting
                    {
                        if (Text.StartsWith("***"))
                        {
                            Globals.queChannelDisplay.Enqueue("R " + Text + " at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss UTC"));
                        }
                        else
                        {
                            Globals.queChannelDisplay.Enqueue("R*** " + Text + " at " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss UTC"));
                        }
                        // queChannelStatus.Enqueue("Connected")
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
                            Disconnect();
                            return false;
                        }
                        else if (ConnectScript.Length > intConnectScriptPtr + 1)
                        {
                            blnTextFound = Text.ToUpper().Contains(ConnectScript[intConnectScriptPtr + 1]);
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
                var bytTemp = new byte[36];
                if (!blnLoggedIn)
                    return SequenceScriptRet;
                bytTemp[0] = Convert.ToByte(intAGWPort - 1);
                objASCIIEncoding.GetBytes("D", 0, 1, bytTemp, 4);
                objASCIIEncoding.GetBytes(Globals.SiteCallsign, 0, Globals.SiteCallsign.Length, bytTemp, 8);
                objASCIIEncoding.GetBytes(From, 0, From.Length, bytTemp, 18);
                Array.Copy(ComputeLengthB((bytData.Length - 1) + 1), 0, bytTemp, 28, 4);
                AppendBuffer(ref bytTemp, bytData);
                try
                {
                    objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);
                }
                catch (Exception ex)
                {
                    _log.Error("[ModemAGW.SequenceScript] " + ex.Message);
                }
            }

            return SequenceScriptRet;
        }

        // Function to test Text against any of the script bailouts.
        private bool EndScript(string sText)
        {
            var EndText = new string[] { "DISCONNECTED", "TIMEOUT", "EXCEEDED", "FAILURE", "BUSY" };
            for (int i = 0, loopTo = (EndText.Length - 1); i <= loopTo; i++)
            {
                if (sText.ToUpper().IndexOf(EndText[i]) != -1)
                    return true;
            }

            return false;
        }

        // Function to extract target call from connect script
        private string ConnectTarget(string Script)
        {
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
        }

        private void ProcessPortInput()
        {
            // Takes each partial block from the tcp port input queue and forms
            // finished frames. Finished frames are processed by the ProcessReceivedFrame
            // method...

            int intDataLength;
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

                AppendBuffer(ref bytDataBuffer, bytBuffer); // Add data to buffer array
                if (bytDataBuffer.Length >= 36) // at least one complete frame header
                {
                    intDataLength = ComputeLengthL(bytDataBuffer, 28); // get and decode the data length field from the header
                    while (bytDataBuffer.Length >= 36 + intDataLength)   // At least one complete frame...
                    {
                        var bytFrame = new byte[0];
                        ExtractFrame(ref bytDataBuffer, ref bytFrame);
                        ProcessReceivedFrame(ref bytFrame);
                        if (bytDataBuffer.Length >= 36)
                        {
                            intDataLength = ComputeLengthL(bytDataBuffer, 28); // get and decode the data length field from the header
                        }
                        else
                        {
                            intDataLength = 0;
                        }
                    }
                }
            }
        } // ProcessPortInput

        private void ProcessReceivedFrame(ref byte[] bytFrame)
        {

            // Process a complete received Frame bytFrame is a complete frame.

            int lDataLength;
            string strFrameType = "";
            int nPort;
            string strCallFrom = "";
            string sCallTo = "";
            string strText = "";
            try
            {
                lDataLength = ComputeLengthL(bytFrame, 28);
                strFrameType = Convert.ToString((char)bytFrame[4]);
                switch (strFrameType)
                {
                    case "R":   // Version request reply (used for ping)
                        {
                            strVersion = (bytFrame[36] + 256 * bytFrame[37]).ToString();
                            strVersion = strVersion + "." + (bytFrame[40] + 256 * bytFrame[41]).ToString();
                            Globals.queChannelDisplay.Enqueue("PAGWPE Status OK @" + DateTime.UtcNow.ToString("HH:mm") + " Ver:" + strVersion);
                            break;
                        }

                    case "X": // Application registration Reply
                        {
                            strCallFrom = ExtractCall(bytFrame, 8);
                            if (bytFrame[36] == 1)
                            {
                                Globals.queChannelDisplay.Enqueue("P" + strCallFrom + " *** REGISTERED to AGWPE");
                                blnLoggedIn = true;
                            }
                            else // Failed CallSign registration
                            {
                                Globals.queChannelDisplay.Enqueue("P*** REGISTRATION of " + strCallFrom + " FAILED!");
                                AGWUnRegisterCallsign(Globals.SiteCallsign); // try to unregister 
                                blnLoggedIn = false;
                            }

                            break;
                        }

                    case "C": // Connection notice
                        {
                            ConnectedCall.Port = 1 + bytFrame[0];
                            strCallFrom = ExtractCall(bytFrame, 8);
                            ConnectedCall.Callsign = strCallFrom;
                            sCallTo = ExtractCall(bytFrame, 18);
                            strText = ExtractText(bytFrame, 36, lDataLength);
                            Globals.queChannelDisplay.Enqueue("P*** CONNECTED to " + strCallFrom);
                            string strStream = "";
                            SequenceScript(strText, strCallFrom);
                            ConnectedCall.ActivityTimer = 0; // activate the connection timer
                            intConnectTimer = -1; // reset the connection timer
                            break;
                        }

                    case "D":   // Data frame
                        {
                            nPort = 1 + bytFrame[0];
                            strCallFrom = ExtractCall(bytFrame, 8);
                            sCallTo = ExtractCall(bytFrame, 18);
                            string strStream = "";
                            ConnectedCall.ActivityTimer = 0; // reset the timer
                            if (intConnectScriptPtr != -1) // still sequencing script
                            {
                                strText = ExtractText(bytFrame, 36, lDataLength);
                                if (SequenceScript(strText, strCallFrom))
                                {
                                    // The script is complete ....pass the last Data in to the channel 
                                    var DataIn = new byte[lDataLength];
                                    Array.Copy(bytFrame, 36, DataIn, 0, lDataLength);
                                    objProtocol.ChannelInput(ref DataIn);
                                }
                            }
                            else  // normal connected data...pass to protocol
                            {
                                var DataIn = new byte[lDataLength];
                                Array.Copy(bytFrame, 36, DataIn, 0, lDataLength);
                                objProtocol.ChannelInput(ref DataIn);
                            }

                            break;
                        }

                    case "d":   // Disconnected frame
                        {
                            nPort = 1 + bytFrame[0];
                            strCallFrom = ExtractCall(bytFrame, 8);
                            sCallTo = ExtractCall(bytFrame, 18);
                            Globals.queChannelDisplay.Enqueue("P*** DISCONNECTED from " + strCallFrom);
                            if (blnNormalDisconnect)
                            {
                                enmState = LinkStates.Disconnected;
                            }
                            else
                            {
                                enmState = LinkStates.LinkFailed;
                            }

                            break;
                        }

                    case "Y":  // Frames outstanding for a specific connection
                        {
                            nPort = 1 + bytFrame[0];
                            strCallFrom = ExtractCall(bytFrame, 18);
                            sCallTo = ExtractCall(bytFrame, 8);
                            if (ConnectedCall.Port == nPort & (ConnectedCall.Callsign ?? "") == (strCallFrom ?? ""))
                            {
                                ConnectedCall.FramesOutstanding = ComputeLengthL(bytFrame, 36);
                            } // '''add others here

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                _log.Error("[ModemAGW.ProcessReceivedFrame] Frame Type:" + strFrameType + " / " + ex.Message);
            }
        } // ProcessReceivedFrame 
    }
}