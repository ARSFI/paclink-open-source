using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;

namespace TNCKissInterface
{
    //
    // Data Link Provider Class.  This class handles the connection management and Data Link access.
    // One data link provider for each unique local callsign desired.
    //
    // Copyright 2008-2010 - Peter R. Woods (N6PRW)
    //
    public class DataLinkProvider
    {
        //
        // This buffer contains the obfuscated return sting when we receive a 'TEST' AX.25 command.
        // This buffer can be created by passing the desired return string to the program:
        // StringObfuscator.exe.  Current value is:
        // TNCKissInterface DLL V1.1.0.19 Copyright 2009-2011 Peter R. Woods (N6PRW)
        // 
        Byte[,] tmpR = { { 0xbe, 0xd2, 0xde, 0x85 }, 
                         { 0xd3, 0x24, 0x89, 0xa3 }, 
                         { 0xb4, 0x67, 0x6a, 0x65 }, 
                         { 0x05, 0x70, 0x8c, 0x21 }, 
                         { 0xd7, 0xd3, 0x26, 0xee }, 
                         { 0x2b, 0xb2, 0x8c, 0x24 }, 
                         { 0x30, 0xda, 0xa1, 0xbe }, 
                         { 0xb6, 0xf3, 0x50, 0x51 }, 
                         { 0x5c, 0xf2, 0xb5, 0x71 }, 
                         { 0x27, 0xfb, 0x77, 0xca }, 
                         { 0xa8, 0xe1, 0xa4, 0x8a }, 
                         { 0x34, 0x19, 0x7c, 0x44 }, 
                         { 0x63, 0xec, 0xfb, 0x41 }, 
                         { 0xfa, 0x47, 0x66, 0x0b }, 
                         { 0x45, 0xb9, 0x55, 0xf9 }, 
                         { 0x83, 0xca, 0xec, 0xc0 }, 
                         { 0x16, 0x18, 0x08, 0x13 }, 
                         { 0xbb, 0x1a, 0xfa, 0xdb }, 
                         { 0x5b, 0x5c, 0x46, 0x4a }, 
                         { 0xea, 0x9c, 0x9d, 0xce }, 
                         { 0xba, 0x57, 0xfa, 0xea }, 
                         { 0xda, 0x13, 0x0f, 0x17 }, 
                         { 0x63, 0x11, 0xef, 0x44 }, 
                         { 0xf7, 0x97, 0x6a, 0xa2 }, 
                         { 0x0b, 0xe4, 0xbd, 0x0a }, 
                         { 0x01, 0xf4, 0x91, 0x90 }, 
                         { 0x87, 0xca, 0x70, 0x12 }, 
                         { 0x33, 0x82, 0xcc, 0x03 }, 
                         { 0x4e, 0x9c, 0x1f, 0xbe }, 
                         { 0x88, 0xd3, 0x94, 0xba }, 
                         { 0x0d, 0x34, 0x4e, 0x74 }, 
                         { 0x52, 0xdd, 0xdb, 0x11 }, 
                         { 0x9f, 0x33, 0x03, 0x79 }, 
                         { 0x65, 0xeb, 0x7b, 0xd9 }, 
                         { 0xd4, 0xa5, 0x83, 0xa4 }, 
                         { 0x65, 0x38, 0x20, 0x5d }, 
                         { 0x8d, 0x4a, 0xa8, 0x8c }, 
                         { 0x72, 0x7c, 0x66, 0x6a } };

        const Int32 MAXQUEUE = 256;
        const Int32 MAXUIFRAME = 32768;         // MAX size of info field allowed is 32K bytes (256K bits)
        const Int32 defMaxUFrameSize = 128;     // Default size of info field buffer in bytes (1024 bits)

        Timer timerTestCommandTimeout;
        Timer timerSegmenter;
        Timer timerBeacon;

        ElapsedEventHandler timerSegmenterHandler;
        ElapsedEventHandler timerTestCommandTimeoutHandler;
        ElapsedEventHandler timerBeaconHandler;

        Int32 _maxUFrameSize = defMaxUFrameSize;
        public Int32 maxUFrameSize
        {
            get { return _maxUFrameSize; }
            set
            {
                if ((value >= 8) && (value <= MAXUIFRAME))
                {
                    _maxUFrameSize = value;
                }
            }
        }

        //
        // TNC Channel Interface is our link to KISS TNC channel.
        //
        Boolean _beaconEnable = false;
        Int32 _beaconTime = 0;

        public String beaconText = "";
        public String beaconDestination = "";
        public String beaconRelay1 = "";
        public String beaconRelay2 = "";

        public Int32 beaconTime
        {
            get { return _beaconTime; }
            set
            {
                _beaconTime = value;
                if (value > 0)
                {
                    timerBeacon.SetTime(1000 * 60 * _beaconTime);
                }
                else
                {
                    timerBeacon.Stop();
                }
            }
        }

        public Boolean beaconEnable
        {
            get { return _beaconEnable; }
            set
            {
                if (value && (beaconTime > 0))
                {
                    timerBeacon.SetTime(1000 * 60 * beaconTime);
                    timerBeacon.Start();
                    _beaconEnable = true;
                }
                else
                {
                    timerBeacon.Stop();
                    _beaconEnable = false;
                }
            }
        }

        TNCChannel _localTncChannel;
        StationAddress _localStationAddress;

        public TNCChannel localTncChannel
        {
            get { return _localTncChannel; }
        }

        public StationAddress localStationAddress
        {
            get { return _localStationAddress; }
        }

        //
        // Digipeater information
        //

        public Boolean digipeatEnable = false;
        public AccessControlListType accessControlListType = AccessControlListType.none;
        List<String[]> accessControlList = new List<String[]>();

        public enum AccessControlListType
        {
            none = 0,
            white,
            black
        }

        //
        // Queue that we pull incoming remote packets from for processing.
        //
        BlockingQueue recvDatagramQ = new BlockingQueue(MAXQUEUE);
        BlockingQueue linkTestQ = new BlockingQueue(MAXQUEUE);

        Boolean runFlag = true;

        //
        // List of active connections using this DLP
        // 
        
        public List<Connection> connectionList = new List<Connection>();        // Made Publoc for AckMode Tests    (JNW Jan15)

        // 
        // Packet processing thread for data arriving at the DLP
        //
        Thread DataLinkThread;

        //#region Public members

        //
        // Incoming packets to this DLP
        //
        BlockingQueue dataLinkProviderQ = new BlockingQueue(MAXQUEUE);

        //
        // Constructor
        //
        public DataLinkProvider(String callSign, TNCChannel channel)
        {
            _localTncChannel = channel;
            _localStationAddress = new StationAddress(callSign);

            timerSegmenterHandler = new ElapsedEventHandler(timerSegmenter_Elapsed);
            timerSegmenter = new Timer(timerSegmenterHandler, "UI Segmenter - " + _localStationAddress.stationIDString);

            timerSegmenter.SetTime(1000 * 60 * 10);     // 10 munite segmenter timeout

            timerTestCommandTimeoutHandler = new ElapsedEventHandler(timerTestCommandTimeout_Elapsed);
            timerTestCommandTimeout = new Timer(timerTestCommandTimeoutHandler, "TestCommandTimeout - " + _localStationAddress.stationIDString);

            timerBeaconHandler = new ElapsedEventHandler(timerBeacon_Elapsed);
            timerBeacon = new Timer(timerBeaconHandler, "Beacon for - " + _localStationAddress.stationIDString);
            //
            // Start processing
            //
            DataLinkThread = new Thread(ProcessDLPackets);
            DataLinkThread.Name = "DataLinkProvider Packet Processing Thread for Station: " + _localStationAddress.stationIDString;
            DataLinkThread.Start();
        }

        public Boolean Enabled
        {
            //
            // Returns false if this class instance has been shut down.
            //
            get { return runFlag; }
        }

        public void Send(Object packet)
        {
            //
            // Send data to the DLP's processing Queue
            //
            Frame.Packet tmpPkt = (Frame.Packet)packet;

            if (tmpPkt.receivedFrame != null)
            {
                //
                // Receive packet, so check to see if we should accept it.
                //
                if (AccessCheckOK(tmpPkt.receivedFrame.staLink.destinationStation.stationIDString) &&
                    AccessCheckOK(tmpPkt.receivedFrame.staLink.sourceStation.stationIDString) &&
                    AccessCheckOK(tmpPkt.receivedFrame.staLink.relayStation1.stationIDString) &&
                    AccessCheckOK(tmpPkt.receivedFrame.staLink.relayStation2.stationIDString))
                {
                    dataLinkProviderQ.Enqueue(packet);
                    return;
                }
            }
            //
            // Transmit packet, so just send it
            //
            dataLinkProviderQ.Enqueue(packet);
        }

        public void Close()
        {
            runFlag = false;
            dataLinkProviderQ.enabled = false;
            recvDatagramQ.enabled = false;
            linkTestQ.enabled = false;

            while (connectionList.Count > 0)
            {
                connectionList[0].Close();
            }
            Thread.Sleep(200);   // Give Close a chance to finish.  W4PHS 23-Jan-2015
            _localTncChannel.RemoveProvider(this);
        }

        public Connection CreateConnection()
        {
            //
            // Create a new connection instance
            //
            return AddConnection(new Connection(this));
        }

        public Connection CreateConnection(Connection.ConnectionParameterBuf parameterBuf)
        {
            //
            // Create a new connection instance
            //
            return AddConnection(new Connection(this, parameterBuf));
        }

        public Connection GetConnection(String stationID)
        {
            //
            // Routine to seach for either an incoming or outgoing Connection handling a specific StationLink.  
            // Returns null if no connection is found.
            //

            Connection retCon = null;

            lock (connectionList)
            {
                foreach (Connection con in connectionList)
                {
                    if (con.connectedStation.GetConnectionPath().Equals(stationID))
                    {
                        retCon = con;
                        break;
                    }
                }
            }
            return retCon;
        }

        public Connection GetAvailableConnection(String stationID)
        {
            //
            // Routine to seach for a local connection handling a specific StationID.  Returns null if
            // no connection is found, or if the connection is currently busy.
            //
            Connection retCon = null;

            lock (connectionList)
            {
                foreach (Connection con in connectionList)
                {
                    if (con.connectedStation.sourceStation.stationIDString.Equals(stationID) &&
                        (con.connectState.Equals(Connection.ConnectionState.Disconnected)))
                    {
                        con.connectedStation.destinationStation.Clear();
                        con.connectedStation.relayStation1.Clear();
                        con.connectedStation.relayStation2.Clear();
                        retCon = con;
                        break;
                    }
                }
            }
            return retCon;
        }

        public void RemoveConnection(Connection con)
        {
            //
            // Remove the specified DLP
            //

            lock (connectionList)
            {
                connectionList.Remove(con);
            }
        }

        //
        // Send Datagram methods.  Use old protocol (no segmentation) by default
        //
        public void SendDatagram(String destinationStation, Byte[] buf)
        {
            SendDatagram(destinationStation, "", "", buf, Frame.ProtocolVersion.V20);
        }
        public void SendDatagram(String destinationStation, String relayStation1, Byte[] buf)
        {
            SendDatagram(destinationStation, relayStation1, "", buf, Frame.ProtocolVersion.V20);
        }
        public void SendDatagram(String destinationStation, String relayStation1, String relayStation2, Byte[] buf)
        {
            SendDatagram(destinationStation, relayStation1, relayStation2, buf, Frame.ProtocolVersion.V20);
        }

        //
        // Allow caller to specify protocol version to use
        //
        public void SendDatagram(String destinationStation, Byte[] buf, Frame.ProtocolVersion version)
        {
            SendDatagram(destinationStation, "", "", buf, version);
        }
        public void SendDatagram(String destinationStation, String relayStation1, Byte[] buf, Frame.ProtocolVersion version)
        {
            SendDatagram(destinationStation, relayStation1, "", buf, version);
        }
        public void SendDatagram(String destinationStation, String relayStation1, String relayStation2, Byte[] buf, Frame.ProtocolVersion version)
        {
            //
            // Split the datagram into segment as needed and send them off
            // Note the buffer returned from GetNextSegment has the appropriate PID byte(s)
            // prepended to the data.
            //
            Int32 tmpFSize = defMaxUFrameSize;

            Station stationLink = new Station();
            stationLink.sourceStation = _localStationAddress;
            stationLink.destinationStation.SetCallSign(destinationStation);
            if (relayStation1.Length > 0)
            {
                stationLink.relayStation1.SetCallSign(relayStation1);
                if (relayStation2.Length > 0)
                {
                    stationLink.relayStation2.SetCallSign(relayStation2);
                }
            }

            //
            // Use user selected value for new protocol version
            //
            if (version.Equals(Frame.ProtocolVersion.V22))
            {
                tmpFSize = _maxUFrameSize;
            }

            Segmenter segment = new Segmenter(tmpFSize, buf, Frame.FrameTypes.UIType, version);
            do
            {
                Frame.Packet packet = new Frame.Packet(
                    Frame.TransmitFrameBuild(stationLink, Station.CommandResponse.DoCommand, 0,
                    Frame.FrameTypes.UIType, segment.GetNextSegment()));

                dataLinkProviderQ.Enqueue(packet);

            } while (segment.segmentsRemaining > 0);
        }

        public Byte[] RecvDatagram()
        {
            //
            // Handle returned datagram packets.  Segmented packets are reassembled before returning
            //
            Byte[] tmpB = null;
            Int32 ptr = 0;
            Int32 j = 0;
            Byte[] segBuf = null;
            Int32 chunksRemaining = 0;

            //
            // Start the timeout timer.
            //
            timerSegmenter.Start();

            do
            {
                //
                // If this is our first time through, look at the number of packets remaining
                // to determine how much buffer space we'll need and allocate it.
                //
                segBuf = (Byte[])recvDatagramQ.Dequeue();

                if (segBuf == null)
                {
                    timerSegmenter.Stop();
                    return null;
                }

                chunksRemaining = segBuf[0] & 0x7f;

                if ((segBuf[0] & Segmenter.StartOfSegment) == Segmenter.StartOfSegment)
                {
                    //
                    // If the start of segment bit is set, look at the number of chunks remaining
                    // to determine how much buffer space we'll need and allocate it.
                    //
                    tmpB = new Byte[MAXUIFRAME * (chunksRemaining + 1)];
                }

                if (tmpB == null)
                {
                    //
                    // No start of segment flag specified.  Return null
                    //
                    timerSegmenter.Stop();
                    return null;
                }

                //
                // Add this packet to the total
                //
                j = 1;
                while (j < segBuf.Length)
                {
                    tmpB[ptr++] = segBuf[j++];
                }

            } while (chunksRemaining > 0);

            //
            // Stop the timer.
            //
            timerSegmenter.Stop();
            return Support.PackByte(tmpB, 0, ptr);
        }

        //
        // Digipeat functions
        //

        public void AccessControlListAdd(String station)
        {
            //
            // Routine to add a station to a digipeat list
            //
            String[] sta;

            if (ValidateStation(out sta, station))
            {
                //
                // Add the station to the list
                //
                accessControlList.Add(sta);
            }
        }

        public void AccessControlListClear()
        {
            //
            // Routine to clear the digipeat list
            //
            accessControlList.Clear();
        }

        Boolean ValidateStation(out String[] sta, String station)
        {
            sta = new String[2];
            Char[] c = { '-' };
            String[] tmpSta = station.Split(c);

            if (tmpSta[0] == "*")
            {
                sta[0] = "*";
                sta[1] = "*";
                return true;
            }

            if (tmpSta.Length == 2)
            {
                sta[0] = tmpSta[0].ToUpper().Trim();
                sta[1] = tmpSta[1].ToUpper().Trim();
                return true;
            }
            return false;
        }

        public void Digipeat(Frame.Packet packet, Int32 relayNum)
        {
            //
            // Function to implement digipeating of packets
            //

            if (digipeatEnable)
            {
                //
                // Digipeating is enabled
                //

                if (AccessCheckOK(packet.receivedFrame.staLink.destinationStation.stationIDString) &&
                    AccessCheckOK(packet.receivedFrame.staLink.sourceStation.stationIDString) &&
                    AccessCheckOK(packet.receivedFrame.staLink.relayStation1.stationIDString) &&
                    AccessCheckOK(packet.receivedFrame.staLink.relayStation2.stationIDString))
                {
                    Byte[] tmpBuf = packet.receivedFrame.rawBuf;

                    //
                    // All stations listed in the packet are allowed to digipeat, so continue
                    //
                    //
                    // Set the CH bit on the specified relay
                    //
                    tmpBuf[13 + (7 * relayNum)] |= 0x80;

                    _localTncChannel.Send(tmpBuf);
                }
            }
        }

        Boolean AccessCheckOK(String s)
        {
            //
            // Function to check the access list to see if we should
            // accept the packet.  Returns True to accept
            //
            Char[] c = { '-' };
            String[] sta = s.Split(c);
            Boolean match = false;

            if (accessControlListType.Equals(AccessControlListType.none) ||
                (s == localStationAddress.stationIDString) ||
                (s.Length == 0))
            {
                //
                // No list checking or he is us, so just return true
                //
                return true;
            }

            //
            // Check the list
            //
            foreach (String[] l in accessControlList)
            {
                if (l[0] == "*")
                {
                    match = true;
                    break;
                }
                if (l[0] == sta[0])
                {
                    if ((l[1] == "*") ||
                        (l[1] == sta[1]))
                    {
                        match = true;
                        break;
                    }
                }
            }

            if (accessControlListType.Equals(AccessControlListType.white))
            {
                return match;
            }
            else
            {
                return !match;
            }
        }

        //#endregion

        //#region Link Test

        public Byte[] LinkTest(String destinationStation, Byte[] buf)
        {
            return LinkTestCommon(destinationStation, "", "", buf, -1);
        }
        public Byte[] LinkTest(String destinationStation, String relayStation1, Byte[] buf)
        {
            return LinkTestCommon(destinationStation, relayStation1, "", buf, -1);
        }
        public Byte[] LinkTest(String destinationStation, String relayStation1, String relayStation2, Byte[] buf, Int32 timeout)
        {
            return LinkTestCommon(destinationStation, relayStation1, relayStation2, buf, timeout);
        }

        Byte[] LinkTestCommon(String destinationStation, String relayStation1, String relayStation2, Byte[] buf, Int32 timeout)
        {
            Station stationLink = new Station();

            stationLink.sourceStation = _localStationAddress;
            stationLink.destinationStation.SetCallSign(destinationStation);
            if (relayStation1.Length > 0)
            {
                stationLink.relayStation1.SetCallSign(relayStation1);
                if (relayStation2.Length > 0)
                {
                    stationLink.relayStation2.SetCallSign(relayStation2);
                }
            }

            Frame.Packet packet = new Frame.Packet(
                Frame.TransmitFrameBuild(stationLink,
                    Station.CommandResponse.DoCommand,
                    1, Frame.FrameTypes.TESTType, buf));

            if (timeout > 0)
            {
                //
                // Start the timeout timer
                //
                timerTestCommandTimeout.SetTime(timeout);
                timerTestCommandTimeout.Start();
            }

            dataLinkProviderQ.Enqueue(packet);

            //
            // Wait until a return packet arrives, or return null if we time out
            //
            return (Byte[])linkTestQ.Dequeue();
        }

        //#endregion

        Connection AddConnection(Connection c)
        {
            //
            // Add a Connection to the list if it is not already there.
            //
            lock (connectionList)
            {
                connectionList.Add(c);
                return c;
            }
        }

        void ProcessDLPackets()
        {
            Frame.Packet packet;
            Connection con;
            Int32 tmpL = 4;

            Support.DbgPrint(Thread.CurrentThread.Name + " Starting...");

            while (runFlag)
            {
                //
                // This thread get incoming packets from the remote and sends them off for procesing
                //
                try
                {
                    packet = (Frame.Packet)dataLinkProviderQ.Dequeue();
                }
                catch
                {
                    continue;
                }

                if (packet == null)
                {
                    continue;
                }

                if (packet.transmitFrame != null)
                {
                    //
                    // Incoming packets are passed in as transmit frames and are sent out to the TNC Channel
                    //
                    _localTncChannel.Send(packet.transmitFrame);
                }
                else
                {
                    //
                    // Packets arriving from lower layer come in as received frames
                    //
                    if (packet.receivedFrame.decodeOK)
                    {
                        //
                        // Process the parsed frame
                        //

                        Int32 pfBit = packet.receivedFrame.pfBit;

                        //
                        // Unnumbered frame
                        //

                        if (packet.receivedFrame.frameType.Equals(Frame.FrameTypes.UIType))
                        {
                            //
                            // Unnumbered Info Frame received, so pop it into the DLP Recv datagram Q
                            // code falls through and Sends DM resp if PFBit is set
                            //
                            recvDatagramQ.Enqueue(packet.receivedFrame.iBuf);
                            if (pfBit == 0)
                            {
                                continue;
                            }
                        }

                        if (packet.receivedFrame.frameType.Equals(Frame.FrameTypes.TESTType))
                        {
                            //
                            // Test frame
                            //
                            if (packet.receivedFrame.cmdResp.Equals(Frame.PacketType.Command))
                            {
                                Int32 i;
                                Int32 f = tmpR.Length >> 1;

                                //
                                // Send out a response to in incoming test command
                                //
                                Byte[] tBuf = new Byte[f + packet.receivedFrame.iBuf.Length];
                                for (i = 0; i < f; i++)
                                {
                                    tBuf[i] = (Byte)(tmpR[i / tmpL, i % tmpL] ^ tmpR[(i + f) / tmpL, i % tmpL]);
                                }
                                packet.receivedFrame.iBuf.CopyTo(tBuf, i);
                                Frame.Packet tmpP = new Frame.Packet(
                                    Frame.TransmitFrameBuild(packet.receivedFrame.staLink.Rev(),
                                        Station.CommandResponse.DoResponse,
                                        pfBit, Frame.FrameTypes.TESTType,
                                        tBuf));

                                dataLinkProviderQ.Enqueue(tmpP);
                            }
                            else
                            {
                                //
                                // Test resp received. Return buffer to upper layer
                                //
                                linkTestQ.Enqueue(packet.receivedFrame.iBuf);
                            }
                            continue;
                        }
                        //
                        // Other packet types get sent to the upper layer (If one is available)
                        //

                        con = GetConnection(packet.receivedFrame.staLink.GetConnectionPathRev());
                        if (con == null)
                        {
                            //
                            // No existing connection available, so check for any listeners
                            //
                            con = GetAvailableConnection(packet.receivedFrame.staLink.destinationStation.stationIDString);

                            if (con != null)
                            {
                                //
                                // Found a listener, so set up the connection 
                                //
                                con.connectedStation.destinationStation.SetCallSign(packet.receivedFrame.staLink.sourceStation.stationIDString);
                                if (packet.receivedFrame.staLink.relayStation2.stationIDString.Length > 0)
                                {
                                    con.connectedStation.relayStation1.SetCallSign(packet.receivedFrame.staLink.relayStation2.stationIDString);
                                    con.connectedStation.relayStation2.SetCallSign(packet.receivedFrame.staLink.relayStation1.stationIDString);
                                }
                                else if (packet.receivedFrame.staLink.relayStation1.stationIDString.Length > 0)
                                {
                                    con.connectedStation.relayStation1.SetCallSign(packet.receivedFrame.staLink.relayStation1.stationIDString);
                                }
                            }
                            else
                            {
                                //
                                // No connection so send DM resp with PF bit set to uframe.pfBit 
                                //
                                _localTncChannel.Send(Frame.TransmitFrameBuild(packet.receivedFrame.staLink.Rev(),
                                    Station.CommandResponse.DoResponse, pfBit, Frame.FrameTypes.DMType, null).ibuf);
                                continue;
                            }
                        }

                        //
                        // Found a connection, so sent the packet off to it
                        //
                        con.Insert(packet);
                        continue;
                    }

                    //
                    // Arrival here means the frame did not decode correctly, so just drop the packet
                    //
                    Support.DbgPrint(Thread.CurrentThread.Name + " - Frame decode error");
                }
            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exiting...");
        }

        //
        // Link Test timeout handler
        //
        void timerTestCommandTimeout_Elapsed(object sender, ElapsedEventArgs e)
        {
            Support.DbgPrint("TestCommandTimeout timer fired...");
            linkTestQ.Wakeup();
            timerTestCommandTimeout.Stop();
        }

        //
        // Segmenter timeout handler
        //
        void timerSegmenter_Elapsed(object sender, ElapsedEventArgs e)
        {
            Support.DbgPrint("Segmenter timer fired...");
            recvDatagramQ.Wakeup();
            timerSegmenter.Stop();
        }

        //
        // Beacon timeout handler
        //
        void timerBeacon_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (beaconEnable)
            {
                Support.DbgPrint("Beacon timer fired...");
                SendDatagram(beaconDestination, beaconRelay1, beaconRelay2, Encoding.ASCII.GetBytes(beaconText));
            }
            else
            {
                timerBeacon.Stop();
            }
        }
    }
}
