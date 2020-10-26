using System;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogPactorTNCChannels
    {
        public DialogPactorTNCChannels()
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
        }

        private TChannelProperties stcSelectedChannel;
        private string[] arySelectedMBOs;

        private void PactorTNCChannels_Load(object sender, EventArgs e)
        {
            InitializeControls();
            ClearEntries();
            FillChannelList();
            cmbChannelName.Text = Globals.Settings.Get("Properties", "Last Pactor Channel", "");
        } // PactorTNCChannels_Load

        private void InitializeControls()
        {
            SetRMSList();
            cmbTNCType.Items.Add("KAM98");
            cmbTNCType.Items.Add("KAM/+");
            cmbTNCType.Items.Add("KAMXL");
            cmbTNCType.Items.Add("DSP-232");
            cmbTNCType.Items.Add("PK-232");
            cmbTNCType.Items.Add("PTC II");
            cmbTNCType.Items.Add("PTC IIe");
            cmbTNCType.Items.Add("PTC IIex");
            cmbTNCType.Items.Add("PTC IIpro");
            cmbTNCType.Items.Add("PTC IIusb");
            cmbTNCType.Items.Add("PTC DR-7800");
            var strPorts = SerialPort.GetPortNames();
            if (strPorts.Length > 0)
            {
                foreach (string strPort in strPorts)
                    cmbTNCSerialPort.Items.Add(Globals.CleanSerialPort(strPort));
            }

            cmbTNCBaudRate.Items.Add("4800");
            cmbTNCBaudRate.Items.Add("9600");
            cmbTNCBaudRate.Items.Add("19200");
            cmbTNCBaudRate.Items.Add("38400");
            cmbTNCBaudRate.Items.Add("57600");
            cmbTNCBaudRate.Items.Add("115200");

            // Radio control baud rate
            cmbRadioBaud.Items.Add("1200");
            cmbRadioBaud.Items.Add("2400");
            cmbRadioBaud.Items.Add("4800");
            cmbRadioBaud.Items.Add("9600");
            cmbRadioBaud.Items.Add("19200");
            cmbRadioBaud.Items.Add("38400");
            cmbRadioBaud.Items.Add("57600");
            cmbRadioBaud.Items.Add("115200");
            cmbRadioBaud.Text = stcSelectedChannel.RDOControlBaud;

            // Serial Ports
            var strRadioPorts = SerialPort.GetPortNames();
            if (strRadioPorts.Length > 0)
            {
                foreach (string strRadioPort in strPorts)
                    cmbRadioPort.Items.Add(Globals.CleanSerialPort(strRadioPort));
            }

            cmbRadioPort.Text = stcSelectedChannel.RDOControlPort;

            // Radio Type
            cmbRadioModel.Items.Add("Kenwood TS-450");
            cmbRadioModel.Items.Add("Kenwood TS-480");
            cmbRadioModel.Items.Add("Kenwood TS-690");
            cmbRadioModel.Items.Add("Kenwood TS-2000");
            cmbRadioModel.Items.Add("Kenwood (other)");
            cmbRadioModel.Items.Add("Icom IC-706");
            cmbRadioModel.Items.Add("Icom IC-7000");
            cmbRadioModel.Items.Add("Icom IC-7200");
            cmbRadioModel.Items.Add("Icom IC-746");
            cmbRadioModel.Items.Add("Icom IC-746pro");
            cmbRadioModel.Items.Add("Icom IC-756pro");
            cmbRadioModel.Items.Add("Icom (other CI-V)");
            cmbRadioModel.Items.Add("Icom IC-M700Pro");
            cmbRadioModel.Items.Add("Icom IC-M710");
            cmbRadioModel.Items.Add("Icom IC-M710RT");
            cmbRadioModel.Items.Add("Icom IC-M802");
            cmbRadioModel.Items.Add("Icom (other NMEA)");
            cmbRadioModel.Items.Add("Micom 3F");
            cmbRadioModel.Items.Add("Yaesu FT-450");
            cmbRadioModel.Items.Add("Yaesu FT-847");
            cmbRadioModel.Items.Add("Yaesu FT-857");
            cmbRadioModel.Items.Add("Yaesu FT-857D");
            cmbRadioModel.Items.Add("Yaesu FT-897");
            cmbRadioModel.Items.Add("Yaesu FT-920");
            cmbRadioModel.Items.Add("Yaesu FT-950");
            cmbRadioModel.Items.Add("Yaesu FT-1000");
            cmbRadioModel.Items.Add("Yaesu FT-2000");
            cmbRadioModel.Items.Add("Yaesu (other)");
            cmbRadioModel.Text = stcSelectedChannel.RDOModel;
            txtRadioAddress.Text = stcSelectedChannel.CIVAddress;
            chkNarrowFilter.Checked = stcSelectedChannel.NarrowFilter;
            rdoManual.Checked = stcSelectedChannel.RDOControl == "Manual";
            rdoSerial.Checked = stcSelectedChannel.RDOControl == "Serial";
            rdoViaPTCII.Checked = stcSelectedChannel.RDOControl == "Via PTCII";
            rdoTTL.Checked = stcSelectedChannel.TTLLevel;
            rdoV24.Checked = !stcSelectedChannel.TTLLevel;
            chkNMEA.Checked = stcSelectedChannel.NMEA;
            chkIDEnabled.Checked = stcSelectedChannel.PactorId;
            chkBusyHold.Checked = stcSelectedChannel.TNCBusyHold;
            chkChannelEnabled.Checked = stcSelectedChannel.Enabled;
            if (Globals.IsAutoforwardEnabled())
            {
                chkAutoforwardEnabled.Visible = true;
                chkAutoforwardEnabled.Checked = stcSelectedChannel.EnableAutoforward;
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
            foreach (TChannelProperties stcChannel in Channels.Entries.Values)
            {
                if (stcChannel.ChannelType == ChannelMode.PactorTNC)
                {
                    if (!string.IsNullOrEmpty(stcChannel.ChannelName.Trim()))
                        cmbChannelName.Items.Add(stcChannel.ChannelName);
                }
            }
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
            stcSelectedChannel = (TChannelProperties)Channels.Entries[cmbChannelName.Text];
            // chkFirstUseOnly.Enabled = False
            chkFirstUseOnly.Checked = true;
            {
                var withBlock = stcSelectedChannel;
                nudPriority.Value = withBlock.Priority;
                nudActivityTimeout.Value = withBlock.TNCTimeout;
                chkChannelEnabled.Checked = withBlock.Enabled;
                if (Globals.IsAutoforwardEnabled())
                {
                    chkAutoforwardEnabled.Checked = withBlock.EnableAutoforward;
                }
                else
                {
                    chkAutoforwardEnabled.Checked = false;
                }

                if (stcSelectedChannel.TNCType.StartsWith("PTC"))
                {
                    chkBusyHold.Visible = true;
                }
                else
                {
                    chkBusyHold.Visible = false;
                    chkBusyHold.Checked = false;
                }

                cmbTNCSerialPort.Text = withBlock.TNCSerialPort;
                cmbTNCBaudRate.Text = withBlock.TNCBaudRate;
                cmbTNCType.Text = withBlock.TNCType;

                // If (cmbTNCType.Text = "PK-232") Or (cmbTNCType.Text = "PK-900") Then txtAudioCenter.Enabled = False
                if (cmbTNCType.Text == "PK-232" | cmbTNCType.Text == "DSP-232")
                    txtAudioCenter.Enabled = false;
                txtTNCConfigurationFile.Text = withBlock.TNCConfigurationFile;
                chkFirstUseOnly.Checked = withBlock.TNCConfigureOnFirstUseOnly;
                txtAudioCenter.Text = withBlock.AudioToneCenter;
                cmbRadioModel.Text = withBlock.RDOModel;
                txtRadioAddress.Text = withBlock.CIVAddress;
                chkNarrowFilter.Checked = withBlock.NarrowFilter;
                cmbRadioBaud.Text = withBlock.RDOControlBaud;
                cmbRadioPort.Text = withBlock.RDOControlPort;
                rdoManual.Checked = withBlock.RDOControl == "Manual";
                rdoSerial.Checked = withBlock.RDOControl == "Serial";
                rdoViaPTCII.Checked = withBlock.RDOControl == "Via PTCII";
                nudFSKLevel.Value = withBlock.TNCFSKLevel;
                nudPSKLevel.Value = withBlock.TNCPSKLevel;
                chkIDEnabled.Checked = withBlock.PactorId;
                chkBusyHold.Checked = withBlock.TNCBusyHold;
                rdoTTL.Checked = withBlock.TTLLevel;
                rdoV24.Checked = !withBlock.TTLLevel;
                chkNMEA.Checked = withBlock.NMEA;
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
                cmbCallSigns.Text = withBlock.RemoteCallsign;

                // Frequencies...
                cmbFreqs.Text = withBlock.RDOCenterFrequency;
                cmbTNCBaudRate.Items.Clear();
                if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text == "PTC DR-7800")
                {
                    cmbTNCBaudRate.Items.Add("38400");
                    cmbTNCBaudRate.Items.Add("57600");
                    cmbTNCBaudRate.Items.Add("115200");
                    if (Convert.ToInt32(withBlock.TNCBaudRate) < 38400)
                        withBlock.TNCBaudRate = "38400";
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

                cmbTNCBaudRate.Text = withBlock.TNCBaudRate;
                btnAdd.Enabled = false;
                btnRemove.Enabled = true;
                btnUpdate.Enabled = true;
                cmbFreqs.Enabled = true;
                cmbCallSigns.Enabled = true;
            }
        } // SetEntries

        private void UpdateChannelProperties(ref TChannelProperties stcChannel)
        {
            stcChannel.ChannelType = ChannelMode.PactorTNC; // TODO: Needs error checking for some parameters
            stcChannel.ChannelName = cmbChannelName.Text;
            stcChannel.Priority = Convert.ToInt32(nudPriority.Value);
            stcChannel.Enabled = chkChannelEnabled.Checked;
            stcChannel.TNCTimeout = Convert.ToInt32(nudActivityTimeout.Value);
            stcChannel.TNCSerialPort = cmbTNCSerialPort.Text;
            stcChannel.TNCBaudRate = cmbTNCBaudRate.Text;
            stcChannel.TNCConfigurationFile = txtTNCConfigurationFile.Text;
            stcChannel.TNCConfigureOnFirstUseOnly = chkFirstUseOnly.Checked;
            stcChannel.TNCType = cmbTNCType.Text;
            var switchExpr = cmbTNCType.Text;
            switch (switchExpr)
            {
                case "KAM/+":
                case "KAMXL":
                    {
                        stcChannel.TNCPort = 2;
                        break;
                    }

                case "KAM98":
                case "PK-232": // , "PK-900"
                    {
                        stcChannel.TNCPort = 1;  // TODO: needs verification, not sure of correct port for XL  and 98
                        break;
                    }

                case "PTC II":
                case "PTC IIe":
                case "PTC IIex":
                case "PTC IIpro":
                case "PTC IIusb":
                case "PTC DR-7800":
                    {
                        stcChannel.TNCPort = 31; // port 31 for SCS Pactor 
                        break;
                    }
            }

            stcChannel.EnableAutoforward = chkAutoforwardEnabled.Checked;
            stcChannel.AudioToneCenter = txtAudioCenter.Text.Trim();
            stcChannel.RDOControlBaud = cmbRadioBaud.Text;
            stcChannel.RDOControlPort = cmbRadioPort.Text;
            stcChannel.RDOModel = cmbRadioModel.Text;
            stcChannel.CIVAddress = txtRadioAddress.Text.Trim().ToUpper();
            stcChannel.NarrowFilter = chkNarrowFilter.Checked;
            if (rdoViaPTCII.Checked)
            {
                stcChannel.RDOControl = "Via PTCII";
            }
            else if (rdoSerial.Checked)
            {
                stcChannel.RDOControl = "Serial";
            }
            else
            {
                stcChannel.RDOControl = "Manual";
            }

            stcChannel.TNCFSKLevel = Convert.ToInt32(nudFSKLevel.Value);
            stcChannel.TNCPSKLevel = Convert.ToInt32(nudPSKLevel.Value);
            stcChannel.TNCBusyHold = chkBusyHold.Checked;
            stcChannel.PactorId = chkIDEnabled.Checked;
            stcChannel.RemoteCallsign = GetCallSign();
            // .FrequenciesScanned = cmbFreqs.Items.Count
            stcChannel.RDOCenterFrequency = cmbFreqs.Text;
            stcChannel.TTLLevel = rdoTTL.Checked;
            stcChannel.NMEA = chkNMEA.Checked;
        } // UpdateProperties

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

        private void btnBrowseConfiguration_Click(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Globals.SiteRootDirectory + "Data";
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
            if (chkAutoforwardEnabled.Checked & !Globals.IsAutoforwardEnabled())
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
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKAM98.aps";
                        break;
                    }

                case "KAM/+":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKAM+.aps";
                        break;
                    }

                case "KAMXL":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKAMXL.aps";
                        break;
                    }

                case "PTC II":
                case "PTC IIpro":
                    {
                        if (Globals.IsAutoforwardEnabled())
                            chkAutoforwardEnabled.Visible = true;
                        chkBusyHold.Visible = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePTCII_pro.aps";
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
                        if (Globals.IsAutoforwardEnabled())
                            chkAutoforwardEnabled.Visible = true;
                        chkBusyHold.Visible = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePTCII_e.aps";
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
                        if (Globals.IsAutoforwardEnabled())
                            chkAutoforwardEnabled.Visible = true;
                        chkBusyHold.Visible = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePTC_DR7800.aps";
                        rdoTTL.Enabled = true;
                        rdoV24.Enabled = true;
                        break;
                    }

                case "DSP-232":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleDSP232.aps";
                        break;
                    }

                case "PK-232":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePK232.aps";
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
                nudFSKLevel.Maximum = 1000;
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
            cmbTNCBaudRate.Items.Clear();
            if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text == "PTC DR-7800")
            {
                cmbTNCBaudRate.Items.Add("38400");
                cmbTNCBaudRate.Items.Add("57600");
                cmbTNCBaudRate.Items.Add("115200");
                if (Convert.ToInt32(stcSelectedChannel.TNCBaudRate) < 38400)
                    stcSelectedChannel.TNCBaudRate = "38400";
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

            cmbTNCBaudRate.Text = stcSelectedChannel.TNCBaudRate;
        } // cmbTNCtype_TextChanged

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Globals.IsValidFileName(cmbChannelName.Text))
            {
                cmbChannelName.Focus();
                return;
            }

            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (chkAutoforwardEnabled.Checked)
                ConfirmAutoforward();
            if (Channels.IsAccount(cmbChannelName.Text))
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

            if (rdoSerial.Checked & (cmbRadioPort.Text ?? "") == (cmbTNCSerialPort.Text ?? ""))
            {
                MessageBox.Show("Radio Control and TNC must use different serial ports.", "Serial Port Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Channels.IsChannel(cmbChannelName.Text))
            {
                MessageBox.Show("The channel name " + cmbChannelName.Text + " is already in use...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
            }
            else
            {
                var stcNewChannel = default(TChannelProperties);
                UpdateChannelProperties(ref stcNewChannel);
                Channels.AddChannel(ref stcNewChannel);
                Channels.FillChannelCollection();
                FillChannelList();
                Globals.Settings.Save("Properties", "Last Pactor Channel", cmbChannelName.Text);
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
                Channels.RemoveChannel(cmbChannelName.Text);
                Channels.FillChannelCollection();
                FillChannelList();
                Globals.Settings.Save("Properties", "Last Pactor Channel", "");
                if (Globals.cllFastStart.Contains(cmbChannelName.Text))
                {
                    Globals.cllFastStart.Remove(cmbChannelName.Text);
                }

                ClearEntries();
                // Me.Close()
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

            if (rdoSerial.Checked & (cmbRadioPort.Text ?? "") == (cmbTNCSerialPort.Text ?? ""))
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
                var stcUpdateChannel = default(TChannelProperties);
                UpdateChannelProperties(ref stcUpdateChannel);
                Channels.UpdateChannel(ref stcUpdateChannel);
                Channels.FillChannelCollection();
                FillChannelList();

                // Clear channel name from the fast start list...
                if (Globals.cllFastStart.Contains(cmbChannelName.Text))
                    Globals.cllFastStart.Remove(cmbChannelName.Text);
                Globals.Settings.Save("Properties", "Last Pactor Channel", cmbChannelName.Text);
                // Me.Close()
            }
        } // btnUpdate_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            Globals.Settings.Save("Properties", "Last Pactor Channel", cmbChannelName.Text);
            Close();
        } // btnClose_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs150.htm");
        } // btnHelp_Click

        private bool SetRMSList()
        {
            var aryResults = new string[0];
            int intIndex;
            string strFreqList;
            if (File.Exists(Globals.SiteRootDirectory + @"\Data\RMS Channels.dat"))
            {
                aryResults = Channels.ParseChannelList(Globals.SiteRootDirectory + @"\Data\RMS Channels.dat");
            }
            else
            {
                MessageBox.Show("Click 'Update Channel List' to download the list of available channels");
                return false;
            }

            if (aryResults.Length > 0)
            {
                arySelectedMBOs = aryResults;
                cmbCallSigns.Items.Clear();
                foreach (string station in aryResults)
                {
                    intIndex = station.IndexOf(":");
                    strFreqList = station.Substring(intIndex + 1);
                    if (Globals.AnyUseableFrequency(strFreqList, cmbTNCType.Text))
                    {
                        cmbCallSigns.Items.Add(station.Substring(0, intIndex));
                    }
                }

                cmbCallSigns.Text = stcSelectedChannel.RemoteCallsign;
                cmbFreqs.Text = stcSelectedChannel.RDOCenterFrequency;
            }
            else
            {
                cmbCallSigns.Items.Clear();
                cmbCallSigns.Text = stcSelectedChannel.RemoteCallsign;
                cmbFreqs.Items.Clear();
                cmbFreqs.Text = stcSelectedChannel.RDOCenterFrequency;
            }

            return true;
        } // SetRMSList

        public string[] ParsePMBOList(string strFilename)
        {
            // 
            // Function to parse the PMBO freq list (used for Public, EMComm and MARS
            // Returns an empty string array if error or file not found...
            // 
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
                foreach (string line in strLines)
                {
                    if (blnSeparatorFound)
                    {
                        if (string.IsNullOrEmpty(line.Trim()) & !string.IsNullOrEmpty(strCallsign)) // blank line at end of station record
                        {
                            if (!string.IsNullOrEmpty(strFrequencies)) // only put in list if freq available
                            {
                                Array.Resize(ref aryResult, aryResult.Length + 1); // make one larger
                                string strTemp = strCallsign + "," + strFrequencies;
                                if (strTemp.EndsWith(","))
                                    strTemp = strTemp.Substring(0, strTemp.Length - 1);
                                aryResult[aryResult.Length - 1] = strTemp;
                            }

                            blnNewCallsign = false;
                            strCallsign = "";
                        }
                        else if (blnNewCallsign & line.Trim().Length > 2)
                        {
                            if ("P E H T A S ".IndexOf(line.Trim().Substring(0, 2)) == -1)
                            {
                                var strTemp1 = line.Split(' ');
                                foreach (string token in strTemp1)
                                {
                                    float num = 0.0F;
                                    if (float.TryParse(token.Replace("#", ""), out num))
                                    {
                                        int intFrequency = Globals.KHzToHz(token.Replace("#", ""));
                                        if (intFrequency > 1800000 & intFrequency < 54000000)
                                        {
                                            strFrequencies += token.Trim() + ",";
                                        }
                                    }
                                }
                            }
                        }
                        else if (!blnNewCallsign & !string.IsNullOrEmpty(line.Trim()))
                        {
                            var strFirstLineTokens = line.Split(',');
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
                        blnSeparatorFound = line.IndexOf("------------------------") != -1;
                    }
                }
            }
            catch
            {
                return aryResult;
            }

            return aryResult;
        } // ParsePMBOList

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
                            if (Globals.CanUseFrequency(strFreqEntry, cmbTNCType.Text))
                            {
                                cmbFreqs.Items.Add(Globals.FormatFrequency(strFreqEntry));
                            }
                        }

                        cmbFreqs.Text = Globals.FormatFrequency(aryFreqs[0]);
                    }

                    return;
                }
            }

            stcSelectedChannel.RemoteCallsign = cmbCallSigns.Text;
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
                strError = Channels.GetChannelRecords(false, Globals.strServiceCodes);
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
    }
}