using System;

namespace TNCKissInterface
{
    public class Compression
    {
        //
        // lzh .net Class library v1.0
        // Ported to C# by Peter Woods 08/22/2008 from the original
        // Pascal version.
        //
        //--------------Original Header Below----------------- 
        // 
        //  LZHUF.C English version 1.0
        //  Based on Japanese version 29-NOV-1988
        //  LZSS coded by Haruhiko OKUMURA
        //  Adaptive Huffman Coding coded by Haruyasu YOSHIZAKI
        //  Edited and translated to English by Kenji RIKITAKE
        //  Converted to Turbo Pascal 5.0
        //  by Peter Sawatzki with assistance of Wayne Sullivan
        //

        //
        // Constants
        //

        //
        // Synchonization object to support multi threading
        //
        static Object LZSync = new Object();
        const Int32 MaxBuf = 10 * 1024 * 1024;  // Set woring buffers to 10 MB
        const Int32 MaxUserBuf = MaxBuf / 10;     // Limit user buffer to 1 MB

        const Int32 N = 2048; //was 4096 in original code;        //{Size of string buffer}
        const Int32 F = 60;          //{60 Size of look-ahead buffer}
        const Int32 Threshold = 2;
        const Int32 NodeNIL = N;     //{End of tree's node}
        const Int32 TBSize = N + F;     //Was N+F-1, but the InsertNode function references location N+F-1, which would would be out-of-bounds;

        //
        // Huffman coding parameters
        //
        const Int32 NChar = (256 - Threshold) + F;

        //
        // character code = 0..N_CHAR-1
        //
        const Int32 T = (NChar * 2) - 1;  //{Size of table}
        const Int32 R = T - 1;          //{root position}
        const Int32 MaxFreq = 0x8000;   //{update when cumulative frequency reaches to this value}

        //const Int32 CRCPoly = 0x00018005;   //CRC-16
        //const Int32 CRCPoly = 0x00011021;   //CRC-16
        //const Int32 CRCPoly = 0x0001c0c1;   //CRC-16
        const Int32 CRCPoly = 0x00018408;

        //
        // Allocate fixed size buffers
        //
        static Byte[] textBuf = new Byte[TBSize];
        static Int32[] lSon = new Int32[N + 1]; //N + 256 + 1];       // Increased from N+1
        static Int32[] dad = new Int32[N + 1]; //N + 256 + 1];        // Increased from N+1
        static Int32[] rSon = new Int32[N + 256 + 1];
        static Int32[] freq = new Int32[T + 1];             // Cumulative freq table
        //
        // Pointing children nodes (son[], son[] + 1)
        //
        static Int32[] son = new Int32[T];

        //
        // Pointing parent nodes. area [T..(T + N_CHAR - 1)] are pointers for leaves
        //
        static Int32[] prnt = new Int32[T + NChar];

        //
        // Tables for encoding/decoding upper 6 bits of sliding dictionary pointer
        // encoder table}
        //
        static Byte[] p_len =
        {0x03,0x04,0x04,0x04,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x06,0x06,0x06,0x06,
         0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,
         0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,
         0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08};

        static Byte[] p_code =
        {0x00,0x20,0x30,0x40,0x50,0x58,0x60,0x68,0x70,0x78,0x80,0x88,0x90,0x94,0x98,0x9C,
         0xA0,0xA4,0xA8,0xAC,0xB0,0xB4,0xB8,0xBC,0xC0,0xC2,0xC4,0xC6,0xC8,0xCA,0xCC,0xCE,
         0xD0,0xD2,0xD4,0xD6,0xD8,0xDA,0xDC,0xDE,0xE0,0xE2,0xE4,0xE6,0xE8,0xEA,0xEC,0xEE,
         0xF0,0xF1,0xF2,0xF3,0xF4,0xF5,0xF6,0xF7,0xF8,0xF9,0xFA,0xFB,0xFC,0xFD,0xFE,0xFF};

        //
        // Decoder table
        //
        static Byte[] d_code =
        {0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
         0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
         0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,0x01,
         0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,0x02,
         0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,
         0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,
         0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,
         0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x09,0x09,0x09,0x09,0x09,0x09,0x09,0x09,
         0x0A,0x0A,0x0A,0x0A,0x0A,0x0A,0x0A,0x0A,0x0B,0x0B,0x0B,0x0B,0x0B,0x0B,0x0B,0x0B,
         0x0C,0x0C,0x0C,0x0C,0x0D,0x0D,0x0D,0x0D,0x0E,0x0E,0x0E,0x0E,0x0F,0x0F,0x0F,0x0F,
         0x10,0x10,0x10,0x10,0x11,0x11,0x11,0x11,0x12,0x12,0x12,0x12,0x13,0x13,0x13,0x13,
         0x14,0x14,0x14,0x14,0x15,0x15,0x15,0x15,0x16,0x16,0x16,0x16,0x17,0x17,0x17,0x17,
         0x18,0x18,0x19,0x19,0x1A,0x1A,0x1B,0x1B,0x1C,0x1C,0x1D,0x1D,0x1E,0x1E,0x1F,0x1F,
         0x20,0x20,0x21,0x21,0x22,0x22,0x23,0x23,0x24,0x24,0x25,0x25,0x26,0x26,0x27,0x27,
         0x28,0x28,0x29,0x29,0x2A,0x2A,0x2B,0x2B,0x2C,0x2C,0x2D,0x2D,0x2E,0x2E,0x2F,0x2F,
         0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,0x3E,0x3F};

        static Byte[] d_len =
        {0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,
         0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,0x03,
         0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,
         0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,
         0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,0x04,
         0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,
         0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,
         0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,
         0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,0x05,
         0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,
         0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,
         0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,0x06,
         0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,
         0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,
         0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,0x07,
         0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08,0x08};


        static UInt16[] CRCTable = 
        {0x0000,0x1021,0x2042,0x3063,0x4084,0x50a5,0x60c6,0x70e7,
         0x8108,0x9129,0xa14a,0xb16b,0xc18c,0xd1ad,0xe1ce,0xf1ef,
         0x1231,0x0210,0x3273,0x2252,0x52b5,0x4294,0x72f7,0x62d6,
         0x9339,0x8318,0xb37b,0xa35a,0xd3bd,0xc39c,0xf3ff,0xe3de,
         0x2462,0x3443,0x0420,0x1401,0x64e6,0x74c7,0x44a4,0x5485,
         0xa56a,0xb54b,0x8528,0x9509,0xe5ee,0xf5cf,0xc5ac,0xd58d,
         0x3653,0x2672,0x1611,0x0630,0x76d7,0x66f6,0x5695,0x46b4,
         0xb75b,0xa77a,0x9719,0x8738,0xf7df,0xe7fe,0xd79d,0xc7bc,
         0x48c4,0x58e5,0x6886,0x78a7,0x0840,0x1861,0x2802,0x3823,
         0xc9cc,0xd9ed,0xe98e,0xf9af,0x8948,0x9969,0xa90a,0xb92b,
         0x5af5,0x4ad4,0x7ab7,0x6a96,0x1a71,0x0a50,0x3a33,0x2a12,
         0xdbfd,0xcbdc,0xfbbf,0xeb9e,0x9b79,0x8b58,0xbb3b,0xab1a,
         0x6ca6,0x7c87,0x4ce4,0x5cc5,0x2c22,0x3c03,0x0c60,0x1c41,
         0xedae,0xfd8f,0xcdec,0xddcd,0xad2a,0xbd0b,0x8d68,0x9d49,
         0x7e97,0x6eb6,0x5ed5,0x4ef4,0x3e13,0x2e32,0x1e51,0x0e70,
         0xff9f,0xefbe,0xdfdd,0xcffc,0xbf1b,0xaf3a,0x9f59,0x8f78,
         0x9188,0x81a9,0xb1ca,0xa1eb,0xd10c,0xc12d,0xf14e,0xe16f,
         0x1080,0x00a1,0x30c2,0x20e3,0x5004,0x4025,0x7046,0x6067,
         0x83b9,0x9398,0xa3fb,0xb3da,0xc33d,0xd31c,0xe37f,0xf35e,
         0x02b1,0x1290,0x22f3,0x32d2,0x4235,0x5214,0x6277,0x7256,
         0xb5ea,0xa5cb,0x95a8,0x8589,0xf56e,0xe54f,0xd52c,0xc50d,
         0x34e2,0x24c3,0x14a0,0x0481,0x7466,0x6447,0x5424,0x4405,
         0xa7db,0xb7fa,0x8799,0x97b8,0xe75f,0xf77e,0xc71d,0xd73c,
         0x26d3,0x36f2,0x0691,0x16b0,0x6657,0x7676,0x4615,0x5634,
         0xd94c,0xc96d,0xf90e,0xe92f,0x99c8,0x89e9,0xb98a,0xa9ab,
         0x5844,0x4865,0x7806,0x6827,0x18c0,0x08e1,0x3882,0x28a3,
         0xcb7d,0xdb5c,0xeb3f,0xfb1e,0x8bf9,0x9bd8,0xabbb,0xbb9a,
         0x4a75,0x5a54,0x6a37,0x7a16,0x0af1,0x1ad0,0x2ab3,0x3a92,
         0xfd2e,0xed0f,0xdd6c,0xcd4d,0xbdaa,0xad8b,0x9de8,0x8dc9,
         0x7c26,0x6c07,0x5c64,0x4c45,0x3ca2,0x2c83,0x1ce0,0x0cc1,
         0xef1f,0xff3e,0xcf5d,0xdf7c,0xaf9b,0xbfba,0x8fd9,0x9ff8,
         0x6e17,0x7e36,0x4e55,0x5e74,0x2e93,0x3eb2,0x0ed1,0x1ef0};

        //
        // Processing buffers
        //
        static Byte[] inBuf = new Byte[MaxBuf];
        static Byte[] outBuf = new Byte[MaxBuf];

        static Int32 inPtr = 0;
        static Int32 inEnd = 0;
        static Int32 outPtr = 0;
        //static Int32 outEnd = 0;

        static Int32 CRC;
        static Boolean EncDec = false;      // true for Encode, false for Decode

        static UInt16 getBuf = 0;
        static Int32 getLen = 0;
        static UInt16 putBuf = 0;
        static Int32 putLen = 0;

        static Int32 textSize = 0;
        static Int32 codeSize = 0;

        static Int32 matchPosition = 0;
        static Int32 matchLength = 0;

        //
        // Main entry points
        //
        public static Int32 Encode(Byte[] iBuf, ref Byte[] oBuf, Boolean prependCRC)
        {
            UInt16 tmp;
            return Encode(iBuf, ref oBuf, out tmp, prependCRC);
        }

        public static Int32 Encode(Byte[] iBuf, ref Byte[] oBuf, out UInt16 retCRC)
        {
            return Encode(iBuf, ref oBuf, out retCRC, false);
        }

        public static Int32 Encode(Byte[] iBuf, ref Byte[] oBuf, out UInt16 retCRC, Boolean prependCRC)
        {
            //
            // Encoding/Compressing
            //
            Int32 i;
            Int32 c;
            Int32 len;
            Int32 r;
            Int32 s;
            Int32 last_match_length;
            Int32 j = 0;

            if (iBuf.Length > MaxUserBuf)
            {
                throw new Exception("Input buffer size ( " + iBuf.Length.ToString() + ") exceeds max buffer size of: " + MaxUserBuf.ToString());
            }

            lock (LZSync)
            {
                //
                // The lock makes the code thread-safe
                //
                Init();
                EncDec = true;

                for (i = 0; i < iBuf.Length; i++)
                {
                    inBuf[inEnd++] = iBuf[i];
                }

                putc((Byte)(inEnd & 0xff));
                putc((Byte)((inEnd >> 8) & 0xff));
                putc((Byte)((inEnd >> 16) & 0xff));
                putc((Byte)((inEnd >> 24) & 0xff));

                codeSize += 4;

                if (inEnd == 0)
                {
                    oBuf = new Byte[0];
                    retCRC = 0;
                    return codeSize;
                }
                textSize = 0;
                StartHuff();
                InitTree();
                s = 0;
                r = N - F;
                for (i = 0; i < r; i++)
                {
                    textBuf[i] = (Byte)0x20;            //fillchar(text_buf[0],r,' ');
                }

                len = 0;
                while ((len < F) && (inPtr < inEnd))
                {
                    textBuf[r + len] = getc();
                    len++;
                }
                textSize = len;
                for (i = 1; i <= F; i++)
                {
                    InsertNode(r - i);
                }
                InsertNode(r);
                do
                {
                    if (matchLength > len)
                    {
                        matchLength = len;
                    }
                    if (matchLength <= Threshold)
                    {
                        matchLength = 1;
                        EncodeChar(textBuf[r]);
                    }
                    else
                    {
                        EncodeChar((255 - Threshold) + matchLength);
                        EncodePosition(matchPosition);
                    }
                    last_match_length = matchLength;
                    i = 0;
                    while ((i < last_match_length) && (inPtr < inEnd))
                    {
                        i++;
                        DeleteNode(s);
                        c = getc();
                        textBuf[s] = (Byte)c;
                        if (s < F - 1)
                        {
                            textBuf[s + N] = (Byte)c;
                        }
                        s = (s + 1) & (N - 1);
                        r = (r + 1) & (N - 1);
                        InsertNode(r);
                    }
                    textSize += i;
                    while (i < last_match_length)
                    {
                        i++;
                        DeleteNode(s);
                        s = (s + 1) & (N - 1);
                        r = (r + 1) & (N - 1);
                        len--;
                        if (len > 0)
                        {
                            InsertNode(r);
                        }
                    }
                } while (len > 0);
                EncodeEnd();
                retCRC = GetCRC();

                //
                // Create a buffer to hold the results
                //
                if (prependCRC)
                {
                    oBuf = new Byte[codeSize + 2];
                    oBuf[0] = (Byte)((retCRC >> 8) & 0xFF);
                    oBuf[1] = (Byte)(retCRC & 0xFF);
                    j = 2;
                }
                else
                {
                    oBuf = new Byte[codeSize];
                    j = 0;
                }

                for (i = 0; i < codeSize; i++)
                {
                    oBuf[j++] = outBuf[i];
                }

                if (prependCRC)
                {
                    codeSize += 2;
                }

                return codeSize;
            }
        }

        public static Int32 Decode(Byte[] iBuf, ref Byte[] oBuf, Boolean checkCRC)
        {
            UInt16 tmp = 0;
            return Decode(iBuf, ref oBuf, out tmp, checkCRC);
        }

        public static Int32 Decode(Byte[] iBuf, ref Byte[] oBuf, out UInt16 retCRC)
        {
            return Decode(iBuf, ref oBuf, out retCRC, false);
        }

        public static Int32 Decode(Byte[] iBuf, ref Byte[] oBuf, out UInt16 retCRC, Boolean checkCRC)
        {
            //
            // Decoding/Uncompressing
            //
            Int32 i;
            Int32 j;
            Int32 k;
            Int32 r;
            Int32 c;
            Int32 count;
            Int32 iBufStart = 0;
            UInt16 suppliedCRC = 0;

            if (iBuf.Length > MaxUserBuf)
            {
                throw new Exception("Input buffer size ( " + iBuf.Length.ToString() + ") exceeds max buffer size of: " + MaxUserBuf.ToString());
            }

            lock (LZSync)
            {
                //
                // The lock makes the code thread-safe
                //
                EncDec = false;
                Init();

                if (checkCRC)
                {
                    iBufStart = 2;
                    suppliedCRC = (UInt16)iBuf[1];
                    suppliedCRC |= (UInt16)(iBuf[0] << 8);
                }

                for (i = iBufStart; i < iBuf.Length; i++)
                {
                    //
                    // Load the user supplied buffer into the internal processing buffer
                    //
                    inBuf[inEnd++] = iBuf[i];
                }

                //
                // Read size of original text
                //
                textSize = getc();
                textSize |= getc() << 8;
                textSize |= getc() << 16;
                textSize |= getc() << 24;

                if (textSize == 0)
                {
                    oBuf = new Byte[0];
                    retCRC = 0;
                    return textSize;
                }

                StartHuff();

                for (i = 0; i < N - F; i++)
                {
                    textBuf[i] = (Byte)0x20;            //fillchar(text_buf[0],N-F,' ');
                }

                r = N - F;
                count = 0;
                while (count < textSize)
                {
                    c = DecodeChar();
                    if (c < 256)
                    {
                        putc((Byte)c);
                        textBuf[r] = (Byte)c;
                        r = (r + 1) & (N - 1);
                        count++;
                    }
                    else
                    {
                        i = ((r - DecodePosition()) - 1) & (N - 1);
                        j = c - 255 + Threshold;
                        for (k = 0; k <= j - 1; k++)
                        {
                            c = textBuf[(i + k) & (N - 1)];
                            putc((Byte)c);
                            textBuf[r] = (Byte)c;
                            r = (r + 1) & (N - 1);
                            count++;
                        }
                    }
                }

                oBuf = new Byte[count];
                retCRC = GetCRC();

                for (i = 0; i < count; i++)
                {
                    oBuf[i] = outBuf[i];
                }

                if (checkCRC && (retCRC != suppliedCRC))
                {
                    //
                    // Check the CRC.  Return 0 if mismatch
                    //
                    count = 0;
                }
                return count;
            }

        }
        public static UInt16 GetCRC()
        {
            return (UInt16)(Swap((UInt16)(CRC & 0xffff)));
        }

        static void Init()
        {
            //
            // Initialize all structures pointers and counters
            //
            inPtr = 0;
            inEnd = 0;
            outPtr = 0;
            //outEnd = 0;

            getBuf = 0;
            getLen = 0;
            putBuf = 0;
            putLen = 0;

            textSize = 0;
            codeSize = 0;

            matchPosition = 0;
            matchLength = 0;

            InitArrayB(ref inBuf);
            InitArrayB(ref outBuf);

            InitArrayB(ref textBuf);

            InitArrayI(ref lSon);
            InitArrayI(ref dad);
            InitArrayI(ref rSon);
            InitArrayI(ref freq);

            InitArrayI(ref prnt);
            InitArrayI(ref son);

            CRC = 0;
        }

        static void DoCRC(Byte c)
        {
            //
            // Update running tally of CRC
            //
            CRC = (UInt16)((CRC << 8) ^ CRCTable[(CRC >> 8) ^ c]);
        }

        static Byte getc()
        {
            //
            // Get a character from the input buffer
            //
            Byte c = 0;
            if (inPtr < inEnd)
            {
                c = inBuf[inPtr++];
                if (!EncDec)
                {
                    //
                    // Do CRC on input for Decode
                    //
                    DoCRC(c);
                }
            }
            return c;
        }

        static void putc(Byte c)
        {
            //
            // Write a character from the output buffer
            //
            outBuf[outPtr++] = c;
            if (EncDec)
            {
                //
                // Do CRC on output for Encode
                //
                DoCRC(c);
            }
        }

        static void InitTree()
        {
            //
            // Initializing tree
            //
            Int32 i;
            for (i = N + 1; i <= N + 256; i++)
            {
                rSon[i] = NodeNIL; // {root
            }

            for (i = 0; i <= N - 1; i++)
            {
                dad[i] = NodeNIL; //{node}
            }
        }

        static void InsertNode(Int32 r)
        {
            //
            // Insert nodes to the tree
            //

            Int32 i;
            Int32 p;
            Boolean geq;
            Int32 c;

            geq = true;
            p = N + 1 + textBuf[r];
            rSon[r] = NodeNIL;
            lSon[r] = NodeNIL;
            matchLength = 0;
            while (true)
            {
                if (geq)
                {
                    if (rSon[p] == NodeNIL)
                    {
                        rSon[p] = r;
                        dad[r] = p;
                        return;
                    }
                    else
                    {
                        p = rSon[p];
                    }
                }
                else
                {
                    if (lSon[p] == NodeNIL)
                    {
                        lSon[p] = r;
                        dad[r] = p;
                        return;
                    }
                    else
                    {
                        p = lSon[p];
                    }
                }
                i = 1;
                while ((i < F) && (textBuf[r + i] == textBuf[p + i]))
                {
                    i++;
                }
                geq = (textBuf[r + i] >= textBuf[p + i]) || (i == F);

                if (i > Threshold)
                {
                    if (i > matchLength)
                    {
                        matchPosition = ((r - p) & (N - 1)) - 1;
                        matchLength = i;
                        if (matchLength >= F)
                        {
                            break;
                        }
                    }
                    if (i == matchLength)
                    {
                        c = ((r - p) & (N - 1)) - 1;
                        if (c < matchPosition)
                        {
                            matchPosition = c;
                        }
                    }
                }
            }

            dad[r] = dad[p];
            lSon[r] = lSon[p];
            rSon[r] = rSon[p];
            dad[lSon[p]] = r;
            dad[rSon[p]] = r;
            if (rSon[dad[p]] == p)
            {
                rSon[dad[p]] = r;
            }
            else
            {
                lSon[dad[p]] = r;
            }
            dad[p] = NodeNIL;      // remove p
        }

        static void DeleteNode(Int32 p)
        {
            //
            // Delete node from the tree
            //
            Int32 q;

            if (dad[p] == NodeNIL)
            {
                return;     // unregistered
            }

            if (rSon[p] == NodeNIL)
            {
                q = lSon[p];
            }
            else
            {
                if (lSon[p] == NodeNIL)
                {
                    q = rSon[p];
                }
                else
                {
                    q = lSon[p];

                    if (rSon[q] != NodeNIL)
                    {
                        do
                        {
                            q = rSon[q];
                        } while (rSon[q] != NodeNIL);
                        rSon[dad[q]] = lSon[q];
                        dad[lSon[q]] = dad[q];
                        lSon[q] = lSon[p];
                        dad[lSon[p]] = q;
                    }
                    rSon[q] = rSon[p];
                    dad[rSon[p]] = q;
                }
            }
            dad[q] = dad[p];
            if (rSon[dad[p]] == p)
            {
                rSon[dad[p]] = q;
            }
            else
            {
                lSon[dad[p]] = q;
            }
            dad[p] = NodeNIL;
        }

        static Byte GetBit()
        {
            //
            // Get one bit
            //
            Byte RetVal;
            while (getLen <= 8)
            {
                getBuf = (UInt16)(getBuf | (getc() << (8 - getLen)));
                getLen += 8;
            }
            RetVal = (Byte)((getBuf >> 15) & (0xff));
            getBuf = (UInt16)(getBuf << 1);
            getLen--;
            return RetVal;
        }

        static Byte GetByte()
        {
            //
            // Get one byte
            //
            Byte RetVal;
            while (getLen <= 8)
            {
                getBuf = (UInt16)(getBuf | (getc() << (8 - getLen)));
                getLen += 8;
            }
            RetVal = Hi(getBuf);
            getBuf = (UInt16)(getBuf << 8);
            getLen -= 8;
            return RetVal;

        }

        static void Putcode(Byte n, Int32 c)
        {
            //
            // Output 'n' bits
            //
            putBuf = (UInt16)(putBuf | (c >> putLen));
            putLen += n;
            if (putLen >= 8)
            {
                putc(Hi(putBuf));
                putLen -= 8;
                if (putLen >= 8)
                {
                    putc(Lo(putBuf));
                    codeSize += 2;
                    putLen -= 8;
                    putBuf = (UInt16)(c << (n - putLen));
                }
                else
                {
                    putBuf = (UInt16)Swap((UInt16)(putBuf & 0xff));
                    codeSize++;
                }
            }
        }

        static void StartHuff()
        {
            //
            // Initialize freq tree
            //
            Int32 i;
            Int32 j;

            for (i = 0; i <= NChar - 1; i++)
            {
                freq[i] = 1;
                son[i] = i + T;
                prnt[i + T] = i;
            }
            i = 0;
            j = NChar;
            while (j <= R)
            {
                freq[j] = freq[i] + freq[i + 1];
                son[j] = i;
                prnt[i] = j;
                prnt[i + 1] = j;
                i += 2;
                j++;
            }
            freq[T] = 0xFFFF;
            prnt[R] = 0;
        }

        static void reconst()
        {
            //
            // Reconstruct freq tree
            //
            Int32 i;
            Int32 j;
            Int32 k;
            Int32 f;
            Int32 n;

            //
            // Halven cumulative freq for leaf nodes
            //
            j = 0;
            for (i = 0; i <= T - 1; i++)
            {
                if (son[i] >= T)
                {
                    freq[j] = (freq[i] + 1) >> 1;
                    son[j] = son[i];
                    j++;
                }
            }

            //
            // Make a tree : first, connect children nodes
            //
            i = 0;
            j = NChar;
            while (j < T)
            {
                k = i + 1;
                f = freq[i] + freq[k];
                freq[j] = f;
                k = j - 1;
                while (f < freq[k])
                {
                    k--;
                }
                k++;

                //
                // Original code segment
                // l:= (j-k)*2;
                // move(freq[k],freq[k+1],l);
                // freq[k]:= f;
                // move(son[k],son[k+1],l);
                // son[k]:= i;
                //
                // New code segment.
                //
                for (n = j; n > k; n--)
                {
                    freq[n] = freq[n - 1];
                    son[n] = son[n - 1];
                }
                freq[k] = f;
                son[k] = i;

                i += 2;
                j++;
            }

            //
            // Connect parent nodes
            //
            for (i = 0; i <= T - 1; i++)
            {
                k = son[i];
                prnt[k] = i;
                if (k < T)
                {
                    prnt[k + 1] = i;
                }
            }
        }

        static void update(Int32 c)
        {
            //
            // Update freq tree
            //
            Int32 i;
            Int32 j;
            Int32 k;
            Int32 n;

            if (freq[R] == MaxFreq)
            {
                reconst();
            }
            c = prnt[c + T];
            do
            {
                freq[c]++;
                k = freq[c];

                //
                // Swap nodes to keep the tree freq-ordered}
                //
                n = c + 1;
                if (k > freq[n])
                {
                    while (k > freq[n + 1])
                    {
                        n++;
                    }
                    freq[c] = freq[n];
                    freq[n] = k;

                    i = son[c];
                    prnt[i] = n;
                    if (i < T)
                    {
                        prnt[i + 1] = n;
                    }
                    j = son[n];
                    son[n] = i;

                    prnt[j] = c;
                    if (j < T)
                    {
                        prnt[j + 1] = c;
                    }
                    son[c] = j;

                    c = n;
                }
                c = prnt[c];
            }
            while (c != 0);       // do it until reaching the root
        }

        static void EncodeChar(Int32 c)
        {
            Int32 code;
            Byte len;
            Int32 k;

            code = 0;
            len = 0;
            k = prnt[c + T];

            //
            // Search connections from leaf node to the root
            //
            do
            {
                code = code >> 1;

                //
                // If node's address is odd, output 1 else output 0
                //
                if ((k & 1) > 0)
                {
                    code += 0x8000;
                }
                len++;
                k = prnt[k];
            } while (k != R);
            Putcode(len, code);
            update(c);
        }

        static void EncodePosition(Int32 c)
        {

            Int32 i;

            //
            // Output upper 6 bits with encoding
            //
            i = c >> 6;
            Putcode(p_len[i], ((Int32)p_code[i]) << 8);

            //
            // Output lower 6 bits directly
            //
            Putcode(6, (c & 0x3F) << 10);
        }

        static void EncodeEnd()
        {
            if (putLen > 0)
            {
                putc(Hi(putBuf));
                codeSize++;
            }
        }

        static UInt16 DecodeChar()
        {
            Int32 c;
            UInt16 RetVal;
            c = son[R];

            //
            // Start searching tree from the root to leaves.
            // Choose node #(son[]) if input bit = 0
            // else choose #(son[]+1) (input bit = 1)
            //
            while (c < T)
            {
                c = son[c + GetBit()];
            }
            c -= T;
            update(c);
            RetVal = (UInt16)c;
            return RetVal;
        }

        static Int32 DecodePosition()
        {
            UInt16 i;
            UInt16 j;
            UInt16 c;

            Int32 RetVal;

            //
            // Decode upper 6 bits from given table
            //
            i = GetByte();
            c = (UInt16)(d_code[i] << 6);
            j = d_len[i];

            //
            // Input lower 6 bits directly
            //
            j -= 2;
            while (j > 0)
            {
                j--;
                i = (UInt16)((i << 1) | GetBit());
            }
            RetVal = c | (i & 0x3F);
            return RetVal;
        }


        //
        // Byte manipulation helper routines
        //
        static Byte Hi(UInt16 X)
        {
            return (Byte)((X >> 8) & 0xff);
        }

        static Byte Lo(UInt16 X)
        {
            return (Byte)(X & 0xff);
        }

        static UInt16 Swap(UInt16 X)
        {
            return ((UInt16)(((X >> 8) & 0xff) | ((X & 0xff) << 8)));
        }

        static void InitArrayB(ref Byte[] b)
        {
            for (Int32 i = 0; i < b.Length; i++)
            {
                b[i] = 0;
            }
        }

        static void InitArrayI(ref Int32[] b)
        {
            for (Int32 i = 0; i < b.Length; i++)
            {
                b[i] = 0;
            }
        }
    }
}
