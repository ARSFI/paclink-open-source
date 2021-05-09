using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TNCKissInterface;
using System.IO;

namespace TestApp
{
    //
    // Simple test application for comunicating to a TNC via the TNCKissInterface DLL.
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    class Program
    {
        static String CR = Convert.ToString((Char)13);
        static String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);

        //
        // COM port Instance
        //
        static COMPort tncComPort;

        //
        // Channel Instance on the TNC
        //
        static TNCChannel tncVHFChannel;

        //
        // Responder thread is launched in response to the @listener command.  
        // Provides a way to perform loopback testing of the DLL without 
        // having an actual TNC
        //
        static Thread ResponderThread;

        //
        // Monitor thread grabs packet information as the packets are seen by 
        // the low level interface and writes then out to a file
        //
        static Thread MonitorThread;

        //
        // Reader thread echos back returned data after a connection has been 
        // established
        //
        static Thread ReaderThread;
        static Thread [] mReaderThread = new Thread[12];

        //static String dbgFile;
        //static String pktFile;

        static Connection con = null;
        static Connection[] mcon = new Connection[12];

        //
        // This is the callback delegate that will be invoked when a connection
        // changes state from connected to disconnected (and vice versa)
        //
        static void NotifyConnectionState(Object src, ConnectionEvents cev)
        {
            Console.WriteLine("Connection Status: " + cev.GetState.ToString() +
                " (Souce:" + cev.GetSource +
                " Destination:" + cev.GetDestination +
                " Relay1:" + cev.GetRelay1 +
                " Relay2:" + cev.GetRelay2 +
                " ID:" + cev.GetConnectionID +
                ")");
        }

        //
        // This is the callback delegate that will be invoked when an event 
        // indication occurs within the AX25 protocol.
        // The indication can be:
        //      data                - receive data available
        //      error               - protocol error occurred
        //      connectRequest      - remote station initiated a connection request
        //      disconnectRequest   - remote station initiated a disconnect request
        //
        static void NotifyIndicationEvents(Object src, IndicationEvents iev)
        {
            //
            // Sample callback delegate that receives connection indication events.
            //
            Console.WriteLine("Event Indication: " + iev.GetIndication.ToString() +
                " (Souce:" + iev.GetSource +
                " Destination:" + iev.GetDestination +
                " Relay1:" + iev.GetRelay1 +
                " Relay2:" + iev.GetRelay2 +
                " ID:" + iev.GetConnectionID +
                ") - Addl Info: " + iev.GetData);
        }

        static void Main(string[] args)
        {
            String localCallSign;
            String cmd;
            String loopback;
            Int32 channel = 0;
            Int32 baud = 9600;
            String tmpc;
            DataLinkProvider dlProvider;
            DataLinkProvider digi1;
            DataLinkProvider digi2;
            Connection.ConnectionParameterBuf conParams;
            Connection.ConnectionStatus cstat;
            Connection.ConnectionStatus[] mcstat = new Connection.ConnectionStatus[12];
            DataLinkProvider[] testProvider = new DataLinkProvider[12];

            Console.Write("Local station callsign-SSID? (e.g. KA1XYZ-5) ? ");
            localCallSign = Console.ReadLine();

            //
            // Set loopback mode to Y if you are not using a TNC.
            //
            Console.Write("Use loopback mode? (y/[n]) ");
            tmpc = Console.ReadLine();
            loopback = tmpc;

            Console.Write("TNC Channel number? ([0]/1/2) ");
            tmpc = Console.ReadLine();
            if (tmpc.Length > 0)
            {
                channel = Convert.ToInt32(Console.ReadLine());
            }
            //
            // Step 1: Create the low level Com Port that communicates directly 
            // with the TNC
            //
            if (loopback.ToUpper().StartsWith("Y"))
            {
                //
                // Use loopback.  No data goes out the serial port
                //
                tncComPort = new COMPort("COM1", 9600, true);
            }
            else
            {
                //
                // No loopback.  Use the real TNC
                //
                Console.Write("Com Port? (COM1, COM2, etc) ");
                String comp = Console.ReadLine().ToUpper();

                Console.Write("Baud? ([9600],19200, etc) ");
                tmpc = Console.ReadLine();
                if (tmpc.Length > 0)
                {
                    baud = Convert.ToInt32(tmpc);           // (JNW Feb 15)
                }

                tncComPort = new COMPort(comp, baud);
            }

            Char[] c = { ' ' };

            //
            // Tell the KISS layer if there are any characters to 'escape' in 
            // the data stream.  The reason for this is some KISS implementations
            // (Like the TNC in the Kenwood D710A) actually act upon certain 
            // character sequences even when in KISS mode.
            //
            // Example: Escape the 'C' character (needed on TM-D710)
            //
            // You can also add escape byte for non printable characters using the
            // SetEscapedCharList().  In this case, supply a string that contains the
            // comma separated list of bytes to escape.  Example: A string value of:
            // "03, 0d" will exclude byte valus 0x03 and 0x0d
            //
            tncComPort.escapedCharList = "C";

            //tncComPort.SetEscapedCharsList("03, 0d");

            //
            // Step 2: Create a TNC channel on top of the COM port. TNCs like 
            // the KPC3+ have one channel (channel 0).  Other TNCs like the KAM
            // Plus have 2 (one for VHF (channel 0) and one for HF (channel 1).  The
            // KISS protocol dictates that TNC Channels MUST be in the range of 0-15. 
            //
            tncVHFChannel = tncComPort.CreateChannel(channel);

            //
            // (Optional) Enable the packet monitor function if we want to 
            // monitor all incoming and outgoing packets.  Setting the value 
            // to 'Both' enables queue monitoring as well as file logging of 
            // the packets.  (We defined the filename above by setting the static
            // member: Support.pktLogFile above.)
            //
            tncVHFChannel.packetMonitorEnable = TNCChannel.PacketMonitorType.Both;

            //
            // Step 3: Create a DataLinkProvider assigning it the desited 
            // local station callsign-ssid.  Multiple Data Link Providers \
            // are supported, however each must have a unique callsign-ssid.
            // 
            dlProvider = tncVHFChannel.CreateProvider(localCallSign);
            digi1 = tncVHFChannel.CreateProvider("RELAY1");
            digi1.digipeatEnable = true;
            digi2 = tncVHFChannel.CreateProvider("RELAY2");
            digi2.digipeatEnable = true;


            //
            // Note: 3a.1 - 3a-3 are optional steps you would do if you want 
            // to change any of the default connection parameter settings for
            // this particular connection, like MAXIFrame, MaxWindowsSize, etc. 
            // Unless you are planning to change the defaults, you don't have 
            // to do this and can skip right to step 4.
            //

            //
            // Step 3a.1: (Optional) Create a Connection parameter buf to 
            // use during XID negotiation.  
            //
            conParams = new Connection.ConnectionParameterBuf();

            //
            // Step 3a.2: (Optional) Make any changes to the parameter values 
            // in the parameter buffer here
            //

            //conParams.maxWindowSize = 1;
            //conParams.maxIFrame = 2;

            //
            // Step 3a.3: Create a connection using the connection parameters 
            // specified in the parameter buffer and skip step 4 below. 
            //
            con = dlProvider.CreateConnection(conParams);

            for (Int32 i = 0; i < testProvider.Length; i++)
            {
                testProvider[i] = tncVHFChannel.CreateProvider("T6EST-" + i.ToString());
                mcon[i] = testProvider[i].CreateConnection(conParams);
                mcon[i].registerStateHandlers.ReportConnectionEvents += new EventHandler<ConnectionEvents>(NotifyConnectionState);
                mcon[i].registerIndicationHandlers.ReportIndicationEvents += new EventHandler<IndicationEvents>(NotifyIndicationEvents);
            }


            //
            // Step 4: Create the Connection using default parameters
            //
            //con = dlProvider.CreateConnection();

            //
            // Register for connection state change notifications, if desired
            //
            con.registerStateHandlers.ReportConnectionEvents += new EventHandler<ConnectionEvents>(NotifyConnectionState);

            //
            // Register for protocol indication event notifications, if desired
            //
            con.registerIndicationHandlers.ReportIndicationEvents += new EventHandler<IndicationEvents>(NotifyIndicationEvents);

            //
            // Launch our packet monitor thread. (If we chose to monitor packets 
            // above). 
            //
            MonitorThread = new Thread(PacketMonitor);
            MonitorThread.Name = "PacketMonitor Thread";
            MonitorThread.Start();

            //
            // Launch the thread the displays the data stream coming back from 
            // the remote station
            //
            ReaderThread = new Thread(Reader);
            ReaderThread.Name = "Reader Thread";
            ReaderThread.Start();

            //
            // Create a thread that adds a second DataLinkProvider with a different 
            // callsign that we can perform loopback testing with.  We only start 
            // the thread when requested by the command '@listener callsign-ssid'. 
            // You would typically only do this when testing in loopback mode.
            //
            // In an applications where you wait for other stations to connect, 
            // you can create a pool of connection threads that would permit 
            // multiple independent incoming connections to your app simultaneously.
            //
            ResponderThread = new Thread(Responder);

            //
            // Command processing loop
            //
            StringBuilder sb = new StringBuilder("Commands:" + CRLF);
            sb.Append("  @h                       - this help text" + CRLF);
            sb.Append("  @listener callsign-ssid  - Create a second DataLinkProvider for callsign-ssid" + CRLF);
            sb.Append("  @con callsign-ssid [digi1] [digi2]" + CRLF);
            sb.Append("                           - Connect to remote station callsign-ssid using optional relays" + CRLF);
            sb.Append("  @mcon n callsign-ssid    - Connect T6EST-n to remote station callsign-ssid" + CRLF);
            sb.Append("  @xid                     - Initiate XID negotiation" + CRLF);
            sb.Append("  @data callsign-ssid text - Send 'text' to calllsign-ssid as a datagram packet" + CRLF);
            sb.Append("  @test callsign-ssid text - Send a TEST command to callsign-ssid & " + CRLF);
            sb.Append("                                wait 5 seconds for a response" + CRLF);
            sb.Append("  @disc                    - Disconnect from the currently connected station" + CRLF);
            sb.Append("  @mdisc n                 - Disconnect T6EST-n" + CRLF);
            sb.Append("  @digi callsign-ssid      - Create a digipeater at callsign-ssid" + CRLF);
            sb.Append("  @bye                     - Disconnect from the currently connected station & " + CRLF);
            sb.Append("                                and exit the program" + CRLF);
            sb.Append("  @koff                    - Send the Exit KISS mode sequence to the TNC" + CRLF);
            sb.Append("  (other text)             - Send as data to the connected station" + CRLF + CRLF);

            Console.WriteLine(sb.ToString());

            while (true && dlProvider.Enabled)
            {

                //
                // Command loop
                //
                Console.Write(">>> ");
                cmd = Console.ReadLine();
                if (cmd.StartsWith("@h"))
                {
                    Console.WriteLine(sb.ToString());
                    continue;
                }

                if (cmd.StartsWith("@xid"))
                {
                    Console.WriteLine("Initiate XID negotiation with remote.  status: " +
                        con.InitiateXID().ToString());
                    continue;
                }

                if (cmd.StartsWith("@test "))
                {
                    //
                    // Test command requested
                    //
                    String h = cmd.Substring(6);
                    Int32 q = h.IndexOf(" ");
                    String c1 = h.Substring(0, q);
                    h = h.Substring(q + 1);

                    Byte[] resp = dlProvider.LinkTest(c1, "", "",
                        Encoding.ASCII.GetBytes(h), 5000);
                    String respC = "(Timeout)";
                    if (resp != null)
                    {
                        respC = Encoding.ASCII.GetString(resp);
                    }
                    Console.WriteLine("Test response: " + respC);
                    continue;
                }

                if (cmd.StartsWith("@koff"))
                {
                    //
                    // Send the Kiss Exit command to the TNC
                    //
                    tncVHFChannel.SetKissModeOff();
                    continue;
                }

                if (cmd.StartsWith("@digi "))
                {
                    //
                    // Start a digipeater
                    //
                    String h = cmd.Substring(6);
                    DataLinkProvider dl = tncVHFChannel.CreateProvider(h);
                    dl.digipeatEnable = true;
                    dl.accessControlListType = DataLinkProvider.AccessControlListType.black;
                    dl.AccessControlListAdd("n6prw-3");
                    continue;
                }

                if (cmd.StartsWith("@data "))
                {
                    //
                    // Send UI datagram
                    //
                    String h = cmd.Substring(6);
                    Int32 q = h.IndexOf(" ");
                    String c1 = h.Substring(0, q);
                    h = h.Substring(q + 1);
                    dlProvider.SendDatagram(c1, Encoding.ASCII.GetBytes(h),
                        Frame.ProtocolVersion.V22);
                    Console.WriteLine("Datagram Sent: " + h);
                    continue;
                }

                if (cmd.StartsWith("@mcon "))
                {
                    //
                    // Connect to remote station
                    //
                    String[] h = cmd.Substring(6).Split(c);
                    Int32 m = 0;

                    if (h.Length == 2)
                    {
                        m = Convert.ToInt32(h[0]);
                        mcstat[m] = mcon[m].Connect(h[1]);
                        Console.WriteLine("Connect status: " + mcstat[m].ToString());
                    }
                    continue;
                }

                if (cmd.StartsWith("@con "))
                {
                    //
                    // Connect to remote station
                    //
                    cstat = Connection.ConnectionStatus.Busy;
                    String[] h = cmd.Substring(5).Split(c);

                    if (h.Length == 1)
                    {
                        cstat = con.Connect(h[0]);
                    }
                    if (h.Length == 2)
                    {
                        cstat = con.Connect(h[0], h[1]);
                    }
                    if (h.Length == 3)
                    {
                        cstat = con.Connect(h[0], h[1], h[2]);
                    }

                    Console.WriteLine("Connect status: " + cstat.ToString());
                    continue;
                }

                if (cmd.StartsWith("@listener "))
                {
                    //
                    // Start the listener we'll use for loopback testing
                    //
                    String h = cmd.Substring(10);
                    responderCallSign = h;
                    if (!ResponderThread.IsAlive)
                    {
                        ResponderThread.Start();
                    }
                    continue;
                }

                if (cmd.StartsWith("@disc"))
                {
                    //
                    // Disconnect from the remote station
                    //
                    cstat = con.Disconnect();
                    Console.WriteLine("Disconnected:  status: " + cstat.ToString());
                    continue;
                }
                if (cmd.StartsWith("@mdisc "))
                {
                    //
                    // Disconnect from the remote station
                    //
                    String[] h = cmd.Substring(7).Split(c);
                    Int32 m = 0;
                    if (h.Length == 1)
                    {
                        m = Convert.ToInt32(h[0]);
                        mcstat[m] = mcon[m].Disconnect();
                        Console.WriteLine("Disconnected:  status: " + mcstat[m].ToString());

                    }
                    continue;
                }


                if (cmd.StartsWith("@exit"))
                {
                    //
                    // Exit KISS mode
                    //
                    tncVHFChannel.SetKissModeOff();
                    Console.WriteLine("Kiss mode exited");
                    break;
                }

                if (cmd.StartsWith("@bye"))
                {
                    //
                    // Disconnect and close the program
                    //
                    cstat = con.Disconnect();
                    Console.WriteLine("Disconnected:  status: " + cstat.ToString());
                    break;
                }

                // everything else is treated as data

                if (!con.isConnected)
                {
                    Console.WriteLine("Not connected...");
                    continue;
                }
                else
                {
                    if (cmd.Length > 0)
                    {
                        con.Send(Encoding.ASCII.GetBytes(cmd + CR));
                    }
                    else
                    {
                        Console.WriteLine("Nothing to send");
                    }
                }
            }
            //con.Close();
            //dlProvider.Close();
            //tncVHFChannel.Close();

            //
            // Time to go.  Just shutting down the lower layer is sufficient.  
            // All the upper instances will be shut down as well.
            //
            tncComPort.Close();
            Console.Write("Press return to exit");
            cmd = Console.ReadLine();
        }

        //
        // Connection data reader thread
        // 
        static void Reader()
        {
            //
            // Read incoming bytes
            //
            Int32 count;

            //
            // Create a buffer to hold the incoming data
            //
            Byte[] inBuf = new Byte[8192];

            while (con.Enabled)
            {
                count = con.Recv(inBuf, true);
                if (count > 0)
                {
                    Console.Write(Support.GetString(Support.PackByte(inBuf, 0, count)));
                }
            }
        }

        //
        // Loopback responder thread
        //
        static String responderCallSign = "";

        static void Responder()
        {
            //
            // Initialize a second node for loopback testing
            //
            String g;
            ResponderThread.Name = "Responder thread for " + responderCallSign.ToUpper();
            Console.WriteLine(Thread.CurrentThread.Name + " starting");
            Byte[] tmpB = new Byte[8192];
            Int32 count = 0;
            Connection.ConnectionStatus cstat;

            //
            // Create the DLP and connection
            //
            DataLinkProvider recvDlProvider = tncVHFChannel.CreateProvider(responderCallSign);
            Connection tmpCon = recvDlProvider.CreateConnection();

            tmpCon.registerStateHandlers.ReportConnectionEvents +=
                new EventHandler<ConnectionEvents>(NotifyConnectionState);
            tmpCon.registerIndicationHandlers.ReportIndicationEvents +=
                new EventHandler<IndicationEvents>(NotifyIndicationEvents);

            while (tmpCon.Enabled)
            {
                //
                // Wait for someone to connect to this instance.  Calling the
                // function with 'true' will cause the function to return only
                // if we enter the Connected state.  Normally the function will 
                // return if either Connected or Disconnected
                //

                tmpCon.WaitForConnect(true);
                //
                // We're connected.  Perform any connection establishment work 
                // here.  Lower loop handles data transfer while this current 
                // connection is active
                // 
                //
                // Loopback to the sender whatever we received
                //
                do
                {
                    //
                    // Loop here while connection is active
                    //
                    count = tmpCon.Recv(tmpB, true);
                    if (count > 0)
                    {
                        g = Encoding.ASCII.GetString(Support.PackByte(tmpB, 0, count));
                        if (g.StartsWith("bye"))
                        {
                            Console.WriteLine("Received a 'bye' command from the other station, so DISC");
                            cstat = tmpCon.Disconnect();
                            Console.WriteLine("Disconnect status: " + cstat.ToString());
                        }
                        else
                        {
                            //
                            // Loop back what was sent
                            //
                            Console.WriteLine(Thread.CurrentThread.Name + " Echoing: " + g);
                            tmpCon.Send(Encoding.ASCII.GetBytes(g));
                        }
                    }
                } while (tmpCon.isConnected);
            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exiting");
        }

        //
        // Packet monitor receive thread
        //
        static void PacketMonitor()
        {
            //
            // Set up a listener to catch the raw packets
            //
            Support.DbgPrint(Thread.CurrentThread.Name + " starting");

            while (!tncVHFChannel.packetMonitorEnable.Equals(
                TNCChannel.PacketMonitorType.None) && (tncVHFChannel.Enabled))
            {
                String foo = tncVHFChannel.PacketMonitor();
                Console.WriteLine(foo);
            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exiting");
        }
    }
}
