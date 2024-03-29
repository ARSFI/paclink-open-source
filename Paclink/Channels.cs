﻿using NLog;
using Paclink.Data;
using Paclink.UI.Common;
using System;
using System.Collections.Generic;

namespace Paclink
{
    class Channels
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        // 
        // This class consists of only shared variables and methods for 
        // saving, updating, recalling, and deleting channel declarations.
        // 
        public static Dictionary<string, ChannelProperties> Entries; // Holds a memory image of all channel structures

        public static bool IsChannel(string strChannelName)
        {
            // 
            // Return true if the channel name is found in the registry list of channels.
            // 
            string strChannelNames = Globals.Settings.Get("Properties", "Channel Names", "");
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
            string strAccountNames = Globals.Settings.Get("Properties", "Account Names", "");
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
            Entries = new Dictionary<string, ChannelProperties>();
            string strChannelNames = Globals.Settings.Get("Properties", "Channel Names", "");
            var strChannelList = strChannelNames.Split('|');
            foreach (var strName in strChannelList)
            {
                if (!string.IsNullOrEmpty(strName))
                {
                    var stcChannel = default(ChannelProperties);
                    stcChannel.ChannelType = (ChannelMode)Enum.Parse(typeof(ChannelMode), Globals.Settings.Get(strName, "Channel Type", "0"));
                    stcChannel.ChannelName = Globals.Settings.Get(strName, "Channel Name", "");
                    stcChannel.Priority = Globals.Settings.Get(strName, "Priority", 5);
                    stcChannel.RemoteCallsign = Globals.Settings.Get(strName, "Remote Callsign", "");
                    // .FrequenciesScanned = objINIFile.GetInteger(strName, "Frequencies Scanned", 5)
                    stcChannel.Enabled = Globals.Settings.Get(strName, "Enabled", false);
                    var switchExpr = stcChannel.ChannelType;
                    switch (switchExpr)
                    {
                        case ChannelMode.Telnet:
                            {
                                // Telnet only properties...
                                stcChannel.EnableAutoforward = Globals.Settings.Get(strName, "Enable Autoforward", true);
                                break;
                            }
                        // .LocalIPAddressIndex = objINIFile.GetInteger(strName, "LocalIP Address Index", 0")
                        case ChannelMode.PacketAGW:
                            {
                                // Packet AGC only properties...
                                stcChannel.AGWPort = Globals.Settings.Get(strName, "AGW Port", "");
                                stcChannel.AGWTimeout = Globals.Settings.Get(strName, "AGW Timeout", 4);
                                stcChannel.AGWPacketLength = Globals.Settings.Get(strName, "AGW Packet Length", 128);
                                stcChannel.AGWMaxFrames = Globals.Settings.Get(strName, "AGW Max Frames", 2);
                                stcChannel.AGWScriptTimeout = Globals.Settings.Get(strName, "AGW Script Timeout", 60);
                                stcChannel.AGWAllowInbound = Globals.Settings.Get(strName, "AGW Allow Inbound", false);
                                stcChannel.AGWScript = GetScript(stcChannel.ChannelName);
                                stcChannel.EnableAutoforward = Globals.Settings.Get(strName, "Enable Autoforward", true);
                                break;
                            }

                        case ChannelMode.PacketTNC:
                            {
                                // Packet TNC only properties...
                                stcChannel.TNCType = Globals.Settings.Get(strName, "TNC Type", "");
                                stcChannel.TNCPort = Globals.Settings.Get(strName, "TNC Port", 1);
                                stcChannel.TNCTimeout = Math.Max(10, Globals.Settings.Get(strName, "TNC Timeout", 10));
                                stcChannel.TNCSerialPort = Globals.Settings.Get(strName, "TNC Serial Port", "");
                                stcChannel.TNCBaudRate = Globals.Settings.Get(strName, "TNC Baud Rate", "9600");
                                stcChannel.TNCConfigurationFile = Globals.Settings.Get(strName, "TNC Configuration File", "");
                                stcChannel.TNCConfigureOnFirstUseOnly = Globals.Settings.Get(strName, "TNC Configuration On First Use Only", false);
                                stcChannel.TNCScript = GetScript(stcChannel.ChannelName);
                                stcChannel.TNCScriptTimeout = Globals.Settings.Get(strName, "TNC Script Timeout", 60);
                                // .TNCAllowInbound = (objPropertiesFile.GetBoolean(strName, "TNC Allow Inbound", False)
                                stcChannel.EnableAutoforward = Globals.Settings.Get(strName, "Enable Autoforward", true);
                                stcChannel.RDOControl = Globals.Settings.Get(strName, "Radio Control", "Manual");
                                stcChannel.RDOControlPort = Globals.Settings.Get(strName, "Radio Control Port", "");
                                stcChannel.RDOControlBaud = Globals.Settings.Get(strName, "Radio Control Baud", "4800");
                                stcChannel.RDOModel = Globals.Settings.Get(strName, "Radio Model", "");
                                if (stcChannel.RDOModel == "Icom (other C-IV)")
                                    stcChannel.RDOModel = "Icom (other CI-V)";
                                stcChannel.RDOCenterFrequency = Globals.Settings.Get(strName, "Center Frequency", "");
                                stcChannel.CIVAddress = Globals.Settings.Get(strName, "CIV Address", "04");
                                stcChannel.TTLLevel = Globals.Settings.Get(strName, "PTC II TTL Level", false);
                                stcChannel.NMEA = Globals.Settings.Get(strName, "Use NMEA Commands", false);
                                stcChannel.TNCOnAirBaud = Globals.Settings.Get(strName, "TNC On-Air Baud", 1200);
                                break;
                            }

                        case ChannelMode.PactorTNC:
                            {
                                // Pactor TNC only properties...
                                stcChannel.TNCType = Globals.Settings.Get(strName, "TNC Type", "");
                                stcChannel.TNCPort = Globals.Settings.Get(strName, "TNC Port", 1);
                                stcChannel.TNCTimeout = Globals.Settings.Get(strName, "TNC Timeout", 4);
                                stcChannel.TNCSerialPort = Globals.Settings.Get(strName, "TNC Serial Port", "");
                                stcChannel.TNCBaudRate = Globals.Settings.Get(strName, "TNC Baud Rate", "9600");
                                stcChannel.TNCConfigurationFile = Globals.Settings.Get(strName, "TNC Configuration File", "");
                                stcChannel.TNCConfigureOnFirstUseOnly = Globals.Settings.Get(strName, "TNC Configuration On First Use Only", false);
                                // .TNCAllowInbound = objPropertiesFile.GetBoolean(strName, "TNC Allow Inbound", False)
                                stcChannel.TNCBusyHold = Globals.Settings.Get(strName, "TNC Busy Hold", true);
                                stcChannel.EnableAutoforward = Globals.Settings.Get(strName, "Enable Autoforward", false);
                                stcChannel.RDOControl = Globals.Settings.Get(strName, "Radio Control", "Manual");
                                stcChannel.RDOControlPort = Globals.Settings.Get(strName, "Radio Control Port", "");
                                stcChannel.RDOControlBaud = Globals.Settings.Get(strName, "Radio Control Baud", "4800");
                                stcChannel.RDOModel = Globals.Settings.Get(strName, "Radio Model", "");
                                if (stcChannel.RDOModel == "Icom (other C-IV)")
                                    stcChannel.RDOModel = "Icom (other CI-V)";
                                stcChannel.RDOCenterFrequency = Globals.Settings.Get(strName, "Center Frequency", "");
                                // .MBOType = objPropertiesFile.GetString(strName, "MBO Type", "Public")
                                stcChannel.AudioToneCenter = Globals.Settings.Get(strName, "Audio Tone Center", "1500");
                                stcChannel.CIVAddress = Globals.Settings.Get(strName, "CIV Address", "04");
                                stcChannel.NarrowFilter = Globals.Settings.Get(strName, "Narrow Filter", false);
                                stcChannel.TNCPSKLevel = Globals.Settings.Get(strName, "TNC PSK Level", 100);
                                stcChannel.TNCFSKLevel = Globals.Settings.Get(strName, "TNC FSK Level", 100);
                                stcChannel.PactorId = Globals.Settings.Get(strName, "Pactor 1 ID", true);
                                stcChannel.TTLLevel = Globals.Settings.Get(strName, "PTC II TTL Level", false);
                                stcChannel.NMEA = Globals.Settings.Get(strName, "Use NMEA Commands", false);
                                stcChannel.PactorUseLongPath = Globals.Settings.Get(strName, "Use Long Path", false);
                                break;
                            }

                        case ChannelMode.Winmor:
                            {
                                stcChannel.TNCBusyHold = Globals.Settings.Get(strName, "TNC Busy Hold", true);
                                stcChannel.EnableAutoforward = Globals.Settings.Get(strName, "Enable Autoforward", false);
                                stcChannel.RDOControl = Globals.Settings.Get(strName, "Radio Control", "Manual");
                                stcChannel.RDOControlPort = Globals.Settings.Get(strName, "Radio Control Port", "");
                                stcChannel.RDOControlBaud = Globals.Settings.Get(strName, "Radio Control Baud", "4800");
                                stcChannel.RDOModel = Globals.Settings.Get(strName, "Radio Model", "");
                                if (stcChannel.RDOModel == "Icom (other C-IV)")
                                    stcChannel.RDOModel = "Icom (other CI-V)";
                                stcChannel.RDOCenterFrequency = Globals.Settings.Get(strName, "Center Frequency", "");
                                stcChannel.AudioToneCenter = Globals.Settings.Get(strName, "Audio Tone Center", "1500");
                                stcChannel.CIVAddress = Globals.Settings.Get(strName, "CIV Address", "04");
                                stcChannel.NarrowFilter = Globals.Settings.Get(strName, "Narrow Filter", false);
                                stcChannel.NMEA = Globals.Settings.Get(strName, "Use NMEA Commands", false);
                                stcChannel.WMBandwidth = Globals.Settings.Get(strName, "WINMOR BW", 500);
                                stcChannel.WMCaptureDevice = Globals.Settings.Get(strName, "WM CaptureDevice", "");
                                stcChannel.WMPlaybackDevice = Globals.Settings.Get(strName, "WM PlaybackDevice", "");
                                stcChannel.WMPTTControl = Globals.Settings.Get(strName, "WM PTT Control", "VOX");
                                stcChannel.WMSerialCtrlSignals = Globals.Settings.Get(strName, "WM Serial Ctrl Signals", "");
                                stcChannel.WMXmitLevel = Globals.Settings.Get(strName, "WM Xmit Level", 100);
                                stcChannel.WMcwID = Globals.Settings.Get(strName, "WM CW ID", true);
                                stcChannel.WMDebugLog = Globals.Settings.Get(strName, "WM DebugLog", true);
                                break;
                            }
                    }

                    if (Entries.ContainsKey(strName) == false)
                    {
                        Entries.Add(stcChannel.ChannelName, stcChannel);
                    }
                    else
                    {
                        // If a duplicate entry is discovered in Channel Names then remove it...
                        string strChannelString = Globals.Settings.Get("Properties", "Channel Names", "");
                        strChannelString = strChannelString.Replace(stcChannel.ChannelName + "|", "");
                        strChannelString += stcChannel.ChannelName + "|";
                        Globals.Settings.Save("Properties", "Channel Names", strChannelString);
                    }
                }
            }
        } // FillChannelCollection

        public static void AddChannel(ref ChannelProperties stcChannel)
        {
            // 
            // Adds the channel properties of a new channel to the registry.
            // 
            string strName = stcChannel.ChannelName;
            if (!string.IsNullOrEmpty(strName))
            {
                if (!IsChannel(strName))
                {
                    string strChannelNames = Globals.Settings.Get("Properties", "Channel Names", "");
                    strChannelNames = strChannelNames + strName + "|";
                    Globals.Settings.Save("Properties", "Channel Names", strChannelNames);
                }

                UpdateProperties(ref stcChannel);
            }
        } // SaveChannel

        public static void RemoveChannel(string strChannelName)
        {
            // 
            // Removes the properties for the channel named in strChannelName from the registry.
            // 
            string strChannelNames = Globals.Settings.Get("Properties", "Channel Names", "");
            while (strChannelNames.IndexOf(strChannelName + "|") != -1) // Added to remove any duplicates
                strChannelNames = strChannelNames.Replace(strChannelName + "|", "");
            Globals.Settings.Save("Properties", "Channel Names", strChannelNames);
            Globals.Settings.DeleteGroup(strChannelName);
        } // RemoveChannel

        public static void UpdateChannel(ref ChannelProperties stcChannel)
        {
            // 
            // Updates the properties for the channel represented in stcChannel.
            // 
            string strName = stcChannel.ChannelName;
            if (IsChannel(strName))
                UpdateProperties(ref stcChannel);
        } // UpdateChannel

        private static void UpdateProperties(ref ChannelProperties stcChannel)
        {
            // 
            // Updates the registry as required for the properties carried in strChannel.
            // 
            string strName = stcChannel.ChannelName;
            Globals.Settings.Save(strName, "Channel Type", stcChannel.ChannelType.ToString());
            Globals.Settings.Save(strName, "Channel Name", stcChannel.ChannelName);
            Globals.Settings.Save(strName, "Priority", stcChannel.Priority);
            Globals.Settings.Save(strName, "Remote Callsign", stcChannel.RemoteCallsign);
            // objPropertiesFile.WriteInteger(strName, "Frequencies Scanned", .FrequenciesScanned)
            Globals.Settings.Save(strName, "Enabled", stcChannel.Enabled);
            if (stcChannel.ChannelType == ChannelMode.Telnet)
            {
                // Telnet only properties...
                Globals.Settings.Save(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
            }
            // objPropertiesFile.WriteString(strName, "LocalIP Address Index", .LocalIPAddressIndex.ToString)
            else if (stcChannel.ChannelType == ChannelMode.PacketAGW)
            {
                // Packet AGW only properties...
                Globals.Settings.Save(strName, "AGW Port", stcChannel.AGWPort);
                Globals.Settings.Save(strName, "AGW Timeout", stcChannel.AGWTimeout);
                Globals.Settings.Save(strName, "AGW Packet Length", stcChannel.AGWPacketLength);
                Globals.Settings.Save(strName, "AGW Max Frames", stcChannel.AGWMaxFrames);
                Globals.Settings.Save(strName, "AGW Script Timeout", stcChannel.AGWScriptTimeout);
                Globals.Settings.Save(strName, "AGW Allow Inbound", stcChannel.AGWAllowInbound);
                Globals.Settings.Save(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                SaveScript(strName, stcChannel.AGWScript);
            }
            else if (stcChannel.ChannelType == ChannelMode.PacketTNC)
            {
                // Packet TNC only properties...
                Globals.Settings.Save(strName, "TNC Type", stcChannel.TNCType);
                Globals.Settings.Save(strName, "TNC Port", stcChannel.TNCPort);
                Globals.Settings.Save(strName, "TNC Timeout", stcChannel.TNCTimeout);
                Globals.Settings.Save(strName, "TNC Serial Port", stcChannel.TNCSerialPort);
                Globals.Settings.Save(strName, "TNC Baud Rate", stcChannel.TNCBaudRate);
                Globals.Settings.Save(strName, "TNC Configuration File", stcChannel.TNCConfigurationFile);
                Globals.Settings.Save(strName, "TNC Configuration On First Use Only", stcChannel.TNCConfigureOnFirstUseOnly);
                Globals.Settings.Save(strName, "TNC Script Timeout", stcChannel.TNCScriptTimeout);
                // objPropertiesFile.WriteBoolean(strName, "TNC Allow Inbound", .TNCAllowInbound)
                Globals.Settings.Save(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                Globals.Settings.Save(strName, "Radio Control", stcChannel.RDOControl);
                Globals.Settings.Save(strName, "Radio Control Port", stcChannel.RDOControlPort);
                Globals.Settings.Save(strName, "Radio Control Baud", stcChannel.RDOControlBaud);
                Globals.Settings.Save(strName, "Radio Model", stcChannel.RDOModel);
                Globals.Settings.Save(strName, "Center Frequency", stcChannel.RDOCenterFrequency);
                Globals.Settings.Save(strName, "Audio Tone Center", stcChannel.AudioToneCenter);
                Globals.Settings.Save(strName, "CIV Address", stcChannel.CIVAddress);
                Globals.Settings.Save(strName, "PTC II TTL Level", stcChannel.TTLLevel);
                Globals.Settings.Save(strName, "Use NMEA Commands", stcChannel.NMEA);
                Globals.Settings.Save(strName, "TNC On-Air Baud", stcChannel.TNCOnAirBaud);
                SaveScript(strName, stcChannel.TNCScript);
            }
            else if (stcChannel.ChannelType == ChannelMode.PactorTNC)
            {
                // Pactor TNC only properties...
                Globals.Settings.Save(strName, "TNC Type", stcChannel.TNCType);
                Globals.Settings.Save(strName, "TNC Port", stcChannel.TNCPort);
                Globals.Settings.Save(strName, "TNC Timeout", stcChannel.TNCTimeout);
                Globals.Settings.Save(strName, "TNC Serial Port", stcChannel.TNCSerialPort);
                Globals.Settings.Save(strName, "TNC Baud Rate", stcChannel.TNCBaudRate);
                Globals.Settings.Save(strName, "TNC Configuration File", stcChannel.TNCConfigurationFile);
                Globals.Settings.Save(strName, "TNC Configuration On First Use Only", stcChannel.TNCConfigureOnFirstUseOnly);
                Globals.Settings.Save(strName, "TNC Busy Hold", stcChannel.TNCBusyHold);
                Globals.Settings.Save(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                Globals.Settings.Save(strName, "Radio Control", stcChannel.RDOControl);
                Globals.Settings.Save(strName, "Radio Control Port", stcChannel.RDOControlPort);
                Globals.Settings.Save(strName, "Radio Control Baud", stcChannel.RDOControlBaud);
                Globals.Settings.Save(strName, "Radio Model", stcChannel.RDOModel);
                Globals.Settings.Save(strName, "Center Frequency", stcChannel.RDOCenterFrequency);
                Globals.Settings.Save(strName, "Audio Tone Center", stcChannel.AudioToneCenter);
                Globals.Settings.Save(strName, "CIV Address", stcChannel.CIVAddress);
                Globals.Settings.Save(strName, "Narrow Filter", stcChannel.NarrowFilter);
                Globals.Settings.Save(strName, "TNC PSK Level", stcChannel.TNCPSKLevel);
                Globals.Settings.Save(strName, "TNC FSK Level", stcChannel.TNCFSKLevel);
                Globals.Settings.Save(strName, "Pactor 1 ID", stcChannel.PactorId);
                Globals.Settings.Save(strName, "PTC II TTL Level", stcChannel.TTLLevel);
                Globals.Settings.Save(strName, "Use NMEA Commands", stcChannel.NMEA);
                Globals.Settings.Save(strName, "Use Long Path", stcChannel.PactorUseLongPath);
            }
            else if (stcChannel.ChannelType == ChannelMode.Winmor)
            {
                Globals.Settings.Save(strName, "TNC Busy Hold", stcChannel.TNCBusyHold);
                Globals.Settings.Save(strName, "Enable Autoforward", stcChannel.EnableAutoforward);
                Globals.Settings.Save(strName, "Radio Control", stcChannel.RDOControl);
                Globals.Settings.Save(strName, "Radio Control Port", stcChannel.RDOControlPort);
                Globals.Settings.Save(strName, "Radio Control Baud", stcChannel.RDOControlBaud);
                Globals.Settings.Save(strName, "Radio Model", stcChannel.RDOModel);
                Globals.Settings.Save(strName, "Center Frequency", stcChannel.RDOCenterFrequency);
                Globals.Settings.Save(strName, "Audio Tone Center", stcChannel.AudioToneCenter);
                Globals.Settings.Save(strName, "CIV Address", stcChannel.CIVAddress);
                Globals.Settings.Save(strName, "Narrow Filter", stcChannel.NarrowFilter);
                Globals.Settings.Save(strName, "Use NMEA Commands", stcChannel.NMEA);
                Globals.Settings.Save(strName, "WINMOR BW", stcChannel.WMBandwidth);
                Globals.Settings.Save(strName, "WM CaptureDevice", stcChannel.WMCaptureDevice);
                Globals.Settings.Save(strName, "WM PlaybackDevice", stcChannel.WMPlaybackDevice);
                Globals.Settings.Save(strName, "WM PTT Control", stcChannel.WMPTTControl);
                Globals.Settings.Save(strName, "WM Serial Ctrl Signals", stcChannel.WMSerialCtrlSignals);
                Globals.Settings.Save(strName, "WM Xmit Level", stcChannel.WMXmitLevel);
                Globals.Settings.Save(strName, "WM CW ID", stcChannel.WMcwID);
                Globals.Settings.Save(strName, "WM DebugLog", stcChannel.WMDebugLog);
            }
        } // UpdateProperties

        private static void SaveScript(string strChannel, string strScript)
        {
            // 
            // Saves a script for the channel named in strChannel.
            // 
            var channelDatabase = new Channel(DatabaseFactory.Get());
            try
            {
                channelDatabase.BeginUpdateSession();
                channelDatabase.WriteScript(strChannel, strScript);
                channelDatabase.CommitUpdateSession();
            }
            catch (Exception e)
            {
                channelDatabase.RollbackUpdateSession();
                _log.Error("[Channels.SaveScript] " + e.Message);
            }
        } // SaveScript

        private static string GetScript(string strChannel)
        {
            // 
            // Gets a script for the channel named in strChannel.
            // 
            var channelDatabase = new Channel(DatabaseFactory.Get());
            try
            {
                return channelDatabase.GetScript(strChannel);
            }
            catch (Exception e)
            {
                _log.Error("[Channels.GetScript] " + e.Message);
            }

            return null;
        } // GetScript

        public static string GetChannelRecords(bool VHFchannels, string strServiceCodes)
        {
            // 
            // Try to get a list of either HF or VHF channels.
            // 
            var lstGateways = Globals.GetChannelsList(VHFchannels);
            // grdChannels.Focus()
            if (lstGateways == null)
            {
                return "Unable to download channel information from Winlink server.";
            }

            var channelDatabase = new Channel(DatabaseFactory.Get());
            try
            {
                channelDatabase.BeginUpdateSession();
                channelDatabase.ClearChannelList(VHFchannels);

                foreach (var station in lstGateways)
                {
                    foreach (var channel in station.GatewayChannels)
                    {
                        channelDatabase.AddChannelRecord(
                            VHFchannels, station.Callsign, channel.Frequency,
                            channel.Gridsquare, channel.Mode, channel.OperatingHours.Replace(" ", ""), channel.ServiceCode);
                    }
                }

                channelDatabase.CommitUpdateSession();
            } 
            catch (Exception e)
            {
                channelDatabase.RollbackUpdateSession();
                return "Could not update channel database: " + e.ToString();
            }

            return "";
        }

        public static bool HasChannelList(bool isPacket)
        {
            var channelDatabase = new Channel(DatabaseFactory.Get());
            return channelDatabase.ContainsChannelList(isPacket);
        }

        public static string[] ParseChannelList(bool isPacket)
        {
            // 
            // Function to parse the channel freq list (used for Public, EMComm and MARS
            // Returns an empty string array if error or file not found.
            // 
            var channelDatabase = new Channel(DatabaseFactory.Get());
            var result = new List<string>();
            foreach (var entry in channelDatabase.GetChannelList(isPacket))
            {
                result.Add(entry.Key + ":" + entry.Value);
            }

            return result.ToArray();
        } // ParseChannelList
    } // Channels
}