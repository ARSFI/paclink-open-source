using System;
using System.Text;
using System.Threading;
using System.Timers;

namespace TNCKissInterface
{
    //
    // Connection Class.  This class handles most of the AX25 protocol and is the
    // primary interface to the DLL.  
    //
    // Copyright 2008-2010 - Peter R. Woods (N6PRW)
    //

    //#region Connection Event Class section

    public static class GlobalVar           // (JNW Jan2015)
    {
        public static Int16 ackModeIDSeed = 1234;
    }

    public class ConnectionEvents : EventArgs
    {
        //
        // Event class used for notification of Connection State changes
        //
        // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
        //

        Connection.ConnectionState connectState;
        String destinationStation;
        String sourceStation;
        String relayStation1;
        String relayStation2;
        Int32 connectionID;

        public ConnectionEvents(Connection.ConnectionState conState, String d, String s, String r1, String r2)
        {
            connectState = conState;
            destinationStation = d;
            sourceStation = s;
            relayStation1 = r1;
            relayStation2 = r2;
            connectionID = -1;
        }

        public ConnectionEvents(Connection.ConnectionState conState, String d, String s, String r1, String r2, Int32 id)
        {
            connectState = conState;
            destinationStation = d;
            sourceStation = s;
            relayStation1 = r1;
            relayStation2 = r2;
            connectionID = id;

        }

        public Connection.ConnectionState GetState
        {
            get { return connectState; }
        }
        public String GetDestination
        {
            get { return destinationStation; }
        }
        public String GetSource
        {
            get { return sourceStation; }
        }
        public String GetRelay1
        {
            get { return relayStation1; }
        }
        public String GetRelay2
        {
            get { return relayStation2; }
        }
        public Int32 GetConnectionID
        {
            get { return connectionID; }
        }
    }

    public class ConnectionStateEvents
    {
        //
        // Event handler class for connection state change notifications
        //
        public event EventHandler<ConnectionEvents> ReportConnectionEvents;

        public void DoEvent(Connection.ConnectionState cs, String d, String s, String r1, String r2, Int32 id)
        {
            //
            // Use local variable so the code is re-entrant
            //
            EventHandler<ConnectionEvents> temp = ReportConnectionEvents;
            temp(this, new ConnectionEvents(cs, d, s, r1, r2, id));
        }
    }
    //#endregion


    //#region Indication Event Class section

    //
    // Data Layer Connection Indication events
    //
    public class IndicationEvents : EventArgs
    {
        //
        // Event class used for notification of Connection State changes
        //

        Connection.IndicationTypes indicationType;
        String indicationData;
        String destinationAddr;
        String sourceAddr;
        String relayAddr1;
        String relayAddr2;
        Int32 connectionID;

        public IndicationEvents(Connection.IndicationTypes indType, String d, String s, String r1, String r2, String data)
        {
            indicationType = indType;
            indicationData = data;
            destinationAddr = d;
            sourceAddr = s;
            relayAddr1 = r1;
            relayAddr2 = r2;
            connectionID = -1;
        }
        public IndicationEvents(Connection.IndicationTypes indType, String d, String s, String r1, String r2, String data, Int32 id)
        {
            indicationType = indType;
            indicationData = data;
            destinationAddr = d;
            sourceAddr = s;
            relayAddr1 = r1;
            relayAddr2 = r2;
            connectionID = id;
        }

        public Connection.IndicationTypes GetIndication
        {
            get { return indicationType; }
        }
        public String GetData
        {
            get { return indicationData; }
        }
        public String GetDestination
        {
            get { return destinationAddr; }
        }
        public String GetSource
        {
            get { return sourceAddr; }
        }
        public String GetRelay1
        {
            get { return relayAddr1; }
        }
        public String GetRelay2
        {
            get { return relayAddr2; }
        }
        public Int32 GetConnectionID
        {
            get { return connectionID; }
        }
    }

    public class IndicationTypeEvents
    {
        //
        // Event handler class for connection state change notifications
        //
        public event EventHandler<IndicationEvents> ReportIndicationEvents;

        //public void DoEvent(Connection.IndicationTypes it, String d, String s, String r1, String r2)
        //{
        //    //
        //    // Use local variable so the code is re-entrant
        //    //
        //    EventHandler<IndicationEvents> temp = ReportIndicationEvents;
        //    temp(this, new IndicationEvents(it, d, s, r1, r2, ""));
        //}

        //public void DoEvent(Connection.IndicationTypes it, String d, String s, String r1, String r2, String data)
        //{
        //    //
        //    // Use local variable so the code is re-entrant
        //    //
        //    EventHandler<IndicationEvents> temp = ReportIndicationEvents;
        //    temp(this, new IndicationEvents(it, d, s, r1, r2, data));
        //}

        public void DoEvent(Connection.IndicationTypes it, String d, String s, String r1, String r2, String data, Int32 id)
        {
            //
            // Use local variable so the code is re-entrant
            //
            EventHandler<IndicationEvents> temp = ReportIndicationEvents;
            temp(this, new IndicationEvents(it, d, s, r1, r2, data, id));
        }
    }
    //#endregion

    public class Connection
    {

        const Int32 MAXQUEUE = 256;
        const Int32 MAXIFRAME = 32768;
        const Int32 MAXRECVBUF = 0x200000;      // 2 mb recv buffer;
        const Int32 MAXRECVBUFMASK = 0x1FFFFF;

        Thread processQueuedRemotePacketsThread;
        Thread receivedPacketReassemberThread;
        Thread sendPacketSegmenterThread;

        static String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);

        public Timer timerAck;        // Changesed to Public for AckMode   (JNW Jan15)
        Timer timerPoll;
        Timer timerXID;
        Timer timerSegmenter;
        Timer timerPendingAck;

        ElapsedEventHandler timerAckHandler;
        ElapsedEventHandler timerPollHandler;
        ElapsedEventHandler timerXIDHandler;
        ElapsedEventHandler timerSegmenterHandler;
        ElapsedEventHandler timerPendingAckHandler;

        //  ACKMODE ID for this connection          (JNW Jan15)

        public Int16 AckModeID;

        //
        // Connection counters
        //
        Int32 _bytesReceived;
        Int32 _bytesSent;
        Int32 _packetsReceived;
        Int32 _packetsSent;

        Object LogSyncObject = new Object();


        public Int32 bytesReceived
        {
            get { return _bytesReceived; }
        }

        public Int32 bytesSent
        {
            get { return _bytesSent; }
        }

        public Int32 packetsSent
        {
            get { return _packetsSent; }
        }

        public Int32 packetsReceived
        {
            get { return _packetsReceived; }
        }

        public Int32 connectionID = -1;

        //
        // Negotiated operational parameters
        // 
        Int32 negMaxIFrame = 0;
        Int32 negMaxWindowSize = 0;
        Int32 negAckTimer = 0;
        Int32 negMaxRetry = 0;
        Boolean negUseIPoll = false;

        Boolean XIDInProgress = false;

        Object recvBufferSyncLock;

        //
        // Receive output ring buffer
        //
        Byte[] recvBuffer = new Byte[MAXRECVBUF];
        Int32 recvBufferInPtr = 0;
        Int32 recvBufferOutPtr = 0;
        Boolean recvWaiting = false;

        //#region control flags

        Boolean REJSent = false;     // REJ frame has been sent
        //Boolean SREJSent = false;    // Selective REJ frame sent
        Boolean remoteBusy = false;  // Remote sent RNR
        Boolean localBusy = false;   // Local station busy
        Boolean pollSent = false;    // We have sent an I(P), so need to wait for RR(F)      (JNW Jan15)
        //Boolean RTTRun = false;      // Round trip "timer" is running

        //
        // Smoothed round trip time values
        //
        public Int32 smoothedRoundTrip = 0;       // Changesed to Public for AckMode   (JNW Jan15)
        //Int32 nextTimerAckInterval = 0;
        //Int32 roundTripTimeDeviation = 0;

        //
        // Connection transfer state variables
        //
        Int32 vSend = 0;         // Send state variable
        Int32 vRecv = 0;         // Receive state variable
        Int32 vRecvAck = 0;      // Last Receive Ack
        Int32 vAck = 0;          // Acknowledge state variable

        //
        // InProgress Packet list
        //
        Frame.Packet[] packetsInProgress = new Frame.Packet[128];
        AutoResetEvent sendIFrameWait = new AutoResetEvent(false);

        Boolean sendIFrameWaiting = false;

        Object connSyncLock;                       // Lock object for changing connection state
        Object frameSyncLock;                      // Lock object for changing IFrame counter variables

        Boolean inTimerRecovery = false;           // Set when in a timer recovery condition

        // Number of retires attempted
        Byte retryCount = 0;

        //
        // Error Indication Strings
        //
        //String errorStringA = "A - F=1 received but P=1 not outstanding";
        //String errorStringB = "B - Unexpected DM with F=1 in states 3, 4 or 5";
        String errorStringC = "C - Unexpected UA in states 3, 4 or 5";
        String errorStringD = "D - UA received without F=1 when SABM or DISC was sent P=1";
        String errorStringE = "E - DM received in states 3, 4 or 5";
        String errorStringF = "F - Data link reset; i.e., SABM received in state 3, 4 or 5";
        String errorStringH = "H - Retry limit reached";
        String errorStringI = "I - N2 timeouts: unacknowledged data";
        //String errorStringJ = "J - N(r) sequence error";
        String errorStringK = "K - FRMR received";
        //String errorStringL = "L - Control field invalid or not implemented";
        //String errorStringM = "M - Information field was received in a U- or S-type frame";
        //String errorStringN = "N - Length of frame incorrect for frame type";
        //String errorStringO = "O - I frame exceeded maximum allowed length";
        //String errorStringP = "P - N(s) out of the window";
        //String errorStringQ = "Q - UI response received, or UI command with P=1 received";
        //String errorStringR = "R - UI frame exceeded maximum allowed length";
        String errorStringS = "S - I response received";
        String errorStringT = "T - N2 timeouts: no response to enquiry";
        String errorStringU = "U - N2 timeouts: extended peer busy condition";
        //String errorStringV = "V - No DL machines available to establish connection";
        //String errorStringY = "Y - Segmenter - data too long for segment";
        String errorStringZ = "Z - Segmenter - reassembly error";

        public Boolean DoSABMEConnect = false;

        // Default protocol version is V2.2 with Mod8 sequence numbers
        //Frame.ProtocolVersion ax25Version = Frame.ProtocolVersion.V22;
        Frame.ProtocolVersion remoteAX25Version = Frame.ProtocolVersion.V20;
        //Frame.SequenceNumberMode sequenceMode = Frame.SequenceNumberMode.Mod8;

        //#endregion

        Station _connectedStation;
        public Station connectedStation
        {
            get { return _connectedStation; }
        }

        public ConnectionParameterBuf parameterBuf;

        DataLinkProvider dataLinkProvider;
        BlockingQueue ConnectionRemoteQ = new BlockingQueue(MAXQUEUE);
        BlockingQueue toRecvQ = new BlockingQueue(MAXQUEUE);
        BlockingQueue toSendQ = new BlockingQueue(MAXQUEUE);

        public ManualResetEvent connectionDisconnected = new ManualResetEvent(false);
        public ManualResetEvent connectionFinal = new ManualResetEvent(false);
        public ManualResetEvent connectionConnected = new ManualResetEvent(false);
        public AutoResetEvent receiveDataAvailable = new AutoResetEvent(false);
        public AutoResetEvent sendDataAvailable = new AutoResetEvent(false);

        public enum ConnectionStatus
        {
            Success = 0,          // Success
            DMReceived,           // Target Station Disconnected
            Timeout,              // Connection timed out
            AlreadyConnected,     // Already connected
            AlreadyDisconnected,  // Already disconnected
            ForcedDisconnect,     // Dirty Disconnect
            NotConnected,         // Not in a connected state
            Closed,               // Connection externally disabled
            ConnectionExists,     // Another connection to this station exists
            Busy                  // Connection is busy
        }

        public enum IndicationTypes
        {
            Error = 0,            // Error Indication
            DisconnectRequest,    // Disconnect Requested Indication
            ConnectRequest,       // Connect Requested Indication
            Data                  // Data Indication
        }

        //
        // Link flags for this connection
        //

        public enum ConnectionState
        {
            //
            // Description
            //
            // Disconnected: Link is inactive.
            // Listening: Connection is initialized and waiting for an inbound connection
            // ConnectPending: Connection request sent to remote station and waiting for ack
            // Connected:  Link up and active 
            // DisconnectPending: A Disc cmd has beed received or an unacked disc command has been sent
            //
            Disconnected,
            ConnectPending,
            Connected,
            DisconnectPending
        };

        ConnectionState _connectState = ConnectionState.Disconnected;

        public ConnectionState connectState
        {
            get { return _connectState; }
            set
            {
                lock (connSyncLock)
                {
                    if (value != _connectState)
                    {
                        //
                        // Block all connection state waiters
                        //
                        connectionFinal.Reset();
                        connectionConnected.Reset();
                        connectionDisconnected.Reset();

                        _connectState = value;

                        registerStateHandlersFull.DoEvent(value,
                            connectedStation.destinationStation.stationIDString,
                            connectedStation.sourceStation.stationIDString,
                            connectedStation.relayStation1.stationIDString,
                            connectedStation.relayStation2.stationIDString,
                            connectionID);

                        if (value == ConnectionState.Connected)
                        {
                            //
                            // Tell any registered listeners the status has changed to connected
                            //
                                Support.DbgPrint("Connection state --> Connected");
                                Support.PktPrint(" Connection state --> Connected" + CRLF, ref LogSyncObject);
                            registerStateHandlers.DoEvent(value,
                               connectedStation.destinationStation.stationIDString,
                               connectedStation.sourceStation.stationIDString,
                               connectedStation.relayStation1.stationIDString,
                               connectedStation.relayStation2.stationIDString,
                               connectionID);

                            //
                            // Free connect waiters
                            //
                            connectionFinal.Set();
                            connectionConnected.Set();
                            timerAck.Stop();
                            timerPoll.Start();
                            return;
                        }

                        if (value == ConnectionState.Disconnected)
                        {
                            //
                            // Tell any registered listeners that the connection status has changed
                            //
                            Support.DbgPrint("Connection state --> Disconnected");
                            Support.PktPrint(" Connection state --> Disconnected" + CRLF, ref LogSyncObject);
                            registerStateHandlers.DoEvent(value,
                               connectedStation.destinationStation.stationIDString,
                               connectedStation.sourceStation.stationIDString,
                               connectedStation.relayStation1.stationIDString,
                               connectedStation.relayStation2.stationIDString,
                               connectionID);

                            //
                            // Wake up any threads waiting on connected frame processing
                            //
                            toRecvQ.Wakeup();
                            toSendQ.Wakeup();

                            if (sendIFrameWaiting)
                            {
                                //
                                // Wake up any Senders
                                //
                                sendIFrameWait.Set();
                            }

                            if (recvWaiting)
                            {
                                //
                                // Wake up any senders
                                //
                                receiveDataAvailable.Set();
                            }

                            //
                            // Free disconnect waiters
                            //
                            connectionFinal.Set();
                            connectionDisconnected.Set();

                            //
                            // Stop the timers
                            //
                            timerAck.Stop();
                            timerPoll.Stop();
                            timerSegmenter.Stop();
                            return;
                        }
                    }
                }
            }
        }

        public Boolean isConnected
        {
            //
            // Returns true if the connection is in a connected state
            //
            get
            {
                if (_connectState.Equals(ConnectionState.Connected))
                {
                    return true;
                }
                return false;
            }
        }

        Boolean runFlag = true;

        public ConnectionStateEvents registerStateHandlers;
        public ConnectionStateEvents registerStateHandlersFull;
        public IndicationTypeEvents registerIndicationHandlers;

        //
        // Constructor
        //
        public Connection(DataLinkProvider dlp, ConnectionParameterBuf cpb)
        {
            //
            // Make a copy of the incoming buffer.  This allows the same user-defined connection parameter 
            // buffer to be used for multiple connections
            //
            ConnectionParameterBuf tmpCpb = new ConnectionParameterBuf();
            tmpCpb.ackTimer = cpb.ackTimer;
            tmpCpb.maxIFrame = cpb.maxIFrame;
            tmpCpb.maxRetry = cpb.maxRetry;
            tmpCpb.maxWindowSize = cpb.maxWindowSize;
            tmpCpb.pollThresh = cpb.pollThresh;
            tmpCpb.pendingAckTimer = cpb.pendingAckTimer;
            tmpCpb.useAckMode = cpb.useAckMode;                     // (JNW Jan15)
            tmpCpb.useIPoll = cpb.useIPoll;                         // (JNW Feb15)
            ConnectionCommon(dlp, tmpCpb);
        }

        public Connection(DataLinkProvider dlp)
        {
            ConnectionCommon(dlp, new ConnectionParameterBuf());
        }

        void ConnectionCommon(DataLinkProvider dlp, ConnectionParameterBuf cpb)
        {
            //
            // Common routine shared by the Constructors.  Initialize the connection instance.
            //
            connSyncLock = new Object();
            frameSyncLock = new Object();
            recvBufferSyncLock = new Object();

            dataLinkProvider = dlp;
            parameterBuf = cpb;

            //
            // Register a blank delegate as a default handler for connection and indication events
            //
            registerStateHandlers = new ConnectionStateEvents();
            registerStateHandlers.ReportConnectionEvents += new EventHandler<ConnectionEvents>(ConnectionEventsDelegate);

            registerStateHandlersFull = new ConnectionStateEvents();
            registerStateHandlersFull.ReportConnectionEvents += new EventHandler<ConnectionEvents>(ConnectionEventsDelegate);

            registerIndicationHandlers = new IndicationTypeEvents();
            registerIndicationHandlers.ReportIndicationEvents += new EventHandler<IndicationEvents>(IndicationEventsDelegate);

            _connectedStation = new Station();
            _connectedStation.sourceStation = dlp.localStationAddress;

            _connectState = ConnectionState.Disconnected;

            timerAckHandler = new ElapsedEventHandler(timerAck_Elapsed);
            timerPollHandler = new ElapsedEventHandler(timerPoll_Elapsed);
            timerXIDHandler = new ElapsedEventHandler(timerXID_Elapsed);
            timerSegmenterHandler = new ElapsedEventHandler(timerSegmenter_Elapsed);
            timerPendingAckHandler = new ElapsedEventHandler(timerPendingAck_Elapsed);

            timerAck = new Timer(timerAckHandler, "Ack - " + dlp.localStationAddress.stationIDString);
            timerPoll = new Timer(timerPollHandler, "Poll - " + dlp.localStationAddress.stationIDString);
            timerXID = new Timer(timerXIDHandler, "XID - " + dlp.localStationAddress.stationIDString);
            timerSegmenter = new Timer(timerSegmenterHandler, "Segmenter - " + dlp.localStationAddress.stationIDString);
            timerPendingAck = new Timer(timerPendingAckHandler, "PendingAck - " + dlp.localStationAddress.stationIDString);

            ConnectionInit();

            processQueuedRemotePacketsThread = new Thread(ProcessQueuedRemotePackets);
            processQueuedRemotePacketsThread.Name = "Connection Packet Remote Inbound Processing Thread for: " +
                dlp.localStationAddress.stationIDString;
            processQueuedRemotePacketsThread.Start();

            receivedPacketReassemberThread = new Thread(ReceivePacketReassember);
            receivedPacketReassemberThread.Name = "Receive Packet Reassembler Thread for: " +
                dlp.localStationAddress.stationIDString;
            receivedPacketReassemberThread.Start();

            sendPacketSegmenterThread = new Thread(SendPacketSegmenter);
            sendPacketSegmenterThread.Name = "Send Packet Segmenter Thread for: " +
                dlp.localStationAddress.stationIDString;
            sendPacketSegmenterThread.Start();

        }

        void ConnectionEventsDelegate(object sender, ConnectionEvents e)
        {
            // Enpty routine to act as first connection event delegate registration
        }

        void IndicationEventsDelegate(object sender, IndicationEvents e)
        {
            // Enpty routine to act as first connection event delegate registration
        }

        public Boolean Enabled
        {
            //
            // Returns false if this class instance has been shut down.
            //
            get { return runFlag; }
        }

        public void Close()
        {
            timerAck.Stop();
            timerPoll.Stop();
            timerSegmenter.Stop();
            timerXID.Stop();
            timerPendingAck.Stop();

            runFlag = false;

            //
            // Disable the queues and release any waiting blocked threads
            //
            ConnectionRemoteQ.enabled = false;
            toRecvQ.enabled = false;

            connectState = ConnectionState.Disconnected;

            //
            // Release any waiters
            //
            connectionDisconnected.Set();
            connectionConnected.Set();
            connectionFinal.Set();
            toRecvQ.Wakeup();
            toSendQ.Wakeup();
            receiveDataAvailable.Set();
            dataLinkProvider.RemoveConnection(this);
        }

        void ConnectionInit()
        {
            //
            // Initialize connection parameters and counters to their default state
            //
            vSend = 0;         // Send state variable
            vRecv = 0;         // Receive state variable
            vAck = 0;          // Acknowledge state variable

            retryCount = 0;
            XIDInProgress = false;

            //ax25Version = Frame.ProtocolVersion.V22;
            remoteAX25Version = Frame.ProtocolVersion.V20;
            //sequenceMode = Frame.SequenceNumberMode.Mod8;

            REJSent = false;     // REJ frame has been sent
            remoteBusy = false;  // Remote sent RNR
            localBusy = false;   // Local stationis busy

            //
            // Added 02-15-2010 by N6PRW
            // Initialize timer recovery flag
            //
            inTimerRecovery = false; // Status of timer recovery

            //RTTRun = false;      // Round trip "timer" is running
            //RRPending = false;
            //REJPending = false;

            //
            // Initialize counters
            //
            _bytesReceived = 0;
            _bytesSent = 0;
            _packetsReceived = 0;
            _packetsSent = 0;

            //
            // Load default values for the parameters
            //
            negMaxIFrame = parameterBuf.maxIFrame;
            negMaxWindowSize = parameterBuf.maxWindowSize;
            negMaxRetry = parameterBuf.maxRetry;
            negAckTimer = parameterBuf.ackTimer;
            negUseIPoll = parameterBuf.useIPoll;         // (JNW Feb15)
         
            if (parameterBuf.useAckMode)
                AckModeID = GlobalVar.ackModeIDSeed++;

            timerAck.Stop();
            InitTimerAckValue();
            timerAck.SetTime(smoothedRoundTrip, true);

            timerPoll.Stop();
            timerPoll.SetTime(parameterBuf.pollThresh, true);

            timerXID.Stop();
            timerXID.SetTime(1000 * 5);      // 5 seconds

            timerSegmenter.Stop();
            timerSegmenter.SetTime(1000 * 60 * 10);   // 10 minutes

            timerPendingAck.Stop();
            timerPendingAck.SetTime(parameterBuf.pendingAckTimer);

            connectionFinal.Reset();
        }

        //#region Timers

        void timerAck_Elapsed(object sender, ElapsedEventArgs e)
        {
            //
            // Added 02-15-2010 by N6PRW
            // Disable the timers if we are in a disconnected state
            //
            if (connectState.Equals(ConnectionState.Disconnected))
            {
                //
                // Timer fired while we were already disconnected
                //
                ConnectionInit();
                return;
            }

            //
            // Select default inquiry frame
            //
            Frame.FrameTypes tmpType = Frame.FrameTypes.RRType;
            if (localBusy)
            {
                tmpType = Frame.FrameTypes.RNRType;
            }

            if (timerAck.Enabled)
            {
                Support.DbgPrint(connectedStation.sourceStation.stationIDString + " Ack timer fired...");

                //
                // Note, we leave the ACK timer running.  If we reach our retry limit, we stop the 
                // timer automatically when we set our state to disconnected
                //
                if (!inTimerRecovery)
                {
                    //
                    // Not in timer recover mode.  Clear the retry count,set timer recovery mode, and stop the
                    // polling timer
                    //
                    retryCount = 0;
                    inTimerRecovery = true;
                    Support.DbgPrint(connectedStation.sourceStation.stationIDString + " entering timer recovery");
                    timerPoll.Stop();
                }

                //
                // Bump the retry count
                //
                retryCount++;
                Support.PktPrint(" ACK timeout: Retry count = " + retryCount.ToString() + CRLF, ref LogSyncObject);

                if (connectState.Equals(ConnectionState.ConnectPending))
                {
                    //
                    // Timer fired during connection pending
                    //
                    tmpType = Frame.FrameTypes.SABMType;
                    if (retryCount == negMaxRetry)
                    {
                        //
                        // Retry limit reached
                        //
                        ErrorIndication(errorStringK);
                        DisconnectIndication();
                        connectState = ConnectionState.Disconnected;
                        return;
                    }
                }

                if (connectState.Equals(ConnectionState.DisconnectPending))
                {
                    //
                    // Timer fired during disconnect pending
                    //
                    tmpType = Frame.FrameTypes.DISCType;
                    if (retryCount == negMaxRetry)
                    {
                        //
                        // Retry limit reached
                        //
                        ErrorIndication(errorStringH);
                        connectState = ConnectionState.Disconnected;
                        return;
                    }
                }

                if (retryCount < negMaxRetry)
                {
                    //
                    // Still have retries left, so resend appropriate inquiry
                    //
                    Support.PktPrint(" Timeout: Resend inquiry.  Retry count = " + retryCount.ToString() + CRLF, ref LogSyncObject);
                    SendToDlp(Station.CommandResponse.DoCommand, tmpType, 1);
                    SelectTimerAckValue();
                    return;
                }

                //
                // Retry limit reached.  Remote host has gone dark, so just disconnect.
                //
                Support.PktPrint(" Timeout: Max retries exhausted.  Disconnect." + CRLF, ref LogSyncObject);
                if (vSend != vAck)
                {
                    ErrorIndication(errorStringI);
                }
                else
                {
                    if (remoteBusy)
                    {
                        ErrorIndication(errorStringU);
                    }
                    else
                    {
                        ErrorIndication(errorStringT);
                    }
                }
                connectState = ConnectionState.Disconnected;
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, 0);
            }
        }

        void timerPoll_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timerPoll.Enabled)
            {
                Support.DbgPrint(connectedStation.sourceStation.stationIDString + " polling timer fired...");
                if (connectState.Equals(ConnectionState.Connected))
                {
                    timerPoll.Stop();
                    retryCount = 0;
                    inTimerRecovery = true;
                    Support.PktPrint(" Polling timer: " + connectedStation.sourceStation.stationIDString + " entering timer recovery" + CRLF, ref LogSyncObject);
                    Support.DbgPrint(connectedStation.sourceStation.stationIDString + " entering timer recovery");
                    TransmitInquiry();
                }
            }
        }

        void timerXID_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timerXID.Enabled)
            {
                Support.DbgPrint(connectedStation.sourceStation.stationIDString + " XID timer fired...");
                Support.PktPrint(" " + connectedStation.sourceStation.stationIDString + " XID timer fired..." + CRLF, ref LogSyncObject);
                timerXID.Stop();
                if (XIDInProgress)
                {
                    //
                    // No response, so just load defaults & continue
                    //
                    negMaxIFrame = parameterBuf.maxIFrame;
                    negMaxWindowSize = parameterBuf.maxWindowSize;
                    negMaxRetry = parameterBuf.maxRetry;
                    negAckTimer = parameterBuf.ackTimer;

                    XIDInProgress = false;
                }
            }
        }

        void timerSegmenter_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (timerSegmenter.Enabled)
            {
                Support.DbgPrint(connectedStation.sourceStation.stationIDString + " Segmenter timer fired...");
                toRecvQ.Wakeup();
                timerSegmenter.Stop();
            }
        }

        void timerPendingAck_Elapsed(object sender, ElapsedEventArgs e)
        {
            Boolean doRecvAck = false;
            if (timerPendingAck.Enabled)
            {
                //timerPendingAck.Stop();
                //if (!inTimerRecovery && connectState.Equals(ConnectionState.Connected) && !localBusy)
                if (!inTimerRecovery && connectState.Equals(ConnectionState.Connected))
                {
                    lock (frameSyncLock)
                    {
                        if (vRecvAck != vRecv)
                        {
                            vRecvAck = vRecv;
                            doRecvAck = true;
                        }
                    }
                    if (doRecvAck)
                    {
                        //
                        // Send off pendingAck (RR) response with PF=0
                        // This occurs if we received an iFrame and have responded with any outbound iFrames to do
                        // do an implicit ack of the inbound one
                        //
                        Support.DbgPrint(connectedStation.sourceStation.stationIDString + " PendingAck required...");
                        EnquiryResponse(0);
                    }
                    return;
                }
            }
        }

        //#endregion

        public ConnectionStatus InitiateXID()
        {
            //
            // Initiate an XID Negotiation with the remote host
            //
            if (!Enabled)
            {
                //
                // Connection is closed and no longer usable
                //
                return ConnectionStatus.Closed;
            }
            if (!isConnected)
            {
                //
                // Connection is offline
                //
                return ConnectionStatus.NotConnected;
            }

            //
            // Kick off hte negotiation
            //

            if (connectState.Equals(ConnectionState.Connected))
            {
                InitiateXIDNegotiation();
                return ConnectionStatus.Success;
            }
            else
            {
                return ConnectionStatus.Busy;
            }
        }

        public ConnectionStatus WaitForConnect()
        {
            return WaitForConnect(false);
        }

        public ConnectionStatus WaitForConnect(Boolean connectOnly)
        {
            //
            // Wait for the link to enter the desired state
            //
            if (connectOnly)
            {
                //
                // Only return if the resulting connected state is Connected
                //
                connectionConnected.WaitOne();
            }
            else
            {
                //
                // Return if the resulting state is either Connected or Disconnected
                //
                connectionFinal.WaitOne();
            }

            if (!Enabled)
            {
                //
                // Connection no longer available
                //
                return ConnectionStatus.Closed;
            }

            if (!isConnected)
            {
                //
                // Connection disconnected
                //
                return ConnectionStatus.NotConnected;
            }
            Support.DbgPrint("Connect: " + _connectedStation.sourceStation.stationIDString + " connected to: " +
                              _connectedStation.destinationStation.stationIDString);
            return ConnectionStatus.Success;
        }

        //
        // ConnecNoWait prototypes
        //
        public ConnectionStatus ConnectNoWait(String destinationStation)
        {
            return ConnectNoWait(destinationStation, "", "");
        }

        public ConnectionStatus ConnectNoWait(String destinationStation, String relayStation1)
        {
            return ConnectNoWait(destinationStation, relayStation1, "");
        }

        public ConnectionStatus ConnectNoWait(String destinationStation, String relayStation1, String relayStation2)
        {
            return ConnectCommon(destinationStation, relayStation1, relayStation2);
        }

        //
        // Standard connect prototypes
        //
        public ConnectionStatus Connect(String destinationStation)
        {
            return Connect(destinationStation, "", "");
        }

        public ConnectionStatus Connect(String destinationStation, String relayStation1)
        {
            return Connect(destinationStation, relayStation1, "");
        }

        public ConnectionStatus Connect(String destinationStation, String relayStation1, String relayStation2)
        {
            ConnectionStatus retStat = ConnectCommon(destinationStation, relayStation1, relayStation2);
            if (retStat.Equals(ConnectionStatus.Success))
            {
                retStat = WaitForConnect();
            }
            return retStat;
        }

        //
        // Common connection routine
        //
        ConnectionStatus ConnectCommon(String destinationStation, String relayStation1, String relayStation2)
        {
            //
            // Attempt to connect to the remote station
            //
            if (!Enabled)
            {
                //
                // Connection no longer available
                //
                return ConnectionStatus.Closed;
            }
            if (relayStation1 != "" || relayStation2 != "")
            {
                Support.PktPrint(" Connect to " + destinationStation + " via " + relayStation1 + " and " + relayStation2 + CRLF, ref LogSyncObject);
            }
            else
            {
                Support.PktPrint(" Connect to " + destinationStation + CRLF, ref LogSyncObject);
            }

            //
            // Clear any existing station info
            //
            _connectedStation.destinationStation.Clear();
            _connectedStation.relayStation1.Clear();
            _connectedStation.relayStation2.Clear();

            _connectedStation.destinationStation.SetCallSign(destinationStation);

            if (_connectedStation.destinationStation.stationIDString.Equals(_connectedStation.sourceStation.stationIDString))
            {
                //
                // Error if caller attempts tp connect to himself
                //
                return ConnectionStatus.NotConnected;
            }

            //
            // Load relay values as needed
            //
            if (relayStation1.Length > 0)
            {
                _connectedStation.relayStation1.SetCallSign(relayStation1);
                if (relayStation2.Length > 0)
                {
                    _connectedStation.relayStation2.SetCallSign(relayStation2);
                }
            }

            //
            // Initiate the link
            //
            ConnectionInit();
            EstablishDataLink(1);
            return ConnectionStatus.Success;
        }

        public void DisconnectNoWait()
        {
            //
            // Send DISC command to the remote station, but don't wait for a response.
            //
            Support.PktPrint(" DisconnectNoWait" + CRLF, ref LogSyncObject);
            DisconnectCommon();
        }

        public ConnectionStatus Disconnect()
        {
            //
            // Send DISC to the remote station, then wait for it to complete disconnecting before returning
            //
            Support.PktPrint(" Disconnect with wait" + CRLF, ref LogSyncObject);
            DisconnectCommon();
            return WaitForDisconnect();
        }

        void DisconnectCommon()
        {
            //
            // Called by upper layer to disconnect from a remote station or to terminate a local listener
            //
            if (connectState.Equals(ConnectionState.Disconnected) || !Enabled)
            {
                return;
            }

            //
            // Build the disconnect frame & ship it off
            //
            Frame.Packet packet = new Frame.Packet(Frame.TransmitFrameBuild(_connectedStation,
                Station.CommandResponse.DoCommand, 1, Frame.FrameTypes.DISCType));

            ProcessLocalPackets(packet);
        }

        public ConnectionStatus WaitForDisconnect()
        {
            ConnectionStatus retStat = ConnectionStatus.Success;

            //
            // Wait for the link to enter a disconnected state
            //
            if (connectState.Equals(ConnectionState.Disconnected) || !Enabled)
            {
                return ConnectionStatus.Closed;
            }

            connectionDisconnected.WaitOne(60000);        // (JNW Jan15)

            if (!Enabled)
            {
                //
                // Connection no longer available
                //
                retStat = ConnectionStatus.Closed;
            }

            return retStat;
        }

        public ConnectionStatus Send(Byte[] buf)
        {
            if (!Enabled)
            {
                //
                // Connection no longer available
                //
                return ConnectionStatus.Closed;
            }

            if (!isConnected)
            {
                //
                // Connection not online
                //
                return ConnectionStatus.NotConnected;
            }

            //
            // Post the data to the send buffer
            //
            toSendQ.Enqueue(buf);
            return ConnectionStatus.Success;
        }

        public void SendPacketSegmenter()
        {
            //
            // This routine segments the inbound frames and queues it for transmission
            //
            Byte[] tmpB;

            Support.DbgPrint(Thread.CurrentThread.Name + " started.");

            while (runFlag)
            {
                //
                // Read the byte buffer
                //
                tmpB = (Byte[])toSendQ.Dequeue();

                if (tmpB == null)
                {
                    //
                    // Null pointer received
                    //
                    continue;
                }

                if (!Enabled)
                {
                    //
                    // Connection no longer available
                    //
                    continue;
                }

                if (!isConnected)
                {
                    //
                    // Connection not online
                    //
                    continue;
                }

                if (tmpB.Length == 0)
                {
                    //
                    // Zero byte buffer received
                    //
                    continue;
                }

                //
                // Connection is online, so send off data.  Note, IFRames do not get fragmented, we just send as many
                // as we need to to get the data across in 'IFrame'-size chunks
                //
                Segmenter segment = new Segmenter(negMaxIFrame, tmpB, Frame.FrameTypes.IType, remoteAX25Version);

                do
                {
                    //
                    // Split the datagram into segments as needed and send them off
                    // Note the buffer returned from GetNextSegment has the appropriate PID byte(s)
                    // prepended to the data.
                    //
                    tmpB = segment.GetNextSegment();
                    Frame.Packet packet = new Frame.Packet(Frame.TransmitFrameBuild(_connectedStation,
                        Station.CommandResponse.DoCommand, 0, Frame.FrameTypes.IType, tmpB));

                    ProcessLocalPackets(packet);
                    _bytesSent += (tmpB.Length - 1);
                    _packetsSent++;

                } while ((segment.segmentsRemaining > 0) && isConnected && Enabled);
                Support.DbgPrint("Segments sent:" + segment.segmentTotal.ToString());
            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exit.");
        }

        public Byte[] RecvAsBuf(Boolean waitFlag)
        {
            Int32 retBytes = 0;
            Int32 i;
            Byte[] retBuf = new Byte[0];

            lock (recvBufferSyncLock)
            {
                if (recvBufferInPtr == recvBufferOutPtr)
                {
                    //
                    // Buffer empty
                    //
                    if (!waitFlag)
                    {
                        //
                        // User chose not to wait
                        //
                        return retBuf;
                    }
                    recvWaiting = true;
                }
            }

            if (recvWaiting)
            {
                //
                // Wait until data shows up
                //
                receiveDataAvailable.WaitOne();
                recvWaiting = false;
            }

            //
            // Return data to the caller
            //
            lock (recvBufferSyncLock)
            {
                retBytes = (recvBufferInPtr - recvBufferOutPtr) & MAXRECVBUFMASK;
                retBuf = new Byte[retBytes];

                for (i = 0; i < retBytes; i++)
                {
                    //
                    // Return data from the ring buffer.  
                    //
                    retBuf[i] = recvBuffer[recvBufferOutPtr];
                    recvBufferOutPtr = (recvBufferOutPtr + 1) & MAXRECVBUFMASK;
                }
            }
            return retBuf;
        }

        public Int32 Recv(Byte[] buf, Boolean waitFlag)
        {
            //
            // Grab bytes from the incoming receive buffer
            //
            Int32 retBytes = 0;
            Int32 i;

            if (buf == null)
            {
                //
                // User passed in null receive buffer
                //
                return 0;
            }

            lock (recvBufferSyncLock)
            {
                if (recvBufferInPtr == recvBufferOutPtr)
                {
                    //
                    // Buffer empty
                    //
                    if (!waitFlag)
                    {
                        //
                        // User chose not to wait
                        //
                        return 0;
                    }
                    recvWaiting = true;
                }
            }

            if (recvWaiting)
            {
                //
                // Wait until data shows up
                //
                receiveDataAvailable.WaitOne();
                recvWaiting = false;
            }

            //
            // Return data to the caller
            //
            lock (recvBufferSyncLock)
            {
                retBytes = (recvBufferInPtr - recvBufferOutPtr) & MAXRECVBUFMASK;
                if (retBytes > buf.Length)
                {
                    retBytes = buf.Length;
                }
                for (i = 0; i < retBytes; i++)
                {
                    //
                    // Return data from the ring buffer.  
                    //
                    buf[i] = recvBuffer[recvBufferOutPtr];
                    recvBufferOutPtr = (recvBufferOutPtr + 1) & MAXRECVBUFMASK;
                }
            }
            return retBytes;
        }

        void ReceivePacketReassember()
        {
            //
            // This routine reassembles the inbound frames and stores the in the receive buffer
            //
            Byte[] tmpB;
            Int32 ptr;
            Int32 j;
            Int32 i;
            Byte[] segBuf; ;
            Int32 chunksRemaining;
            Boolean processingError;

            Support.DbgPrint(Thread.CurrentThread.Name + " started.");

            while (runFlag)
            {

                ptr = 0;
                chunksRemaining = 0;
                tmpB = null;
                processingError = false;

                do
                {
                    //
                    // If this is our first time through, look at the number of packets remaining
                    // to determine how much buffer space we'll need and allocate it.
                    //

                    segBuf = (Byte[])toRecvQ.Dequeue();

                    if (segBuf == null)
                    {
                        //
                        // Getting a null either means the connection disconnected or was terminated.  Either way, 
                        // discard the inprogress buffer & loop
                        //
                        timerSegmenter.Stop();
                        processingError = true;
                        break;
                    }

                    chunksRemaining = segBuf[0] & 0x7f;

                    if ((segBuf[0] & Segmenter.StartOfSegment) == Segmenter.StartOfSegment)
                    {
                        //
                        // If the start of segment bit is set, look at the number of chunks remaining
                        // to determine how much buffer space we'll need and allocate it.
                        //
                        tmpB = new Byte[MAXIFRAME]; // Uuse a big buffer in case the sender sends too large a packet
                        timerSegmenter.Start();
                    }
                    else
                    {
                        if (tmpB == null)
                        {
                            //
                            // Protocol error, start of segment bit not set, so return a null response
                            //
                            timerSegmenter.Stop();
                            ErrorIndication(errorStringZ);
                            processingError = true;
                            break;
                        }
                    }

                    _packetsReceived++;

                    //
                    // Add this packet to the total
                    //
                    j = 1;
                    while (j < segBuf.Length)
                    {
                        tmpB[ptr++] = segBuf[j++];
                    }

                } while (chunksRemaining > 0);
                timerSegmenter.Stop();

                if (!processingError)
                {
                    //
                    // Got the whole segment.  Let's stuff it in the receive buffer
                    //
                    tmpB = Support.PackByte(tmpB, 0, ptr);

                    lock (recvBufferSyncLock)
                    {
                        for (i = 0; i < ptr; i++)
                        {
                            //
                            // Store the data in the ring buffer. By design, if the buffer overflows, 
                            // the old data is overwritten.  If the application using this DLL does not call Recv()
                            // when they should, it's their own fault...
                            //
                            recvBuffer[recvBufferInPtr] = tmpB[i];
                            recvBufferInPtr = (recvBufferInPtr + 1) & MAXRECVBUFMASK;
                            if (recvWaiting)
                            {
                                receiveDataAvailable.Set();
                            }
                        }
                    }
                    _bytesReceived += ptr;
                    DataIndication();
                }
            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exit.");
        }

        public void Insert(Object packet)
        {
            //
            // Drop a packet into the inbound processing Queue
            //
            ConnectionRemoteQ.Enqueue(packet);
        }

        void ProcessLocalPackets(Frame.Packet packet)
        {
            //
            // This routine handles localally created packets. Packets originiating from
            // the local station should be in the AX25Frame.Command object format. 
            //

            if (!Enabled)
            {
                //
                // Connection no longer available
                //
                return;
            }

            if (packet.transmitFrame.AckModeID != 0)            // Set if  Packet is CMD and P set     (JNW Jan15)
                if (AckModeID != 0)                             // Set if Sessioni s using AckMode
                {
                    packet.transmitFrame.AckModeID = AckModeID; // Set real ID into packet

                    //  Extend Timeout to 60- secs

                    timerAck.SetTime(60000);

                }
                else
                    packet.transmitFrame.AckModeID = 0;         // Dont use AckMode Opcode.


            switch (connectState)
            {
                case ConnectionState.Disconnected:
                    DoDisconnectedStateLocal(packet);
                    break;
                case ConnectionState.ConnectPending:
                    DoConnectPendingStateLocal(packet);
                    break;
                case ConnectionState.Connected:
                    DoConnectedStateLocal(packet);
                    break;
                case ConnectionState.DisconnectPending:
                    DoDisconnectPendingStateLocal(packet);
                    break;
                default:
                    break;
            }
        }

        void ProcessQueuedRemotePackets()
        {
            //
            // This routine handles inbound packet.  The Q returns an AX25Frame.Packet object.  Packets from the remote station 
            // are parsed and placed in a parsed AX25 frame format.
            //
            // This routine handles both inbound and outbound packets.  The Q returns an AX25Frame.Packet object.  Packets originiating from
            // the local station should be in the AX25Frame.Command object format.  Packets originating from the remote station are parsed and 
            // placed in a AX25Frame.HDLCFrame format are and the are Packets arriving from the remote host are returned in a
            // parsed format called AX25in a parsed AX25 frame format
            //
            Support.DbgPrint(Thread.CurrentThread.Name + " started.");

            Frame.Packet packet;

            while (runFlag)
            {
                try
                {
                    packet = (Frame.Packet)ConnectionRemoteQ.Dequeue();
                    if (packet == null)
                    {
                        // Loop if null buffer.
                        continue;
                    }
                }
                catch
                {
                    // Loop if we get an exception
                    continue;
                }

                //
                // Set the version of the AX25 protocol the remote station is running
                //
                remoteAX25Version = packet.receivedFrame.version;

                switch (connectState)
                {

                    case ConnectionState.Disconnected:
                        DoDisconnectedStateRemote(packet);
                        break;
                    case ConnectionState.ConnectPending:
                        DoConnectPendingStateRemote(packet);
                        break;
                    case ConnectionState.Connected:
                        DoConnectedStateRemote(packet);
                        break;
                    case ConnectionState.DisconnectPending:
                        DoDisconnectPendingStateRemote(packet);
                        break;
                    default:
                        break;
                }

                if (sendIFrameWaiting)
                {
                    //
                    // Release any IFrame sends that are blocked so they can check the sequence numbers again
                    //
                    sendIFrameWait.Set();
                }
            }

            Support.DbgPrint(Thread.CurrentThread.Name + " exit.");
        }

        //
        // Packet processing routines
        //

        //#region Disconnected State

        //
        // Begin Disconnected state
        //

        void DoDisconnectedStateLocal(Frame.Packet packet)
        {
            //
            // Packet arrived from local station
            //

            //
            // The only thing we should be getting from above is a Connect
            // EstablishDataLink sends the SABM command and sets out stat to connectPending.
            //
            timerAck.SetTime(smoothedRoundTrip * 2, true);
            EstablishDataLink(1);
            return;
        }

        void DoDisconnectedStateRemote(Frame.Packet packet)
        {
            Frame.FrameClasses frameClass = packet.receivedFrame.frameClass;
            Frame.FrameTypes frameType = packet.receivedFrame.frameType;
            Frame.PacketType cmdResp = packet.receivedFrame.cmdResp;
            Int32 pfBit = packet.receivedFrame.pfBit;

            //
            // Packet arrived from remote station 
            //
            if (frameType.Equals(Frame.FrameTypes.SABMType))
            {
                //
                // connect request, to send UA response
                //
                ConnectIndication();
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.UAType, pfBit);

                ClearExceptions();
                InitSequenceNumbers();
                InitTimerAckValue();
                timerAck.SetTime(smoothedRoundTrip * 2, true);
                connectState = ConnectionState.Connected;
                timerPendingAck.Start();
                timerPoll.Start();
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.UIType))
            {
                //
                // if the poll bit is set, send a DM response
                //
                if (pfBit == 1)
                {
                    SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, 1);
                }
                return;
            }

            //
            // Send DM response to all other commands
            //
            if (cmdResp.Equals(Frame.PacketType.Command))
            {
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, pfBit);
            }
        }

        //
        // End Disconnected state
        //

        //#endregion

        //#region ConnectPending State

        //
        // Begin ConnectPending state
        //

        void DoConnectPendingStateLocal(Frame.Packet packet)
        {
            //
            // Packet arrived from local station
            //
            if (packet.transmitFrame.frameType.Equals(Frame.FrameTypes.SABMType))
            {
                //
                // Send incoming connect commands to the link provider
                //
                dataLinkProvider.Send(packet);
            }

            if (packet.transmitFrame.frameType.Equals(Frame.FrameTypes.DISCType))
            {
                //
                // Disconnect request frmo above before we connected.  Just shutdown.
                //
                connectState = ConnectionState.Disconnected;
            }
        }

        void DoConnectPendingStateRemote(Frame.Packet packet)
        {
            Frame.FrameClasses frameClass = packet.receivedFrame.frameClass;
            Frame.FrameTypes frameType = packet.receivedFrame.frameType;
            Int32 pfBit = packet.receivedFrame.pfBit;

            //
            // Packet arrived from remote station
            //
            if (frameType.Equals(Frame.FrameTypes.UAType))
            {
                if (pfBit == 1)
                {
                    timerAck.SetTime(smoothedRoundTrip * 2, true);
                    SelectTimerAckValue();
                    connectState = ConnectionState.Connected;
                    timerPendingAck.Start();
                }
                else
                {
                    ErrorIndication(errorStringD);
                }
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.DMType) ||
                frameType.Equals(Frame.FrameTypes.FRMRType))
            {
                if (pfBit == 1)
                {
                    //
                    // DM or FRMR response received, so terminate connection
                    //
                    ErrorIndication(errorStringK);
                    connectState = ConnectionState.Disconnected;
                }
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.SABMType))
            {
                //
                // Connect request, so send UA response
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.UAType, pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.SABMEType))
            {
                //
                // Extended Connect request, send FRMR response as we don't support this yet
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.FRMRType, pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.DISCType))
            {
                //
                // Disconnect request, so send DM response
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.UIType))
            {
                //
                // if the poll bit is set, send a DM response
                //
                if (pfBit == 1)
                {
                    SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, 1);
                }
                return;
            }
        }

        //
        // End ConnectPending state
        //

        //#endregion

       // #region DisconnectPending State

        //
        // Begin DisconnectPending state
        //

        void DoDisconnectPendingStateLocal(Frame.Packet packet)
        {
            //
            // Packet arrived from local station (Should be complete)
            // 
            if (packet.transmitFrame.frameType.Equals(Frame.FrameTypes.DISCType))
            {
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, 1);
                connectState = ConnectionState.Disconnected;
            }
        }

        void DoDisconnectPendingStateRemote(Frame.Packet packet)
        {
            //
            // Packet arrived from remote station (Should be complete)
            //
            Frame.ReceivedFrame parsedFrame = packet.receivedFrame;
            Frame.FrameClasses frameClass = packet.receivedFrame.frameClass;
            Frame.FrameTypes frameType = packet.receivedFrame.frameType;
            Frame.PacketType cmdResp = packet.receivedFrame.cmdResp;
            Int32 pfBit = packet.receivedFrame.pfBit;

            if (frameType.Equals(Frame.FrameTypes.UAType) ||
                frameType.Equals(Frame.FrameTypes.DMType))
            {
                //
                // Disconnect or UA reponse.  Complete disconnect if pfBit is set
                //
                if (pfBit == 1)
                {
                    connectState = ConnectionState.Disconnected;
                }
                else if (frameType.Equals(Frame.FrameTypes.UAType))
                {
                    ErrorIndication(errorStringD);
                }

                return;
            }

            if (frameType.Equals(Frame.FrameTypes.DISCType))
            {
                //
                // Disconnect request, send UA response
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.UAType, pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.UIType))
            {
                //
                // if the poll bit is set, send a DM response
                //
                if (pfBit == 1)
                {
                    SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, 1);
                }
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.SABMType))
            {
                //
                // Connect request, send DM response
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.SABMEType))
            {
                //
                // Extended Connect request, send FRMR response until we support this
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.FRMRType, pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameClasses.IClass) ||
                frameType.Equals(Frame.FrameClasses.SClass))
            {
                if ((pfBit == 1) && (cmdResp.Equals(Frame.PacketType.Command)))
                {
                    //
                    // All I and S Frame commands get DMed
                    //
                    SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.DMType, pfBit);
                }
                return;
            }
        }

        //
        // End Disconnected Pending State
        //

        //#endregion

        //#region Connected State

        //
        // Begin Connected state
        //

        void DoConnectedStateLocal(Frame.Packet packet)
        {
            //
            // Packet arrived from local station
            //
            if (packet.transmitFrame.frameType.Equals(Frame.FrameTypes.DISCType))
            {
                connectState = ConnectionState.DisconnectPending;
                dataLinkProvider.Send(packet);
                timerPoll.Stop();
                timerAck.Start();
                return;
            }

            if (packet.transmitFrame.frameType.Equals(Frame.FrameTypes.SABMType))
            {
                EstablishDataLink(1);
                return;
            }

            if (packet.transmitFrame.frameType.Equals(Frame.FrameTypes.IType))
            {
                //
                // We are sending out an IFrame so stop the PendingAck
                //
                do
                {
                    if (!isConnected || !Enabled)
                    {
                        //
                        // Connection went away, so close down
                        //
                        timerPendingAck.Stop();
                        return;
                    }

                    lock (frameSyncLock)
                    {
                        if ((((vSend - vAck) & parameterBuf.seqNumMask) == negMaxWindowSize) || remoteBusy || pollSent) // (JNW Jan15)
                        {
                            //
                            // Buffer full or remote is busy or waiting for RR(F)
                            //
                            sendIFrameWaiting = true;
                        }
                        else
                        {
                            sendIFrameWaiting = false;
                        }
                    }
                    if (sendIFrameWaiting)
                    {
                        //
                        // We've reached the limit of the number of outstanding iFrames.  Wait until some have been 
                        // acknowledged
                        //
                        sendIFrameWait.WaitOne();
                    }
                } while (sendIFrameWaiting);

                lock (frameSyncLock)
                {
                    //
                    // Send off the iFrame
                    //
                    //timerPendingAck.Stop();
                    vRecvAck = vRecv;
                    Frame.AddSequence(ref packet.transmitFrame, vRecv, vSend);
                    packetsInProgress[vSend] = packet;
                    //dataLinkProvider.Send(packet);
                    vSend = (vSend + 1) & parameterBuf.seqNumMask;

                    //  If at window, set P bit  if enabled            (JNW Jan15)

                    if (negUseIPoll)
                    {
                        if (((vSend - vAck) & parameterBuf.seqNumMask) == negMaxWindowSize)
                        {
                            Frame.AddPBit(ref packet.transmitFrame);
                            pollSent = true;

                            if (AckModeID != 0)                             // Set if Session is using AckMode
                            {
                                packet.transmitFrame.AckModeID = AckModeID; // Set real ID into packet
                 
                                //  Extend Timeout to 60 secs

                                timerAck.SetTime(60000);
                            }
                            else
                                packet.transmitFrame.AckModeID = 0;         // Dont use AckMode Opcode.

                            inTimerRecovery = true;                         // So we ignore RR without F
                        }
                    }
        
                    if (!timerAck.Enabled)
                    {
                        timerPoll.Stop();
                        timerAck.Start();
                    }
                }
                dataLinkProvider.Send(packet);
            }
        }

        void DoConnectedStateRemote(Frame.Packet packet)
        {
            //
            // Packet arrived from remote station
            //

            Frame.FrameClasses frameClass = packet.receivedFrame.frameClass;
            Frame.FrameTypes frameType = packet.receivedFrame.frameType;
            Frame.PacketType cmdResp = packet.receivedFrame.cmdResp;
            Int32 pfBit = packet.receivedFrame.pfBit;

            if (frameType.Equals(Frame.FrameTypes.XIDType))
            {
                //
                // Received an XID command/response
                //
                NegotiateXIDParameters(packet.receivedFrame);
                return;
            }

            //
            // Got a packet from the remote station, so stop the poll timer.
            //
            timerPoll.Stop();

            //
            // Check for UFrames first
            // 
            if (frameType.Equals(Frame.FrameTypes.DISCType))
            {
                //
                // Disconnect request, so send UA response
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.UAType, pfBit);
                DisconnectIndication();
                connectState = ConnectionState.Disconnected;
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.DMType))
            {
                //
                // DM response received, so complete abort connection
                //
                ErrorIndication(errorStringE);
                connectState = ConnectionState.Disconnected;
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.UAType))
            {
                //
                // Received UA command, reestablish the link
                //
                ErrorIndication(errorStringC);
                EstablishDataLink(pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.FRMRType))
            {
                //
                // Received FRMR command
                //
                ErrorIndication(errorStringK);
                EstablishDataLink(pfBit);
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.SABMType))
            {
                //
                // Received SABM command, Initialize the link
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.UAType, pfBit);
                ClearExceptions();
                ErrorIndication(errorStringF);
                lock (frameSyncLock)
                {
                    if (vSend != vAck)
                    {
                        ConnectIndication();
                    }
                }
                timerAck.Stop();
                timerPoll.Start();

                //
                // N6PRW
                // Change from AX.25 spec.  If we send UA response and the remote tnc does not 
                // receive it,  then any iFrames we previously sent are lost.  What we'll 
                // do instead is to resend unacked iFrames.
                //
                InvokeRetransmission("Received SABM command, Initialize the link");

                //InitSequenceNumbers();

                return;
            }

            if (frameType.Equals(Frame.FrameTypes.SABMEType) ||
                (frameType.Equals(Frame.FrameTypes.SREJType) &&
                (cmdResp.Equals(Frame.PacketType.Command))))
            {
                //
                // Received SABME or SREJ command, Send FRMR response until we support
                //
                if (packet.receivedFrame.cmdResp.Equals(Frame.PacketType.Command))
                {
                    SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.FRMRType, pfBit);
                }
                return;
            }

            if (frameType.Equals(Frame.FrameTypes.UIType))
            {
                //
                // if the poll bit is set, send inquiry response
                //
                if (pfBit == 1)
                {
                    EnquiryResponse(1);
                }
                return;
            }

            //#region ICLass handling (IFrames)

            //
            // Update on 02-15-2010 by N6PRW
            // Updated the IClass handler to correct some timer recovery issues         
            //
            if (frameClass.Equals(Frame.FrameClasses.IClass))
            {
                //
                // Received IFrame from remote station
                //
                if (packet.receivedFrame.cmdResp == Frame.PacketType.Response)
                {
                    // 
                    // Ignore Iframes with respose bit set
                    //
                    ErrorIndication(errorStringS);
                    return;
                }

                //
                // Reset the link if send sequencing is bad
                //
                if (ErrorRecovery(packet.receivedFrame.numR))
                {
                    return;
                }

                //
                // Update on 02-15-2010 by N6PRW
                // Check acknowledgement of iframes
                //
                CheckIFrameAcked(packet.receivedFrame.numR);

                if (localBusy)
                {
                    //
                    // If this was a command with the poll bit set, send back an RNR
                    //
                    if (pfBit == 1)
                    {
                        EnquiryResponse(1);
                    }
                    return;
                }

                if (vRecv == packet.receivedFrame.numS)
                {
                    //
                    // Valid receive frame sequence.  Clear the exception
                    //
                    REJSent = false;

                    lock (frameSyncLock)
                    {
                        vRecv = (vRecv + 1) & parameterBuf.seqNumMask;
                    }
                    //
                    // Received a frame, so send it up and start the pendingack timer
                    //
                    toRecvQ.Enqueue(packet.receivedFrame.iBuf);
                    
                    if (pfBit == 1)                 // Should always reply to a I(P)     (JNW Feb15)
                    {
                      EnquiryResponse(1);
 //                       timerPendingAck.Stop();
                    }
 
                    timerPendingAck.Restart();
                }
                else
                {
                    if (REJSent)
                    {
                        //
                        // If this was a command with the poll bit set, send back an approrpriate response
                        //
                        CheckNeedForResponse(cmdResp, pfBit);
                    }
                    else
                    {
                        REJSent = true;
                        SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.REJType, pfBit);
                    }
                    //
                    // Removed on 02-15-2010 by N6PRW
                    // Return should be placed outside the else clause. (see below)
                    //return;
                }
                //
                // Added on 02-15-2010 by N6PRW
                // This bug was detected when a remote PTC-II connected via a digipeater.  We were sending
                // back unexpected RR responses with pf=0.  This caused the remote PTC-II to retransmit
                // backets we had already received.  Moving the return statement here corrects the issue
                //
                return;
            }

            //
            // End of ICLASS section
            //

            //#endregion

            //#region SClass Command handling (RR, RNR, REJ)
            //
            // Update on 02-15-2010 by N6PRW
            // Rewrote the SClass handler (RR, RNR, REJ, SREJ) to correct some timer recovery issues         
            //

            if (frameClass.Equals(Frame.FrameClasses.SClass))
            {
                //
                // SFrame processing
                //
                if (frameType.Equals(Frame.FrameTypes.RNRType))
                {
                    remoteBusy = true;
                }
                else
                {
                    remoteBusy = false;
                }
    
                //  if stopped after sending I(P), release      (JNW Jan15)

                if (pollSent) 
                    // This DLL doesn't repond to I(P), so cancel on any RR  (JNW Feb15)
                    if (cmdResp.Equals(Frame.PacketType.Response))   
 //                       if ((pfBit == 1) && cmdResp.Equals(Frame.PacketType.Response))
                       pollSent = false;           // Cancel I(P) sent flag

                if (inTimerRecovery)
                {
                    //
                    // Handle timer recovery for RR, RNR and REJ types
                    //
                    if ((pfBit == 1) && cmdResp.Equals(Frame.PacketType.Response))
                    {
                        //
                        // Handle Response from remote with pf=1
                        //

                        // Stop Timer T1 (Ack timer)
                        //
                        timerAck.Stop();
                        InitTimerAckValue();
                        if (ErrorRecovery(packet.receivedFrame.numR))
                        {
                            return;
                        }

                        lock (frameSyncLock)
                        {
                            //
                            // Update the ack counter
                            //
                            vAck = packet.receivedFrame.numR;
                        }
                        
                        if (vSend == vAck)
                        {
                            //
                            // We are all caught up. Exit timer recovery
                            //
                            timerPoll.Restart();
                            inTimerRecovery = false;
                            retryCount = 0;                     // (JNW Feb 2015)
                            return;
                        }

                        
                        //
                        // Resend the unacked packets
                        //
                        InvokeRetransmission("In timer recovery");
                        return;
                    }

                    if ((pfBit == 1) && cmdResp.Equals(Frame.PacketType.Command))
                    {
                        //
                        // Handle Command from remote with pf=1
                        // Send a response to the enquiry
                        //
                        EnquiryResponse(1);
                    }

                    if (pfBit == 0)
                    {
                        // Super without F received in Timer Recovery - Ignore    JNW Mar 15

                        return;
                    }

                    if (ErrorRecovery(packet.receivedFrame.numR))
                    {
                        return;
                    }
                    //
                    // Update vAck
                    //
                    lock (frameSyncLock)
                    {
                        vAck = packet.receivedFrame.numR;
                    }

                    if (frameType.Equals(Frame.FrameTypes.REJType))
                    {
                        //
                        // For REJ Commands, resend the unacked packets
                        //
                        InvokeRetransmission("Received REJ command in timer recovery");
                    }
                    //
                    // End of Timer recovery
                    //
                    return;
                }

                //
                // Not in timer recovery
                //
                // Check if a response is required
                //
                CheckNeedForResponse(cmdResp, pfBit);

                //
                // Reset the link if send sequencing is bad
                //
                if (ErrorRecovery(packet.receivedFrame.numR))
                {
                    return;
                }

                if (frameType.Equals(Frame.FrameTypes.REJType))
                {
                    //
                    // It's a reject type
                    //
                    lock (frameSyncLock)
                    {
                        //
                        // Update the Ack counter
                        //
                        vAck = packet.receivedFrame.numR;
                    }
                    timerAck.Stop();
                    timerPoll.Stop();
                    InitTimerAckValue();
                    InvokeRetransmission("Received REJ. Not in timer recovery.");
                    return;
                }

                //
                // RR or RNR
                //
                CheckIFrameAcked(packet.receivedFrame.numR);
                return;
            }

            //
            // End of SClass
            //

            //#endregion

        }

        //
        // End Connected state
        //

        //#endregion

        //#region Connection Support Routines

        void SendToDlp(Station.CommandResponse cr, Frame.FrameTypes ft, Int32 pfBit)
        {
            SendToDlp(cr, ft, pfBit, null);
        }

        void SendToDlp(Station.CommandResponse cr, Frame.FrameTypes ft, Int32 pfBit, Byte[] buf)
        {
            Int32 tmpI = (Int32)ft;
            Frame.Packet tmpP = new Frame.Packet();
            Frame.TransmitFrame tmpC = null;
            if (buf == null)
            {
                tmpC = Frame.TransmitFrameBuild(_connectedStation, cr, pfBit, ft);
            }
            else
            {
                tmpC = Frame.TransmitFrameBuild(_connectedStation, cr, pfBit, ft, buf);
            }

            if (tmpC.AckModeID != 0)            // Set if  Packet is CMD and P set     (JNW Jan15)
                if (AckModeID != 0)             // Set if Session is using AckMode
                {
                    tmpC.AckModeID = AckModeID; // Set real ID into packet
                
                    //  Extend Timeout to 60- secs

                    timerAck.SetTime(60000);
                }

                else
                    tmpC.AckModeID = 0;         // Dont use AckMode Opcode.

            lock (frameSyncLock)
            {
                if ((tmpI & 0x01) == 0)
                {
                    Frame.AddSequence(ref tmpC, vRecv, vSend);
                    vRecvAck = vRecv;
                    //timerPendingAck.Stop();
                }
                if ((tmpI & 0x3) == 1)
                {
                    Frame.AddSequence(ref tmpC, vRecv);
                    vRecvAck = vRecv;
                    //timerPendingAck.Stop();
                }
                //tmpP.transmitFrame = tmpC;
                //dataLinkProvider.Send(tmpP);
            }
            tmpP.transmitFrame = tmpC;
            dataLinkProvider.Send(tmpP);
        }

        void EstablishDataLink(Int32 pfBit)
        {
            //
            // Build connection command with poll bit set. 
            // Set our connection state to ConnectPending & send off the request
            //
            Frame.Packet pkt = new Frame.Packet(Frame.TransmitFrameBuild(_connectedStation,
                Station.CommandResponse.DoCommand, pfBit, Frame.FrameTypes.SABMType));

            if (DoSABMEConnect)
            {
                pkt = new Frame.Packet(Frame.TransmitFrameBuild(_connectedStation,
                Station.CommandResponse.DoCommand, pfBit, Frame.FrameTypes.SABMEType));
            }

            connectState = ConnectionState.ConnectPending;
            retryCount = 0;
            ClearExceptions();
            InitSequenceNumbers();
            timerPendingAck.Stop();
            timerPoll.Stop();
            timerAck.Restart();
            timerXID.Stop();

            ProcessLocalPackets(pkt);
        }

        void InvokeRetransmission(String reason)
        {
            lock (frameSyncLock)
            {
                Support.DbgPrint("Invoke retransmission: " + reason);
                Support.PktPrint(" Invoke retransmission: " + reason + CRLF, ref LogSyncObject);
                Int32 numResend = ((vSend - vAck) & parameterBuf.seqNumMask);
                if (numResend > 0)
                {
                    Support.DbgPrint("Resend " + numResend.ToString() + " frames");
                    Support.PktPrint(" Resend " + numResend.ToString() + " frames" + CRLF, ref LogSyncObject);
                }
                // W4PHS 25-Jul-2012.  Start timer in case retransmission gets lost.
                if (!timerAck.Enabled)
                {
                    timerPoll.Stop();
                    timerAck.Start();
                }
                Int32 j = vAck;
                for (Int32 i = 0; i < ((vSend - vAck) & parameterBuf.seqNumMask); i++)
                {
                    vRecvAck = vRecv;
                    Frame.Packet tmpP = packetsInProgress[j];
                    Frame.AddSequence(ref tmpP.transmitFrame, vRecv, j);
                    Frame.ClearPBit(ref tmpP.transmitFrame);           // (JNW Jan17)

                    j = (j + 1) & parameterBuf.seqNumMask;

                    //  If at window, set P bit  if enabled            (JNW Jan17)

                    if (negUseIPoll)
                    {
                        if (((j - vAck) & parameterBuf.seqNumMask) == negMaxWindowSize)
                        {
                            Frame.AddPBit(ref tmpP.transmitFrame);
                            pollSent = true;
                            tmpP.transmitFrame.AckModeID = 0;         // Dont use AckMode Opcode.
                            inTimerRecovery = true;                   // So we ignore RR without F
                        }
                    }              

                    //timerPendingAck.Stop();
                    dataLinkProvider.Send(tmpP);
                }
            }
        }

        void CheckIFrameAcked(Int32 numR)
        {
            if (remoteBusy)
            {
                //
                // Remote station is busy, insure ack timer running
                //
                lock (frameSyncLock)
                {
                    vAck = numR;
                }

                if (!timerAck.Enabled)
                {
                    timerAck.Start();
                }
            }
            else
            {
                //
                // Remote is not busy
                //
                if (vSend != numR)
                {
                    //
                    // Not all of our outstanding IFrames have been acked.
                    //
                    if (vAck != numR)
                    {
                        //
                        // Although not all were acked, some additional ones were, so restart the ACK timer
                        //
                        lock (frameSyncLock)
                        {
                            vAck = numR;
                        }
                        timerAck.Restart();
                    }
                }
                else
                {
                    //
                    // All outstanding IFrames ACked, so stop the ACK timer & start the Poll timer
                    //
                    lock (frameSyncLock)
                    {
                        vAck = numR;
                    }
                    timerAck.Stop();
                    timerPoll.Start();
                    SelectTimerAckValue();
                    //
                    // Added on 02-15-2010 by N6PRW
                    // Reset the timer recovery flag
                    //
                    inTimerRecovery = false;
                }
            }
        }

        void TransmitInquiry()
        {
            if (localBusy)
            {
                SendToDlp(Station.CommandResponse.DoCommand, Frame.FrameTypes.RNRType, 1);
            }
            else
            {
                SendToDlp(Station.CommandResponse.DoCommand, Frame.FrameTypes.RRType, 1);
            }
            timerAck.Start();
        }

        void EnquiryResponse(Int32 pfBit)
        {
            if (localBusy)
            {
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.RNRType, pfBit);
            }
            else
            {
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.RRType, pfBit);
            }
        }

        void CheckNeedForResponse(Frame.PacketType packetType, Int32 pfBit)
        {
            if (packetType.Equals(Frame.PacketType.Command) && (pfBit == 1))
            {
                EnquiryResponse(pfBit);
            }
            else if (pfBit == 1)
            {
                // W4PHS ErrorIndication(errorStringA);
            }
        }

        void DataIndication()
        {
            CommonIndication(IndicationTypes.Data, "");
        }

        void ConnectIndication()
        {
            Support.DbgPrint("Connect indication");
            Support.PktPrint(" Connect indication" + CRLF, ref LogSyncObject);
            CommonIndication(IndicationTypes.ConnectRequest, "");
        }

        void DisconnectIndication()
        {
            Support.DbgPrint("Disconnect indication");
            Support.PktPrint(" Disconnect indication" + CRLF, ref LogSyncObject);
            CommonIndication(IndicationTypes.DisconnectRequest, "");
        }

        void ErrorIndication(String e)
        {
            Support.DbgPrint("Error indication: " + e);
            Support.PktPrint(" Error indication: " + e + CRLF, ref LogSyncObject);
            CommonIndication(IndicationTypes.Error, e);
        }

        void CommonIndication(IndicationTypes it, String data)
        {
            //
            // Tell any registered listeners of the event indication
            //
            registerIndicationHandlers.DoEvent(it,
               connectedStation.destinationStation.stationIDString,
               connectedStation.sourceStation.stationIDString,
               connectedStation.relayStation1.stationIDString,
               connectedStation.relayStation2.stationIDString,
               data, connectionID);
        }

        void ClearExceptions()
        {
            REJSent = false;
        }

        void InitSequenceNumbers()
        {
            lock (frameSyncLock)
            {
                vAck = 0;
                vSend = 0;
                vRecv = 0;
                vRecvAck = 0;
            }
        }

        void InitTimerAckValue()
        {
            smoothedRoundTrip = negAckTimer * ((2 * _connectedStation.numRelays) + 1);
            //roundTripTimeDeviation = 0;
        }

        void SelectTimerAckValue()
        {
            //
            // Currently not used.
            //
            // Select the new timer value for the ACK timer.  The timer algorithms used here are pulled from the
            // AX.25 protocol spec.
            //
            //if (retryCount == 0)
            //{
            //    //
            //    // This transaction has not been retried
            //    //
            //    smoothedRoundTrip = ((7 * smoothedRoundTrip) + rttElapsed + 10) >> 3;
            //    timerAck.SetTime(smoothedRoundTrip * 2);
            //}
            //else
            //{
            //    //
            //    // We're in the middle of our retry attempts
            //    //
            //    if (timerAck.expired)
            //    {
            //        //
            //        // Timer expired, so set next ack timer value based on the retry count
            //        //
            //        timerAck.SetTime((2 * (retryCount + 1)) * smoothedRoundTrip);
            //    }
            //}
        }

        Boolean ErrorRecovery(Int32 numR)
        {
            lock (frameSyncLock)
            {
                if (((vSend - vAck) & parameterBuf.seqNumMask) >= ((vSend - numR) & parameterBuf.seqNumMask))
                {
                    //
                    // The sequence number are OK, so just return
                    //
                    return false;
                }
            }
            //
            // Send Sequencing bad, restart connection
            //
            Support.DbgPrint("Send Sequencing bad, restart connection");
            Support.PktPrint(" Send Sequencing bad, restart connection" + CRLF, ref LogSyncObject);
            EstablishDataLink(1);
            return true;
        }

        //#endregion

        //#region XID Negotiation

        void InitiateXIDNegotiation()
        {
            //
            // Send an XID command to the remote station.  If a negotiation is already in progress, just return
            //
            if (!XIDInProgress)
            {
                XIDInProgress = true;
                timerXID.Start();
                //
                // Build the XID command
                //
                Frame.Packet tmpP = new Frame.Packet(Frame.XIDInfo.Encode(_connectedStation, parameterBuf,
                    Station.CommandResponse.DoCommand, 1));

                //
                // Ship it off
                //
                dataLinkProvider.Send(tmpP);
            }
        }

        void NegotiateXIDParameters(Frame.ReceivedFrame frameInfo)
        {
            if (frameInfo.pfBit == 1)
            {
                if (frameInfo.cmdResp.Equals(Frame.PacketType.Command))
                {
                    //
                    // Do the negotiation 
                    //
                    Frame.XIDInfo xidFrame = frameInfo.xidFrame;

                    ConnectionParameterBuf tmpPBuf = GetParamBuf(frameInfo);       // Get negotiated values

                    //
                    // Update our parameters with the negotiated values.
                    //
                    negAckTimer = tmpPBuf.ackTimer;
                    InitTimerAckValue();
                    timerAck.SetTime(smoothedRoundTrip * 2, true);
                    negMaxIFrame = tmpPBuf.maxIFrame;
                    negMaxRetry = tmpPBuf.maxRetry;
                    negMaxWindowSize = tmpPBuf.maxWindowSize;

                    //
                    // Build the response
                    //
                    Frame.Packet tmpP = new Frame.Packet(Frame.XIDInfo.Encode(_connectedStation, parameterBuf,
                        Station.CommandResponse.DoResponse, 1));

                    //
                    // Ship it off
                    //
                    dataLinkProvider.Send(tmpP);

                    StringBuilder sb = new StringBuilder("Successful remote init XID negotiation:" + CRLF);
                    sb.Append("  maxIFrame = " + negMaxIFrame.ToString() + CRLF);
                    sb.Append("  maxWindowSize = " + negMaxWindowSize.ToString() + CRLF);
                    sb.Append("  maxAckTimer = " + negAckTimer.ToString() + CRLF);
                    sb.Append("  maxRetry = " + negMaxRetry.ToString() + CRLF);

                    Support.DbgPrint(sb.ToString());
                    Support.PktPrint(sb.ToString() + CRLF, ref LogSyncObject);
                    return;
                }
                else
                {
                    //
                    // Response from XID
                    // 
                    if (XIDInProgress == true)
                    {
                        //
                        // Got a response back. Update our negotiated parameters
                        //

                        timerXID.Stop();
                        ConnectionParameterBuf tmpPBuf = GetParamBuf(frameInfo);
                        negAckTimer = tmpPBuf.ackTimer;
                        InitTimerAckValue();
                        timerAck.SetTime(smoothedRoundTrip * 2, true);
                        negMaxIFrame = tmpPBuf.maxIFrame;
                        negMaxRetry = tmpPBuf.maxRetry;
                        negMaxWindowSize = tmpPBuf.maxWindowSize;

                        //
                        // Negotiation complete
                        //
                        XIDInProgress = false;
                        StringBuilder sb = new StringBuilder("Successful local init XID negotiation:" + CRLF);
                        sb.Append("  maxIFrame = " + negMaxIFrame.ToString() + CRLF);
                        sb.Append("  maxWindowSize = " + negMaxWindowSize.ToString() + CRLF);
                        sb.Append("  maxAckTimer = " + negAckTimer.ToString() + CRLF);
                        sb.Append("  maxRetry = " + negMaxRetry.ToString() + CRLF);

                        Support.DbgPrint(sb.ToString());
                        Support.PktPrint(sb.ToString() + CRLF, ref LogSyncObject);
                        return;
                    }
                }
                //
                // We received an XID command w/o PF Bit, or we were not in the negotiation phase.
                // We'll send back a FRMR 
                //
                SendToDlp(Station.CommandResponse.DoResponse, Frame.FrameTypes.FRMRType, frameInfo.pfBit);
                return;

            }
        }

        ConnectionParameterBuf GetParamBuf(Frame.ReceivedFrame frameInfo)
        {
            //
            // Create a tmp connection param buf to handle negotiated 
            //
            Frame.XIDInfo.PLV p;
            Frame.XIDInfo xidFrame = frameInfo.xidFrame;

            ConnectionParameterBuf tmpPBuf = new ConnectionParameterBuf();

            //
            // Update the copy with any changes from the original
            //
            tmpPBuf.maxIFrame = parameterBuf.maxIFrame;
            tmpPBuf.maxRetry = parameterBuf.maxRetry;
            tmpPBuf.maxWindowSize = parameterBuf.maxWindowSize;
            tmpPBuf.ackTimer = parameterBuf.ackTimer;

            //
            // Digest the requested params from the remote station
            //
            p = xidFrame.GetPLV(Frame.XIDInfo.ParameterIndicator.RXIFieldLen);
            if (p != null)
            {
                if ((p.PV / 8) < parameterBuf.maxIFrame)
                {
                    tmpPBuf.maxIFrame = p.PV / 8;
                }
            }

            p = xidFrame.GetPLV(Frame.XIDInfo.ParameterIndicator.RXWindowSize);
            if (p != null)
            {
                if (p.PV < parameterBuf.maxWindowSize)
                {
                    tmpPBuf.maxWindowSize = p.PV;
                }
            }

            p = xidFrame.GetPLV(Frame.XIDInfo.ParameterIndicator.AckTimer);
            if (p != null)
            {
                if (p.PV < parameterBuf.ackTimer)
                {
                    tmpPBuf.ackTimer = p.PV;
                }
            }

            p = xidFrame.GetPLV(Frame.XIDInfo.ParameterIndicator.NumRetries);
            if (p != null)
            {
                if (p.PV < parameterBuf.maxRetry)
                {
                    tmpPBuf.maxRetry = p.PV;
                }
            }

            return tmpPBuf;
        }

        //#endregion

        public class ConnectionParameterBuf
        {
            const Int32 REJCmdRsp = 0x02;
            const Int32 SREJCmdRsp = 0x04;
            const Int32 Mod8 = 0x40;
            const Int32 Mod128 = 0x80;

            //
            // Fixed operational functions always used
            //
            const Int32 opFuncByte0 = 0x80;  // Extended Address
            const Int32 opFuncByte1 = 0xA0;  // TEST cmd/rsp, 16 bit FCS
            const Int32 opFuncByte2 = 0x02;  // Synchronous TX

            // Default connection Poll Threshold 60000 ms (1 minute) 
            public const Int32 defPollThresh = (1000 * 60);

            // Default Max iFrame is 128 octets (1024 bits)
            public const Int32 defMaxIFrame = 128;

            // Default Max windows size is 4 frames (i.e. # of outstanding buffers)
            public const Int32 defMaxWindowSize = 0x4;

            // Default Timeout 30000ms (3 sec)
            public const Int32 defAckTimer = 3000;

            // Default PendingAckTimer (500 msec)
            public const Int32 defPendingAckTimer = 500;

            // Default Max retry = 10
            public const Int32 defMaxRetry = 10;

            Int32 _maxRetry = defMaxRetry;
            public Int32 maxRetry
            {
                get { return _maxRetry; }
                set
                {
                    if ((value >= 0) && (value <= 20))
                    {
                        _maxRetry = value;
                    }
                }
            }

            Int32 _maxIFrame = defMaxIFrame;
            public Int32 maxIFrame
            {
                get { return _maxIFrame; }
                set
                {
                    if ((value >= 2) && (value <= MAXIFRAME))
                    {
                        _maxIFrame = value;
                    }
                }
            }

            Int32 _maxWindowSize = defMaxWindowSize;
            public Int32 maxWindowSize
            {
                set
                {
                    Int32 pMax = 7;
                    if (_seqNumMode.Equals(Frame.SequenceNumberMode.Mod128))
                    {
                        pMax = 127;
                    }
                    if ((value >= 0) && (value <= pMax))
                    {
                        _maxWindowSize = value;
                    }
                }
                get { return _maxWindowSize; }
            }

            Int32 _ackTimer = defAckTimer;
            public Int32 ackTimer
            {
                get { return _ackTimer; }
                set
                {
                    if ((value >= 3000) && (value < 1000 * 60 * 60 * 24))
                    {
                        _ackTimer = value;
                    }
                }
            }

            Int32 _pendingAckTimer = defPendingAckTimer;
            public Int32 pendingAckTimer
            {
                get { return _pendingAckTimer; }
                set
                {
                    if ((value >= 100) && (value <= (_ackTimer / 2)))
                    {
                        _pendingAckTimer = value;
                    }
                }
            }

            Int32 _pollThresh = defPollThresh;
            public Int32 pollThresh
            {
                get { return _pollThresh; }
                set
                {
                    if ((value > (_ackTimer << 1)) && (value <= (1000 * 60 * 60 * 24)))     // max is 24 hours
                    {
                        _pollThresh = value;
                    }
                }

            }

            Int32 _optionalFuncByte0 = opFuncByte0 | REJCmdRsp;
            public Int32 optionalFuncByte0
            {
                get { return _optionalFuncByte0; }
            }

            Int32 _optionalFuncByte1 = opFuncByte1 | Mod8;
            public Int32 optionalFuncByte1
            {
                get { return _optionalFuncByte1; }
            }

            public Int32 optionalFuncByte2
            {
                get { return opFuncByte2; }
            }

            Boolean _permitREJCmdRsp = true;
            public Boolean permitREJCmdRsp
            {
                get { return _permitREJCmdRsp; }
                //
                // Not currently modifyable
                //
                //set
                //{
                //    _permitREJCmdRsp = value;
                //    if (value)
                //    {
                //        _optionalFuncByte0 |= REJCmdRsp;
                //    }
                //    else
                //    {
                //        _optionalFuncByte0 &= ~REJCmdRsp;
                //    }
                //}
            }

            Boolean _permitSREJCmdRsp = false;
            public Boolean permitSREJCmdRsp
            {
                get { return _permitSREJCmdRsp; }
                //
                // Not currently modifyable
                //
                //set
                //{
                //    _permitSREJCmdRsp = value;
                //    if (value)
                //    {
                //        _optionalFuncByte0 |= SREJCmdRsp;
                //    }
                //    else
                //    {
                //        _optionalFuncByte0 &= ~SREJCmdRsp;
                //    }
                //}
            }

            Frame.SequenceNumberMode _seqNumMode = Frame.SequenceNumberMode.Mod8;
            public Frame.SequenceNumberMode seqNumMode
            {
                get { return _seqNumMode; }
                //
                // Not currently modifyable
                //
                //set
                //{
                //    _seqNumMode = value;
                //    if (value.Equals(AX25Frame.SequenceNumberMode.Mod8))
                //    {
                //        //
                //        // If shifting into mod 8 mode, make sure the maxWindowSize parameter is reset also.
                //        //
                //        _optionalFuncByte1 = opFuncByte1 | Mod8;
                //        maxWindowSize = maxWindowSize;
                //        _seqNumMask = 0x07;
                //    }
                //    else
                //    {
                //        _optionalFuncByte0 = opFuncByte1 | Mod128;
                //        _seqNumMask = 0x7f;
                //    }
                //}
            }

            Int32 _seqNumMask = 0x07;
            public Int32 seqNumMask
            {
                get { return _seqNumMask; }
            }

            //  AckMode Control Flag                    JNW (Jan15)

            bool _useAckMode = false;
            public bool useAckMode
            {
                get { return _useAckMode; }
                set { _useAckMode = value; }
            }

            //  Send I(P) at end of Window Flag         (JNW Feb 15)

            bool _useIPoll = false;
            public bool useIPoll
            {
                get { return _useIPoll; }
                set { _useIPoll = value; }
            }

        }
    }
}
