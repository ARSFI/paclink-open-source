using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{

    // 
    // This class implements the Compression, CRC, and challenge password functions previously provided 
    // in a no managed .DLL
    // 
    // The Class names are:
    // Compression
    // Crc
    // WinlinkAuth


    public class Compression
    {
        // 
        // lzh .net Class library v1.0
        // Ported to C# by Peter Woods 08/22/2008 from the original
        // Pascal version.
        // Ported to VB by Peter Woods 10/20/2010 from the C# code
        // 
        // --------------Original Header Below----------------- 
        // 
        // LZHUF.C English version 1.0
        // Based on Japanese version 29-NOV-1988
        // LZSS coded by Haruhiko OKUMURA
        // Adaptive Huffman Coding coded by Haruyasu YOSHIZAKI
        // Edited and translated to English by Kenji RIKITAKE
        // Converted to Turbo Pascal 5.0
        // by Peter Sawatzki with assistance of Wayne Sullivan
        // 

        // 
        // Constants
        // 
        // 
        // Synchonization object to support multi threading
        // 
        private static object LZSync = new object();

        // 
        // Define the string buffer size.  Note, was 4096 in the original pascal version.
        // 
        private const int N = 2048;

        // 
        // Define the size of the look-ahead buffer
        // 
        private const int F = 60;

        // 
        // Define the Threshold
        // 
        private const int Threshold = 2;

        // 
        // Set the NULL value to the size of the string buffer
        // 
        private const int NodeNIL = N;

        // 
        // Huffman coding parameters
        // 
        private const int NChar = 256 - Threshold + F;

        // 
        // Define the table size
        // character code = 0..N_CHAR-1
        // 
        private const int T = NChar * 2 - 1;

        // 
        // Define the Root position
        // 
        private const int R = T - 1;

        // 
        // MAx frequency is 32678
        // 
        private const int MaxFreq = 0x8000;

        // 
        // CRC polynomial
        // 
        private const int CRCPoly = 0x18408;

        // 
        // Allocate fixed size buffers
        // 
        // 
        // textBuf [0..N+F-2] Byte->Byte
        // 
        private const int TBSize = N + F - 2;
        private static int textBufMask = 0xFF;
        private static byte[] textBuf = new byte[2108];

        // 
        // lSon [0..N] Word->UInt16
        // 
        private static int[] lSon = new int[2050];

        // 
        // dad [0..N] Word->UInt16
        // 
        private static int[] dad = new int[2050];

        // 
        // rSon [0..N+256] Word->UInt16
        // 
        private static int[] rSon = new int[2306];

        // 
        // Cumulative freq table [0..T] Word->UInt16
        // 
        private static int[] freq = new int[629];

        // 
        // Pointing children nodes (son[], son[] + 1) [0..T-1] Word->UInt16
        // 
        private static int[] son = new int[628];

        // 
        // Pointing parent nodes use [0..T-1]. Area [T..(T + N_CHAR - 1)] are pointers for leaves
        // Word->UInt16
        // 
        private static int[] parent = new int[942];

        // 
        // Tables for encoding/decoding upper 6 bits of sliding dictionary pointer
        // encoder table}
        // 
        // Position Encode length
        // 
        private static byte[] p_len = new byte[] { 0x3, 0x4, 0x4, 0x4, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8 };










        // 
        // Position Encode Table
        // 
        private static int[] p_code = new int[] { 0x0, 0x20, 0x30, 0x40, 0x50, 0x58, 0x60, 0x68, 0x70, 0x78, 0x80, 0x88, 0x90, 0x94, 0x98, 0x9C, 0xA0, 0xA4, 0xA8, 0xAC, 0xB0, 0xB4, 0xB8, 0xBC, 0xC0, 0xC2, 0xC4, 0xC6, 0xC8, 0xCA, 0xCC, 0xCE, 0xD0, 0xD2, 0xD4, 0xD6, 0xD8, 0xDA, 0xDC, 0xDE, 0xE0, 0xE2, 0xE4, 0xE6, 0xE8, 0xEA, 0xEC, 0xEE, 0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF };










        // 
        // Position decode table
        // 
        private static int[] d_code = new int[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x1, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x2, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x9, 0x9, 0x9, 0x9, 0x9, 0x9, 0x9, 0x9, 0xA, 0xA, 0xA, 0xA, 0xA, 0xA, 0xA, 0xA, 0xB, 0xB, 0xB, 0xB, 0xB, 0xB, 0xB, 0xB, 0xC, 0xC, 0xC, 0xC, 0xD, 0xD, 0xD, 0xD, 0xE, 0xE, 0xE, 0xE, 0xF, 0xF, 0xF, 0xF, 0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11, 0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x14, 0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15, 0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17, 0x18, 0x18, 0x19, 0x19, 0x1A, 0x1A, 0x1B, 0x1B, 0x1C, 0x1C, 0x1D, 0x1D, 0x1E, 0x1E, 0x1F, 0x1F, 0x20, 0x20, 0x21, 0x21, 0x22, 0x22, 0x23, 0x23, 0x24, 0x24, 0x25, 0x25, 0x26, 0x26, 0x27, 0x27, 0x28, 0x28, 0x29, 0x29, 0x2A, 0x2A, 0x2B, 0x2B, 0x2C, 0x2C, 0x2D, 0x2D, 0x2E, 0x2E, 0x2F, 0x2F, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F };










































        // 
        // Position decode length
        // 
        private static int[] d_len = new[] { 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x3, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x4, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x5, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x6, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x7, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8, 0x8 };










































        // 
        // CRC Table 
        // Word->UInt16
        // 
        private static int CRCMask = 0xFFFF;
        private static int[] CRCTable = new[] { 0x0, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF, 0x1231, 0x210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE, 0x2462, 0x3443, 0x420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485, 0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D, 0x3653, 0x2672, 0x1611, 0x630, 0x76D7, 0x66F6, 0x5695, 0x46B4, 0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC, 0x48C4, 0x58E5, 0x6886, 0x78A7, 0x840, 0x1861, 0x2802, 0x3823, 0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B, 0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0xA50, 0x3A33, 0x2A12, 0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A, 0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0xC60, 0x1C41, 0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49, 0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0xE70, 0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78, 0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F, 0x1080, 0xA1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067, 0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E, 0x2B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256, 0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D, 0x34E2, 0x24C3, 0x14A0, 0x481, 0x7466, 0x6447, 0x5424, 0x4405, 0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C, 0x26D3, 0x36F2, 0x691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634, 0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB, 0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x8E1, 0x3882, 0x28A3, 0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A, 0x4A75, 0x5A54, 0x6A37, 0x7A16, 0xAF1, 0x1AD0, 0x2AB3, 0x3A92, 0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9, 0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0xCC1, 0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8, 0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0xED1, 0x1EF0 };










































        // 
        // Processing buffers
        // 
        private static byte[] inBuf = null;
        private static byte[] outBuf = null;
        private static int inPtr = 0;
        private static int inEnd = 0;
        private static int outPtr = 0;
        private static int CRC;
        private static bool EncDec = false; // true for Encode, false for Decode
        private static int getBuf = 0;
        private static int getLen = 0;
        private static int putBuf = 0;
        private static int putLen = 0;
        private static int textSize = 0;
        private static int codeSize = 0;
        private static int matchPosition = 0;
        private static int matchLength = 0;

        // 
        // Main entry points
        // 
        public static int Encode(byte[] iBuf, ref byte[] oBuf, bool prependCRC)
        {
            int EncodeRet = default;
            var tmp = default(ushort);
            EncodeRet = Encode(iBuf, ref oBuf, tmp, prependCRC);
            return EncodeRet;
        }

        public static int Encode(byte[] iBuf, ref byte[] oBuf, ushort retCRC)
        {
            int EncodeRet = default;
            EncodeRet = Encode(iBuf, ref oBuf, retCRC, false);
            return EncodeRet;
        }

        public static int Encode(byte[] iBuf, ref byte[] oBuf, ushort retCRC, bool prependCRC)
        {
            int EncodeRet = default;
            // 
            // Encoding/Compressing
            // 
            int i;
            int c;
            int len;
            int r;
            int s;
            int last_match_length;
            int j = 0;
            lock (LZSync)
            {
                // 
                // The lock makes the code thread-safe
                // 
                inBuf = new byte[iBuf.Length + 100 + 1];
                outBuf = new byte[iBuf.Length * 2 + 100000 + 1];
                Init();
                EncDec = true;
                var loopTo = iBuf.Length - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    // 
                    // Load the user supplied buffer into the internal processing buffer
                    // 
                    inBuf[inEnd] = iBuf[i];
                    inEnd += 1;
                }

                putc(Conversions.ToByte(inEnd & 0xFF));
                putc(Conversions.ToByte(inEnd >> 8 & 0xFF));
                putc(Conversions.ToByte(inEnd >> 16 & 0xFF));
                putc(Conversions.ToByte(inEnd >> 24 & 0xFF));
                codeSize += 4;
                if (inEnd == 0)
                {
                    oBuf = new byte[0];
                    retCRC = 0;
                    return codeSize;
                }

                textSize = 0;
                StartHuff();
                InitTree();
                s = 0;
                r = N - F;
                var loopTo1 = r - 1;
                for (i = 0; i <= loopTo1; i++)
                    // fillchar(text_buf[0],r,' ');
                    textBuf[i] = Conversions.ToByte(0x20);
                len = 0;
                while (len < F && inPtr < inEnd)
                {
                    textBuf[r + len] = Conversions.ToByte(getc() & 0xFF);
                    len += 1;
                }

                textSize = len;
                for (i = 1; i <= F; i++)
                    InsertNode(r - i);
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
                        EncodeChar(255 - Threshold + matchLength);
                        EncodePosition(matchPosition);
                    }

                    last_match_length = matchLength;
                    i = 0;
                    while (i < last_match_length && inPtr < inEnd)
                    {
                        i += 1;
                        DeleteNode(s);
                        c = getc();
                        textBuf[s] = Conversions.ToByte(c & 0xFF);
                        if (s < F - 1)
                        {
                            textBuf[s + N] = Conversions.ToByte(c);
                        }

                        s = s + 1 & N - 1;
                        r = r + 1 & N - 1;
                        InsertNode(r);
                    }

                    textSize += i;
                    while (i < last_match_length)
                    {
                        i += 1;
                        DeleteNode(s);
                        s = s + 1 & N - 1;
                        r = r + 1 & N - 1;
                        len -= 1;
                        if (len > 0)
                        {
                            InsertNode(r);
                        }
                    }
                }
                while (len > 0);
                EncodeEnd();
                retCRC = GetCRC();

                // 
                // Create a buffer to hold the results
                // 
                if (prependCRC)
                {
                    oBuf = new byte[codeSize + 1 + 1];
                    oBuf[0] = Conversions.ToByte(retCRC >> 8 & 0xFF);
                    oBuf[1] = Conversions.ToByte(retCRC & 0xFF);
                    j = 2;
                }
                else
                {
                    oBuf = new byte[codeSize];
                    j = 0;
                }

                var loopTo2 = codeSize - 1;
                for (i = 0; i <= loopTo2; i++)
                {
                    oBuf[j] = outBuf[i];
                    j += 1;
                }

                if (prependCRC)
                {
                    codeSize += 2;
                }

                EncodeRet = codeSize;
            }

            return EncodeRet;
        }

        public static int Decode(byte[] iBuf, ref byte[] oBuf, bool checkCRC, int intUncompressedSize)
        {
            ushort tmp = 0;
            return DecodeWork(iBuf, ref oBuf, tmp, checkCRC, intUncompressedSize);
        }

        public static int Decode(byte[] iBuf, ref byte[] oBuf, ushort retCRC, int intUncompressedSize)
        {
            return DecodeWork(iBuf, ref oBuf, retCRC, false, intUncompressedSize);
        }

        public static int DecodeWork(byte[] iBuf, ref byte[] oBuf, ushort retCRC, bool checkCRC, int intUncompressedSize)
        {
            int DecodeWorkRet = default;
            // 
            // Decoding/Uncompressing
            // 
            int i;
            int j;
            int k;
            int r;
            int c;
            int count;
            int iBufStart = 0;
            int suppliedCRC = 0;
            lock (LZSync)
            {
                // 
                // The lock makes the code thread-safe
                // 
                inBuf = new byte[iBuf.Length + 100 + 1];
                outBuf = new byte[intUncompressedSize + 20000 + 1];
                EncDec = false;
                Init();
                if (checkCRC)
                {
                    iBufStart = 2;
                    suppliedCRC = Conversions.ToInteger(iBuf[1]) & 0xFF;
                    suppliedCRC = suppliedCRC | Conversions.ToInteger(iBuf[0]) << 8;
                }

                var loopTo = iBuf.Length - 1;
                for (i = iBufStart; i <= loopTo; i++)
                {
                    // 
                    // Load the user supplied buffer into the internal processing buffer
                    // 
                    inBuf[inEnd] = iBuf[i];
                    inEnd += 1;
                }

                // 
                // Read size of original text
                // 
                textSize = getc();
                textSize = textSize | getc() << 8;
                textSize = textSize | getc() << 16;
                textSize = textSize | getc() << 24;
                if (textSize == 0)
                {
                    oBuf = new byte[0];
                    retCRC = 0;
                    DecodeWorkRet = textSize;
                    return textSize;
                }

                StartHuff();
                for (i = 0; i <= N - F - 1; i++)
                    // fillchar(text_buf[0],N-F,' ');
                    textBuf[i] = Conversions.ToByte(0x20);
                r = N - F;
                count = 0;
                while (count < textSize)
                {
                    c = DecodeChar();
                    if (c < 256)
                    {
                        putc(Conversions.ToByte(c & 0xFF));
                        textBuf[r] = Conversions.ToByte(c & 0xFF);
                        r = r + 1 & N - 1;
                        count += 1;
                    }
                    else
                    {
                        i = r - DecodePosition() - 1 & N - 1;
                        j = c - 255 + Threshold;
                        var loopTo1 = j - 1;
                        for (k = 0; k <= loopTo1; k++)
                        {
                            c = Conversions.ToInteger(textBuf[i + k & N - 1]);
                            putc(Conversions.ToByte(c & 0xFF));
                            textBuf[r] = Conversions.ToByte(c & 0xFF);
                            r = r + 1 & N - 1;
                            count += 1;
                        }
                    }
                }

                oBuf = new byte[count];
                retCRC = GetCRC();
                var loopTo2 = count - 1;
                for (i = 0; i <= loopTo2; i++)
                    oBuf[i] = outBuf[i];
                if (checkCRC && retCRC != suppliedCRC)
                {
                    // 
                    // Check the CRC.  Return 0 if mismatch
                    // 
                    count = 0;
                }

                return count;
            }
        }

        public static ushort GetCRC()
        {
            return Conversions.ToUShort(Swap(CRC & 0xFFFF));
        }

        private static void Init()
        {
            // 
            // Initialize all structures pointers and counters
            // 
            inPtr = 0;
            inEnd = 0;
            outPtr = 0;
            getBuf = 0;
            getLen = 0;
            putBuf = 0;
            putLen = 0;
            textSize = 0;
            codeSize = 0;
            matchPosition = 0;
            matchLength = 0;
            InitArrayB(ref textBuf);
            InitArrayI(ref lSon);
            InitArrayI(ref dad);
            InitArrayI(ref rSon);
            InitArrayI(ref freq);
            InitArrayI(ref parent);
            InitArrayI(ref son);
            CRC = 0;
        }

        private static void DoCRC(int c)
        {
            // 
            // Update running tally of CRC
            // 
            CRC = (CRC << 8 ^ CRCTable[CRC >> 8 ^ c]) & CRCMask;
        }

        private static int getc()
        {
            // 
            // Get a character from the input buffer
            // 
            int c = 0;
            if (inPtr < inEnd)
            {
                c = Conversions.ToInteger(inBuf[inPtr]) & 0xFF;
                inPtr += 1;
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

        private static void putc(int c)
        {
            // 
            // Write a character from the output buffer
            // 
            if (outPtr == outBuf.Length - 1)
            {
                // 
                // Make more room in the output buffer
                // 
                var tmpBuf = new byte[outBuf.Length + 100000 + 1];
                InitArrayB(ref tmpBuf);
                outBuf.CopyTo(tmpBuf, 0);
                outBuf = tmpBuf;
            }

            outBuf[outPtr] = Conversions.ToByte(c & 0xFF);
            outPtr += 1;
            if (EncDec)
            {
                // 
                // Do CRC on output for Encode
                // 
                DoCRC(c);
            }
        }

        private static void InitTree()
        {
            // 
            // Initializing tree
            // 
            int i;
            for (i = N + 1; i <= N + 256; i++)
                // {root
                rSon[i] = NodeNIL;
            for (i = 0; i <= N - 1; i++)
                // {node}
                dad[i] = NodeNIL;
        }

        private static void InsertNode(int r)
        {
            // 
            // Insert nodes to the tree
            // 

            int i;
            int p;
            bool geq;
            int c;
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
                else if (lSon[p] == NodeNIL)
                {
                    lSon[p] = r;
                    dad[r] = p;
                    return;
                }
                else
                {
                    p = lSon[p];
                }

                i = 1;
                while (i < F && textBuf[r + i] == textBuf[p + i])
                    i += 1;
                geq = textBuf[r + i] >= textBuf[p + i] || i == F;
                if (i > Threshold)
                {
                    if (i > matchLength)
                    {
                        matchPosition = (r - p & N - 1) - 1;
                        matchLength = i;
                        if (matchLength >= F)
                        {
                            break;
                        }
                    }

                    if (i == matchLength)
                    {
                        c = (r - p & N - 1) - 1;
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

            dad[p] = NodeNIL;
            // remove p
        }

        private static void DeleteNode(int p)
        {
            // 
            // Delete node from the tree
            // 
            int q;
            if (dad[p] == NodeNIL)
            {
                // unregistered
                return;
            }

            if (rSon[p] == NodeNIL)
            {
                q = lSon[p];
            }
            else if (lSon[p] == NodeNIL)
            {
                q = rSon[p];
            }
            else
            {
                q = lSon[p];
                if (rSon[q] != NodeNIL)
                {
                    do
                        q = rSon[q];
                    while (rSon[q] != NodeNIL);
                    rSon[dad[q]] = lSon[q];
                    dad[lSon[q]] = dad[q];
                    lSon[q] = lSon[p];
                    dad[lSon[p]] = q;
                }

                rSon[q] = rSon[p];
                dad[rSon[p]] = q;
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

        private static int GetBit()
        {
            // 
            // Get one bit
            // 
            int retVal;
            while (getLen <= 8)
            {
                getBuf = (getBuf | getc() << 8 - getLen) & 0xFFFF;
                getLen += 8;
            }

            retVal = getBuf >> 15 & 0x1;
            getBuf = getBuf << 1 & 0xFFFF;
            getLen -= 1;
            return retVal;
        }

        private static int GetByte()
        {
            // 
            // Get one byte
            // 
            int retVal;
            while (getLen <= 8)
            {
                getBuf = (getBuf | getc() << 8 - getLen) & 0xFFFF;
                getLen += 8;
            }

            retVal = Hi(getBuf) & 0xFF;
            getBuf = getBuf << 8 & 0xFFFF;
            getLen -= 8;
            return retVal;
        }

        private static void Putcode(int n, int c)
        {
            // 
            // Output 'n' bits
            // 
            putBuf = (putBuf | c >> putLen) & 0xFFFF;
            putLen += n;
            if (putLen >= 8)
            {
                putc(Hi(putBuf) & 0xFF);
                putLen -= 8;
                if (putLen >= 8)
                {
                    putc(Lo(putBuf) & 0xFF);
                    codeSize += 2;
                    putLen -= 8;
                    putBuf = c << n - putLen & 0xFFFF;
                }
                else
                {
                    putBuf = Swap(putBuf & 0xFF);
                    codeSize += 1;
                }
            }
        }

        private static void StartHuff()
        {
            // 
            // Initialize freq tree
            // 
            int i;
            int j;
            for (i = 0; i <= NChar - 1; i++)
            {
                freq[i] = 1;
                son[i] = i + T;
                parent[i + T] = i;
            }

            i = 0;
            j = NChar;
            while (j <= R)
            {
                freq[j] = freq[i] + freq[i + 1] & 0xFFFF;
                son[j] = i;
                parent[i] = j;
                parent[i + 1] = j;
                i += 2;
                j += 1;
            }

            freq[T] = 0xFFFF;
            parent[R] = 0;
        }

        private static void reconst()
        {
            // 
            // Reconstruct freq tree
            // 
            int i;
            int j;
            int k;
            int f;
            int n;

            // 
            // Halven cumulative freq for leaf nodes
            // 
            j = 0;
            for (i = 0; i <= T - 1; i++)
            {
                if (son[i] >= T)
                {
                    freq[j] = freq[i] + 1 >> 1;
                    son[j] = son[i];
                    j += 1;
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
                f = freq[i] + freq[k] & 0xFFFF;
                freq[j] = f;
                k = j - 1;
                while (f < freq[k])
                    k -= 1;
                k += 1;

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
                var loopTo = k + 1;
                for (n = j; n >= loopTo; n -= 1)
                {
                    freq[n] = freq[n - 1];
                    son[n] = son[n - 1];
                }

                freq[k] = f;
                son[k] = i;
                i += 2;
                j += 1;
            }

            // 
            // Connect parent nodes
            // 
            for (i = 0; i <= T - 1; i++)
            {
                k = son[i];
                parent[k] = i;
                if (k < T)
                {
                    parent[k + 1] = i;
                }
            }
        }

        private static void update(int c)
        {
            // 
            // Update freq tree
            // 
            int i;
            int j;
            int k;
            int n;
            if (freq[R] == MaxFreq)
            {
                reconst();
            }

            c = parent[c + T];
            do
            {
                freq[c] += 1;
                k = freq[c];

                // 
                // Swap nodes to keep the tree freq-ordered}
                // 
                n = c + 1;
                if (k > freq[n])
                {
                    while (k > freq[n + 1])
                        n += 1;
                    freq[c] = freq[n];
                    freq[n] = k;
                    i = son[c];
                    parent[i] = n;
                    if (i < T)
                    {
                        parent[i + 1] = n;
                    }

                    j = son[n];
                    son[n] = i;
                    parent[j] = c;
                    if (j < T)
                    {
                        parent[j + 1] = c;
                    }

                    son[c] = j;
                    c = n;
                }

                c = parent[c];
            }
            while (c != 0);
            // do it until reaching the root
        }

        private static void EncodeChar(int c)
        {
            int code;
            byte len;
            int k;
            code = 0;
            len = 0;
            k = parent[c + T];

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

                len += Conversions.ToByte(1);
                k = parent[k];
            }
            while (k != R);
            Putcode(len, code);
            update(c);
        }

        private static void EncodePosition(int c)
        {
            int i;

            // 
            // Output upper 6 bits with encoding
            // 
            i = c >> 6;
            Putcode(p_len[i], p_code[i] << 8);

            // 
            // Output lower 6 bits directly
            // 
            Putcode(6, (c & 0x3F) << 10);
        }

        private static void EncodeEnd()
        {
            if (putLen > 0)
            {
                putc(Hi(putBuf));
                codeSize += 1;
            }
        }

        private static int DecodeChar()
        {
            int c;
            int RetVal;
            c = son[R];

            // 
            // Start searching tree from the root to leaves.
            // Choose node #(son[]) if input bit = 0
            // else choose #(son[]+1) (input bit = 1)
            // 
            while (c < T)
                c = son[c + GetBit()];
            c -= T;
            update(c);
            RetVal = c & 0xFFFF;
            return RetVal;
        }

        private static int DecodePosition()
        {
            int i;
            int j;
            int c;
            int RetVal;

            // 
            // Decode upper 6 bits from given table
            // 
            i = GetByte();
            c = d_code[i] << 6 & 0xFFFF;
            j = d_len[i];

            // 
            // Input lower 6 bits directly
            // 
            j -= 2;
            while (j > 0)
            {
                j -= 1;
                i = (i << 1 | GetBit()) & 0xFFFF;
            }

            RetVal = c | i & 0x3F;
            return RetVal;
        }


        // 
        // Byte manipulation helper routines
        // 
        private static int Hi(int X)
        {
            return X >> 8 & 0xFF;
        }

        private static int Lo(int X)
        {
            return X & 0xFF;
        }

        private static int Swap(int X)
        {
            return X >> 8 & 0xFF | (X & 0xFF) << 8;
        }

        private static void InitArrayB(ref byte[] b)
        {
            for (int i = 0, loopTo = b.Length - 1; i <= loopTo; i++)
                b[i] = 0;
        }

        private static void InitArrayI(ref int[] b)
        {
            for (int i = 0, loopTo = b.Length - 1; i <= loopTo; i++)
                b[i] = 0;
        }
    }

    public class Crc
    {
        // 
        // This class holds the routines to create and check F2BB CRC values
        // that wrap the WinLink messages.  This CRC is different than the CRC used in
        // the Compression class above.

        private static int CRCSTART = 0xFFFF;
        private static int CRCFINISH = 0xF0B8;
        private static int[] crcTable = new[] { 0x0, 0x1189, 0x2312, 0x329B, 0x4624, 0x57AD, 0x6536, 0x74BF, 0x8C48, 0x9DC1, 0xAF5A, 0xBED3, 0xCA6C, 0xDBE5, 0xE97E, 0xF8F7, 0x1081, 0x108, 0x3393, 0x221A, 0x56A5, 0x472C, 0x75B7, 0x643E, 0x9CC9, 0x8D40, 0xBFDB, 0xAE52, 0xDAED, 0xCB64, 0xF9FF, 0xE876, 0x2102, 0x308B, 0x210, 0x1399, 0x6726, 0x76AF, 0x4434, 0x55BD, 0xAD4A, 0xBCC3, 0x8E58, 0x9FD1, 0xEB6E, 0xFAE7, 0xC87C, 0xD9F5, 0x3183, 0x200A, 0x1291, 0x318, 0x77A7, 0x662E, 0x54B5, 0x453C, 0xBDCB, 0xAC42, 0x9ED9, 0x8F50, 0xFBEF, 0xEA66, 0xD8FD, 0xC974, 0x4204, 0x538D, 0x6116, 0x709F, 0x420, 0x15A9, 0x2732, 0x36BB, 0xCE4C, 0xDFC5, 0xED5E, 0xFCD7, 0x8868, 0x99E1, 0xAB7A, 0xBAF3, 0x5285, 0x430C, 0x7197, 0x601E, 0x14A1, 0x528, 0x37B3, 0x263A, 0xDECD, 0xCF44, 0xFDDF, 0xEC56, 0x98E9, 0x8960, 0xBBFB, 0xAA72, 0x6306, 0x728F, 0x4014, 0x519D, 0x2522, 0x34AB, 0x630, 0x17B9, 0xEF4E, 0xFEC7, 0xCC5C, 0xDDD5, 0xA96A, 0xB8E3, 0x8A78, 0x9BF1, 0x7387, 0x620E, 0x5095, 0x411C, 0x35A3, 0x242A, 0x16B1, 0x738, 0xFFCF, 0xEE46, 0xDCDD, 0xCD54, 0xB9EB, 0xA862, 0x9AF9, 0x8B70, 0x8408, 0x9581, 0xA71A, 0xB693, 0xC22C, 0xD3A5, 0xE13E, 0xF0B7, 0x840, 0x19C9, 0x2B52, 0x3ADB, 0x4E64, 0x5FED, 0x6D76, 0x7CFF, 0x9489, 0x8500, 0xB79B, 0xA612, 0xD2AD, 0xC324, 0xF1BF, 0xE036, 0x18C1, 0x948, 0x3BD3, 0x2A5A, 0x5EE5, 0x4F6C, 0x7DF7, 0x6C7E, 0xA50A, 0xB483, 0x8618, 0x9791, 0xE32E, 0xF2A7, 0xC03C, 0xD1B5, 0x2942, 0x38CB, 0xA50, 0x1BD9, 0x6F66, 0x7EEF, 0x4C74, 0x5DFD, 0xB58B, 0xA402, 0x9699, 0x8710, 0xF3AF, 0xE226, 0xD0BD, 0xC134, 0x39C3, 0x284A, 0x1AD1, 0xB58, 0x7FE7, 0x6E6E, 0x5CF5, 0x4D7C, 0xC60C, 0xD785, 0xE51E, 0xF497, 0x8028, 0x91A1, 0xA33A, 0xB2B3, 0x4A44, 0x5BCD, 0x6956, 0x78DF, 0xC60, 0x1DE9, 0x2F72, 0x3EFB, 0xD68D, 0xC704, 0xF59F, 0xE416, 0x90A9, 0x8120, 0xB3BB, 0xA232, 0x5AC5, 0x4B4C, 0x79D7, 0x685E, 0x1CE1, 0xD68, 0x3FF3, 0x2E7A, 0xE70E, 0xF687, 0xC41C, 0xD595, 0xA12A, 0xB0A3, 0x8238, 0x93B1, 0x6B46, 0x7ACF, 0x4854, 0x59DD, 0x2D62, 0x3CEB, 0xE70, 0x1FF9, 0xF78F, 0xE606, 0xD49D, 0xC514, 0xB1AB, 0xA022, 0x92B9, 0x8330, 0x7BC7, 0x6A4E, 0x58D5, 0x495C, 0x3DE3, 0x2C6A, 0x1EF1, 0xF78 };










































        public static bool CheckCRC(byte[] buf)
        {
            // 
            // Validate CRC.  Return true if CRC is valid, false otherwise
            // 
            int crc = FetchCrc(buf);
            return crc == CRCFINISH;
        }

        public static byte[] CreateCrc(byte[] buf)
        {
            // 
            // Return 2 byte CRC
            // 
            int crc = FetchCrc(buf) ^ CRCSTART;
            var ret = new[] { Conversions.ToByte(crc & 0xFF), Conversions.ToByte(crc >> 8 & 0xFF) };
            return ret;
        }

        public static int UpdCrc(byte b, int crc)
        {
            // 
            // Provides an interative call to calculate CRC on the fly
            // 
            int retCRC = (crc >> 8 & 0xFF ^ crcTable[crc & 0xFF ^ Conversions.ToInteger(b)]) & 0xFFFF;
            return retCRC;
        }

        public static int FetchCrc(byte[] buf)
        {
            // 
            // Compute the Resulting CRC on the supplied data buffer
            // 
            int crc = CRCSTART;
            foreach (byte b in buf)
                crc = UpdCrc(b, crc);
            return crc;
        }
    }

    public class WinlinkAuth
    {
        // 
        // Implementation of the WinLink password challenge/response protocol
        // 
        private static byte[] salt = new byte[] { 77, 197, 101, 206, 190, 249, 93, 200, 51, 243, 93, 237, 71, 94, 239, 138, 68, 108, 70, 185, 225, 137, 217, 16, 51, 122, 193, 48, 194, 195, 198, 175, 172, 169, 70, 84, 61, 62, 104, 186, 114, 52, 61, 168, 66, 129, 192, 208, 187, 249, 232, 193, 41, 113, 41, 45, 240, 16, 29, 228, 208, 228, 61, 20 };










        public static string MD5Hash(string text)
        {
            var retBuf = MD5ByteHash(text);
            return Encoding.ASCII.GetString(retBuf);
        }

        public static uint ChallengedPassword(string challengePhrase, string password)
        {
            return ChallengedPassword(challengePhrase, password, salt);
        }

        public static uint ChallengedPassword(string challengePhrase, string password, string secret)
        {
            return ChallengedPassword(challengePhrase, password, Encoding.ASCII.GetBytes(secret));
        }

        private static uint ChallengedPassword(string challengePhrase, string password, byte[] slt)
        {
            // 
            // Calculate the challenge password response as follows:
            // - Concatenate the challenge phrase, password, and supplied secret value (i.e. the salt)
            // - Generate an MD5 hash of the result
            // - Convert the first 4 bytes of the hash to an integer (big endian) and return it
            // 
            int i;
            int m = challengePhrase.Length + password.Length;
            var tmpCP = new byte[(m + slt.Length)];
            Encoding.ASCII.GetBytes(challengePhrase + password).CopyTo(tmpCP, 0);
            slt.CopyTo(tmpCP, m);
            var retHash = MD5ByteHash(tmpCP);
            var loopTo = tmpCP.Length - 1;
            for (i = 0; i <= loopTo; i++)
                // 
                // Zeroize the password array
                // 
                tmpCP[i] = 0;
            // 
            // Create a positive integer return value from the hash bytes
            // 
            uint retVal = Convert.ToUInt32(retHash[3] & 0x3F);
            // Trim the sign bit from what will be the high byte
            for (i = 2; i >= 0; i -= 1)
                retVal = retVal << 8 | retHash[i];
            return retVal;
        }

        private static byte[] MD5ByteHash(string text)
        {
            return MD5ByteHash(Encoding.ASCII.GetBytes(text));
        }

        private static byte[] MD5ByteHash(byte[] iBuf)
        {
            // 
            // Calculate the MD5 hash of the supplied buffer
            // 
            var hash = MD5.Create();
            var retBuf = hash.ComputeHash(iBuf);
            hash.Clear();
            return retBuf;
        }
    }
}