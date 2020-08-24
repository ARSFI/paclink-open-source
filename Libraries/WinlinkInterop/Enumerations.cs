﻿using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;

namespace WinlinkInterop
{
    public static class Enumerations
    {
        public enum AccountValidationCodes
        {
            NoInternet = 0,              // Could not validate, because we do not have a connection to the server
            Valid = 1,                   // The callsign and password are valid
            NotRegistered = 2,           // This callsign is not registered with Winlink
            LockedOut = 3,               // The account is locked out
            BadPassword = 4,             // The password is incorrect
            SecureLogon = 5,             // Account requires secure logon
            NotSecureLogon = 6          // Account does not require secure logon
        }

        public class CMSInfo
        {
            // 
            // Holds information about the active CMS site
            // 
            public string strCMSName;
            public string strCMSIP;
            public string strCMSCity;

            public CMSInfo(string CMSname = "", string city = "", string ip = "")
            {
                strCMSCity = city;
                strCMSName = CMSname;
                strCMSIP = ip;
            }
        }

        public class CMSconnection
        {
            // 
            // Holds information about a connection we've established with a CMS.
            // 
            public string strHostAddress;
            public int intPort;
            public bool blnSSL;
            public TcpClient objTcpClient;
            public SslStream objSSL;

            public CMSconnection()
            {
                // Constructor
                objTcpClient = null;
                blnSSL = false;
                strHostAddress = "";
                intPort = 0;
            }

            public void Close()
            {
                // 
                // Close connections and cleanup.
                // 
                try
                {
                    blnSSL = false;
                    strHostAddress = "";
                    intPort = 0;
                    if (objSSL is object)
                    {
                        objSSL.Dispose();
                        objSSL = null;
                    }

                    if (objTcpClient is object)
                    {
                        if (objTcpClient.Connected)
                        {
                            objTcpClient.Close();
                            objTcpClient = null;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        public class GroupAddressEntry
        {
            public string strCallsign;               // Callsign for group
            public List<string> lstAddress;       // List of associated addresses
            public string strSubjectFilter;          // Filter for group address entry
            public string strEnteredBy;              // Who created this entry

            public GroupAddressEntry()
            {
                lstAddress = new List<string>();
            }
        }

        // 
        // Class describing a callsign entry.
        // 
        public class CallsignEntry
        {
            public string strCallsign;
            public string strPassword;
            public string strPasswordHash;
            public string strPasswordChallenge;
            public DateTime dttTimestamp;
            public string strPrefix;
            public string strSuffix;
            public long intTactical;
            public long intNoPurge;
            public long intUseWhiteList;
            public long intAttachments;
            public long intGatewayAccess;
            public long intSecureLogin;
            public long intOptions;
            public long intHF;

            public CallsignEntry()
            {
                strCallsign = "";
                strPassword = "";
                strPasswordHash = "";
                dttTimestamp = DateTime.MinValue;
                strPrefix = "";
                strSuffix = "";
                intTactical = 0;
                intNoPurge = 0;
                intUseWhiteList = 0;
                intAttachments = 0;
                intGatewayAccess = 0;
                intSecureLogin = 0;
                intOptions = 0;
                intHF = 0;
            }
        }

        // 
        // Class used to pass parameters to and receive results from an API service call.
        // 
        public class ApiParam
        {
            public string strCmd = "";
            public System.Collections.Specialized.NameValueCollection reqParm = null;
            public string strIPaddress = "";
            public int intConnectTimeout = 0;
            public bool blnDoRetry = true;
            public string strResponse = null;
            public bool blnFinished = false;
            public bool blnConnectTimeout = false;     // Timeout while trying to connect to a CMS
            public bool blnAbort = false;
        }
    }
}