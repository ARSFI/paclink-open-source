using System;
using System.Windows.Forms;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class DialogTelnetChannels
    {
        private ITelnetChannelsBacking _backingObject = null;
        public ITelnetChannelsBacking BackingObject => _backingObject;

        public DialogTelnetChannels(ITelnetChannelsBacking backingObject)
        {
            _backingObject = backingObject;

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

        private void TelnetChannels_Load(object sender, EventArgs e)
        {
            ClearEntries();
            FillChannelList();
            cmbChannelName.Text = BackingObject.GetLastUsedTelnetChannel();
        }

        private void cmbChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChannelName.Text) & cmbChannelName.Text != "<Enter a new channel>")
            {
                if (BackingObject.ChannelExists(cmbChannelName.Text))
                {
                    SetEntries();
                }
                else
                {
                    ClearEntries();
                }
            }
        }

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
        }

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            var channelNames = BackingObject.GetTelnetChannelNames();
            foreach (var channelName in channelNames)
            {
                cmbChannelName.Items.Add(channelName);
            }
        }

        private void ClearEntries()
        {
            cmbChannelName.Text = "";
            nudPriority.Value = 1;
            chkEnabled.Checked = true;
            btnAdd.Enabled = true;
            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void SetEntries()
        {
            nudPriority.Value = BackingObject.GetChannelPriority(cmbChannelName.Text);
            chkEnabled.Checked = BackingObject.IsChannelEnabled(cmbChannelName.Text);
            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (!BackingObject.IsValidChannelName(cmbChannelName.Text))
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

            if (BackingObject.IsAccount(cmbChannelName.Text))
            {
                MessageBox.Show(cmbChannelName.Text + " is in use as an account name...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (BackingObject.IsChannel(cmbChannelName.Text))
            {
                MessageBox.Show("The channel name " + cmbChannelName.Text + " is already in use...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
            }
            else
            {
                BackingObject.AddChannel(cmbChannelName.Text, ChannelMode.Telnet, Convert.ToInt32(nudPriority.Value), chkEnabled.Checked, true, "WL2K");
                BackingObject.FillChannelCollection();
                FillChannelList();
                ClearEntries();
                BackingObject.SaveCurrentTelnetChannel(cmbChannelName.Text);
                Close();
            }
        }

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
                BackingObject.RemoveChannel(cmbChannelName.Text);
                BackingObject.FillChannelCollection();
                FillChannelList();
                BackingObject.SaveCurrentTelnetChannel("");
                Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show("The telnet channel " + cmbChannelName.Text + " is not found...",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                BackingObject.UpdateChannel(cmbChannelName.Text, ChannelMode.Telnet, Convert.ToInt32(nudPriority.Value), chkEnabled.Checked, true, "WL2K");
                BackingObject.FillChannelCollection();
                BackingObject.SaveCurrentTelnetChannel(cmbChannelName.Text);
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            BackingObject.SaveCurrentTelnetChannel(cmbChannelName.Text);
            Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs120.htm");
        }
    }
}