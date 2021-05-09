using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TNCKissInterface
{
    public class TNCChannelInterface
    {

        //
        // Channel Management Interface class.  This class interfaces the Data Link Providers with the
        // KISS TNC Channel. 
        //

        public TNCChannel channel;

        public enum RunState { Running, Closed };
        public RunState runState = RunState.Running;

        //
        // List of active Data Link Providers.  A Data Link Provider handles a specific local Station
        // callsign.
        //

        List<DataLinkProvider> dataLinkProviderList = new List<DataLinkProvider>();

        // 
        // Packet processing thread for incoming link layer packets
        //

        Thread LinkRecvThread;

        //
        // Constructor
        //

        public TNCChannelInterface(COMPort kissComPort, Int32 tncChannelNumber)
        {
            //
            // Create the object for communicating to the TNC via the specified TNC channel
            //

            channel = new TNCChannel(kissComPort, tncChannelNumber);
            LinkRecvThread = new Thread(RecvFromPort);
            LinkRecvThread.Start();
        }

        public DataLinkProvider GetProvider(String StationID)
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

        public DataLinkProvider CreateProvider(String stationID)
        {
            //
            // Create a new provider, If one already exists, then we'll return that one instead
            //

            return AddProvider(new DataLinkProvider(stationID, this));
        }

        public void Close()
        {
            //
            // Close down the channel
            // Close all open DLPs
            // Clear the loop variable for the receive thread and unblock it
            //

            runState = RunState.Closed;


            while (dataLinkProviderList.Count > 0)
            {
                dataLinkProviderList.First().Close();
            }

        }


        //
        // Procesing thread for incoming packets from the TNC.  The TNC places a buffer
        // containing the incoming data into the channel's receive queue.  This routine pulls
        // these frames from the queue for processing.
        // 
        // Processing steps:
        //      - Parse the raw incoming frame into an HDLC buffer.  This process
        //        removes the byte stuffing and verifies the packet CRC.
        //      - Find the Data Link Provider handling incoming data for the Station ID
        //        specified in the incoming address field of the packet.  If no matching
        //        dlp is registered, then we simply drop the packet.  (Future: we add in support
        //        for multi-cast packets, which will forward to all registered DLPs)
        //      - Place the frame, source, and buffer type into a packet object and place it into the
        //        receive queue of the target DLP.
        // 

        void RecvFromPort()
        {
            Byte[] buf;
            while (runState.Equals(RunState.Running))
            {
                try
                {
                    buf = channel.Recv();
                }
                catch
                {
                    continue;
                }

                if (buf == null)
                {
                    continue;
                }

                //
                // Parse raw buffer
                //

                AX25Frame.ParsedFrame pFrame = new AX25Frame.ParsedFrame(HDLCFramer.ParseHDLCFrame(buf));

                AX25Frame.Packet pkt = new AX25Frame.Packet(
                     AX25Frame.Packet.PacketType.ParsedFrame,
                     AX25Frame.Packet.Source.Remote,
                     pFrame);

                //
                // Look for the DLP handling this packet destination
                //

                DataLinkProvider dlp = GetProvider(pFrame.frameInfo.staLink.destinationAddr.stationIDString);

                if (dlp == null)
                {
                    //
                    // Ignore the packet and Loop if we cannot find a provider for the incoming frame
                    // TODO - Add support for multi-cast
                    //
                    continue;
                }

                //
                // Send up to the data link provider handling the specified StationID
                //
                dlp.dataLinkProviderQ.Enqueue(pkt);
            }
        }
    }
}
