using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink.UI.Common
{
    public interface IPactorTNCChannelBacking : IFormBacking
    {
        string LastPactorChannel
        {
            get;
            set;
        }

        IEnumerable<string> TncTypes
        {
            get;
        }

        IEnumerable<string> SerialPorts
        {
            get;
        }

        IEnumerable<string> BaudRates
        {
            get;
        }
        IEnumerable<string> RadioModels
        {
            get;
        }

        string SiteRootDirectory
        {
            get;
        }

        bool IsAutoforwardEnabled
        {
            get;
        }

        IEnumerable<string> ChannelNames
        {
            get;
        }

        bool HasChannelList
        {
            get;
        }

        bool IsAccount(string name);

        bool IsChannel(string name);

        bool IsValidChannelName(string channelName);

        bool ChannelExists(string channelName);

        void RemoveChannel(string channelName);

        void GetChannelInfo(
            string channelName, out bool firstUseOnly, out int priority,
            out int activityTimeout, out bool enabled, out bool autoForward,
            out bool busyHold, out string tncSerialPort, out string tncBaudRate,
            out string tncType, out string tncConfigurationFile, out string audioToneCenter,
            out string radioModel, out string civAddress, out bool narrowFilter,
            out string radioBaud, out string radioPort, out bool radioManual,
            out bool radioSerial, out bool viaPTCII, out int fskLevel,
            out int pskLevel, out bool pactorIdEnabled, out bool ttlLevel,
            out bool nmea, out string remoteCallsign, out string centerFrequency,
            out bool longPath);

        void AddChannel(
            string channelName, int priority, bool enabled, int tncTimeout, string tncSerialPort,
            string tncBaudRate, string tncConfigFile, bool firstUseOnly, string tncType,
            bool enableAutoForward, string audioToneCenter, string radioBaud, string radioPort,
            string radioModel, string civAddress, bool narrowFilter, bool viaPTCII, bool viaSerial,
            int fskLevel, int pskLevel, bool busyHold, bool useLongPath, bool pactorIdEnabled,
            string remoteCallsign, string centerFreq, bool ttlLevel, bool nmea);

        void UpdateChannel(
            string channelName, int priority, bool enabled, int tncTimeout, string tncSerialPort,
            string tncBaudRate, string tncConfigFile, bool firstUseOnly, string tncType,
            bool enableAutoForward, string audioToneCenter, string radioBaud, string radioPort,
            string radioModel, string civAddress, bool narrowFilter, bool viaPTCII, bool viaSerial,
            int fskLevel, int pskLevel, bool busyHold, bool useLongPath, bool pactorIdEnabled,
            string remoteCallsign, string centerFreq, bool ttlLevel, bool nmea);

        string[] GetChannelList();

        bool ContainsUsableFrequency(string freqList, string tncType);

        bool CanUseFrequency(string freq, string tncType);

        string FormatFrequency(string freq);

        string DownloadChannelList();
    }
}
