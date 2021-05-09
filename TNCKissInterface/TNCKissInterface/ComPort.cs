using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace TNCKissInterface
{
    //
    // This class implements KISS framing and serial COM port IO.  Incoming/Outgoing 
    // data is stored in a KISS Buffer that includes the TNC Channel to send/recv from.
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    //[LicenseProviderAttribute(typeof(LicFileLicenseProvider))]
    public class COMPort
    {
        //#region Class variables

        String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);
        Int32 MAXBUF = 65536;           // 64K max recv buffer
        Int32 MAXQUEUESIZE = 1024;      // Max number of outstanding buffers in a queue

        //
        // KISS framing flags
        //
        const Byte FEND = 0xc0;        // Kiss frame delimeter byte
        const Byte FESC = 0xdb;        // Kiss frame esc byte
        const Byte TFEND = 0xdc;       // Kiss escaped frame delimeter 
        const Byte TFESC = 0xdd;       // Kiss escaped esc

        SerialPort port;

        //  KISS over TCP variables     (JNW Jan15)

        public bool TCPMode;
        public TcpClient client;

        Int32 TCPPort = 8100;           // (JNW APR15)
        String TCPHost = "127.0.0.1";

        NetworkStream netStream;
        byte[] myReadBuffer = new byte[1024];

        Boolean runFlag = true;

        BlockingQueue receivePortDataQueue;      // Get COM port data via delegate
        BlockingQueue sendQ;                     // Outbound queue
        BlockingQueue[] recvQs = new BlockingQueue[16];       // Each channel get it's own inbound queue

        Thread recvThread;
        Thread sendThread;

        SerialDataReceivedEventHandler comHandler;

        //
        // List of active Channels.
        //

        List<TNCChannel> tncChannelList = new List<TNCChannel>();

        Int32 _KISSFrameErrorCount = 0;

        Int32 _inboundQueueErrorCount = 0;
        Int32 _outboundQueueErrorCount = 0;

        public String escapedCharList = "";

        //
        // Initialize the list to NULL
        //
        Byte[] escapedByteList = null;

        public Int32 KISSFrameErrorCount
        {
            get { return _KISSFrameErrorCount; }
        }
        public Int32 inboundQueueErrorCount
        {
            get { return _inboundQueueErrorCount; }
        }
        public Int32 outboundQueueErrorCount
        {
            get { return _outboundQueueErrorCount; }
        }

        Boolean loopBackMode = false;       // Debug loopback flag

        //#endregion

        //#region Class constructors & public methods

        //private License license = null;

        //protected void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (license != null)
        //        {
        //            license.Dispose();
        //            license = null;
        //        }
        //    }
        //}

        //
        // Constructors
        //
        //public COMPort()//String comPort, Int32 baud)
        //{
        //    license = LicenseManager.Validate(typeof(COMPort), this);

        //    //ComPortInit(comPort, baud, false);
        //}

        public COMPort(SerialPort sPort)
        {
            COMPortInit(null, 0, false, "", 0, sPort);
        }

        public COMPort(String comPort, Int32 baud)
        {
            COMPortInit(comPort, baud, false, "", 0, null);            // JNW Apr15
        }

        //  Overload for KISS over TCP

        public COMPort(String comPort, String Host, Int32 Port)  // JNW Apr15
        {
            COMPortInit(comPort, 0, false, Host, Port, null);
        }

        public COMPort(String comPort, Int32 baud, Boolean loopback)
        {
            COMPortInit(comPort, baud, loopback, "", 0, null);         // JNW Apr15
        }

        public void SetEscapedCharsList(String sList)
        {
            Char[] s = { ',' };
            Int32 j = 0;
            String[] bList = sList.Trim().ToUpper().Split(s);
            String[] valid = new String[bList.Length];
            escapedByteList = null;

            foreach (String ss in bList)
            {
                if (IsValidHexByte(ss.Trim()))
                {
                    valid[j++] = ss.Trim();
                }
            }
            if (j > 0)
            {
                escapedByteList = new Byte[j];
                for (Int32 i = 0; i < j; i++)
                {
                    escapedByteList[i] = Convert.ToByte(valid[i], 16);
                }
            }
        }

        //
        // Public methods
        //
        //public void COMPortInit(String comPort, Int32 baud)
        //{
        //    COMPortInit(comPort, baud, false);
        //}

        void COMPortInit(String comPort, Int32 baud, Boolean loopback, String Host, Int32 Port, SerialPort sPort)            // JNW Apr15
        {
            //
            // Initialization
            // 
            loopBackMode = loopback;

            receivePortDataQueue = new BlockingQueue(MAXQUEUESIZE);        // Port level receive queue
            sendQ = new BlockingQueue(MAXQUEUESIZE);    // packet level send queue for all channels

            if (comPort != null && comPort.Equals("TCP"))
            {
                TCPMode = true;
                TCPPort = Port;             // JNW Apr15
                TCPHost = Host;
            }
            else
                TCPMode = false;

            if (TCPMode)
            {
                try
                {
                    client = new TcpClient(TCPHost, TCPPort);

                    netStream = client.GetStream();

                    // This will run in the background.
                    Task.Run(() =>
                    {
                        var count = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        myReadCallBack(count);
                    });

                }
                catch (Exception ex)
                {
                    Support.DbgPrint("TCP Connect Failed.  Ex: " + ex.Message);
                }
            }
            else
            {
                if (sPort == null)
                    port = new SerialPort(comPort, baud, Parity.None, 8, StopBits.One);
                else
                    port = sPort;

                // No flow control.  Upper AX.25 layer handles dropped data
                port.Handshake = Handshake.None;
                port.RtsEnable = true;

                if (!loopBackMode) // We don't use the serial port in loopback mode
                {
                    // Add the read event handler
                    //comHandler = new SerialDataReceivedEventHandler(SerialPortRead);
                    //port.DataReceived += comHandler;

                    if (sPort == null)
                    {
                        port.Open();
                    }

                    //port.ReadTimeout = 1;
                    port.DiscardInBuffer();
                    Task.Run(() =>
                    { 
                        SerialPortRead();
                    });
                }
            }

            if (client != null || port != null)
            {
                recvThread = new Thread(recvFromPort);      // Threads handle com port I/O
                recvThread.Name = "ComPort Receive Thread for Port: " + comPort;

                sendThread = new Thread(sendToPort);
                sendThread.Name = "ComPort Transmit Thread for Port: " + comPort;

                recvThread.Start();     // Light 'em up
                sendThread.Start();
            }
        }

        public TNCChannel CreateChannel(Int32 channelNumber)
        {
            TNCChannel tmpChannel;
            //
            // Create a TNC channel on top of this com port
            //
            if (channelNumber < 0 || channelNumber > 15)
            {
                throw new Exception("Channel number must be between 0 and 15 inclusive");
            }
            tmpChannel = new TNCChannel(this, channelNumber);
            tncChannelList.Add(tmpChannel);
            recvQs[channelNumber] = new BlockingQueue(MAXQUEUESIZE);    // One receive queue for each channel
            return tmpChannel;
        }


        //public void AddChannel(TNCChannel channel, Int32 channelNumber)
        //{
        //    tncChannelList.Add(channel);
        //    if ((channelNumber >= 0) && (channelNumber <= 15))
        //    {
        //        recvQs[channelNumber] = new BlockingQueue(MAXQUEUESIZE);    // One receive queue for each channel
        //    }
        //}

        public Boolean Enabled
        {
            //
            // Returns false if this class instance has been shut down.
            //
            get { return runFlag; }
        }

        public void Close()
        {
            //
            // Time to go...
            //

            runFlag = false;

            while (tncChannelList.Count > 0)
            {
                TNCChannel tmpC = tncChannelList[0];
                tmpC.Close();
                Thread.Sleep(200);   // Give Close a chance to finish.  W4PHS 23-Jan-2015
                tncChannelList.Remove(tmpC);
            }

            //
            // Close down any outbound queues.  Any threads waiting on those queues will be signaled
            //

            for (Int32 i = 0; i < 16; i++)
            {
                if (recvQs[i] != null)
                {
                    recvQs[i].enabled = false;
                    recvQs[i] = null;
                }
            }

            if (!loopBackMode && !TCPMode) // We don't use the serial port in loopback mode
            {
                // Remove the event handler
               port.DataReceived -= comHandler;
            }

            receivePortDataQueue.enabled = false;
            sendQ.enabled = false;

            if (TCPMode && client != null )                    // (JNW Feb15)
            {
                // Close TCP Connection

                netStream.Dispose();
                client.Close();
                Thread.Sleep(200);
            }

            if (!loopBackMode && !TCPMode) // We don't use the serial port in loopback mode
            {
                Thread.Sleep(200);
               port.DiscardInBuffer();
               port.DiscardOutBuffer();

               port.Close();
               Thread.Sleep(200);   // Give Close a chance to finish.  W4PHS 23-Jan-2015
               port.Dispose();

               Thread.Sleep(200);   // Give Close a chance to finish.  W4PHS 23-Jan-2015

            }
        }

        public Byte[] Recv(Int32 channel)
        {
            //
            // Get the next incoming frame from the TNC
            //
            return (Byte[])recvQs[channel].Dequeue();    // Pull a packet from the receive queue
        }

        public Boolean Send(Int32 channel, Byte[] buf)
        {
            //
            // Send data stream to the TNC (command 0)
            //
            return SendCmd(channel, 0, buf);
        }

        public Boolean SendCmd(Int32 channel, Int32 cmd, Byte[] buf)
        {
            //
            // Send incoming packet to the TNC via the COM port
            //
            Boolean ret = true;
            if (buf.Length > (MAXBUF >> 1))
            {
                _outboundQueueErrorCount++;
                ret = false;
            }

            //
            // Insert the command
            //

            if (!(ret = sendQ.Enqueue(new KissBuf(channel, cmd, buf))))
            {
                // Outbound Queue is full, so just drop the packet... c'est la vie.
                _outboundQueueErrorCount++;
                ret = false;
            }

            return ret;
        }
        
        //  Overload for AckMode                (JNW Jan15)

        public Boolean SendCmd(Int32 channel, Int32 cmd, Frame.TransmitFrame transmitFrame)
        {
            Byte[] buf = transmitFrame.ibuf;

            //
            // Send incoming packet to the TNC via the COM port
            //
            Boolean ret = true;
            if (buf.Length > (MAXBUF >> 1))
            {
                _outboundQueueErrorCount++;
                ret = false;
            }

            //
            // Insert the command
            //

            if (!(ret = sendQ.Enqueue(new KissBuf(channel, transmitFrame))))
            {
                // Outbound Queue is full, so just drop the packet... c'est la vie.
                _outboundQueueErrorCount++;
                ret = false;
            }

            return ret;
        }

        //#endregion

        //#region COM port communication thread routines

        //
        // COM port receive thread
        //

        private void recvFromPort()
        {
            Boolean inFrame = false;
            Boolean doUnescape = false;
            Boolean gotChannel = false;
            Byte[] rcvBuf = new Byte[MAXBUF];
            Byte[] portBuf;
            Int32 count = 0;
            Int32 channel = 0;
            Int32 command  = 0;                       // KISS Command Byte (JNW Jan15)


            Support.DbgPrint(Thread.CurrentThread.Name + " starting");

            while (runFlag)
            {
                //
                // COM port receive process loop
                //
                try
                {
                    portBuf = (Byte[])receivePortDataQueue.Dequeue();
                    if (portBuf == null)
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

                foreach (byte tmp in portBuf)
                {
                    if (inFrame)
                    {
                        //
                        // In a frame  
                        //
                        if (doUnescape)
                        {
                            //
                            // Process escaped characters
                            // Characters other than TFEND and TFESC are discarded
                            //
                            if (tmp == TFEND)
                            {
                                rcvBuf[count++] = FEND;
                            }
                            else if (tmp == TFESC)
                            {
                                rcvBuf[count++] = FESC;
                            }
                            else
                            {
                                //
                                // Ignore the FESC character for other characters.
                                //
                                rcvBuf[count++] = tmp;
                            }
                            doUnescape = false;
                            continue;
                        }

                        if (tmp == FESC)
                        {
                            // Process next character as escaped
                            doUnescape = true;
                            continue;
                        }

                        if (tmp == FEND)
                        {
                            if (!gotChannel)
                            {
                                //
                                // looks like back-to-back FEND characters,
                                // so bump the frame error count and keep going
                                //
                                //_KISSFrameErrorCount++;
                                continue;
                            }

                            if (count > 0)
                            {

                                //
                                // End of frame, so send up the packet
                                //
                                if (recvQs[channel] == null)
                                {
                                    // Bad channel received
                                    _KISSFrameErrorCount++;
                                }
                                else
                                {
                                    if (!recvQs[channel].Enqueue((object)Support.PackByte(rcvBuf, 0, count)))
                                    {
                                        // Inbound Queue is full, so just drop the packet... c'est la vie.
                                        _inboundQueueErrorCount++;
                                    }
                                }
                            }

                            count = 0;      // Reset buffer len
                            inFrame = true;  // Set to True to handle the case where the same FEND byte is used for both end and start
                            gotChannel = false;
                            continue;
                        }

                        if (count >= MAXBUF)
                        {
                            //
                            // Strange happenings on the channel.  Frame should
                            // never get this big.  Reset & continue.
                            //
                            _KISSFrameErrorCount++;
                            count = 0;
                            inFrame = false;
                            gotChannel = false;
                            continue;
                        }

                        if (!gotChannel)
                        {
                            //
                            // Get channel # from upper nibble of first byte
                            //

                            command = tmp & 0x0f;       // Extract Command Byte     (JNW Jan15)
                            channel = Convert.ToInt32(tmp >> 4);
                            gotChannel = true;
                            continue;
                        }

                        rcvBuf[count++] = tmp;      // Normal character
                    }
                    else
                    {
                        // Not in a frame.  Ignore all but FEND
                        if (tmp == FEND)
                        {
                            //
                            // Start of frame
                            //
                            inFrame = true;
                        }
                    }
                }
            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exit");
        }

        //
        // Com port send thread
        //

        void sendToPort()
        {
            KissBuf kBuf = null;
            Byte[] tmpB = new Byte[MAXBUF];
            Byte[] buf;
            Int32 count = 0;

            Support.DbgPrint(Thread.CurrentThread.Name + " starting");

            while (runFlag)      /* 1/22/2015 W4PHS */
            {
                //
                // COM port send process loop
                //
                try
                {
                    kBuf = (KissBuf)sendQ.Dequeue();
                    if (kBuf == null)
                    {
                        continue;
                    }        
                }
                catch
                {
                    Support.DbgPrint(Thread.CurrentThread.Name + " sendQ.Dequeue Exception");
                    continue;
                }

                //
                // Add Framing character and channel/command information to the start of the buffer
                //
                count = 0;

                tmpB[count++] = FEND;

                //  If Ackmode specified, send Header           (JNW Jan15)

                if (kBuf.Frame != null)
                {
                    //  Ackmode FOrmat
                    // See if Acxkmode Needed

                    buf = kBuf.Frame.ibuf;

                    if (kBuf.Frame.AckModeID != 0)
                    {
                        tmpB[count++] = (Byte)((kBuf.chan << 4) | 0x0c);
                        tmpB[count++] = (Byte)(kBuf.Frame.AckModeID >> 8);
                        tmpB[count++] = (Byte)(kBuf.Frame.AckModeID & 0xff);
                    }
                    else
                    {
                        tmpB[count++] = (Byte)((kBuf.chan << 4) | kBuf.cmd);

                    }
                }
                else
                {
                    //  Old (Non-AckMode) format

                    buf = kBuf.buf;
                    if (kBuf.cmd == 0x0f)
                        tmpB[count++] = 0xff;       // Kiss Mode off
                    else
                        tmpB[count++] = (Byte)((kBuf.chan << 4) | kBuf.cmd);
                }

                if (kBuf.cmd != 0x0f)
                {
                    //
                    // PRW change 10/10/2010 - Move the fill buffer code inside the else clause.  Otherwise our kiss exit 
                    // sequence ended up being: C0 FF 00 C0 rather than the correct value of: C0 FF C0
                    //
                    foreach (Byte b in buf)
                    {
                        //
                        // In addition to escaping the normal FEND & FESC bytes, we will also
                        // insert a dummy escape sequence for Capital C.  This will break up the string "TC 0 <CR>" that
                        // screws up the D710 TNC while in KISS mode.  Since it is an invalid escape, it should be discarded
                        // at the remote end.
                        //
                        if (Escaped(b))
                        {
                            //
                            // This byte is on the list of characters to escape
                            //
                            tmpB[count++] = FESC;
                        }

                        if ((b == FEND) || (b == FESC))
                        {
                            if (b == FEND)
                            {
                                tmpB[count++] = FESC;
                                tmpB[count++] = TFEND;
                            }
                            if (b == FESC)
                            {
                                tmpB[count++] = FESC;
                                tmpB[count++] = TFESC;
                            }
                        }
                        else
                        {
                            tmpB[count++] = b;
                        }
                    }
                }

                tmpB[count++] = FEND;

                try
                {
                    if (loopBackMode)
                    {
                        // Send data back to receive port
                        if (!receivePortDataQueue.Enqueue((Object)Support.PackByte(tmpB, 0, count)))
                        {
                            // Inbound Queue is full, so just drop the packet... c'est la vie.
                            _inboundQueueErrorCount++;
                        }
                    }
                    else
                    {
                        if (TCPMode)
                            netStream.Write(tmpB, 0, count);
                        else
                            port.Write(tmpB, 0, count);
                    }
                }
                catch
                {
                }

                // Close if connection lost

                if (TCPMode)
                    runFlag = client.Connected;
                else
                    runFlag = port.IsOpen;          /* 1/22/2015 W4PHS */

            }
            Support.DbgPrint(Thread.CurrentThread.Name + " exit");
        }

        //  Socket Read Callback        (JNW Jan15)

        public void myReadCallBack(int count)
        {
            if (runFlag == false || count == 0)       // Socket Closed?
                return;

            try
            {
                if (!receivePortDataQueue.Enqueue((Object)(Support.PackByte(myReadBuffer, 0, count))))
                {
                    // Inbound Queue is full, so just drop the packet... c'est la vie.
                    Support.DbgPrint("dropped inbound bytes.  Count = " + count.ToString());
                  _inboundQueueErrorCount++;
                }

                Task.Run(() =>
                {
                    var count = netStream.Read(myReadBuffer, 0, myReadBuffer.Length);
                    myReadCallBack(count);
                });
            }
            catch (Exception ex)
            {
                Support.DbgPrint("Exception during tcp port read.  Ex: " + ex.Message);
            }
        }

        //
        // COM port read event handler delegate
        //

        void SerialPortRead()
        {
            Byte[] tmpBuf = new Byte[2048];
            Int32 numBytes;
            Int32 count;

            try
            {
                count = port.Read(tmpBuf, 0, tmpBuf.Length);

                if (!receivePortDataQueue.Enqueue((Object)(Support.PackByte(tmpBuf, 0, count))))
                {
                    // Inbound Queue is full, so just drop the packet... c'est la vie.
                    Support.DbgPrint("dropped inbound bytes.  Count = " + count.ToString());
                    _inboundQueueErrorCount++;
                }

                if (runFlag)
                {
                    Task.Run(() =>
                    {
                        SerialPortRead();
                    });
                }
            }
            catch (Exception ex)
            {
                Support.DbgPrint("Exception during serial port read.  Ex: " + ex.Message);
            }
        }

        Boolean Escaped(Byte b)
        {
            if (escapedCharList.Contains(Convert.ToChar(b).ToString()))
            {
                return true;
            }

            if (escapedByteList != null)
            {
                for (Int32 i = 0; i < escapedByteList.Length; i++)
                {
                    if (b == escapedByteList[i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        Boolean IsValidHexByte(String sByte)
        {
            String tmpB = "0123456789ABCDEF";
            sByte = sByte.Trim().ToUpper();

            if (sByte.Length < 1 || (sByte.Length > 2))
            {
                return false;
            }

            for (Int32 i = 0; i < sByte.Length; i++)
            {
                if (!tmpB.Contains(sByte.Substring(i, 1)))
                {
                    return false;
                }
            }
            return true;
        }

        //#endregion

        //  Modified for AckMode            (JNW Jan15)

        public class KissBuf
        {
            public Int32 chan;      // Channel
            public Frame.TransmitFrame Frame;
            public Int32 cmd;       // Command type
            public Byte[] buf;      // Buffer

            public KissBuf(Int32 channel, int command, Byte[] buffer)
            {
                chan = channel;
                Frame = null;
                cmd = command;
                buf = buffer;
            }
            public KissBuf(Int32 channel, Frame.TransmitFrame tFrame)
            {
                chan = channel;
                Frame = tFrame;
            }

        }
    }
}
