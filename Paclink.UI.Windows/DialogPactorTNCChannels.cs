using Paclink.UI.Common;
using System;
using System.Linq;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogPactorTNCChannels : IPactorTNCChannelWindow
    {
        public IPactorTNCChannelBacking BackingObject
        {
            get;
            private set;
        }

        public DialogPactorTNCChannels(IPactorTNCChannelBacking backingObject)
        {
            InitializeComponent();
            _btnClose.Name = "btnClose";
            _btnUpdate.Name = "btnUpdate";
            _btnRemove.Name = "btnRemove";
            _btnAdd.Name = "btnAdd";
            _cmbRadioModel.Name = "cmbRadioModel";
            _cmbRadioPort.Name = "cmbRadioPort";
            _cmbRadioBaud.Name = "cmbRadioBaud";
            _txtAudioCenter.Name = "txtAudioCenter";
            _cmbTNCBaudRate.Name = "cmbTNCBaudRate";
            _cmbTNCSerialPort.Name = "cmbTNCSerialPort";
            _txtTNCConfigurationFile.Name = "txtTNCConfigurationFile";
            _cmbTNCType.Name = "cmbTNCType";
            _cmbChannelName.Name = "cmbChannelName";
            _txtRadioAddress.Name = "txtRadioAddress";
            _chkNarrowFilter.Name = "chkNarrowFilter";
            _rdoViaPTCII.Name = "rdoViaPTCII";
            _nudFSKLevel.Name = "nudFSKLevel";
            _nudPSKLevel.Name = "nudPSKLevel";
            _rdoSerial.Name = "rdoSerial";
            _rdoManual.Name = "rdoManual";
            _btnBrowseConfiguration.Name = "btnBrowseConfiguration";
            _grpChannelSetting.Name = "grpChannelSetting";
            _btnUpdateChannelList.Name = "btnUpdateChannelList";
            _chkBusyHold.Name = "chkBusyHold";
            _chkIDEnabled.Name = "chkIDEnabled";
            _chkChannelEnabled.Name = "chkChannelEnabled";
            _chkAutoforwardEnabled.Name = "chkAutoforwardEnabled";
            _Label3.Name = "Label3";
            _Label10.Name = "Label10";
            _Label5.Name = "Label5";
            _cmbFreqs.Name = "cmbFreqs";
            _cmbCallSigns.Name = "cmbCallSigns";
            _nudPriority.Name = "nudPriority";
            _nudActivityTimeout.Name = "nudActivityTimeout";
            _Label4.Name = "Label4";
            _Label2.Name = "Label2";
            _Label1.Name = "Label1";
            _chkFirstUseOnly.Name = "chkFirstUseOnly";
            _Label13.Name = "Label13";
            _rdoV24.Name = "rdoV24";
            _rdoTTL.Name = "rdoTTL";
            _chkNMEA.Name = "chkNMEA";
            _grpRadioControl.Name = "grpRadioControl";
            _grpPTCLevels.Name = "grpPTCLevels";
            _Label15.Name = "Label15";
            _lblRadioAddress.Name = "lblRadioAddress";
            _Label8.Name = "Label8";
            _Label9.Name = "Label9";
            _grpTNCSettings.Name = "grpTNCSettings";
            _lblPSKLevel.Name = "lblPSKLevel";
            _lblFSKLevel.Name = "lblFSKLevel";
            _Label14.Name = "Label14";
            _Label18.Name = "Label18";
            _Label7.Name = "Label7";
            _Label21.Name = "Label21";
            _Label22.Name = "Label22";
            _btnHelp.Name = "btnHelp";
            _Label16.Name = "Label16";

            BackingObject = backingObject;
        }
        
        private string[] arySelectedMBOs;

        private void PactorTNCChannels_Load(object sender, EventArgs e)
        {
            InitializeControls();
            ClearEntries();
            FillChannelList();
            cmbChannelName.Text = BackingObject.LastPactorChannel;

            if (!string.IsNullOrEmpty(cmbChannelName.Text) && cmbChannelName.Text != "<Enter a new channel>")
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
        } // PactorTNCChannels_Load

        private void InitializeControls()
        {
            SetRMSList();

            cmbTNCType.Items.AddRange(BackingObject.TncTypes.ToArray());
            cmbTNCSerialPort.Items.AddRange(BackingObject.SerialPorts.ToArray());
            cmbTNCBaudRate.Items.AddRange(BackingObject.BaudRates.ToArray());
            cmbRadioBaud.Items.AddRange(BackingObject.BaudRates.ToArray());
            cmbRadioPort.Items.AddRange(BackingObject.SerialPorts.ToArray());
            cmbRadioModel.Items.AddRange(BackingObject.RadioModels.ToArray());
            if (BackingObject.IsAutoforwardEnabled)
            {
                chkAutoforwardEnabled.Visible = true;
            }
            else
            {
                chkAutoforwardEnabled.Visible = false;
                chkAutoforwardEnabled.Checked = false;
            }
        } // InitializeControls

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            cmbChannelName.Items.AddRange(BackingObject.ChannelNames.ToArray());
        } // FillChannelList

        private void ClearEntries()
        {
            cmbChannelName.Text = "";
            cmbCallSigns.Text = "";
            cmbFreqs.Text = "";
            cmbTNCType.Text = "";
            cmbTNCSerialPort.Text = "";
            nudPriority.Value = 5;
            nudActivityTimeout.Value = 4;
            chkChannelEnabled.Checked = true;
            chkAutoforwardEnabled.Checked = false;
            chkBusyHold.Checked = true;
            chkIDEnabled.Checked = true;
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
            if (cmbTNCSerialPort.Items.Count != 0)
                cmbTNCSerialPort.Text = cmbTNCSerialPort.Items[0].ToString();
            cmbTNCBaudRate.Text = "9600";
            txtTNCConfigurationFile.Text = "";
            txtAudioCenter.Text = "1500";
            rdoManual.Checked = true;
            lblFSKLevel.Enabled = false;
            lblPSKLevel.Enabled = false;
            nudFSKLevel.Enabled = false;
            nudPSKLevel.Enabled = false;
            cmbFreqs.Enabled = false;
            cmbCallSigns.Enabled = false;
            cmbRadioModel.Text = "00";
            lblRadioAddress.Enabled = false;
            txtRadioAddress.Enabled = false;
            txtRadioAddress.Text = "";
        } // ClearEntries

        private void SetEntries()
        {
            bool firstUseOnly;
            int priority;
            int activityTimeout;
            bool enabled;
            bool autoForward;
            bool busyHold;
            string tncSerialPort;
            string tncBaudRate;
            string tncType;
            string tncConfigurationFile;
            string audioToneCenter;
            string radioModel;
            string civAddress;
            bool narrowFilter;
            string radioBaud;
            string radioPort;
            bool radioManual;
            bool radioSerial;
            bool viaPTCII;
            int fskLevel;
            int pskLevel;
            bool pactorIdEnabled;
            bool ttlLevel;
            bool nmea;
            string remoteCallsign;
            string centerFrequency;
            bool longPath;

            BackingObject.GetChannelInfo(
                cmbChannelName.Text, out firstUseOnly, out priority,
                out activityTimeout, out enabled, out autoForward,
                out busyHold, out tncSerialPort, out tncBaudRate,
                out tncType, out tncConfigurationFile, out audioToneCenter,
                out radioModel, out civAddress, out narrowFilter,
                out radioBaud, out radioPort, out radioManual,
                out radioSerial, out viaPTCII, out fskLevel,
                out pskLevel, out pactorIdEnabled, out ttlLevel,
                out nmea, out remoteCallsign, out centerFrequency,
                out longPath);

            chkFirstUseOnly.Checked = true;
            nudPriority.Value = priority;
            nudActivityTimeout.Value = activityTimeout;
            chkChannelEnabled.Checked = enabled;

            if (BackingObject.IsAutoforwardEnabled)
            {
                chkAutoforwardEnabled.Checked = autoForward;
            }
            else
            {
                chkAutoforwardEnabled.Checked = false;
            }

            if (tncType.StartsWith("PTC"))
            {
                chkBusyHold.Visible = true;
            }
            else
            {
                chkBusyHold.Visible = false;
                chkBusyHold.Checked = false;
            }

            cmbTNCSerialPort.Text = tncSerialPort;
            cmbTNCBaudRate.Text = tncBaudRate;
            cmbTNCType.Text = tncType;

            if (cmbTNCType.Text == "PK-232" || cmbTNCType.Text == "DSP-232")
            {
                txtAudioCenter.Enabled = false;
            }

            txtTNCConfigurationFile.Text = tncConfigurationFile;
            chkFirstUseOnly.Checked = firstUseOnly;
            txtAudioCenter.Text = audioToneCenter;
            cmbRadioModel.Text = radioModel;
            txtRadioAddress.Text = civAddress;
            chkNarrowFilter.Checked = narrowFilter;
            cmbRadioBaud.Text = radioBaud;
            cmbRadioPort.Text = radioPort;
            rdoManual.Checked = radioManual;
            rdoSerial.Checked = radioSerial;
            rdoViaPTCII.Checked = viaPTCII;
            nudFSKLevel.Value = fskLevel;
            nudPSKLevel.Value = pskLevel;
            chkIDEnabled.Checked = pactorIdEnabled;
            chkBusyHold.Checked = busyHold;
            rdoTTL.Checked = ttlLevel;
            rdoV24.Checked = !ttlLevel;
            chkNMEA.Checked = nmea;

            if (cmbRadioModel.Text.IndexOf("Icom") != -1)
            {
                chkNMEA.Enabled = true;
                lblRadioAddress.Enabled = true;
                txtRadioAddress.Enabled = true;
            }
            else
            {
                chkNMEA.Enabled = false;
                chkNMEA.Checked = false;
                lblRadioAddress.Enabled = false;
                txtRadioAddress.Enabled = false;
                txtRadioAddress.Text = "";
            }

            // Remote Call signs...
            cmbCallSigns.Text = remoteCallsign;

            // Frequencies...
            cmbFreqs.Text = centerFrequency;
            cmbTNCBaudRate.Items.Clear();
            if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text == "PTC DR-7800")
            {
                cmbTNCBaudRate.Items.Add("38400");
                cmbTNCBaudRate.Items.Add("57600");
                cmbTNCBaudRate.Items.Add("115200");
                if (Convert.ToInt32(tncBaudRate) < 38400)
                    tncBaudRate = "38400";
            }
            else
            {
                cmbTNCBaudRate.Items.Add("4800");
                cmbTNCBaudRate.Items.Add("9600");
                cmbTNCBaudRate.Items.Add("19200");
                cmbTNCBaudRate.Items.Add("38400");
                cmbTNCBaudRate.Items.Add("57600");
                cmbTNCBaudRate.Items.Add("115200");
            }

            cmbTNCBaudRate.Text = tncBaudRate;
            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
            cmbFreqs.Enabled = true;
            cmbCallSigns.Enabled = true;

            chkLongPath.Checked = longPath;
        } // SetEntries

        private void rdoSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSerial.Checked)
            {
                cmbRadioModel.Enabled = true;
                cmbRadioPort.Enabled = true;
                grpPTCLevels.Enabled = false;
                chkNarrowFilter.Enabled = true;
                txtRadioAddress.Enabled = cmbRadioModel.Text.IndexOf("Icom") != -1;
                cmbRadioBaud.Enabled = true;
                chkNarrowFilter.Enabled = true;
            }
        } // rdoSerial_CheckedChanged

        private void cmbChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChannelName.Text) && cmbChannelName.Text != "<Enter a new channel>")
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
        } // cmbChannelName_SelectedIndexChanged

        private void btnBrowseConfiguration_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = BackingObject.SiteRootDirectory + "Data";
            openFileDialog1.Filter = "aps files |*.aps";
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog1.FileName))
                {
                    txtTNCConfigurationFile.Text = openFileDialog1.FileName;
                }
            }
        } // btnBrowseConfiguration_Click

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
                cmbFreqs.Enabled = true;
                cmbCallSigns.Enabled = true;
            }
        } // cmbChannelName_TextChanged

        private void rdoManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoManual.Checked)
            {
                cmbRadioModel.Enabled = false;
                cmbRadioPort.Enabled = false;
                grpPTCLevels.Enabled = false;
                chkNarrowFilter.Enabled = false;
                txtRadioAddress.Enabled = false;
                cmbRadioBaud.Enabled = false;
                chkNarrowFilter.Enabled = true;
            }
        } // rdoManual_CheckedChanged

        private void rdoViaPTCII_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoViaPTCII.Checked)
            {
                cmbRadioModel.Enabled = true;
                grpPTCLevels.Enabled = true;
                cmbRadioPort.Enabled = false;
                chkNarrowFilter.Enabled = true;
                txtRadioAddress.Enabled = cmbRadioModel.Text.IndexOf("Icom") != -1;
                cmbRadioBaud.Enabled = true;
                chkNarrowFilter.Enabled = true;
            }
        } // rdoViaPTCII_CheckedChanged

        private void cmbRadioModel_TextChanged(object sender, EventArgs e)
        {
            if (cmbRadioModel.Text.IndexOf("Icom") != -1)
            {
                grpPTCLevels.Enabled = rdoViaPTCII.Checked & !(cmbRadioModel.Text.IndexOf("Icom") != -1);
                chkNMEA.Checked = true;
                lblRadioAddress.Enabled = true;
                txtRadioAddress.Enabled = true;
                txtRadioAddress.Text = "00";
                if (cmbRadioModel.Text.IndexOf("IC-M") != -1)
                {
                    chkNMEA.Checked = true;
                    chkNMEA.Enabled = true;
                    txtRadioAddress.Text = "00";
                    if (cmbRadioModel.Text == "Icom IC-M700Pro")
                        txtRadioAddress.Text = "02";
                    if (cmbRadioModel.Text == "Icom IC-M710")
                        txtRadioAddress.Text = "01";
                    if (cmbRadioModel.Text == "Icom IC-M710RT")
                        txtRadioAddress.Text = "03";
                    if (cmbRadioModel.Text == "Icom IC-M802")
                        txtRadioAddress.Text = "08";
                }
                else
                {
                    chkNMEA.Checked = false;
                    chkNMEA.Enabled = false;
                }
            }
            else
            {
                chkNMEA.Enabled = false;
                chkNMEA.Checked = false;
                lblRadioAddress.Enabled = false;
                txtRadioAddress.Enabled = false;
                txtRadioAddress.Text = "";
                if (cmbRadioModel.Text.IndexOf("Micom") != -1)
                {
                    rdoViaPTCII.Checked = false;
                    rdoViaPTCII.Enabled = false;
                    rdoSerial.Checked = true;
                }
                else
                {
                    rdoViaPTCII.Enabled = true;
                }
            }
        } // cmbRadioModel_TextChanged

        private void ConfirmAutoforward()
        {
            if (chkAutoforwardEnabled.Checked && !BackingObject.IsAutoforwardEnabled)
            {
                if (cmbTNCType.Text.StartsWith("PTC ") == false)
                {
                    chkAutoforwardEnabled.Checked = false;
                }
            }
        } // ConfirmAutoforward

        private void cmbTNCtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strFilename = default(string);
            chkAutoforwardEnabled.Visible = false;
            chkBusyHold.Visible = false;
            var switchExpr = cmbTNCType.Text;
            switch (switchExpr)
            {
                case "KAM98":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKAM98.aps";
                        break;
                    }

                case "KAM/+":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKAM+.aps";
                        break;
                    }

                case "KAMXL":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKAMXL.aps";
                        break;
                    }

                case "PTC II":
                case "PTC IIpro":
                    {
                        if (BackingObject.IsAutoforwardEnabled)
                            chkAutoforwardEnabled.Visible = true;
                        chkBusyHold.Visible = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePTCII_pro.aps";
                        if (cmbTNCType.Text == "PTC II")
                        {
                            rdoTTL.Checked = true;
                            rdoV24.Enabled = false;
                        }
                        else
                        {
                            rdoTTL.Enabled = true;
                            rdoV24.Enabled = true;
                        }

                        break;
                    }

                case "PTC IIe":
                case "PTC IIex":
                case "PTC IIusb":
                    {
                        if (BackingObject.IsAutoforwardEnabled)
                            chkAutoforwardEnabled.Visible = true;
                        chkBusyHold.Visible = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePTCII_e.aps";
                        if (cmbTNCType.Text == "PTC IIusb")
                        {
                            rdoTTL.Enabled = true;
                            rdoV24.Enabled = true;
                        }
                        else
                        {
                            rdoTTL.Enabled = false;
                            rdoV24.Enabled = false;
                        }

                        break;
                    }

                case "PTC DR-7800":
                    {
                        if (BackingObject.IsAutoforwardEnabled)
                            chkAutoforwardEnabled.Visible = true;
                        chkBusyHold.Visible = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePTC_DR7800.aps";
                        rdoTTL.Enabled = true;
                        rdoV24.Enabled = true;
                        break;
                    }

                case "DSP-232":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleDSP232.aps";
                        break;
                    }

                case "PK-232":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePK232.aps";
                        break;
                    }
                    // Case "PK-900"
                    // strFilename = SiteRootDirectory & "Data\ExamplePK900.aps"
            }

            // If cmbTNCType.Text = "PK-232" Or cmbTNCType.Text = "PK-900" Then
            if (cmbTNCType.Text == "PK-232")
            {
                txtAudioCenter.Text = "2210";
                txtAudioCenter.Enabled = false; // PK-232 uses only fixed tones @ 2210 center
            }
            else if (cmbTNCType.Text == "DSP-232")
            {
                txtAudioCenter.Text = "1500";
                txtAudioCenter.Enabled = false; // The DSP-232 is programmed for fixed tones @ 1500 center
            }
            else
            {
                txtAudioCenter.Text = "1500";
                txtAudioCenter.Enabled = true;
            }

            if (File.Exists(strFilename))
            {
                txtTNCConfigurationFile.Text = strFilename;
            }
            else
            {
                txtTNCConfigurationFile.Text = "";
            }
        } // cmbTNCtype_SelectedIndexChanged

        private void cmbTNCtype_TextChanged(object sender, EventArgs e)
        {
            if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text == "PTC DR-7800")
            {
                lblFSKLevel.Enabled = true;
                lblFSKLevel.Text = "PTC II FSK Level (mv):";
                lblPSKLevel.Enabled = true;
                nudFSKLevel.Enabled = true;
                nudFSKLevel.Maximum = 9000;
                nudFSKLevel.Minimum = 10;
                nudFSKLevel.Increment = 10;
                nudPSKLevel.Enabled = true;
            }
            else
            {
                lblFSKLevel.Enabled = false;
                lblPSKLevel.Enabled = false;
                nudFSKLevel.Enabled = false;
                nudPSKLevel.Enabled = false;
            }

            rdoViaPTCII.Enabled = cmbTNCType.Text == "PTC II" | cmbTNCType.Text == "PTC IIpro" | cmbTNCType.Text == "PTC IIusb" | cmbTNCType.Text == "PTC DR-7800";
            string baudRate = cmbTNCBaudRate.Text;
            cmbTNCBaudRate.Items.Clear();
            if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text == "PTC DR-7800")
            {
                cmbTNCBaudRate.Items.Add("38400");
                cmbTNCBaudRate.Items.Add("57600");
                cmbTNCBaudRate.Items.Add("115200");
                if (Convert.ToInt32(baudRate) < 38400)
                    baudRate = "38400";
            }
            else
            {
                cmbTNCBaudRate.Items.Add("4800");
                cmbTNCBaudRate.Items.Add("9600");
                cmbTNCBaudRate.Items.Add("19200");
                cmbTNCBaudRate.Items.Add("38400");
                cmbTNCBaudRate.Items.Add("57600");
                cmbTNCBaudRate.Items.Add("115200");
            }

            cmbTNCBaudRate.Text = baudRate;
        } // cmbTNCtype_TextChanged

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!BackingObject.IsValidChannelName(cmbChannelName.Text))
            {
                cmbChannelName.Focus();
                return;
            }

            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (chkAutoforwardEnabled.Checked)
                ConfirmAutoforward();
            if (BackingObject.IsAccount(cmbChannelName.Text))
            {
                MessageBox.Show(cmbChannelName.Text + " is in use as an account name...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            int audioFreq = 0;
            if (!int.TryParse(txtAudioCenter.Text.Trim(), out audioFreq))
            {
                MessageBox.Show("Audio Tones Center must be between 1000 and 2300 Hz", "Tone Center Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (audioFreq < 1000 || audioFreq > 2300)
            {
                MessageBox.Show("Audio Tones Center must be between 1000 and 2300 Hz", "Tone Center Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtRadioAddress.Enabled)
            {
                try
                {
                    byte byt = byte.Parse(txtRadioAddress.Text.ToUpper().Trim(), System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    MessageBox.Show("Radio Address must be between 00 and FF hex", "Radio Address Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (rdoSerial.Checked && (cmbRadioPort.Text ?? "") == (cmbTNCSerialPort.Text ?? ""))
            {
                MessageBox.Show("Radio Control and TNC must use different serial ports.", "Serial Port Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (BackingObject.IsChannel(cmbChannelName.Text))
            {
                MessageBox.Show("The channel name " + cmbChannelName.Text + " is already in use...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
            }
            else
            {
                BackingObject.AddChannel(
                    cmbChannelName.Text, Convert.ToInt32(nudPriority.Value), chkChannelEnabled.Checked,
                    Convert.ToInt32(nudActivityTimeout.Value), cmbTNCSerialPort.Text, cmbTNCBaudRate.Text,
                    txtTNCConfigurationFile.Text, chkFirstUseOnly.Checked, cmbTNCType.Text, chkAutoforwardEnabled.Checked,
                    txtAudioCenter.Text.Trim(), cmbRadioBaud.Text, cmbRadioPort.Text, cmbRadioModel.Text,
                    txtRadioAddress.Text.Trim().ToUpper(), chkNarrowFilter.Checked, rdoViaPTCII.Checked,
                    rdoSerial.Checked, Convert.ToInt32(nudFSKLevel.Value), Convert.ToInt32(nudPSKLevel.Value),
                    chkBusyHold.Checked, chkLongPath.Checked, chkIDEnabled.Checked, GetCallSign(),
                    cmbFreqs.Text, rdoTTL.Checked, chkNMEA.Checked);

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
                MessageBox.Show("The pactor channel " + cmbChannelName.Text + " is not found...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (MessageBox.Show("Confirm removal of pactor channel " + cmbChannelName.Text + "...", "Remove Channel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                BackingObject.RemoveChannel(cmbChannelName.Text);

                FillChannelList();
                ClearEntries();
            }
        } // btnRemove_Click

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (chkAutoforwardEnabled.Checked)
                ConfirmAutoforward();
            int audioFreq = 0;
            if (!int.TryParse(txtAudioCenter.Text.Trim(), out audioFreq))
            {
                MessageBox.Show("Audio Tones Center must be between 1000 and 2300 Hz", "Tone Center Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (audioFreq < 1000 || audioFreq > 2300)
            {
                MessageBox.Show("Audio Tones Center must be between 1000 and 2300 Hz", "Tone Center Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtRadioAddress.Enabled)
            {
                try
                {
                    byte byt = byte.Parse(txtRadioAddress.Text.ToUpper().Trim(), System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    MessageBox.Show("Radio Address must be between 00 and FF hex", "Tone Center Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (rdoSerial.Checked && (cmbRadioPort.Text ?? "") == (cmbTNCSerialPort.Text ?? ""))
            {
                MessageBox.Show("Radio Control and TNC must use different serial ports.", "Serial Port Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show("The pactor channel " + cmbChannelName.Text + " is not found...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                BackingObject.UpdateChannel(
                    cmbChannelName.Text, Convert.ToInt32(nudPriority.Value), chkChannelEnabled.Checked,
                    Convert.ToInt32(nudActivityTimeout.Value), cmbTNCSerialPort.Text, cmbTNCBaudRate.Text,
                    txtTNCConfigurationFile.Text, chkFirstUseOnly.Checked, cmbTNCType.Text, chkAutoforwardEnabled.Checked,
                    txtAudioCenter.Text.Trim(), cmbRadioBaud.Text, cmbRadioPort.Text, cmbRadioModel.Text,
                    txtRadioAddress.Text.Trim().ToUpper(), chkNarrowFilter.Checked, rdoViaPTCII.Checked,
                    rdoSerial.Checked, Convert.ToInt32(nudFSKLevel.Value), Convert.ToInt32(nudPSKLevel.Value),
                    chkBusyHold.Checked, chkLongPath.Checked, chkIDEnabled.Checked, GetCallSign(),
                    cmbFreqs.Text, rdoTTL.Checked, chkNMEA.Checked);

                FillChannelList();
            }
        } // btnUpdate_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            BackingObject.LastPactorChannel = cmbChannelName.Text;
            Close();
        } // btnClose_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs150.htm");
        } // btnHelp_Click

        private bool SetRMSList()
        {
            if (!BackingObject.HasChannelList)
            {
                MessageBox.Show("Click 'Update Channel List' to download the list of available channels");
                return false;
            }

            // Save old values for restore later.
            string remoteCallsign = cmbCallSigns.Text;
            string freq = cmbFreqs.Text;

            var aryResults = BackingObject.GetChannelList();
            int intIndex;
            string strFreqList;

            if (aryResults.Length > 0)
            {
                arySelectedMBOs = aryResults;
                cmbCallSigns.Items.Clear();
                foreach (string station in aryResults)
                {
                    intIndex = station.IndexOf(":");
                    strFreqList = station.Substring(intIndex + 1);
                    if (BackingObject.ContainsUsableFrequency(strFreqList, cmbTNCType.Text))
                    {
                        cmbCallSigns.Items.Add(station.Substring(0, intIndex));
                    }
                }

                cmbCallSigns.Text = remoteCallsign;
                cmbFreqs.Text = freq;
            }
            else
            {
                cmbCallSigns.Items.Clear();
                cmbCallSigns.Text = remoteCallsign;
                cmbFreqs.Items.Clear();
                cmbFreqs.Text = freq;
            }

            return true;
        } // SetRMSList

        private void cmbCallSigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strFreqEntry;
            string[] strStation;

            for (int i = 0, loopTo = arySelectedMBOs.Length - 1; i <= loopTo; i++)
            {
                strStation = arySelectedMBOs[i].Split(':');
                // Look for a channel line matching the selected station.
                if ((strStation[0] ?? "") == (GetCallSign() ?? ""))
                {
                    // We found a matching channel.  Set up its frequencies.
                    var aryFreqs = strStation[1].Split(',');
                    if (aryFreqs.Length >= 1)
                    {
                        cmbFreqs.Items.Clear();
                        cmbFreqs.Text = "";
                        for (int j = 0, loopTo1 = aryFreqs.Length - 1; j <= loopTo1; j++)
                        {
                            strFreqEntry = aryFreqs[j];
                            if (BackingObject.CanUseFrequency(strFreqEntry, cmbTNCType.Text))
                            {
                                cmbFreqs.Items.Add(BackingObject.FormatFrequency(strFreqEntry));
                            }
                        }

                        cmbFreqs.Text = BackingObject.FormatFrequency(aryFreqs[0]);
                    }

                    return;
                }
            }
        } // cmbCallSigns_SelectedIndexChanged

        private void chkAutoforwardEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoforwardEnabled.Checked)
            {
                chkFirstUseOnly.Enabled = true;
            }
            else
            {
                // chkFirstUseOnly.Enabled = False
                chkFirstUseOnly.Checked = true;
            }
        }

        private void btnUpdateChannelList_Click(object sender, EventArgs e)
        {
            // 
            // Update the list of channels.
            // 
            string strError;
            btnUpdateChannelList.Enabled = false;
            try
            {
                Cursor = Cursors.WaitCursor;
                strError = BackingObject.DownloadChannelList();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            btnUpdateChannelList.Enabled = true;
            if (string.IsNullOrEmpty(strError))
            {
                SetRMSList();
                MessageBox.Show("The channel update completed successfully");
            }
            else
            {
                MessageBox.Show(strError);
            }
        }

        private string GetCallSign()
        {
            // 
            // Get the call sign from the call sign field and clean it up.
            // 
            return cmbCallSigns.Text.Trim().ToUpper();
        }

        public void RefreshWindow()
        {
            Refresh();
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}