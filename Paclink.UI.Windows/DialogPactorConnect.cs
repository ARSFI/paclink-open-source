using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class DialogPactorConnect : IPactorConnectWindow
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public IPactorConnectBacking BackingObject
        {
            get;
            private set;
        }

        public DialogPactorConnect(IPactorConnectBacking backingObject)
        {
            BackingObject = backingObject;
            blnLoading = true;

            InitializeComponent();
            _cmbCallSigns.Name = "cmbCallSigns";
            _cmbFrequencies.Name = "cmbFrequencies";
            _Label1.Name = "Label1";
            _Label2.Name = "Label2";
            _lblUSB.Name = "lblUSB";
            _lblBusy.Name = "lblBusy";
            _Label4.Name = "Label4";
            _btnConnect.Name = "btnConnect";
            _btnCancel.Name = "btnCancel";
            _btnHelp.Name = "btnHelp";
            _lblPMBOType.Name = "lblPMBOType";
            _chkResumeDialog.Name = "chkResumeDialog";
        }

        private DateTime dttLastBusyUpdate = DateTime.Now;
        private bool blnChangesNotSaved;
        private bool blnLoading;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnConnect.Enabled = false;
            btnHelp.Enabled = false;
            tmrPollClient.Enabled = false;

            DialogResult = DialogResult.Cancel;
            Close();
            BackingObject.FormClosed();
        } // btnCancel_Click

        private void btnConnect_Click(object sender, EventArgs e)
        {
            tmrPollClient.Enabled = false;
            // Check call signs
            if (!BackingObject.IsValidCallsign(cmbCallSigns.Text))
            {
                MessageBox.Show(cmbCallSigns.Text + " is not a valid amateur or MARS callsign...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbCallSigns.Focus();
                return;
            }

            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = BackingObject.GetCenterFreq(argstrChannel);
            cmbFrequencies.Text = argstrChannel;
            if (!BackingObject.IsValidFreq(strCenterFreq, out intHz))
            {
                MessageBox.Show(
                    "Improper frequency syntax! Select frequency from selected RMS\r\n" +
                    "or enter a frequency 1800 - 54000 KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (intHz < 1800000 | intHz > 54000000)
            {
                MessageBox.Show(
                    "Not a valid frequency. Select frequency from selected RMS\r\n" + 
                    "or enter a frequency 1800 - 54000 KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lblBusy.BackColor == Color.Tomato)
            {
                if (BackingObject.TNCBusyHold)
                {
                    if (MessageBox.Show("The channel appears busy - Continue with connect?", "Channel Busy", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
            }

            DialogResult = DialogResult.OK;
            if (blnChangesNotSaved)
            {
                UpdateChannelProperties();
            }

            if (BackingObject.PactorDialogResume)
            {
                BackingObject.PactorDialogResuming = true;
            }
        } // btnConnect_Click

        public void UpdateChannelProperties()
        {
            BackingObject.UpdateChannel(cmbCallSigns.Text, cmbFrequencies.Text);
            blnChangesNotSaved = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs190.htm");
        } // btnHelp_Click

        public bool ChannelBusy
        {
            get
            {
                bool ChannelBusyRet = default;
                ChannelBusyRet = lblBusy.BackColor == Color.Tomato;
                return ChannelBusyRet;
            }

            set
            {
                if (value)
                {
                    lblBusy.BackColor = Color.Tomato;
                    dttLastBusyUpdate = DateTime.Now;
                }
                else
                {
                    lblBusy.BackColor = Color.Lime;
                }
            }
        } // ChannelBusy

        private void chkResumeDialog_CheckedChanged(object sender, EventArgs e)
        {
            BackingObject.PactorDialogResume = chkResumeDialog.Checked;
        } // chkResumeDialog_CheckedChanged

        private void cmbCallSigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFreqEntry;
            int intIndex = -1;
            string tncName = BackingObject.TNCType;

            cmbCallSigns.Refresh();
            foreach (var freq in BackingObject.GetFrequencies(cmbCallSigns.Text, tncName))
            {
                intIndex++;

                if (intIndex == 0)
                {
                    // Clear frequency list only on the first pass through the loop.
                    cmbFrequencies.Items.Clear();
                    cmbFrequencies.Text = "";
                    cmbFrequencies.Items.Add(freq);
                }
            }

            blnChangesNotSaved = true;
        } // cmbCallSigns_SelectedIndexChanged

        private void cmbFrequencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbFrequencies.Refresh();
            if (blnLoading)
                return;
            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = BackingObject.GetCenterFreq(argstrChannel);
            //cmbFrequencies.Text = argstrChannel;
            if (!BackingObject.IsValidFreq(strCenterFreq, out intHz))
            {
                return;
            }
            else if (intHz < 1800000 || intHz > 54000000)
            {
                lblUSB.Text = "USB Dial: -----";
            }
            else
            {
                try
                {
                    intHz -= BackingObject.AudioToneCenter;
                    lblUSB.Text = "USB Dial: " + (intHz / (double)1000).ToString("##0000.000") + " KHz";
                }
                catch
                {
                    lblUSB.Text = "USB Dial: -----";
                }
            }

            Refresh();

            BackingObject.SetRadioControlInfo(cmbFrequencies.Text);
            blnChangesNotSaved = true;
        } // Sub cmbFreqs_SelectedIndexChanged

        private void cmbFrequencies_TextChanged(object sender, EventArgs e)
        {
            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = BackingObject.GetCenterFreq(argstrChannel);
            //cmbFrequencies.Text = argstrChannel;
            if (!BackingObject.IsValidFreq(strCenterFreq, out intHz))
            {
                return;
            }
            else if (intHz < 1800000 || intHz > 54000000)
            {
                lblUSB.Text = "USB Dial: -----";
            }
            else
            {
                try
                {
                    intHz -= BackingObject.AudioToneCenter;
                    lblUSB.Text = "USB Dial: " + (intHz / (double)1000).ToString("##0000.000") + " KHz";
                    BackingObject.SetRadioControlInfo(strCenterFreq);
                }
                catch
                {
                    lblUSB.Text = "USB Dial: -----";
                }
            }

            Refresh();
        } // cmbFrequencies_TextChanged

        private void DialogPactorConnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            BackingObject.FormClosing();
            BackingObject.WindowLeft = Left;
            BackingObject.WindowTop = Top;
        } // DialogPactorConnect_FormClosing

        private void PactorConnect_Load(object sender, EventArgs e)
        {
            // Initializize the controls...
            var aryResults = BackingObject.ChannelNames.ToArray();
            int intIndex;
            string strFreqList;
            Top = BackingObject.WindowTop;
            Left = BackingObject.WindowLeft;
            try
            {
                BringToFront();
                Text = "Pactor: " + BackingObject.ChannelName;
                lblPMBOType.Text = BackingObject.ServiceCodes;
                chkResumeDialog.Checked = BackingObject.PactorDialogResume;
                if (aryResults.Length == 0)
                {
                    MessageBox.Show("Click 'Update Channel List' to download the list of available channels");
                    return;
                }

                if (aryResults.Length > 0)
                {
                    cmbCallSigns.Items.Clear();

                    foreach (var callsign in aryResults)
                    {
                        cmbCallSigns.Items.Add(callsign);
                    }

                    cmbCallSigns.Text = BackingObject.RemoteCallsign;
                    cmbFrequencies.Text = BackingObject.CenterFrequency;
                }
                else
                {
                    cmbCallSigns.Items.Clear();
                    cmbCallSigns.Text = BackingObject.RemoteCallsign;
                    cmbFrequencies.Items.Clear();
                    cmbFrequencies.Text = BackingObject.CenterFrequency;
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                // Set radio parameters if RadioControl class active and a frequency is assigned...

                if (BackingObject.CanRadioControl)
                {
                    if (!BackingObject.IsValidFrequency)
                    {
                        MessageBox.Show("Freq Syntax Error! Enter value in KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (!BackingObject.RefreshRadioControlInfo())
                    {
                        MessageBox.Show(
                            "Failure to set Radio parameters...Check exception log for details!",
                            "Radio Control Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                blnLoading = false;
            }
            catch (Exception ex)
            {
                Log.Error("[PactorConnect.PactorConnect_Load] " + ex.Message);
            }
        } // PactorConnect_Load

        private void tmrPollClient_Tick(object sender, EventArgs e)
        {
            BringToFront();
            BackingObject.PollModem();
        } // tmrPollClient_Tick

        public void RefreshWindow()
        {
            Refresh();
        }

        public void CloseWindow()
        {
            Close();
        }

        public UiDialogResult ShowModal()
        {
            ShowModal();

            if (DialogResult == DialogResult.Cancel)
            {
                return UiDialogResult.Cancel;
            }
            else
            {
                return UiDialogResult.OK;
            }
        }
    } // DialogPactorConnect
}