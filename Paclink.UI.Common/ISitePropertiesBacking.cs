using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.UI.Common
{
    public interface ISitePropertiesBacking : IFormBacking
    {
        string SiteRootDirectory { get; }

        string RMSRelayIPPath { get; }

        int RMSRelayPort { get; }

        bool UseRMSRelay { get; }

        string SiteCallsign { get; }

        string ServiceCodes { get; }

        string SecureLoginPassword { get; }

        string POP3Password { get; }

        string GridSquare { get; }

        int SMTPPortNumber { get; }

        int POP3PortNumber { get; }

        bool LanAccessible { get; }

        bool RadarEnabled { get; }

        bool ForceHFRouting { get; }

        string Prefix { get; }

        string Suffix { get; }

        IEnumerable<string> LocalIPAddresses { get; }

        int SelectedLocalIPAddress { get; }

        bool IsCallsignAndGridSquareValid();

        bool IsValidRadioCallsign(string callsign);

        void AddRadioAccount(string strCallsign, string strPassword);

        bool RemoveRadioAccount(string strCallsign);

        void UpdateSiteCallsign(string callsign, string password);

        void UpdatePOP3Password(string password);

        void UpdateGridSquare(string gridSquare);

        void UpdateServiceCode(string serviceCode);

        void UpdateSmtpPortNumber(int portNumber);

        void UpdatePop3PortNumber(int portNumber);

        void UpdateLanAccessible(bool lanAccessible);

        void UpdateRadarEnabled(bool radarEnabled);

        void UpdateUseRMSRelay(bool rmsRelay);

        void UpdateRMSRelayHost(string ipAddress, int port);

        void UpdateForceHFRouting(bool hfRouting);

        void UpdateLocalIPAddress(int selectedIndex);

        void UpdatePrefixAndSuffix(string prefix, string suffix);

        bool IsValidPort(int selectedLocalIP, int proposedPort);

        void RefreshRMSRelayIPAddress();
    }
}
