using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paclink
{
    public class DialogPacketTNCChannelViewModel : IPacketTNCChannelBacking
    {
        public string SiteRootDirectory
        {
            get
            {
                return Globals.SiteRootDirectory;
            }
        }

        public string LastPacketChannel
        {
            get
            {
                return Globals.Settings.Get("Properties", "Last Packet TNC Channel", "");
            }
            set
            {
                Globals.Settings.Save("Properties", "Last Packet TNC Channel", value);
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

        public bool HasChannelList
        {
            get
            {
                return Channels.HasChannelList(true);
            }
        }

        public IEnumerable<string> ChannelNames
        {
            get
            {
                foreach (ChannelProperties stcChannel in Channels.Entries.Values)
                {
                    if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                    {
                        if (!string.IsNullOrEmpty(stcChannel.ChannelName.Trim()))
                        {
                            yield return stcChannel.ChannelName;
                        }
                    }
                }
            }
        }

        public IEnumerable<string> ChannelList
        {
            get
            {
                foreach (var name in Channels.ParseChannelList(true))
                {
                    yield return name;
                }
            }
        }

        public bool ContainsChannel(string name)
        {
            return Channels.Entries.ContainsKey(name);
        }

        public bool IsAccount(string name)
        {
            return Channels.IsAccount(name);
        }

        public bool IsChannel(string name)
        {
            return Channels.IsChannel(name);
        }

        public bool IsValidChannelName(string name)
        {
            return Globals.IsValidFileName(name);
        }

        public bool IsFreqInLimits(string freq)
        {
            return Globals.WithinLimits(freq, 2400, 29);
        }

        public void AddChannel(string name, 
            string centerFrequency, int priority, string remoteCallsign, int activityTimeout,
            int scriptTimeout, string script, bool enabled, string serialPort,
            string tncType, int tncPort, string tncConfigurationFile,
            bool tncConfigureOnFirstUseOnly, string radioControlBaud, string radioControlPort,
            string radioModel, int onAirBaud, string civAddress,
            bool radioControlSerial, bool radioControlPTC, bool radioControlManual,
            bool radioTTLLevel, string freqMHZ, string tncBaudRate)
        {
            ChannelProperties stcChannel = new ChannelProperties();

            stcChannel.ChannelType = ChannelMode.PacketTNC;
            stcChannel.ChannelName = name;
            stcChannel.Priority = priority;
            stcChannel.RemoteCallsign = CleanupCallSign(remoteCallsign);
            stcChannel.RDOCenterFrequency = centerFrequency;
            stcChannel.Enabled = enabled;
            stcChannel.TNCTimeout = activityTimeout;
            stcChannel.TNCScript = script;
            stcChannel.TNCScriptTimeout = scriptTimeout;
            stcChannel.TNCSerialPort = serialPort;
            stcChannel.TNCBaudRate = tncBaudRate;
            stcChannel.TNCConfigurationFile = tncConfigurationFile;
            stcChannel.TNCConfigureOnFirstUseOnly = tncConfigureOnFirstUseOnly;
            stcChannel.TNCPort = tncPort;
            stcChannel.TNCType = tncType;
            stcChannel.TNCOnAirBaud = onAirBaud;
            stcChannel.EnableAutoforward = true; // Packet Channels always enabled
            if (!radioControlManual)
            {
                stcChannel.RDOCenterFrequency = (1000 * double.Parse(freqMHZ, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00000.0");
            }

            stcChannel.RDOControlBaud = radioControlBaud;
            stcChannel.RDOControlPort = radioControlPort;
            stcChannel.RDOModel = radioModel;
            stcChannel.CIVAddress = civAddress.Trim().ToUpper();
            if (radioControlPTC)
            {
                stcChannel.RDOControl = "Via PTCII";
            }
            else if (radioControlSerial)
            {
                stcChannel.RDOControl = "Serial";
            }
            else
            {
                stcChannel.RDOControl = "Manual";
            }

            stcChannel.TTLLevel = radioTTLLevel;

            Channels.AddChannel(ref stcChannel);
            Channels.FillChannelCollection();
        }

        public void UpdateChannel(string name,
            string centerFrequency, int priority, string remoteCallsign, int activityTimeout,
            int scriptTimeout, string script, bool enabled, string serialPort,
            string tncType, int tncPort, string tncConfigurationFile,
            bool tncConfigureOnFirstUseOnly, string radioControlBaud, string radioControlPort,
            string radioModel, int onAirBaud, string civAddress,
            bool radioControlSerial, bool radioControlPTC, bool radioControlManual,
            bool radioTTLLevel, string freqMHZ, string tncBaudRate)
        {
            ChannelProperties stcChannel = new ChannelProperties();

            stcChannel.ChannelType = ChannelMode.PacketTNC;
            stcChannel.ChannelName = name;
            stcChannel.Priority = priority;
            stcChannel.RemoteCallsign = CleanupCallSign(remoteCallsign);
            stcChannel.RDOCenterFrequency = centerFrequency;
            stcChannel.Enabled = enabled;
            stcChannel.TNCTimeout = activityTimeout;
            stcChannel.TNCScript = script;
            stcChannel.TNCScriptTimeout = scriptTimeout;
            stcChannel.TNCSerialPort = serialPort;
            stcChannel.TNCBaudRate = tncBaudRate;
            stcChannel.TNCConfigurationFile = tncConfigurationFile;
            stcChannel.TNCConfigureOnFirstUseOnly = tncConfigureOnFirstUseOnly;
            stcChannel.TNCPort = tncPort;
            stcChannel.TNCType = tncType;
            stcChannel.TNCOnAirBaud = onAirBaud;
            stcChannel.EnableAutoforward = true; // Packet Channels always enabled
            if (!radioControlManual)
            {
                stcChannel.RDOCenterFrequency = (1000 * double.Parse(freqMHZ, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00000.0");
            }

            stcChannel.RDOControlBaud = radioControlBaud;
            stcChannel.RDOControlPort = radioControlPort;
            stcChannel.RDOModel = radioModel;
            stcChannel.CIVAddress = civAddress.Trim().ToUpper();
            if (radioControlPTC)
            {
                stcChannel.RDOControl = "Via PTCII";
            }
            else if (radioControlSerial)
            {
                stcChannel.RDOControl = "Serial";
            }
            else
            {
                stcChannel.RDOControl = "Manual";
            }

            stcChannel.TTLLevel = radioTTLLevel;

            Channels.UpdateChannel(ref stcChannel);
            Channels.FillChannelCollection();

            if (Globals.cllFastStart.Contains(name))
                Globals.cllFastStart.Remove(name);
        }

        public void RemoveChannel(string name)
        {
            Channels.RemoveChannel(name);
            Channels.FillChannelCollection();

            if (Globals.cllFastStart.Contains(name))
            {
                Globals.cllFastStart.Remove(name);
            }
        }

        public void GetChannelInfo(
            string name, out string centerFrequency, out int priority, out int activityTimeout,
            out int scriptTimeout, out string script, out bool enabled, out string serialPort,
            out string tncType, out int tncPort, out string tncConfigurationFile, 
            out bool tncConfigureOnFirstUseOnly, out string radioControlBaud, out string radioControlPort,
            out string radioModel, out int onAirBaud, out string civAddress,
            out bool radioControlSerial, out bool radioControlPTC, out bool radioControlManual,
            out bool radioTTLLevel, out string freqMHZ, out string tncBaudRate, out string remoteCallsign)
        {
            var entry = Channels.Entries[name];

            centerFrequency = entry.RDOCenterFrequency;
            remoteCallsign = entry.RemoteCallsign;
            activityTimeout = Math.Max(10, entry.TNCTimeout);
            priority = entry.Priority;
            scriptTimeout = entry.TNCScriptTimeout;
            script = entry.TNCScript;
            enabled = entry.Enabled;
            serialPort = entry.TNCSerialPort;
            tncType = entry.TNCType;
            tncPort = entry.TNCPort;
            tncConfigurationFile = entry.TNCConfigurationFile;
            tncConfigureOnFirstUseOnly = entry.TNCConfigureOnFirstUseOnly;

            radioControlBaud = entry.RDOControlBaud;
            radioControlPort = entry.RDOControlPort;
            radioModel = entry.RDOModel;
            onAirBaud = entry.TNCOnAirBaud;
            civAddress = entry.CIVAddress;
            radioControlSerial = entry.RDOControl == "Serial";
            radioControlPTC = entry.RDOControl == "Via PTCII";
            radioControlManual = entry.RDOControl == "Manual";
            radioTTLLevel = entry.TTLLevel;

            try
            {
                string centerFreq = entry.RDOCenterFrequency;

                centerFreq = centerFreq.Substring(0, centerFreq.IndexOf('(')).Trim();
                freqMHZ = (0.001 * double.Parse(centerFreq, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00.000");
            }
            catch
            {
                freqMHZ = "";
            }

            tncBaudRate = entry.TNCBaudRate;
        }

        public string DownloadChannelList()
        {
            return Channels.GetChannelRecords(true, Globals.strServiceCodes);
        }

        public bool CanUseBaudAndFrequency(string freqList, string tncType, string baud)
        {
            return Globals.AnyUseableFrequency(freqList, tncType) && Globals.CanUseBaud(freqList, baud);
        }

        public string CleanupCallSign(string strCallsign)
        {
            // 
            // Remove the baud rate indicator.
            // 
            var strTokens = strCallsign.Split(' ');
            if (strTokens.Length < 1)
                return "";
            return strTokens[0].Trim().ToUpper();
        }

        public string[] GetUsableFreqs(string[] freqList, string tncType)
        {
            List<string> result = new List<string>();

            foreach (var freq in freqList)
            {
                if (Globals.CanUseFrequency(freq, tncType))
                {
                    result.Add(Globals.FormatFrequency(freq));
                }
            }

            return result.ToArray();
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
