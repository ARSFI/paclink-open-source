using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DnsClient;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using WinlinkServiceClasses;

namespace WinlinkInterop
{
    public class WinlinkInterop
    {
        // 
        // Common interface to Winlink servers.
        // 
        // Data members.
        // 
        private bool blnUseProxy = false;          // Connect to a proxy server
        private string strProxyIP = "127.0.0.1";      // IP address of proxy server
        private int intProxyPort = 21;            // Proxy server port number
        private int intServicesPort = 8085;       // TCP/IP port on CMS to connect to for Winlink services
        private int intStatusReportIndex = 0;     // Index to last StatusReport server we used.
        private double dblMonitorPeriod = 240;        // Number of seconds between Internet monitor scans
        private int intConnectionTimeoutSeconds = 30; // Number of seconds to wait before declaring a connection timeout
        private int intSatPhoneTimeout = 120;      // Timeout in seconds when connecting through a sat phone
        private int intSystemServiceCount = 0;    // Number of system services since starting
        private Thread thrMonitorInternet = null;  // Handle for Internet monitor thread
        private string strPrimaryCallsign = "";              // Callsign of site
        private string strLastError = "";             // Last recorded error
        private string strLastFailedInternetSite = "";  // Last public web site that failed
        private string strDNSServer = "";            // Address of DNS server to use for name translations
        private string strForceCMS = "";             // Force use of a specific CMS
        private DateTime dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);    // Time when we determined Internet is down
        private DateTime dttInternetAvailable = DateTime.UtcNow.AddDays(-1);    // Time when we determined Internet is up
        private object objLock = new object();                   // Used for SyncLock
        private int[] aryNetParamCMS = new int[2];            // CMS index numbers sorted by net parameter order
        private int intNumNetParamCMS = 0;
        private bool blnEnableInternet = true;     // Set False to simulate Internet outage
        private bool blnSatPhoneMode = false;
        private string strAccountAccessCode = "C6B607C4AB604A679E396A01E1CA1E98";
        private string strMessageAccessCode = "6F34B1991BDB4DFDADE94A8EEBA0B5D8";
        private string strPasswordAccessCode = "35863CDB1C9E49A087C31CCAE1BBF350";
        private string strMPSAccessCode = "4EBD510A782E4E179197B4D5C60C282D";
        private string strWebServiceKey = "";
        private Thread thrConnectCMS = null;
        public string strLastWebServiceUrl = "";
        private string strForceBadIP = "";            // If non-blank, city name of CMS to set to an invalid IP address for testing.
        private object strForceServiceOffline = "";             // Force a CMS to assume a required service is not running
        private object blnDisableWebServices = false;           // Set to True to disable web services
        private Dictionary<string, bool> dicAccountCache = new Dictionary<string, bool>();
        private Dictionary<string, PasswordCacheEntry> dicPasswordCache = new Dictionary<string, PasswordCacheEntry>();
        private Dictionary<string, NetworkParamCacheEntry> dicNetworkParamCache = new Dictionary<string, NetworkParamCacheEntry>();
        private int intNetworkParamCacheExpire = 12;       // Expiration period in hours network parameter cache entries
        private DateTime dttLastNetworkParamCacheExpire = DateTime.UtcNow;
        private object objNetworkParamCacheLock = new object();
        private object objAccountCacheLock = new object();
        private int intNumCMSConnections = 0;
        private double dblTotalCMSConnectionTime = 0;
        private double dblMaxSuccessfulConnectTime = 0;
        private double dblMaxAPIConnectTime = 0;
        private int intNumFailedCMSConnections = 0;
        private double dblTotalFailedCMSTime = 0;
        private int intErrorCount = 0;
        private string strFavorApiServer = "";
        private JsonEnum objRequestedModeEnum;
        private JsonEnum objModeEnum;
        private JsonEnum objUserTypeEnum;
        private string strCurrentApiServer = "";
        private StringBuilder sbNotes = new StringBuilder();
        private bool blnEnableNotes = false;
        private DateTime dttLastPingTime = DateTime.MinValue;
        private bool blnLastPingSuccess = false;
        private string[] strLocalIPAddresses = null;
        private string strLogsDirectory = "";
        private object objLogLock = new object();

        // 
        // The smoothed SSN by month from 2012 to 2020 for VOACAP propration predictions.  Each line represents a year values, Jan - Dec.
        // The first line in the table is for 2012.
        // URL for obtaining SSN: ftp://ftp.ngdc.noaa.gov/STP/space-weather/solar-data/solar-indices/sunspot-numbers/predicted/table_international-sunspot-numbers_monthly-predicted.txt
        // Last updated: Oct. 23, 2015
        // 
        private static double[] arySSN = new double[] { 65.5, 66.9, 66.8, 64.6, 61.7, 58.9, 57.8, 58.2, 58.1, 58.6, 59.7, 59.6, 58.7, 58.4, 57.6, 57.9, 59.9, 62.6, 65.5, 68.9, 73.0, 74.9, 75.3, 75.9, 77.3, 78.4, 80.8, 81.9, 80.5, 79.7, 78.6, 75.6, 70.8, 67.3, 65.4, 63.7, 61.9, 60.5, 59.7, 59.1, 57.8, 55.5, 53.1, 51.2, 49.7, 48.6, 47.4, 46.1, 44.8, 43.2, 41.4, 39.4, 37.5, 36.6, 35.9, 35.4, 34.7, 33.5, 31.8, 30.1, 29.2, 28.6, 27.9, 27.3, 26.7, 25.8, 24.5, 23.0, 21.8, 20.9, 20.2, 19.6, 18.8, 17.7, 16.7, 15.5, 14.5, 13.7, 13.4, 13.1, 12.5, 11.8, 11.1, 10.7, 10.1, 9.6, 9.3, 9.0, 8.9, 8.9, 8.9, 8.9, 9.2, 9.5, 9.8, 10.2, 10.5, 10.8, 11.3, 11.7, 12.3, 13.2, 13.9, 14.5, 15.2, 16.2, 17.6, 19.0 };
        private static byte[] salt = new byte[] { 77, 197, 101, 206, 190, 249, 93, 200, 51, 243, 93, 237, 71, 94, 239, 138, 68, 108, 70, 185, 225, 137, 217, 16, 51, 122, 193, 48, 194, 195, 198, 175, 172, 169, 70, 84, 61, 62, 104, 186, 114, 52, 61, 168, 66, 129, 192, 208, 187, 249, 232, 193, 41, 113, 41, 45, 240, 16, 29, 228, 208, 228, 61, 20 };

        private class ConnectCMSArgs
        {
            // Used to pass arguments to CMS connection thread.
            public bool blnSkip;
            public string strCaller;
            public int intPort;
            public int intTimeout;
            public string strForceCMSArg;
            public Enumerations.CMSconnection objCMSConnection;
            public bool blnConnectionPending = false;
        }

        private class PasswordCacheEntry
        {
            // Used to cache information about passwords for callsigns.
            public string strCallsign;
            public string strPassword;
            public DateTime dttEntryTime;
            public bool blnPasswordCorrect;
        }

        private class NetworkParamCacheEntry
        {
            // Used to catch duplicate parameter reports.
            public string strCallsign;
            public string strParam;
            public string strValue;
            public DateTime dttPosted;

            public NetworkParamCacheEntry()
            {
                strCallsign = "";
                strParam = "";
                strValue = "";
                dttPosted = DateTime.MinValue;
            }
        }

        private class MonitorConnectionParam
        {
            public TcpClient objIPPort;
            public DateTime dttTimeout;
            public bool blnConnecting = true;
            public bool blnTimeout = false;
        }

        public WinlinkInterop(string strSiteCallsign)
        {
            // 
            // WinlinkInterop constructor.
            // 
            strPrimaryCallsign = strSiteCallsign;
            dttInternetAvailable = DateTime.UtcNow.AddDays(-1);
            dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);
            // 
            // Set up Enum mapping for Json responses.
            // 
            objRequestedModeEnum = new JsonEnum("RequestedMode");
            objRequestedModeEnum.Add("AnyAll", Conversions.ToString(GatewayOperatingMode.AnyAll));
            objRequestedModeEnum.Add("Packet", Conversions.ToString(GatewayOperatingMode.Packet));
            objRequestedModeEnum.Add("Pactor", Conversions.ToString(GatewayOperatingMode.Pactor));
            objRequestedModeEnum.Add("Winmor", Conversions.ToString(GatewayOperatingMode.Winmor));
            objRequestedModeEnum.Add("RobustPacket", Conversions.ToString(GatewayOperatingMode.RobustPacket));
            objRequestedModeEnum.Add("AllHf", Conversions.ToString(GatewayOperatingMode.AllHf));
            objModeEnum = new JsonEnum("Mode");
            objModeEnum.Add("AnyAll", Conversions.ToString(GatewayOperatingMode.AnyAll));
            objModeEnum.Add("Packet", Conversions.ToString(GatewayOperatingMode.Packet));
            objModeEnum.Add("Pactor", Conversions.ToString(GatewayOperatingMode.Pactor));
            objModeEnum.Add("Winmor", Conversions.ToString(GatewayOperatingMode.Winmor));
            objModeEnum.Add("RobustPacket", Conversions.ToString(GatewayOperatingMode.RobustPacket));
            objModeEnum.Add("AllHf", Conversions.ToString(GatewayOperatingMode.AllHf));
            objUserTypeEnum = new JsonEnum("UserType");
            objUserTypeEnum.Add("AnyAll", Conversions.ToString(UserType.AnyAll));
            objUserTypeEnum.Add("Client", Conversions.ToString(UserType.Client));
            objUserTypeEnum.Add("Sysop", Conversions.ToString(UserType.Sysop));
        }

        ~WinlinkInterop()
        {
            // 
            // Destructor
            // 
            Close();
        }

        public void Close()
        {
            // 
            // Close the WinlinkInterop object.
            // 
            StopMonitoring();
            return;
        }

        public void phs(string strText = "")
        {
            return;
        }

        public void SetCallsign(string strSiteCallsign)
        {
            // 
            // Set the callsign of the site using this object.
            // 
            strPrimaryCallsign = Globals.BaseCallsign(strSiteCallsign);
        }

        public void SetWebServiceKey(string strkey)
        {
            strWebServiceKey = strkey;
            return;
        }

        public void SetMPSAccessCode(string strAccessCode)
        {
            // 
            // Set the web access code needed to perform CMS functions.
            // 
            strMPSAccessCode = strAccessCode;
            return;
        }

        public void SetAccountAccessCode(string strAccessCode)
        {
            // 
            // Set the web access code needed to perform CMS functions.
            // 
            strAccountAccessCode = strAccessCode;
            return;
        }

        public void SetPasswordAccessCode(string strAccessCode)
        {
            // 
            // Set the web access code needed to perform CMS functions.
            // 
            strPasswordAccessCode = strAccessCode;
            return;
        }

        public void SetMessageAccessCode(string strAccessCode)
        {
            // 
            // Set the web access code needed to perform CMS functions.
            // 
            strMessageAccessCode = strAccessCode;
            return;
        }

        public void SetTimeoutSeconds(int intTimeoutSeconds)
        {
            // 
            // Set the number of seconds to wait before declaring a connection timeout.
            // 
            intConnectionTimeoutSeconds = intTimeoutSeconds;
            return;
        }

        public void SetDNSServer(string strDNS)
        {
            // 
            // Set a specific DNS server for name translation.
            // 
            strDNSServer = strDNS;
            return;
        }

        public void ForceCMS(string strCMS)
        {
            strForceCMS = strCMS;
            return;
        }

        public void SetApiServer(string strServer)
        {
            // 
            // Specify a CMS that is to be favored for API services.
            // 
            strFavorApiServer = strServer;
            if (!string.IsNullOrEmpty(strServer))
                strCurrentApiServer = strServer;
            return;
        }

        public string GetApiServer()
        {
            // 
            // Get the name of the CMS being used as the API server.
            // 
            return strCurrentApiServer;
        }

        public void SetEnableInternet(bool blnEnable)
        {
            // 
            // Enable/Disable Internet access.
            // 
            blnEnableInternet = blnEnable;
            return;
        }

        public void StartMonitoring(double dblPeriod = 240)
        {
            // 
            // Start a background thread to monitor Internet availability.
            // 
            lock (objLock)
            {
                if (dblPeriod < 30)
                    dblPeriod = 30;
                dblMonitorPeriod = dblPeriod;
                if (thrMonitorInternet == null)
                {
                    thrMonitorInternet = new Thread(MonitorInternetThread);
                    thrMonitorInternet.Start();
                }
            }

            return;
        }

        public void StopMonitoring()
        {
            // 
            // Stop the background thread that monitors the Internet.
            // 
            lock (objLock)
            {
                if (!Information.IsNothing(thrMonitorInternet) && thrMonitorInternet.IsAlive)
                    thrMonitorInternet.Abort();
                thrMonitorInternet = null;
            }

            return;
        }

        public void EnableNotes(bool blnEnable = true)
        {
            // 
            // Enable/Disable storing notes
            blnEnableNotes = blnEnable;
            return;
        }

        public string GetNotes(bool blnClear = true)
        {
            // 
            // Fetch any pending notes.
            // 
            string strNotes = sbNotes.ToString();
            if (blnClear)
            {
                sbNotes = new StringBuilder();
            }

            return strNotes;
        }

        private void AddNote(string strNote)
        {
            // 
            // Add a new note line to the list of pending notes.
            // 
            if (blnEnableNotes)
            {
                sbNotes.AppendLine(Strings.Format(DateTime.UtcNow, "yyyy-MM-dd hh:mm:ss") + " " + strNote);
            }

            return;
        }

        public void Debug(string strValue = "")
        {
            strLastError = strValue;
            return;
        }

        public string LastError()
        {
            // 
            // Get the last recorded error.
            // Clear the error after returning it.
            // 
            string strError = strLastError;
            strLastError = "";
            return strError;
        }

        public string LastFailedInternetSite()
        {
            // 
            // Get the domain name of the last public web site that failed the Internet test.
            // 
            string strError = new string(strLastFailedInternetSite.ToCharArray());
            return strError;
        }

        public void SetCMSConnectFail(string strCityVal)
        {
            // 
            // Force a CMS with a specified city name to have connection failures.
            // 
            strForceBadIP = strCityVal.Trim().ToUpper();
            return;
        }

        public void SetCMSServiceOffline(string strCityVal)
        {
            // 
            // Set a CMS to simulate a service being unavailable.
            // 
            strForceServiceOffline = strCityVal.Trim().ToUpper();
            return;
        }

        public void DisableWebServices(bool blnDisable)
        {
            // 
            // Disable web services
            // 
            blnDisableWebServices = blnDisable;
            return;
        }

        public string[] GetLocalIPAddresses()
        {
            // 
            // Function to get an array with the IP addresses of the local computer.
            // 
            // If we've already found the local IP addresses, just return them.
            // 
            if (strLocalIPAddresses is object)
            {
                return strLocalIPAddresses;
            }
            // 
            // Set up the list of local IP addresses.
            // 
            try
            {
                // Count how many IP addresses we have.  Don't count blank addresses.
                var tempIPList = new List<string>();
                foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    var ipProperties = adapter.GetIPProperties();
                    foreach (var ipAddress in ipProperties.UnicastAddresses)
                    {
                        tempIPList.Add(ipAddress.Address.ToString());
                    }
                }
                strLocalIPAddresses = tempIPList.ToArray();
            }
            catch
            {
                Debug(Information.Err().Description);
                strLocalIPAddresses = null;
                return null;
            }

            return strLocalIPAddresses;
        }  // InitializeLocalIPAddresses

        public double GetSSN(double dblCurrentSFI = 0.0, double dblCurrentSFIProportion = 0.0)
        {
            // 
            // Compute a weighted average of the actual, current SFI and the predicted SSN from the published table.
            // 
            double dblSunSpotNumber;
            // 
            // Get current predicted SSN from table.
            // 
            double dblTableSunSpotNumber = GetPredictedSSN(DateTime.UtcNow);
            // 
            // See if we need to compute a weighted average with actual SFI.
            // 
            if (Conversions.ToBoolean(Operators.AndObject(Operators.ConditionalCompareObjectGreater(dblCurrentSFI, 0.0, false), Operators.ConditionalCompareObjectGreater(dblCurrentSFIProportion, 0.0, false))))
            {
                // Compute weighted average.  Convert current SFI to SSN.
                double dblSSNcurrent = SFItoSSN(Conversions.ToDouble(dblCurrentSFI));
                // Compute weighted average: 80% for table, smoothed value, 20% for current value.
                dblSunSpotNumber = Conversions.ToDouble(Operators.AddObject(Operators.MultiplyObject(dblCurrentSFIProportion, dblSSNcurrent), Operators.MultiplyObject(Operators.SubtractObject(1.0, dblCurrentSFIProportion), dblTableSunSpotNumber)));
            }
            else
            {
                // Use only the predicted SSN from the table.
                dblSunSpotNumber = dblTableSunSpotNumber;
            }
            // 
            // Finished
            // 
            return dblSunSpotNumber;
        }

        public double GetPredictedSSN([Optional, DateTimeConstant(599266080000000000/* #1/1/1900# */)] DateTime dttUtcDate)
        {
            // 
            // Get the predicted smoothed sunspot number for a specified date.
            // If the date is omitted, the current date is used.
            // 
            if (dttUtcDate == DateTime.Parse("1900-01-01"))
                dttUtcDate = DateTime.UtcNow;
            if (dttUtcDate < DateTime.Parse("2012-01-01") | dttUtcDate > DateTime.Parse("2020-12-31"))
            {
                return 46.0;
            }

            double dblTableSunSpotNumber;
            int intYearIndex = dttUtcDate.Year - 2012;
            if (intYearIndex >= 0 & (intYearIndex + 1) * 12 < arySSN.GetUpperBound(0))
            {
                dblTableSunSpotNumber = arySSN[12 * Math.Max(0, dttUtcDate.Year - 2012) + dttUtcDate.Month - 1];
            }
            else
            {
                // Invalid year
                dblTableSunSpotNumber = 46.0;
            }

            return dblTableSunSpotNumber;
        }

        public AccountGatewayAccessGetResponse CheckGatewayAccess(string strCallsign, bool blnSkipCMS = false)
        {
            // 
            // Determine if an account is authorized for gateway access.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return null;
            }
            // 
            // Check if callsign has gateway access privilege.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", Globals.BaseCallsign(strCallsign));
            string strResponse = JsonCommand("/account/gatewayAccess/get?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<AccountGatewayAccessGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            else
            {
                return objResponse;
            }
        }

        public Enumerations.AccountValidationCodes ValidateWinlinkLogon(string strSystemCallsign, string strUserCallsign, string strUserPassword)
        {
            // 
            // Check if a specified callsign and password are correct for a Winlink user.
            // 
            Enumerations.AccountValidationCodes enmResult;
            string strBaseCallsign;
            if (Globals.IsValidTacticalAddress(strUserCallsign))
            {
                strBaseCallsign = strUserCallsign;
            }
            else
            {
                strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
                if (!IsValidCallsign(strUserCallsign) | Globals.IsValidTacticalAddress(strUserCallsign, 12))
                {
                    return Enumerations.AccountValidationCodes.NotRegistered;
                }
            }
            // 
            // See if this is a registered account.
            // 
            enmResult = AccountExists(strSystemCallsign, strBaseCallsign);
            if (enmResult == Enumerations.AccountValidationCodes.NoInternet)
            {
                // No internet. Say account is valid.
                return Enumerations.AccountValidationCodes.Valid;
            }

            if (enmResult != Enumerations.AccountValidationCodes.Valid & enmResult != Enumerations.AccountValidationCodes.SecureLogon)
            {
                // Not registered or locked out.  See if it's locked out.
                var enmLockResult = AccountLockedOut(strSystemCallsign, strBaseCallsign, false);
                if (enmLockResult == Enumerations.AccountValidationCodes.LockedOut)
                    return enmLockResult;
                // Not locked out.
                return enmResult;
            }
            // 
            // The account exists.  See if it is locked out.
            // 
            enmResult = AccountLockedOut(strSystemCallsign, strBaseCallsign, false);
            if (enmResult != Enumerations.AccountValidationCodes.Valid)
            {
                return enmResult;
            }
            // 
            // Validate the password.
            // 
            enmResult = AccountPasswordValidate(strBaseCallsign, strUserPassword);
            if (enmResult == Enumerations.AccountValidationCodes.NoInternet)
            {
                // Punt if we can't access the Internet
                enmResult = Enumerations.AccountValidationCodes.Valid;
            }

            return enmResult;
        }

        public bool AccountRegistered(object strUserCallsign)
        {
            // 
            // Check to see if the specified account is registered with the Winlink system. Return True if it is.
            // 
            if (!(IsValidCallsign(Conversions.ToString(strUserCallsign)) | Globals.IsValidTacticalAddress(Conversions.ToString(strUserCallsign), 12)))
                return false;
            var enmValid = AccountExists(Conversions.ToString(strUserCallsign), Conversions.ToString(strUserCallsign));
            if (enmValid == Enumerations.AccountValidationCodes.NoInternet | enmValid == Enumerations.AccountValidationCodes.SecureLogon | enmValid == Enumerations.AccountValidationCodes.Valid)
            {
                return true;
            }

            return false;
        }

        public Enumerations.AccountValidationCodes AccountExists(string strSystemCallsign, string strUserCallsign)
        {
            // 
            // Check if a callsign is a registered Winlink user.
            // 
            if (!(IsValidCallsign(strUserCallsign) | Globals.IsValidTacticalAddress(strUserCallsign, 12)))
            {
                return Enumerations.AccountValidationCodes.NotRegistered;
            }
            // 
            // Check the cache and see if we know the account exists.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
            if (SearchAccountCache(strBaseCallsign))
            {
                return Enumerations.AccountValidationCodes.Valid;
            }
            // 
            // See if the account exists.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            string strResponse = JsonCommand("/account/exists?format=json", lstParam);
            if (strResponse == null)
            {
                return Enumerations.AccountValidationCodes.NoInternet;
            }

            var objResponse = JsonSerializer<AccountExistsResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return Enumerations.AccountValidationCodes.NoInternet;
            }
            // Check returned response
            if (objResponse.CallsignExists)
            {
                SetAccountCache(strBaseCallsign, true);
                return Enumerations.AccountValidationCodes.Valid;
            }
            else
            {
                SetAccountCache(strBaseCallsign, false);
                return Enumerations.AccountValidationCodes.NotRegistered;
            }
        }

        public bool BlockedCallsign(string strCallsign)
        {
            // 
            // Return True if the specified callsign is blocked (locked out).
            // 
            if (AccountLockedOut(strCallsign, strCallsign) == Enumerations.AccountValidationCodes.LockedOut)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Enumerations.AccountValidationCodes AccountLockedOut(string strSystemCallsign, string strUserCallsign, bool blnCheckRegistered = true)
        {
            // 
            // Check if a specified callsign is locked out.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
            if (!IsValidCallsign(strBaseCallsign))
            {
                return Enumerations.AccountValidationCodes.NotRegistered;
            }

            if (blnCheckRegistered && !AccountRegistered(strBaseCallsign))
            {
                return Enumerations.AccountValidationCodes.NotRegistered;
            }
            // 
            // See if the account is locked out.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            string strResponse = JsonCommand("/account/lockedOut/get?format=json", lstParam);
            if (strResponse == null)
            {
                return Enumerations.AccountValidationCodes.NoInternet;
            }

            var objResponse = JsonSerializer<AccountLockedOutGetResponse>.DeSerialize(strResponse);
            // If objResponse IsNot Nothing AndAlso objResponse.ErrorMessage = "AccountDoesNotExist" Then
            // Return AccountValidationCodes.NotRegistered
            // End If
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return Enumerations.AccountValidationCodes.NoInternet;
            }

            if (objResponse.LockedOut)
            {
                return Enumerations.AccountValidationCodes.LockedOut;
            }
            else
            {
                return Enumerations.AccountValidationCodes.Valid;
            }
        }

        public Enumerations.AccountValidationCodes AccountSecureLogon(string strSystemCallsign, string strUserCallsign)
        {
            // 
            // Check if a specified callsign requires secure logon.
            // 
            return Enumerations.AccountValidationCodes.SecureLogon;
        }

        public bool AccountHasPassword(string strUserCallsign)
        {
            // 
            // Return True if the specified account has a password on the CMS.
            // 
            return true;     // All accounts are required to have passwords
        }

        public bool ValidatePassword(string strCallsign, string strPassword, string strWebServiceUrl = "")
        {
            // 
            // Return True if the specified password is correct for a callsign or False if it isn't.
            // 
            var enmValidation = AccountPasswordValidate(strCallsign, strPassword);
            if (enmValidation == Enumerations.AccountValidationCodes.SecureLogon | enmValidation == Enumerations.AccountValidationCodes.Valid | enmValidation == Enumerations.AccountValidationCodes.NoInternet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Enumerations.AccountValidationCodes AccountPasswordValidate(string strUserCallsign, string strUserPassword, string strWebServiceUrl = "", string strForceCMS = "")
        {
            // 
            // Check if the specified password is correct for a callsign.
            // 
            string strBaseCallsign;
            if (Globals.IsValidTacticalAddress(strUserCallsign))
            {
                strBaseCallsign = strUserCallsign;
            }
            else
            {
                strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
                if (!IsValidCallsign(strBaseCallsign))
                {
                    return Enumerations.AccountValidationCodes.NotRegistered;
                }
            }

            if (!AccountRegistered(strBaseCallsign))
            {
                return Enumerations.AccountValidationCodes.NotRegistered;
            }

            strUserPassword = strUserPassword.Trim();
            // 
            // Check the syntax of the password.
            // 
            string strError = Globals.CheckPasswordSyntax(strUserPassword);
            if (!string.IsNullOrEmpty(strError))
            {
                return Enumerations.AccountValidationCodes.BadPassword;
            }
            // 
            // See if we have a cache entry for this callsign.
            // 
            var objPassword = SearchPasswordCache(strBaseCallsign);
            if (objPassword is object)
            {
                // We have a chacke entry for this callsign. See if we've seen this password.
                if (string.Compare(objPassword.strPassword, strUserPassword, false) == 0)
                {
                    // We have an entry matching the callsign and password.
                    if (objPassword.blnPasswordCorrect)
                    {
                        return Enumerations.AccountValidationCodes.Valid;
                    }
                    else
                    {
                        return Enumerations.AccountValidationCodes.BadPassword;
                    }
                }
            }
            // 
            // Verify that the account exists.
            // 
            var enmAccount = AccountExists(strUserCallsign, strUserCallsign);
            if (enmAccount != Enumerations.AccountValidationCodes.Valid & enmAccount != Enumerations.AccountValidationCodes.SecureLogon & enmAccount != Enumerations.AccountValidationCodes.NoInternet)
            {
                // Account is not registered.
                return Enumerations.AccountValidationCodes.BadPassword;
            }
            // 
            // Find a CMS to send the request to.
            // 
            if (string.IsNullOrEmpty(strWebServiceUrl))
            {
                strWebServiceUrl = GetWebServiceURL(false, strBaseCallsign, true, strForceCMS);
                if (string.IsNullOrEmpty(strWebServiceUrl))
                {
                    // Can't access server.
                    return Enumerations.AccountValidationCodes.NoInternet;
                }
            }
            // 
            // Validate the password.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strUserPassword);
            lstParam.Add("WebServiceAccesscode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/password/validate?format=json", lstParam);
            if (strResponse == null)
            {
                return Enumerations.AccountValidationCodes.BadPassword;
            }

            var objResponse = JsonSerializer<AccountPasswordValidateResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return Enumerations.AccountValidationCodes.BadPassword;
            }
            // Update the password cache with the results.
            SetPasswordCache(strBaseCallsign, strUserPassword, objResponse.IsValid);
            if (objResponse.IsValid)
            {
                return Enumerations.AccountValidationCodes.Valid;
            }
            else
            {
                return Enumerations.AccountValidationCodes.BadPassword;
            }
        }

        private bool SearchAccountCache(object strCallsign)
        {
            // 
            // Return True if the account is known to be valid.
            // 
            lock (objAccountCacheLock)
            {
                if (dicAccountCache.ContainsKey(Conversions.ToString(strCallsign)))
                {
                    return dicAccountCache[Conversions.ToString(strCallsign)];
                }
                else
                {
                    return false;
                }
            }
        }

        private void SetAccountCache(string strCallsign, bool blnExists = true)
        {
            // 
            // Set whether an account exists.
            // 
            lock (objAccountCacheLock)
            {
                if (dicAccountCache.ContainsKey(strCallsign))
                {
                    dicAccountCache[strCallsign] = blnExists;
                }
                else
                {
                    dicAccountCache.Add(strCallsign, blnExists);
                }
            }

            return;
        }

        private PasswordCacheEntry SearchPasswordCache(object strCallsign)
        {
            // 
            // Try to find an unexpired password cache entry for a callsign.
            // 
            lock (objLock)
            {
                PasswordCacheEntry objPassword = null;
                if (dicPasswordCache.ContainsKey(Conversions.ToString(strCallsign)))
                {
                    objPassword = dicPasswordCache[Conversions.ToString(strCallsign)];
                    if ((DateTime.UtcNow - objPassword.dttEntryTime).TotalHours > 6)
                    {
                        // Expired entry
                        objPassword = null;
                    }
                }

                return objPassword;
            }
        }

        private void SetPasswordCache(string strCallsign, string strPassword, bool blnPasswordCorrect)
        {
            // 
            // Make an entry in the Password cache for a specified callsign and password.
            // 
            lock (objLock)
            {
                if (!dicPasswordCache.ContainsKey(strCallsign))
                {
                    var objPassword = new PasswordCacheEntry();
                    objPassword.strCallsign = strCallsign;
                    dicPasswordCache.Add(strCallsign, objPassword);
                }

                dicPasswordCache[strCallsign].strPassword = strPassword;
                dicPasswordCache[strCallsign].blnPasswordCorrect = blnPasswordCorrect;
                dicPasswordCache[strCallsign].dttEntryTime = DateTime.UtcNow;
            }

            return;
        }

        public bool ValidateRegistrationKey(string strCallsign, string strkey)
        {
            // 
            // Check registration record (at the CMS) to insure that the key has not been rescinded (voided)
            // Will also return false if no record can be found (hacked registration)
            // Returns True if the key is registered or if we cannot access the database to check it.
            // 
            // If we don't have an Internet connection, assume the key is valid.
            // 
            if (HaveInternetConnection() == false)
                return true;
            // 
            // Check the registration key.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            return WinlinkRegistration.RegistrationHelper.IsValidRegistrationKey(strBaseCallsign, strkey);
        }

        public List<WhitelistRecord> WhitelistGet(string strCallsign)
        {
            // 
            // Get a list of the whitelist addresses for a callsign.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            if (!AccountRegistered(strBaseCallsign))
            {
                return null;
            }
            // 
            // Get the Whitelist
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            string strResponse = JsonCommand("/whitelist/get?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<WhitelistGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse.AccessList;
        }

        public string WhitelistAdd(string strCallsign, string strPassword, string strAddress, bool blnAllow)
        {
            // 
            // Add a whitelist or blacklist address for a callsign.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            if (!AccountRegistered(strBaseCallsign))
            {
                return "The callsign is not valid";
            }
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Address", strAddress);
            lstParam.Add("Allow", Conversions.ToString(blnAllow));
            string strResponse = JsonCommand("/whitelist/add?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<WhitelistAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error on service adding whitelist entry.";
            }

            return "";
        }

        public string WhitelistDelete(string strCallsign, string strPassword, string strAddress)
        {
            // 
            // Remove a whitelist or blacklist address for a callsign.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return "The callsign is not valid";
            }

            if (ValidatePassword(strCallsign, strPassword) == false)
            {
                return "Incorrect password";
            }
            // 
            // Remove the address
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("Address", strAddress);
            string strResponse = JsonCommand("/whitelist/delete?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<WhitelistDeleteResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error on service deleting whitelist entry.";
            }

            return "";
        }

        public double SFItoSSN(double dblSFI)
        {
            // 
            // Convert Solar Flux Index to Sun Spot Number.
            // 
            double dblSSN = 33.52 * Math.Sqrt(85.12 + dblSFI) - 408.99;
            return dblSSN;
        }

        public double SSNtoSFI(double dblSSN)
        {
            // 
            // Convert Sun Spot Number to Solar Flux Index.
            // 
            double temp = (dblSSN + 408.99) / 33.52;
            double SFI = temp * temp - 85.12;
            return SFI;
        }

        public string GetCMSIP(bool blnSkip, string strCaller)
        {
            // 
            // Get a string with the IP address of a CMS.
            // 
            return "cms-z.winlink.org";
        }

        private string GetWebServiceURL(bool blnSkip, string strCaller, bool blnNetParamHost = true, string strForceCMSArg = "")
        {
            // 
            // Get the IP address of an available CMS and return a URL to perform a web service with it.
            // Return an empty string if we can't find a CMS.
            // 
            // See if we are in test mode where we aren't allowing web service requests.
            // 
            if (blnUseProxy)
            {
                return "http://" + strProxyIP + ":" + intProxyPort.ToString();
            }
            else
            {
                return "https://api.winlink.org";
            }
        }

        public Enumerations.CMSInfo CMSAvailable(bool blnSkip = false, int intTimeout = 6)
        {
            // 
            // Returns True if a CMS is available for connection.
            // 
            Enumerations.CMSInfo objCMSInfo = null;
            Enumerations.CMSInfo objServer = null;
            bool blnInternetAvailable = true;
            bool blnTestedInternet = false;
            int intPort = 8772;
            // 
            // If we're using the AWS CMS, assume if we have an Internet connection we can connect to the CMS.
            // 
            if (blnUseProxy)
            {
                if (HaveInternetConnection())
                {
                    objCMSInfo = new Enumerations.CMSInfo("Proxy", "Proxy", strProxyIP);
                    return objCMSInfo;
                }
                else
                {
                    return null;
                }
            }
            else if (HaveInternetConnection())
            {
                objCMSInfo = new Enumerations.CMSInfo("CMS", "CMS", "cms-z.winlink.org");
                return objCMSInfo;
            }
            else
            {
                return null;
            }
        }

        public bool PingCMS()
        {
            // 
            // Perform a Ping service on the CMS, and return True if the CMS is accessible.
            // 
            // Don't ping more often than once every 4 minutes if we were successful last time.
            // 
            if (blnLastPingSuccess && (DateTime.UtcNow - dttLastPingTime).TotalSeconds < 60 * 4)
            {
                return true;
            }
            // 
            // Try to ping the CMS.
            // 
            dttLastPingTime = DateTime.UtcNow;
            var lstParam = new ParamList();
            string strResponse = JsonCommand("/ping?format=json", lstParam);
            if (strResponse == null)
            {
                blnLastPingSuccess = false;
                return false;
            }

            var objResponse = JsonSerializer<PingResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                blnLastPingSuccess = false;
                return false;
            }

            blnLastPingSuccess = true;
            return true;
        }

        public Enumerations.CMSconnection ConnectToServer(string strHostAddress, int intPort, bool blnSSL, int intTimeout = 20)
        {
            // 
            // Connect to a CMS.  This call supports both SSL and non-SSL telnet connections.
            // SSL connections are through port 8773.  Non-SSL connections are through port 8772.
            // 
            // Call separate routine for SSL connections.
            // 
            if (blnSSL)
            {
                return ConnectToCMSSSL(strHostAddress, intPort, intTimeout);
            }
            // 
            // Make a non-SSL connection to a CMS or other server.
            // 
            Enumerations.CMSconnection objCMSconnection = null;
            TcpClient objIPPort = null;
            WinlinkServer objTrial;
            if (string.IsNullOrEmpty(strHostAddress))
                strHostAddress = "cms.winlink.org";
            if (intPort <= 0)
                intPort = 8772;
            // 
            // Increase the timeout value for sat phone connections.
            // 
            if (blnSatPhoneMode & intTimeout < intSatPhoneTimeout)
            {
                intTimeout = intSatPhoneTimeout;
            }
            // 
            // Try to connect to the server.
            // 
            EventsLog("[ConnectToServer] Attempting non-SSL connection to " + strHostAddress + ":" + intPort.ToString() + " Timeout = " + intTimeout.ToString());
            objIPPort = ConnectToIP(strHostAddress, intPort, intTimeout);
            if (objIPPort == null & strHostAddress == "cms.winlink.org")
            {
                EventsLog("[ConnectToServer] Unable to connect using DNS address.  Try IP address.");
                // Temporary kludge for Iridium GO:  If failure using DNS lookup, use the IP address of a AWS CMS.
                objIPPort = ConnectToIP("52.72.230.141", intPort, intTimeout);
            }

            if (objIPPort == null)
            {
                // Unable to connect to the AWS CMS
                EventsLog("[ConnectToServer] Unable to connect to CMS");
                return null;
            }
            // 
            // Successful connection.  Remember that our Internet connection is available.
            // 
            EventsLog("[ConnectToServer] Successfully made non-SSL connection to " + strHostAddress);
            ClearFailedInternetSite();
            dttInternetAvailable = DateTime.UtcNow;
            dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);
            // 
            // Set up the CMSconnection object.
            // 
            objTrial = Globals.AWSSites[0];
            objCMSconnection = new Enumerations.CMSconnection();
            objCMSconnection.blnSSL = false;
            objCMSconnection.strHostAddress = strHostAddress;
            objCMSconnection.intPort = intPort;
            objCMSconnection.strCMSCity = objTrial.strCity;
            objCMSconnection.strCMSName = objTrial.strName;
            objCMSconnection.strCMSIP = objTrial.strName;
            objCMSconnection.objTcpClient = objIPPort;
            return objCMSconnection;
        }

        private Enumerations.CMSconnection ConnectToCMSSSL(string strHostAddress, int intPort, int intTimeout = 20)
        {
            // 
            // Make a SSL Telnet connection to a CMS.
            // 
            Enumerations.CMSconnection objCMSconnection = null;
            TcpClient objIPPort = null;
            WinlinkServer objTrial;
            if (string.IsNullOrEmpty(strHostAddress))
                strHostAddress = "cms.winlink.org";
            if (intPort <= 0)
                intPort = 8773;
            // 
            // Increase the timeout value for sat phone connections.
            // 
            if (blnSatPhoneMode & intTimeout < intSatPhoneTimeout)
            {
                intTimeout = intSatPhoneTimeout;
            }
            // 
            // Try to connect to the AWS SSL server.
            // 
            try
            {
                EventsLog("[ConnectToServerSSL] Attempting SSL connection to " + strHostAddress + ":" + intPort.ToString() + " Timeout = " + intTimeout.ToString());
                objCMSconnection = new Enumerations.CMSconnection();
                objCMSconnection.objTcpClient = new TcpClient();
                objCMSconnection.objTcpClient.Connect(strHostAddress, intPort);
                if (objCMSconnection.objTcpClient.Connected == false)
                {
                    objCMSconnection.Close();
                    // Can't make SSL connection. Try to establish non-SSL connection.
                    EventsLog("[ConnectToCMSSSL] Unable to make SSL connection.  Try non-SSL.");
                    return ConnectToServer(strHostAddress, 8772, false, intTimeout);
                }

                objCMSconnection.objSSL = new SslStream(objCMSconnection.objTcpClient.GetStream(), true);
                objCMSconnection.objSSL.AuthenticateAsClient("cms.winlink.org");
                // 
                // Set up the CMSconnection object.
                // 
                objTrial = Globals.AWSSites[0];
                objCMSconnection.strCMSCity = "CMS-SSL";
                objCMSconnection.strCMSName = objTrial.strName;
                objCMSconnection.strCMSIP = objTrial.strName;
                objCMSconnection.blnSSL = true;
                objCMSconnection.strHostAddress = strHostAddress;
                objCMSconnection.intPort = intPort;
                EventsLog("[ConnectToCMSSSL] Successfully made SSL connection to CMS.");
            }
            catch
            {
                // Unable to establish SSL connection.  Try to make a non-SSL connection.
                EventsLog("[ConnectToCMSSSL] Unable to make SSL connection.  Try non-SSL. " + Information.Err().Description);
                try
                {
                    objCMSconnection.Close();
                    objCMSconnection = null;
                    // Try to establish non-SSL connection.
                    return ConnectToServer(strHostAddress, 8772, false, intTimeout);
                }
                catch (Exception ex)
                {
                }

                objCMSconnection = null;
            }

            return objCMSconnection;
        }

        public TcpClient ConnectToIP(string strIPaddress, int intPort, int intTimeout = 30)
        {
            // 
            // Attempt to connect to a specific IP address.
            // If successful, return a connected Ipport object.
            // If unable to connect to any CMS, return Nothing.
            // Note: On return the AcceptData property of the Ipport is set to False.  You must set it to True to accept data.
            // 
            if (!blnEnableInternet)
                return null;
            var objIPPort = new TcpClient();
            objIPPort.LingerState = new LingerOption(true, intTimeout);
            // Use the default timeout  2017-12-15  objIPPort.Timeout = intTimeout
            // Start a thread to timeout the connection if it hands.
            var objParam = new MonitorConnectionParam();
            var thrMonitorConnection = new Thread(MonitorConnection);
            try
            {
                objParam.objIPPort = objIPPort;
                objParam.dttTimeout = DateTime.UtcNow.AddSeconds(intTimeout);
                thrMonitorConnection.Start(objParam);
            }
            catch
            {
            }
            // Try to connect
            var dttStartConnectTime = DateTime.UtcNow;
            try
            {
                objIPPort.Connect(strIPaddress, intPort);
                if (objParam.blnTimeout)
                {
                    // Timeout abort occurred
                    objIPPort.Close();
                    objIPPort = null;
                    double dblFailTime = (DateTime.UtcNow - dttStartConnectTime).TotalSeconds;
                    dblTotalFailedCMSTime += dblFailTime;
                    intNumFailedCMSConnections += 1;
                    EventsLog("[ConnectToIP] Timeout on connection to " + strIPaddress + ":" + intPort.ToString() + ". Fail time = " + dblFailTime.ToString());
                }
                else
                {
                    double dblConnectTime = (DateTime.UtcNow - dttStartConnectTime).TotalSeconds;
                    if (dblConnectTime > dblMaxSuccessfulConnectTime)
                        dblMaxSuccessfulConnectTime = dblConnectTime;
                    dblTotalCMSConnectionTime += dblConnectTime;
                    intNumCMSConnections += 1;
                }
            }
            catch (Exception ex)
            {
                double dblFailTime = (DateTime.UtcNow - dttStartConnectTime).TotalSeconds;
                objParam.blnConnecting = false;
                Thread.Sleep(100);
                EventsLog("[ConnectToIP] Exception connecting to " + strIPaddress + ":" + intPort.ToString() + ". " + ex.Message);
                try
                {
                    objIPPort.Close();
                }
                catch
                {
                }

                objIPPort = null;
                dblTotalFailedCMSTime += dblFailTime;
                intNumFailedCMSConnections += 1;
            }

            objParam.blnConnecting = false;
            thrMonitorConnection.Abort();
            thrMonitorConnection = null;
            // 
            // Finished
            // 
            return objIPPort;
        }

        private void MonitorConnection(object objArg)
        {
            // 
            // Thread to timeout a connection attempt.
            // 
            MonitorConnectionParam objParam = (MonitorConnectionParam)objArg;
            while (objParam.blnConnecting)
            {
                if (DateTime.UtcNow > objParam.dttTimeout)
                {
                    // Timeout occurred
                    objParam.blnTimeout = true;
                    try
                    {
                        objParam.objIPPort.Close();
                    }
                    catch
                    {
                    }

                    break;
                }

                Thread.Sleep(500);
            }

            objParam.blnConnecting = false;
            return;
        }

        private bool CheckServices(WinlinkServer objServer, string strIP, string strCaller = "")
        {
            // 
            // Check the status of services on the specified CMS and return True if they are available.
            // Returns True if services are available on the CMS.
            // 
            // See if we should force a service to be offline for testing.
            // 
            if (Conversions.ToBoolean(blnDisableWebServices))
            {
                return true;
            }

            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(objServer.strCity.ToUpper(), strForceServiceOffline, false)))
            {
                return false;
            }

            intSystemServiceCount += 1;
            // 
            // See if it's time to check the service status.
            // 
            if (DateTime.UtcNow.Subtract(objServer.dttServiceCheck).TotalMinutes >= 30)
            {
                // 
                // Do a new check of service status on the CMS.
                // 
                if (blnUseProxy)
                {
                    objServer.blnServicesAvailable = TestCMS(strProxyIP, intProxyPort, strCaller);
                }
                else
                {
                    objServer.blnServicesAvailable = TestCMS(strIP, intServicesPort, strCaller);
                }

                objServer.blnServicesAvailable = TestCMS(strIP, intServicesPort, strCaller);
                objServer.dttServiceCheck = DateTime.UtcNow;
            }
            // 
            // Finished.  Return the status of services.
            // 
            return objServer.blnServicesAvailable;
        }

        private Enumerations.CMSInfo GetCMSHost(bool blnSkip, string strCaller, string strForceCMSArg = "")
        {
            // 
            // Try to find an available CMS server.  Return Nothing if there is no available server.
            // 
            Enumerations.CMSInfo objServer = null;
            // 
            // See if we recently determined that the Internet is down.
            // 
            if (blnSatPhoneMode == false && CheckInternetDownTime() == false | !blnEnableInternet)
                return null;
            if (!string.IsNullOrEmpty(strForceCMSArg))
            {
                strForceCMS = strForceCMSArg;
            }
            // 
            // See if we should use CMS on AWS.
            // 
            return GetCMSHostAWS(blnSkip, strCaller, strForceCMSArg);
        }

        private string GetNetParamHostIP(string strCaller, string strForceCMSArg = "")
        {
            // 
            // Get the IP address of a CMS that can be used to access network parameters.
            // Return an empty string if we can't find one.
            // 
            var objCMS = GetNetParamHost(strCaller, 0, strForceCMSArg);
            if (objCMS is object)
            {
                return objCMS.strCMSIP;
            }
            else
            {
                return "";
            }
        }

        public Enumerations.CMSInfo GetCMSHostAWS(bool blnSkip, string strCaller, string strForceCMSArg = "")
        {
            // 
            // Try to find an available CMS server on AWS.  Return Nothing if there is no available server.
            // 
            Enumerations.CMSInfo objServer = null;
            // 
            // See if we recently determined that the Internet is down.
            // 
            if (blnSatPhoneMode == false && TestInternetWork() == false | !blnEnableInternet)
                return null;
            if (!string.IsNullOrEmpty(strForceCMSArg))
            {
                strForceCMS = strForceCMSArg;
            }
            // 
            // We have an Internet connection.  Return the object for the AWS CMS.
            // 
            var objTrial = Globals.AWSSites[0];
            objServer = objTrial.objCMSInfo;
            objServer.strCMSIP = objTrial.strDefaultIP;
            objTrial.dttServerAvailable = DateTime.UtcNow;
            objTrial.dttServerUnavailable = DateTime.UtcNow.AddDays(-1);
            strCurrentApiServer = "CMS";
            // 
            // Finished
            // 
            return objServer;
        }

        private Enumerations.CMSInfo GetNetParamHost(string strCaller, int intStartingIndex = 0, string strForceCMSArg = "")
        {
            // 
            // Try to find an available CMS server that supports network parameter requests.  Return Nothing if there is no available server.
            // 
            Enumerations.CMSInfo objServer = null;
            // 
            // See if we recently determined that the Internet is down.
            // 
            if (CheckInternetDownTime() == false | !blnEnableInternet)
                return null;
            if (Conversions.ToBoolean(blnDisableWebServices))
            {
                return null;
            }
            // 
            // See if should connect to a CMS on AWS.
            // 
            return GetCMSHostAWS(false, strCaller);
        }

        public bool TestCMS(string strIP, int intPort, string strCaller = "")
        {
            // 
            // Test a CMS and return True if it's available.
            // 
            return true;
        }

        public string GetAutoupdateURL(bool blnTest)
        {
            // 
            // Get the full URL including the port number of the Autoupdate or AutoupdateTest server.
            // Return an empty strin if we don't have an Internet connection.
            // 
            if (blnSatPhoneMode)
                return "";
            // 
            // We think we have an Internet connection.  Form the URL.
            // 
            if (CheckInternetDownTime() == false | !blnEnableInternet)
                return "";  // Port to connect to on Autoupdate server
            
            const int intAutoupdatePort = 8776;

            string strPath;
            // New Autoupdate server
            if (blnTest)
            {
                strPath = "https://" + "autoupdatetest.winlink.org:443/";
            }
            else
            {
                strPath = "https://" + "autoupdate2.winlink.org:443/";
            }

            return strPath;
        }

        public string GetAutoupdatePassword()
        {
            // 
            // Get the password for the Autoupdate server.
            // 
            return "U53FLW!";
        }

        public List<string> GetAutoupdateFileList(bool blnTest)
        {
            // 
            // Get the list of files available on the Autoupdate server.
            // 
            var lstFileNames = new List<string>();
            string strURL = GetAutoupdateURL(blnTest);
            if (strURL == null | string.IsNullOrEmpty(strURL))
                return lstFileNames;
            string strPassword = GetAutoupdatePassword();
            // 
            // Set up a WebClient to access the Autoupdate site.
            // 
            var client = new WebClientExtended();
            client.Timeout = 30000;  // set timeout (milliseconds)
            client.Credentials = new NetworkCredential("Autoupdate", strPassword);

            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */        // 
                                                                                                                                                         // Try to download the list of files.
                                                                                                                                                         // 
            string FileInfo;
            try
            {
                FileInfo = client.DownloadString(strURL + "Files.txt");
                client.Dispose();
                client = null;
            }
            catch (Exception ex)
            {
                client.Dispose();
                client = null;
                return lstFileNames;
            }
            // 
            // Build the list of return files
            // 
            var strFiles = FileInfo.Split(Conversions.ToChar(Microsoft.VisualBasic.Constants.vbCr));
            foreach (string strFile in strFiles)
            {
                // 
                // Loop through all the lines in the file
                // 
                string strLine = Globals.StripComment(strFile);
                if (strLine.Length > 0)
                {
                    lstFileNames.Add(strLine);
                }
            }
            // 
            // Finished
            // 
            return lstFileNames;
        }

        public string DownloadUpdateFile(bool blnTest, string strRemoteFile, string strLocalPath, string strLocalFile = "")
        {
            // 
            // Attempt to download file for Autoupdate.
            // Return an empty string on success or an error message on failure.
            // 
            if (string.IsNullOrEmpty(strLocalFile))
                strLocalFile = strRemoteFile;
            string strURL = GetAutoupdateURL(blnTest);
            if (strURL == null | string.IsNullOrEmpty(strURL))
                return "Autoupdate server is not available";
            string strPassword = GetAutoupdatePassword();
            // 
            // Set up a WebClient to access the Autoupdate site.
            // 
            var client = new WebClientExtended();
            client.Timeout = 30000;  // set timeout (milliseconds)
            client.Credentials = new NetworkCredential("Autoupdate", strPassword);
            // 
            // Try to download the file.
            // 
            bool blnException = false;
            try
            {
                strURL += strRemoteFile.Replace(" ", "%20");
                strLocalPath += strLocalFile;
                client.DownloadFile(strURL, strLocalPath);
                client.Dispose();
                client = null;
            }
            catch (WebException ex)
            {
                blnException = true;
                client.Dispose();
                client = null;
            }

            if (blnException)
            {
                // Exception using extended webclient.  Try using standard webclient
                try
                {
                    var objStandardClient = new WebClient();
                    objStandardClient.Credentials = new NetworkCredential("Autoupdate", strPassword);
                    objStandardClient.DownloadFile(strURL, strLocalPath);
                    objStandardClient.Dispose();
                    objStandardClient = null;
                }
                catch (WebException ex)
                {
                    return ex.Message + " (" + ex.InnerException.ToString() + ")";
                }
            }
            // 
            // Success
            // 
            return "";
        }

        private bool HaveLanConnection()
        {
            // 
            // Ask Windows if Windows says we have an Internet connection.
            // 
            if (blnSatPhoneMode)
                return true;
            if (!blnEnableInternet)
                return false;
            var intFlags = default(int);
            var dwReserved = default(int);
            bool blnConnected = InternetGetConnectedState(ref intFlags, dwReserved);
            if (blnConnected == false)
            {
                // No network interface
                dttInternetUnavailable = DateTime.UtcNow;
                dttInternetAvailable = DateTime.UtcNow.AddDays(-1);
                SetError("HaveLanConnection: No network interface available");
                SetFailedInternetSite("(No LAN connection)");
            }

            return blnConnected;
        }

        public bool HaveInternetConnection(bool blnForceTest = false)
        {
            // 
            // Check if we can actually access Internet sites.
            // 
            return TestInternet(blnForceTest);
        }

        public bool TestInternet(bool blnForceTest = false)
        {
            // 
            // Return True if we determine we have actual Internet connectivity.
            // 
            bool blnInternetOK = false;
            // 
            // See if Internet access has been manually disabled.
            // 
            if (!blnEnableInternet)
            {
                return false;
            }
            // 
            // Always say we're connected in sat phone mode.
            // 
            if (blnSatPhoneMode)
                return true;
            // 
            // Do fast check to see if we have a LAN (but not necessarily WAN) connection.
            // 
            if (!HaveLanConnection())
                return false;
            // 
            // If we recently determined that the Internet is up or down, use that rather than doing a new test.
            // 
            if (!blnForceTest)
            {
                // See if we recently determined that the Internet is down.
                if (CheckInternetDownTime() == false)
                {
                    return false;
                }
                // See if we recently determined that the Internet is up.
                if (CheckInternetUpTime())
                {
                    return true;
                }
            }
            // 
            // We have to do an Internet status check.
            // 
            lock (objLock)
                // Call private work routine to do the test.
                blnInternetOK = TestInternetWork(blnForceTest);
            // 
            // Finished
            // 
            return blnInternetOK;
        }

        private bool TestInternetWork(bool blnForceCheck = false, int intMaxBadSites = 4)
        {
            // 
            // Private function to check for Internet availability.
            // This routine assumes SyncLock objLock has been done external to it.
            // 
            // Return quickly if we are set to never use the Internet.
            // 
            if (!blnEnableInternet)
                return false;
            // 
            // Always say we're connected in sat phone mode.
            // 
            if (blnSatPhoneMode)
                return true;
            try
            {
                // 
                // First, see if there's a network interface configured and active.
                // 
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    // No network interface
                    dttInternetUnavailable = DateTime.UtcNow;
                    dttInternetAvailable = DateTime.UtcNow.AddDays(-1);
                    SetError("TestInternetWork: No network interface available");
                    SetFailedInternetSite("(No network interface available)");
                    return false;
                }
                // 
                // See if Windows says we have an Internet connection.
                // 
                if (HaveLanConnection() == false)
                {
                    // No Internet connection
                    dttInternetUnavailable = DateTime.UtcNow;
                    dttInternetAvailable = DateTime.UtcNow.AddDays(-1);
                    SetError("TestInternetWork: No network interface");
                    SetFailedInternetSite("(No Internet connection)");
                    return false;
                }
            }
            catch
            {
                return false;
            }
            // 
            // Try to ping a CMS.
            // 
            return PingCMS();
        }

        private bool CheckInternetDownTime()
        {
            // 
            // Check to see if we recently determined that the Internet connection is down.
            // Return True if we think the Internet is up, return False if we determined recently that it's down.
            // 
            if (DateTime.UtcNow.Subtract(dttInternetUnavailable).TotalSeconds >= dblMonitorPeriod & blnEnableInternet)
            {
                // Internet connection may be good.
                return true;
            }
            else
            {
                // Recent check showed Internet is down.
                return false;
            }
        }

        private bool CheckInternetUpTime()
        {
            // 
            // Check to see if we recently determined that the Internet connection is available.
            // Return True if we determined recently the Internet is up.
            // 
            if (DateTime.UtcNow.Subtract(dttInternetAvailable).TotalSeconds <= dblMonitorPeriod & blnEnableInternet)
            {
                // Internet connection is good.
                return true;
            }
            else
            {
                // We don't know if it's up.
                return false;
            }
        }

        private void MonitorInternetThread()
        {
            // 
            // Background thread to monitor the state of the Internet connection.
            // 
            do
            {
                // 
                // Check the internet status
                // 
                if (blnSatPhoneMode)
                {
                    // Don't do test when in sat phone mode.
                    dttInternetAvailable = DateTime.UtcNow;
                    dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);
                }
                else
                {
                    lock (objLock)
                    {
                        if (TestInternetWork(true))
                        {
                            // The Internet connection is good.
                            dttInternetAvailable = DateTime.UtcNow;
                            dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);
                        }
                        else
                        {
                            // The Internet is down
                            dttInternetUnavailable = DateTime.UtcNow;
                            dttInternetAvailable = DateTime.UtcNow.AddDays(-1);
                        }
                    }
                }
                // 
                // Sleep for a while.
                // 
                Thread.Sleep((int)(dblMonitorPeriod * 1000));
            }
            while (true);
        }

        public bool TestConnection(string strHost, int intPort = 80, int intTimeout = -1)
        {
            // 
            // Test if we can make a connection, and return True if we can.
            // 
            if (!blnEnableInternet)
                return false;
            if (intTimeout < 0)
                intTimeout = intConnectionTimeoutSeconds;
            if (blnSatPhoneMode)
            {
                intTimeout = intSatPhoneTimeout;
            }

            try
            {
                var client = new TcpClient();
                var ar = client.BeginConnect(strHost, intPort, null, null);
                using (var wh = ar.AsyncWaitHandle)
                {
                    if (!wh.WaitOne(TimeSpan.FromSeconds(intTimeout), false))
                    {
                        SetError("TestConnection: Timeout connecting to " + strHost);
                        client.Close();
                        throw new TimeoutException();
                    }

                    client.EndConnect(ar);
                }
                // Connection was successful
                return true;
            }
            catch
            {
                // Unable to connect
                SetError("TestConnection: Exception " + Information.Err().Description);
                return false;
            }
        }

        private bool DoConn(string strIPAddr, int intPort, bool blnValidateResp = false)
        {
            // 
            // Return true if the TCP connection was successful.
            // 
            if (!blnEnableInternet)
                return false;
            using (var objConn = new TcpClient())
            {
                // 
                // Wrap the socket in a using block to dispose the object and resources as soon as we are done with it.
                // 
                int intTimeout = 30;
                if (blnSatPhoneMode)
                {
                    intTimeout = intSatPhoneTimeout;
                }

                try
                {
                    objConn.NoDelay = true;
                    objConn.ReceiveTimeout = intTimeout * 1000;
                    objConn.SendTimeout = intTimeout * 1000;
                    objConn.Connect(strIPAddr, intPort);
                    if (blnValidateResp)
                    {
                        // 
                        // Validate a CMS Telnet connection.  Do a packet handshake with the remote server
                        // 
                        using (var st = objConn.GetStream())
                        {
                            int ret = 0;
                            var bufIn = new byte[1025];
                            try
                            {
                                // The CMS should respond to the connection with a logon prompt.
                                ret = st.Read(bufIn, 0, bufIn.Length);
                            }
                            catch
                            {
                                // We did not get a logon prompt.  The CMS appears to be dead.
                                objConn.Client.Close();
                                return false;
                            }
                            // See if the logon prompt looks good.
                            string q = Encoding.ASCII.GetString(bufIn, 0, ret).ToLower();
                            if (!(q.Contains("callsign") | q.Contains("#") | Information.IsNumeric(q)))
                            {
                                // Invalid logon prompt.
                                objConn.Client.Close();
                                return false;
                            }
                            // The logon prompt looks good.  Send "#" and see if we get a good reply.
                            try
                            {
                                var bytPound = Encoding.ASCII.GetBytes("#");
                                st.Write(bytPound, 0, bytPound.Length);
                                ret = st.Read(bufIn, 0, bufIn.Length);
                                if (ret > 0)
                                {
                                    string result = Encoding.ASCII.GetString(bufIn, 0, ret);
                                    if (result.StartsWith("#") == false)
                                    {
                                        objConn.Client.Close();
                                        return false;
                                    }
                                    // Success
                                    objConn.Client.Close();
                                    return true;
                                }

                                objConn.Client.Close();
                                return false;
                            }
                            catch
                            {
                                objConn.Client.Close();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        // We don't need to validate the Telnet connection.
                        objConn.Client.Close();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            // 
            // Connection failed
            // 
            return false;
        }

        private bool PingSite(string strAddress)
        {
            // 
            // Ping a specified site.
            // 
            if (!blnEnableInternet)
                return false;
            return My.MyProject.Computer.Network.Ping(strAddress);
        }

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(ref int lpSFlags, int dwReserved);

        public enum InetConnState
        {
            modem = 0x1,
            lan = 0x2,
            proxy = 0x4,
            ras = 0x10,
            offline = 0x20,
            configured = 0x40
        }

        public void SetSatPhoneMode(bool blnSatPhone)
        {
            // 
            // Turn sat phone mode on or off.
            // 
            blnSatPhoneMode = blnSatPhone;
            return;
        }

        public string PostVersionRecord(string strCallsign, string strProgramName, string strProgramVersion, string strProgramOptions = "")
        {
            // 
            // Post the version information to the winlink system.
            // Return an empty string on success or an error message on failure.
            // 
            // Validate arguments.
            // 
            if (!IsValidCallsign(strCallsign))
            {
                return "Invalid callsign/ssid specified: " + strCallsign;
            }

            if (!AccountRegistered(strCallsign))
            {
                return "Callsign is not registered with Winlink system: " + strCallsign;
            }

            if (strProgramName.Length == 0 | strProgramName.Length > 48)
            {
                return "Invalid Program name specified: " + strProgramName;
            }

            if (strProgramVersion.Length == 0 | strProgramVersion.Length > 24)
            {
                return "Invalid Program version specified: " + strProgramVersion;
            }
            // 
            // See if this is a duplicate entry.
            // 
            string strParam = "Version_" + strProgramName;
            string strValue = strProgramVersion + "_" + strProgramOptions;
            if (CheckRecentNetworkParameter(strCallsign, strParam, strValue))
            {
                return "";
            }
            // 
            // Set up arguments for the API service.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Program", strProgramName);
            lstParam.Add("Version", strProgramVersion);
            lstParam.Add("Comments", strProgramOptions);
            string strResponse = JsonCommand("/version/add?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error posting program version";
            }

            var recPostVersion = JsonSerializer<VersionAddResponse>.DeSerialize(strResponse);
            if (recPostVersion == null || recPostVersion.HasError | !string.IsNullOrEmpty(recPostVersion.ResponseStatus.ErrorCode))
            {
                ReportError(recPostVersion.ErrorMessage);
                if (recPostVersion is object)
                {
                    return "Error posting program version: " + recPostVersion.ResponseStatus.ErrorCode;
                }
                else
                {
                    return "Error posting program version";
                }
            }
            else
            {
                // Remember we reported this version info
                AddRecentNetworkParameter(strCallsign, strParam, strValue);
                return "";
            }
            // 
            // Finished
            // 
            return "";
        }

        public string PostUsageRecord(string strCallsign, string strPassword, string strRecordType, string strUsageRecord)
        {
            // 
            // Post the usage record the winlink system.
            // Return an empty string on success or an error message on failure.
            // 
            // Validate arguments.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return "Invalid callsign/ssid specified: " + strCallsign;
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return "Invalid or missing password";
            }

            if (strRecordType.Length == 0 | strRecordType.Length > 20)
            {
                return "Invalid record type specified: " + strRecordType;
            }

            if (strUsageRecord.Length == 0 | strUsageRecord.Length > 500)
            {
                return "Invalid usage record specified: " + strUsageRecord;
            }
            // 
            // Report the usage to the system.
            // 
            if (HaveInternetConnection())
            {
                if (SetNetworkParameter(strCallsign, strPassword, strRecordType, strUsageRecord) == false)
                {
                    return "Unable to report usage to system";
                }
            }

            return "";
        }

        public string PostGatewayChannelRecord(string strUserCallsign, string strChannelCallsign, string strGridSquare, int intFrequency, int intMode, int intBaud, int intPower, int intAntennaHeight, int intAntennaGain, int intAntennaDirection, string strHours, string strServiceCode)
        {
            // 
            // Post the gateway radio channel information to the winlink system.
            // Return an empty string on success or an error message if failure.
            // 
            if (!AccountRegistered(strChannelCallsign))
            {
                return "Invalid callsign/ssid specified: " + strChannelCallsign;
            }

            if (!Globals.IsValidGridSquare(strGridSquare))
            {
                return "Invalid format of GridSquare: " + strGridSquare;
            }

            if (strServiceCode.Length > 16)
            {
                return "Max length of 16 exceeded on ServiceCode: " + strServiceCode;
            }

            if (strHours.Length > 255)
            {
                return "Max length of 255 exceeded on Hours: " + strServiceCode;
            }

            if (intFrequency < 1)
            {
                return "Invalid channel frequency: " + intFrequency.ToString();
            }

            string strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
            if (!AccountRegistered(strBaseCallsign))
            {
                return "Callsign is not registered with Winlink system: " + strBaseCallsign;
            }

            if (!AccountRegistered(Globals.BaseCallsign(strChannelCallsign)))
            {
                return "Channel callsign is not registered with Winlink system: " + Globals.BaseCallsign(strChannelCallsign);
            }

            if (string.IsNullOrEmpty(strServiceCode))
            {
                strServiceCode = "PUBLIC";
            }
            // Make sure there's not more than one service code.
            var lstCodes = strServiceCode.Split(' ');
            strServiceCode = lstCodes[0];
            // 
            // Set up arguments for the API service.
            // 
            var lstParam = new ParamList();
            lstParam.Add("BaseCallsign", strUserCallsign);
            lstParam.Add("Baud", intBaud.ToString());
            lstParam.Add("Callsign", strChannelCallsign);
            lstParam.Add("Direction", "0");
            lstParam.Add("Frequency", intFrequency.ToString());
            lstParam.Add("Gain", "0");
            lstParam.Add("GridSquare", strGridSquare);
            lstParam.Add("Height", "25");
            lstParam.Add("Hours", strHours);
            lstParam.Add("Mode", intMode.ToString());
            lstParam.Add("Power", intPower.ToString());
            lstParam.Add("ServiceCode", strServiceCode);
            string strResponse = JsonCommand("/channel/add?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error posting channel record (no response)";
            }

            var objResponse = JsonSerializer<ChannelAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                ReportError();
                if (objResponse == null)
                {
                    return "Error posting channel record.";
                }
                else
                {
                    if (objResponse is object)
                        ReportError(objResponse.ErrorMessage);
                    return "Error posting channel record: " + objResponse.ResponseStatus.ErrorCode + " " + objResponse.ResponseStatus.Message;
                }
            }
            else
            {
                return "";
            }
        }

        public string RemoveGatewayChannelRecord(string strUserCallsign, string strChannelCallsign, int intFrequency, int intMode)
        {
            // 
            // Remove the gateway radio channel information.
            // 
            if (!AccountRegistered(strChannelCallsign))
            {
                return "The channel callsign is not valid";
            }

            if (intFrequency < 1)
            {
                return "The channel frequency is not valid: " + intFrequency.ToString();
            }
            // 
            // Set up arguments for the API service.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strChannelCallsign);
            lstParam.Add("Frequency", intFrequency.ToString());
            lstParam.Add("Mode", intMode.ToString());
            string strResponse = JsonCommand("/channel/delete?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error removing all channel records: " + strResponse;
            }

            var objResponse = JsonSerializer<ChannelDeleteResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error removing channel record: " + objResponse.ResponseStatus.ErrorCode;
            }
            else
            {
                return "";
            }
        }

        public string PurgeAllChannels(string strRMSBaseCallsign)
        {
            // 
            // Delete information about the channels registered by an RMS before storing the new channel information.
            // 
            if (!AccountRegistered(strRMSBaseCallsign))
            {
                return "The callsign is not valid";
            }
            // 
            // Execute the command and check the results.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strRMSBaseCallsign);
            string strResponse = JsonCommand("/channel/delete/all?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error deleting old channels for " + strRMSBaseCallsign + "  " + strResponse;
            }

            var objResponse = JsonSerializer<ChannelDeleteAllResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error deleting old channels for " + strRMSBaseCallsign + "  " + objResponse.ResponseStatus.ErrorCode;
            }
            else
            {
                return "";
            }
        }

        public string WatchSet(string strCallsign, string strPassword, string strProgram, int intAllowedTardyHours, string strNotificationEmails)
        {
            // 
            // Set parameters to notify people if a program stops reporting it's alive.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return "The callsign is not valid";
            }

            if (string.IsNullOrEmpty(strPassword))
            {
                return "Missing password";
            }

            if (string.IsNullOrEmpty(strProgram))
            {
                return "Missing program name";
            }

            if (HaveInternetConnection() == false)
            {
                return "There is no Internet connection";
            }

            if (!AccountRegistered(strCallsign))
            {
                return "Callsign is not registered with the Winlink system: " + strCallsign;
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return "The password is not correct";
            }
            // 
            // Execute the command and check the results.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("Program", strProgram);
            lstParam.Add("AllowedTardyHours", intAllowedTardyHours.ToString());
            lstParam.Add("NotificationEmails", strNotificationEmails);
            string strResponse = JsonCommand("/watch/set?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error setting gateway watch notification: " + strResponse;
            }

            var objResponse = JsonSerializer<WatchSetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error setting gateway watch notification parameters " + objResponse.ResponseStatus.ErrorCode;
            }
            else
            {
                return "";
            }
        }

        public List<WatchRecord> WatchList(string strCallsign, string strPassword)
        {
            // 
            // Get a list of all program watches set for an account.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return null;
            }

            if (string.IsNullOrEmpty(strPassword))
            {
                return null;
            }

            if (HaveInternetConnection() == false)
            {
                return null;
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return null;
            }
            // 
            // Execute the command and check the results.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            string strResponse = JsonCommand("/watch/list?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<WatchListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse.List;
        }

        public WatchGetResponse WatchGet(string strCallsign, string strPassword, string strProgram)
        {
            // 
            // Get values set for gateway watch notification.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return null;
            }

            if (string.IsNullOrEmpty(strPassword))
            {
                return null;
            }

            if (HaveInternetConnection() == false)
            {
                return null;
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return null;
            }
            // 
            // Execute the command and check the results.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("Program", strProgram);
            string strResponse = JsonCommand("/watch/get?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<WatchGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse;
        }

        public string WatchDelete(string strCallsign, string strPassword, string strProgram)
        {
            // 
            // Delete a gateway watch notification.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return "Missing callsign";
            }

            if (string.IsNullOrEmpty(strPassword))
            {
                return "Missing password";
            }

            if (string.IsNullOrEmpty(strProgram))
            {
                return "Missing program name";
            }

            if (HaveInternetConnection() == false)
            {
                return "Not connected to the Internet";
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return "Incorrect password";
            }
            // 
            // Execute the command and check the results.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("Program", strProgram);
            string strResponse = JsonCommand("/watch/delete?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<WatchDeleteResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error setting gateway watch notification parameters " + objResponse.ResponseStatus.ErrorCode;
            }
            else
            {
                return "";
            }
        }

        public string WatchPing(string strCallsign, string strProgram)
        {
            // 
            // Report that a program is alive.
            // 
            if (!AccountRegistered(strCallsign))
            {
                return "Callsign is not registered with Winlink: " + strCallsign;
            }

            if (string.IsNullOrEmpty(strProgram))
            {
                return "Missing program name";
            }

            if (HaveInternetConnection(true) == false)
            {
                return "No Internet connection";
            }
            // 
            // Execute the command and check the results.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Program", strProgram);
            string strResponse = JsonCommand("/watch/ping?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error reporting program-alive ping";
            }

            var objResponse = JsonSerializer<WatchPingResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse == null)
                {
                    return "Error reporting program is alive (null response)";
                }
                else
                {
                    ReportError(objResponse.ErrorMessage);
                    return "Error reporting program is alive " + objResponse.ResponseStatus.Message + " (" + objResponse.ResponseStatus.ErrorCode + ")";
                }
            }
            else
            {
                return "";
            }
        }

        public List<ProgramVersionsRecord> GetProgramVersions()
        {
            // 
            // Get information about all programs reporting their version information.
            // 
            var lstParam = new ParamList();
            string strResponse = JsonCommand("/program/versions?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<ProgramVersionsResponse>.DeSerialize(strResponse);
            if (objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            else
            {
                return objResponse.ProgramVersionList;
            }
        }

        public List<VersionRecord> ProgramVersionList(string strProgram, int intHours = 72)
        {
            // 
            // Get information about running program versins.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Program", strProgram);
            lstParam.Add("HistoryHours", intHours.ToString());
            string strResponse = JsonCommand("/version/list?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<VersionListResponse>.DeSerialize(strResponse);
            if (objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            else
            {
                return objResponse.VersionList;
            }
        }

        public string SessionRecordAdd(DateTime Timestamp, string Application, string Version, string Cms, string Server, string ServerGrid, string Client, string ClientGrid, string Sid, string Mode, int Frequency, int Kilometers, int Degrees, string LastCommand, int MessagesSent, int MessagesReceived, int BytesSent, int BytesReceived, int HoldingSeconds, string IdTag = "")
        {
            // 
            // Report a new session for a program like RMS Trimode.
            // 
            // Set up arguments for the API service.
            // 
            if (!string.IsNullOrEmpty(Server) && !AccountRegistered(Server))
            {
                return "RMS callsign is not registered with Winlink: " + Server;
            }

            var lstParam = new ParamList();
            lstParam.Add("Application", Application);
            lstParam.Add("Version", Version);
            lstParam.Add("Cms", Cms);
            lstParam.Add("Server", Server);
            if (!string.IsNullOrEmpty(ServerGrid))
                lstParam.Add("ServerGrid", ServerGrid);
            lstParam.Add("Client", Client);
            if (!string.IsNullOrEmpty(ClientGrid))
                lstParam.Add("ClientGrid", ClientGrid);
            lstParam.Add("Sid", Sid);
            lstParam.Add("Mode", Mode);
            lstParam.Add("Frequency", Frequency.ToString());
            lstParam.Add("Kilometers", Kilometers.ToString());
            lstParam.Add("Degrees", Degrees.ToString());
            lstParam.Add("LastCommand", LastCommand);
            lstParam.Add("MessagesSent", MessagesSent.ToString());
            lstParam.Add("MessagesReceived", MessagesReceived.ToString());
            lstParam.Add("BytesSent", BytesSent.ToString());
            lstParam.Add("BytesReceived", BytesReceived.ToString());
            lstParam.Add("HoldingSeconds", HoldingSeconds.ToString());
            lstParam.Add("IdTag", IdTag);
            // 
            // Execute the API command.
            // 
            string strResponse = JsonCommand("/session/add?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error reporting session";
            }

            var objResponse = JsonSerializer<SessionRecordAddResponse>.DeSerialize(strResponse);
            if (objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return objResponse.ResponseStatus.ErrorCode;
            }
            else
            {
                return "";
            }
        }

        public string SetSecureLogon(string strCallsign, string strPassword, bool blnEnable, string strWebServiceUrl = "")
        {
            // 
            // Set or clear the flag to require the callsign to use secure logon.
            // 
            return "";
        }

        public bool SetAccountLockout(string strCallsign, bool blnLockout)
        {
            // 
            // Set or clear the flag to lockout an account.
            // 
            if (!IsValidCallsign(strCallsign))
            {
                return false;
            }
            // 
            // Finished
            // 
            return true;
        }

        public string SetPassword(string strCallsign, string strOldPassword, string strNewPassword)
        {
            // 
            // If the password is being changed and the old password is correct, change the password on the CMS.
            // 
            string strBaseCallsign;
            if (Globals.IsValidTacticalAddress(strCallsign))
            {
                strBaseCallsign = strCallsign;
            }
            else
            {
                strBaseCallsign = Globals.BaseCallsign(strCallsign);
                if (!IsValidCallsign(strBaseCallsign) | Globals.IsValidTacticalAddress(strCallsign, 12))
                {
                    return "Error changing password The callsign Is Not valid";
                }
            }
            // 
            // Validate the new password syntax
            // 
            if (!string.IsNullOrEmpty(Globals.CheckPasswordSyntax(strNewPassword)))
            {
                return "Error changing password: The syntax of the password Is Not valid";
            }
            // 
            // Never set an empty password.
            // 
            if (string.IsNullOrEmpty(strNewPassword))
                return "";
            // 
            // Find a CMS to send the request to.
            // 
            string strWebServiceUrl = GetWebServiceURL(false, strCallsign);
            if (string.IsNullOrEmpty(strWebServiceUrl))
            {
                // Can't access server.
                return "Error changing password: Cannot connect to a CMS";
            }
            // 
            // See if we are changing the password.
            // 
            if (ValidatePassword(strCallsign, strNewPassword, strWebServiceUrl) == false)
            {
                // 
                // Make the password change.
                // Set up arguments for the API service.
                // 
                var lstParam = new ParamList();
                lstParam.Add("Callsign", strBaseCallsign);
                lstParam.Add("OldPassword", strOldPassword);
                lstParam.Add("NewPassword", strNewPassword);
                lstParam.Add("WebServiceAccesscode", strAccountAccessCode);
                string strResponse = JsonCommand("/account/password/change?format=json", lstParam);
                if (strResponse == null)
                {
                    return "Error changing password";
                }

                var objResponse = JsonSerializer<AccountPasswordSetResponse>.DeSerialize(strResponse);
                if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
                {
                    if (objResponse is object)
                        ReportError(objResponse.ErrorMessage);
                    return "Error changing password";
                }
                else
                {
                    // Cache the password.
                    SetPasswordCache(strBaseCallsign, strNewPassword, true);
                    return "";
                }
            }
            // 
            // Finished
            // 
            return "";
        }

        public string AddTacticalAddress(string strTacticalAddress, string strPassword)
        {
            // 
            // If a specified tactical address isn't registered with the system, register it.
            // 
            if (!Globals.IsValidTacticalAddress(strTacticalAddress, 12))
            {
                return "Error adding tactical address: The syntax of the tactical address Is Not valid.";
            }
            // 
            // See if the tactical address is already registered.
            // 
            if (SearchAccountCache(strTacticalAddress))
            {
                // This tactical address is already registered and stored in the account cache.
                return "";
            }

            var lstParam = new ParamList();
            lstParam.Add("TacticalAccount", strTacticalAddress);
            lstParam.Add("WebServiceAccesscode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/tactical/exists?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error adding tactical address:  " + strTacticalAddress + " Unable to connect to a CMS";
            }

            var objResponse = JsonSerializer<AccountTacticalExistsResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error adding tactical address " + strTacticalAddress;
            }

            if (objResponse.Tactical)
            {
                // This tactical address is already registered.
                SetAccountCache(strTacticalAddress, true);
                return "";
            }
            // 
            // Register a new tactical address.  Note: /account/add works for both callsigns and tactical addresses.
            // 
            lstParam = new ParamList();
            lstParam.Add("Callsign", strTacticalAddress);
            lstParam.Add("Password", strPassword);
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            strResponse = JsonCommand("/account/add?format=json", lstParam);
            if (strResponse == null)
            {
                return "Unable to connect to a Winlink server";
            }

            var objResponseAdd = JsonSerializer<AccountAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error adding tactical address " + strTacticalAddress;
            }
            // 
            // Success.  Add an entry to the account cache.
            // 
            SetAccountCache(strTacticalAddress, true);
            return "";
        }

        public string RemoveTacticalAddress(string strTacticalAddress)
        {
            // 
            // Remove a tactical address from the Winlink system.
            // 
            if (!Globals.IsValidTacticalAddress(strTacticalAddress, 12))
            {
                return "Error removing tactical address The syntax of the tactical address Is Not valid.";
            }

            var lstParam = new ParamList();
            lstParam.Add("TacticalAccount", strTacticalAddress);
            lstParam.Add("WebServiceAccesscode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/tactical/exists?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error removing tactical address:  " + strTacticalAddress + " Unable to connect to a CMS";
            }

            var objResponse = JsonSerializer<AccountTacticalExistsResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error removing tactical address " + strTacticalAddress;
            }

            if (objResponse.Tactical == false)
            {
                // This tactical address isn't registered.
                SetAccountCache(strTacticalAddress, false);
                return "";
            }
            // 
            // Remove a tactical address.
            // 
            strResponse = JsonCommand("/account/tactical/remove?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error removing tactical address " + strTacticalAddress + " Unable to connect to a CMS";
            }

            var objResponseRemove = JsonSerializer<AccountTacticalRemoveResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error removing tactical address " + strTacticalAddress;
            }
            // 
            // Success.  Remove entry from the account cache.
            // 
            SetAccountCache(strTacticalAddress, false);
            return "";
        }

        public bool TacticalAddressExists(string strTacticalAddress, string strWebServiceUrl = "")
        {
            // 
            // Return True if the specified Tactical Address is registered with the system.
            // 
            if (!Globals.IsValidTacticalAddress(strTacticalAddress, 12))
            {
                return false;
            }
            // 
            // See if the tactical address is already registered.
            // 
            if (SearchAccountCache(strTacticalAddress))
            {
                // This tactical address exists and is stored in the account cache.
                return true;
            }

            var lstParam = new ParamList();
            lstParam.Add("TacticalAccount", strTacticalAddress);
            lstParam.Add("WebServiceAccesscode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/tactical/exists?format=json", lstParam);
            if (strResponse == null)
            {
                return true;
            }

            var objResponse = JsonSerializer<AccountTacticalExistsResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return false;
            }

            if (objResponse.Tactical)
            {
                // This tactical address is already registered.
                SetAccountCache(strTacticalAddress, true);
                return true;
            }

            return false;
        }

        public void TestServers()
        {
        }

        private void ServerDNSlookup(WinlinkServer objServer)
        {
            // 
            // Try to look up the IP addrewss(es) associated with a Winlink server.
            // 
            if (blnSatPhoneMode)
            {
                // DNS lookup may fail on sat phones, so use the default IP address.
                objServer.lstIP = new List<string>();
                objServer.lstIP.Add(objServer.strDefaultIP);
                return;
            }
            // 
            // See if we should set an invalid IP address for a CMS for testing.
            // 
            if ((objServer.strCity.ToUpper() ?? "") == (strForceBadIP ?? ""))
            {
                // Force an invalid IP address.
                objServer.lstIP[0] = "95.151.53.237";
            }
            else
            {
                objServer.lstIP = TranslateDomainName(objServer.strName);
                // 
                // See if we need to add a default IP address.
                // 
                if (!string.IsNullOrEmpty(objServer.strDefaultIP))
                {
                    if (!objServer.lstIP.Contains(objServer.strDefaultIP))
                    {
                        // 
                        // The default IP is not on the list, so we'll add it
                        // 
                        objServer.lstIP.Add(objServer.strDefaultIP);
                    }
                }
            }
            // 
            // Set the list of IP addresses and see if we got any.
            // 
            if (objServer.lstIP.Count > 0)
            {
                objServer.blnLookedUpDNS = true;
            }
            else
            {
                SetError("No IP addresses for " + objServer.strName);
            }

            return;
        }

        private void PublicDNSlookup(PublicServer objServer)
        {
            // 
            // Try to look up the IP addrewss(es) associated with a public server.
            // 
            objServer.lstIP = TranslateDomainName(objServer.strName);
            // 
            // Check the list of IP addresses and see if we got any.
            // 
            if (objServer.lstIP.Count > 0)
            {
                objServer.dttDNSLookup = DateTime.UtcNow;
            }
            else
            {
                SetError("No IP addresses for " + objServer.strName);
            }

            return;
        }

        public List<string> TranslateDomainName(string strDomainName)
        {
            // 
            // Translate a domain name to a list of IP addresses.
            // Return an empty list if we couldn't do the translation.
            // 
            var lstTmpIPAddr = new List<string>();
            if (strDNSServer is object && !string.IsNullOrEmpty(strDNSServer))
            {
                // 
                // Use IPworks to do the DNS lookup.
                // 
                try
                {
                    // Set the IP address of the DNS server we want to use.
                    if (strDNSServer.ToUpper().Trim() == "GOOGLE")
                    {
                        strDNSServer = "8.8.8.8";
                    }

                    var objDns = new LookupClient(IPAddress.Parse(strDNSServer));

                    // Try to do the DNS translation
                    var dnsResult = objDns.Query(strDomainName, QueryType.A);
                    var lstIPAddresses = dnsResult.Answers.ARecords();
                    foreach (var objRecord in lstIPAddresses)
                    {
                        if (!lstTmpIPAddr.Contains(objRecord.Address.ToString()))
                        {
                            // 
                            // Add the resolved IP if it is not already in the list
                            // 
                            lstTmpIPAddr.Add(objRecord.Address.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetError("[ServerDNSlookup] exception " + strDomainName + " " + ex.Message);
                }
            }
            else
            {
                // 
                // Use .NET function to do the DNS lookup.
                // 
                try
                {
                    var objHostEntry = Dns.GetHostEntry(strDomainName);
                    foreach (IPAddress objIP in objHostEntry.AddressList)
                    {
                        string strIP = objIP.ToString().Trim();
                        if (!string.IsNullOrEmpty(strIP) && lstTmpIPAddr.Contains(strIP) == false)
                        {
                            // 
                            // Add the resolved IP if it is not already in the list
                            // 
                            lstTmpIPAddr.Add(strIP);
                        }
                    }

                    objHostEntry = null;
                }
                catch (Exception ex)
                {
                    SetError("[ServerDNSlookup] exception " + strDomainName + " " + ex.Message);
                }
            }
            // 
            // Finished
            // 
            return lstTmpIPAddr;
        }

        private void SetError(string strError)
        {
            // 
            // Record an error, but don't overwrite an earlier error.
            // 
            if (string.IsNullOrEmpty(strLastError))
            {
                strLastError = strError;
            }

            return;
        }

        public void ClearError()
        {
            // 
            // Clear any pending error.
            // 
            strLastError = "";
            return;
        }

        private void SetFailedInternetSite(string strSite)
        {
            // 
            // Record the domain name of the last public web site that failed the Internet test.
            // 
            strLastFailedInternetSite = strSite;
            return;
        }

        private void ClearFailedInternetSite()
        {
            // 
            // Clear the domain name of the last public web sie that failed the Internet test.
            // 
            strLastFailedInternetSite = "";
            return;
        }

        public bool IsValidHamCallsign(string strCallsign)
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

        public bool IsUSHAMCallSign(string strCallsign)
        {
            // 
            // Return True if the callsign is a US ham callsign.
            // 
            var strExclude = new string[] { "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ" };
            int i;
            if (strCallsign == null)
                return false;
            strCallsign = Globals.BaseCallsign(strCallsign);
            int intLen = strCallsign.Length;
            if (intLen < 4 | intLen > 6)
                return false;
            if (IsValidSHARESCallsign(strCallsign))
                return false;
            if (IsValidMARSCallsign(strCallsign))
                return false;
            if (IsValidUKCadetCallsign(strCallsign))
                return false;
            // The first letter must be K, N, W or A.
            string strFirst = Conversions.ToString(strCallsign[0]);
            if (strFirst != "K" & strFirst != "N" & strFirst != "W" & strFirst != "A")
                return false;
            // Count digits
            int intDigits = 0;
            var loopTo = intLen - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (char.IsDigit(strCallsign[i]))
                    intDigits += 1;
            }

            if (intDigits != 1)
                return false;
            // Second or third character must be a digit
            if (!char.IsDigit(strCallsign[1]) & !char.IsDigit(strCallsign[2]))
                return false;
            // Check generally excluded callsigns
            var loopTo1 = strExclude.Length - 1;
            for (i = 0; i <= loopTo1; i++)
            {
                if ((strCallsign.Substring(0, 2) ?? "") == (strExclude[i] ?? ""))
                    return false;
            }

            if (char.IsDigit(strCallsign[2]) & intLen == 6)
            {
                // 2x3 callsigns
                if (strFirst == "A" || strFirst == "N")
                    return false;
            }

            if (char.IsDigit(strCallsign[2]) & intLen == 5)
            {
                // 2x2 callsigns
                if (strFirst == "A" && strCallsign[1] > 'L')
                    return false;
            }
            // 
            // This is a US ham callsign
            // 
            return true;
        }

        public bool IsValidUKCadetCallsign(string strCallsign)
        {
            strCallsign = strCallsign.Trim().ToUpper();
            if (strCallsign.Length < 4)
                return false;
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

        public bool IsSHARESorMARScallsign(string strCallsign)
        {
            return IsValidSHARESCallsign(strCallsign) | IsValidMARSCallsign(strCallsign);
        }

        public bool IsValidMARSCallsign(string strCallsign)
        {
            // 
            // Tests for MARS callsigns.
            // 
            strCallsign = strCallsign.Trim().ToUpper();
            if (strCallsign.Length < 4)
                return false;
            if (Globals.IsValidTacticalAddress(strCallsign))
                return false;
            if (IsValidSHARESCallsign(strCallsign))
                return false;
            if (strCallsign.Length == 6)
            {
                // Old Navy MARS callsigns
                if (strCallsign.StartsWith("NN") & !char.IsDigit(strCallsign[2]) & char.IsDigit(strCallsign[3]) & !AllDigits(strCallsign, 3, 4))
                    return true;
            }
            // Make sure callsign starts with valid letter
            if (Conversions.ToString(strCallsign[0]) != "N" & Conversions.ToString(strCallsign[0]) != "M" & Conversions.ToString(strCallsign[0]) != "A")
            {
                return false;
            }

            int i;
            var loopTo = strCallsign.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (!char.IsLetter(strCallsign[i]))
                    break;
            }

            if (i >= strCallsign.Length)
                return true;
            if (strCallsign.StartsWith("M"))
                return false; // Special case for UK Cadet callsigns
                              // Allow two letters followed by two digits followed by two letters.
            var objRegex = new Regex("^[A-Z][A-Z][0-9][0-9][A-Z][A-Z]");
            if (strCallsign.Length >= 6 && objRegex.IsMatch(strCallsign))
                return true;
            // Try three leading letters followed by a digit.
            objRegex = new Regex("^[A-Z][A-Z][A-Z][0-9]");
            return objRegex.IsMatch(strCallsign);
        } // IsMARSallsign

        public bool IsValidSHARESCallsign(string strCallsign)
        {
            // 
            // Returns True if the callsign is a SHARES callsign.
            // 
            strCallsign = strCallsign.Trim().ToUpper();
            if (strCallsign.Length < 4)
                return false;
            if (strCallsign.Length == 6)
            {
                if (strCallsign.StartsWith("NCS") & AllDigits(strCallsign, 3, 5))
                    return true;
                if (strCallsign.StartsWith("WGY") & AllDigits(strCallsign, 3, 5))
                    return true;
                if (strCallsign.StartsWith("NWL") & AllDigits(strCallsign, 3, 3))
                    return true;
                if (strCallsign.StartsWith("YSC") & AllDigits(strCallsign, 3, 3))
                    return true;  // Micronesia
                                  // Check for callsigns like NNA4TN
                var objRegexSHARES = new Regex("^NN[A-Z][0-9][A-Z][A-Z]");
                if (objRegexSHARES.IsMatch(strCallsign))
                    return true;
                if (strCallsign.StartsWith("NN") & char.IsLetter(strCallsign[2]) & char.IsDigit(strCallsign[3]) & char.IsLetter(strCallsign[4]) & char.IsLetter(strCallsign[5]))
                    return true;
            }

            if (strCallsign.Length >= 5 && strCallsign.StartsWith("KGD") & AllDigits(strCallsign, 3, 4))
                return true;
            if ((strCallsign.Length >= 5 && strCallsign.StartsWith("KNY")) & AllDigits(strCallsign, 3, 4))
                return true;
            return false;
        }

        public bool IsValidCallsign(string strCallsign)
        {
            // 
            // Checks the format of callsign.
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
            // at least one letter
            if (!Regex.IsMatch(parts[0], "[A-Z]"))
                return false;
            // if there is a -ssid, check it for value 1 to 15 or R, T, X for radio-only.
            if (parts.Length > 1)
            {
                // There is a SSID.  See if it's a radio-only SSID.
                if (parts[1] != "R" & parts[1] != "T" & parts[1] != "X")
                {
                    int id;
                    if (!int.TryParse(parts[1], out id))
                        return false;
                    if (id < 1 | id > 15)
                        return false;
                }
            }
            // all good
            return true;
        }

        public bool IsValidSSID(string strCallsign)
        {
            if (strCallsign.IndexOf("-") == -1)
                return true;
            if (strCallsign.EndsWith("-1") | strCallsign.EndsWith("-2") | strCallsign.EndsWith("-3") | strCallsign.EndsWith("-4") | strCallsign.EndsWith("-5") | strCallsign.EndsWith("-6") | strCallsign.EndsWith("-7") | strCallsign.EndsWith("-8") | strCallsign.EndsWith("-9") | strCallsign.EndsWith("-10") | strCallsign.EndsWith("-11") | strCallsign.EndsWith("-12") | strCallsign.EndsWith("-13") | strCallsign.EndsWith("-14") | strCallsign.EndsWith("-15"))
                return true;
            // accept any -ssid of G-Z as well 
            int intIndex = strCallsign.IndexOf("-");
            if ("GHIJKLMNOPQRSTUVWXYZ".IndexOf(strCallsign.Substring(1 + intIndex).ToUpper()) != -1)
                return true;
            return false;
        }

        public bool IsSHARESorMARSServiceCode(string strServiceCode)
        {
            return IsSHARESServiceCode(strServiceCode) | IsMARSServiceCode(strServiceCode);
        }

        public bool IsHamServiceCode(string strServiceCode)
        {
            // 
            // Return True if a set of service codes contains the PUBLIC or EMCOMM ham service code.
            // 
            if (Information.IsNothing(strServiceCode))
                return false;
            var aryCodes = strServiceCode.Trim().ToUpper().Split(' ');
            foreach (string strCode in aryCodes)
            {
                if (strCode == "PUBLIC" | strCode == "EMCOMM")
                    return true;
            }

            return false;
        }

        public bool IsSHARESServiceCode(string strServiceCode)
        {
            // 
            // Return True if a set of service codes contains the SHARES service code.
            // 
            if (Information.IsNothing(strServiceCode))
                return false;
            var aryCodes = strServiceCode.Trim().ToUpper().Split(' ');
            foreach (string strCode in aryCodes)
            {
                if (strCode == "SHARES121013" | strCode == "SHARES121213")
                    return true;
            }

            return false;
        }

        public bool IsMARSServiceCode(string strServiceCode)
        {
            if (Information.IsNothing(strServiceCode))
                return false;
            var aryCodes = strServiceCode.Trim().ToUpper().Split(' ');
            foreach (string strCode in aryCodes)
            {
                if (strCode == "MARS211576")
                    return true;
            }

            return false;
        }

        private bool AllDigits(string strInput, int intStart, int intEnd)
        {
            // 
            // Return True if the characters in the specified range are all digits.  Indexes are 0 based.
            // 
            if (strInput.Length <= intEnd)
                return false;
            for (int i = intStart, loopTo = intEnd; i <= loopTo; i++)
            {
                if (!char.IsDigit(strInput[i]))
                    return false;
            }

            return true;
        }

        public List<Enumerations.PasswordEntry> DownloadPasswords(string strSiteCallsign, string strChallenge, bool blnSkipCMS = false, bool blnSecureLogonOnly = false, string strForceCMSArg = "")
        {
            // 
            // Download the callsigns and password hashcodes for users set for secure login.
            // 
            ClearError();
            // 
            // Try to get the list of password hashcodes.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Challenge", strChallenge);
            lstParam.Add("WebServiceAccesscode", strPasswordAccessCode);
            string strResponse = JsonCommand("/passwords/list?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<PasswordsListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            // 
            // We got password hashcodes.  Set up a list to return.
            // 
            var lstPasswords = new List<Enumerations.PasswordEntry>();
            Enumerations.PasswordEntry objPassword;
            foreach (PasswordHashRecord objPasswordRecord in objResponse.PasswordHash)
            {
                string strCallsign = objPasswordRecord.Callsign.Trim().ToUpper();
                string strPasswordHash = objPasswordRecord.PasswordHash;
                if (!string.IsNullOrEmpty(strCallsign) & !string.IsNullOrEmpty(strPasswordHash))
                {
                    objPassword = new Enumerations.PasswordEntry();
                    objPassword.strCallsign = strCallsign;
                    objPassword.strPasswordHash = strPasswordHash;
                    objPassword.strChallenge = strChallenge;
                    lstPasswords.Add(objPassword);
                }
            }
            // 
            // Finished
            // 
            return lstPasswords;
        }

        public List<GroupAddressRecord> DownloadGroupAddresses()
        {
            // 
            // Download the list of group addresses.
            // 
            ClearError();
            // 
            // Try to get the list of group addresses.
            // 
            string strResponse = JsonCommand("/groupAddress/list?format=json");
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<GroupAddressListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                {
                    if (objResponse is object)
                        ReportError(objResponse.ErrorMessage);
                }

                return null;
            }
            // 
            // Finished
            // 
            return objResponse.AddressList;
        }

        public List<GatewayStatusRecord> GatewayChannelList(bool blnPacketChannels, string strServiceCodes, string strCaller, string strForceCMSArg = "")
        {
            // 
            // Get a list of the available gateway channels, and return it as a List(Of GatewayStatusRecord).
            // Return Nothing if we can't get the list.
            // 
            // Send the request.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Mode", Conversions.ToString(GatewayOperatingMode.AnyAll));
            lstParam.Add("HistoryHours", "168");     // 7 days
            lstParam.Add("ServiceCodes", strServiceCodes);
            string strResponse = JsonCommand("/gateway/status?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            strResponse = CleanNonAscii(strResponse);
            strResponse = objModeEnum.Convert(strResponse);
            strResponse = objRequestedModeEnum.Convert(strResponse);
            var objResponse = JsonSerializer<GatewayStatusResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            // 
            // Select either HF or VHF/UHF channels.
            // 
            var lstGateways = new List<GatewayStatusRecord>();
            GatewayStatusRecord objNewStation;
            bool blnUseChannel;
            foreach (GatewayStatusRecord ObjStation in objResponse.Gateways)
            {
                if (ObjStation.HoursSinceStatus < 5 * 24)
                {
                    // Create a new GatewayStatusRecord
                    objNewStation = new GatewayStatusRecord();
                    objNewStation.BaseCallsign = ObjStation.BaseCallsign.Trim().ToUpper();
                    objNewStation.Callsign = ObjStation.Callsign.Trim().ToUpper();
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

        public MeshNodeMasterRecord GetMeshNodeMasterRecord(string strHostIp = "")
        {
            // 
            // Get the Master record for a MESH node.
            // 
            string strResponse = JsonMeshCommand("hosts=1", strHostIp);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<MeshNodeMasterRecord>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse;
        }

        public MeshNodeServicesRecord GetMeshNodeServicesRecord(string strHostIp = "")
        {
            // 
            // Get the Master record for a MESH node.
            // 
            string strResponse = JsonMeshCommand("services=1", strHostIp);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<MeshNodeServicesRecord>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse;
        }

        public string GetMeshNodeServicesJson(string strHostIp = "")
        {
            // 
            // Return the raw Json code for the advertised services.
            // 
            string strResponse = JsonMeshCommand("services=1", strHostIp);
            if (strResponse == null)
            {
                return "(Unable to obtain list of services from " + strHostIp + ")";
            }

            return strResponse;
        }

        public string AccountAdd(string strCallsign, string strPassword)
        {
            // 
            // Add an authorized callsign.
            // 
            // Check if the account already exists in the Winlink system.
            // 
            string strResult;
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            bool blnTacticalAddress = IsTacticalAddress(strCallsign);
            if (!IsValidCallsign(strBaseCallsign) & !blnTacticalAddress)
            {
                return "The specified callsign Is Not valid";
            }

            if (!string.IsNullOrEmpty(strPassword))
            {
                strResult = Globals.CheckPasswordSyntax(strPassword);
                if (!string.IsNullOrEmpty(strResult))
                {
                    return strResult;
                }
            }
            // 
            // Check the system to see if this account is known.
            // 
            if (blnTacticalAddress)
            {
                if (TacticalAddressExists(strCallsign))
                {
                    return "Tactical address already registered with Winlink";
                }
            }
            else
            {
                var enmStatus = AccountExists(strCallsign, strCallsign);
                if (enmStatus == Enumerations.AccountValidationCodes.NoInternet)
                {
                    return "Cannot add account, because there Is no connection available ot any CMS.";
                }

                if (enmStatus == Enumerations.AccountValidationCodes.Valid)
                {
                    return "Callsign Is already registered with Winlink";
                }
            }
            // 
            // This callsign is not currently registered, so register it now.
            // Validate the password.
            // 
            if (!string.IsNullOrEmpty(strPassword))
            {
                strResult = Globals.CheckPasswordSyntax(strPassword);
                if (!string.IsNullOrEmpty(strResult))
                {
                    return strResult;
                }
            }
            // 
            // Create an entry for the callsign in the Callsigns table.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/add?format=json", lstParam);
            if (strResponse == null)
            {
                return "Unable to connect to a Winlink server";
            }

            var objResponse = JsonSerializer<AccountAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error adding Winlink account";
            }
            // 
            // Finished
            // 
            return "";
        }

        public string AccountRemove(object strSystemCallsign, object strCallsign)
        {
            // 
            // Remove a callsign from the system.
            // 
            if (!(IsValidCallsign(Conversions.ToString(strCallsign)) | Globals.IsValidTacticalAddress(Conversions.ToString(strCallsign), 12)))
            {
                return "The specified callsign Is Not valid";
            }
            // 
            // Remove the account.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", Conversions.ToString(strCallsign));
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/remove?format=json", lstParam);
            if (strResponse == null)
            {
                return "Unable to connect to a Winlink server";
            }

            var objResponse = JsonSerializer<AccountRemoveResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error removing Winlink account";
            }

            SetAccountCache(Globals.BaseCallsign(Conversions.ToString(strCallsign)), false);
            return "";
        }

        public string SysopAdd(string strCallsign, string strPassword, Enumerations.SysopInfo objSysopArg)
        {
            // 
            // Add information about a user with arguments passed in as an object.
            // 
            return SetSysopInfo(strCallsign, strPassword, objSysopArg.City, objSysopArg.Comments, objSysopArg.Country, objSysopArg.Email, objSysopArg.GridSquare, objSysopArg.Phones, objSysopArg.PostalCode, objSysopArg.State, objSysopArg.StreetAddress1, objSysopArg.StreetAddress2, objSysopArg.SysopName, objSysopArg.Website);
        }

        public string SetSysopInfo(string strCallsign, string strPassword, string strCity, string strAdditionalData, string strCountry, string strEmail, string strGridSquare, string strPhoneNumber, string strPostalCode, string strState, string strAddress1, string strAddress2, string strSysopName, string strWebSiteUrl)
        {
            // 
            // Send the user's contact info to the system.
            // 
            if (string.IsNullOrEmpty(strCallsign))
            {
                return "Missing callsign";
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return "Invalid or missing password";
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return "Incorrect password";
            }

            var lstParam = new ParamList();
            lstParam.Add("Callsign", Globals.BaseCallsign(strCallsign));
            lstParam.Add("Password", strPassword);
            lstParam.Add("City", Globals.CleanArg(strCity));
            lstParam.Add("Comments", Globals.CleanArg(strAdditionalData));
            lstParam.Add("Country", Globals.CleanArg(strCountry));
            lstParam.Add("Email", Globals.CleanArg(strEmail));
            lstParam.Add("GridSquare", Globals.CleanArg(strGridSquare));
            lstParam.Add("Phones", Globals.CleanArg(strPhoneNumber));
            lstParam.Add("PostalCode", Globals.CleanArg(strPostalCode));
            lstParam.Add("State", Globals.CleanArg(strState));
            lstParam.Add("StreetAddress1", Globals.CleanArg(strAddress1));
            lstParam.Add("StreetAddress2", Globals.CleanArg(strAddress2));
            lstParam.Add("SysopName", Globals.CleanArg(strSysopName));
            lstParam.Add("Website", Globals.CleanArg(strWebSiteUrl));
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            string strResponse = JsonCommand("/sysop/add?format=json", lstParam);
            if (strResponse == null)
            {
                return "Unable to connect to a Winlink server";
            }

            var objResponse = JsonSerializer<SysopAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                if (objResponse is object)
                {
                    return "Error setting contact information for callsign. " + objResponse.ResponseStatus.ErrorCode;
                }
                else
                {
                    return "Error setting contact information for callsign";
                }
            }
            // 
            // Finished
            // 
            return "";
        }

        public string RegisterSysopCallsign(string strCallsign, string strPassword, string strCity, string strAdditionalData, string strCountry, string strEmail, string strGridSquare, string strPhoneNumber, string strPostalCode, string strState, string strAddress1, string strAddress2, string strSysopName, string strWebSiteUrl)
        {
            // 
            // If there isn't already an entry in the Callsigns CMS table for this callsign, make one.
            // Return an error message or blank for success.
            // 
            string strResult;
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            if (!IsValidCallsign(strBaseCallsign))
            {
                return "The specified callsign Is Not valid";
            }

            if (!string.IsNullOrEmpty(strPassword))
            {
                strResult = Globals.CheckPasswordSyntax(strPassword);
                if (!string.IsNullOrEmpty(strResult))
                {
                    return strResult;
                }
            }
            // 
            // Check if the callsign is already registered.
            // 
            var enmStatus = AccountExists(strCallsign, strCallsign);
            if (enmStatus == Enumerations.AccountValidationCodes.NoInternet)
            {
                return "Cannot register sysop information, because there Is no access to any CMS.";
            }

            if (enmStatus != Enumerations.AccountValidationCodes.Valid)
            {
                // 
                // This callsign is not currently registered, so register it now.
                // Create an entry for the callsign in the Callsigns table.
                // 
                strResult = AccountAdd(strCallsign, strPassword);
                if (!string.IsNullOrEmpty(strResult))
                    return strResult;
            }
            // 
            // Now add a record to the SysopRecords table.
            // 
            strResult = SetSysopInfo(strBaseCallsign, strPassword, strCity, strAdditionalData, strCountry, strEmail, strGridSquare, strPhoneNumber, strPostalCode, strState, strAddress1, strAddress2, strSysopName, strWebSiteUrl);
            if (!string.IsNullOrEmpty(strResult))
                return strResult;
            // 
            // Finished
            // 
            return "";
        }

        public SortedDictionary<string, SysopRecord> GetSysops()
        {
            // 
            // Get information about all sysops.
            // 
            string strResponse = JsonCommand("/sysop/list?format=json");
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<SysopListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            // 
            // We got a list of sysop records.  Build a sorted dictionary of sysop info.  The key is the base callsign.
            // 
            var dicSysop = new SortedDictionary<string, SysopRecord>();
            foreach (SysopRecord objSysop in objResponse.SysopList)
                dicSysop.Add(objSysop.Callsign, objSysop);
            // 
            // Finished
            // 
            return dicSysop;
        }

        public List<InquiryRecord> UpdateCatalogList()
        {
            // 
            // Download the latest catalog list.
            // 
            string strResponse = JsonCommand("/inquiries/catalog?format=json");
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<InquiriesCatalogResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }
            // 
            // Finished
            // 
            return objResponse.Inquiries;
        }

        public SysopRecord GetSysopInfo(string strBaseCallsign, string strPassword)
        {
            // 
            // Get information about an account.
            // 
            if (AccountRegistered(strBaseCallsign) == false)
            {
                return null;
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return null;
            }

            if (!ValidatePassword(strBaseCallsign, strPassword))
            {
                return null;
            }

            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strPassword);
            string strResponse = JsonCommand("/sysop/get?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<SysopGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse.Sysop;
        }

        public string TrafficAdd(TrafficAdd objLogging)
        {
            // 
            // Add logging for a new message.
            // 
            if (!AccountRegistered(objLogging.Callsign))
            {
                return "Error adding traffic to CMS: The callsign Is Not valid";
            }

            if (objLogging.Mime == null || string.IsNullOrEmpty(objLogging.Mime))
            {
                return "Error adding traffic to CMS: No Mime parameter";
            }
            // 
            // Check that the MIME file isn't too big.
            // 
            if (objLogging.Mime.Length >= 120000)
            {
                return "Error adding traffic to CMS: Size of MIME file exceeds 120 kb";
            }
            // 
            // Set up the parameters.
            // 
            string strMessageId = objLogging.MessageId;
            int intAt = strMessageId.IndexOf('@');
            if (intAt > 0)
                strMessageId = strMessageId.Substring(0, intAt);
            var lstParam = new ParamList();
            lstParam.Add("Callsign", objLogging.Callsign);
            lstParam.Add("Client", objLogging.Client.ToString());
            lstParam.Add("MessageId", strMessageId);
            lstParam.Add("Timestamp", Globals.DateToRFC822Date(objLogging.TimeStamp));
            lstParam.Add("TrafficType", objLogging.TrafficType.ToString());
            lstParam.Add("Event", objLogging.Event.ToString());
            lstParam.Add("Frequency", objLogging.Frequency.ToString());
            lstParam.Add("Mime", objLogging.Mime);
            // 
            // Execute the function.
            // 
            string strResponse = JsonCommand("/traffic/add?format=json", lstParam);
            var objResponse = JsonSerializer<TrafficAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error adding traffic to CMS: " + objResponse.ErrorMessage;
            }
            // 
            // Finished.
            // 
            return "";
        }

        public string AccountPasswordRecoveryEmailSet(string strCallsign, string strPassword, string strRecoveryEmail, bool blnNeedToCheckPassword = true)
        {
            // 
            // Register the password recovery email for an account.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            if (!AccountRegistered(strBaseCallsign))
            {
                return "The callsign Is Not valid";
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return "Invalid or missing password";
            }

            string strWebServiceUrl = GetWebServiceURL(false, strBaseCallsign);
            if (string.IsNullOrEmpty(strWebServiceUrl))
            {
                return "Cannot connect to a CMS";
            }
            // 
            // Verify that the account exists and they know the right password.
            // 
            if (blnNeedToCheckPassword)
            {
                var enmResult = AccountPasswordValidate(strCallsign, strPassword, strWebServiceUrl);
                if (enmResult == Enumerations.AccountValidationCodes.NoInternet)
                {
                    return "Cannot set password recovery e-mail address, because you aren't connected to the Internet.";
                }

                if (enmResult != Enumerations.AccountValidationCodes.SecureLogon & enmResult != Enumerations.AccountValidationCodes.Valid)
                {
                    // Invalid password.  Don't allow the password recovery e-mail to be changed.
                    return "Cannot set password recovery e-mail address, because the password for this account is not correct.";
                }
            }
            // 
            // See if the password recovery e-mail is already set correctly.
            // 
            strRecoveryEmail = strRecoveryEmail.Trim();
            string strOldRecoveryEmail = AccountPasswordRecoveryEmailGet(strBaseCallsign, strPassword);
            if ((strOldRecoveryEmail.ToLower() ?? "") == (strRecoveryEmail.ToLower() ?? ""))
            {
                // Don't need to change the e-mail
                return "";
            }
            // 
            // Set the password recovery e-mail.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("RecoveryEmail", strRecoveryEmail);
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/password/recovery/email/set?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error setting recovery e-mail: Cannot connect to Winlink server";
            }

            var objResponse = JsonSerializer<AccountPasswordRecoveryEmailSetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error setting password recovery e-mail address";
            }
            // 
            // Finished.
            // 
            return "";
        }

        public string AccountPasswordRecoveryEmailGet(string strCallsign, string strPassword)
        {
            // 
            // Get the password recovery email for an account.  An empty string is returned if there is no registered recovery email.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            if (!AccountRegistered(strBaseCallsign))
            {
                return "The callsign is not valid";
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return "Invalid or missing password";
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return "Incorrect password for account";
            }
            // 
            // Get the password recovery e-mail.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            string strResponse = JsonCommand("/account/password/recovery/email/get?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error getting recovery e-mail: Cannot connect to Winlink server";
            }

            var objResponse = JsonSerializer<AccountPasswordRecoveryEmailGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error getting password recovery e-mail address";
            }

            return objResponse.RecoveryEmail.Trim();
        } // AccountRecoveryEmailSet

        public string AccountPasswordSend(string strCallsign)
        {
            // 
            // Request that an e-mail with the password be sent to the password recovery e-mail address.
            // 
            string strBaseCallsign = Globals.BaseCallsign(strCallsign);
            if (!IsValidCallsign(strBaseCallsign))
            {
                return "The callsign is not valid";
            }

            if (!AccountRegistered(strBaseCallsign))
            {
                return "Callsign is not registered with Winlink system: " + strBaseCallsign;
            }
            // 
            // Check if we have an Internet connection.
            // 
            if (!HaveInternetConnection())
            {
                return "Your request could not be completed, because you currently are not connected to the Internet.  You must connect to the Internet or request the password recovery through the www.winlink.org web site.";
            }
            // 
            // Request the password.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", strBaseCallsign);
            string strResponse = JsonCommand("/account/password/send?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error sending recovery e-mail: Cannot connect to Winlink server";
            }

            var objResponse = JsonSerializer<AccountPasswordSendResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error sending password recovery e-mail address";
            }

            return "";
        }

        public string MessageSend(string strMessageId, string strMime)
        {
            // 
            // Send a MIME encoded message through a CMS.
            // 
            strMime = strMime.Replace("smtp:", "").Replace("SMTP:", "");      // Remove "smtp:" from any e-mail addresses.
            if (strMime.Length >= 120000)
            {
                return "Error sending message: Message is too large to send to a CMS";
            }

            var lstParam = new ParamList();
            lstParam.Add("MessageId", strMessageId);
            lstParam.Add("Mime", strMime);
            lstParam.Add("Message", strMime);
            lstParam.Add("WebServiceAccessCode", strMessageAccessCode);
            string strResponse = JsonCommand("/message/send?format=json", lstParam);
            if (strResponse == null)
            {
                return "Error sending e-mail: Cannot connect to Winlink server";
            }

            var objResponse = JsonSerializer<MessageSendResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "Error sending message: " + objResponse.ResponseStatus.ErrorCode;
            }

            return "";
        }

        public string SendEmailMessage(string strFrom, string strTo, string strSubject, string strBody)
        {
            // 
            // Do a simple e-mail send to a list of recipients separated by commas or semicolons with no attachments.
            // 
            // Check message size
            // 
            if (strBody.Length >= 120000)
            {
                return "Error sending message: Message is too large to send to a CMS";
            }
            // 
            // Send a message to each recipient.
            // 
            strTo = strTo.Replace(',', ';');
            var aryTok = strTo.Split(';');
            foreach (string strRecipient in aryTok)
            {
                // 
                // Send the message.
                // 
                var lstParam = new ParamList();
                lstParam.Add("From", strFrom.Trim().Replace("smtp:", "").Replace("SMTP:", ""));
                lstParam.Add("To", strRecipient.Trim().Replace("smtp:", "").Replace("SMTP:", ""));
                lstParam.Add("Subject", strSubject.Trim());
                lstParam.Add("Body", strBody);
                lstParam.Add("WebServiceAccessCode", strMessageAccessCode);
                string strResponse = JsonCommand("/message/send/simple?format=json", lstParam);
                if (strResponse == null)
                {
                    return "Error sending e-mail: Cannot connect to Winlink server";
                }

                var objResponse = JsonSerializer<MessageSendSimpleResponse>.DeSerialize(strResponse);
                if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
                {
                    if (objResponse is object)
                        ReportError(objResponse.ErrorMessage);
                    return "Error sending message";
                }
            }

            return "";
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public List<MessageListRecord> MessageList(string strCallsign, string strPassword)
        {
            // 
            // Get a list of pending messages for a callsign.
            // 
            if (IsValidCallsign(strCallsign) == false & IsTacticalAddress(strCallsign) == false)
            {
                ReportError("Not valid callsign or tactical address");
                return null;
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                ReportError("Not valid password");
                return null;
            }

            if (AccountRegistered(strCallsign) == false)
            {
                ReportError("Account is not registered with Winlink: " + strCallsign);
                return null;
            }

            if (ValidatePassword(strCallsign, strPassword) == false)
            {
                ReportError("Incorrect password specified for Winlink account");
                return null;
            }

            var lstParam = new ParamList();
            lstParam.Add("AccountName", strCallsign);
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            string strResponse = JsonCommand("/message/list?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<MessageListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                ReportError(objResponse.ResponseStatus.ErrorCode);
                return null;
            }

            return objResponse.Messages;
        }

        public MessageGetDecodedResponse MessageGetDecoded(string strCallsign, string strPassword, string strMessageId, bool blnMarkAsForwarded = false)
        {
            // 
            // Get and decode a message stored on a CMS.
            // 
            if (AccountRegistered(strCallsign) == false)
            {
                return null;
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return null;
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return null;
            }

            var lstParam = new ParamList();
            lstParam.Add("AccountName", strCallsign);
            lstParam.Add("Callsign", strCallsign);
            lstParam.Add("Password", strPassword);
            lstParam.Add("MessageId", strMessageId);
            lstParam.Add("MarkAsForwarded", Conversions.ToString(blnMarkAsForwarded));
            lstParam.Add("WebServiceAccessCode", strMessageAccessCode);
            string strResponse = JsonCommand("/message/get/decoded?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<MessageGetDecodedResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse;
        }

        public bool MessageDelete(string strMessageId, bool blnCompleteDeletion = false)
        {
            // 
            // Delete the specified message from a CMS.
            // 
            var lstParam = new ParamList();
            lstParam.Add("MessageId", strMessageId);
            lstParam.Add("CompleteDeletion", Conversions.ToString(blnCompleteDeletion));
            lstParam.Add("WebServiceAccessCode", strMessageAccessCode);
            string strResponse = JsonCommand("/message/delete?format=json", lstParam);
            if (strResponse == null)
            {
                return false;
            }

            var objResponse = JsonSerializer<MessageDeleteResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return false;
            }

            return true;
        }

        public bool MessageOnCMS(string strMessageId)
        {
            // 
            // Return True if a message with the specified ID is stored on a CMS.
            // 
            var lstParam = new ParamList();
            lstParam.Add("MessageId", strMessageId);
            lstParam.Add("WebServiceAccessCode", strMessageAccessCode);
            string strResponse = JsonCommand("/message/exists?format=json", lstParam);
            if (strResponse == null)
            {
                return false;
            }

            var objResponse = JsonSerializer<MessageExistsResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return false;
            }

            return objResponse.Exists;
        }

        public List<MessagePickupStationRecord> GetAllMPS(object strRequester)
        {
            // 
            // Get a list of all MPS assignments for all callsigns.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Requester", Conversions.ToString(strRequester));
            lstParam.Add("WebServiceAccessCode", strMPSAccessCode);
            string strResponse = JsonCommand("/mps/list?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<MessagePickupStationListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse.MpsList;
        }

        public List<MessagePickupStationRecord> GetAllMPSForCallsign(string strRequester, string strUserCallsign)
        {
            // 
            // Get a list of all MPS assignments for a specified callsign.
            // 
            if (!AccountRegistered(strUserCallsign))
            {
                return null;
            }

            var lstParam = new ParamList();
            lstParam.Add("Requester", strRequester);
            lstParam.Add("Callsign", Globals.BaseCallsign(strUserCallsign));
            lstParam.Add("WebServiceAccessCode", strAccountAccessCode);
            string strResponse = JsonCommand("/mps/get?format=json", lstParam);
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<MessagePickupStationGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse.MpsList;
        }

        public bool DeleteAllMPSForCallsign(string strRequester, string strUserCallsign, string strUserPassword, bool blnUseFullCallsign = false)
        {
            // 
            // Delete all MPS assignments for a specified callsign.
            // 
            string strBaseCallsign;
            if (Globals.IsValidTacticalAddress(strUserCallsign) | blnUseFullCallsign)
            {
                strBaseCallsign = strUserCallsign;
            }
            else
            {
                strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
                if (!AccountRegistered(strBaseCallsign))
                {
                    return false;
                }
            }

            if (!ValidatePassword(strBaseCallsign, strUserPassword))
            {
                return Conversions.ToBoolean("Incorrect password for account");
            }
            // 
            // Send the request.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Requester", strRequester);
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strUserPassword);
            lstParam.Add("WebServiceAccessCode", strMPSAccessCode);
            string strResponse = JsonCommand("/mps/delete?format=json", lstParam);
            if (strResponse == null)
            {
                return false;
            }

            var objResponse = JsonSerializer<MessagePickupStationDeleteResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return false;
            }

            DeleteRecentNetworkParameter(strBaseCallsign, "MPS");
            return true;
        }

        public bool AddMPSEntry(string strRequester, string strUserCallsign, string strUserPassword, string strMPSCallsign)
        {
            // 
            // Add or update an MPS associated with a callsign.
            // 
            string strBaseCallsign;
            if (Globals.IsValidTacticalAddress(strUserCallsign))
            {
                strBaseCallsign = strUserCallsign;
            }
            else
            {
                strBaseCallsign = Globals.BaseCallsign(strUserCallsign);
                if (!AccountRegistered(strBaseCallsign))
                {
                    return false;
                }
            }

            string strBaseMPSCallsign = Globals.BaseCallsign(strMPSCallsign);
            if (!AccountRegistered(strBaseMPSCallsign))
            {
                return false;
            }

            if (!ValidatePassword(strBaseCallsign, strUserPassword))
            {
                return false;
            }
            // 
            // See if this is a duplicate of a recent entry.
            // 
            if (CheckRecentNetworkParameter(strBaseCallsign, "MPS", strMPSCallsign))
            {
                // Duplicate of recent entry
                return true;
            }

            var lstParam = new ParamList();
            lstParam.Add("Requester", strRequester);
            lstParam.Add("Callsign", strBaseCallsign);
            lstParam.Add("Password", strUserPassword);
            lstParam.Add("MpsCallsign", strBaseMPSCallsign);
            lstParam.Add("WebServiceAccessCode", strMPSAccessCode);
            string strResponse = JsonCommand("/mps/add?format=json", lstParam);
            if (strResponse == null)
            {
                return false;
            }

            var objResponse = JsonSerializer<MessagePickupStationAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return false;
            }

            AddRecentNetworkParameter(strBaseCallsign, "MPS", strBaseMPSCallsign);
            return true;
        }

        /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        public List<ParamRecord> GetAllNetworkParameters()
        {
            // 
            // Get all network parameters.
            // 
            string strResponse = JsonCommand("/radioNetwork/params/list?format=json");
            if (strResponse == null)
            {
                return null;
            }

            var objResponse = JsonSerializer<RadioNetworkParamsListResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return null;
            }

            return objResponse.ParamList;
        }

        public bool SetNetworkParameter(string strCallsign, string strPassword, string strParam, string strValue)
        {
            // 
            // Set a network parameter for a callsign.
            // 
            if (IsValidCallsign(strCallsign) == false | string.IsNullOrEmpty(strParam))
            {
                return false;
            }

            if (!AccountRegistered(strCallsign))
            {
                return false;
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return false;
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return false;
            }
            // 
            // Don't send a duplicate of a report we've sent recently.
            // 
            if (CheckRecentNetworkParameter(strCallsign, strParam, strValue))
            {
                // We sent the same parameter recently.
                return true;
            }
            // 
            // Set up arguments.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", Globals.BaseCallsign(strCallsign));
            lstParam.Add("Password", strPassword);
            lstParam.Add("Param", strParam);
            lstParam.Add("Value", strValue);
            string strResponse = JsonCommand("/radioNetwork/params/add?format=json", lstParam);
            if (strResponse == null)
            {
                return false;
            }

            var objResponse = JsonSerializer<RadioNetworkParamsAddResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return false;
            }
            // Success.  Add a cache entry for this parameter
            AddRecentNetworkParameter(strCallsign, strParam, strValue);
            return true;
        }

        public string GetNetworkParameter(string strCallsign, string strPassword, string strParam)
        {
            // 
            // Get a network parameter for a callsign.
            // 
            if (IsValidCallsign(strCallsign) == false | string.IsNullOrEmpty(strParam))
            {
                return "---Invalid callsign";
            }

            if (!AccountRegistered(strCallsign))
            {
                return "---Account not registered";
            }

            if (Globals.IsValidPassword(strPassword) == false)
            {
                return "---Invalid password";
            }

            if (!ValidatePassword(strCallsign, strPassword))
            {
                return "---Invalid password";
            }
            // 
            // Set up arguments.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Callsign", Globals.BaseCallsign(strCallsign));
            lstParam.Add("Password", strPassword);
            lstParam.Add("Param", strParam);
            string strResponse = JsonCommand("/radioNetwork/params/get?format=json", lstParam);
            if (strResponse == null)
            {
                return "---No response from CMS";
            }

            var objResponse = JsonSerializer<RadioNetworkParamsGetResponse>.DeSerialize(strResponse);
            if (objResponse == null || objResponse.HasError | !string.IsNullOrEmpty(objResponse.ResponseStatus.ErrorCode))
            {
                if (objResponse is object)
                    ReportError(objResponse.ErrorMessage);
                return "---" + objResponse.ErrorMessage;
            }

            string strValue = "";
            if (objResponse.ParamList.Count > 0)
            {
                var objParamRecord = objResponse.ParamList[0];
                strValue = objParamRecord.Value;
            }
            else
            {
                strValue = "";
            }
            // Success.  Add a cache entry for this parameter
            AddRecentNetworkParameter(strCallsign, strParam, strValue);
            return strValue;
        }

        public bool CheckRecentNetworkParameter(string strCallsign, string strParam, string strValue)
        {
            // 
            // Return True if a network parameter is a duplicate of one posted recently.
            // 
            bool blnResult = false;
            // Remove any expired entries.
            ExpireRecentNetworkParameter();
            // Check for the new parameter
            string strKey = strCallsign + ":" + strParam;
            lock (objNetworkParamCacheLock)
            {
                if (dicNetworkParamCache.ContainsKey(strKey))
                {
                    var objCache = dicNetworkParamCache[strKey];
                    if ((objCache.strValue ?? "") == (strValue ?? "") & (DateTime.UtcNow - objCache.dttPosted).TotalHours < intNetworkParamCacheExpire)
                    {
                        blnResult = true;
                    }
                }
            }

            return blnResult;
        }

        public void AddRecentNetworkParameter(string strCallsign, string strParam, string strValue)
        {
            // 
            // Add an entry to the network parameter cache.
            // 
            // Remove any expired entries.
            // 
            ExpireRecentNetworkParameter();
            // 
            // See if this duplicates an existing entry
            // 
            if (CheckRecentNetworkParameter(strCallsign, strParam, strValue) == false)
            {
                // 
                // This is a new entry. Delete any old entry with the same parameter name regardless of value.
                // 
                string strKey = strCallsign + ":" + strParam;
                lock (objNetworkParamCacheLock)
                {
                    if (dicNetworkParamCache.ContainsKey(strKey))
                    {
                        dicNetworkParamCache.Remove(strKey);
                    }
                    // 
                    // Add the new entry
                    // 
                    var objCache = new NetworkParamCacheEntry();
                    objCache.strCallsign = strCallsign;
                    objCache.strParam = strParam;
                    objCache.strValue = strValue;
                    objCache.dttPosted = DateTime.UtcNow;
                    dicNetworkParamCache.Add(strKey, objCache);
                }
            }

            return;
        }

        private void DeleteRecentNetworkParameter(string strCallsign, string strParam = "*")
        {
            // 
            // Delete any matching network parameter cache entry.
            // 
            lock (objNetworkParamCacheLock)
            {
                if (strParam == "*")
                {
                    // Delete all for this callsign
                    var lstRemove = new List<string>();
                    foreach (KeyValuePair<string, NetworkParamCacheEntry> kvp in dicNetworkParamCache)
                    {
                        var objCache = kvp.Value;
                        if ((objCache.strCallsign ?? "") == (strCallsign ?? ""))
                        {
                            lstRemove.Add(kvp.Key);
                        }
                    }

                    foreach (var strKey in lstRemove)
                        dicNetworkParamCache.Remove(strKey);
                }
                else
                {
                    // Delete a specific parameter for this callsign.
                    string strKey = strCallsign + ":" + strParam;
                    if (dicNetworkParamCache.ContainsKey(strKey))
                    {
                        dicNetworkParamCache.Remove(strKey);
                    }
                }
            }

            return;
        }

        private void ExpireRecentNetworkParameter()
        {
            // 
            // Expire and delete any recent network parameter cache entries.
            // 
            if ((DateTime.UtcNow - dttLastNetworkParamCacheExpire).TotalMinutes < 10)
                return;
            // 
            // Time to scan the cache list and make a list of entries to delete.
            // 
            dttLastNetworkParamCacheExpire = DateTime.UtcNow;
            var lstRemove = new List<string>();
            lock (objNetworkParamCacheLock)
            {
                foreach (KeyValuePair<string, NetworkParamCacheEntry> kvp in dicNetworkParamCache)
                {
                    var objCache = kvp.Value;
                    if ((DateTime.UtcNow - objCache.dttPosted).TotalHours >= intNetworkParamCacheExpire)
                    {
                        lstRemove.Add(kvp.Key);
                    }
                }
                // 
                // Delete expired entries.
                // 
                foreach (var strKey in lstRemove)
                    dicNetworkParamCache.Remove(strKey);
            }

            return;
        }

        public bool ValidateServiceCodes(string strCodes)
        {
            // 
            // Make sure a user doesn't intermix ham service codes with SHARES or MARS service codes.
            // Return True if the service codes are ok.
            // 
            var aryCodes = strCodes.ToUpper().Trim().Split(' ');
            if (aryCodes.Count() < 2)
                return true;
            bool blnSHARES = false;
            bool blnMARS = false;
            bool blnHam = false;
            bool blnOther = false;
            foreach (string strCode in aryCodes)
            {
                if (strCode.StartsWith("SHARES"))
                {
                    blnSHARES = true;
                }
                else if (strCode.StartsWith("MARS") | strCode.StartsWith("CFARS"))
                {
                    blnMARS = true;
                }
                else if (strCode == "PUBLIC" | strCode == "EMCOMM" | strCode.StartsWith("BPQ"))
                {
                    blnHam = true;
                }
                else
                {
                    blnOther = true;
                }
            }

            if (blnSHARES & (blnHam | blnMARS))
                return false;
            if (blnMARS & (blnHam | blnSHARES))
                return false;
            return true;
        }

        public static string GetRandomChallengePhrase()
        {
            string strSet = "1234567890";
            var objRNG = new RNGCryptoServiceProvider();
            var bytBuffer = new byte[8];
            objRNG.GetBytes(bytBuffer);
            var sbdInput = new StringBuilder();
            foreach (byte bytSingle in bytBuffer)
                sbdInput.Append(strSet.Substring(bytSingle % 10, 1));
            return sbdInput.ToString();
        } // GetChallengePhrase

        // 
        // Implementation of the WinLink password challenge/response protocol
        // 
        public static string MD5Hash(string text)
        {
            var retBuf = MD5ByteHash(text);
            return Encoding.ASCII.GetString(retBuf);
        }

        public static string ChallengedPasswordString(string challengePhrase, string password)
        {
            int intResult = ChallengedPassword(challengePhrase, password);
            return Strings.Format(intResult, "0000000000").Substring(2, 8);
        }

        public static int ChallengedPassword(string challengePhrase, string password)
        {
            return ChallengedPassword(challengePhrase, password, salt);
        }

        public static int ChallengedPassword(string challengePhrase, string password, string secret)
        {
            return ChallengedPassword(challengePhrase, password, Encoding.ASCII.GetBytes(secret));
        }

        private static int ChallengedPassword(string challengePhrase, string password, byte[] slt)
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
            int retVal = Convert.ToInt32(retHash[3] & 0x3F);
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

        public string TimeStamp()
        {
            // 
            // This function returns the current time/date in 
            // 2004/08/24 05:33 format string.
            // 
            string strDateTime = Strings.Format(DateTime.UtcNow, @"yyyy\/MM\/dd HH\:mm");
            // Handle localication issue
            strDateTime = strDateTime.Replace(".", "/");
            return strDateTime;
        }

        public bool IsTacticalAddress(string strCallsign)
        {
            // 
            // Check to see if a callsign is a tactical address.
            // 
            return Globals.IsValidTacticalAddress(strCallsign);
        }

        public int GetMaxPactorLevel(int intWinlinkMode)
        {
            // 
            // Convert a Winlink Pactor channel mode code to the maximum corresponding Pactor level.
            // 
            int intPactorMode;
            switch (intWinlinkMode)
            {
                case 11: // Pactor 1 only
                    {
                        intPactorMode = 1;
                        break;
                    }

                case 12:         // Pactor 1 And 2 
                    {
                        intPactorMode = 2;
                        break;
                    }

                case 13:         // Pactor 1, 2, 3
                    {
                        intPactorMode = 3;
                        break;
                    }

                case 14:         // Pactor 2 only
                    {
                        intPactorMode = 2;
                        break;
                    }

                case 15:         // Pactor 2, 3
                    {
                        intPactorMode = 3;
                        break;
                    }

                case 16:         // Pactor 3 only
                    {
                        intPactorMode = 3;
                        break;
                    }

                case 17:         // Pactor 1, 2, 3, 4
                    {
                        intPactorMode = 4;
                        break;
                    }

                case 18:         // Pactor 2, 3, 4
                    {
                        intPactorMode = 4;
                        break;
                    }

                case 19:         // Pactor 3, 4
                    {
                        intPactorMode = 4;
                        break;
                    }

                case 20:         // Pactor 4 only
                    {
                        intPactorMode = 4;
                        break;
                    }

                default:
                    {
                        intPactorMode = 0;
                        break;
                    }
            }

            return intPactorMode;
        }

        public int SignalWidth(int intMode)
        {
            // 
            // Get the bandwidth in Hz of a transmitted signal as a function of the Winlink channel mode code.
            // 
            int intSignalWidth = 2400;
            if (intMode >= 11 & intMode <= 20)
            {
                // Some Pactor level
                int intPactorLevel = GetMaxPactorLevel(intMode);
                if (intPactorLevel >= 3)
                {
                    intSignalWidth = 2400;
                }
                else
                {
                    intSignalWidth = 500;
                }
            }
            else
            {
                // Something other than Pactor
                switch (intMode)
                {
                    case 21:             // Winmor 500
                        {
                            intSignalWidth = 500;
                            break;
                        }

                    case 22:             // Winmor 1600
                        {
                            intSignalWidth = 1600;
                            break;
                        }

                    case 30:             // Robust packet
                        {
                            intSignalWidth = 500;
                            break;
                        }

                    case 31:             // Robust packet
                        {
                            intSignalWidth = 500;
                            break;
                        }

                    case 40:             // ARDOP 200
                        {
                            intSignalWidth = 200;
                            break;
                        }

                    case 41:             // ARDOP 500
                        {
                            intSignalWidth = 500;
                            break;
                        }

                    case 42:             // ARDOP 1000
                        {
                            intSignalWidth = 1000;
                            break;
                        }

                    case 43:             // ARDOP 2500
                        {
                            intSignalWidth = 2500;
                            break;
                        }

                    case 50:             // VARA 2300
                        {
                            // VARA center is exactly 1470Hz, from 340 to 2600
                            // Relative to a 1500 Hz center, the bandwidth is 2320 Hz to account for the low offset.
                            intSignalWidth = 2320;
                            break;
                        }

                    case 53:         // Vara 500
                        {
                            intSignalWidth = 500;
                            break;
                        }

                    default:
                        {
                            intSignalWidth = 2400;
                            break;
                        }
                }
            }

            return intSignalWidth;
        }

        private string JsonCommand(string strArg, ParamList lstParamEntry = null, bool blnDoRetry = true)
        {
            // 
            // Execute a Json command.  Return the results or null if an error occurred.
            // 
            var client = new WebClientExtended();
            string strCmd;
            string strSpecialWebServiceKey = "";

            // 
            // Find a CMS to use.
            // 
            string strIPaddress;
            if (blnUseProxy)
            {
                strIPaddress = strProxyIP;
            }
            else
            {
                strIPaddress = "api.winlink.org";
            }
            // 
            // Make the complete URL.
            // 
            if (blnUseProxy)
            {
                strCmd = "http://" + strProxyIP + ":" + intProxyPort + strArg;
            }
            else
            {
                strArg = strArg.Replace("?format=json", "");
                strCmd = "https://cms.winlink.org" + strArg + "?format=json";
            }
            // 
            // Set up arguments as a NameValueCollection.
            // 
            var reqparm = new System.Collections.Specialized.NameValueCollection();
            bool blnFoundRequester = false;
            if (lstParamEntry is object)
            {
                foreach (ParamEntry objParam in lstParamEntry.lstParams)
                {
                    if (objParam.strParam != "WebServiceKey")
                    {
                        if (objParam.enmType == ParamType.ParTyp_String)
                        {
                            reqparm.Add(objParam.strParam, objParam.strValue);
                            if (objParam.strParam == "Requester")
                                blnFoundRequester = true;
                        }
                        else if (objParam.enmType == ParamType.ParTyp_Datetime)
                        {
                            reqparm.Add(objParam.strParam, Globals.FormatDateEx(objParam.dttValue));
                        }
                    }
                    else
                    {
                        strSpecialWebServiceKey = objParam.strValue;
                    }
                }
            }

            if (!blnFoundRequester)
            {
                reqparm.Add("Requester", strPrimaryCallsign);
            }
            // 
            // Add the API service key for AWS access.
            // 
            if (!string.IsNullOrEmpty(strSpecialWebServiceKey))
            {
                reqparm.Add("Key", strSpecialWebServiceKey);
            }
            else if (strWebServiceKey is object)
            {
                reqparm.Add("Key", strWebServiceKey);
            }

            string strArgs = "";
            for (int i = 0, loopTo = reqparm.Count - 1; i <= loopTo; i++)
                strArgs += reqparm.GetKey(i) + "=" + reqparm.Get(i) + "; ";
            // 
            // Post the request to the server.
            // 
            int intTimeout = intConnectionTimeoutSeconds;
            if (blnSatPhoneMode)
            {
                intTimeout = intSatPhoneTimeout;
            }

            string strResponse = ExecuteApiFunction(strCmd, reqparm, strIPaddress, intTimeout, blnDoRetry);
            return strResponse;
        }

        private string JsonMeshCommand(string strCmd, string strIPaddress = null, int intRequestedTimeout = 0, bool blnDoRetry = true, ParamList lstParamEntry = null)


        {
            // 
            // Execute a Json command to a MESH host.  Return the results or null if an error occurred.
            // 
            var client = new WebClientExtended();
            // 
            // Make the complete URL.
            // 
            if (string.IsNullOrEmpty(strIPaddress))
                strIPaddress = "localnode.local.mesh:8080";
            if (!strIPaddress.Contains(":"))
            {
                if (!strIPaddress.ToLower().Contains(".local.mesh"))
                {
                    strIPaddress += ".local.mesh";
                }

                strIPaddress += ":8080";
            }

            strCmd = "http://" + strIPaddress + "/cgi-bin/sysinfo.json?" + strCmd;
            // 
            // Set up arguments as a NameValueCollection.
            // 
            var reqparm = new System.Collections.Specialized.NameValueCollection();
            if (lstParamEntry is object)
            {
                foreach (ParamEntry objParam in lstParamEntry.lstParams)
                {
                    if (objParam.strParam != "WebServiceAccessCode")
                    {
                        if (objParam.enmType == ParamType.ParTyp_String)
                        {
                            reqparm.Add(objParam.strParam, objParam.strValue);
                        }
                        else if (objParam.enmType == ParamType.ParTyp_Datetime)
                        {
                            reqparm.Add(objParam.strParam, Globals.FormatDateEx(objParam.dttValue));
                        }
                    }
                }
            }

            string strArgs = "";
            for (int i = 0, loopTo = reqparm.Count - 1; i <= loopTo; i++)
                strArgs += reqparm.GetKey(i) + "=" + reqparm.Get(i) + "; ";
            // 
            // Post the request to the server.
            // 
            int intTimeout = intConnectionTimeoutSeconds;
            if (blnSatPhoneMode)
            {
                intTimeout = intSatPhoneTimeout;
            }
            else if (intRequestedTimeout > 0)
            {
                intTimeout = intRequestedTimeout;
            }

            string strResponse = ExecuteApiFunction(strCmd, reqparm, strIPaddress, intTimeout, blnDoRetry);
            return strResponse;
        }

        private string ExecuteApiFunction(string strCmd, System.Collections.Specialized.NameValueCollection reqParam, string strIPaddress, int intConnectTimeout, bool blnDoRetry)


        {
            // 
            // Perform an Api function.  Return the response string.
            // 
            // Two types of timeouts can occur:
            // 1. A connection timeout which means we failed to connect to a CMS.
            // 2. An operation timeout which means the CMS did not complete the API in a reasonable time.
            // 
            string strResponse = null;
            bool blnTimeoutOccurred = false;
            Enumerations.ApiParam objApiArgs;
            for (int intTry = 1; intTry <= 2; intTry++)
            {
                // 
                // Set up an object to pass in the parameters.
                // 
                objApiArgs = new Enumerations.ApiParam();
                objApiArgs.strCmd = strCmd;
                objApiArgs.reqParm = reqParam;
                objApiArgs.strIPaddress = strIPaddress;
                objApiArgs.intConnectTimeout = intConnectTimeout;
                objApiArgs.blnDoRetry = blnDoRetry;
                // 
                // Do the API function.  Exit the loop if we didn't have a timeout or if retries are disabled.
                // 
                blnTimeoutOccurred = ExecuteApiWork(objApiArgs);
                strResponse = objApiArgs.strResponse;
                if (blnTimeoutOccurred == false | blnDoRetry == false)
                    break;
                // Timeout occurred. Retry the operation one time.
            }
            // 
            // Finished
            // 
            return strResponse;
        }

        private bool ExecuteApiWork(Enumerations.ApiParam objApiArgs)
        {
            // 
            // Excute a thread to perform an API function.
            // Return True if a timeout occurred while the API service was being performed.
            // Note: If a connection timeout occurs, blnConnectTimeout is set and this routine returns False.
            // 
            // Start a thread to perform the actual API function.
            // We do it this way so we can have a watchdog timer to kill the operation if it doesn't complete in a reasonable time.
            // There are two types of timeouts: Connect and Operation
            // 
            string strCMSname = GetCMSNameFromIP(objApiArgs.strIPaddress);
            bool blnTimeoutOccurred = false;
            var dttStart = DateTime.UtcNow;
            int intTotalTimeout = objApiArgs.intConnectTimeout + 60;
            Thread thrDoApi;
            thrDoApi = new Thread(ExecuteApiFunctionThread);
            thrDoApi.Start(objApiArgs);
            // 
            // Wait for the Api function to finish or for it to time out.
            // We have to be generous with the operation timeout, because some requests take a while for the CMS to process.
            // 
            while (objApiArgs.blnFinished == false & (DateTime.UtcNow - dttStart).TotalSeconds < intTotalTimeout)
                Thread.Sleep(100);
            double dblApiTime = (DateTime.UtcNow - dttStart).TotalSeconds;
            // Thread finished or timed out.
            if (objApiArgs.blnFinished)
            {
                // 
                // The operation finished or the connection attempt timed out.  Remember if a connection timeout occurred.
                // 
                blnTimeoutOccurred = objApiArgs.blnConnectTimeout;
            }
            else
            {
                // 
                // Timeout occurred while waiting for the operation to finish.
                // 
                blnTimeoutOccurred = true;
                objApiArgs.blnAbort = true;
                thrDoApi.Abort();
                Thread.Sleep(200);
                objApiArgs.strResponse = "{\"HasError\":true,\"ErrorCode\":1,\"ErrorMessage\":\"" + "CMS API operation timeout to " + strCMSname + "\",\"ServerName\":\"" + "CMS" + "\"}";
            }
            // 
            // Finished
            // 
            return blnTimeoutOccurred;
        }

        private void ExecuteApiFunctionThread(object objParams)
        {
            // 
            // Do the actual work to perform an API function.
            // 
            Enumerations.ApiParam objApiArgs = (Enumerations.ApiParam)objParams;
            // 
            // Post the request to the server.
            // 
            var client = new WebClientExtended();
            var dttStart = DateTime.UtcNow;
            double dblConnectTime;
            byte[] responsebytes = null;
            bool blnTimeout = false;
            string strResponse = null;
            string strIPaddress = objApiArgs.strIPaddress;
            string strCMSname = GetCMSNameFromIP(objApiArgs.strIPaddress);
            try
            {
                client.Timeout = objApiArgs.intConnectTimeout * 1000;  // set timeout
                                                                       // 
                                                                       // Connect to a CMS and do the actual function.
                                                                       // 
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                responsebytes = client.UploadValues(objApiArgs.strCmd, "POST", objApiArgs.reqParm);
                // Successful connection.  See if our thread was aborted.
                try
                {
                    if (objApiArgs.blnAbort)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
                // We haven't been aborted.
                if (responsebytes == null)
                {
                    try
                    {
                        objApiArgs.blnFinished = true;
                    }
                    catch
                    {
                    }

                    return;
                }

                strResponse = Encoding.UTF8.GetString(responsebytes);
                if (strResponse == null)
                {
                    try
                    {
                        objApiArgs.blnFinished = true;
                    }
                    catch
                    {
                    }

                    return;
                }

                dblConnectTime = (DateTime.UtcNow - dttStart).TotalSeconds;
                if (dblConnectTime > dblMaxAPIConnectTime)
                {
                    dblMaxAPIConnectTime = dblConnectTime;
                }
                // 
                // Normal completion of API function.
                // 
                // File.WriteAllText("c:\Test\Response.html", strResponse)
                // Remove non-ASCII characters, and return the result.
                // 
                try
                {
                    objApiArgs.strResponse = CleanNonAscii(strResponse);
                    objApiArgs.blnFinished = true;
                }
                catch
                {
                }

                return;
            }
            catch (WebException webEx)
            {
                // 
                // Exception on API. webEx.Response has the actual web service error.  Convert it to Json response format.
                // 
                dblConnectTime = (DateTime.UtcNow - dttStart).TotalSeconds;
                ReportError();
                if (webEx.Status == WebExceptionStatus.Timeout)
                {
                    // Timeout connecting to this CMS.
                    blnTimeout = true;
                }

                try
                {
                    HttpWebResponse response = (HttpWebResponse)webEx.Response;
                    if (response == null || response.StatusDescription == null)
                    {
                        if (blnTimeout)
                        {
                            strResponse = "{\"HasError\":true,\"ErrorCode\":1,\"ErrorMessage\":\"" + "CMS Connection timeout to " + strCMSname + "\",\"ServerName\":\"" + strCMSname + "\"}";
                        }
                        else
                        {
                            strResponse = "{\"HasError\":true,\"ErrorCode\":1,\"ErrorMessage\":\"" + "CMS Connection failure to " + strCMSname + "\",\"ServerName\":\"" + strCMSname + "\"}";
                        }
                    }
                    else
                    {
                        strResponse = "{\"HasError\":true,\"ErrorCode\":1,\"ErrorMessage\":\"" + response.StatusDescription + "\",\"ServerName\":\"" + strCMSname + "\"}";
                    }
                }
                catch
                {
                    strResponse = "{\"HasError\":true,\"ErrorCode\":1,\"ErrorMessage\":\"" + "CMS Connection failure to " + strCMSname + "\",\"ServerName\":\"" + strCMSname + "\"}";
                }
            }
            // 
            // Finished
            // 
            try
            {
                objApiArgs.strResponse = strResponse;
                objApiArgs.blnConnectTimeout = blnTimeout;
                objApiArgs.blnFinished = true;
            }
            catch
            {
            }

            return;
        }

        private string GetCMSNameFromIP(string strIP)
        {
            // 
            // Attempt to get the name of the CMS matching a specified IP address.
            // Return the IP address if no match is found.
            // 
            if (blnUseProxy)
            {
                return "Proxy";
            }
            else
            {
                return "CMS";
            }
        }

        private string CleanNonAscii(string strInput)
        {
            // 
            // Remove non-ascii characters from a string.
            // 
            string strResult = Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(strInput)));
            return strResult;
        }

        private void ReportError(string strError = "")
        {
            // 
            // This routine is called if an error occurs on a system service.
            // 
            strLastError = strError;
            intErrorCount += 1;
            return;
        }

        public void SetLogsFolder(string strFolder)
        {
            // 
            // Set the name of the folder where the log file should be written.
            // 
            strLogsDirectory = strFolder;
            return;
        }

        private void EventsLog(string strText, bool blnAddCrLf = true)
        {
            // 
            // Writes the indicated text to the event log.
            // 
            if (string.IsNullOrEmpty(strLogsDirectory))
                return;
            lock (objLogLock)
            {
                try
                {
                    if (blnAddCrLf)
                    {
                        My.MyProject.Computer.FileSystem.WriteAllText(strLogsDirectory + "WinlinkInterop " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", TimestampEx() + " " + strText.Trim() + Microsoft.VisualBasic.Constants.vbCrLf, true);
                    }
                    else
                    {
                        My.MyProject.Computer.FileSystem.WriteAllText(strLogsDirectory + "WinlinkInterop " + Strings.Format(DateTime.UtcNow, "yyyyMMdd") + ".log", TimestampEx() + " " + strText.Trim(), true);
                    }
                }
                catch
                {
                }
            }
        }

        private string TimestampEx()
        {
            // 
            // This function returns the current time/date in 
            // 2004/08/24 05:33:12 format string.
            // 
            string strDateTime = Strings.Format(DateTime.UtcNow, @"yyyy\/MM\/dd HH\:mm\:ss");
            // Handle localization issue
            strDateTime = strDateTime.Replace(".", "/");
            return strDateTime;
        }
    }
}