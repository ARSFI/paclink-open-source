using System;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogTelnetChannels
    {
        public DialogTelnetChannels()
        {
            InitializeComponent();
            _btnAdd.Name = "btnAdd";
            _btnRemove.Name = "btnRemove";
            _btnUpdate.Name = "btnUpdate";
            _btnClose.Name = "btnClose";
            _Label1.Name = "Label1";
            _Label2.Name = "Label2";
            _cmbChannelName.Name = "cmbChannelName";
            _chkEnabled.Name = "chkEnabled";
            _nudPriority.Name = "nudPriority";
            _btnHelp.Name = "btnHelp";
            _Label9.Name = "Label9";
        }

        private TChannelProperties stcSelectedChannel;

        private void TelnetChannels_Load(object sender, EventArgs e)
        {
            ClearEntries();
            FillChannelList();
            cmbChannelName.Text = Globals.Settings.Get("Properties", "Last Telnet Channel", "");
        } // TelnetChannels_Load

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

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            foreach (TChannelProperties stcChannel in Channels.Entries.Values)
            {
                if (stcChannel.ChannelType == EChannelModes.Telnet)
                {
                    if (!string.IsNullOrEmpty(stcChannel.ChannelName.Trim()))
                        cmbChannelName.Items.Add(stcChannel.ChannelName);
                }
            }
        } // FillChannelList

        private void ClearEntries()
        {
            cmbChannelName.Text = "";
            nudPriority.Value = 1;
            chkEnabled.Checked = true;
            btnAdd.Enabled = true;
            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
        } // ClearEntries

        private void SetEntries()
        {
            stcSelectedChannel = (TChannelProperties)Channels.Entries[cmbChannelName.Text];
            {
                var withBlock = stcSelectedChannel;
                nudPriority.Value = withBlock.Priority;
                chkEnabled.Checked = withBlock.Enabled;
            }

            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
        } // SetEntries

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (!Globals.IsValidFileName(cmbChannelName.Text))
            {
                cmbChannelName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(cmbChannelName.Text))
            {
                MessageBox.Show("A channel name is required...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (Channels.IsAccount(cmbChannelName.Text))
            {
                MessageBox.Show(cmbChannelName.Text + " is in use as an account name...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (Channels.IsChannel(cmbChannelName.Text))
            {
                MessageBox.Show("The channel name " + cmbChannelName.Text + " is already in use...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
            }
            else
            {
                var stcNewChannel = new TChannelProperties()
                {
                    ChannelType = EChannelModes.Telnet,
                    ChannelName = cmbChannelName.Text,
                    Priority = Convert.ToInt32(nudPriority.Value),
                    Enabled = chkEnabled.Checked,
                    EnableAutoforward = true, // Telnet Channels always enabled
                    RemoteCallsign = "WL2K"
                };

                Channels.AddChannel(ref stcNewChannel);
                Channels.FillChannelCollection();
                FillChannelList();
                ClearEntries();
                Globals.Settings.Save("Properties", "Last Telnet Channel", cmbChannelName.Text);
                Close();
            }
        } // btnAdd_Click

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show("The telnet channel " + cmbChannelName.Text + " is not found...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (MessageBox.Show(
                "Confirm removal of telnet channel " + cmbChannelName.Text + "...", "Remove Channel",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Channels.RemoveChannel(cmbChannelName.Text);
                Channels.FillChannelCollection();
                FillChannelList();
                Globals.Settings.Save("Properties", "Last Telnet Channel", "");
                Close();
            }
        } // btnRemove_Click

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show("The telnet channel " + cmbChannelName.Text + " is not found...",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                var stcUpdateChannel = default(TChannelProperties);
                {
                    var withBlock = stcUpdateChannel;
                    withBlock.ChannelType = EChannelModes.Telnet;
                    withBlock.ChannelName = cmbChannelName.Text;
                    withBlock.Priority = Convert.ToInt32(nudPriority.Value);
                    withBlock.Enabled = chkEnabled.Checked;
                    withBlock.EnableAutoforward = true; // Telnet Channels always enabled
                    withBlock.RemoteCallsign = "WL2K";
                }

                Channels.UpdateChannel(ref stcUpdateChannel);
                Channels.FillChannelCollection();
                Globals.Settings.Save("Properties", "Last Telnet Channel", cmbChannelName.Text);
                Close();
            }
        } // btnUpdate_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            Globals.Settings.Save("Properties", "Last Telnet Channel", cmbChannelName.Text);
            Close();
        } // btnClose_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs120.htm");
        } // btnHelp_Click
    }
} // TelnetChannels