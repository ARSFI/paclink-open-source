using System;
using System.Collections;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace RMS_Link_Test
{

    // This module holds global variables and utility routines used in this program...
    static class Globals
    {
        public const string CR = "\r";
        public const string LF = "\n";
        public const string CRLF = CR + LF;
        
        public static INIFile objINIFile = new INIFile();
        public static string strProductVersion = Application.ProductVersion;
        public static Queue queDisplayQueue = Queue.Synchronized(new Queue());
        public static string strExecutionDirectory;

        public static string TimestampEx()
        {
            // This function returns the current time/date in 
            // 2004/08/24 05:33:12 format string...

            return DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm");
        } // Timestamp

        public static string FormatDate(DateTime dttDate)
        {
            // Returns the dttDate as a string in Winlink format (Example: 2004/08/24 07:23)...

            return dttDate.ToString("yyyy/MM/dd HH:mm");
        } // FormatDate (Date)
    } // Global
}