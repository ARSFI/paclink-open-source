using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public partial class DialogPactorConnect
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DialogPactorConnect()
        {
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

        public DialogPactorConnect(IModem objSender, ref ChannelProperties Channel)
        {
            // This call is required by the Windows Form Designer...
            _objModem = objSender;
            stcChannel = Channel;
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
        } // New

        private ChannelProperties stcChannel;
        private string[] arySelectedMBOs;
        private DateTime dttLastBusyUpdate = DateTime.Now;
        private bool blnChangesNotSaved;
        private IModem _objModem;
        private bool blnLoading;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnConnect.Enabled = false;
            btnHelp.Enabled = false;
            tmrPollClient.Enabled = false;
            Globals.blnPactorDialogClosing = true;
            Globals.blnPactorDialogResuming = false;
            Globals.blnChannelActive = false;
            Globals.ObjSelectedModem.Close();
            Globals.ObjSelectedModem = null;
            Globals.stcEditedSelectedChannel = default;
            DialogResult = DialogResult.Cancel;
            Close();
        } // btnCancel_Click

        private void btnConnect_Click(object sender, EventArgs e)
        {
            tmrPollClient.Enabled = false;
            // Check call signs
            if (!Globals.IsValidRadioCallsign(cmbCallSigns.Text))
            {
                MessageBox.Show(cmbCallSigns.Text + " is not a valid amateur or MARS callsign...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbCallSigns.Focus();
                return;
            }

            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = Globals.ExtractFreq(ref argstrChannel);
            cmbFrequencies.Text = argstrChannel;
            if (!Globals.IsValidFrequency(strCenterFreq, ref intHz))
            {
                MessageBox.Show(
                    "Improper frequency syntax! Select frequency from selected RMS " + Globals.CR +
                    "or enter a frequency 1800 - 54000 KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (intHz < 1800000 | intHz > 54000000)
            {
                MessageBox.Show(
                    "Not a valid frequency. Select frequency from selected RMS " + Globals.CR + 
                    "or enter a frequency 1800 - 54000 KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lblBusy.BackColor == Color.Tomato)
            {
                if (stcChannel.TNCBusyHold)
                {
                    if (MessageBox.Show("The channel appears busy - Continue with connect?", "Channel Busy", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
            }

            DialogResult = DialogResult.OK;
            if (blnChangesNotSaved)
                UpdateChannelProperties(ref stcChannel);
            if (Globals.blnPactorDialogResume)
                Globals.blnPactorDialogResuming = true;
        } // btnConnect_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs190.htm");
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
            Globals.blnPactorDialogResume = chkResumeDialog.Checked;
            Globals.Settings.Save("Properties", "Pactor Dialog Resume", Globals.blnPactorDialogResume);
        } // chkResumeDialog_CheckedChanged

        private void cmbCallSigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFreqEntry;
            int intIndex;
            string tncName = stcChannel.TNCType;

            cmbCallSigns.Refresh();
            for (int i = 0, loopTo = arySelectedMBOs.Length - 1; i <= loopTo; i++)
            {
                if (arySelectedMBOs[i].StartsWith(cmbCallSigns.Text))
                {
                    var aryFrequencies = arySelectedMBOs[i].Substring(arySelectedMBOs[i].IndexOf(":") + 1).Split(',');
                    if (aryFrequencies.Length > 1)
                    {
                        cmbFrequencies.Items.Clear();
                        cmbFrequencies.Text = "";
                        intIndex = 0;
                        for (int j = 0, loopTo1 = aryFrequencies.Length - 1; j <= loopTo1; j++)
                        {
                            strFreqEntry = aryFrequencies[j].ToString();
                            if (Globals.CanUseFrequency(strFreqEntry, tncName))
                            {
                                cmbFrequencies.Items.Add(Globals.FormatFrequency(strFreqEntry));
                                intIndex += 1;
                            }
                        }
                    }

                    break;
                }
            }

            stcChannel.RemoteCallsign = cmbCallSigns.Text;
            blnChangesNotSaved = true;
        } // cmbCallSigns_SelectedIndexChanged

        private void cmbFrequencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cmbFrequencies.Refresh();
            if (blnLoading)
                return;
            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = Globals.ExtractFreq(ref argstrChannel);
            //cmbFrequencies.Text = argstrChannel;
            if (!Globals.IsValidFrequency(strCenterFreq, ref intHz))
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
                    intHz -= Convert.ToInt32(stcChannel.AudioToneCenter);
                    lblUSB.Text = "USB Dial: " + (intHz / (double)1000).ToString("##0000.000") + " KHz";
                }
                catch
                {
                    lblUSB.Text = "USB Dial: -----";
                }
            }

            Refresh();
            stcChannel.RDOCenterFrequency = cmbFrequencies.Text;
            if (Globals.objRadioControl != null)
            {
                Globals.objRadioControl.SetParameters(ref stcChannel);
            }

            blnChangesNotSaved = true;
        } // Sub cmbFreqs_SelectedIndexChanged

        private void cmbFrequencies_TextChanged(object sender, EventArgs e)
        {
            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = Globals.ExtractFreq(ref argstrChannel);
            //cmbFrequencies.Text = argstrChannel;
            if (!Globals.IsValidFrequency(strCenterFreq, ref intHz))
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
                    intHz -= Convert.ToInt32(stcChannel.AudioToneCenter);
                    lblUSB.Text = "USB Dial: " + (intHz / (double)1000).ToString("##0000.000") + " KHz";
                    stcChannel.RDOCenterFrequency = strCenterFreq;
                    if (Globals.objRadioControl != null)
                    {
                        Globals.objRadioControl.SetParameters(ref stcChannel);
                    }
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
            Globals.Settings.Save("Pactor Control", "Top", Top);
            Globals.Settings.Save("Pactor Control", "Left", Left);
        } // DialogPactorConnect_FormClosing

        private void PactorConnect_Load(object sender, EventArgs e)
        {
            // Initializize the controls...
            var aryResults = Channels.ParseChannelList(false);
            int intIndex;
            string strFreqList;
            Top = Globals.Settings.Get("Pactor Control", "Top", 100);
            Left = Globals.Settings.Get("Pactor Control", "Left", 100);
            try
            {
                BringToFront();
                Text = "Pactor: " + stcChannel.ChannelName;
                lblPMBOType.Text = Globals.strServiceCodes;
                chkResumeDialog.Checked = Globals.blnPactorDialogResume;
                if (aryResults.Length == 0)
                {
                    MessageBox.Show("Click 'Update Channel List' to download the list of available channels");
                    return;
                }

                if (aryResults.Length > 0)
                {
                    arySelectedMBOs = aryResults;
                    cmbCallSigns.Items.Clear();
                    foreach (string strStationCallsign in aryResults)
                        cmbCallSigns.Items.Add(strStationCallsign.Substring(0, strStationCallsign.IndexOf(":")));
                    cmbCallSigns.Text = stcChannel.RemoteCallsign;
                    cmbFrequencies.Text = stcChannel.RDOCenterFrequency;
                    foreach (string station in aryResults)
                    {
                        intIndex = station.IndexOf(":");
                        strFreqList = station.Substring(intIndex + 1);
                        if (Globals.AnyUseableFrequency(strFreqList, ""))
                        {
                            cmbCallSigns.Items.Add(station.Substring(0, intIndex));
                        }
                    }

                    cmbCallSigns.Text = stcChannel.RemoteCallsign;
                    cmbFrequencies.Text = stcChannel.RDOCenterFrequency;
                }
                else
                {
                    cmbCallSigns.Items.Clear();
                    cmbCallSigns.Text = stcChannel.RemoteCallsign;
                    cmbFrequencies.Items.Clear();
                    cmbFrequencies.Text = stcChannel.RDOCenterFrequency;
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                // Set radio parameters if RadioControl class active and a frequency is assigned...

                if (Globals.objRadioControl != null)
                {
                    int argintFreqHz = 0;
                    if (!Globals.IsValidFrequency(Globals.StripMode(stcChannel.RDOCenterFrequency), intFreqHz: ref argintFreqHz))
                    {
                        MessageBox.Show("Freq Syntax Error! Enter value in KHz.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (!Globals.objRadioControl.SetParameters(ref stcChannel))
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
            _objModem.Poll();
        } // tmrPollClient_Tick

        public void UpdateChannelProperties(ref ChannelProperties Channel)
        {
            Channel.RemoteCallsign = cmbCallSigns.Text.Trim().ToUpper();
            Channel.RDOCenterFrequency = cmbFrequencies.Text.Trim();
            // Channels.UpdateChannel(Channel)
            Globals.stcEditedSelectedChannel = Channel;
            blnChangesNotSaved = false;
        } // UpdateProperties
    } // DialogPactorConnect
}