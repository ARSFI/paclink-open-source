using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        private double dblMonitorPeriod = 240;        // Number of seconds between Internet monitor scans
        private int intConnectionTimeoutSeconds = 30; // Number of seconds to wait before declaring a connection timeout
        private int intSatPhoneTimeout = 120;      // Timeout in seconds when connecting through a sat phone
        private string strPrimaryCallsign = "";              // Callsign of site
        private string strLastError = "";             // Last recorded error
        private DateTime dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);    // Time when we determined Internet is down
        private DateTime dttInternetAvailable = DateTime.UtcNow.AddDays(-1);    // Time when we determined Internet is up
        private object objLock = new object();                   // Used for SyncLock
        private bool blnEnableInternet = true;     // Set False to simulate Internet outage
        private bool blnSatPhoneMode = false;
        private string strAccountAccessCode = "C6B607C4AB604A679E396A01E1CA1E98";
        private string strWebServiceKey = "";
        private Dictionary<string, bool> dicAccountCache = new Dictionary<string, bool>();
        private Dictionary<string, PasswordCacheEntry> dicPasswordCache = new Dictionary<string, PasswordCacheEntry>();
        private Dictionary<string, NetworkParamCacheEntry> dicNetworkParamCache = new Dictionary<string, NetworkParamCacheEntry>();
        private int intNetworkParamCacheExpire = 12;       // Expiration period in hours network parameter cache entries
        private DateTime dttLastNetworkParamCacheExpire = DateTime.UtcNow;
        private object objNetworkParamCacheLock = new object();
        private object objAccountCacheLock = new object();
        private int intNumCMSConnections;
        private double dblTotalCMSConnectionTime;
        private double dblMaxSuccessfulConnectTime;
        private double dblMaxAPIConnectTime;
        private int intNumFailedCMSConnections;
        private double dblTotalFailedCMSTime;
        private int intErrorCount;
        private JsonEnum objRequestedModeEnum;
        private JsonEnum objModeEnum;
        private JsonEnum objUserTypeEnum;
        private DateTime dttLastPingTime = DateTime.MinValue;
        private bool blnLastPingSuccess;
        private string strLogsDirectory = "";
        private object objLogLock = new object();

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
            objRequestedModeEnum.Add("AnyAll", GatewayOperatingMode.AnyAll.ToString());
            objRequestedModeEnum.Add("Packet", GatewayOperatingMode.Packet.ToString());
            objRequestedModeEnum.Add("Pactor", GatewayOperatingMode.Pactor.ToString());
            objRequestedModeEnum.Add("Winmor", GatewayOperatingMode.Winmor.ToString());
            objRequestedModeEnum.Add("RobustPacket", GatewayOperatingMode.RobustPacket.ToString());
            objRequestedModeEnum.Add("AllHf", GatewayOperatingMode.AllHf.ToString());
            objModeEnum = new JsonEnum("Mode");
            objModeEnum.Add("AnyAll", GatewayOperatingMode.AnyAll.ToString());
            objModeEnum.Add("Packet", GatewayOperatingMode.Packet.ToString());
            objModeEnum.Add("Pactor", GatewayOperatingMode.Pactor.ToString());
            objModeEnum.Add("Winmor", GatewayOperatingMode.Winmor.ToString());
            objModeEnum.Add("RobustPacket", GatewayOperatingMode.RobustPacket.ToString());
            objModeEnum.Add("AllHf", GatewayOperatingMode.AllHf.ToString());
            objUserTypeEnum = new JsonEnum("UserType");
            objUserTypeEnum.Add("AnyAll", UserType.AnyAll.ToString());
            objUserTypeEnum.Add("Client", UserType.Client.ToString());
            objUserTypeEnum.Add("Sysop", UserType.Sysop.ToString());
        }

        public void Close()
        {
            // 
            // Close the WinlinkInterop object.
            // 
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
        }

        public bool AccountRegistered(string strUserCallsign)
        {
            // 
            // Check to see if the specified account is registered with the Winlink system. Return True if it is.
            // 
            if (!(IsValidCallsign(strUserCallsign) | Globals.IsValidTacticalAddress(strUserCallsign, 12)))
                return false;
            var enmValid = AccountExists(strUserCallsign, strUserCallsign);
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

        private bool SearchAccountCache(string strCallsign)
        {
            // 
            // Return True if the account is known to be valid.
            // 
            lock (objAccountCacheLock)
            {
                if (dicAccountCache.ContainsKey(strCallsign))
                {
                    return dicAccountCache[strCallsign];
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

        private PasswordCacheEntry SearchPasswordCache(string strCallsign)
        {
            // 
            // Try to find an unexpired password cache entry for a callsign.
            // 
            lock (objLock)
            {
                PasswordCacheEntry objPassword = null;
                if (dicPasswordCache.ContainsKey(strCallsign))
                {
                    objPassword = dicPasswordCache[strCallsign];
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
            dttInternetAvailable = DateTime.UtcNow;
            dttInternetUnavailable = DateTime.UtcNow.AddDays(-1);
            // 
            // Set up the CMSconnection object.
            // 
            objCMSconnection = new Enumerations.CMSconnection();
            objCMSconnection.blnSSL = false;
            objCMSconnection.strHostAddress = strHostAddress;
            objCMSconnection.intPort = intPort;
            objCMSconnection.objTcpClient = objIPPort;
            return objCMSconnection;
        }

        private Enumerations.CMSconnection ConnectToCMSSSL(string strHostAddress, int intPort, int intTimeout = 20)
        {
            // 
            // Make a SSL Telnet connection to a CMS.
            // 
            Enumerations.CMSconnection objCMSconnection = null;
            if (string.IsNullOrEmpty(strHostAddress))
                strHostAddress = "cms-z.winlink.org"; //!!!.k0qed temp
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
                objCMSconnection.blnSSL = true;
                objCMSconnection.strHostAddress = strHostAddress;
                objCMSconnection.intPort = intPort;
                EventsLog("[ConnectToCMSSSL] Successfully made SSL connection to CMS.");
            }
            catch (Exception ex)
            {
                // Unable to establish SSL connection.  Try to make a non-SSL connection.
                EventsLog("[ConnectToCMSSSL] Unable to make SSL connection.  Try non-SSL. " + ex.Message);
                try
                {
                    objCMSconnection.Close();
                    // Try to establish non-SSL connection.
                    return ConnectToServer(strHostAddress, 8772, false, intTimeout);
                }
                catch
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
            var strFiles = FileInfo.Split(new[] { '\r' });
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
            catch (Exception ex)
            {
                // Unable to connect
                SetError("TestConnection: Exception " + ex.Message);
                return false;
            }
        }

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(ref int lpSFlags, int dwReserved);


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

        public List<GatewayStatusRecord> GatewayChannelList(bool blnPacketChannels, string strServiceCodes, string strCaller, string strForceCMSArg = "")
        {
            // 
            // Get a list of the available gateway channels, and return it as a List(Of GatewayStatusRecord).
            // Return Nothing if we can't get the list.
            // 
            // Send the request.
            // 
            var lstParam = new ParamList();
            lstParam.Add("Mode", GatewayOperatingMode.AnyAll.ToString());
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
                if (objResponse != null)
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
                        File.AppendAllText(strLogsDirectory + "WinlinkInterop " + DateTime.UtcNow.ToString("yyyyMMdd") + ".log", TimestampEx() + " " + strText.Trim() + Environment.NewLine);
                    }
                    else
                    {
                        File.AppendAllText(strLogsDirectory + "WinlinkInterop " + DateTime.UtcNow.ToString("yyyyMMdd") + ".log", TimestampEx() + " " + strText.Trim());
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
            return DateTime.UtcNow.ToString(@"yyyy/MM/dd HH:mm:ss");
        }
    }
}