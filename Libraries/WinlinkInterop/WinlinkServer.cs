using System;
using System.Collections.Generic;

namespace WinlinkInterop
{
    public class WinlinkServer
    {
        // 
        // Class describing a Winlink server (CMS, WLW or AutoUpdate)
        // 
        // General data members for WinlinkInterop.
        // 
        public string strName;
        public string strCity;
        public int intNetParamPrio;
        public List<string> lstIP;
        public string strDefaultIP;
        public int intPort;
        public object objLock = new object();
        public Enumerations.CMSInfo objCMSInfo;
        public bool blnLookedUpDNS;
        public DateTime dttServerAvailable;
        public DateTime dttServerUnavailable;
        public DateTime dttServiceCheck;
        public bool blnServicesAvailable;

        public WinlinkServer(string name, string defaultIP, int intPortArg, string city, int intNetParamPrioArg = 5)
        {
            // 
            // Constructor for a new WinlinkServer object.
            // 
            strName = name;
            strDefaultIP = defaultIP;
            intPort = intPortArg;
            strCity = city;
            intNetParamPrio = intNetParamPrioArg;
            objCMSInfo = new Enumerations.CMSInfo(strName, strCity, "");
            lstIP = new List<string>();
            if (!string.IsNullOrEmpty(strDefaultIP))
            {
                // 
                // Add the default IP to the list
                // 
                lstIP.Add(strDefaultIP);
            }

            blnLookedUpDNS = false;
            dttServerAvailable = DateTime.UtcNow.AddDays(-1);
            dttServerUnavailable = DateTime.UtcNow.AddDays(-1);
            dttServiceCheck = DateTime.UtcNow.AddDays(-1);
            blnServicesAvailable = true;
        }
    }

    public class PublicServer
    {
        // 
        // Class describing a public web server such as www.google.com
        // 
        // General data members
        // 
        public string strName;
        public List<string> lstIP;
        public int intIPindex;
        public object objLock = new object();
        public DateTime dttServerAvailable;
        public DateTime dttServerUnavailable;
        public DateTime dttDNSLookup;

        public PublicServer(string name)
        {
            // 
            // Constructor for a new PublicServer object.
            // 
            strName = name;
            lstIP = new List<string>();
            intIPindex = 0;
            dttServerAvailable = DateTime.UtcNow.AddDays(-1);
            dttServerUnavailable = DateTime.UtcNow.AddDays(-1);
            dttDNSLookup = DateTime.UtcNow.AddDays(-1);
        }
    }
}