using NLog;
using Paclink.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paclink
{
    public class DialogPacketAGWChannelsViewModel : IPacketAGWChannelBacking
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private TcpClient _objTCPPort;
        private byte[] bytTCPData;
        private IPacketAgwChannelWindow _window;
        private Timer _tenSecTimer;

        public IAGWEngineBacking AgwEngineDialog
        {
            get; private set;
        }

        public IEnumerable<string> ChannelNames
        {
            get
            {
                foreach (ChannelProperties stcChannel in Channels.Entries.Values)
                {
                    if (stcChannel.ChannelType == ChannelMode.PacketAGW)
                    {
                        yield return stcChannel.ChannelName;
                    }
                }
            }
        }

        public DialogPacketAGWChannelsViewModel(IAGWEngineBacking agwEngineDialog)
        {
            AgwEngineDialog = agwEngineDialog; 
        }

        public string SiteRootDirectory
        {
            get
            {
                return Globals.SiteRootDirectory;
            }
        }

        public bool IsChannelNameAChannel(string name)
        {
            return Channels.IsChannel(name);
        }
        public bool IsChannelNameAnAccount(string name)
        {
            return Channels.IsAccount(name);
        }

        public void AddChannel(
            string name, int priority, string remoteCallsign, int timeout,
            int agwPacketLength, string agwPort, string agwScript, int agwScriptTimeout,
            bool enabled)
        {
            var stcNewChannel = new ChannelProperties();

            stcNewChannel.ChannelType = ChannelMode.PacketAGW;
            stcNewChannel.ChannelName = name;
            stcNewChannel.Priority = priority;
            stcNewChannel.RemoteCallsign = remoteCallsign;
            stcNewChannel.AGWTimeout = timeout;
            stcNewChannel.AGWPacketLength = agwPacketLength;
            stcNewChannel.AGWPort = agwPort;
            stcNewChannel.AGWScript = agwScript;
            stcNewChannel.AGWScriptTimeout = agwScriptTimeout;
            stcNewChannel.Enabled = enabled;
            stcNewChannel.EnableAutoforward = true; // Packet Channels always enabled

            Channels.AddChannel(ref stcNewChannel);
            Channels.FillChannelCollection();
        }

        public void UpdateChannel(
            string name, int priority, string remoteCallsign, int timeout,
            int agwPacketLength, string agwPort, string agwScript, int agwScriptTimeout,
            bool enabled)
        {
            var stcNewChannel = new ChannelProperties();

            stcNewChannel.ChannelType = ChannelMode.PacketAGW;
            stcNewChannel.ChannelName = name;
            stcNewChannel.Priority = priority;
            stcNewChannel.RemoteCallsign = remoteCallsign;
            stcNewChannel.AGWTimeout = timeout;
            stcNewChannel.AGWPacketLength = agwPacketLength;
            stcNewChannel.AGWPort = agwPort;
            stcNewChannel.AGWScript = agwScript;
            stcNewChannel.AGWScriptTimeout = agwScriptTimeout;
            stcNewChannel.Enabled = enabled;
            stcNewChannel.EnableAutoforward = true; // Packet Channels always enabled

            Channels.UpdateChannel(ref stcNewChannel);
            Channels.FillChannelCollection();
        }

        public void RemoveChannel(string name)
        {
            Channels.RemoveChannel(name);
            Channels.FillChannelCollection();
        }

        public void GetChannelInfo(
            string name, out int priority, out string remoteCallsign, out int timeout,
            out int agwPacketLength, out int maxOutstanding, out string agwPort, 
            out string agwScript, out int agwScriptTimeout,
            out bool enabled)
        {
            var entry = Channels.Entries[name];

            priority = entry.Priority;
            remoteCallsign = entry.RemoteCallsign;
            timeout = entry.AGWTimeout;
            agwPacketLength = entry.AGWPacketLength;
            agwPort = entry.AGWPort;
            agwScript = entry.AGWScript;
            agwScriptTimeout = entry.AGWScriptTimeout;
            enabled = entry.Enabled;

            maxOutstanding = Math.Max(1, entry.AGWMaxFrames); // temp fix to fix channels with old values of 0
        }

        public bool ContainsChannel(string name)
        {
            return Channels.Entries.ContainsKey(name);
        }

        public void UpdateAGWPortInfo(IPacketAgwChannelWindow window)
        {
            _window = window;

            try
            {
                string strAGWIniPath = AgwEngineDialog.AgwPath + "AGWPE.INI";
                string[] strTokens;

                _window.ClearItems();

                if (AgwEngineDialog.AgwLocation == 0)
                {
                    _window.SetAgwPort("<AGW engine not configured or not enabled>");
                    return;
                }
                else if (AgwEngineDialog.AgwLocation == 1) // If local read the INI file to get port info
                {
                    if (!File.Exists(strAGWIniPath))
                    {
                        _window.SetAgwPort("<AGW engine not installed or not configured>");
                        return;
                    }

                    string strAGWData = File.ReadAllText(strAGWIniPath);
                    int intAGWPorts = 0;
                    var blnMainFlag = default(bool);
                    var srdAGWData = new StringReader(strAGWData);
                    do
                    {
                        string strLine = srdAGWData.ReadLine();
                        if (strLine == null) break;
                        if (blnMainFlag)
                        {
                            strTokens = strLine.Split('=');
                            if (strTokens[0].IndexOf("PORTS") != 1)
                            {
                                intAGWPorts = Convert.ToInt32(strTokens[1]);
                                break;
                            }
                        }

                        if (strLine.IndexOf("[MAIN]") != -1)
                            blnMainFlag = true;
                    }
                    while (true);
                    for (int intIndex = 0, loopTo = intAGWPorts - 1; intIndex <= loopTo; intIndex++)
                    {
                        var strType = default(string);
                        var strFrequency = default(string);
                        var strPort = default(string);
                        string strPortIniFile = AgwEngineDialog.AgwPath + "PORT" + intIndex.ToString() + ".ini";
                        if (File.Exists(strPortIniFile))
                        {
                            string strPortData = File.ReadAllText(strPortIniFile);
                            var srdPortData = new StringReader(strPortData);
                            do
                            {
                                string strLine = srdPortData.ReadLine();
                                if (string.IsNullOrEmpty(strLine))
                                    break;
                                strTokens = strLine.Split('=');
                                var switchExpr = strTokens[0];
                                switch (switchExpr)
                                {
                                    case "FREQUENCY":
                                        {
                                            strFrequency = strTokens[1];
                                            break;
                                        }

                                    case "PORT":
                                        {
                                            strPort = strTokens[1].Replace(":", "");
                                            break;
                                        }

                                    case "TYPE":
                                        {
                                            strType = strTokens[1];
                                            break;
                                        }
                                }
                            }
                            while (true);

                            window.AddAgwPortItem((intIndex + 1).ToString() + ": " + strType + " Frequency:" + strFrequency + " Port:" + strPort);
                        }
                    }
                }
                else
                {
                    // This requires a remote login and port properties retrieval to set cmbAGWPort
                    try
                    {
                        window.ClearItems();
                        window.SetAgwPort("Requesting port info from remote AGW Engine @ host " + AgwEngineDialog.AgwHost);
                        if (objTCPPort != null) objTCPPort.Close();

                        _tenSecTimer = new Timer((obj) => OnAgwTimeout(), null, 10000, Timeout.Infinite);

                        objTCPPort = new TcpClient();
                        objTCPPort.ConnectAsync(AgwEngineDialog.AgwHost, AgwEngineDialog.AgwTcpPort).ContinueWith(t =>
                        {
                            OnConnected(this);
                        }).Wait(0);
                    }
                    catch
                    {
                        window.SetAgwPort("Could not connect to remote AGW Engine @ host " + AgwEngineDialog.AgwHost);
                        _tenSecTimer.Dispose();
                        _tenSecTimer = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[DialogPacketAGWChannels.UpdateAGWPortInfo]: " + ex.Message);
            }
        } // UpdateAGWPortInfo

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

        private TcpClient objTCPPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objTCPPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                _objTCPPort = value;
            }
        }

        private void OnAgwTimeout()
        {
            _tenSecTimer.Dispose();
            _tenSecTimer = null;

            Log.Error("[PacketAGWChannels]  10 sec timeout on remote AGWPE port info Request to " + AgwEngineDialog.AgwHost);
            _window.AddAGWPortInfo("", "Timeout on port info request to remote computer AGW Engine @ host " + AgwEngineDialog.AgwHost, true);
            try
            {
                objTCPPort.Close();
                objTCPPort = null;
            }
            catch
            {
            }
        }

        private void LoginAGWRemote()
        {
            // Private Sub to Login to remote AGWPE with UserID and password ("P" Frame)...

            try
            {
                var bytTemp2 = new byte[546];
                var asc = new ASCIIEncoding(); // had to declare this to eliminate an ocasional error
                asc.GetBytes("P", 0, 1, bytTemp2, 4);
                Array.Copy(Globals.ComputeLengthB(255 + 255), 0, bytTemp2, 28, 4);
                asc.GetBytes(AgwEngineDialog.AgwUserId, 0, AgwEngineDialog.AgwUserId.Length, bytTemp2, 36);
                asc.GetBytes(AgwEngineDialog.AgwPassword, 0, AgwEngineDialog.AgwPassword.Length, bytTemp2, 36 + 255);
                objTCPPort.GetStream().Write(bytTemp2, 0, bytTemp2.Length); ;
            }
            catch (Exception ex)
            {
                _window.AddAGWPortInfo("", "Error in Remote AGW Engine Login @ host " + AgwEngineDialog.AgwHost, true);
                Log.Error("[PacketAGWChannels, LoginAGWRemote] " + ex.Message);

                _tenSecTimer.Dispose();
                _tenSecTimer = null;
            }
        }  // LoginAGWRemote 

        private void GetAGWPortInfo()
        {
            // Private Sub to Request AGW Port Information ("G" Frame)...

            var bytTemp = new byte[36];
            try
            {
                if (objTCPPort == null || !objTCPPort.Connected)
                    return;
                bytTemp[4] = (byte)Globals.Asc('G');
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length); ;
            }
            catch (Exception ex)
            {
                Log.Error("[AGWEngine, RequestAGWPortInfo] " + ex.Message);

                _tenSecTimer.Dispose();
                _tenSecTimer = null;

                _window.AddAGWPortInfo("", "Error requesting Port Info from Remote AGW Engine @ host " + AgwEngineDialog.AgwHost, true);
            }
        } // GetAGWPortInfo

        private void OnDataIn(byte[] buffer, int length)
        {
            while (true)
            {
                int intDataLength;
                try
                {
                    if (length <= 0)
                    {
                        objTCPPort.Close();
                        objTCPPort = null;
                        return;
                    }
                    byte[] result = new byte[length];
                    Array.Copy(buffer, result, length);
                    Globals.ConcatanateByteArrays(ref bytTCPData, result); // Add data to buffer array
                    if (bytTCPData.Length < 36)
                        break; // not a complete frame header
                    intDataLength = Globals.ComputeLengthL(bytTCPData, 28); // get and decode the data length field from the header
                    if (bytTCPData.Length < 36 + intDataLength)
                        break; // not A complete "G" frame...
                    if (Convert.ToString((char)bytTCPData[4]) != "G")
                    {
                        bytTCPData = new byte[0];
                        break;
                    }

                    _tenSecTimer.Dispose();
                    _tenSecTimer = null;

                    objTCPPort.Close();
                    objTCPPort = null;
                    string strPort1Info = "";
                    var bytTemp1 = new byte[intDataLength];
                    Array.Copy(bytTCPData, 36, bytTemp1, 0, bytTemp1.Length);
                    string strPortInfo = Globals.GetString(bytTemp1);
                    int intPtr1 = strPortInfo.IndexOf(";");
                    int intPortCnt = Convert.ToInt32(strPortInfo.Substring(0, intPtr1));
                    for (int i = 1, loopTo = intPortCnt; i <= loopTo; i++)
                    {
                        int intPtr2 = strPortInfo.IndexOf("with ", intPtr1);
                        if (intPtr2 == -1)
                            break;
                        intPtr1 = strPortInfo.IndexOf(";", intPtr2 + 5);
                        if (intPtr1 == -1)
                            break;
                        string strTemp = i.ToString() + ": ";
                        strTemp += strPortInfo.Substring(intPtr2 + 5, intPtr1 - (intPtr2 + 5)).Trim();
                        _window.AddAGWPortInfo(strTemp, "", false);
                        if (i == 1)
                            strPort1Info = strTemp;
                    }

                    _window.AddAGWPortInfo("", strPort1Info, true); // update to text to port 1 and enable retry button.
                }
                catch (Exception ex)
                {
                    Log.Error("[AGWEngine, tcpOnDataIn] " + ex.Message);
                    _window.AddAGWPortInfo("", "Port Info request failure with host " + AgwEngineDialog.AgwHost, true);
                }
                break;
            }

            Task<int> t = null;
            try
            {
                t = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                t.ContinueWith(k =>
                {
                    if (k.Exception == null)
                    {
                        OnDataIn(buffer, k.Result);
                    }
                });
                t.Wait(0);
            }
            catch (Exception e)
            {
                // empty
            }

        } // objTCPPort_OnDataIn

        private void OnConnected(object sender)
        {
            if (!string.IsNullOrEmpty(AgwEngineDialog.AgwUserId))
            {
                LoginAGWRemote();  // do a secure AGWPE login 
            }

            bytTCPData = new byte[0];
            GetAGWPortInfo();    // Request port info from AGWPE

            byte[] buffer = new byte[1024];

            Task<int> task = null;
            try
            {
                task = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                task.ContinueWith(t =>
                {
                    if (t.Exception == null)
                    {
                        OnDataIn(buffer, t.Result);
                    }
                });
                task.Wait(0);
            }
            catch (Exception e)
            {
                // empty
            }
        } // objTCPPort_OnReadyToSend
    }
}
