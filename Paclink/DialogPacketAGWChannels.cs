using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink
{
    public partial class DialogPacketAGWChannels
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private DialogAgwEngineViewModel _dialogAgwEngine = new DialogAgwEngineViewModel();

        public DialogPacketAGWChannels()
        {
            InitializeComponent();
            _cmbAGWPort.Name = "cmbAGWPort";
            _Label11.Name = "Label11";
            _Label10.Name = "Label10";
            _Label9.Name = "Label9";
            _Label5.Name = "Label5";
            _Label4.Name = "Label4";
            _txtRemoteCallsign.Name = "txtRemoteCallsign";
            _chkEnabled.Name = "chkEnabled";
            _cmbChannelName.Name = "cmbChannelName";
            _Label3.Name = "Label3";
            _Label2.Name = "Label2";
            _Label1.Name = "Label1";
            _btnClose.Name = "btnClose";
            _btnUpdate.Name = "btnUpdate";
            _btnRemove.Name = "btnRemove";
            _btnAdd.Name = "btnAdd";
            _Label8.Name = "Label8";
            _Label7.Name = "Label7";
            _txtScript.Name = "txtScript";
            _Label6.Name = "Label6";
            _nudMaxOutstanding.Name = "nudMaxOutstanding";
            _nudActivityTimeout.Name = "nudActivityTimeout";
            _nudPriority.Name = "nudPriority";
            _nudScriptTimeout.Name = "nudScriptTimeout";
            _btnRetryRemote.Name = "btnRetryRemote";
            _nudPacketLength.Name = "nudPacketLength";
            _btnHelp.Name = "btnHelp";
        }

        private ChannelProperties stcSelectedChannel;
        private TcpClient _objTCPPort;

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

        private byte[] bytTCPData;

        public delegate void AddAGWPortInfoCallback(string AddItem, string Text, bool EnableRetry);

        private void PacketAGWChannels_Load(object sender, EventArgs e)
        {
            UpdateAGWPortInfo();
            ClearEntries();
            FillChannelList();
            cmbChannelName.DroppedDown = true;
        } // PacketAGWChannels_Load

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            foreach (ChannelProperties stcChannel in Channels.Entries.Values)
            {
                if (stcChannel.ChannelType == ChannelMode.PacketAGW)
                {
                    cmbChannelName.Items.Add(stcChannel.ChannelName);
                }
            }
        } // FillChannelList

        private void ClearEntries()
        {
            cmbChannelName.Text = "";
            nudPriority.Value = 3;
            txtRemoteCallsign.Text = "";
            nudActivityTimeout.Value = 4;
            nudPacketLength.Value = 128;
            nudMaxOutstanding.Value = 2;
            nudScriptTimeout.Value = 60;
            txtScript.Clear();
            chkEnabled.Checked = true;
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
        } // ClearEntries

        private void SetEntries()
        {
            stcSelectedChannel = (ChannelProperties)Channels.Entries[cmbChannelName.Text];
            {
                var withBlock = stcSelectedChannel;
                nudPriority.Value = withBlock.Priority;
                cmbAGWPort.Text = withBlock.AGWPort;
                txtRemoteCallsign.Text = withBlock.RemoteCallsign;
                nudActivityTimeout.Value = withBlock.AGWTimeout;
                nudPacketLength.Value = withBlock.AGWPacketLength;
                nudMaxOutstanding.Value = Math.Max(1, withBlock.AGWMaxFrames); // temp fix to fix channels with old values of 0
                nudScriptTimeout.Value = withBlock.AGWScriptTimeout;
                txtScript.Text = withBlock.AGWScript;
                chkEnabled.Checked = withBlock.Enabled;
            }

            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
        } // SetEntries

        private void UpdateAGWPortInfo()
        {
            try
            {
                string strAGWIniPath = _dialogAgwEngine.AgwPath + "AGWPE.INI";
                string[] strTokens;
                cmbAGWPort.Items.Clear();
                if (_dialogAgwEngine.AgwLocation == 0)
                {
                    cmbAGWPort.Text = "<AGW engine not configured or not enabled>";
                    return;
                }
                else if (_dialogAgwEngine.AgwLocation == 1) // If local read the INI file to get port info
                {
                    if (!File.Exists(strAGWIniPath))
                    {
                        cmbAGWPort.Text = "<AGW engine not installed or not configured>";
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
                        string strPortIniFile = _dialogAgwEngine.AgwPath + "PORT" + intIndex.ToString() + ".ini";
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
                            cmbAGWPort.Items.Add((intIndex + 1).ToString() + ": " + strType + " Frequency:" + strFrequency + " Port:" + strPort);
                        }
                    }
                }
                else
                {
                    // This requires a remote login and port properties retrieval to set cmbAGWPort
                    try
                    {
                        cmbAGWPort.Items.Clear();
                        cmbAGWPort.Text = "Requesting port info from remote AGW Engine @ host " + _dialogAgwEngine.AgwHost;
                        if (objTCPPort != null) objTCPPort.Close();
                        tmrTimer10sec.Enabled = true;
                        objTCPPort = new TcpClient();
                        objTCPPort.ConnectAsync(_dialogAgwEngine.AgwHost, _dialogAgwEngine.AgwTcpPort).ContinueWith(t =>
                        {
                            OnConnected(this);
                        }).Wait(0);
                    }
                    catch
                    {
                        cmbAGWPort.Text = "Could not connect to remote AGW Engine @ host " + _dialogAgwEngine.AgwHost;
                        tmrTimer10sec.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("[DialogPacketAGWChannels.UpdateAGWPortInfo]: " + ex.Message);
            }
        } // UpdateAGWPortInfo

        private void cmbChannelName_Leave(object sender, EventArgs e)
        {
        } // cmbChannelName_Leave

        private void cmbChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChannelName.Text) & cmbChannelName.Text != "<Enter a new channel>")
            {
                if (Channels.Entries.ContainsKey(cmbChannelName.Text))
                {
                    SetEntries();
                }
                else
                {
                    ClearEntries();
                }
            }
        } // cmbChannelName_SelectedIndexChanged

        private void cmbChannelName_TextChanged(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text))
            {
                btnAdd.Enabled = false;
                btnRemove.Enabled = true;
                btnUpdate.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = true;
                btnRemove.Enabled = false;
                btnUpdate.Enabled = false;
            }
        } // cmbChannelName_TextChanged

        private void txtPacketLength_TextChanged(object sender, EventArgs e)
        {
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
                asc.GetBytes(_dialogAgwEngine.AgwUserId, 0, _dialogAgwEngine.AgwUserId.Length, bytTemp2, 36);
                asc.GetBytes(_dialogAgwEngine.AgwPassword, 0, _dialogAgwEngine.AgwPassword.Length, bytTemp2, 36 + 255);
                objTCPPort.GetStream().Write(bytTemp2, 0, bytTemp2.Length);;
            }
            catch (Exception ex)
            {
                AddAGWPortInfo("", "Error in Remote AGW Engine Login @ host " + _dialogAgwEngine.AgwHost, true);
                Log.Error("[PacketAGWChannels, LoginAGWRemote] " + ex.Message);
                tmrTimer10sec.Enabled = false;
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
                objTCPPort.GetStream().Write(bytTemp, 0, bytTemp.Length);;
            }
            catch (Exception ex)
            {
                Log.Error("[AGWEngine, RequestAGWPortInfo] " + ex.Message);
                tmrTimer10sec.Enabled = false;
                AddAGWPortInfo("", "Error requesting Port Info from Remote AGW Engine @ host " + _dialogAgwEngine.AgwHost, true);
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

                    tmrTimer10sec.Enabled = false;
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
                        AddAGWPortInfo(strTemp, "", false);
                        if (i == 1)
                            strPort1Info = strTemp;
                    }

                    AddAGWPortInfo("", strPort1Info, true); // update to text to port 1 and enable retry button.
                }
                catch (Exception ex)
                {
                    Log.Error("[AGWEngine, tcpOnDataIn] " + ex.Message);
                    AddAGWPortInfo("", "Port Info request failure with host " + _dialogAgwEngine.AgwHost, true);
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
            if (!string.IsNullOrEmpty(_dialogAgwEngine.AgwUserId))
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

        private void tmrTimer10sec_Tick(object sender, EventArgs e)
        {
            tmrTimer10sec.Enabled = false;
            Log.Error("[PacketAGWChannels]  10 sec timeout on remote AGWPE port info Request to " + _dialogAgwEngine.AgwHost);
            AddAGWPortInfo("", "Timeout on port info request to remote computer AGW Engine @ host " + _dialogAgwEngine.AgwHost, true);
            try
            {
                objTCPPort.Close();
                objTCPPort = null;
            }
            catch
            {
            }
        } // tmrTimer10sec_Tick

        private void AddAGWPortInfo(string AddItem, string Text, bool EnableRetry)
        {
            if (cmbAGWPort.InvokeRequired)
            {
                var objCMBDelegate = new AddAGWPortInfoCallback(AddAGWPortInfo);
                Invoke(objCMBDelegate, new object[] { AddItem, Text, EnableRetry });
                return;
            }

            if (!string.IsNullOrEmpty(AddItem))
                cmbAGWPort.Items.Add(AddItem);
            if (!string.IsNullOrEmpty(Text))
                cmbAGWPort.Text = Text;
            btnRetryRemote.Enabled = EnableRetry;
        } // AddAGWPortInfo

        private void btnRetryRemote_Click(object sender, EventArgs e)
        {
            btnRetryRemote.Enabled = false;
            UpdateAGWPortInfo();
        } // btnRetryRemote_Click

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (Channels.IsAccount(cmbChannelName.Text))
            {
                MessageBox.Show(
                    cmbChannelName.Text + " is in use as an account name...",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (Channels.IsChannel(cmbChannelName.Text))
            {
                MessageBox.Show(
                    "The channel name " + cmbChannelName.Text + " is already in use...",
                    "Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                cmbChannelName.Focus();
            }
            else
            {
                var stcNewChannel = new ChannelProperties();

                stcNewChannel.ChannelType = ChannelMode.PacketAGW;
                stcNewChannel.ChannelName = cmbChannelName.Text;
                stcNewChannel.Priority = Convert.ToInt32(nudPriority.Value);
                stcNewChannel.RemoteCallsign = txtRemoteCallsign.Text;
                stcNewChannel.AGWTimeout = Convert.ToInt32(nudActivityTimeout.Value);
                stcNewChannel.AGWPacketLength = Convert.ToInt32(nudPacketLength.Value);
                stcNewChannel.AGWPort = cmbAGWPort.Text;
                stcNewChannel.AGWScript = txtScript.Text;
                stcNewChannel.AGWScriptTimeout = Convert.ToInt32(nudScriptTimeout.Value);
                stcNewChannel.Enabled = chkEnabled.Checked;
                stcNewChannel.EnableAutoforward = true; // Packet Channels always enabled

                Channels.AddChannel(ref stcNewChannel);
                Channels.FillChannelCollection();
                FillChannelList();
                btnAdd.Enabled = false;
                btnRemove.Enabled = true;
                btnUpdate.Enabled = true;
                // Me.Close()
            }
        } // btnAdd_Click

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show(
                    "The packet channel " + cmbChannelName.Text + " is not found...",
                    "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (MessageBox.Show(
                "Confirm removal of packet channel " + cmbChannelName.Text + "...", 
                "Remove Channel",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Channels.RemoveChannel(cmbChannelName.Text);
                Channels.FillChannelCollection();
                FillChannelList();
                // Me.Close()
            }

            ClearEntries();
        } // btnRemove_Click

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show(
                    "The AGW packet channel " + cmbChannelName.Text + " is not found...",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            else if (string.IsNullOrEmpty(cmbAGWPort.Text.Trim()))
            {
                MessageBox.Show(
                    "AGW Port not selected!...",
                    "Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                return; // 
            }
            else if (string.IsNullOrEmpty(txtRemoteCallsign.Text.Trim()))
            {
                MessageBox.Show(
                    "No remote callsign entered!...",
                    "Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                return;
            }
            else
            {
                var stcUpdateChannel = new ChannelProperties();

                stcUpdateChannel.ChannelType = ChannelMode.PacketAGW;
                stcUpdateChannel.ChannelName = cmbChannelName.Text;
                stcUpdateChannel.Priority = Convert.ToInt32(nudPriority.Value);
                stcUpdateChannel.RemoteCallsign = txtRemoteCallsign.Text;
                stcUpdateChannel.AGWTimeout = Convert.ToInt32(nudActivityTimeout.Value);
                stcUpdateChannel.AGWPacketLength = Convert.ToInt32(nudPacketLength.Value);
                stcUpdateChannel.AGWPort = cmbAGWPort.Text;
                stcUpdateChannel.AGWScript = txtScript.Text;
                stcUpdateChannel.AGWScriptTimeout = Convert.ToInt32(nudScriptTimeout.Value);
                stcUpdateChannel.AGWMaxFrames = Convert.ToInt32(nudMaxOutstanding.Value);
                stcUpdateChannel.Enabled = chkEnabled.Checked;
                stcUpdateChannel.EnableAutoforward = true; // Packet Channels always enabled

                Channels.UpdateChannel(ref stcUpdateChannel);
                Channels.FillChannelCollection();
                FillChannelList();
                // Me.Close()
            }
        } // btnUpdate_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        } // btnClose_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs140.htm");
        } // btnHelp_Click
    }
} // PacketAGWChannels