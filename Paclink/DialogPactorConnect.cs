using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public partial class DialogPactorConnect
    {
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

        public DialogPactorConnect(IClient objSender, ref TChannelProperties Channel)
        {
            // This call is required by the Windows Form Designer...
            objClient = objSender;
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

        private TChannelProperties stcChannel;
        private string[] arySelectedMBOs;
        private DateTime dttLastBusyUpdate = DateTime.Now;
        private bool blnChangesNotSaved;
        private IClient objClient;
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
            Globals.objSelectedClient.Close();
            Globals.objSelectedClient = null;
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
                Interaction.MsgBox(cmbCallSigns.Text + " is not a valid amateur or MARS callsign...", MsgBoxStyle.Information);
                cmbCallSigns.Focus();
                return;
            }

            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = Globals.ExtractFreq(ref argstrChannel);
            cmbFrequencies.Text = argstrChannel;
            if (!Globals.IsValidFrequency(strCenterFreq, ref intHz))
            {
                Interaction.MsgBox("Improper frequency syntax! Select frequency from selected RMS " + Globals.CR + "or enter a frequency 1800 - 54000 KHz.", MsgBoxStyle.Information);
                return;
            }
            else if (intHz < 1800000 | intHz > 54000000)
            {
                Interaction.MsgBox("Not a valid frequency. Select frequency from selected RMS " + Globals.CR + "or enter a frequency 1800 - 54000 KHz.", MsgBoxStyle.Information);
                return;
            }

            if (lblBusy.BackColor == Color.Tomato)
            {
                if (stcChannel.TNCBusyHold)
                {
                    if (Interaction.MsgBox("The channel appears busy - Continue with connect?", MsgBoxStyle.YesNo, "Channel Busy") == MsgBoxResult.No)
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
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs190.htm");
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
            Globals.objINIFile.WriteBoolean("Properties", "Pactor Dialog Resume", Globals.blnPactorDialogResume);
        } // chkResumeDialog_CheckedChanged

        private void cmbCallSigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFreqEntry;
            int intIndex;
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
                            if (Globals.CanUseFrequency(strFreqEntry, ""))
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
            cmbFrequencies.Refresh();
            if (blnLoading)
                return;
            var intHz = default(int);
            string argstrChannel = cmbFrequencies.Text;
            string strCenterFreq = Globals.ExtractFreq(ref argstrChannel);
            cmbFrequencies.Text = argstrChannel;
            if (!Globals.IsValidFrequency(strCenterFreq, ref intHz))
            {
                return;
            }
            else if (intHz < 1800000 | intHz > 54000000)
            {
                lblUSB.Text = "USB Dial: -----";
            }
            else
            {
                try
                {
                    intHz -= Convert.ToInt32(stcChannel.AudioToneCenter);
                    lblUSB.Text = "USB Dial: " + Strings.Format(intHz / (double)1000, "##0000.000") + " KHz";
                }
                catch
                {
                    lblUSB.Text = "USB Dial: -----";
                }
            }

            Refresh();
            stcChannel.RDOCenterFrequency = cmbFrequencies.Text;
            if (!Information.IsNothing(Globals.objRadioControl))
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
            cmbFrequencies.Text = argstrChannel;
            if (!Globals.IsValidFrequency(strCenterFreq, ref intHz))
            {
                return;
            }
            else if (intHz < 1800000 | intHz > 54000000)
            {
                lblUSB.Text = "USB Dial: -----";
            }
            else
            {
                try
                {
                    intHz -= Convert.ToInt32(stcChannel.AudioToneCenter);
                    lblUSB.Text = "USB Dial: " + Strings.Format(intHz / (double)1000, "##0000.000") + " KHz";
                    stcChannel.RDOCenterFrequency = strCenterFreq;
                    Globals.objRadioControl.SetParameters(ref stcChannel);
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
            Globals.objINIFile.WriteInteger("Pactor Control", "Top", Top);
            Globals.objINIFile.WriteInteger("Pactor Control", "Left", Left);
        } // DialogPactorConnect_FormClosing

        private void PactorConnect_Load(object sender, EventArgs e)
        {
            // Initializize the controls...
            var aryResults = new string[0];
            int intIndex;
            string strFreqList;
            Top = Globals.objINIFile.GetInteger("Pactor Control", "Top", 100);
            Left = Globals.objINIFile.GetInteger("Pactor Control", "Left", 100);
            try
            {
                BringToFront();
                Text = "Pactor: " + stcChannel.ChannelName;
                lblPMBOType.Text = Globals.strServiceCodes;
                chkResumeDialog.Checked = Globals.blnPactorDialogResume;
                if (File.Exists(Globals.SiteRootDirectory + @"\Data\RMS Channels.dat"))
                {
                    aryResults = Channels.ParseChannelList(Globals.SiteRootDirectory + @"\Data\RMS Channels.dat");
                }
                else
                {
                    Interaction.MsgBox("Click 'Update Channel List' to download the list of available channels");
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

                if (!Information.IsNothing(Globals.objRadioControl))
                {
                    int argintFreqHz = 0;
                    if (!Globals.IsValidFrequency(Globals.StripMode(stcChannel.RDOCenterFrequency), intFreqHz: ref argintFreqHz))
                    {
                        Interaction.MsgBox("Freq Syntax Error! Enter value in KHz.", MsgBoxStyle.Exclamation);
                    }
                    else if (!Globals.objRadioControl.SetParameters(ref stcChannel))
                    {
                        Interaction.MsgBox("Failure to set Radio parameters...Check exception log for details!", MsgBoxStyle.Exclamation, "Radio Control Error!");
                    }
                }

                blnLoading = false;
            }
            catch
            {
                Logs.Exception("[PactorConnect.PactorConnect_Load] " + Information.Err().Description);
            }
        } // PactorConnect_Load

        public string[] ParsePMBOList(string strFilename)
        {
            // Function to parse the PMBO frequency list (used for Public, EMComm and MARS
            // Returns an empty string array if error or file not found...

            var aryResult = new string[0];
            if (!File.Exists(strFilename))
                return aryResult;
            try
            {
                var strLines = File.ReadAllLines(strFilename);
                var blnSeparatorFound = default(bool);
                var blnNewCallsign = default(bool);
                string strCallsign = "";
                string strFrequencies = "";
                foreach (string strLine in strLines)
                {
                    if (blnSeparatorFound)
                    {
                        if (string.IsNullOrEmpty(strLine.Trim()) & !string.IsNullOrEmpty(strCallsign)) // Blank line at end of station record
                        {
                            if (!string.IsNullOrEmpty(strFrequencies)) // Only put in list if frequency is available
                            {
                                Array.Resize(ref aryResult, aryResult.Length + 1); // Make one larger
                                string strCallsignAndFrequencies = strCallsign + "," + strFrequencies;
                                if (strCallsignAndFrequencies.EndsWith(","))
                                    strCallsignAndFrequencies = strCallsignAndFrequencies.Substring(0, strCallsignAndFrequencies.Length - 1);
                                aryResult[aryResult.Length - 1] = strCallsignAndFrequencies;
                            }

                            blnNewCallsign = false;
                            strCallsign = "";
                        }
                        else if (blnNewCallsign & strLine.Trim().Length > 2)
                        {
                            if ("P E H T A S ".IndexOf(strLine.Trim().Substring(0, 2)) == -1)
                            {
                                var strTokens = strLine.Split(' ');
                                foreach (string strToken in strTokens)
                                {
                                    if (Information.IsNumeric(strToken.Replace("#", "")))
                                    {
                                        int intFrequency = Convert.ToInt32(strToken.Substring(0, strToken.IndexOf(".")));
                                        if (intFrequency > 1800 & intFrequency < 54000) // Only select HF frequencies
                                        {
                                            strFrequencies += strToken.Trim() + ",";
                                        }
                                    }
                                }
                            }
                        }
                        else if (!blnNewCallsign & !string.IsNullOrEmpty(strLine.Trim()))
                        {
                            var strFirstLineTokens = strLine.Split(',');
                            strCallsign = strFirstLineTokens[0].Trim();
                            if (strCallsign.IndexOf(".") != -1)
                                strCallsign = strCallsign.Substring(0, strCallsign.IndexOf(".")).Trim();
                            if (strCallsign.IndexOf("/") != -1)
                                strCallsign = strCallsign.Substring(1 + strCallsign.IndexOf("/")).Trim();
                            blnNewCallsign = true;
                            strFrequencies = "";
                        }
                    }
                    else
                    {
                        blnSeparatorFound = strLine.IndexOf("------------------------") != -1;
                    }
                }
            }
            catch
            {
                return aryResult;
            }

            return aryResult;
        } // ParsePMBOList

        private void tmrPollClient_Tick(object sender, EventArgs e)
        {
            BringToFront();
            objClient.Poll();
        } // tmrPollClient_Tick

        public void UpdateChannelProperties(ref TChannelProperties Channel)
        {
            Channel.RemoteCallsign = cmbCallSigns.Text.Trim().ToUpper();
            Channel.RDOCenterFrequency = cmbFrequencies.Text.Trim();
            // Channels.UpdateChannel(Channel)
            Globals.stcEditedSelectedChannel = Channel;
            blnChangesNotSaved = false;
        } // UpdateProperties
    } // DialogPactorConnect
}