using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TNCKissInterface
{
    //
    // Channel Management Interface class.  This class interfaces the Data Link Providers with the
    // KISS TNC Channel.  This class supports up the the max of 16 possible channels for a TNC.  By default, 
    // single channel TNCs use channel 0.
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    public class TNCChannel
    {

        Boolean runFlag = true;
        static String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);
        Object packetLogSyncObject = new Object();
     
        public enum PacketMonitorType
        {
            None = 0,       // No packet logging
            LogFile,        // Send to log file (if a log filename was defined)
            Queue,          // Send to the PacketMonitor() queue
            Both            // Send to both places
        }

        //
        // Queue used to monitor outgoing and incoming packets at the KISS Layer.  To have events sent
        // to the queue, set the packetMonitorEnable flag to true
        //

        BlockingQueue packetMonitorQueue = new BlockingQueue(4096);
        PacketMonitorType _packetMonitorEnable = PacketMonitorType.None;
        public PacketMonitorType packetMonitorEnable
        {
            get { return _packetMonitorEnable; }
            set
            {
                _packetMonitorEnable = value;
                if (value.Equals(PacketMonitorType.Queue) || value.Equals(PacketMonitorType.Both))
                {
                    packetMonitorQueue.enabled = true;
                }
                else
                {
                    packetMonitorQueue.enabled = false;
                }
            }
        }

        //
        // List of active Data Link Providers.  A Data Link Provider handles a specific local Station
        // callsign.
        //
        List<DataLinkProvider> dataLinkProviderList = new List<DataLinkProvider>();

        // 
        // Packet processing thread for incoming link layer packets
        //
        Thread ChannelRecvThread;
        COMPort comSerialPort;
        Int32 chanNum;

        //
        // Constructor
        //

        //#region Public Methods

        public TNCChannel(COMPort comPort, Int32 tncChannelNumber)
        {
            //
            // Create the object for communicating to the TNC via the specified TNC channel
            //
            comSerialPort = comPort;
            chanNum = tncChannelNumber;

            ChannelRecvThread = new Thread(RecvFromPort);
            ChannelRecvThread.Name = "Channel Receive Thread for TNC Channel " + tncChannelNumber.ToString();
            ChannelRecvThread.Start();
        }

        public Boolean Enabled
        {
            //
            // Returns false if this class instance has been shut down.
            //
            get { return runFlag; }
        }

        public void RemoveProvider(DataLinkProvider d)
        {
            //
            // Remove the specified DLP
            //
            lock (dataLinkProviderList)
            {
                dataLinkProviderList.Remove(d);
            }
        }

        public DataLinkProvider CreateProvider(String sourceStation)
        {
            //
            // Create a new provider, If one already exists, then we'll return that one instead
            //

            return AddProvider(new DataLinkProvider(sourceStation, this));
        }

        public void Close()
        {
            //
            // Close down the channel
            // Close all open DLPs
            // Clear the loop variable for the receive thread and unblock it
            //
            runFlag = false;
            _packetMonitorEnable = PacketMonitorType.None;
            packetMonitorQueue.enabled = false;

            while (dataLinkProviderList.Count > 0)
            {
                dataLinkProviderList[0].Close();
            }
            Thread.Sleep(200);   // Give Close a chance to finish.  W4PHS 23-Jan-2015
        }

        public void SetTXDelay(Int32 delay)
        {
            SendCommand(1, delay);
        }

        public void SetPersistence(Int32 persistence)
        {
            SendCommand(2, persistence);
        }

        public void SetSlotTime(Int32 slot)
        {
            SendCommand(3, slot);
        }

        public void SetTXTail(Int32 tail)
        {
            SendCommand(4, tail);
        }

        public void SetTXFullDuplex(Int32 fullDuplex)
        {
            SendCommand(5, fullDuplex);
        }

        public void SetKissModeOff()
        {
            SendCommand(0x0f, 0);
        }

        public Boolean Send(Byte[] buf)
        {
            String parsedFrameString = "";
            //
            // Post outbound packet to monitor queue if enabled
            //

            Byte[] tmp = new Byte[buf.Length];
            buf.CopyTo(tmp, 0);

            new Frame.ReceivedFrame(tmp, out parsedFrameString);

            if (!_packetMonitorEnable.Equals(PacketMonitorType.None))
            {
                StringBuilder frameString = new StringBuilder("|chan:" + chanNum.ToString() + "|-->", 4096);
                frameString.Append(parsedFrameString);
                if (_packetMonitorEnable.Equals(PacketMonitorType.LogFile) || _packetMonitorEnable.Equals(PacketMonitorType.Both))
                {
                    Support.PktPrint(frameString.ToString() + CRLF, ref packetLogSyncObject);
                }

                if (_packetMonitorEnable.Equals(PacketMonitorType.Queue) || _packetMonitorEnable.Equals(PacketMonitorType.Both))
                {
                    packetMonitorQueue.Enqueue(frameString.ToString());
                }
            }

            return comSerialPort.SendCmd(chanNum, 0, buf);
        }

        //  Overload for AckMode                (JNW Jan15)

        public Boolean Send(Frame.TransmitFrame transmitFrame)
        {
            String parsedFrameString = "";
            Byte[] buf = transmitFrame.ibuf;

            //
            // Post outbound packet to monitor queue if enabled
            //

            Byte[] tmp = new Byte[buf.Length];
            buf.CopyTo(tmp, 0);

            new Frame.ReceivedFrame(tmp, out parsedFrameString);

            if (!_packetMonitorEnable.Equals(PacketMonitorType.None))
            {
                StringBuilder frameString = new StringBuilder("|chan:" + chanNum.ToString() + "|-->", 4096);
                frameString.Append(parsedFrameString);
                if (_packetMonitorEnable.Equals(PacketMonitorType.LogFile) || _packetMonitorEnable.Equals(PacketMonitorType.Both))
                {
                    Support.PktPrint(frameString.ToString() + CRLF, ref packetLogSyncObject);
                }

                if (_packetMonitorEnable.Equals(PacketMonitorType.Queue) || _packetMonitorEnable.Equals(PacketMonitorType.Both))
                {
                    packetMonitorQueue.Enqueue(frameString.ToString());
                }
            }

            return comSerialPort.SendCmd(chanNum, 0, transmitFrame);
        }
        public Byte[] Recv()
        {
            return comSerialPort.Recv(chanNum);
        }

        public String PacketMonitor()
        {
            return (String)packetMonitorQueue.Dequeue();
        }

        //#endregion

        DataLinkProvider GetProvider(String StationID)
        {
            //
            // Routine to seach for the DLP handling a given station callsign.  Returns null if
            // no provider is found.
            //

            DataLinkProvider retDlp = null;
            lock (dataLinkProviderList)
            {
                foreach (DataLinkProvider dlp in dataLinkProviderList)
                {
                    if (dlp.localStationAddress.stationIDString.Equals(StationID))
                    {
                        retDlp = dlp;
                        break;
                    }
                }
            }
            return retDlp;
        }

        public Connection GetConnectionByACKModeID(DataLinkProvider dlp, Int16 AckModeID)
        {
            //
            // Routine to seach for either an incoming or outgoing Connection handling a specific StationLink.  
            // Returns null if no connection is found.
            //

            Connection retCon = null;

            lock (dlp.connectionList)
            {
                foreach (Connection con in dlp.connectionList)
                {
                    if (con.AckModeID == AckModeID)
                    {
                        retCon = con;
                        break;
                    }
                }
            }
            return retCon;
        }

        DataLinkProvider AddProvider(DataLinkProvider d)
        {
            //
            // Add a DLP to the list if it is not already there.
            //

            lock (dataLinkProviderList)
            {
                foreach (DataLinkProvider dlp in dataLinkProviderList)
                {
                    if (dlp.localStationAddress.stationIDString.Equals(d.localStationAddress.stationIDString))
                    {
                        return dlp;     //Connection already exists, so return it instead
                    }
                }
                dataLinkProviderList.Add(d);
                return d;
            }
        }

        //
        // Procesing thread for incoming packets from the TNC.  The TNC places a buffer
        // containing the incoming data into the channel's receive queue.  This routine pulls
        // these frames from the queue for processing.
        // 
        // Processing steps:
        //      - Parse the raw incoming frame into a receive buffer.  
        //      - Find the Data Link Provider handling incoming data for the Station ID
        //        specified in the incoming address field of the packet.  If no matching
        //        dlp is registered, then we simply drop the packet.  (Future: we add in support
        //        for multi-cast packets, which will forward to all registered DLPs)
        //      - Place the frame, source, and buffer type into a packet object and place it into the
        //        receive queue of the target DLP.
        // 

        void RecvFromPort()
        {
            String parsedFrameString = "";
            Byte[] buf;

            Support.DbgPrint(Thread.CurrentThread.Name + "Starting.");
            while (runFlag && comSerialPort.Enabled)
            {
                try
                {
                    buf = Recv();
                }
                catch
                {
                    continue;
                }

                if (buf == null)
                {
                    continue;
                }

                // check for ACKMODE Response (Need to be at channel level to find DLC)       (JNW Jan15) 

                if (buf.Length == 2)            // Only ACK frames are this short
                {
                    Int16 AckID = (Int16)(buf[0] << 8 | buf[1]);
                    Connection con;

                    lock (dataLinkProviderList)
            
                    foreach (DataLinkProvider dlp in dataLinkProviderList)
                    {
                        con = GetConnectionByACKModeID(dlp, AckID);

                        if (con != null)
                        {
                            // Set Timer back to normal

                            con.timerAck.SetTime(con.smoothedRoundTrip, true);

                            break;

                        }
                    }

                    continue;                   // Donf pass Ack frame up to next level
                }
          

                //
                // Parse raw buffer
                //
                Frame.ReceivedFrame pFrame = new Frame.ReceivedFrame(buf, out parsedFrameString);

                //
                // Output to monitor queue is enabled
                //
                if (!_packetMonitorEnable.Equals(PacketMonitorType.None))
                {
                    StringBuilder frameString = new StringBuilder("|chan:" + chanNum.ToString() + "|<--", 4096);
                    frameString.Append(parsedFrameString);
                    if (_packetMonitorEnable.Equals(PacketMonitorType.LogFile) || _packetMonitorEnable.Equals(PacketMonitorType.Both))
                    {
                        Support.PktPrint(frameString.ToString() + CRLF, ref packetLogSyncObject);
                    }

                    if (_packetMonitorEnable.Equals(PacketMonitorType.Queue) || _packetMonitorEnable.Equals(PacketMonitorType.Both))
                    {
                        packetMonitorQueue.Enqueue(frameString.ToString());
                    }
                }

                Frame.Packet packet = new Frame.Packet(pFrame);


                DigipeatCheckAndProcess(packet);
                    //continue;
                

                //
                // Send up to the data link provider handling the specified StationID
                //
                //dlp.Send(packet);
            }
            Support.DbgPrint(Thread.CurrentThread.Name + "Exiting.");
        }

        void SendCommand(Int32 c, Int32 i)
        {
            Byte[] b = { Convert.ToByte(i & 0xff) };
            comSerialPort.SendCmd(chanNum, c, b);
        }

        void DigipeatCheckAndProcess(Frame.Packet packet)
        {
            DataLinkProvider dlp = null;
            Int32 relayNum = 0;
            Int32 relayCount = 0;

            if (packet.receivedFrame != null)
            {
                if (packet.receivedFrame.decodeOK)
                {
                    //
                    // Packet looks OK
                    //
                    Station sta = packet.receivedFrame.staLink;
                    if (sta.relayStation1.stationIDString.Length > 0)
                    {
                        relayCount++;
                    }
                    if (sta.relayStation2.stationIDString.Length > 0)
                    {
                        relayCount++;
                    }

                    if ((relayCount >= 1) && (sta.relayStation1.chBit == 0))
                    {
                        //
                        // Relay address 1 has not been repeated, so see if we have a DLP for it.
                        //
                        dlp = GetProvider(sta.relayStation1.stationIDString);
                        relayNum = 1;
                    }
                    else if ((relayCount >= 2) && (sta.relayStation2.chBit == 0))
                    {
                        //
                        // Relay address 2 has not been repeated, so see if we have a DLP for it.
                        //
                        dlp = GetProvider(sta.relayStation2.stationIDString);
                        relayNum = 2;
                    }
                    else
                    {
                        dlp = GetProvider(sta.destinationStation.stationIDString);
                    }
                }
                if (dlp != null)
                {
                    //
                    // We have a provider, check whether this is a digi or if packet is at its final destination
                    //
                    if (relayNum == 0)
                    {
                        //
                        // All digipeating complete (if any) so send it up for processing
                        //
                        dlp.Send(packet);
                    }
                    else
                    {
                        //
                        // Need to digipeat it, so send it up for digipeating
                        //
                        dlp.Digipeat(packet, relayNum);
                    }
                }
            }
        }
    }
}

