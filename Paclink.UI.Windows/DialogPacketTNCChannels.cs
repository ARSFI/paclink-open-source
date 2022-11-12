using Paclink.UI.Common;
using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;

namespace Paclink.UI.Windows
{
    public partial class DialogPacketTNCChannels : IPacketTNCChannelWindow
    {
        private string _remoteCallsign;
        private string _radioCenterFrequency;
        private string _tncBaudRate;

        public DialogPacketTNCChannels(IPacketTNCChannelBacking backingObject)
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

            _remoteCallsign = string.Empty;
            _radioCenterFrequency = string.Empty;
            _tncBaudRate = string.Empty;

            BackingObject = backingObject;
        }

        private string[] arySelectedMBOs;

        public IPacketTNCChannelBacking BackingObject { get; private set; }

        private void PacketTNCChannels_Load(object sender, EventArgs e)
        {
            InitializeControls();
            ClearEntries();
            FillChannelList();
            cmbChannelName.Text = BackingObject.LastPacketChannel;
            if (!string.IsNullOrEmpty(cmbChannelName.Text) && cmbChannelName.Text != "<Enter a new channel>")
            {
                if (BackingObject.ContainsChannel(cmbChannelName.Text))
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

            foreach (var port in BackingObject.SerialPorts)
            {
                cmbTNCSerialPort.Items.Add(port);
                cmbRadioPort.Items.Add(port);
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
            if (!BackingObject.HasChannelList)
            {
                MessageBox.Show("Click 'Update Channel List' to download the list of available channels");
                return false;
            }

            var aryResults = BackingObject.ChannelList.ToArray();
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
                    if (BackingObject.CanUseBaudAndFrequency(strFreqList, cmbTNCType.Text, cmbOnAirBaud.Text))
                    {
                        var strItems = strFreqList.Split('|');
                        cmbRemoteCallsign.Items.Add(station.Substring(0, intIndex));
                        strStationCall = station.Substring(0, intIndex);
                        if ((strStationCall ?? "") == (_remoteCallsign ?? ""))
                            blnFoundCallsign = true;
                    }
                }

                if (blnFoundCallsign)
                {
                    cmbRemoteCallsign.Text = _remoteCallsign;
                    cmbFreqs.Text = _radioCenterFrequency;
                }
                else
                {
                    cmbRemoteCallsign.Text = "";
                    cmbFreqs.Text = "";
                    _remoteCallsign = "";
                    cmbFreqs.Text = _radioCenterFrequency;
                }
            }
            else
            {
                cmbRemoteCallsign.Items.Clear();
                cmbRemoteCallsign.Text = _remoteCallsign;
                cmbFreqs.Items.Clear();
                cmbFreqs.Text = _radioCenterFrequency;
            }

            return true;
        } // SetRMSList

        private void FillChannelList()
        {
            cmbChannelName.Items.Clear();
            foreach (var name in BackingObject.ChannelNames)
            {
                cmbChannelName.Items.Add(name);
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
            string centerFrequency;
            int priority;
            int activityTimeout;
            int scriptTimeout;
            string script;
            bool enabled;
            string serialPort;
            string tncType;
            int tncPort;
            string tncConfigurationFile;
            bool tncConfigureOnFirstUseOnly;
            string radioControlBaud;
            string radioControlPort;
            string radioModel;
            int onAirBaud;
            string civAddress;
            bool radioControlSerial;
            bool radioControlPTC;
            bool radioControlManual;
            bool radioTTLLevel;
            string freqMHZ;
            string tncBaudRate;
            string remoteCallsign;

            BackingObject.GetChannelInfo(
                cmbChannelName.Text, out centerFrequency, out priority, out activityTimeout,
                out scriptTimeout, out script, out enabled, out serialPort,
                out tncType, out tncPort, out tncConfigurationFile,
                out tncConfigureOnFirstUseOnly, out radioControlBaud, out radioControlPort,
                out radioModel, out onAirBaud, out civAddress,
                out radioControlSerial, out radioControlPTC, out radioControlManual,
                out radioTTLLevel, out freqMHZ, out tncBaudRate, out remoteCallsign);

            cmbFreqs.Text = centerFrequency;
            cmbRemoteCallsign.Text = remoteCallsign;
            nudPriority.Value = priority;
            nudActivityTimeout.Value = activityTimeout;
            nudScriptTimeout.Value = scriptTimeout;
            txtScript.Text = script;
            chkEnabled.Checked = enabled;
            cmbTNCSerialPort.Text = serialPort;
            cmbTNCType.Text = tncType;
            nudTNCPort.Value = tncPort;
            txtTNCConfigurationFile.Text = tncConfigurationFile;
            chkFirstUseOnly.Checked = tncConfigureOnFirstUseOnly;

            // Radio Type
            cmbRadioBaud.Text = radioControlBaud;
            cmbRadioPort.Text = radioControlPort;
            cmbRadioModel.Text = radioModel;
            cmbOnAirBaud.Text = onAirBaud.ToString();
            txtRadioAdd.Text = civAddress;
            rdoSerial.Checked = radioControlSerial;
            rdoViaPTCII.Checked = radioControlPTC;
            rdoManual.Checked = radioControlManual;
            rdoTTL.Checked = radioTTLLevel;
            rdoV24.Checked = !radioTTLLevel;

            txtFreqMHz.Text = freqMHZ;

            cmbTNCBaudRate.Items.Clear();
            if (cmbTNCType.Text.StartsWith("PTC II") | cmbTNCType.Text.IndexOf("DR-7800") != -1)
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

            _remoteCallsign = remoteCallsign;
            _radioCenterFrequency = centerFrequency;
            _tncBaudRate = cmbTNCBaudRate.Text;

            btnAdd.Enabled = false;
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
            cmbFreqs.Enabled = true;
        } // SetEntries

        private void cmbChannelName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbChannelName.Text) && cmbChannelName.Text != "<Enter a new channel>")
            {
                if (BackingObject.ContainsChannel(cmbChannelName.Text))
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
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKAM98.aps";
                        lblTNCPort.Enabled = false;
                        nudTNCPort.Enabled = false;
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KAM/+":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKAM+.aps";
                        lblTNCPort.Enabled = false;
                        nudTNCPort.Enabled = false;
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KAMXL":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKAMXL.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PTC II":
                case "PTC IIpro":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePTCII_pro.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PTC IIe":
                case "PTC IIex":
                case "PTC IIusb":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePTCII_e.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PTC DR-7800":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePTC_DR7800.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "KPC9612":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKPC9612.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "KPC9612+":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKPC9612+.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "KPC3":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKPC3.aps";
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KPC3+":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKPC3+.aps";
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "KPC4":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKPC4.aps";
                        cmbOnAirBaud.Text = "1200";
                        cmbOnAirBaud.Enabled = false;
                        break;
                    }

                case "PK-88":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePK88.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PK-96":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePK96.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PK-232":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePK232.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }

                case "PK-900":
                    {
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExamplePK900.aps";
                        cmbOnAirBaud.Enabled = true;
                        break;
                    }
                // Case "TNC2/W8DEDhost"
                // strFilename = SiteRootDirectory & "Data\ExampleTNC2_W8DEDhost.aps"
                case "TM-D700 int":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKenwoodKISSD700.aps";
                        break;
                    }

                case "TM-D710 int":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKenwoodKISSD710.aps";
                        break;
                    }

                case "TM-D72":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleKenwoodTMD72.aps";
                        break;
                    }

                case "TS-2000 int":
                case "ALINCO int":
                case "TH-D7 int":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleTascoKISS.aps";
                        break;
                    }

                case "Generic KISS":
                    {
                        cmbOnAirBaud.Enabled = true;
                        strFilename = BackingObject.SiteRootDirectory + @"Data\ExampleNativeKISS.aps";
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
            if (cmbTNCType.Text.StartsWith("PTC II") || cmbTNCType.Text.IndexOf("DR-7800") != -1)
            {
                cmbTNCBaudRate.Items.Add("38400");
                cmbTNCBaudRate.Items.Add("57600");
                cmbTNCBaudRate.Items.Add("115200");
                if (Convert.ToInt32(_tncBaudRate) < 38400)
                    _tncBaudRate = "38400";
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

            cmbTNCBaudRate.Text = _tncBaudRate;
        } // cmbTNCtype_SelectedIndexChanged

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!BackingObject.IsValidChannelName(cmbChannelName.Text))
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

            if (BackingObject.IsAccount(cmbChannelName.Text))
            {
                MessageBox.Show(cmbChannelName.Text + " is in use as an account name...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbChannelName.Focus();
                return;
            }

            if (rdoManual.Checked == false)
            {
                if (string.IsNullOrEmpty(cmbRadioModel.Text.Trim()) || string.IsNullOrEmpty(cmbRadioBaud.Text.Trim()) || string.IsNullOrEmpty(cmbRadioPort.Text.Trim()))

                {
                    MessageBox.Show("The parameters for Radio control are not complete!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (BackingObject.IsChannel(cmbChannelName.Text))
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
                    if (!BackingObject.IsFreqInLimits(txtFreqMHz.Text))
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

                _remoteCallsign = cmbRemoteCallsign.Text;
                _radioCenterFrequency = (1000 * double.Parse(txtFreqMHz.Text, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00000.0");
                _tncBaudRate = cmbTNCBaudRate.Text;

                BackingObject.AddChannel(
                    cmbChannelName.Text, cmbFreqs.Text, Convert.ToInt32(nudPriority.Value), cmbRemoteCallsign.Text,
                    Convert.ToInt32(nudActivityTimeout.Value), Convert.ToInt32(nudScriptTimeout.Value), txtScript.Text, chkEnabled.Checked,
                    cmbTNCSerialPort.Text, cmbTNCType.Text, Convert.ToInt32(nudTNCPort.Value), txtTNCConfigurationFile.Text,
                    chkFirstUseOnly.Checked, cmbRadioBaud.Text, cmbRadioPort.Text, cmbRadioModel.Text,
                    Convert.ToInt32(cmbOnAirBaud.Text), txtRadioAdd.Text, rdoSerial.Checked, rdoViaPTCII.Checked, rdoManual.Checked,
                    rdoTTL.Checked, _radioCenterFrequency,
                    cmbTNCBaudRate.Text);

                FillChannelList();

                BackingObject.LastPacketChannel = cmbChannelName.Text;
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
                BackingObject.RemoveChannel(cmbChannelName.Text);
                FillChannelList();

                BackingObject.LastPacketChannel = "";
                _remoteCallsign = string.Empty;
                _radioCenterFrequency = string.Empty;
                _tncBaudRate = string.Empty;

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
                    if (!BackingObject.IsFreqInLimits(txtFreqMHz.Text))
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

                _remoteCallsign = cmbRemoteCallsign.Text;
                _radioCenterFrequency = (1000 * double.Parse(txtFreqMHz.Text, System.Globalization.NumberStyles.AllowDecimalPoint)).ToString("##00000.0");
                _tncBaudRate = cmbTNCBaudRate.Text;

                BackingObject.UpdateChannel(
                    cmbChannelName.Text, cmbFreqs.Text, Convert.ToInt32(nudPriority.Value), cmbRemoteCallsign.Text,
                    Convert.ToInt32(nudActivityTimeout.Value), Convert.ToInt32(nudScriptTimeout.Value), txtScript.Text, chkEnabled.Checked,
                    cmbTNCSerialPort.Text, cmbTNCType.Text, Convert.ToInt32(nudTNCPort.Value), txtTNCConfigurationFile.Text,
                    chkFirstUseOnly.Checked, cmbRadioBaud.Text, cmbRadioPort.Text, cmbRadioModel.Text,
                    Convert.ToInt32(cmbOnAirBaud.Text), txtRadioAdd.Text, rdoSerial.Checked, rdoViaPTCII.Checked, rdoManual.Checked,
                    rdoTTL.Checked, _radioCenterFrequency,
                    cmbTNCBaudRate.Text);

                FillChannelList();

                BackingObject.LastPacketChannel = cmbChannelName.Text;
                // Me.Close()
            }
        } // btnUpdate_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            BackingObject.LastPacketChannel = cmbChannelName.Text;
            Close();
        } // btnClose_Click

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs130.htm");
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

        /* TODO ERROR: Skipped IfDirectiveTrivia */
        private void cmbRemoteCallsign_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 
            // An entry in the Remote Callsign list was selected.
            // 
            string strFreqEntry;
            string[] strStation;
            string strCallsign = BackingObject.CleanupCallSign(cmbRemoteCallsign.Text);
            cmbRemoteCallsign.Text = strCallsign;
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

                        foreach (var freq in BackingObject.GetUsableFreqs(aryFreqs, cmbTNCType.Text))
                        {
                            cmbFreqs.Items.Add(freq);
                        }

                        if (cmbFreqs.Items.Count > 0)
                        {
                            cmbFreqs.Text = cmbFreqs.Items[0].ToString();
                        }
                    }

                    return;
                }
            }
        } // cmbCallSigns_SelectedIndexChanged
        /* TODO ERROR: Skipped ElseDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
        private void cmbOnAirBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 
            // On-air baud rate changed.
            // 
            SetRMSList();
        }

        private void cmbFreqs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 
            // An entry in the frequency list has been selected.
            // 
        }

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
} // DialogPacketTNCChannels