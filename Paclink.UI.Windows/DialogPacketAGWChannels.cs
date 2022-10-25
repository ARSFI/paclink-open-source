using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class DialogPacketAGWChannels : IPacketAgwChannelWindow
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DialogPacketAGWChannels(IPacketAGWChannelBacking backingObject)
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

            BackingObject = backingObject;
        }

        public IPacketAGWChannelBacking BackingObject { get; set; }

        public delegate void AddAGWPortInfoCallback(string AddItem, string Text, bool EnableRetry);

        public void ClearItems()
        {
            if (InvokeRequired)
            {
                Invoke(ClearItems);
                return;
            }

            cmbAGWPort.Items.Clear();
        }

        public void SetAgwPort(string port)
        {
            if (InvokeRequired)
            {
                Invoke(SetAgwPort, new object[] { port });
                return;
            }

            cmbAGWPort.Text = port;
        }

        public void AddAgwPortItem(string port)
        {
            if (InvokeRequired)
            {
                Invoke(AddAgwPortItem, new object[] { port });
                return;
            }

            cmbAGWPort.Items.Add(port);
        }

        private void PacketAGWChannels_Load(object sender, EventArgs e)
        {
            BackingObject.UpdateAGWPortInfo(this);
            ClearEntries();
            FillChannelList();
            cmbChannelName.DroppedDown = true;
        } // PacketAGWChannels_Load

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            foreach (var channelName in BackingObject.ChannelNames)
            {
                cmbChannelName.Items.Add(channelName);
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
            int priority = 0;
            string agwPort = string.Empty;
            string remoteCallsign = string.Empty;
            int activityTimeout = 0;
            int packetLength = 0;
            int maxOutstanding = 0;
            string agwScript = string.Empty;
            bool enabled = false;
            int agwScriptTimeout = 0;

            BackingObject.GetChannelInfo(
                cmbChannelName.Text, out priority, out remoteCallsign, out activityTimeout,
                out packetLength, out maxOutstanding, out agwPort, 
                out agwScript, out agwScriptTimeout, out enabled);

            nudPriority.Value = priority;
            cmbAGWPort.Text = agwPort;
            txtRemoteCallsign.Text = remoteCallsign;
            nudActivityTimeout.Value = activityTimeout;
            nudPacketLength.Value = packetLength;
            nudMaxOutstanding.Value = maxOutstanding;
            nudScriptTimeout.Value = agwScriptTimeout;
            txtScript.Text = agwScript;
            chkEnabled.Checked = enabled;

            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
        } // SetEntries

        private void cmbChannelName_Leave(object sender, EventArgs e)
        {
        } // cmbChannelName_Leave

        private void cmbChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChannelName.Text) && cmbChannelName.Text != "<Enter a new channel>")
            {
                if (BackingObject.ContainsChannel(cmbChannelName.Text))
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

        public void AddAGWPortInfo(string AddItem, string Text, bool EnableRetry)
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
            BackingObject.UpdateAGWPortInfo(this);
        } // btnRetryRemote_Click

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (BackingObject.IsChannelNameAnAccount(cmbChannelName.Text))
            {
                MessageBox.Show(
                    cmbChannelName.Text + " is in use as an account name...",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (BackingObject.IsChannelNameAChannel(cmbChannelName.Text))
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
                BackingObject.AddChannel(
                    cmbChannelName.Text, Convert.ToInt32(nudPriority.Value),
                    txtRemoteCallsign.Text, Convert.ToInt32(nudActivityTimeout.Value),
                    Convert.ToInt32(nudPacketLength.Value), cmbAGWPort.Text,
                    txtScript.Text, Convert.ToInt32(nudScriptTimeout.Value),
                    chkEnabled.Checked);

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
                BackingObject.RemoveChannel(cmbChannelName.Text);
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
                BackingObject.UpdateChannel(
                    cmbChannelName.Text, Convert.ToInt32(nudPriority.Value),
                    txtRemoteCallsign.Text, Convert.ToInt32(nudActivityTimeout.Value),
                    Convert.ToInt32(nudPacketLength.Value), cmbAGWPort.Text,
                    txtScript.Text, Convert.ToInt32(nudScriptTimeout.Value),
                    chkEnabled.Checked);

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
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs140.htm");
        } // btnHelp_Click

        public void RefreshWindow()
        {
            Refresh();
        }

        public void CloseWindow()
        {
            Close();
        }
    }
} // PacketAGWChannels