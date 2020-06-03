using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public partial class DialogPolling
    {
        public DialogPolling()
        {
            InitializeComponent();
            _chkAutoPoll.Name = "chkAutoPoll";
            _chkAutoSend.Name = "chkAutoSend";
            _txtInterval.Name = "txtInterval";
            _Label1.Name = "Label1";
            _btnUpdate.Name = "btnUpdate";
            _btnCancel.Name = "btnCancel";
            _btnHelp.Name = "btnHelp";
        }

        public static bool PollOnReceivedMessage;     // Start polling immediately on a received message if set
        public static bool AutoPoll;                  // Polls every PollInterval minutes if set
        public static int AutoPollInterval;          // Poll interval in minutes
        public static int MinutesRemaining;          // Minutes remaining until the next poll

        public static void InitializePollingFlags()
        {
            AutoPoll = Globals.objINIFile.GetBoolean(Application.ProductName, "Auto Poll", false);
            PollOnReceivedMessage = Globals.objINIFile.GetBoolean(Application.ProductName, "Poll on Received Message", false);
            AutoPollInterval = Globals.objINIFile.GetInteger(Application.ProductName, "Auto Poll Interval", 60);
            MinutesRemaining = AutoPollInterval;
        } // InitializePolling

        private void Properties_Load(object sender, EventArgs e)
        {
            chkAutoPoll.Checked = AutoPoll;
            txtInterval.Enabled = AutoPoll;
            txtInterval.Text = AutoPollInterval.ToString();
            chkAutoSend.Enabled = true;
            chkAutoSend.Checked = PollOnReceivedMessage;
        } // Properties_Load

        private void chkAutoPoll_CheckedChanged(object sender, EventArgs e)
        {
            txtInterval.Enabled = chkAutoPoll.Checked;
        } // chkAutoPoll_CheckedChanged

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Update connection properties...
            AutoPoll = chkAutoPoll.Checked;
            if (!AutoPoll)
                txtInterval.Text = "60";
            try
            {
                AutoPollInterval = Conversions.ToInteger(txtInterval.Text);
            }
            catch
            {
                Interaction.MsgBox("Polling interval must be an integer between 3 and 120 minutes!");
                return;
            }

            if (AutoPollInterval < 3 | AutoPollInterval > 120)
            {
                Interaction.MsgBox("Polling interval must be an integer between 3 and 120 minutes!");
                return;
            }
            else if (AutoPollInterval < 15)
            {
                if (Interaction.MsgBox("To reduce QRM and channel loading polling intervals less than 15" + Constants.vbCrLf + "minutes should be used only during emergencies!   Continue?", MsgBoxStyle.YesNo, "Short Polling Interval!") != MsgBoxResult.Yes)

                    return;
            }

            PollOnReceivedMessage = chkAutoSend.Checked;
            Globals.objINIFile.WriteBoolean(Application.ProductName, "Auto Poll", AutoPoll);
            Globals.objINIFile.WriteBoolean(Application.ProductName, "Poll on Received Message", PollOnReceivedMessage);
            Globals.objINIFile.WriteInteger(Application.ProductName, "Auto Poll Interval", AutoPollInterval);
            MinutesRemaining = AutoPollInterval;
            Close();
        } // btnUpdate_Click

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        } // btnCancel_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs200.htm");
        } // btnHelp_Click
    }
} // Polling