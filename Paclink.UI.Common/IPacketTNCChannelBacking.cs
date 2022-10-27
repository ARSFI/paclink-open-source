using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IPacketTNCChannelBacking : IFormBacking
    {
        string SiteRootDirectory
        {
            get;
        }

        string LastPacketChannel
        {
            get;
            set;
        }

        IEnumerable<string> SerialPorts
        {
            get;
        }

        bool HasChannelList
        {
            get;
        }

        IEnumerable<string> ChannelList
        {
            get;
        }

        IEnumerable<string> ChannelNames
        {
            get;
        }

        bool ContainsChannel(string name);

        bool IsAccount(string name);

        bool IsChannel(string name);

        void RemoveChannel(string name);

        void AddChannel(string name,
            string centerFrequency, int priority, string remoteCallsign, int activityTimeout,
            int scriptTimeout, string script, bool enabled, string serialPort,
            string tncType, int tncPort, string tncConfigurationFile,
            bool tncConfigureOnFirstUseOnly, string radioControlBaud, string radioControlPort,
            string radioModel, int onAirBaud, string civAddress,
            bool radioControlSerial, bool radioControlPTC, bool radioControlManual,
            bool radioTTLLevel, string freqMHZ, string tncBaudRate);

        void UpdateChannel(string name,
            string centerFrequency, int priority, string remoteCallsign, int activityTimeout,
            int scriptTimeout, string script, bool enabled, string serialPort,
            string tncType, int tncPort, string tncConfigurationFile,
            bool tncConfigureOnFirstUseOnly, string radioControlBaud, string radioControlPort,
            string radioModel, int onAirBaud, string civAddress,
            bool radioControlSerial, bool radioControlPTC, bool radioControlManual,
            bool radioTTLLevel, string freqMHZ, string tncBaudRate);

        void GetChannelInfo(
            string name, out string centerFrequency, out int priority, out int activityTimeout,
            out int scriptTimeout, out string script, out bool enabled, out string serialPort,
            out string tncType, out int tncPort, out string tncConfigurationFile,
            out bool tncConfigureOnFirstUseOnly, out string radioControlBaud, out string radioControlPort,
            out string radioModel, out int onAirBaud, out string civAddress,
            out bool radioControlSerial, out bool radioControlPTC, out bool radioControlManual,
            out bool radioTTLLevel, out string freqMHZ, out string tncBaudRate, out string remoteCallsign);

        string DownloadChannelList();

        bool CanUseBaudAndFrequency(string freqList, string tncType, string baud);

        bool IsValidChannelName(string name);

        bool IsFreqInLimits(string freq);

        string CleanupCallSign(string strCallsign);

        string[] GetUsableFreqs(string[] freqList, string tncType);
    }
}
