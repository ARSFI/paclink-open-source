using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink
{
    public class DialogPactorTNCChannelViewModel : IPactorTNCChannelBacking
    {
        public string LastPactorChannel
        {
            get
            {
                return Globals.Settings.Get("Properties", "Last Pactor Channel", "");
            }
            set
            {
                Globals.Settings.Save("Properties", "Last Pactor Channel", value);
            }
        }

        public IEnumerable<string> TncTypes
        {
            get
            {
                return new List<string>()
                {
                    "KAM98",
                    "KAM/+",
                    "KAMXL",
                    "DSP-232",
                    "PK-232",
                    "PTC II",
                    "PTC IIe",
                    "PTC IIex",
                    "PTC IIpro",
                    "PTC IIusb",
                    "PTC DR-7800"
                };
            }
        }

        public IEnumerable<string> BaudRates
        {
            get
            {
                return new List<string>()
                {
                    "1200",
                    "2400",
                    "4800",
                    "9600",
                    "19200",
                    "38400",
                    "57600",
                    "115200"
                };
            }
        }

        public IEnumerable<string> SerialPorts
        {
            get
            {
                foreach (var port in SerialPort.GetPortNames())
                {
                    yield return Globals.CleanSerialPort(port);
                }
            }
        }

        public IEnumerable<string> RadioModels
        {
            get
            {
                return new List<string>()
                {
                    "Kenwood TS-450",
                    "Kenwood TS-480",
                    "Kenwood TS-690",
                    "Kenwood TS-2000",
                    "Kenwood (other)",
                    "Icom IC-706",
                    "Icom IC-7000",
                    "Icom IC-7200",
                    "Icom IC-746",
                    "Icom IC-746pro",
                    "Icom IC-756pro",
                    "Icom (other CI-V)",
                    "Icom IC-M700Pro",
                    "Icom IC-M710",
                    "Icom IC-M710RT",
                    "Icom IC-M802",
                    "Icom (other NMEA)",
                    "Micom 3F",
                    "Yaesu FT-450",
                    "Yaesu FT-847",
                    "Yaesu FT-857",
                    "Yaesu FT-857D",
                    "Yaesu FT-897",
                    "Yaesu FT-920",
                    "Yaesu FT-950",
                    "Yaesu FT-1000",
                    "Yaesu FT-2000",
                    "Yaesu (other)",
                };
            }
        }

        public string SiteRootDirectory
        {
            get
            {
                return Globals.SiteRootDirectory;
            }
        }

        public bool IsAutoforwardEnabled
        {
            get
            {
                return Globals.IsAutoforwardEnabled();
            }
        }

        public IEnumerable<string> ChannelNames
        {
            get
            {
                foreach (ChannelProperties stcChannel in Channels.Entries.Values)
                {
                    if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                    {
                        string name = stcChannel.ChannelName.Trim();
                        if (!string.IsNullOrEmpty(name))
                        {
                            yield return name;
                        }
                    }
                }
            }
        }

        public bool HasChannelList
        {
            get
            {
                return Channels.HasChannelList(false);
            }
        }

        public bool IsAccount(string name)
        {
            return Channels.IsAccount(name);
        }

        public bool IsChannel(string name)
        {
            return Channels.IsChannel(name);
        }

        public bool IsValidChannelName(string channelName)
        {
            return Globals.IsValidFileName(channelName);
        }

        public bool ChannelExists(string channelName)
        {
            return Channels.Entries.ContainsKey(channelName);
        }

        public void RemoveChannel(string channelName)
        {
            Channels.RemoveChannel(channelName);
            Channels.FillChannelCollection();

            Globals.Settings.Save("Properties", "Last Pactor Channel", "");
            if (Globals.cllFastStart.Contains(channelName))
            {
                Globals.cllFastStart.Remove(channelName);
            }
        }

        public void AddChannel(
            string channelName, int priority, bool enabled, int tncTimeout, string tncSerialPort, 
            string tncBaudRate, string tncConfigFile, bool firstUseOnly, string tncType,
            bool enableAutoForward, string audioToneCenter, string radioBaud, string radioPort,
            string radioModel, string civAddress, bool narrowFilter, bool viaPTCII, bool viaSerial,
            int fskLevel, int pskLevel, bool busyHold, bool useLongPath, bool pactorIdEnabled,
            string remoteCallsign, string centerFreq, bool ttlLevel, bool nmea)
        {
            var stcChannel = default(ChannelProperties);
            stcChannel = PopulateChannelProperties(
                channelName, priority, enabled, tncTimeout, tncSerialPort, tncBaudRate, 
                tncConfigFile, firstUseOnly, tncType, enableAutoForward, audioToneCenter, 
                radioBaud, radioPort, radioModel, civAddress, narrowFilter, viaPTCII, 
                viaSerial, fskLevel, pskLevel, busyHold, useLongPath, pactorIdEnabled, 
                remoteCallsign, centerFreq, ttlLevel, nmea, stcChannel);

            Channels.AddChannel(ref stcChannel);
            Channels.FillChannelCollection();

            LastPactorChannel = channelName;
        }

        public void UpdateChannel(
            string channelName, int priority, bool enabled, int tncTimeout, string tncSerialPort,
            string tncBaudRate, string tncConfigFile, bool firstUseOnly, string tncType,
            bool enableAutoForward, string audioToneCenter, string radioBaud, string radioPort,
            string radioModel, string civAddress, bool narrowFilter, bool viaPTCII, bool viaSerial,
            int fskLevel, int pskLevel, bool busyHold, bool useLongPath, bool pactorIdEnabled,
            string remoteCallsign, string centerFreq, bool ttlLevel, bool nmea)
        {
            var stcChannel = default(ChannelProperties);
            stcChannel = PopulateChannelProperties(
                channelName, priority, enabled, tncTimeout, tncSerialPort, tncBaudRate,
                tncConfigFile, firstUseOnly, tncType, enableAutoForward, audioToneCenter,
                radioBaud, radioPort, radioModel, civAddress, narrowFilter, viaPTCII,
                viaSerial, fskLevel, pskLevel, busyHold, useLongPath, pactorIdEnabled,
                remoteCallsign, centerFreq, ttlLevel, nmea, stcChannel);

            Channels.UpdateChannel(ref stcChannel);
            Channels.FillChannelCollection();

            if (Globals.cllFastStart.Contains(channelName))
                Globals.cllFastStart.Remove(channelName);

            LastPactorChannel = channelName;
        }

        public void GetChannelInfo(
            string channelName, out bool firstUseOnly, out int priority, 
            out int activityTimeout, out bool enabled, out bool autoForward,
            out bool busyHold, out string tncSerialPort, out string tncBaudRate,
            out string tncType, out string tncConfigurationFile, out string audioToneCenter,
            out string radioModel, out string civAddress, out bool narrowFilter,
            out string radioBaud, out string radioPort, out bool radioManual,
            out bool radioSerial, out bool viaPTCII, out int fskLevel,
            out int pskLevel, out bool pactorIdEnabled, out bool ttlLevel,
            out bool nmea, out string remoteCallsign, out string centerFrequency,
            out bool longPath)
        {
            var stcSelectedChannel = (ChannelProperties)Channels.Entries[channelName];

            priority = stcSelectedChannel.Priority;
            activityTimeout = stcSelectedChannel.TNCTimeout;
            enabled = stcSelectedChannel.Enabled;

            if (IsAutoforwardEnabled)
            {
                autoForward = stcSelectedChannel.EnableAutoforward;
            }
            else
            {
                autoForward = false;
            }

            tncSerialPort = stcSelectedChannel.TNCSerialPort;
            tncBaudRate = stcSelectedChannel.TNCBaudRate;
            tncType = stcSelectedChannel.TNCType;
            firstUseOnly = stcSelectedChannel.TNCConfigureOnFirstUseOnly;
            tncConfigurationFile = stcSelectedChannel.TNCConfigurationFile;
            audioToneCenter = stcSelectedChannel.AudioToneCenter;
            radioModel = stcSelectedChannel.RDOModel;
            civAddress = stcSelectedChannel.CIVAddress;
            narrowFilter = stcSelectedChannel.NarrowFilter;
            radioBaud = stcSelectedChannel.RDOControlBaud;
            radioPort = stcSelectedChannel.RDOControlPort;
            radioManual = stcSelectedChannel.RDOControl == "Manual";
            radioSerial = stcSelectedChannel.RDOControl == "Serial";
            viaPTCII = stcSelectedChannel.RDOControl == "Via PTCII";
            fskLevel = stcSelectedChannel.TNCFSKLevel;
            pskLevel = stcSelectedChannel.TNCPSKLevel;
            pactorIdEnabled = stcSelectedChannel.PactorId;
            busyHold = stcSelectedChannel.TNCBusyHold;
            ttlLevel = stcSelectedChannel.TTLLevel;
            nmea = stcSelectedChannel.NMEA;
            remoteCallsign = stcSelectedChannel.RemoteCallsign;
            centerFrequency = stcSelectedChannel.RDOCenterFrequency;
            longPath = stcSelectedChannel.PactorUseLongPath;
        }

        public string[] GetChannelList()
        {
            return Channels.ParseChannelList(false);
        }

        public bool ContainsUsableFrequency(string freqList, string tncType)
        {
            return Globals.AnyUseableFrequency(freqList, tncType);
        }

        public string FormatFrequency(string freq)
        {
            return Globals.FormatFrequency(freq);
        }

        public bool CanUseFrequency(string freq, string tncType)
        {
            return Globals.CanUseFrequency(freq, tncType);
        }

        public string DownloadChannelList()
        {
            return Channels.GetChannelRecords(false, Globals.strServiceCodes);
        }

        private static ChannelProperties PopulateChannelProperties(
            string channelName, int priority, bool enabled, int tncTimeout, 
            string tncSerialPort, string tncBaudRate, string tncConfigFile, 
            bool firstUseOnly, string tncType, bool enableAutoForward, 
            string audioToneCenter, string radioBaud, string radioPort, 
            string radioModel, string civAddress, bool narrowFilter, 
            bool viaPTCII, bool viaSerial, int fskLevel, int pskLevel, 
            bool busyHold, bool useLongPath, bool pactorIdEnabled,
            string remoteCallsign, string centerFreq, bool ttlLevel,
            bool nmea, ChannelProperties stcChannel)
        {
            stcChannel.ChannelType = ChannelMode.PactorTNC; // TODO: Needs error checking for some parameters
            stcChannel.ChannelName = channelName;
            stcChannel.Priority = priority;
            stcChannel.Enabled = enabled;
            stcChannel.TNCTimeout = tncTimeout;
            stcChannel.TNCSerialPort = tncSerialPort;
            stcChannel.TNCBaudRate = tncBaudRate;
            stcChannel.TNCConfigurationFile = tncConfigFile;
            stcChannel.TNCConfigureOnFirstUseOnly = firstUseOnly;
            stcChannel.TNCType = tncType;

            switch (tncType)
            {
                case "KAM/+":
                case "KAMXL":
                    {
                        stcChannel.TNCPort = 2;
                        break;
                    }

                case "KAM98":
                case "PK-232": // , "PK-900"
                    {
                        stcChannel.TNCPort = 1;  // TODO: needs verification, not sure of correct port for XL  and 98
                        break;
                    }

                case "PTC II":
                case "PTC IIe":
                case "PTC IIex":
                case "PTC IIpro":
                case "PTC IIusb":
                case "PTC DR-7800":
                    {
                        stcChannel.TNCPort = 31; // port 31 for SCS Pactor 
                        break;
                    }
            }

            stcChannel.EnableAutoforward = enableAutoForward;
            stcChannel.AudioToneCenter = audioToneCenter;
            stcChannel.RDOControlBaud = radioBaud;
            stcChannel.RDOControlPort = radioPort;
            stcChannel.RDOModel = radioModel;
            stcChannel.CIVAddress = civAddress;
            stcChannel.NarrowFilter = narrowFilter;
            if (viaPTCII)
            {
                stcChannel.RDOControl = "Via PTCII";
            }
            else if (viaSerial)
            {
                stcChannel.RDOControl = "Serial";
            }
            else
            {
                stcChannel.RDOControl = "Manual";
            }

            stcChannel.TNCFSKLevel = fskLevel;
            stcChannel.TNCPSKLevel = pskLevel;
            stcChannel.TNCBusyHold = busyHold;
            stcChannel.PactorUseLongPath = useLongPath;
            stcChannel.PactorId = pactorIdEnabled;
            stcChannel.RemoteCallsign = remoteCallsign;
            stcChannel.RDOCenterFrequency = centerFreq;
            stcChannel.TTLLevel = ttlLevel;
            stcChannel.NMEA = nmea;
            return stcChannel;
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
