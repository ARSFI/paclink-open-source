using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;

namespace WinlinkInterop
{
    public static class Enumerations
    {
        public enum WL2KPostType
        {
            WLW = 0,
            CMS,
            AU
        }

        public enum WL2KOperatingMode
        {
            // Edited by RM 1/31/2013 to agree with Mapping used by CMS 
            // Packet_300 = 0
            Packet_1200 = 0,
            Packet_2400 = 1,
            Packet_4800 = 2,
            Packet_9600 = 3,
            Packet_19200 = 4,
            Packet_38400 = 5,
            Packet_GT38400 = 6,
            Pactor_1 = 11,
            Pactor_12 = 12,
            Pactor_123 = 13,
            Pactor_2 = 14,
            Pactor_23 = 15,
            Pactor_3 = 16,
            Pactor_1234 = 17,
            Pactor_234 = 18,
            Pactor_34 = 19,
            Pactor_4 = 20,
            WINMOR_500 = 21,
            WINMOR_1600 = 22,
            SCSRobustPacket = 30,
            VaraFM_1200 = 51,
            VaraFM_9600 = 52
        }

        public enum WL2KGroupReference
        {
            TestSite = 0,
            PublicSite = 1,
            EMCOMMSite = 2,
            MARSSite = 3,
            UKCadetSite = 4
        }

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
            public string strCMSCity;
            public string strCMSName;
            public string strCMSIP;
            public bool blnSSL;
            public TcpClient objTcpClient;
            public SslStream objSSL;

            public CMSconnection()
            {
                // Constructor
                objTcpClient = null;
                strCMSCity = "";
                strCMSName = "";
                strCMSIP = "";
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

        public enum ChannelTypeSet
        {
            PactorOnly,
            HFchannels,
            AllChannels
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

        public class clsSysop
        {
            public string strBaseCallsign;            // Base callsign of the RMS
            public string strName;                    // Name of the sysop
            public string strEmail;                   // E-mail address
        }

        public class PasswordEntry
        {
            public string strCallsign;                // Callsign
            public string strChallenge;               // Password challenge string
            public string strPasswordHash;            // Password hash code
        }

        // 
        // Class with arguments for /sysop/add
        // 
        public class SysopInfo
        {
            public string Callsign;
            public string City;
            public string Comments;
            public string Country;
            public string Email;
            public string GridSquare;
            public string Phones;
            public string PostalCode;
            public string State;
            public string StreetAddress1;
            public string StreetAddress2;
            public string SysopName;
            public string Website;
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

        // -----------------------------------------------------------------------
        // Class describing an SMTP mail server.
        // 
        public class SmtpMailServer
        {
            public string strUser = "";                     // Name Of the user To log onto the mail server */
            public string strPassword = "";                 // Password For mail server */
            public string strMailServer = "127.0.0.1";      // Domain name Or IP address Of mail server */
            public int intMailPort = 25;                    // IP port used On the mail server */
        }

        // ---------------------------------------------------------------------------------
        // Class describing a Winlink SMTP message.
        // 
        public class SmtpMessage
        {
            public string strMessageText;                   // Message text */
            public string strMessageID;                     // ID Of this message */
            public string strRecipients;                    // Recipients Of message */
            public string strFrom;                          // Sender */
            public string strSubject;                       // Subject Of message */
            public DateTime dttMessageTime;                 // Timestamp */
            public DateTime dttSendTime;                    // Time When message was sent */
        }
    }
}