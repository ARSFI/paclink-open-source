using NLog.Fluent;
using Paclink.Data;
using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Paclink
{
    public class DialogSitePropertiesViewModel : ISitePropertiesBacking
    {
        public string SiteRootDirectory => Globals.SiteRootDirectory;

        public string RMSRelayIPPath => Globals.strRMSRelayIPPath;

        public int RMSRelayPort => Globals.intRMSRelayPort;

        public bool UseRMSRelay => Globals.blnUseRMSRelay;

        public string SiteCallsign => Globals.SiteCallsign;

        public string ServiceCodes => Globals.strServiceCodes;

        public string SecureLoginPassword => Globals.SecureLoginPassword;

        public string MailSystemPassword => Globals.MailSystemPassword;

        public string GridSquare => Globals.SiteGridSquare;

        public int SMTPPortNumber => Globals.intSMTPPortNumber;

        public int POP3PortNumber => Globals.intPOP3PortNumber;

        public bool LanAccessible => Globals.blnLAN;

        public bool RadarEnabled => Globals.blnEnableRadar;

        public bool ForceHFRouting => Globals.blnForceHFRouting;

        public string Prefix => Globals.Settings.Get("Properties", "Prefix", "");

        public string Suffix => Globals.Settings.Get("Properties", "Suffix", "");

        public IEnumerable<string> LocalIPAddresses => Globals.strLocalIPAddresses;

        public int SelectedLocalIPAddress => Globals.Settings.Get("Properties", "Default Local IP Address Index", 0);

        public bool IsCallsignAndGridSquareValid()
        {
            if (!Globals.IsValidRadioCallsign(Globals.SiteCallsign))
                return false;
            if (Globals.SiteGridSquare.Length != 4 & Globals.SiteGridSquare.Length != 6)
                return false;
            return true;
        }

        public bool IsValidRadioCallsign(string callsign)
        {
            return Globals.IsValidRadioCallsign(callsign);
        }

        public void AddRadioAccount(string strCallsign, string strPassword)
        {
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            strAccountList = strAccountList + strCallsign + "|";
            Globals.Settings.Save("Properties", "Site Callsign", strCallsign);
            Globals.Settings.Save("Properties", "Secure Login Password", Globals.SecureLoginPassword);
            Globals.Settings.Save("Properties", "Account Names", strAccountList);
            Globals.Settings.Save(strCallsign, "EMail Password", strPassword);
            Accounts.RefreshAccountsList();
        } // AddRadioAccount

        public bool RemoveRadioAccount(string strCallsign)
        {
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            strAccountList = strAccountList.Replace(strCallsign + "|", "");
            Globals.Settings.Save("Properties", "Site Callsign", "");
            Globals.Settings.Save("Properties", "Secure Login Password", "");
            Globals.Settings.Save("Properties", "EMail Password", "");
            Globals.Settings.Save("Properties", "Account Names", strAccountList);
            Globals.Settings.DeleteGroup(strCallsign);

            // Remove callsign account directory...
            try
            {
                var messageStore = new MessageStore(DatabaseFactory.Get());
                messageStore.DeleteAccountEmails(strCallsign);
            }
            catch (Exception ex)
            {
                Log.Error("[Properties.RemoveRadioAccount] " + ex.Message);
            }

            return default;
        } // RemoveRadioAccount

        public void UpdateSiteCallsign(string callsign, string password)
        {
            string strExistingCallsign = Globals.SiteCallsign;

            if (!string.IsNullOrEmpty(strExistingCallsign))
            {
                if (strExistingCallsign.IndexOf("-") > -1)
                {
                    RemoveRadioAccount(strExistingCallsign);
                }
            }

            Globals.SiteCallsign = callsign;
            Globals.SecureLoginPassword = password;
            Globals.Settings.Save("Properties", "Site Callsign", Globals.SiteCallsign);
            Globals.Settings.Save("Properties", "Secure Login Password", Globals.SecureLoginPassword);

            AddRadioAccount(Globals.SiteCallsign, Globals.SecureLoginPassword);
            UserInterfaceFactory.GetUiSystem().GetMainForm().UpdateSiteCallsign(Globals.SiteCallsign);
            Globals.UpdateAccountDirectories();

            // TBD: why are we re-saving these when they're not touched?
            Globals.Settings.Save("Properties", "Program Directory", Globals.SiteBinDirectory);
            Globals.Settings.Save(Globals.strProductName, "Start Minimized", Globals.blnStartMinimized);
        }

        public void UpdateMailPassword(string password)
        {
            Globals.MailSystemPassword = password;
            Globals.Settings.Save("Properties", "EMail Password", Globals.MailSystemPassword);
        }

        public void UpdateGridSquare(string gridSquare)
        {
            Globals.SiteGridSquare = gridSquare;
            Globals.Settings.Save("Properties", "Grid Square", Globals.SiteGridSquare);
        }

        public void UpdateServiceCode(string serviceCode)
        {
            Globals.strServiceCodes = serviceCode;
            Globals.Settings.Save("Properties", "ServiceCodes", Globals.strServiceCodes);
        }

        public void UpdateSmtpPortNumber(int portNumber)
        {
            Globals.intSMTPPortNumber = portNumber;
            Globals.Settings.Save("Properties", "SMTP Port Number", Globals.intSMTPPortNumber);
        }

        public void UpdatePop3PortNumber(int portNumber)
        {
            Globals.intPOP3PortNumber = portNumber;
            Globals.Settings.Save("Properties", "POP3 Port Number", Globals.intPOP3PortNumber);
        }

        public void UpdateLanAccessible(bool lanAccessible)
        {
            Globals.blnLAN = lanAccessible;
            Globals.Settings.Save("Properties", "LAN Connection", Globals.blnLAN);
        }

        public void UpdateRadarEnabled(bool radarEnabled)
        {
            Globals.blnEnableRadar = radarEnabled;
            Globals.Settings.Save("Properties", "Enable Radar", Globals.blnEnableRadar);
        }

        public void UpdateUseRMSRelay(bool rmsRelay)
        {
            Globals.blnUseRMSRelay = rmsRelay;
            Globals.Settings.Save("Properties", "Use RMS Relay", Globals.blnUseRMSRelay);
        }

        public void UpdateRMSRelayHost(string ipAddress, int port)
        {
            Globals.strRMSRelayIPPath = ipAddress;
            Globals.intRMSRelayPort = port;

            Globals.Settings.Save("Properties", "Local IP Path", Globals.strRMSRelayIPPath);
            Globals.Settings.Save("Properties", "RMS Relay Port", Globals.intRMSRelayPort);
        }

        public void UpdateForceHFRouting(bool hfRouting)
        {
            Globals.blnForceHFRouting = hfRouting;
            Globals.Settings.Save("Properties", "Force radio-only", Globals.blnForceHFRouting);
        }

        public void UpdateLocalIPAddress(int selectedIndex)
        {
            Globals.Settings.Save("Properties", "Default Local IP Address Index", selectedIndex);
            try
            {
                Globals.strLocalIPAddress = Globals.strLocalIPAddresses[selectedIndex];
            }
            catch
            {
                // Don't update if selection is invalid.
            }
        }

        public void UpdatePrefixAndSuffix(string prefix, string suffix)
        {
            Globals.Settings.Save("Properties", "Prefix", prefix);
            Globals.Settings.Save("Properties", "Suffix", suffix);
        }

        public bool IsValidPort(int selectedLocalIP, int proposedPort)
        {
            var objTestPort = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPAddress testIP = IPAddress.Any;
                if (Globals.strLocalIPAddresses[selectedLocalIP] != "Default")
                {
                    // Listen on all interfaces by default.
                    testIP = IPAddress.Parse(Globals.strLocalIPAddresses[selectedLocalIP]);
                }
                objTestPort.Bind(new IPEndPoint(testIP, proposedPort));
                objTestPort.Listen(10);
                objTestPort.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RefreshRMSRelayIPAddress()
        {
            Globals.strRMSRelayIPPath = Globals.Settings.Get("Properties", "Local IP Path", "localhost");
        }

        public void FormLoading()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }
    }
}
