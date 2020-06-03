
// Class to hold a message address type...
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public class WinlinkAddress
    {
        private string strSMTPAddress;
        private string strRadioAddress;

        public WinlinkAddress(string strNewAddress)
        {
            // Called to create a new address object...

            FormatAddress(strNewAddress);
        } // New

        public bool IsCallsign
        {
            // Returns True if address is callsign or tactical address,
            // returns False if an Internet address...

            get
            {
                if (RadioAddress.IndexOf("@") > -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        } // IsCallsign

        public string SMTPAddress
        {
            // Sets or returns address in Internet address format...

            get
            {
                return strSMTPAddress;
            }

            set
            {
                FormatAddress(value);
            }
        } // SMTPAddress

        public string RadioAddress
        {
            // Sets or returns address in radio address format...

            get
            {
                return strRadioAddress;
            }

            set
            {
                FormatAddress(value);
            }
        } // RadioAddress

        public static bool IsValidRadioCallsign(string strCallsign)
        {
            // Checks the format of Ham or MARS call...

            int intIndex;
            int intDigitFound = 0;
            int intDash;
            int intSSID;
            string strChr;
            strCallsign = strCallsign.ToUpper().Trim();
            if (strCallsign.Length < 3 | strCallsign.Length > 10)
                return false;
            intDash = strCallsign.IndexOf("-");

            // Must be at least 3 characters ahead of "-"...
            if (intDash >= 0 & intDash < 3)
                return false; // must be at least 3 char ahead of "-"

            // Must be only one "-"...
            if (intDash > 0 & intDash != strCallsign.LastIndexOf("-"))
                return false; // must be only one "-"
            if (intDash > 0) // there is a -ssid, check it for value 1 to 15
            {
                try
                {
                    // Try to convert to integer; a failure inidcates non integer...
                    intSSID = Conversions.ToInteger(strCallsign.Substring(intDash + 1));

                    // Must be 1-15...
                    if (intSSID < 1 | intSSID > 15)
                        return false;
                }
                catch
                {
                    return false;
                } // SSID was not an integer
            }

            // If you get here the ssid is missing or is OK ("-1" to "-15")...
            if (intDash == -1)
                intDash = strCallsign.Length;

            // Search up to the "-" for alpha and numerics...
            var loopTo = intDash - 1;
            for (intIndex = 0; intIndex <= loopTo; intIndex++)
            {
                strChr = strCallsign.Substring(intIndex, 1);

                // Return false if not A-Z or 0-9...
                if (!(strChr.CompareTo(Conversions.ToString('A')) >= 0 & strChr.CompareTo(Conversions.ToString('Z')) <= 0 | strChr.CompareTo(Conversions.ToString('0')) >= 0 & strChr.CompareTo(Conversions.ToString('9')) <= 0))
                    return false;

                // Count the numeric digits...
                if (strChr.CompareTo(Conversions.ToString('0')) >= 0 & strChr.CompareTo(Conversions.ToString('9')) <= 0)
                    intDigitFound += 1; // count the numeric digits
            }

            // Return true only if 1 or 2 digits found...
            return intDigitFound > 0 & intDigitFound < 3;
        } // IsValidCallsign

        private void FormatAddress(string strNewAddress)
        {
            // Formats a new address and saves it in both SMTP and radio format...

            strNewAddress = ParseAddress(strNewAddress);
            strNewAddress = CorrectFaultyAddress(strNewAddress);
            if (!string.IsNullOrEmpty(strNewAddress))
            {
                string strAddress = strNewAddress.ToUpper();
                if (strAddress.StartsWith("SMTP:"))
                {
                    strSMTPAddress = strNewAddress;
                    strRadioAddress = MakeRadioAddress(strSMTPAddress);
                }
                else if (strAddress.IndexOf("@WINLINK.ORG") > -1)
                {
                    strRadioAddress = MakeRadioAddress(strNewAddress);
                    strSMTPAddress = MakeSMTPAddress(strRadioAddress);
                }
                else if (strAddress.IndexOf("@") > -1)
                {
                    strSMTPAddress = strNewAddress;
                    strRadioAddress = MakeRadioAddress(strSMTPAddress);
                }
                else
                {
                    strRadioAddress = MakeRadioAddress(strNewAddress);
                    strSMTPAddress = MakeSMTPAddress(strRadioAddress);
                }
            }
            else
            {
                strSMTPAddress = "";
                strRadioAddress = "";
            }
        } // FormatAddress

        public static string ParseAddress(string strLine)
        {
            // Parses a complex Internet address and returns the basic address only.
            // Returns an empty string if it is a bad address and cannot be parsed...

            int intStart;
            int intEnd;
            try
            {
                // Remove any '"text"' style substring...
                intStart = strLine.IndexOf("\"");
                if (intStart > -1)
                {
                    intEnd = strLine.Substring(intStart + 1).IndexOf("\"");
                    if (intEnd > -1)
                    {
                        strLine = strLine.Replace(strLine.Substring(intStart, intEnd), "").Trim();
                    }
                    else if (strLine.IndexOf("<") == -1)
                    {
                        // Bad address - cannot be parsed...
                        return "";
                    }
                }

                // Remove '(text)' style substring...
                intStart = strLine.IndexOf("(");
                if (intStart > -1)
                {
                    intEnd = strLine.IndexOf(")");
                    if (intEnd > -1)
                    {
                        strLine = strLine.Replace(strLine.Substring(intStart, intEnd - intStart + 1), "").Trim();
                    }
                    else
                    {
                        // Bad address - cannot be parsed...
                        return "";
                    }
                }

                // Remove '[text]' style substring...
                intStart = strLine.IndexOf("[");
                if (intStart > -1)
                {
                    intEnd = strLine.IndexOf("]");
                    if (intEnd > -1)
                    {
                        strLine = strLine.Replace(strLine.Substring(intStart, intEnd - intStart + 1), "").Trim();
                    }
                    else
                    {
                        // Bad address - cannot be parsed...
                        return "";
                    }
                }

                // Extract address from < > boundries if they exist...
                strLine = strLine.Trim();
                intStart = strLine.IndexOf("<");
                if (intStart != -1)
                {
                    intStart += 1;
                    intEnd = strLine.IndexOf(">");
                    if (intEnd != -1)
                    {
                        strLine = strLine.Substring(intStart, intEnd - intStart).Trim();
                    }
                    else
                    {
                        // Bad address - cannot be parsed...
                        return "";
                    }
                }

                // Check for ":" in the address and remove all characters up to and including it...
                if (strLine.IndexOf(":") == -1)
                {
                    return strLine.Trim();
                }
                else
                {
                    return strLine.Substring(strLine.IndexOf(":") + 1).Trim();
                }
            }
            catch
            {
                Logs.Exception("[7754] " + strLine + " " + Information.Err().Description);
                return "";
            }
        } // ParseAddress

        private string CorrectFaultyAddress(string strAddress)
        {
            if (strAddress.ToUpper().StartsWith("SERVICE@"))
            {
                if (strAddress.Substring(strAddress.IndexOf("@")).IndexOf(".") == -1)
                {
                    return "Service";
                }
                else
                {
                    return strAddress;
                }
            }
            else
            {
                return strAddress;
            }
        } // CorrectFaultyAddress

        private string MakeRadioAddress(string strAddress)
        {
            // Converts an Internet format address to a radio format... 

            string strBuffer = Strings.UCase(strAddress);
            if (strBuffer.IndexOf("@WINLINK.ORG") != -1)
            {
                strBuffer = strBuffer.Replace("SMTP:", "");
                return strBuffer.Replace("@WINLINK.ORG", "").ToUpper().Trim();
            }
            else if (strBuffer.IndexOf("@") != -1 & strBuffer.StartsWith("SMTP:") == false)
            {
                return "SMTP:" + strAddress;
            }
            else if (strBuffer.StartsWith("SMTP:"))
            {
                return strAddress;
            }
            else
            {
                strAddress = strAddress.ToUpper();
                strAddress = strAddress.Replace("'", "");
                if (strAddress.ToUpper() == "SERVICE")
                    return "Service";
                if (strAddress.ToUpper() == "SYSTEM")
                    return "System";
                return strAddress;
            }
        } // MakeRadioAddress

        private string MakeSMTPAddress(string strAddress)
        {
            // Converts a radio callsign into an SMTP address...

            return strAddress + "@Winlink.org";
        } // MakeSMTPAddress
    } // WinlinkAddress
}