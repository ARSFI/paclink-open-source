using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TNCKissInterface
{
    //
    // Support routines
    //
    // To enable packet logging, set the logFile string to point to a target directory\file
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    // 
    public static class Support
    {
        public static String pktLogFile = "";
        public static String dbgLogFile = "";

        static Object sync = new Object();

        static String CRLF = Convert.ToString((Char)13) + Convert.ToString((Char)10);

        public static Byte[] PackByte(Byte[] buf, Int32 start, Int32 count)
        {
            Byte[] tmpB = new Byte[count];
            for (Int32 i = 0; i < count; i++)
            {
                tmpB[i] = buf[i + start];
            }
            return tmpB;
        }

        //
        // Convert a byte array to a display friendly format
        //
        public static String GetString(Byte[] buf)
        {
            StringBuilder sb = new StringBuilder("");
            for (Int32 i = 0; i < buf.Length; i++)
            {
                if ((buf[i] > 126) || (buf[i] < 32))
                {
                    sb.Append("[" + buf[i].ToString("x2") + "]");
                }
                else
                {
                    sb.Append(Convert.ToChar(buf[i]).ToString());
                }
            }

            return sb.ToString();
        }

        //
        // Output packet information
        //
        public static void PktPrint(String s, ref Object syncObj)
        {
            String filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            String fileName = "ax25PktLog.log";

            DateTime time = DateTime.Now;
            s = time.Year.ToString("d2") + "-" +
                time.Month.ToString("d2") + "-" +
                time.Day.ToString("d2") + ":" +
                time.Hour.ToString("d2") + ":" +
                time.Minute.ToString("d2") + ":" +
                time.Second.ToString("d2") + "." + time.Millisecond.ToString("d3") + s;
            try
            {
                lock (syncObj)
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    StreamWriter fs = File.AppendText(Path.Combine(filePath, fileName));
                    fs.Write(s);
                    fs.Close();
                }
            }
            catch
            {
            }
        }

        [Conditional("DEBUG")]
        public static void DbgPrint(String s)
        {
            DbgPrint(s, false);
        }

        private static object dbgLock = new object();

        [Conditional("DEBUG")]
        public static void DbgPrint(String s, Boolean noCR)
        {
            lock (dbgLock)
            {
                String filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
                String fileName = "ax25DbgLog.log";

                DateTime time = DateTime.Now;
                s = time.Year.ToString("d2") + "-" +
                    time.Month.ToString("d2") + "-" +
                    time.Day.ToString("d2") + ":" +
                    time.Hour.ToString("d2") + ":" +
                    time.Minute.ToString("d2") + ":" +
                    time.Second.ToString("d2") + "." + time.Millisecond.ToString("d3") + "|" + s;

                try
                {
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    StreamWriter fs = File.AppendText(Path.Combine(filePath, fileName));

                    if (noCR)
                    {
                        fs.Write(s);
                    }
                    else
                    {
                        fs.WriteLine(s);
                    }
                    fs.Close();
                }
                catch
                {
                }
            }
        }

        //
        // Format byte data for display
        //
        public static String DumpRawFrame(Byte[] buf)
        {
            StringBuilder s = new StringBuilder("", 4096);

            s.Append("|Buf(");

            for (Int32 i = 0; i < buf.Length; i++)
            {
                s.Append(buf[i].ToString("x2"));
                if (i < buf.Length - 1)
                {
                    s.Append(" ");
                }
            }
            s.Append(")");
            return s.ToString();
        }
    }
}