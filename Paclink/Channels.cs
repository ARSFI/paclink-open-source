using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using WinlinkServiceClasses;

namespace Paclink
{
    public enum EChannelModes
    {
        Telnet,
        PacketAGW,
        PacketTNC,
        PactorTNC,
        Winmor
        // Additional mode names added here as the program is expanded for other devices or protocols.
    } // EChannelClass

    public struct TChannelProperties
    {
        // 
        // This structure holds the properties for any declared channel. The
        // Channels class, declared below, is used to save and recall the
        // declared channels. The value held in ChannelProperties.ChannelType
        // is used to determine which of the members of ChannelProperties is
        // saved to or recovered from the registry for any given channel...
        // 
        // All channels types...
        public DateTime StartTimestamp;
        public EChannelModes ChannelType;
        public string ChannelName;
        public int Priority;
        public string RemoteCallsign;
        // Public FrequenciesScanned As Integer
        public bool Enabled;
        public bool EnableAutoforward;

        // Telnet specific properties...
        // Public LocalIPAddressIndex As Integer

        // PacketAGW specific properties...
        public string AGWPort;
        public int AGWTimeout;
        public int AGWPacketLength;
        public int AGWMaxFrames;
        public string AGWScript;
        public int AGWScriptTimeout;
        public bool AGWAllowInbound;

        // Packet and Pactor TNC specific properties...
        public string TNCType;
        public int TNCTimeout;
        public string TNCSerialPort;
        public string TNCBaudRate;
        public string TNCConfigurationFile;
        public bool TNCConfigureOnFirstUseOnly;
        public string TNCScript; // Not used in Pactor
        public int TNCScriptTimeout; // Not used in Pactor
        public int TNCFSKLevel;
        public int TNCPSKLevel;
        public int TNCPort;
        public bool TNCBusyHold;
        public int TNCOnAirBaud;

        // WINMOR Channel Specific Properties
        public string WMCaptureDevice;
        public string WMPlaybackDevice;
        public int WMXmitLevel;
        public int WMBandwidth;
        public string WMPTTControl;
        public string WMSerialCtrlSignals;
        public bool WMcwID;
        public bool WMDebugLog;

        // Radio control specific properties...
        public string RDOControl;      // Manual or Direct Serial
        public string RDOControlPort;  // Used for radio control
        public string RDOControlBaud;  // Baud rate for radio control (8N1 assumed)
        public string RDOCenterFrequency;   // Center frequency last selected in KHZ and may include " (p3)" tag
        public string RDOModel;        // The radio model 
        public string AudioToneCenter; // The center (average) of the Mark/Space tones set from the initialization file
        public string CIVAddress;      // The CIV address (Icom radios only)
        public bool NarrowFilter;   // True to use narrow filter for P1 and P2
        public bool PactorId;       // Flag to indicate whether to ID on Pactor 1 FEC
        public bool TTLLevel;       // Flag to indicate PTC II TTL or RS-232 Levels
        public bool NMEA;           // Flag to indicate use of NMEA commands (Icom only)
    } // TChannel

    class Channels
    {
        // 
        // This class consists of only shared variables and methods for 
        // saving, updating, recalling, and deleting channel declarations.
        // 
        public static Collection Entries; // Holds a memory image of all channel structures

        public static bool IsChannel(string strChannelName)
        {
            // 
            // Return true if the channel name is found in the registry list of channels.
            // 
            string strChannelNames = Globals.objINIFile.GetString("Properties", "Channel Names", "");
            if (string.IsNullOrEmpty(strChannelNames))
                return false;
            if (strChannelNames.IndexOf(strChannelName + "|") != -1)
                return true;
            else
                return false;
        } // IsChannel

        public static bool IsAccount(string strAccountName)
        {
            // 
            // Return true if the account name is found in the registry list of channels.
            // 
            string strAccountNames = Globals.objINIFile.GetString("Properties", "Account Names", "");
            if (string.IsNullOrEmpty(strAccountNames))
                return false;
            if (strAccountName.IndexOf(strAccountName + "|") != -1)
                return true;
            else
                return false;
        } // IsAccount

        public static void FillChannelCollection()
        {
            // 
            // Fill the Channels.Entries collection with the property structures of
            // all of the defined channels.
            // 
            Entries = new Collection();
            string strChannelNames = Globals.objINIFile.GetString("Properties", "Channel Names", "");
            var strChannelList = strChannelNames.Split('|');
            foreach (var strName in strChannelList)
            {
                if (!string.IsNullOrEmpty(strName))
                {
                    var stcChannel = default(TChannelProperties);
                    stcChannel.ChannelType = (EChannelModes)Enum.Parse(typeof(EChannelModes), Globals.objINIFile.GetString(strName, "Channel Type", "0"));
                    stcChannel.ChannelName = Globals.objINIFile.GetString(strName, "Channel Name", "");
                    stcChannel.Priority = Globals.objINIFile.GetInteger(strName, "Priority", 5);
                    stcChannel.RemoteCallsign = Globals.objINIFile.GetString(strName, "Remote Callsign", "");
                    // .FrequenciesScanned = objINIFile.GetInteger(strName, "Frequencies Scanned", 5)
                    stcChannel.Enabled = Globals.objINIFile.GetBoolean(strName, "Enabled", false);
                    var switchExpr = stcChannel.ChannelType;
                    switch (switchExpr)
                    {
                        case EChannelModes.Telnet:
                            {
                                // Telnet only properties...
                                stcChannel.EnableAutoforward = Globals.objINIFile.GetBoolean(strName, "Enable Autoforward", true);
                                break;
                            }
                        // .LocalIPAddressIndex = objINIFile.GetInteger(strName, "LocalIP Address Index", 0")
                        case EChannelModes.PacketAGW:
                            {
                                // Packet AGC only properties...
                                stcChannel.AGWPort = Globals.objINIFile.GetString(strName, "AGW Port", "");
                                stcChannel.AGWTimeout = Globals.objINIFile.GetInteger(strName, "AGW Timeout", 4);
                                stcChannel.AGWPacketLength = Globals.objINIFile.GetInteger(strName, "AGW Packet Length", 128);
                                stcChannel.AGWMaxFrames = Globals.objINIFile.GetInteger(strName, "AGW Max Frames", 2);
                                stcChannel.AGWScriptTimeout = Globals.objINIFile.GetInteger(strName, "AGW Script Timeout", 60);
                                stcChannel.AGWAllowInbound = Globals.objINIFile.GetBoolean(strName, "AGW Allow Inbound", false);
                                stcChannel.AGWScript = GetScript(stcChannel.ChannelName);
                                stcChannel.EnableAutoforward = Globals.objINIFile.GetBoolean(strName, "Enable Autoforward", true);
                                break;
                            }

                        case EChannelModes.PacketTNC:
                            {
                                // Packet TNC only properties...
                                stcChannel.TNCType = Globals.objINIFile.GetString(strName, "TNC Type", "");
                                stcChannel.TNCPort = Globals.objINIFile.GetInteger(strName, "TNC Port", 1);
                                stcChannel.TNCTimeout = Math.Max(10, Globals.objINIFile.GetInteger(strName, "TNC Timeout", 10));
                                stcChannel.TNCSerialPort = Globals.objINIFile.GetString(strName, "TNC Serial Port", "");
                                stcChannel.TNCBaudRate = Globals.objINIFile.GetString(strName, "TNC Baud Rate", "9600");
                                stcChannel.TNCConfigurationFile = Globals.objINIFile.GetString(strName, "TNC Configuration File", "");
                                stcChannel.TNCConfigureOnFirstUseOnly = Globals.objINIFile.GetBoolean(strName, "TNC Configuration On First Use Only", false);
                                stcChannel.TNCScript = GetScript(stcChannel.ChannelName);
                                stcChannel.TNCScriptTimeout = Globals.objINIFile.GetInteger(strName, "TNC Script Timeout", 60);
                                // .TNCAllowInbound = (objPropertiesFile.GetBoolean(strName, "TNC Allow Inbound", False)
                                stcChannel.EnableAutoforward = Globals.objINIFile.GetBoolean(strName, "Enable Autoforward", true);
                                stcChannel.RDOControl = Globals.objINIFile.GetString(strName, "Radio Control", "Manual");
                                stcChannel.RDOControlPort = Globals.objINIFile.GetString(strName, "Radio Control Port", "");
                                stcChannel.RDOControlBaud = Globals.objINIFile.GetString(strName, "Radio Control Baud", "4800");
                                stcChannel.RDOModel = Globals.objINIFile.GetString(strName, "Radio Model", "");
                                if (stcChannel.RDOModel == "Icom (other C-IV)")
                                    stcChannel.RDOModel = "Icom (other CI-V)";
                                stcChannel.RDOCenterFrequency = Globals.objINIFile.GetString(strName, "Center Frequency", "");
                                stcChannel.CIVAddress = Globals.objINIFile.GetString(strName, "CIV Address", "04");
                                stcChannel.TTLLevel = Globals.objINIFile.GetBoolean(strName, "PTC II TTL Level", false);
                                stcChannel.NMEA = Globals.objINIFile.GetBoolean(strName, "Use NMEA Commands", false);
                                stcChannel.TNCOnAirBaud = Globals.objINIFile.GetInteger(strName, "TNC On-Air Baud", 1200);
                                break;
                            }

                        case EChannelModes.PactorTNC:
                            {
                                // Pactor TNC only properties...
                                stcChannel.TNCType = Globals.objINIFile.GetString(strName, "TNC Type", "");
                                stcChannel.TNCPort = Globals.objINIFile.GetInteger(strName, "TNC Port", 1);
                                stcChannel.TNCTimeout = Globals.objINIFile.GetInteger(strName, "TNC Timeout", 4);
                                stcChannel.TNCSerialPort = Globals.objINIFile.GetString(strName, "TNC Serial Port", "");
                                stcChannel.TNCBaudRate = Globals.objINIFile.GetString(strName, "TNC Baud Rate", "9600");
                                stcChannel.TNCConfigurationFile = Globals.objINIFile.GetString(strName, "TNC Configuration File", "");
                                stcChannel.TNCConfigureOnFirstUseOnly = Globals.objINIFile.GetBoolean(strName, "TNC Configuration On First Use Only", false);
                                // .TNCAllowInbound = objPropertiesFile.GetBoolean(strName, "TNC Allow Inbound", False)
                                stcChannel.TNCBusyHold = Globals.objINIFile.GetBoolean(strName, "TNC Busy Hold", true);
                                stcChannel.EnableAutoforward = Globals.objINIFile.GetBoolean(strName, "Enable Autoforward", false);
                                stcChannel.RDOControl = Globals.objINIFile.GetString(strName, "Radio Control", "Manual");
                                stcChannel.RDOControlPort = Globals.objINIFile.GetString(strName, "Radio Control Port", "");
                                stcChannel.RDOControlBaud = Globals.objINIFile.GetString(strName, "Radio Control Baud", "4800");
                                stcChannel.RDOModel = Globals.objINIFile.GetString(strName, "Radio Model", "");
                                if (stcChannel.RDOModel == "Icom (other C-IV)")
                                    stcChannel.RDOModel = "Icom (other CI-V)";
                                stcChannel.RDOCenterFrequency = Globals.objINIFile.GetString(strName, "Center Frequency", "");
                                // .MBOType = objPropertiesFile.GetString(strName, "MBO Type", "Public")
                                stcChannel.AudioToneCenter = Globals.objINIFile.GetString(strName, "Audio Tone Center", "1500");
                                stcChannel.CIVAddress = Globals.objINIFile.GetString(strName, "CIV Address", "04");
                                stcChannel.NarrowFilter = Globals.objINIFile.GetBoolean(strName, "Narrow Filter", false);
                                stcChannel.TNCPSKLevel = Globals.objINIFile.GetInteger(strName, "TNC PSK Level", 100);
                                stcChannel.TNCFSKLevel = Globals.objINIFile.GetInteger(strName, "TNC FSK Level", 100);
                                stcChannel.PactorId = Globals.objINIFile.GetBoolean(strName, "Pactor 1 ID", true);
                                stcChannel.TTLLevel = Globals.objINIFile.GetBoolean(strName, "PTC II TTL Level", false);
                                stcChannel.NMEA = Globals.objINIFile.GetBoolean(strName, "Use NMEA Commands", false);
                                break;
                            }

                        case EChannelModes.Winmor:
                            {
                                stcChannel.TNCBusyHold = Globals.objINIFile.GetBoolean(strName, "TNC Busy Hold", true);
                                stcChannel.EnableAutoforward = Globals.objINIFile.GetBoolean(strName, "Enable Autoforward", false);
                                stcChannel.RDOControl = Globals.objINIFile.GetString(strName, "Radio Control", "Manual");
                                stcChannel.RDOControlPort = Globals.objINIFile.GetString(strName, "Radio Control Port", "");
                                stcChannel.RDOControlBaud = Globals.objINIFile.GetString(strName, "Radio Control Baud", "4800");
                                stcChannel.RDOModel = Globals.objINIFile.GetString(strName, "Radio Model", "");
                                if (stcChannel.RDOModel == "Icom (other C-IV)")
                                    stcChannel.RDOModel = "Icom (other CI-V)";
                                stcChannel.RDOCenterFrequency = Globals.objINIFile.GetString(strName, "Center Frequency", "");
                                stcChannel.AudioToneCenter = Globals.objINIFile.GetString(strName, "Audio Tone Center", "1500");
                                stcChannel.CIVAddress = Globals.objINIFile.GetString(strName, "CIV Address", "04");
                                stcChannel.NarrowFilter = Globals.objINIFile.GetBoolean(strName, "Narrow Filter", false);
                                stcChannel.NMEA = Globals.objINIFile.GetBoolean(strName, "Use NMEA Commands", false);
                                stcChannel.WMBandwidth = Globals.objINIFile.GetInteger(strName, "WINMOR BW", 500);
                                stcChannel.WMCaptureDevice = Globals.objINIFile.GetString(strName, "WM CaptureDevice", "");
                                stcChannel.WMPlaybackDevice = Globals.objINIFile.GetString(strName, "WM PlaybackDevice", "");
                                stcChannel.WMPTTControl = Globals.objINIFile.GetString(strName, "WM PTT Control", "VOX");
                                stcChannel.WMSerialCtrlSignals = Globals.objINIFile.GetString(strName, "WM Serial Ctrl Signals", "");
                                stcChannel.WMXmitLevel = Globals.objINIFile.GetInteger(strName, "WM Xmit Level", 100);
                                stcChannel.WMcwID = Globals.objINIFile.GetBoolean(strName, "WM CW ID", true);
                                stcChannel.WMDebugLog = Globals.objINIFile.GetBoolean(strName, "WM DebugLog", true);
                                break;
                            }
                    }

                    if (Entries.Contains(strName) == false)
                    {
                        Entries.Add(stcChannel, stcChannel.ChannelName);
                    }
                    else
                    {
                        // If a duplicate entry is discovered in Channel Names then remove it...
                        string strChannelString = Globals.objINIFile.GetString("Properties", "Channel Names", "");
                        strChannelString = strChannelString.Replace(stcChannel.ChannelName + "|", "");
                        strChannelString += stcChannel.ChannelName + "|";
                        Globals.objINIFile.WriteString("Properties", "Channel Names", strChannelString);
                    }
                }
            }
        } // FillChannelCollection

        public static void AddChannel(ref TChannelProperties stcChannel)
        {
            // 
            // Adds the channel properties of a new channel to the registry.
            // 
            string strName = stcChannel.ChannelName;
            if (!string.IsNullOrEmpty(strName))
            {
                if (!IsChannel(strName))
                {
                    string strChannelNames = Globals.objINIFile.GetString("Properties", "Channel Names", "");
                    strChannelNames = strChannelNames + strName + "|";
                    Globals.objINIFile.WriteString("Properties", "Channel Names", strChannelNames);
                }

                UpdateProperties(ref stcChannel);
            }
        } // SaveChannel

        public static void RemoveChannel(string strChannelName)
        {
            // 
            // Removes the properties for the channel named in strChannelName from the registry.
            // 
            string strChannelNames = Globals.objINIFile.GetString("Properties", "Channel Names", "");
            while (strChannelNames.IndexOf(strChannelName + "|") != -1) // Added to remove any duplicates
                strChannelNames = strChannelNames.Replace(strChannelName + "|", "");
            Globals.objINIFile.WriteString("Properties", "Channel Names", strChannelNames);
            Globals.objINIFile.DeleteSection(strChannelName);
        } // RemoveChannel

        public static void UpdateChannel(ref TChannelProperties stcChannel)
        {
            // 
            // Updates the properties for the channel represented in stcChannel.
            // 
            string strName = stcChannel.ChannelName;
            if (IsChannel(strName))
                UpdateProperties(ref stcChannel);
        } // UpdateChannel

        private static void UpdateProperties(ref TChannelProperties stcChannel)
        {
            // 
            // Updates the registry as required for the properties carried in strChannel.
            // 
            string strName = stcChannel.ChannelName;
            Globals.objINIFile.WriteString(strName, "Channel Type", stcChannel.ChannelType.ToString());
            Globals.objINIFile.WriteString(strName, "Channel Name", stcChannel.ChannelName);
            Globals.objINIFile.WriteInteger(strName, "Priority", stcChannel.Priority);
            Globals.objINIFile.WriteString(strName, "Remote Callsign", stcChannel.RemoteCallsign);
            // objPropertiesFile.WriteInteger(strName, "Frequencies Scanned", .FrequenciesScanned)
            Globals.objINIFile.WriteBoolean(strName, "Enabled", stcChannel.Enabled);
            if (stcChannel.ChannelType == EChannelModes.Telnet)
            {
                // Telnet only properties...
                Globals.objINIFile.WriteBoolean(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
            }
            // objPropertiesFile.WriteString(strName, "LocalIP Address Index", .LocalIPAddressIndex.ToString)
            else if (stcChannel.ChannelType == EChannelModes.PacketAGW)
            {
                // Packet AGW only properties...
                Globals.objINIFile.WriteString(strName, "AGW Port", stcChannel.AGWPort);
                Globals.objINIFile.WriteInteger(strName, "AGW Timeout", stcChannel.AGWTimeout);
                Globals.objINIFile.WriteInteger(strName, "AGW Packet Length", stcChannel.AGWPacketLength);
                Globals.objINIFile.WriteInteger(strName, "AGW Max Frames", stcChannel.AGWMaxFrames);
                Globals.objINIFile.WriteInteger(strName, "AGW Script Timeout", stcChannel.AGWScriptTimeout);
                Globals.objINIFile.WriteBoolean(strName, "AGW Allow Inbound", stcChannel.AGWAllowInbound);
                Globals.objINIFile.WriteBoolean(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                SaveScript(strName, stcChannel.AGWScript);
            }
            else if (stcChannel.ChannelType == EChannelModes.PacketTNC)
            {
                // Packet TNC only properties...
                Globals.objINIFile.WriteString(strName, "TNC Type", stcChannel.TNCType);
                Globals.objINIFile.WriteInteger(strName, "TNC Port", stcChannel.TNCPort);
                Globals.objINIFile.WriteInteger(strName, "TNC Timeout", stcChannel.TNCTimeout);
                Globals.objINIFile.WriteString(strName, "TNC Serial Port", stcChannel.TNCSerialPort);
                Globals.objINIFile.WriteString(strName, "TNC Baud Rate", stcChannel.TNCBaudRate);
                Globals.objINIFile.WriteString(strName, "TNC Configuration File", stcChannel.TNCConfigurationFile);
                Globals.objINIFile.WriteBoolean(strName, "TNC Configuration On First Use Only", stcChannel.TNCConfigureOnFirstUseOnly);
                Globals.objINIFile.WriteInteger(strName, "TNC Script Timeout", stcChannel.TNCScriptTimeout);
                // objPropertiesFile.WriteBoolean(strName, "TNC Allow Inbound", .TNCAllowInbound)
                Globals.objINIFile.WriteBoolean(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                Globals.objINIFile.WriteString(strName, "Radio Control", stcChannel.RDOControl);
                Globals.objINIFile.WriteString(strName, "Radio Control Port", stcChannel.RDOControlPort);
                Globals.objINIFile.WriteString(strName, "Radio Control Baud", stcChannel.RDOControlBaud);
                Globals.objINIFile.WriteString(strName, "Radio Model", stcChannel.RDOModel);
                Globals.objINIFile.WriteString(strName, "Center Frequency", stcChannel.RDOCenterFrequency);
                Globals.objINIFile.WriteString(strName, "Audio Tone Center", stcChannel.AudioToneCenter);
                Globals.objINIFile.WriteString(strName, "CIV Address", stcChannel.CIVAddress);
                Globals.objINIFile.WriteBoolean(strName, "PTC II TTL Level", stcChannel.TTLLevel);
                Globals.objINIFile.WriteBoolean(strName, "Use NMEA Commands", stcChannel.NMEA);
                Globals.objINIFile.WriteInteger(strName, "TNC On-Air Baud", stcChannel.TNCOnAirBaud);
                SaveScript(strName, stcChannel.TNCScript);
            }
            else if (stcChannel.ChannelType == EChannelModes.PactorTNC)
            {
                // Pactor TNC only properties...
                Globals.objINIFile.WriteString(strName, "TNC Type", stcChannel.TNCType);
                Globals.objINIFile.WriteInteger(strName, "TNC Port", stcChannel.TNCPort);
                Globals.objINIFile.WriteInteger(strName, "TNC Timeout", stcChannel.TNCTimeout);
                Globals.objINIFile.WriteString(strName, "TNC Serial Port", stcChannel.TNCSerialPort);
                Globals.objINIFile.WriteString(strName, "TNC Baud Rate", stcChannel.TNCBaudRate);
                Globals.objINIFile.WriteString(strName, "TNC Configuration File", stcChannel.TNCConfigurationFile);
                Globals.objINIFile.WriteBoolean(strName, "TNC Configuration On First Use Only", stcChannel.TNCConfigureOnFirstUseOnly);
                Globals.objINIFile.WriteBoolean(strName, "TNC Busy Hold", stcChannel.TNCBusyHold);
                Globals.objINIFile.WriteBoolean(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                Globals.objINIFile.WriteString(strName, "Radio Control", stcChannel.RDOControl);
                Globals.objINIFile.WriteString(strName, "Radio Control Port", stcChannel.RDOControlPort);
                Globals.objINIFile.WriteString(strName, "Radio Control Baud", stcChannel.RDOControlBaud);
                Globals.objINIFile.WriteString(strName, "Radio Model", stcChannel.RDOModel);
                Globals.objINIFile.WriteString(strName, "Center Frequency", stcChannel.RDOCenterFrequency);
                Globals.objINIFile.WriteString(strName, "Audio Tone Center", stcChannel.AudioToneCenter);
                Globals.objINIFile.WriteString(strName, "CIV Address", stcChannel.CIVAddress);
                Globals.objINIFile.WriteBoolean(strName, "Narrow Filter", stcChannel.NarrowFilter);
                Globals.objINIFile.WriteInteger(strName, "TNC PSK Level", stcChannel.TNCPSKLevel);
                Globals.objINIFile.WriteInteger(strName, "TNC FSK Level", stcChannel.TNCFSKLevel);
                Globals.objINIFile.WriteBoolean(strName, "Pactor 1 ID", stcChannel.PactorId);
                Globals.objINIFile.WriteBoolean(strName, "PTC II TTL Level", stcChannel.TTLLevel);
                Globals.objINIFile.WriteBoolean(strName, "Use NMEA Commands", stcChannel.NMEA);
            }
            else if (stcChannel.ChannelType == EChannelModes.Winmor)
            {
                Globals.objINIFile.WriteBoolean(strName, "TNC Busy Hold", stcChannel.TNCBusyHold);
                Globals.objINIFile.WriteBoolean(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                Globals.objINIFile.WriteString(strName, "Radio Control", stcChannel.RDOControl);
                Globals.objINIFile.WriteString(strName, "Radio Control Port", stcChannel.RDOControlPort);
                Globals.objINIFile.WriteString(strName, "Radio Control Baud", stcChannel.RDOControlBaud);
                Globals.objINIFile.WriteString(strName, "Radio Model", stcChannel.RDOModel);
                Globals.objINIFile.WriteString(strName, "Center Frequency", stcChannel.RDOCenterFrequency);
                Globals.objINIFile.WriteString(strName, "Audio Tone Center", stcChannel.AudioToneCenter);
                Globals.objINIFile.WriteString(strName, "CIV Address", stcChannel.CIVAddress);
                Globals.objINIFile.WriteBoolean(strName, "Narrow Filter", stcChannel.NarrowFilter);
                Globals.objINIFile.WriteBoolean(strName, "Use NMEA Commands", stcChannel.NMEA);
                Globals.objINIFile.WriteInteger(strName, "WINMOR BW", stcChannel.WMBandwidth);
                Globals.objINIFile.WriteString(strName, "WM CaptureDevice", stcChannel.WMCaptureDevice);
                Globals.objINIFile.WriteString(strName, "WM PlaybackDevice", stcChannel.WMPlaybackDevice);
                Globals.objINIFile.WriteString(strName, "WM PTT Control", stcChannel.WMPTTControl);
                Globals.objINIFile.WriteString(strName, "WM Serial Ctrl Signals", stcChannel.WMSerialCtrlSignals);
                Globals.objINIFile.WriteInteger(strName, "WM Xmit Level", stcChannel.WMXmitLevel);
                Globals.objINIFile.WriteBoolean(strName, "WM CW ID", stcChannel.WMcwID);
                Globals.objINIFile.WriteBoolean(strName, "WM DebugLog", stcChannel.WMDebugLog);
            }
        } // UpdateProperties

        private static void SaveScript(string strChannel, string strScript)
        {
            // 
            // Saves a script in the \Data subdirectory for the channel named in strChannel.
            // 
            try
            {
                string strScriptFilePath = Globals.SiteRootDirectory + @"Data\" + strChannel + ".script";
                if (strScript is object && strScript.Length > 0)
                {
                    My.MyProject.Computer.FileSystem.WriteAllText(strScriptFilePath, strScript, false);
                }
                else if (File.Exists(strScriptFilePath))
                    File.Delete(strScriptFilePath);
            }
            catch
            {
                Logs.Exception("[Channels.SaveScript] " + Information.Err().Description);
            }
        } // SaveScript

        private static string GetScript(string strChannel)
        {
            // 
            // Gets a script from the \Data subdirectory for the channel named in strChannel.
            // 
            try
            {
                string strScriptFilePath = Globals.SiteRootDirectory + @"Data\" + strChannel + ".script";
                if (File.Exists(strScriptFilePath))
                {
                    return My.MyProject.Computer.FileSystem.ReadAllText(strScriptFilePath);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                Logs.Exception("[Channels.GetScript] " + Information.Err().Description);
            }

            return null;
        } // GetScript

        public static string GetChannelRecords(bool VHFchannels, string strServiceCodes)
        {
            // 
            // Update the HF channel list.
            // 
            string strCallSign;
            string strFreqEntry;
            string strSQL = "";
            var dicStation = new SortedDictionary<string, string>();

            // 
            // Try to get a list of either HF or VHF channels.
            // 
            var lstGateways = Globals.GetChannelsList(VHFchannels);
            // grdChannels.Focus()
            if (lstGateways == null)
            {
                return "Unable to download channel information from Winlink server.";
            }
            // 
            // Build a dictionary with channel callsign as the key and frequency entries as values.
            // 
            foreach (GatewayStatusRecord ObjStation in lstGateways)
            {
                strCallSign = ObjStation.Callsign;
                foreach (GatewayChannelRecord objChan in ObjStation.GatewayChannels)
                {
                    strFreqEntry = objChan.Frequency.ToString();
                    strFreqEntry += "|" + objChan.Gridsquare;
                    strFreqEntry += "|" + objChan.Mode.ToString();
                    strFreqEntry += "|" + objChan.OperatingHours.Replace(" ", "");
                    strFreqEntry += "|" + objChan.ServiceCode;
                    if (dicStation.ContainsKey(strCallSign))
                    {
                        dicStation[strCallSign] += "," + strFreqEntry;
                    }
                    else
                    {
                        dicStation.Add(strCallSign, strFreqEntry);
                    }
                }
            }
            // 
            // Create a string in the format we want to store the channels in.
            // 
            var sbdChannels = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in dicStation)
                sbdChannels.Append(kvp.Key + ":" + kvp.Value + Globals.CRLF);
            // 
            // Create the channel file.
            // 
            if (VHFchannels)
            {
                My.MyProject.Computer.FileSystem.WriteAllText(Globals.SiteRootDirectory + @"Data\RMS VHF Channels.dat", sbdChannels.ToString(), false);
            }
            else
            {
                My.MyProject.Computer.FileSystem.WriteAllText(Globals.SiteRootDirectory + @"Data\RMS Channels.dat", sbdChannels.ToString(), false);
            }

            return "";
        }

        public static string[] ParseChannelList(string strFilename)
        {
            // 
            // Function to parse the channel freq list (used for Public, EMComm and MARS
            // Returns an empty string array if error or file not found.
            // 
            var aryResult = new string[0];
            if (!File.Exists(strFilename))
                return aryResult;
            try
            {
                aryResult = File.ReadAllLines(strFilename);
            }
            catch
            {
                return aryResult;
            }

            return aryResult;
        } // ParseChannelList
    } // Channels
}