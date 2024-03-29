﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using NLog;
using Paclink.Data;
using Paclink.UI.Common;
using winlink;
using winlink.cms.webservices;

namespace Paclink
{
    static class Globals
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public static IDatabase Database = DatabaseFactory.Get();
        public static IProperties Settings = new Properties(Database);
        public static IPollingBacking PollingData = new DialogPollingViewModel();

        public const string LF = "\n";
        public const string CR = "\r";
        public const string CRLF = CR + LF;

        // Use this constant to clear an RTF text display...
        public const string CLEAR = "\u0001";

        // Holds all enabled channel names in order of priority...
        public static ArrayList AutomaticChannels = new ArrayList();

        // Holds all of the properties for the currently active channel...
        public static ChannelProperties stcSelectedChannel;
        public static ChannelProperties stcEditedSelectedChannel = default;

        // Main form display components...
        public static Queue queChannelDisplay = Queue.Synchronized(new Queue());
        public static Queue queProgressDisplay = Queue.Synchronized(new Queue());
        public static Queue queRateDisplay = Queue.Synchronized(new Queue());
        public static Queue queSMTPDisplay = Queue.Synchronized(new Queue());
        public static Queue queSMTPStatus = Queue.Synchronized(new Queue());
        public static Queue queStateDisplay = Queue.Synchronized(new Queue());
        public static Queue queStatusDisplay = Queue.Synchronized(new Queue());

        // Global Strings...
        public static string MailSystemPassword = "";
        public static string SecureLoginPassword = "";
        public static string SiteBinDirectory = "";
        public static string SiteCallsign = "";
        public static string strServiceCodes = "PUBLIC";
        public static string SiteDataDirectory = "";
        public static string SiteGridSquare = "";
        public static string SiteRootDirectory = "";
        public static string strConnectedCallsign = ""; // Callsign of the connected station
        public static string strConnectedGridSquare = ""; // Grid square of connected station.
        public static string strLocalIPAddress = "Default";
        public static string[] strLocalIPAddresses;
        public static string strProductName;
        public static string strProductVersion;
        public static string strRMSRelayIPPath;
        // Public strExecutionDirectory As String
        public static string strMARSServiceCode = "MARS211576"; //TODO: This should be a configuration item as it can change over time
        public static string strHamServiceCode = "PUBLIC";
        public static string strNewAUVersion;

        // Global Booleans...
        public static bool blnAutoForwarding;
        public static bool blnChannelActive;
        public static bool blnEnableRadar;
        public static bool blnEndBearingDisplay;
        public static bool blnFQSeen;
        public static bool blnLAN;
        public static bool blnManualAbort;
        public static bool blnPactorDialogClosing;
        public static bool blnPactorDialogResume = true;
        public static bool blnPactorDialogResuming;
        public static bool blnWINMORDialogClosing;
        public static bool blnWINMORDialogResume = true;
        public static int intPendingForClients;
        public static int intPendingForWinlink;
        public static bool blnProgramClosing;
        public static bool blnUpdateComplete;
        public static bool blnStartingChannel;
        public static bool blnStartMinimized;
        public static bool blnUseExternalDNS;
        public static bool blnUseRMSRelay;
        public static bool blnRunningInTestMode;
        public static bool blnAbortAU;
        public static bool blnEnablAutoforward = false;
        public static bool blnForceHFRouting = false;
        public static bool TerminalIsActive = false;    // True when the simple terminal is active (com port is in use)

        // Global Integers...
        public static int intAutoforwardChannelIndex = 999999;
        public static int intPOP3PortNumber;
        public static int intSMTPPortNumber;
        private static int intProgressBarNumerator;
        private static int intProgressBarDenominator;
        public static int intRMSRelayPort = 8772;
        public static int intComCloseTime = 200;             // ms to pause after closing a serial port

        // Global DateTime
        public static DateTime dttPostVersionRecord = DateTime.UtcNow.AddDays(-2);

        // Global Collections...
        public static List<string> cllFastStart = new List<string>();

        // Global Objects...
        public static IPactorConnectWindow dlgPactorConnect;
        public static POP3Port objPOP3Port;
        public static PrimaryThread objPrimaryThread;
        public static IRadio objRadioControl;
        public static IModem ObjScsModem;
        public static IModem ObjSelectedModem;
        public static SMTPPort objSMTPPort;
        public static DateTime objUpTime = DateTime.UtcNow;
        public static Main objMain;

        // Enumerations
        public static ProtocolB2.EB2States enmEB2States;

        static Globals()
        {
#if DEBUG
            var host = "http://cms-z.winlink.org";
#else
            var host = Globals.Settings.Get("System", "WebServices API Domain", "https://api.winlink.org");
#endif
            //initialize api static class with our access key 
            WinlinkWebServices.SetConfiguration(new WinlinkWebServiceConfiguration { WebServicesHost = host, WebServiceAccessKey = "3EBF46286B3D4A049929F1FB999F4FD1" });
        }

        public class Proposal
        {
            public string msgID;
            public int uncompressedSize;
            public int compressedSize;
        }

        public static string Left(this String str, int length)
        {
            return str.Substring(0, length);
        }

        public static string Right(this String str, int length)
        {
            if (str.Length >= length)
            {
                return str.Substring(str.Length - length, length);
            }
            else
            {
                return str;
            }
        }

        public static void ResetProgressBar(int intValue = 0)
        {
            queProgressDisplay.Enqueue(0);
            intProgressBarNumerator = 0;
            if (intValue == 0)
            {
                intProgressBarDenominator = 0;
            }
            else
            {
                intProgressBarDenominator += intValue;
            }
        } // ResetProgressBar

        public static void UpdateProgressBar(int intDelta = 0)
        {
            if (intProgressBarNumerator + intDelta > intProgressBarDenominator)
            {
                intProgressBarNumerator = intProgressBarDenominator;
            }
            else
            {
                intProgressBarNumerator += intDelta;
            }

            if (enmEB2States == ProtocolB2.EB2States.ReceivingB2Messages | enmEB2States == ProtocolB2.EB2States.SendingB2Messages | enmEB2States == ProtocolB2.EB2States.WaitingForAcknowledgement)

            {
                if (intProgressBarDenominator == 0)
                {
                    queProgressDisplay.Enqueue(0);
                }
                else if (intProgressBarNumerator > intProgressBarDenominator)
                {
                    queProgressDisplay.Enqueue(100);
                }
                else
                {
                    queProgressDisplay.Enqueue(Math.Min(100, (int)(intProgressBarNumerator / (double)intProgressBarDenominator * 100)));
                }
            }
            else
            {
                intProgressBarNumerator = 0;
                intProgressBarDenominator = 0;
                queProgressDisplay.Enqueue(0);
            }
        } // UpdateProgressBar

        public static string ProgressBarStatus()
        {
            if (intProgressBarDenominator > 0)
            {
                return intProgressBarNumerator.ToString() + "/" + intProgressBarDenominator.ToString();
            }
            else
            {
                return "";
            }
        } // ProgressBarStatus

        public static string Timestamp()
        {
            // This function returns the current time/date in 
            // 2004/08/24 05:33 format string...

            return DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm");
        } // Timestamp

        public static string TimestampEx()
        {
            // This function returns the current time/date in 
            // 2004/08/24 05:33:12 format string...

            return DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss");
        } // TimestampEx

        public static string FormatDate(DateTime dttDate)
        {
            // Returns the dttDate as a string in Winlink format (Example: 2004/08/24 07:23)...

            return dttDate.ToString("yyyy/MM/dd HH:mm");
        } // FormatDate (Date)

        public static string FormatDate(string strDate)
        {
            // Returns the strDate as a string in Winlink format (Example: 2004/08/24 07:23)...

            return Convert.ToDateTime(strDate).ToString("yyyy/MM/dd HH:mm");
        } // FormatDate (String)

        public static string ReformatDate(string strSource)
        {
            // Converts a mm/dd/yyyy format to a Winlink format (Example: 2004/08/24 07:23)...

            strSource = strSource.Trim();
            DateTime tmpVal = default(DateTime);
            if (DateTime.TryParse(strSource, out tmpVal))
            {
                try
                {
                    var sDate = strSource.Split('/');
                    if (Convert.ToInt64(sDate[0]) <= 12)
                    {
                        var sTime = sDate[2].Split(' ');
                        if ((sTime.Length - 1) == 0)
                        {
                            Array.Resize(ref sTime, 2);
                        }

                        return (sTime[0] + "/" + sDate[0] + "/" + sDate[1] + " " + sTime[1]).Trim();
                    }
                    else
                    {
                        return strSource;
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("[ReformatDate] " + strSource + " " + ex.Message);
                    return strSource;
                }
            }
            else
            {
                return strSource;
            }
        } // ReformatDate (string)

        public static string GetUptime()
        {
            string GetUptimeRet = default;
            // 
            // Determine how long the application has been running
            // 
            var objDelta = DateTime.UtcNow.Subtract(objUpTime);
            GetUptimeRet = string.Format("Uptime: {0:d} {1:d2}:{2:d2}:{3:d2}", objDelta.Days, objDelta.Hours, objDelta.Minutes, objDelta.Seconds);
            return GetUptimeRet;
        }

        public static void FrameDebug(byte[] bytFrame, int intUpperBound = -1)
        {
            if (intUpperBound == -1)
                intUpperBound = bytFrame.GetUpperBound(0);
            for (int intIndex = 0, loopTo = intUpperBound; intIndex <= loopTo; intIndex++)
            {
                if (bytFrame[intIndex] < 0x20 | bytFrame[intIndex] > 0x7E)
                {
                    Debug.Write("[" + bytFrame[intIndex].ToString("X") + "]");
                }
                else
                {
                    Debug.Write(Convert.ToString((char)bytFrame[intIndex]));
                }
            }

            Debug.Write(" " + DateTime.Now.Second.ToString() + "." + DateTime.Now.Millisecond.ToString("000") + " ");
            Debug.WriteLine("");
        } // FrameDebug

        public static byte[] GetBytes(string strText)
        {
            // Converts a text string to a byte array...

            var bytBuffer = new byte[strText.Length];
            for (int intIndex = 0, loopTo = bytBuffer.Length - 1; intIndex <= loopTo; intIndex++)
                bytBuffer[intIndex] = Convert.ToByte(Globals.Asc(strText.Substring(intIndex, 1)[0]));
            return bytBuffer;
        } // GetBytes

        public static string GetString(byte[] bytBuffer, int intFirst = 0, int intLast = -1)
        {
            // Converts a byte array to a text string...

            if (intFirst > bytBuffer.GetUpperBound(0))
                return "";
            if (intLast == -1 | intLast > bytBuffer.GetUpperBound(0))
                intLast = bytBuffer.GetUpperBound(0);
            var sbdInput = new StringBuilder();
            for (int intIndex = intFirst, loopTo = intLast; intIndex <= loopTo; intIndex++)
            {
                byte bytSingle = bytBuffer[intIndex];
                if (bytSingle != 0)
                    sbdInput.Append((char)bytSingle);
            }

            return sbdInput.ToString();
        } // GetString

        public static int ComputeLengthL(byte[] Buffer, int Index)
        {
            int ComputeLengthLRet = default;
            // Compute the length based on the 4 byte value LSB to MSB
            // normally index = 28 for data length...

            int intLength;
            int n;
            intLength = 0;
            for (n = 0; n <= 3; n++)
                intLength = intLength + (int)(Buffer[n + Index] * Math.Pow(256, n));
            ComputeLengthLRet = intLength;
            return ComputeLengthLRet;
        }  // ComputeLengthL

        public static byte[] ComputeLengthB(long lLength)
        {
            byte[] ComputeLengthBRet = default;
            // Compute the 4 byte array based on the length
            // order is Lsbyte (0) to Msbyte (3)...

            var bLength = new byte[4];
            int n;
            long lLen;
            long lQ;
            lLen = lLength;
            for (n = 1; n <= 4; n++)
            {
                lQ = lLen / Convert.ToInt32(Math.Pow(256, 4 - n)); // note integer divide 
                lLen = lLen - Convert.ToInt32(lQ * Math.Pow(256, 4 - n));
                bLength[4 - n] = Convert.ToByte(lQ);
            }

            ComputeLengthBRet = bLength;
            return ComputeLengthBRet;
        } // ComputeLengthB

        public static void ConcatanateByteArrays(ref byte[] bytOriginal, byte[] bytToAdd)
        {
            // Concatanate one byte array to another...

            int intOriginalSize;
            intOriginalSize = bytOriginal.Length;
            Array.Resize(ref bytOriginal, bytOriginal.Length + (bytToAdd.Length - 1) + 1);
            Array.Copy(bytToAdd, 0, bytOriginal, intOriginalSize, bytToAdd.Length);
        } // ConcatanateByteArrays

        internal static string GetNewRandomMid()
        {
            // Returns a new random message ID...

            string strSet = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var objEncoder = new RNGCryptoServiceProvider();
            var bytBuffer = new byte[12];
            objEncoder.GetBytes(bytBuffer);
            var sbdInput = new StringBuilder();
            foreach (byte bytSingle in bytBuffer)
                sbdInput.Append(strSet.Substring(bytSingle % 36, 1));
            return sbdInput.ToString();
        } // GetNewRandomMid

        public static string StringToSQL(string strText)
        {
            // Returns a string formatted to be included in an SQL string...

            return strText.Replace(@"\", @"\\").Replace("'", "''");
        } // StringToSQL

        public static DateTime RFC822DateToDate(string sDate)
        {
            // This function converts a standard RFC 822 date/time string to a UTC Date type
            // with full correction for UTC to local offset. If the argument does not convert
            // to a proper date then the current UTC date/time is returned...

            if (sDate.IndexOf(",") < 0)
                sDate = "---, " + sDate;
            string sMonths = "   JANFEBMARAPRMAYJUNJULAUGSEPOCTNOVDEC";
            var sDateParts = sDate.Split(' ');
            int nMonth;
            int nOffsetHours = 0;
            int nOffsetMinutes = 0;
            bool bWest = true;
            try
            {
                nMonth = sMonths.IndexOf(sDateParts[2].ToUpper()) / 3;
                if (nMonth < 1)
                    return DateTime.UtcNow;
                string sNewDate = sDateParts[3] + "/" + nMonth.ToString() + "/" + sDateParts[1] + " " + sDateParts[4];
                DateTime dtDate = Convert.ToDateTime(sNewDate);
                var switchExpr = sDateParts[5];
                switch (switchExpr)
                {
                    case "UT":
                    case "GMT":
                    case "Z":
                        {
                            break;
                        }
                    // Do nothing...
                    case "A":
                        {
                            nOffsetHours = 1;
                            break;
                        }

                    case "M":
                        {
                            nOffsetHours = 12;
                            break;
                        }

                    case "N":
                        {
                            bWest = false;
                            nOffsetHours = 1;
                            break;
                        }

                    case "Y":
                        {
                            bWest = false;
                            nOffsetHours = 12;
                            break;
                        }

                    case "EDT":
                        {
                            nOffsetHours = 4;
                            break;
                        }

                    case "EST":
                    case "CDT":
                        {
                            nOffsetHours = 5;
                            break;
                        }

                    case "CST":
                    case "MDT":
                        {
                            nOffsetHours = 6;
                            break;
                        }

                    case "MST":
                    case "PDT":
                        {
                            nOffsetHours = 7;
                            break;
                        }

                    case "PST":
                        {
                            nOffsetHours = 8;
                            break;
                        }

                    default:
                        {
                            if (sDateParts[5].Substring(0, 1) != "-")
                                bWest = false;
                            nOffsetHours = Convert.ToInt32(sDateParts[5].Substring(1, 2));
                            nOffsetMinutes = Convert.ToInt32(sDateParts[5].Substring(3, 2));
                            break;
                        }
                }

                if (bWest)
                {
                    dtDate = dtDate.AddHours(nOffsetHours);
                    dtDate = dtDate.AddMinutes(nOffsetMinutes);
                }
                else
                {
                    dtDate = dtDate.AddHours(-nOffsetHours);
                    dtDate = dtDate.AddMinutes(-nOffsetMinutes);
                }

                return dtDate;
            }
            catch
            {
                return DateTime.UtcNow;
            }
        } // RFC822DateToDate

        public static string DateToRFC822Date(DateTime dtUTCDate)
        {
            // This function converts a Date type to a standard RFC 822 date string. The date
            // arguement must be in UTC...

            string sDays = "SunMonTueWedThuFriSat";
            string sMonths = "JanFebMarAprMayJunJulAugSepOctNovDec";
            string sDay;
            string sMonth;
            sDay = sDays.Substring(3 * (int)dtUTCDate.DayOfWeek, 3) + ", ";
            sMonth = " " + sMonths.Substring(3 * (dtUTCDate.Month - 1), 3) + " ";
            return sDay + dtUTCDate.ToString("dd") + sMonth + dtUTCDate.ToString("yyyy") + " " + dtUTCDate.ToString("HH:mm:ss") + " -0000";
        } // DateToRFC822Date

        public static string ParseAddress(string strLine)
        {
            // Parses a complex Internet address and returns the basic address only.
            // Returns and empty string if it is a bad address and cannot be parsed...

            int intStart;
            int intEnd;
            try
            {
                // Remove any quoted or paren strings...
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

                intStart = strLine.IndexOf("(");
                if (intStart > -1)
                {
                    intEnd = strLine.Substring(intStart + 1).IndexOf(")");
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

                // Extract address from < > boundaries if they exist...
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
                return "";
            }
        } // ParseAddress

        public static bool IsValidFileName(string strName)
        {
            string strInvalidCharacters = @"\/:*?""<>|";
            foreach (char chr in strName)
            {
                if (strInvalidCharacters.IndexOf(chr) != -1)
                {
                    UserInterfaceFactory.GetUiSystem().DisplayModalError(
                        "The characters " + strInvalidCharacters + " are not allowed in file, account, or channel names...",
                        "Error");
                    return false;
                }
            }

            return true;
        } // IsValidFileName

        public static bool IsValidRadioCallsign(string strCallsign)
        {
            // Function to check validity of Ham, MARS or UK Kadet call... 
            int intIndex;
            int intDigitFound = 0;
            int intDash;
            int lSSID;
            string strChar;
            if (string.IsNullOrEmpty(strCallsign))
                return false;
            strCallsign = strCallsign.ToUpper().Trim();
            if (strCallsign.Length < 3 | strCallsign.Length > 10)
                return false;
            intDash = strCallsign.IndexOf("-");
            if (intDash >= 0 & intDash < 3)
                return false; // must be at least 3 char ahead of "-"
            if (intDash > 0 & intDash != strCallsign.LastIndexOf("-"))
                return false; // must be only one "-"
            if (intDash > 0) // there is a -ssid, check it for value 1 to 15
            {
                try
                {
                    lSSID = Convert.ToInt32(strCallsign.Substring(intDash + 1)); // Try to convert to integer a failure inidcates non integer
                    if (lSSID < 1 | lSSID > 15)
                        return false; // must be 1-15
                }
                catch
                {
                    return false;
                } // SSID was not an integer 
            }
            // If you get here the ssid is missing or is OK ("-1" to "-15")
            if (intDash == -1)
                intDash = strCallsign.Length;
            var loopTo = intDash - 1;
            for (intIndex = 0; intIndex <= loopTo; intIndex++) // Search up to the "-" for alpha and numerics 
            {
                strChar = strCallsign.Substring(intIndex, 1);
                // Return false if not A-Z or 0-9...
                if (!(strChar.CompareTo(Convert.ToString('A')) >= 0 & strChar.CompareTo(Convert.ToString('Z')) <= 0 | strChar.CompareTo(Convert.ToString('0')) >= 0 & strChar.CompareTo(Convert.ToString('9')) <= 0))
                    return false;

                // Count the numeric digits...
                if (strChar.CompareTo(Convert.ToString('0')) >= 0 & strChar.CompareTo(Convert.ToString('9')) <= 0)
                    intDigitFound += 1;
            }

            return intDigitFound > 0;
        } // IsValidHamCall

        public static bool IsUKCadetCall(string strName)
        {
            // Function to check validity of UK Cadet call... Tested but not yet used.
            // Will be used to identify UK Cadet calls and add to Public PMBO list similar to 
            // Way EMComm list is now processed ....RM

            // All calls are in the series MXXxxX, where the first call-sign letter is 
            // always a M (i.e. Military) then there are _always_ two letters following
            // (the first shows the Service the second is often an area identifier); 
            // the xx are _always_ two digits and the last X is optional and is a call-sign
            // identifier (like A could be 1st mobile, B the second mobile, etc)
            // all calls may include an optional -ssid 1 - 15)
            // Typical calls:  MRW48   MFJ03   MCC72C-10   MAC74B, etc.

            int intIndex;
            int intDigitFound = 0;
            int intDash;
            int lSSID;
            string strChar;
            strName = strName.ToUpper().Trim();
            if (strName.Length < 5 | strName.Length > 10)
                return false;
            if (!strName.StartsWith("M"))
                return false;
            intDash = strName.IndexOf("-");
            if (intDash >= 0 & intDash < 5)
                return false; // must be at least 5 char ahead of "-"
            if (intDash > 0 & intDash != strName.LastIndexOf("-"))
                return false; // must be only one "-"
            if (intDash > 0) // there is a -ssid, check it for value 1 to 15
            {
                try
                {
                    lSSID = Convert.ToInt32(strName.Substring(intDash + 1)); // Try to convert to integer a failure inidcates non integer
                    if (lSSID < 1 | lSSID > 15)
                        return false; // must be 1-15
                }
                catch
                {
                    return false;
                } // SSID was not an integer 
            }
            // If you get here the ssid is missing or is OK ("-1" to "-15")
            if (intDash == -1)
                intDash = strName.Length;
            var loopTo = intDash - 1;
            for (intIndex = 0; intIndex <= loopTo; intIndex++) // Search up to the "-" for alpha and numerics 
            {
                strChar = strName.Substring(intIndex, 1);
                if (intIndex < 3)  // Return false if not A-Z...
                {
                    if (!(strChar.CompareTo(Convert.ToString('A')) >= 0 & strChar.CompareTo(Convert.ToString('Z')) <= 0))
                        return false;
                }
                else if (intIndex == 3 | intIndex == 4) // look for two digits
                {
                    if (!(strChar.CompareTo(Convert.ToString('0')) >= 0 & strChar.CompareTo(Convert.ToString('9')) <= 0))
                        return false;
                }
                else if (!(strChar.CompareTo(Convert.ToString('A')) >= 0 & strChar.CompareTo(Convert.ToString('Z')) <= 0)) // look for Alphas
                {
                    return false;
                }
            }

            return true;
        } // IsUKKadetCall

        public static bool IsMARSCallsign(string strCallsign)
        {
            if (string.IsNullOrEmpty(strCallsign))
                return false;
            var chrCallsign = strCallsign.ToUpper().ToCharArray();
            if (chrCallsign[0] >= 'A' & chrCallsign[0] <= 'Z')
            {
                if (chrCallsign[1] >= 'A' & chrCallsign[1] <= 'Z')
                {
                    if (chrCallsign[2] >= 'A' & chrCallsign[2] <= 'Z')
                    {
                        if (chrCallsign[3] >= '0' & chrCallsign[3] <= '9')
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        } // IsMARSCallsign

        public static bool IsWEBSiteOnLine()
        {
            try
            {
                // Check if the WEB site MySQL server is on line...
                var objTCP = new System.Net.Sockets.TcpClient();
                objTCP.Connect("winlink.org", 3306);
                objTCP.Close();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[IsWEBSiteOnLine] " + ex.Message);
                return false;
            }
        } // IsWEBSiteOnLine

        public static bool IsCMSSiteOnLine(string strURL)
        {
            try
            {
                // Check if the WEB site MySQL server is on line...
                var objTCP = new System.Net.Sockets.TcpClient();
                objTCP.Connect(strURL, 3306);
                objTCP.Close();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("[IsCMSSiteOnLine] " + ex.Message);
                return false;
            }
        } // IsCMSSiteOnLine

        public static bool IsManualPactorOnly()
        {
            if (Channels.Entries.Count == 0)
                return false;
            var stcChannel = default(ChannelProperties);
            foreach (ChannelProperties currentStcChannel in Channels.Entries.Values)
            {
                stcChannel = currentStcChannel;
                if (stcChannel.ChannelType != ChannelMode.PactorTNC & stcChannel.Enabled == true)
                    return false;
                if (stcChannel.ChannelType == ChannelMode.PactorTNC & stcChannel.EnableAutoforward)
                    return false;
            }

            stcSelectedChannel = stcChannel;
            return true;
        } // IsManualPactorOnly

        public static void AppendBuffer(ref byte[] bytOriginal, byte[] bytToAdd)
        {
            // Concatanate one byte array to another...

            int intOriginalSize;
            intOriginalSize = bytOriginal.Length;
            Array.Resize(ref bytOriginal, bytOriginal.Length + (bytToAdd.Length - 1) + 1);
            Array.Copy(bytToAdd, 0, bytOriginal, intOriginalSize, bytToAdd.Length);
        } // AppendBuffer

        public static void UpdateAccountDirectories()
        {
            // Remove and account directories that are no longer used...
            string strDirectoryNames = Settings.Get("Properties", "Account Names", "");

            var messageStore = new MessageStore(DatabaseFactory.Get());
            foreach (string account in messageStore.GetAccountsWithEmails())
            {
                if (strDirectoryNames.IndexOf(account + "|") == -1)
                {
                    messageStore.DeleteAccountEmails(account);
                }
            }
        } // UpdateAccountDirectories

        public static bool WithinLimits(string TestValue, double HighLimit, double LowLimit)
        {
            // Simple function to test a string value to numeric limits
            double dblTestValue = 0.0;
            if (!double.TryParse(TestValue, out dblTestValue))
                return false;
            if (dblTestValue > HighLimit)
                return false;
            if (dblTestValue < LowLimit)
                return false;
            return true;
        } // WithinLimits

        public static void InitializeLocalIPAddresses()
        {
            // Subroutine for initializing available and default IP addresses for multi home applications...
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            int intCount = adapters.Length;
            var tempIPList = new List<string>();
            if (intCount > 0)
            {
                tempIPList.Add("Default");
            }
            else
            {
                // MsgBox("No network or local IP interface found on this computer..." & vbCrLf & _
                // "You many need to correct this problem and restart the program" & vbCrLf & _
                // "for features requiring local or remote IP connections to work.", MessageBoxIcon.Critical)
                return;
            }

            // Get the dotted IP address for each adapter...
            foreach (var adapter in adapters)
            {
                var ipProperties = adapter.GetIPProperties();
                foreach (var ipAddress in ipProperties.UnicastAddresses)
                {
                    if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        tempIPList.Add(ipAddress.Address.ToString());
                    }
                }
            }
            strLocalIPAddresses = tempIPList.ToArray();

            int intIndex = Settings.Get("Properties", "Default Local IP Address Index", 0);
            if (intIndex < 0)
                intIndex = 0;
            try
            {
                strLocalIPAddress = strLocalIPAddresses[intIndex];
            }
            catch
            {
                // If index failure (caused by change in IPConfig) then set to default
                if (strLocalIPAddresses.Length > 0)
                    strLocalIPAddress = strLocalIPAddresses[0];
            }
        }  // InitializeLocalIPAddresses

        public static void PostVersionRecord(bool blnReportClosing = false)
        {
            // 
            // Start a thread to post a version report.
            // 
            dttPostVersionRecord = DateTime.UtcNow;
            var thrPostVersion = new Thread(PostVersionRecordThread);
            thrPostVersion.Start(blnReportClosing);
        }

        public static void PostVersionRecordThread(object objReportClosing)
        {
            try
            {
                WinlinkWebServices.AddProgramVersion(SiteCallsign, strProductName, strProductVersion, "");
            }
            catch (Exception ex)
            {
                _log.Warn(ex, ex.Message);
            }
        }

        public static string GetBaseCallsign(string strCallsign)
        {
            if (string.IsNullOrEmpty(strCallsign))
                return "";
            var strTokens = strCallsign.Trim().Split('-');
            return strTokens[0].Trim();
        } // GetBaseCallsign

        public static int KHzToHz(string strKHz)
        {
            var intFrequency = default(int);
            strKHz = ExtractFreq(strKHz);
            try
            {
                var strTokens = strKHz.Split('.');
                int tmpVal = 0;
                if (int.TryParse(strTokens[0], out tmpVal))
                {
                    intFrequency = tmpVal * 1000;
                }

                if (strTokens.Length > 1 && int.TryParse(strTokens[1].Trim(), out tmpVal))
                {
                    string strHertz = strTokens[1].Trim();
                    var switchExpr = strHertz.Length;
                    switch (switchExpr)
                    {
                        case 1:
                            {
                                strHertz += "00";
                                break;
                            }

                        case 2:
                            {
                                strHertz += "0";
                                break;
                            }
                    }

                    return intFrequency + Convert.ToInt32(strHertz);
                }
            }
            catch
            {
                return 0;
            }

            return intFrequency;
        } // KHzToHz

        public static string HzToKHz(int intFrequency)
        {
            try
            {
                string strFrequency = intFrequency.ToString();
                return strFrequency.Insert(strFrequency.Length - 3, ".");
            }
            catch
            {
                return "0.000";
            }
        } // HzToKHz

        public static bool IsValidFrequency(string strFreqKHz, [Optional, DefaultParameterValue(0)] ref int intFreqHz)
        {
            // function to hande checking and conversion of frequencies.
            // sets intFreq to an interger value (Hz) if valid and returns True
            // should handle all local country settings using either "." or "," as decimal indicator.
            string strJustFreq = strFreqKHz.Trim();
            int intPtr = strFreqKHz.ToLower().IndexOf("w"); // WINMOR designator
            if (intPtr != -1)
            {
                strJustFreq = strFreqKHz.Substring(0, intPtr).Trim();
            }
            else
            {
                intPtr = strFreqKHz.ToLower().IndexOf("(p3)"); // Pactor 3 designator
                if (intPtr != -1)
                {
                    strJustFreq = strFreqKHz.Substring(0, intPtr).Trim();
                }
            }

            intPtr = strJustFreq.IndexOf(".");
            int intDecimals;
            try
            {
                if (strJustFreq.IndexOf(".") != -1)
                {
                    intPtr = strJustFreq.IndexOf(".");
                    intDecimals = strJustFreq.Substring(1 + intPtr).Trim().Length;
                    intFreqHz = 1000 * Convert.ToInt32(strJustFreq.Substring(0, intPtr));
                    var switchExpr = intDecimals;
                    switch (switchExpr)
                    {
                        case 1:
                            {
                                intFreqHz += 100 * Convert.ToInt32(strJustFreq.Substring(1 + intPtr));
                                break;
                            }

                        case 2:
                            {
                                intFreqHz += 10 * Convert.ToInt32(strJustFreq.Substring(1 + intPtr).Trim());
                                break;
                            }

                        case 3:
                            {
                                intFreqHz += Convert.ToInt32(strJustFreq.Substring(1 + intPtr).Trim());
                                break;
                            }
                    }

                    return true;
                }
                else if (strJustFreq.IndexOf(",") != -1)
                {
                    intPtr = strJustFreq.IndexOf(",");
                    intDecimals = strJustFreq.Substring(1 + intPtr).Trim().Length;
                    intFreqHz = 1000 * Convert.ToInt32(strJustFreq.Substring(0, intPtr));
                    switch (intDecimals)
                    {
                        case 1:
                            {
                                intFreqHz += 100 * Convert.ToInt32(strJustFreq.Substring(1 + intPtr));
                                break;
                            }

                        case 2:
                            {
                                intFreqHz += 10 * Convert.ToInt32(strJustFreq.Substring(1 + intPtr).Trim());
                                break;
                            }

                        case 3:
                            {
                                intFreqHz += Convert.ToInt32(strJustFreq.Substring(1 + intPtr).Trim());
                                break;
                            }
                    }

                    return true;
                }
                else
                {
                    intFreqHz = Convert.ToInt32(strFreqKHz) * 1000;
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsAutoforwardEnabled()
        {
            if (IsMARSStation() | blnEnablAutoforward)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsMARSStation()
        {
            // 
            // Determine if MARS service code has been entered.
            // 
            return VerifyServiceCode(strMARSServiceCode);
        }

        public static bool VerifyServiceCode(string strTestCode)
        {
            // 
            // Determine if the service code is in the list.
            // 
            var strCodeAry = strServiceCodes.Split(new char[] { ' ' });
            strTestCode = strTestCode.ToUpper();
            // Make the full string
            foreach (var strCode in strCodeAry)
            {
                if ((strTestCode ?? "") == (strCode ?? ""))
                {
                    return true;
                }
            }

            return false;
        }

        public static string MapServiceCode(string strCode)
        {
            // 
            // Do any required channel service code mapping.
            // 
            strCode = strCode.ToUpper();
            return strCode;
        }

        public static string SelectServiceCodes(string strCodes)
        {
            // 
            // Convert a list of service codes into an SQL WHERE clause.
            // 
            string strResult = "";
            if (string.IsNullOrEmpty(strCodes.Trim()))
            {
                strResult = "ServiceCode = 'PUBLIC' OR ServiceCode = 'public' OR GroupReference < 2";
            }
            else
            {
                // Split string based on spaces
                var strCodeAry = strCodes.Split(new char[] { ' ' });
                // Make the full string
                foreach (var strCode in strCodeAry)
                {
                    // Use For Each loop over words and display them
                    if (string.IsNullOrEmpty(strResult))
                    {
                        strResult = "(";
                    }
                    else
                    {
                        strResult = strResult + " OR ";
                    }

                    strResult = strResult + "ServiceCode = '" + strCode + "'";
                }

                strResult = strResult + ")";
            }

            return strResult;
        }

        public static string GroupReferenceToServiceCode(int intGroup)
        {
            // 
            // Convert a GroupReference number to the corresponding service code.
            // 
            string strServiceCode;
            var switchExpr = intGroup; // Group
            switch (switchExpr)
            {
                case 0:
                    {
                        strServiceCode = strHamServiceCode;
                        break;
                    }

                case 1:
                    {
                        strServiceCode = strHamServiceCode;
                        break;
                    }

                case 2:
                    {
                        strServiceCode = strHamServiceCode;
                        break;
                    }

                case 3:
                    {
                        strServiceCode = strMARSServiceCode;
                        break;
                    }

                default:
                    {
                        strServiceCode = "";
                        break;
                    }
            }

            return strServiceCode;
        }

        public static bool AnyUseableFrequency(string strFreqList, string strTNC)
        {
            // 
            // Determine if a station has any channels we can use.
            // 
            var aryFreqEntry = strFreqList.Split(',');
            foreach (string strFreqEntry in aryFreqEntry)
            {
                if (CanUseFrequency(strFreqEntry, strTNC))
                {
                    return true;
                }
            }
            // There are no viable channels.
            return false;
        }

        public static bool CanUseFrequency(string strFreqEntry, string strTNC)
        {
            // 
            // Determine if this channel should be displayed.
            // 
            int intMode;
            var strFreqTokens = strFreqEntry.Split('|');
            if (strFreqTokens.Length < 5)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(strTNC))
            {
                // Check the mode versus TNC type.
                intMode = Convert.ToInt32(strFreqTokens[2]);
                if (intMode >= 21)
                {
                    // We don't support ARDOP or VARA -- TBD
                    return false;
                }

                if (intMode == 16 | intMode == 19)
                {
                    // Require a modem capable of P3
                    if (strTNC.IndexOf("PTC II") == -1 & strTNC.IndexOf("DR-7800") == -1)
                    {
                        return false;
                    }
                }
                else if (intMode == 20)
                {
                    // Require a modem capable of P4.
                    if (strTNC.IndexOf("DR-7800") == -1)
                    {
                        return false;
                    }
                }
            }
            // Check the service code
            if (strFreqTokens.Length >= 5)
            {
                return VerifyServiceCode(strFreqTokens[4]);
            }
            else
            {
                return true;
            }
        }

        public static bool CanUseBaud(string strFreqEntry, string strSpecifiedBaud)
        {
            // 
            // Determine if the baud rate of a VHF channel can be used with the selected TNC.
            // 
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped ElseDirectiveTrivia */
            return true;     // Filtering by baud rate seems to be a bad idea.
            /* TODO ERROR: Skipped EndIfDirectiveTrivia */
        }

        public static string FormatBaud(string strBaudCode)
        {
            // 
            // Convert a baud rate code into a display string.
            // 
            string strBaud;
            switch (strBaudCode)
            {
                case "0":
                    {
                        strBaud = "1200";
                        break;
                    }

                case "1":
                    {
                        strBaud = "1200";
                        break;
                    }

                case "2":
                    {
                        strBaud = "4800";
                        break;
                    }

                case "3":
                    {
                        strBaud = "9600";
                        break;
                    }

                case "4":
                    {
                        strBaud = "9600";
                        break;
                    }

                default:
                    {
                        strBaud = "?";
                        break;
                    }
            }

            return strBaud;
        }

        public static string FormatFrequency(string strFreqEntry)
        {
            // 
            // Convert a service code line to the proper display format.
            // 
            string strFreq;
            string strMode = "";
            var strFreqTokens = strFreqEntry.Split('|');
            if (strFreqTokens.Length < 1)
            {
                return "";
            }

            strFreq = (0.001 * double.Parse(strFreqTokens[0], NumberStyles.AllowDecimalPoint)).ToString("##00.000");
            if (strFreqTokens.Length < 3)
            {
                return strFreq;
            }
            else
            {
                var switchExpr = Convert.ToInt32(strFreqTokens[2]); // Mode
                switch (switchExpr)
                {
                    // Case 0 : strMode = "1200"
                    // Case 1 : strMode = "2400"
                    // Case 2 : strMode = "4800"
                    // Case 3 : strMode = "9600"
                    // Case 4 : strMode = "19200"
                    // Case 5 : strMode = "38400"
                    // Case 6 : strMode = ">38400"

                    case 0:
                        {
                            strMode = "1200";
                            break;
                        }

                    case 1:
                        {
                            strMode = "1200";
                            break;
                        }

                    case 2:
                        {
                            strMode = "4800";
                            break;
                        }

                    case 3:
                        {
                            strMode = "9600";
                            break;
                        }

                    case 4:
                        {
                            strMode = "9600";
                            break;
                        }

                    case 11:
                        {
                            strMode = "P1";
                            break;
                        }

                    case 12:
                        {
                            strMode = "P1,P2";
                            break;
                        }

                    case 13:
                        {
                            strMode = "P1,P2,P3";
                            break;
                        }

                    case 31:
                        {
                            strMode = "P1,P2,P3,P4";
                            break;
                        }

                    case 14:
                        {
                            strMode = "P2";
                            break;
                        }

                    case 15:
                        {
                            strMode = "P2,P3";
                            break;
                        }

                    case 16:
                        {
                            strMode = "P3";
                            break;
                        }

                    case 17:
                        {
                            strMode = "P1,P2,P3,P4";
                            break;
                        }

                    case 18:
                        {
                            strMode = "P2,P3,P4";
                            break;
                        }

                    case 19:
                        {
                            strMode = "P3,P4";
                            break;
                        }

                    case 20:
                        {
                            strMode = "P4";
                            break;
                        }

                    case 21:
                        {
                            strMode = "500";
                            break;
                        }

                    case 22:
                        {
                            strMode = "1600";
                            break;
                        }

                    case 30:
                        {
                            strMode = "RP";
                            break;
                        }

                    default:
                        {
                            strMode = "";
                            break;
                        }
                }

                if (!string.IsNullOrEmpty(strMode))
                {
                    return strFreq + " (" + strMode + ")";
                }
                else
                {
                    return strFreq;
                }
            }
        }

        public static string StripMode(string strFreq)
        {
            // 
            // Remove any mode specification from a frequency.
            // 
            int intIndex = strFreq.IndexOf('(');
            if (intIndex > 0)
            {
                return strFreq.Substring(0, intIndex - 1);
            }
            else
            {
                return strFreq;
            }
        }

        public static bool UseWideFilter(string strFreqEntry)
        {
            // 
            // Determine if we should use a wide or narrow filter based on the channel mode.
            // 
            // If there is no mode, use a wide filter.
            if (strFreqEntry.IndexOf('(') == -1)
            {
                return true;
            }

            if (strFreqEntry.IndexOf("P3") != -1 | strFreqEntry.IndexOf("P4") != -1 | strFreqEntry.IndexOf("w3") != -1 | strFreqEntry.IndexOf("500") != -1 | strFreqEntry.IndexOf("1600") != -1)



            {
                // Use a wide filter
                return true;
            }
            else
            {
                // Use a narrow filter.
                return false;
            }
        }

        public static string ExtractFreq(string strChannel)
        {
            // 
            // Extract the frequency from a full channel description such as 1234.000 (P1,P2,P3)
            // 
            var strFreqArray = strChannel.Trim().Split(' ');
            string strCenterFreq = strFreqArray[0].Trim();
            return strCenterFreq;
        }

        public static bool UseRMSRelay()
        {
            // 
            // Return True if we should connect to RMS Relay rather than connecting directly to a CMS.
            // 
            return blnUseRMSRelay;
        }

        private static List<GatewayStatusRecord> lstGateways = null;
        private static bool blnFinishedGettingChannels = false;

        public static List<GatewayStatusRecord> GetChannelsList(bool blnPacket)
        {
            // 
            // Get a GatewayStatusResponse object with a list of RMS and channels.  Return Nothing if we are unable to obtain the list.
            // 
            // Get the channel list in a thread to avoid blocking status messages from Pactor modems.
            // 
            blnFinishedGettingChannels = false;
            lstGateways = null;
            var thrGetChannels = new Thread(GetChannelsThread);
            thrGetChannels.Priority = ThreadPriority.BelowNormal;
            thrGetChannels.Start(blnPacket);
            // 
            // Wait for the thread to finish.
            // 
            while (blnFinishedGettingChannels == false)
            {
                Thread.Sleep(100);
                UserInterfaceFactory.GetUiSystem().Yield();
            }
            // 
            // Finished
            // 
            return lstGateways;
        }

        private static void GetChannelsThread(object objArgument)
        {
            // 
            // This routine runs as a thread to get the list of channels.
            // When it finishes, it sets blnFinishedGettingChannels True and returns the list in lstGateways.
            // 
            bool blnPacketChannels = Convert.ToBoolean(objArgument);
            // 
            // Try to get the full list of channels matching our service code(s).
            // 
            var lstChannels = GatewayChannelList(blnPacketChannels, strServiceCodes, "Paclink:" + SiteCallsign);
            if (lstChannels != null)
            {
                // 
                // We got a gateway status response.  Select the channels we want.
                // 
                lstGateways = new List<GatewayStatusRecord>();
                GatewayStatusRecord objNewStation;
                bool blnUseChannel;
                foreach (GatewayStatusRecord ObjStation in lstChannels)
                {
                    if (ObjStation.HoursSinceStatus < 5 * 24)
                    {
                        // Create a new GatewayStatusRecord
                        objNewStation = new GatewayStatusRecord();
                        objNewStation.BaseCallsign = ObjStation.BaseCallsign;
                        objNewStation.Callsign = ObjStation.Callsign;
                        objNewStation.Comments = ObjStation.Comments;
                        objNewStation.HoursSinceStatus = ObjStation.HoursSinceStatus;
                        objNewStation.LastStatus = ObjStation.LastStatus;
                        objNewStation.Latitude = ObjStation.Latitude;
                        objNewStation.Longitude = ObjStation.Longitude;
                        objNewStation.Timestamp = ObjStation.Timestamp;
                        objNewStation.GatewayChannels = new List<GatewayChannelRecord>();
                        // Check each channel for this station.
                        foreach (GatewayChannelRecord objChan in ObjStation.GatewayChannels)
                        {
                            // Determine if we should include this channel in the list.
                            blnUseChannel = false;
                            if (blnPacketChannels)
                            {
                                // Select packet channels
                                if (objChan.Mode < 10 & objChan.Frequency > 50000000 & (objChan.Baud == "1200" | objChan.Baud == "9600"))
                                {
                                    blnUseChannel = true;
                                }
                            }
                            // Select HF channels
                            else if (objChan.Mode >= 10 & objChan.Frequency > 1800000)
                            {
                                blnUseChannel = true;
                            }

                            if (blnUseChannel)
                            {
                                // Use this channel
                                objNewStation.GatewayChannels.Add(objChan);
                            }
                        }
                        // If gateway has any channels, add it to the list we will return
                        if (objNewStation.GatewayChannels.Count > 0)
                        {
                            lstGateways.Add(objNewStation);
                        }
                    }
                }
            }
            // 
            // Finished
            // 
            blnFinishedGettingChannels = true;
            return;
        }

        private static List<GatewayStatusRecord> GatewayChannelList(bool blnPacketChannels, string strServiceCodes, string strCaller, string strForceCMSArg = "")
        {
            // 
            // Get a list of the available gateway channels, and return it as a List of GatewayStatusRecord.
            // Return Nothing if we can't get the list.

            List<GatewayStatusRecord> objResponse = null;
            try
            {
                objResponse = WinlinkWebServices.GetGatewayStatusRecords(GatewayOperatingMode.AnyAll, 168, strServiceCodes);
            }
            catch (Exception ex)
            {
                _log.Warn(ex, ex.Message);
                return null;
            }

            // 
            // Select either HF or VHF/UHF channels.
            // 
            var lstGateways = new List<GatewayStatusRecord>();
            GatewayStatusRecord objNewStation;
            bool blnUseChannel;
            foreach (GatewayStatusRecord objStation in objResponse)
            {
                if (objStation.HoursSinceStatus < 5 * 24)
                {
                    // Create a new GatewayStatusRecord
                    objNewStation = new GatewayStatusRecord();
                    objNewStation.BaseCallsign = objStation.BaseCallsign.Trim().ToUpper();
                    objNewStation.Callsign = objStation.Callsign.Trim().ToUpper();
                    objNewStation.Comments = objStation.Comments;
                    objNewStation.HoursSinceStatus = objStation.HoursSinceStatus;
                    objNewStation.LastStatus = objStation.LastStatus;
                    objNewStation.Latitude = objStation.Latitude;
                    objNewStation.Longitude = objStation.Longitude;
                    objNewStation.Timestamp = objStation.Timestamp;
                    objNewStation.GatewayChannels = new List<GatewayChannelRecord>();
                    // Check each channel for this station.
                    foreach (GatewayChannelRecord objChan in objStation.GatewayChannels)
                    {
                        // Determine if we should include this channel in the list.
                        blnUseChannel = false;
                        if (blnPacketChannels)
                        {
                            // Select packet channels
                            if ((objChan.Mode < 10 | objChan.Mode == 51 | objChan.Mode == 52) & objChan.Frequency > 50000000 & (objChan.Baud == "1200" | objChan.Baud == "9600"))
                            {
                                blnUseChannel = true;
                            }
                        }
                        // Select HF channels
                        else if (objChan.Mode >= 10 & objChan.Mode != 51 & objChan.Mode != 52 & objChan.Frequency > 1800000)
                        {
                            blnUseChannel = true;
                        }

                        if (blnUseChannel)
                        {
                            // Use this channel
                            objNewStation.GatewayChannels.Add(objChan);
                        }
                    }
                    // If gateway has any channels, add it to the list we will return
                    if (objNewStation.GatewayChannels.Count > 0)
                    {
                        lstGateways.Add(objNewStation);
                    }
                }
            }
            // 
            // Finished.  Return the channel information.
            // 
            return lstGateways;
        }

        public static string CleanSerialPort(string strPortName)
        {
            // 
            // Remove any trailing characters after digits in a serial port name.
            // 
            int intIndex;
            intIndex = strPortName.Length - 1;
            while (intIndex > 0)
            {
                if (char.IsDigit(strPortName[intIndex]))
                    break;
                intIndex -= 1;
            }

            if (intIndex > 0 & intIndex + 1 < strPortName.Length)
            {
                strPortName = strPortName.Substring(0, intIndex + 1);
            }

            return strPortName;
        }

        public static string FormatNetTime()
        {
            // 
            // Return the current UTC time in the format yyyymmddhhmmss
            // 
            return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }

        public static DateTime ParseNetworkDate(string strDate, [Optional, DateTimeConstant(627667488000000000/* #1/1/1990# */)] DateTime dttDefaultDate, string strComment = "")
        {
            // 
            // Parse a network date in the format yyyyMMddHHmmss.
            // 
            DateTime dttDate;
            if (strDate == null || strDate.Length != 14)
                return dttDefaultDate;
            var provider = CultureInfo.InvariantCulture;
            try
            {
                dttDate = DateTime.ParseExact(strDate, "yyyyMMddHHmmss", provider);
            }
            catch
            {
                if (!string.IsNullOrEmpty(strComment))
                {
                }
                // Exceptions("[ParseNetworkDate] ( " & strComment & ") Error parsing " & strDate & ", " & Err.Description)
                else
                {
                    // Exceptions("[ParseNetworkDate] Error parsing " & strDate & ", " & Err.Description)
                }

                if (dttDefaultDate == DateTime.Parse("1990-01-01"))
                {
                    dttDate = DateTime.UtcNow;
                }
                else
                {
                    dttDate = dttDefaultDate;
                }
            }

            return dttDate;
        }

        public static string GetWindowsVersion()
        {
            // 
            // Return a string with Windows version information.
            // 
            string strVersion = "";
            try
            {
                strVersion = Environment.OSVersion.Version.ToString();
            }
            catch (Exception ex)
            {
                strVersion = "(Error)";
            }

            return strVersion;
        }

        public static int Asc(char c)
        {
            int converted = c;
            if (converted >= 0x80)
            {
                byte[] buffer = new byte[2];
                // if the resulting conversion is 1 byte in length, just use the value
                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                {
                    converted = buffer[0];
                }
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }
            return converted;
        }

        public static string GetMailSystemPassword(string secureLoginPassword)
        {
            return Globals.Settings.Get("Properties", "EMail Password", SecureLoginPassword);
        }

        public static void SaveMailSystemPassword(string mailSystemPassword)
        {
            Globals.Settings.Save("Properties", "EMail Password", mailSystemPassword);
        }
    } 
}