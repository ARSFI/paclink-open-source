using Paclink.UI.Common;
using System;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogPolling : IPollingWindow
    {
        private IPollingBacking _backingObject;

        public DialogPolling(IPollingBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _chkAutoPoll.Name = "chkAutoPoll";
            _chkAutoSend.Name = "chkAutoSend";
            _txtInterval.Name = "txtInterval";
            _Label1.Name = "Label1";
            _btnUpdate.Name = "btnUpdate";
            _btnCancel.Name = "btnCancel";
            _btnHelp.Name = "btnHelp";
        }

        public IPollingBacking BackingObject => _backingObject;

        private void Properties_Load(object sender, EventArgs e)
        {
            chkAutoPoll.Checked = BackingObject.AutoPoll;
            txtInterval.Enabled = BackingObject.AutoPoll;
            txtInterval.Text = BackingObject.AutoPollInterval.ToString();
            chkAutoSend.Enabled = true;
            chkAutoSend.Checked = BackingObject.PollOnReceivedMessage;
        } // Properties_Load

        private void chkAutoPoll_CheckedChanged(object sender, EventArgs e)
        {
            txtInterval.Enabled = chkAutoPoll.Checked;
        } // chkAutoPoll_CheckedChanged

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Update connection properties...
            bool newAutoPoll = chkAutoPoll.Checked;
            if (!newAutoPoll)
                txtInterval.Text = "60";

            int newAutoPollInterval = 60;
            try
            {
                newAutoPollInterval = Convert.ToInt32(txtInterval.Text);
            }
            catch
            {
                MessageBox.Show("Polling interval must be an integer between 3 and 120 minutes!");
                return;
            }

            if (newAutoPollInterval < 3 | newAutoPollInterval > 120)
            {
                MessageBox.Show("Polling interval must be an integer between 3 and 120 minutes!");
                return;
            }
            else if (newAutoPollInterval < 15)
            {
                if (MessageBox.Show("To reduce QRM and channel loading polling intervals less than 15\r\nminutes should be used only during emergencies!   Continue?", "Short Polling Interval!", MessageBoxButtons.YesNo) != DialogResult.Yes)

                    return;
            }

            bool newPollOnReceivedMessage = chkAutoSend.Checked;
            BackingObject.UpdateParameters(newPollOnReceivedMessage, newAutoPoll, newAutoPollInterval, newAutoPollInterval);
            Close();
        } // btnUpdate_Click

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        } // btnCancel_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs200.htm");
        } // btnHelp_Click

        public void RefreshWindow()
        {
            throw new NotImplementedException();
        }

        public void CloseWindow()
        {
            throw new NotImplementedException();
        }

        public UiDialogResult ShowModal()
        {
            throw new NotImplementedException();
        }
    }
} // Polling