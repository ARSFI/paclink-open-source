using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TNCKissInterface
{
    public static class HDLCFramer
    {
        public static Int32 HDLCFrameErrorCount = 0;
        public static Int32 HDLCCRCErrorCount = 0;

        const Int32 MAXBUF = 65536;          // 64K max recv buffer

        public static Boolean debugDumpFrame = true;

        static String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);

        //
        // HDLC framing flags
        //
        const Byte HDLC_ESC_ASYNC = 0x7d;   // Async HDLC escape character
        const Byte HDLC_FLAG = 0x7e;        // Async HDLC frame delimeter
        const Byte HDLC_ESC_XOR = 0x20;     // Async HDLC special character invert
        const Byte C = 0x43;    // Temp.. Escape the letter 'C'.  Needed for TM-D710

        //
        // CRC flags & gen table
        //
        const Int32 FCS_START = 0xffff; // Starting bit string for FCS calculation
        const Int32 FCS_FINAL = 0xf0b8;	// FCS when summed over frame and sender FCS

        static Int32[] FcsTable = {
                              0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
                              0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
                              0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
                              0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
                              0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
                              0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
                              0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
                              0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
                              0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
                              0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
                              0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
                              0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
                              0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
                              0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
                              0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
                              0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
                              0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
                              0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
                              0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
                              0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
                              0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
                              0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
                              0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
                              0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
                              0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
                              0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
                              0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
                              0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
                              0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
                              0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
                              0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
                              0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78 };

        public static Byte[] BuildHDLCFrame(Byte[] buf)
        {
            return BuildHDLCFrame(buf, 0, buf.Length);
        }

        public static Byte[] BuildHDLCFrame(Byte[] buf, Int32 start, Int32 count)
        {
            Int32 crc;
            Int32 l;
            Byte[] hBuf = new Byte[MAXBUF];

            //
            // Add the CRC & wrap buffer in HDLC frame
            //

            crc = GenCRC(buf, start, count);
            l = count + 2;
            Array.Resize(ref buf, l + start);    //Make space for the CRC &

            buf[(l + start) - 2] = Convert.ToByte(crc & 0xff);
            buf[(l + start) - 1] = Convert.ToByte((crc >> 8) & 0xff);

            buf = Stuff(buf, start, l); // Stuff any escape bytes

            l = buf.Length + 2;                     // Add space for the HDLC_FLAG bytes on the now stuffed buffer
            buf.CopyTo(hBuf, 1);
            hBuf[0] = HDLC_FLAG;
            hBuf[l - 1] = HDLC_FLAG;

            Array.Resize(ref hBuf, l);

            return hBuf;
        }

        //public static Byte [] ParseHDLCFrame(Byte[] buf)
        //{
        //    String tmp;
        //    return ParseHDLCFrame(buf, 0, buf.Length, out tmp);
        //}
        //public static Byte [] ParseHDLCFrame(Byte[] buf, out String packetDump)
        //{
        //    return ParseHDLCFrame(buf, 0, buf.Length, out packetDump);
        //}
        //public static Byte [] ParseHDLCFrame(Byte[] buf, Int32 start, Int32 count, out String packetDump)
        //{
        //    //
        //    // HDLC buffer received
        //    //
        //    //HDLCFrame frameBuf = new HDLCFrame();
        //    Byte crcLo = 0;
        //    Byte crcHi = 0;
        //    Int32 crc;

        //    if (true)

        //    //if ((buf[0] == HDLC_FLAG) && 
        //    //    (buf[count - 1] == HDLC_FLAG))
        //    {

        //        // HDLC wrapper OK
        //        //Byte[] tmpBuf = UnStuff(buf, 1, count - 2);  // Remove byte stuffing
        //        Byte[] tmpBuf = buf;
        //        count = tmpBuf.Length;

        //        //crc = GenCRC(tmpBuf, 0, count - 2);   // don't include the crc in the crc

        //        //crcLo = tmpBuf[count - 2];
        //        //crcHi = tmpBuf[count - 1];
        //        //Array.Resize(ref tmpBuf, count - 2);

        //        //frameBuf.CRC = crc;
        //        //frameBuf.rawBuf = tmpBuf;

        //        //if ((Convert.ToByte(crc & 0xff) == crcLo) &&
        //        //    (Convert.ToByte((crc >> 8) & 0xff) == crcHi))
        //        {
        //            // CRC OK
        //            //frameBuf.frameOK = true;
        //        }
        //        //else
        //        //{
        //            // CRC error
        //          //  frameBuf.error = "CRC Error - Packet CRC:" + crcHi.ToString("x2") + crcLo.ToString("x2");
        //          //  HDLCCRCErrorCount++;
        //       // }
        //    //}
        //    //else
        //    //{
        //    //    frameBuf.error = "Invalid HDLC framing bytes - Start:" + buf[0].ToString("x2") + " End:" + buf[count-1].ToString("x2");
        //    //    frameBuf.rawBuf = buf;
        //    //    HDLCFrameErrorCount++;
        //    //}

        //    //packetDump = DumpRawFrame(frameBuf);

        //    //return frameBuf;

        //}


        #region Debug routines

        //
        // Frame Debug routines
        //

 

        #endregion

        #region CRC routines

        //
        // CRC Routines
        //

        private static Int32 GenCRC(Byte[] buf)
        {
            return GenCRC(buf, 0, buf.Length);
        }

        private static Int32 GenCRC(Byte[] buf, Int32 start, Int32 count)
        {
            Int32 crc = FCS_START;
            for (Int32 i = start; i < count + start; i++)
            {
                crc = Fcs(crc, buf[i]);
            }

            return (crc ^ 0xffff);
        }

        private static Int32 Fcs(Int32 f, Int32 c)
        {
            return ((f >> 8) ^ FcsTable[(f ^ c) & 0x00ff]);
        }

        #endregion

        #region Async HDLC byte stuffing routines

        private static Byte[] Stuff(Byte[] buf)
        {
            return Stuff(buf, 0, buf.Length);
        }

        private static Byte[] Stuff(Byte[] buf, Int32 start, Int32 count)
        {
            //
            // Add byte stuffing escape characters to buffer
            //

            Int32 j = 0;
            Byte xorMask = 0;
            Byte[] retBuf = new Byte[2 * count];    // worst case

            for (Int32 i = start; i < count + start; i++)
            {
                if ((buf[i] == HDLC_ESC_ASYNC) || (buf[i] == HDLC_FLAG) || (buf[i] == C))
                {
                    retBuf[j++] = HDLC_ESC_ASYNC;
                    xorMask = HDLC_ESC_XOR;
                }
                retBuf[j++] = (Byte)(buf[i] ^ xorMask);
                xorMask = 0;
            }

            Array.Resize(ref retBuf, j);
            return retBuf;
        }

        private static Byte[] UnStuff(Byte[] buf)
        {
            return UnStuff(buf, 0, buf.Length);
        }

        private static Byte[] UnStuff(Byte[] buf, Int32 start, Int32 count)
        {
            //
            // Remove byte stuffing escape characters from buffer
            //

            Int32 j = 0;
            Byte xorMask = 0;
            Byte[] retBuf = new Byte[count];

            for (Int32 i = start; i < count + start; i++)
            {
                if (buf[i] == HDLC_ESC_ASYNC)
                {
                    i++;
                    xorMask = HDLC_ESC_XOR;
                }
                retBuf[j++] = (Byte)(buf[i] ^ xorMask);
                xorMask = 0;
            }

            Array.Resize(ref retBuf, j);
            return retBuf;
        }

        #endregion

        public class HDLCFrame
        {
            public Boolean frameOK = false;
            public Int32 CRC = 0;
            public String error;
            public Byte[] rawBuf;
        }
    }
}
