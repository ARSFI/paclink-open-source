using Paclink.UI.Common;
using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogPacketTNCChannels
    {
        public DialogPacketTNCChannels()
        {
            InitializeComponent();
            _Label8.Name = "Label8";
            _Label7.Name = "Label7";
            _txtScript.Name = "txtScript";
            _Label6.Name = "Label6";
            _Label4.Name = "Label4";
            _chkEnabled.Name = "chkEnabled";
            _cmbChannelName.Name = "cmbChannelName";
            _Label3.Name = "Label3";
            _Label2.Name = "Label2";
            _Label1.Name = "Label1";
            _btnClose.Name = "btnClose";
            _btnUpdate.Name = "btnUpdate";
            _btnRemove.Name = "btnRemove";
            _btnAdd.Name = "btnAdd";
            _Label14.Name = "Label14";
            _cmbTNCType.Name = "cmbTNCType";
            _Label22.Name = "Label22";
            _cmbTNCSerialPort.Name = "cmbTNCSerialPort";
            _btnBrowseConfiguration.Name = "btnBrowseConfiguration";
            _txtTNCConfigurationFile.Name = "txtTNCConfigurationFile";
            _Label18.Name = "Label18";
            _cmbTNCBaudRate.Name = "cmbTNCBaudRate";
            _Label21.Name = "Label21";
            _nudActivityTimeout.Name = "nudActivityTimeout";
            _nudScriptTimeout.Name = "nudScriptTimeout";
            _nudPriority.Name = "nudPriority";
            _chkFirstUseOnly.Name = "chkFirstUseOnly";
            _rdoV24.Name = "rdoV24";
            _rdoTTL.Name = "rdoTTL";
            _rdoViaPTCII.Name = "rdoViaPTCII";
            _txtRadioAdd.Name = "txtRadioAdd";
            _cmbRadioModel.Name = "cmbRadioModel";
            _rdoSerial.Name = "rdoSerial";
            _rdoManual.Name = "rdoManual";
            _cmbRadioPort.Name = "cmbRadioPort";
            _cmbRadioBaud.Name = "cmbRadioBaud";
            _txtFreqMHz.Name = "txtFreqMHz";
            _cmbOnAirBaud.Name = "cmbOnAirBaud";
            _nudTNCPort.Name = "nudTNCPort";
            _lblTNCPort.Name = "lblTNCPort";
            _btnHelp.Name = "btnHelp";
            _Label9.Name = "Label9";
            _grpRadioControl.Name = "grpRadioControl";
            _Label11.Name = "Label11";
            _grpPTCLevels.Name = "grpPTCLevels";
            _Label15.Name = "Label15";
            _lblRadioAddress.Name = "lblRadioAddress";
            _Label10.Name = "Label10";
            _Label16.Name = "Label16";
            _Label12.Name = "Label12";
            _btnUpdateChannelList.Name = "btnUpdateChannelList";
            _cmbRemoteCallsign.Name = "cmbRemoteCallsign";
            _lblFrequency.Name = "lblFrequency";
            _cmbFreqs.Name = "cmbFreqs";
        }

        private ChannelProperties stcSelectedChannel;
        private string[] arySelectedMBOs;

        private void PacketTNCChannels_Load(object sender, EventArgs e)
        {
            InitializeControls();
            ClearEntries();
            FillChannelList();
            cmbChannelName.Text = Globals.Settings.Get("Properties", "Last Packet TNC Channel", "");
            if (!string.IsNullOrEmpty(cmbChannelName.Text) & cmbChannelName.Text != "<Enter a new channel>")
            {
                if (Channels.Entries.ContainsKey(cmbChannelName.Text))
                {
                    SetEntries();
                    SetRMSList();
                }
            }
        } // PacketTNCChannels_Load

        private void InitializeControls()
        {
            SetRMSList();
            cmbTNCType.Items.Add("KPC3");
            cmbTNCType.Items.Add("KPC3+");
            cmbTNCType.Items.Add("KPC4");
            cmbTNCType.Items.Add("KPC9612");
            cmbTNCType.Items.Add("KPC9612+");
            cmbTNCType.Items.Add("KAM/+");
            cmbTNCType.Items.Add("KAMXL");
            cmbTNCType.Items.Add("KAM98");
            cmbTNCType.Items.Add("PK-88");
            cmbTNCType.Items.Add("PK-96");
            cmbTNCType.Items.Add("PK-232");
            cmbTNCType.Items.Add("PK-900");
            cmbTNCType.Items.Add("PTC II");
            cmbTNCType.Items.Add("PTC IIe");
            cmbTNCType.Items.Add("PTC IIex");
            cmbTNCType.Items.Add("PTC IIpro");
            cmbTNCType.Items.Add("PTC IIusb");
            cmbTNCType.Items.Add("PTC DR-7800");
            cmbTNCType.Items.Add("TS-2000 int");
            cmbTNCType.Items.Add("TH-D7 int");
            cmbTNCType.Items.Add("TM-D700 int");
            cmbTNCType.Items.Add("TM-D710 int");
            cmbTNCType.Items.Add("TM-D72");
            cmbTNCType.Items.Add("ALINCO int");
            cmbTNCType.Items.Add("Generic KISS");
            var strPorts = SerialPort.GetPortNames();
            if (strPorts.Length > 0)
            {
                foreach (string strPort in strPorts)
                {
                    cmbTNCSerialPort.Items.Add(Globals.CleanSerialPort(strPort));
                    cmbRadioPort.Items.Add(Globals.CleanSerialPort(strPort));
                }
            }

            cmbRadioBaud.Items.Add("4800");
            cmbRadioBaud.Items.Add("9600");
            cmbRadioBaud.Items.Add("19200");
            cmbRadioBaud.Items.Add("38400");
            cmbRadioBaud.Items.Add("58600");
            cmbTNCBaudRate.Items.Add("4800");
            cmbTNCBaudRate.Items.Add("9600");
            cmbTNCBaudRate.Items.Add("19200");
            cmbTNCBaudRate.Items.Add("38400");
            cmbTNCBaudRate.Items.Add("57600");
            cmbTNCBaudRate.Items.Add("115200");
            cmbTNCBaudRate.Text = "9600";
            // Radio Type
            cmbRadioModel.Items.Add("Kenwood TS-2000");
            cmbRadioModel.Items.Add("Kenwood TM-D700");
            cmbRadioModel.Items.Add("Kenwood TM-D710");
            cmbRadioModel.Items.Add("Kenwood TH-D7");
            cmbRadioModel.Items.Add("Icom IC-706");
            cmbRadioModel.Items.Add("Icom IC-7000");
            cmbRadioModel.Items.Add("Icom IC-746");
            cmbRadioModel.Items.Add("Yaesu FT-847");
            cmbRadioModel.Items.Add("Yaesu FT-857");
            cmbRadioModel.Items.Add("Yaesu FT-857D");
            cmbRadioModel.Items.Add("Yaesu FT-897");
            cmbOnAirBaud.Items.Add("1200");
            cmbOnAirBaud.Items.Add("9600");
        } // InitializeControls

        private bool SetRMSList()
        {
            if (!Channels.HasChannelList(true))
            {
                MessageBox.Show("Click 'Update Channel List' to download the list of available channels");
                return false;
            }

            var aryResults = Channels.ParseChannelList(true);
            int intIndex;
            string strFreqList;
            string strStationCall;
            bool blnFoundCallsign;

            if (aryResults.Length > 0)
            {
                arySelectedMBOs = aryResults;
                cmbRemoteCallsign.Items.Clear();
                blnFoundCallsign = false;
                foreach (string station in aryResults)
                {
                    intIndex = station.IndexOf(":");
                    strFreqList = station.Substring(intIndex + 1);
                    if (Globals.AnyUseableFrequency(strFreqList, cmbTNCType.Text) & Globals.CanUseBaud(strFreqList, cmbOnAirBaud.Text))
                    {
                        var strItems = strFreqList.Split('|');
                        // cmbRemoteCallsign.Items.Add(station.Substring(0, intIndex) & " (" & FormatBaud(strItems(2)) & ")")
                        cmbRemoteCallsign.Items.Add(station.Substring(0, intIndex));
                        strStationCall = station.Substring(0, intIndex);
                        if ((strStationCall ?? "") == (stcSelectedChannel.RemoteCallsign ?? ""))
                            blnFoundCallsign = true;
                    }
                }

                if (blnFoundCallsign)
                {
                    cmbRemoteCallsign.Text = stcSelectedChannel.RemoteCallsign;
                    cmbFreqs.Text = stcSelectedChannel.RDOCenterFrequency;
                }
                else
                {
                    cmbRemoteCallsign.Text = "";
                    cmbFreqs.Text = "";
                    stcSelectedChannel.RemoteCallsign = "";
                    cmbFreqs.Text = stcSelectedChannel.RDOCenterFrequency;
                }
            }
            else
            {
                cmbRemoteCallsign.Items.Clear();
                cmbRemoteCallsign.Text = stcSelectedChannel.RemoteCallsign;
                cmbFreqs.Items.Clear();
                cmbFreqs.Text = stcSelectedChannel.RDOCenterFrequency;
            }

            return true;
        } // SetRMSList

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            foreach (ChannelProperties stcChannel in Channels.Entries.Values)
            {
                if (stcChannel.ChannelType == ChannelMode.PacketTNC)
                {
                    if (!string.IsNullOrEmpty(stcChannel.ChannelName.Trim()))
                        cmbChannelName.Items.Add(stcChannel.ChannelName);
                }
            }
        } // FillChannelList

        private void ClearEntries()
        {
            cmbChannelName.Text = "";
            cmbFreqs.Text = "";
            cmbFreqs.Enabled = false;
            nudPriority.Value = 3;
            cmbRemoteCallsign.Text = "";
            nudActivityTimeout.Value = 10;
            nudScriptTimeout.Value = 60;
            txtScript.Clear();
            chkEnabled.Checked = true;
            btnAdd.Enabled = false;
            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
            if (cmbTNCSerialPort.Items.Count != 0)
            {
                cmbTNCSerialPort.Text = cmbTNCSerialPort.Items[0].ToString();
            }

            cmbTNCBaudRate.Text = "";
            txtTNCConfigurationFile.Text = "";
        } // ClearEntries

        private void SetEntries()
        {
            stcSelectedChannel = (ChannelProperties)Channels.Entries[cmbChannelName.Text];
            {
                var withBlock = stcSelectedChannel;
                string strPhsFreq = withBlock.RDOCenterFrequency;
                cmbFreqs.Text = withBlock.RDOCenterFrequency;
                nudPriority.Value = withBlock.Priority;
                // cmbRemoteCallsign.Text = .RemoteCallsign
                nudActivityTimeout.Value = Math.Max(10, withBlock.TNCTimeout);
                nudScriptTimeout.Value = withBlock.TNCScriptTimeout;
                txtScript.Text = withBlock.TNCScript;
                chkEnabled.Checked = withBlock.Enabled;
                cmbTNCSerialPort.Text = withBlock.TNCSerialPort;
                cmbTNCType.Text = withBlock.TNCType;
                nudTNCPort.Value = withBlock.TNCPort;
                txtTNCConfigurationFile.Text = withBlock.TNCConfigurationFile;
                chkFirstUseOnly.Checked = withBlock.TNCConfigureOnFirstUseOnly;

                // Radio Type
                cmbRadioBaud.Text = stcSelectedChannel.RDOControlBaud;
                cmbRadioPort.Text = stcSelectedChannel.RDOControlPort;
                cmbRadioModel.Text = stcSelectedChannel.RDOModel;
                cmbOnAirBaud.Text = stcSelectedChannel.TNCOnAirBaud.ToString();
                txtRadioAdd.Text = stcSelectedChannel.CIVAddress;
                rdoSerial.Checked = stcSelectedChannel.RDOControl == "Serial";
                rdoViaPTCII.Checked = stcSelectedChannel.RDOControl == "Via PTCII";
                rdoManual.Checked = stcSelectedChannel.RDOControl == "Manual";
                rdoTTL.Checked = stcSelectedChannel.TTLLevel;
                rdoV24.Checked = !stcSelectedChannel.TTLLevel;
                try
                {
                    txtFreqMHz.Text = (0.001 * double.Parse(stcSelectedChannel.RDOCenterFrequency, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00.000");
                }
                catch
                {
                }

                cmbTNCBaudRate.Items.Clear();
                if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text.IndexOf("DR-7800") != -1)
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
            }

            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
            cmbFreqs.Enabled = true;
        } // SetEntries

        private void UpdateChannelProperties(ref ChannelProperties stcChannel)
        {
            stcChannel.ChannelType = ChannelMode.PacketTNC;
            stcChannel.ChannelName = cmbChannelName.Text;
            stcChannel.Priority = Convert.ToInt32(nudPriority.Value);
            stcChannel.RemoteCallsign = CleanupCallSign(cmbRemoteCallsign.Text);
            stcChannel.RDOCenterFrequency = cmbFreqs.Text;
            stcChannel.Enabled = chkEnabled.Checked;
            stcChannel.TNCTimeout = Convert.ToInt32(nudActivityTimeout.Value);
            stcChannel.TNCScript = txtScript.Text;
            stcChannel.TNCScriptTimeout = Convert.ToInt32(nudScriptTimeout.Value);
            stcChannel.TNCSerialPort = cmbTNCSerialPort.Text;
            stcChannel.TNCBaudRate = cmbTNCBaudRate.Text;
            stcChannel.TNCConfigurationFile = txtTNCConfigurationFile.Text;
            stcChannel.TNCConfigureOnFirstUseOnly = chkFirstUseOnly.Checked;
            stcChannel.TNCPort = Convert.ToInt32(nudTNCPort.Value);
            stcChannel.TNCType = cmbTNCType.Text;
            stcChannel.TNCOnAirBaud = Convert.ToInt32(cmbOnAirBaud.Text);
            stcChannel.EnableAutoforward = true; // Packet Channels always enabled
            if (!rdoManual.Checked)
            {
                stcChannel.RDOCenterFrequency = (1000 * double.Parse(txtFreqMHz.Text, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00000.0");
            }

            stcChannel.RDOControlBaud = cmbRadioBaud.Text;
            stcChannel.RDOControlPort = cmbRadioPort.Text;
            stcChannel.RDOModel = cmbRadioModel.Text;
            stcChannel.CIVAddress = txtRadioAdd.Text.Trim().ToUpper();
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

            stcChannel.TTLLevel = rdoTTL.Checked;
        } // UpdateProperties

        private void cmbChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChannelName.Text) & cmbChannelName.Text != "<Enter a new channel>")
            {
                if (Channels.Entries.ContainsKey(cmbChannelName.Text))
                {
                    SetEntries();
                    SetRMSList();
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
                cmbFreqs.Enabled = true;
            }
        } // cmbChannelName_TextChanged

        private void cmbTNCtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbOnAirBaud.Enabled = false;
            var switchExpr = cmbTNCType.Text;
            switch (switchExpr)
            {
                case "KPC4":
                case "KPC9612":
                case "KPC9612+":
                case "KAM/+":
                case "KAMXL":
                case "PTC II":
                case "PTC IIpro":
                case "PK-900":
                    {
                        nudTNCPort.Maximum = 2; // TODO: May want to detect how many modems are installed in PTC types and limit to 1 or 2
                        break;
                    }

                default:
                    {
                        nudTNCPort.Maximum = 1;
                        break;
                    }
            }

            var strFilename = default(string);
            lblTNCPort.Enabled = true;
            nudTNCPort.Enabled = true;
            var switchExpr1 = cmbTNCType.Text;
            switch (switchExpr1)
            {
                case "KAM98":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKAM98.aps";
                        lblTNCPort.Enabled = false;
                        nudTNCPort.Enabled = false;
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KAM/+":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKAM+.aps";
                        lblTNCPort.Enabled = false;
                        nudTNCPort.Enabled = false;
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KAMXL":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKAMXL.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PTC II":
                case "PTC IIpro":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePTCII_pro.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PTC IIe":
                case "PTC IIex":
                case "PTC IIusb":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePTCII_e.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PTC DR-7800":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePTC_DR7800.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "KPC9612":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKPC9612.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "KPC9612+":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKPC9612+.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "KPC3":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKPC3.aps";
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KPC3+":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKPC3+.aps";
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KPC4":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKPC4.aps";
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "PK-88":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePK88.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PK-96":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePK96.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PK-232":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePK232.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PK-900":
                    {
                        strFilename = Globals.SiteRootDirectory + @"Data\ExamplePK900.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }
                // Case "TNC2/W8DEDhost"
                // strFilename = SiteRootDirectory & "Data\ExampleTNC2_W8DEDhost.aps"
                case "TM-D700 int":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKenwoodKISSD700.aps";
                        break;
                    }

                case "TM-D710 int":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKenwoodKISSD710.aps";
                        break;
                    }

                case "TM-D72":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleKenwoodTMD72.aps";
                        break;
                    }

                case "TS-2000 int":
                case "ALINCO int":
                case "TH-D7 int":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleTascoKISS.aps";
                        break;
                    }

                case "Generic KISS":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = Globals.SiteRootDirectory + @"Data\ExampleNativeKISS.aps";
                        break;
                    }
            }

            if (System.IO.File.Exists(strFilename))
            {
                txtTNCConfigurationFile.Text = strFilename;
            }
            else
            {
                txtTNCConfigurationFile.Text = "";
            }

            cmbTNCBaudRate.Items.Clear();
            if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text.IndexOf("DR-7800") != -1)
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
        } // cmbTNCtype_SelectedIndexChanged

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Globals.IsValidFileName(cmbChannelName.Text))
            {
                cmbChannelName.Focus();
                return;
            }

            cmbChannelName.Text = cmbChannelName.Text.Trim();
            cmbChannelName.Text = cmbChannelName.Text.Replace("|", "");
            if (string.IsNullOrEmpty(cmbTNCBaudRate.Text.Trim()))
            {
                MessageBox.Show("Select TNC Baud rate!", "No baud rate selected!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(cmbOnAirBaud.Text.Trim()))
            {
                MessageBox.Show("Select On Air Baud rate!", "No baud rate selected!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Channels.IsAccount(cmbChannelName.Text))
            {
                MessageBox.Show(cmbChannelName.Text + " is in use as an account name...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (rdoManual.Checked == false)
            {
                if (string.IsNullOrEmpty(cmbRadioModel.Text.Trim()) | string.IsNullOrEmpty(cmbRadioBaud.Text.Trim()) | string.IsNullOrEmpty(cmbRadioPort.Text.Trim()))

                {
                    MessageBox.Show("The parameters for Radio control are not complete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (Channels.IsChannel(cmbChannelName.Text))
            {
                MessageBox.Show("The channel name " + cmbChannelName.Text + " is already in use...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
            }
            else
            {
                if (!System.IO.File.Exists(txtTNCConfigurationFile.Text))
                {
                    MessageBox.Show("Invalid TNC configuration file.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTNCConfigurationFile.Focus();
                    return;
                }

                if (!rdoManual.Checked)
                {
                    if (!Globals.WithinLimits(txtFreqMHz.Text, 2400, 29))
                    {
                        MessageBox.Show("Frequency must be in MHz between 29.0 and 2400", "Frequency Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtFreqMHz.Focus();
                        return;
                    }

                    if (rdoSerial.Checked & (cmbRadioPort.Text ?? "") == (cmbTNCSerialPort.Text ?? ""))
                    {
                        if (!(cmbRadioModel.Text == "Kenwood TS-2000" & cmbTNCType.Text == "TS-2000 int" | cmbRadioModel.Text == "Kenwood TM-D700" & cmbTNCType.Text == "TM-D700 int" | cmbRadioModel.Text == "Kenwood TH-D7" & cmbTNCType.Text == "TH-D7 int"))

                        {
                            MessageBox.Show("Radio Control and TNC must use different serial ports.", "Serial Port Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                var stcNewChannel = default(ChannelProperties);
                UpdateChannelProperties(ref stcNewChannel);
                Channels.AddChannel(ref stcNewChannel);
                Channels.FillChannelCollection();
                FillChannelList();
                Globals.Settings.Save("Properties", "Last Packet TNC Channel", cmbChannelName.Text);
            }

            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
            // Me.Close()
        } // btnAdd_Click

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show("The packet channel " + cmbChannelName.Text + " is not found...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (MessageBox.Show("Confirm removal of packet channel " + cmbChannelName.Text + "...", "Remove Channel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Channels.RemoveChannel(cmbChannelName.Text);
                Channels.FillChannelCollection();
                FillChannelList();
                if (Globals.cllFastStart.Contains(cmbChannelName.Text))
                {
                    Globals.cllFastStart.Remove(cmbChannelName.Text);
                }

                Globals.Settings.Save("Properties", "Last Packet TNC Channel", "");
                ClearEntries();
                // Me.Close()
            }
        } // btnRemove_Click

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cmbChannelName.Items.Contains(cmbChannelName.Text) == false)
            {
                MessageBox.Show("The packet channel " + cmbChannelName.Text + " is not found...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (!System.IO.File.Exists(txtTNCConfigurationFile.Text))
                {
                    MessageBox.Show("Invalid TNC configuration file.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtTNCConfigurationFile.Focus();
                    return;
                }

                if (!rdoManual.Checked)
                {
                    if (!Globals.WithinLimits(txtFreqMHz.Text, 2400, 29))
                    {
                        MessageBox.Show("Frequency must be in MHz between 29.0 and 2400", "Frequency Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtFreqMHz.Focus();
                        return;
                    }

                    if (cmbTNCType.Text == "TS-2000 int")
                    {
                        MessageBox.Show("Radio Control of TS-2000 not possible when using internal TNC!", "Control Limitation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (rdoSerial.Checked & (cmbRadioPort.Text ?? "") == (cmbTNCSerialPort.Text ?? ""))
                    {
                        MessageBox.Show("Radio Control and TNC must use different serial ports.", "Serial Port Conflict", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                var stcUpdateChannel = default(ChannelProperties);
                UpdateChannelProperties(ref stcUpdateChannel);
                Channels.UpdateChannel(ref stcUpdateChannel);
                Channels.FillChannelCollection();
                FillChannelList();

                // Clear channel name from the fast start list...
                if (Globals.cllFastStart.Contains(cmbChannelName.Text))
                    Globals.cllFastStart.Remove(cmbChannelName.Text);
                Globals.Settings.Save("Properties", "Last Packet TNC Channel", cmbChannelName.Text);
                // Me.Close()
            }
        } // btnUpdate_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            Globals.Settings.Save("Properties", "Last Packet TNC Channel", cmbChannelName.Text);
            Close();
        } // btnClose_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs130.htm");
        } // btnHelp_Click

        private void rdoManual_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoManual.Checked)
            {
                txtFreqMHz.Enabled = false;
                cmbRadioModel.Enabled = false;
                cmbRadioPort.Enabled = false;
                grpPTCLevels.Enabled = false;
                txtRadioAdd.Enabled = false;
                cmbRadioBaud.Enabled = false;
            }
        }

        private void rdoViaPTCII_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoViaPTCII.Checked)
            {
                cmbRadioModel.Enabled = true;
                grpPTCLevels.Enabled = true;
                cmbRadioPort.Enabled = false;
                txtRadioAdd.Enabled = cmbRadioModel.Text.IndexOf("Icom") != -1;
                cmbRadioBaud.Enabled = true;
                txtFreqMHz.Enabled = true;
            }
        }

        private void rdoSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSerial.Checked)
            {
                cmbRadioModel.Enabled = true;
                cmbRadioPort.Enabled = !(cmbTNCType.Text == "TH-D7 int" | cmbTNCType.Text == "TM-D700 int" | cmbTNCType.Text == "TS-2000 int");

                grpPTCLevels.Enabled = false;
                txtRadioAdd.Enabled = cmbRadioModel.Text.IndexOf("Icom") != -1;
                cmbRadioBaud.Enabled = !(cmbTNCType.Text == "TH-D7 int" | cmbTNCType.Text == "TM-D700 int" | cmbTNCType.Text == "TS-2000 int");

                txtFreqMHz.Enabled = true;
            }
        }

        private void cmbRadioModel_TextChanged(object sender, EventArgs e)
        {
            txtRadioAdd.Enabled = cmbRadioModel.Text.IndexOf("Icom") != -1;
        }

        private void cmbTNCtype_TextChanged(object sender, EventArgs e)
        {
            rdoViaPTCII.Enabled = cmbTNCType.Text.IndexOf("PTC II") != -1 | cmbTNCType.Text.IndexOf("DR-7800") != -1;
            if (cmbTNCType.Text == "TH-D7 int" | cmbTNCType.Text == "TM-D700 int" | cmbTNCType.Text == "TS-2000 int")

            {
                rdoManual.Checked = true;
                rdoSerial.Enabled = false;
            }
            else
            {
                cmbRadioPort.Enabled = rdoSerial.Checked;
                cmbRadioBaud.Enabled = rdoSerial.Checked;
                rdoSerial.Enabled = true;
            }
        }

        private void txtFreqMHz_TextChanged(object sender, EventArgs e)
        {
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
                strError = Channels.GetChannelRecords(true, Globals.strServiceCodes);
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

        /* TODO ERROR: Skipped IfDirectiveTrivia */
        private void cmbRemoteCallsign_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 
            // An entry in the Remote Callsign list was selected.
            // 
            string strFreqEntry;
            string[] strStation;
            string strCallsign = CleanupCallSign(cmbRemoteCallsign.Text);
            cmbRemoteCallsign.Text = strCallsign;
            stcSelectedChannel.RemoteCallsign = strCallsign;
            for (int i = 0, loopTo = arySelectedMBOs.Length - 1; i <= loopTo; i++)
            {
                strStation = arySelectedMBOs[i].Split(':');
                // Look for a channel line matching the selected station.
                if ((strStation[0] ?? "") == (strCallsign ?? ""))
                {
                    // We found the station.  Set up its list of frequencies and baud rates.
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

            stcSelectedChannel.RemoteCallsign = cmbRemoteCallsign.Text;
        } // cmbCallSigns_SelectedIndexChanged
        /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        private void cmbOnAirBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 
            // On-air baud rate changed.
            // 
            SetRMSList();
        }

        private string CleanupCallSign(string strCallsign)
        {
            // 
            // Remove the baud rate indicator.
            // 
            var strTokens = strCallsign.Split(' ');
            if (strTokens.Length < 1)
                return "";
            return strTokens[0].Trim().ToUpper();
        }

        private void cmbFreqs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 
            // An entry in the frequency list has been selected.
            // 
        }
    }
} // DialogPacketTNCChannels