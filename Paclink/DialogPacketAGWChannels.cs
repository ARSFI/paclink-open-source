using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public partial class DialogPacketAGWChannels
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

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

        private TChannelProperties stcSelectedChannel;
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
            foreach (TChannelProperties stcChannel in Channels.Entries.Values)
            {
                if (stcChannel.ChannelType == EChannelModes.PacketAGW)
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
            stcSelectedChannel = (TChannelProperties)Channels.Entries[cmbChannelName.Text];
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
                string strAGWIniPath = DialogAGWEngine.AGWPath + "AGWPE.INI";
                string[] strTokens;
                cmbAGWPort.Items.Clear();
                if (DialogAGWEngine.AGWLocation == 0)
                {
                    cmbAGWPort.Text = "<AGW engine not configured or not enabled>";
                    return;
                }
                else if (DialogAGWEngine.AGWLocation == 1) // If local read the INI file to get port info
                {
                    if (!File.Exists(strAGWIniPath))
                    {
                        cmbAGWPort.Text = "<AGW engine not installed or not configured>";
                        return;
                    }

                    string strAGWData = File.ReadAllText(strAGWIniPath);
                    int intAGWPorts;
                    var blnMainFlag = default(bool);
                    var srdAGWData = new StringReader(strAGWData);
                    do
                    {
                        string strLine = srdAGWData.ReadLine();
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
                        string strPortIniFile = DialogAGWEngine.AGWPath + "PORT" + intIndex.ToString() + ".ini";
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
                        cmbAGWPort.Text = "Requesting port info from remote AGW Engine @ host " + DialogAGWEngine.AGWHost;
                        if (objTCPPort != null) objTCPPort.Close();
                        tmrTimer10sec.Enabled = true;
                        objTCPPort = new TcpClient();
                        objTCPPort.ConnectAsync(DialogAGWEngine.AGWHost, DialogAGWEngine.AGWTCPPort).ContinueWith(t =>
                        {
                            OnConnected(this);
                        }).Wait(0);
                    }
                    catch
                    {
                        cmbAGWPort.Text = "Could not connect to remote AGW Engine @ host " + DialogAGWEngine.AGWHost;
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
                asc.GetBytes(DialogAGWEngine.AGWUserId, 0, DialogAGWEngine.AGWUserId.Length, bytTemp2, 36);
                asc.GetBytes(DialogAGWEngine.AGWPassword, 0, DialogAGWEngine.AGWPassword.Length, bytTemp2, 36 + 255);
                objTCPPort.GetStream().Write(bytTemp2, 0, bytTemp2.Length);;
            }
            catch (Exception ex)
            {
                AddAGWPortInfo("", "Error in Remote AGW Engine Login @ host " + DialogAGWEngine.AGWHost, true);
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
                AddAGWPortInfo("", "Error requesting Port Info from Remote AGW Engine @ host " + DialogAGWEngine.AGWHost, true);
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
                    AddAGWPortInfo("", "Port Info request failure with host " + DialogAGWEngine.AGWHost, true);
                }
                break;
            }

            Task<int> t = null;
            try
            {
                t = objTCPPort.GetStream().ReadAsync(buffer, 0, 1024);
                t.ContinueWith(k =>
                {
                    OnDataIn(buffer, k.Result);
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
            if (!string.IsNullOrEmpty(DialogAGWEngine.AGWUserId))
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
                    OnDataIn(buffer, t.Result);
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
            Log.Error("[PacketAGWChannels]  10 sec timeout on remote AGWPE port info Request to " + DialogAGWEngine.AGWHost);
            AddAGWPortInfo("", "Timeout on port info request to remote computer AGW Engine @ host " + DialogAGWEngine.AGWHost, true);
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
                var stcNewChannel = default(TChannelProperties);
                {
                    var withBlock = stcNewChannel;
                    withBlock.ChannelType = EChannelModes.PacketAGW;
                    withBlock.ChannelName = cmbChannelName.Text;
                    withBlock.Priority = Convert.ToInt32(nudPriority.Value);
                    withBlock.RemoteCallsign = txtRemoteCallsign.Text;
                    withBlock.AGWTimeout = Convert.ToInt32(nudActivityTimeout.Value);
                    withBlock.AGWPacketLength = Convert.ToInt32(nudPacketLength.Value);
                    withBlock.AGWPort = cmbAGWPort.Text;
                    withBlock.AGWScript = txtScript.Text;
                    withBlock.AGWScriptTimeout = Convert.ToInt32(nudScriptTimeout.Value);
                    withBlock.Enabled = chkEnabled.Checked;
                    withBlock.EnableAutoforward = true; // Packet Channels always enabled
                }

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
                var stcUpdateChannel = default(TChannelProperties);
                {
                    var withBlock = stcUpdateChannel;
                    withBlock.ChannelType = EChannelModes.PacketAGW;
                    withBlock.ChannelName = cmbChannelName.Text;
                    withBlock.Priority = Convert.ToInt32(nudPriority.Value);
                    withBlock.RemoteCallsign = txtRemoteCallsign.Text;
                    withBlock.AGWTimeout = Convert.ToInt32(nudActivityTimeout.Value);
                    withBlock.AGWPacketLength = Convert.ToInt32(nudPacketLength.Value);
                    withBlock.AGWPort = cmbAGWPort.Text;
                    withBlock.AGWScript = txtScript.Text;
                    withBlock.AGWScriptTimeout = Convert.ToInt32(nudScriptTimeout.Value);
                    withBlock.AGWMaxFrames = Convert.ToInt32(nudMaxOutstanding.Value);
                    withBlock.Enabled = chkEnabled.Checked;
                    withBlock.EnableAutoforward = true; // Packet Channels always enabled
                }

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