using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WinlinkInterop
{
    static class Globals
    {
        public static bool IsValidTacticalAddress(string strCallsign, int intMaxLen = 24)
        {
            // 
            // Checks the format of a tactical address.
            // 
            if (string.IsNullOrEmpty(strCallsign))
                return false;
            strCallsign = strCallsign.ToUpper();
            var parts = strCallsign.Split('-');
            if (parts.Length > 2)
                return false;
            // at least three characters before any "-" character and no more than 12 total
            if (parts[0].Length < 3 | strCallsign.Length > intMaxLen)
            {
                return false;
            }

            // alphas only before any "-" character
            if (!Regex.IsMatch(parts[0], "^[A-Z]*$"))
                return false;

            // alphanumerics only after any "-" character...
            if (parts.Length == 2)
            {
                // only alphanumeric characters after a "-" are accepted
                if (!Regex.IsMatch(parts[1], "^[A-Z0-9]*$"))
                {
                    return false;
                }
            }

            return true;
        }

        // 
        // Check to see if a password has valid syntax.
        // 
        public static string CheckPasswordSyntax(string strPassword)
        {
            strPassword = strPassword.Trim();
            if (strPassword.Length < 6 | strPassword.Length > 12)
            {
                return "Invalid password: A password must be between 6 and 12 characters long.";
            }
            return "";
        }

        public static string StripComment(string strIn)
        {
            int intComment;
            intComment = strIn.IndexOf(";");
            if (intComment >= 0)
            {
                // 
                // Strip off comments
                // 
                strIn = strIn.Substring(0, intComment);
            }

            return strIn.Trim();
        }

        public static string BaseCallsign(string strCallsign)
        {
            // 
            // Extract the base callsign from a callsign that may have an SSID.
            // 
            if (string.IsNullOrEmpty(strCallsign))
                return "";
            if (IsValidTacticalAddress(strCallsign))
                return strCallsign;
            string[] strTokens;
            strTokens = strCallsign.Trim().Split('.');
            if (strTokens.Count() > 1)
                strCallsign = strTokens[0];
            strTokens = strCallsign.Trim().Split('-');
            return strTokens[0].Trim().ToUpper();
        }

        public static string FormatDateEx(DateTime dttDate)
        {
            // 
            // This function returns the current time/date in 1.3.2.2
            // 2004/08/24 05:33:12 format string...
            // 
            return dttDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}