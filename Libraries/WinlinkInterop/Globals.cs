using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace WinlinkInterop
{
    static class Globals
    {

        // 
        // Information about all Winlink servers.
        // 
        public static WinlinkServer[] AWSSites = new WinlinkServer[] { new WinlinkServer("cms-z.Winlink.org", "", 8772, "CMS", 5) };

        public static bool IsValidCallsign(string strCallsign)
        {
            // 
            // Check the syntax of a callsign.
            // 
            if (string.IsNullOrEmpty(strCallsign))
                return false;
            strCallsign = strCallsign.Trim().ToUpper();
            // split into callsign and ssid
            var parts = strCallsign.Split('-');
            // must be no more than one "-"
            if (parts.Length > 2)
                return false;
            // at least three characters before any "-" character 
            if (parts[0].Length < 3)
                return false;
            // no more than 10 total length
            if (strCallsign.Length > 10)
                return false;
            // only alphanumerics in callsign portion
            if (!Regex.IsMatch(parts[0], "^[A-Z0-9]*$"))
                return false;
            // at least one number
            if (!Regex.IsMatch(parts[0], "[0-9]"))
                return false;
            // if there is a -ssid, check it for value 1 to 15, R, X, or T.
            if (parts.Length > 1)
            {
                // There is an SSID.  Check for radio-only designators.
                if (parts[1] != "R" & parts[1] != "X" & parts[1] != "T")
                {
                    // Check for numeric SSID.
                    int intSSID;
                    if (!int.TryParse(parts[1], out intSSID))
                        return false;
                    if (intSSID < 1 | intSSID > 15)
                        return false;
                }
            }
            // all good
            return true;
        }

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

        public static bool IsValidGridSquare(string strGridSquare)
        {
            if (strGridSquare.Length != 6)
                return false;
            strGridSquare = strGridSquare.ToUpper();
            var objRegex = new Regex("^[A-Z][A-Z][0-9][0-9][A-Z][A-Z]");
            return objRegex.IsMatch(strGridSquare);
        }

        private static bool IsValidHamCallsign(string strCallsign)
        {
            int intDash;
            strCallsign = strCallsign.ToUpper().Trim();
            intDash = strCallsign.IndexOf("-");
            if (intDash == -1)
            {
                if (strCallsign.Length < 3 | strCallsign.Length > 6)
                    return false;
            }
            else if (strCallsign.Length < 3 | strCallsign.Length > 9)
                return false;
            Regex objRegex;
            objRegex = new Regex("^[A-Z][A-Z][0-9][A-Z]");
            if (objRegex.IsMatch(strCallsign))
                return IsValidSSID(strCallsign);
            objRegex = new Regex("^[A-Z][0-9][A-Z]");
            if (objRegex.IsMatch(strCallsign))
                return IsValidSSID(strCallsign);
            objRegex = new Regex("^[A-Z][0-9][0-9][A-Z]");
            if (objRegex.IsMatch(strCallsign))
                return IsValidSSID(strCallsign);
            objRegex = new Regex("^[0-9][A-Z][0-9][A-Z]");
            if (objRegex.IsMatch(strCallsign))
                return IsValidSSID(strCallsign);
            objRegex = new Regex("^[0-9][A-Z][A-Z][0-9][A-Z]");
            if (objRegex.IsMatch(strCallsign))
                return IsValidSSID(strCallsign);
            return IsValidUKCadetCallsign(strCallsign);
        }

        private static bool IsValidSSID(string strCallsign)
        {
            return true;
            if (strCallsign.IndexOf("-") == -1)
                return true;
            if (strCallsign.EndsWith("-1") | strCallsign.EndsWith("-2") | strCallsign.EndsWith("-3") | strCallsign.EndsWith("-4") | strCallsign.EndsWith("-5") | strCallsign.EndsWith("-6") | strCallsign.EndsWith("-7") | strCallsign.EndsWith("-8") | strCallsign.EndsWith("-9") | strCallsign.EndsWith("-10") | strCallsign.EndsWith("-11") | strCallsign.EndsWith("-12") | strCallsign.EndsWith("-13") | strCallsign.EndsWith("-14") | strCallsign.EndsWith("-15"))













                return true;
            return false;
        }

        private static bool IsValidUKCadetCallsign(string strCallsign)
        {
            strCallsign = strCallsign.ToUpper();
            var objRegex = new Regex("^[M][A-Z][A-Z][0-9]");
            if (objRegex.IsMatch(strCallsign))
            {
                return IsValidSSID(strCallsign);
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidPassword(string strPassword)
        {
            // 
            // Return True/False depending on if the syntax of the password is valid.
            // 
            if (string.IsNullOrEmpty(CheckPasswordSyntax(strPassword)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // 
        // Check to see if a password has valid syntax.
        // 
        public static string CheckPasswordSyntax(string strPassword)
        {
            const string strValidChars = ".+!@#$%^&*()_";

            strPassword = strPassword.Trim();
            if (strPassword.Length < 6 | strPassword.Length > 12)
            {
                return "Invalid password: A password must be between 6 and 12 characters long.";
            }

            foreach (char chrSingle in strPassword)
            {
                if (char.IsLetterOrDigit(chrSingle) == false & strValidChars.IndexOf(chrSingle) < 0)
                {
                    return "Invalid password: A password must be composed of any combination of A-Z, a-z, 0-9, .+!@#$%^&*()_";
                }
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
        } // End StripComment

        public static string CleanArg(string strInput, bool blnRemoveCrLf = false)
        {
            // 
            // Replace '&', '<' and '>' characters in a string with the html equivalent.  1.3.0.2
            // 
            if (strInput == null)
                return "";
            var sbdValue = new StringBuilder();
            foreach (char c in strInput)
            {
                if (Conversions.ToString(c) == "&")
                {
                    sbdValue.Append("&amp;");
                }
                else if (Conversions.ToString(c) == "<")
                {
                    sbdValue.Append("&lt;");
                }
                else if (Conversions.ToString(c) == ">")
                {
                    sbdValue.Append("&gt;");
                }
                else if (Conversions.ToString(c) == Constants.vbCr | Conversions.ToString(c) == Constants.vbLf)
                {
                    if (!blnRemoveCrLf)
                        sbdValue.Append(c);
                }
                else if (!char.IsControl(c))
                {
                    sbdValue.Append(c);
                }
            }

            return sbdValue.ToString();
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

        public static byte[] GetBytes(string strText)
        {
            // 
            // Convert a string to an array of bytes.
            // 
            var bytBuffer = new byte[strText.Length];
            for (int intIndex = 0, loopTo = bytBuffer.Length - 1; intIndex <= loopTo; intIndex++)
                bytBuffer[intIndex] = Conversions.ToByte(Strings.Asc(strText.Substring(intIndex, 1)));
            return bytBuffer;
        } // GetBytes

        public static string FormatDateEx(DateTime dttDate)
        {
            // 
            // This function returns the current time/date in 1.3.2.2
            // 2004/08/24 05:33:12 format string...
            // 
            return Strings.Format(dttDate, "yyyy-MM-dd HH:mm:ss");
        }

        public static string DateToRFC822Date(DateTime dtUTCDate)
        {
            // 
            // This function converts a Date type to a standard RFC 822 date string. The date
            // arguement must be in UTC.
            // 
            string sDays = "SunMonTueWedThuFriSat";
            string sMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";
            string sDay;
            string sMonth;
            sDay = sDays.Substring(3 * (int)dtUTCDate.DayOfWeek, 3) + ", ";
            sMonth = " " + sMonths.Substring(3 * (dtUTCDate.Month - 1), 3) + " ";
            return sDay + Strings.Format(dtUTCDate, "dd") + sMonth + Strings.Format(dtUTCDate, "yyyy") + " " + Strings.Format(dtUTCDate, @"HH\:mm\:ss") + " -0000";
        }
    }
}