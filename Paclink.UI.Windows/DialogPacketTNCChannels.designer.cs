using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink.UI.Windows
{
    public partial class DialogPacketTNCChannels : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is object)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPacketTNCChannels));
            _Label8 = new Label();
            _Label7 = new Label();
            _txtScript = new TextBox();
            _Label6 = new Label();
            _Label4 = new Label();
            _chkEnabled = new CheckBox();
            _cmbChannelName = new ComboBox();
            _cmbChannelName.SelectedIndexChanged += new EventHandler(cmbChannelName_SelectedIndexChanged);
            _cmbChannelName.TextChanged += new EventHandler(cmbChannelName_TextChanged);
            _Label3 = new Label();
            _Label2 = new Label();
            _Label1 = new Label();
            _btnClose = new Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _btnRemove = new Button();
            _btnRemove.Click += new EventHandler(btnRemove_Click);
            _btnAdd = new Button();
            _btnAdd.Click += new EventHandler(btnAdd_Click);
            _Label14 = new Label();
            _cmbTNCType = new ComboBox();
            _cmbTNCType.SelectedIndexChanged += new EventHandler(cmbTNCtype_SelectedIndexChanged);
            _cmbTNCType.TextChanged += new EventHandler(cmbTNCtype_TextChanged);
            _Label22 = new Label();
            _cmbTNCSerialPort = new ComboBox();
            _btnBrowseConfiguration = new Button();
            _btnBrowseConfiguration.Click += new EventHandler(btnBrowseConfiguration_Click);
            _txtTNCConfigurationFile = new TextBox();
            _Label18 = new Label();
            _cmbTNCBaudRate = new ComboBox();
            _Label21 = new Label();
            _ToolTip1 = new ToolTip(components);
            _nudActivityTimeout = new NumericUpDown();
            _nudScriptTimeout = new NumericUpDown();
            _nudPriority = new NumericUpDown();
            _chkFirstUseOnly = new CheckBox();
            _rdoV24 = new RadioButton();
            _rdoTTL = new RadioButton();
            _rdoViaPTCII = new RadioButton();
            _rdoViaPTCII.CheckedChanged += new EventHandler(rdoViaPTCII_CheckedChanged);
            _txtRadioAdd = new TextBox();
            _cmbRadioModel = new ComboBox();
            _cmbRadioModel.TextChanged += new EventHandler(cmbRadioModel_TextChanged);
            _rdoSerial = new RadioButton();
            _rdoSerial.CheckedChanged += new EventHandler(rdoSerial_CheckedChanged);
            _rdoManual = new RadioButton();
            _rdoManual.CheckedChanged += new EventHandler(rdoManual_CheckedChanged);
            _cmbRadioPort = new ComboBox();
            _cmbRadioBaud = new ComboBox();
            _txtFreqMHz = new TextBox();
            _txtFreqMHz.TextChanged += new EventHandler(txtFreqMHz_TextChanged);
            _cmbOnAirBaud = new ComboBox();
            _cmbOnAirBaud.SelectedIndexChanged += new EventHandler(cmbOnAirBaud_SelectedIndexChanged);
            _nudTNCPort = new NumericUpDown();
            _lblTNCPort = new Label();
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            _Label9 = new Label();
            _grpRadioControl = new GroupBox();
            _Label11 = new Label();
            _grpPTCLevels = new GroupBox();
            _Label15 = new Label();
            _lblRadioAddress = new Label();
            _Label10 = new Label();
            _Label16 = new Label();
            _Label12 = new Label();
            _btnUpdateChannelList = new Button();
            _btnUpdateChannelList.Click += new EventHandler(btnUpdateChannelList_Click);
            _cmbRemoteCallsign = new ComboBox();
            _cmbRemoteCallsign.SelectedIndexChanged += new EventHandler(cmbRemoteCallsign_SelectedIndexChanged);
            _lblFrequency = new Label();
            _cmbFreqs = new ComboBox();
            _cmbFreqs.SelectedIndexChanged += new EventHandler(cmbFreqs_SelectedIndexChanged);
            ((System.ComponentModel.ISupportInitialize)_nudActivityTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudScriptTimeout).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudTNCPort).BeginInit();
            _grpRadioControl.SuspendLayout();
            _grpPTCLevels.SuspendLayout();
            SuspendLayout();
            // 
            // Label8
            // 
            _Label8.AutoSize = true;
            _Label8.CausesValidation = false;
            _Label8.Location = new Point(735, 208);
            _Label8.Name = "_Label8";
            _Label8.Size = new Size(47, 13);
            _Label8.TabIndex = 118;
            _Label8.Text = "seconds";
            // 
            // Label7
            // 
            _Label7.AutoSize = true;
            _Label7.CausesValidation = false;
            _Label7.Location = new Point(563, 208);
            _Label7.Name = "_Label7";
            _Label7.Size = new Size(118, 13);
            _Label7.TabIndex = 117;
            _Label7.Text = "Script inactivity timeout:";
            // 
            // txtScript
            // 
            _txtScript.CausesValidation = false;
            _txtScript.CharacterCasing = CharacterCasing.Upper;
            _txtScript.Location = new Point(565, 78);
            _txtScript.Multiline = true;
            _txtScript.Name = "_txtScript";
            _txtScript.RightToLeft = RightToLeft.No;
            _txtScript.ScrollBars = ScrollBars.Both;
            _txtScript.Size = new Size(231, 120);
            _txtScript.TabIndex = 13;
            _ToolTip1.SetToolTip(_txtScript, "Enter optional connect scrip. One line for simple vias or multiline.");
            // 
            // Label6
            // 
            _Label6.AutoSize = true;
            _Label6.CausesValidation = false;
            _Label6.Location = new Point(610, 62);
            _Label6.Name = "_Label6";
            _Label6.Size = new Size(119, 13);
            _Label6.TabIndex = 116;
            _Label6.Text = "Optional connect script:";
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(358, 129);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(137, 13);
            _Label4.TabIndex = 111;
            _Label4.Text = "Activity timeout (in minutes):";
            // 
            // chkEnabled
            // 
            _chkEnabled.AutoSize = true;
            _chkEnabled.CheckAlign = ContentAlignment.MiddleRight;
            _chkEnabled.Location = new Point(370, 93);
            _chkEnabled.Name = "_chkEnabled";
            _chkEnabled.Size = new Size(109, 17);
            _chkEnabled.TabIndex = 9;
            _chkEnabled.Text = "Channel enabled:";
            _ToolTip1.SetToolTip(_chkEnabled, "Enable this channel ");
            _chkEnabled.UseVisualStyleBackColor = true;
            // 
            // cmbChannelName
            // 
            _cmbChannelName.FormattingEnabled = true;
            _cmbChannelName.Location = new Point(111, 33);
            _cmbChannelName.MaxDropDownItems = 24;
            _cmbChannelName.Name = "_cmbChannelName";
            _cmbChannelName.RightToLeft = RightToLeft.No;
            _cmbChannelName.Size = new Size(190, 21);
            _cmbChannelName.Sorted = true;
            _cmbChannelName.TabIndex = 0;
            _ToolTip1.SetToolTip(_cmbChannelName, "Enter Channel Name");
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label3.Location = new Point(323, 36);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(101, 13);
            _Label3.TabIndex = 110;
            _Label3.Text = "Remote callsign:";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(16, 67);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(82, 13);
            _Label2.TabIndex = 109;
            _Label2.Text = "Channel priority:";
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label1.Location = new Point(16, 36);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(91, 13);
            _Label1.TabIndex = 108;
            _Label1.Text = "Channel name:";
            // 
            // btnClose
            // 
            _btnClose.Location = new Point(473, 376);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(129, 30);
            _btnClose.TabIndex = 22;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(336, 376);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(129, 30);
            _btnUpdate.TabIndex = 21;
            _btnUpdate.Text = "Update The Channel";
            _btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            _btnRemove.Location = new Point(201, 376);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Size = new Size(129, 30);
            _btnRemove.TabIndex = 20;
            _btnRemove.Text = "Remove This Channel";
            _btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            _btnAdd.Location = new Point(61, 376);
            _btnAdd.Name = "_btnAdd";
            _btnAdd.Size = new Size(129, 30);
            _btnAdd.TabIndex = 19;
            _btnAdd.Text = "Add New Channel";
            _btnAdd.UseVisualStyleBackColor = true;
            // 
            // Label14
            // 
            _Label14.AutoSize = true;
            _Label14.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _Label14.Location = new Point(153, 67);
            _Label14.Name = "_Label14";
            _Label14.Size = new Size(59, 13);
            _Label14.TabIndex = 122;
            _Label14.Text = "TNC Type:";
            // 
            // cmbTNCType
            // 
            _cmbTNCType.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTNCType.FormattingEnabled = true;
            _cmbTNCType.Location = new Point(218, 63);
            _cmbTNCType.MaxDropDownItems = 24;
            _cmbTNCType.Name = "_cmbTNCType";
            _cmbTNCType.Size = new Size(123, 21);
            _cmbTNCType.TabIndex = 2;
            _ToolTip1.SetToolTip(_cmbTNCType, "Select TNC type");
            // 
            // Label22
            // 
            _Label22.AutoSize = true;
            _Label22.Location = new Point(19, 129);
            _Label22.Name = "_Label22";
            _Label22.Size = new Size(57, 13);
            _Label22.TabIndex = 125;
            _Label22.Text = "Serial port:";
            // 
            // cmbTNCSerialPort
            // 
            _cmbTNCSerialPort.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTNCSerialPort.FormattingEnabled = true;
            _cmbTNCSerialPort.Location = new Point(82, 126);
            _cmbTNCSerialPort.Name = "_cmbTNCSerialPort";
            _cmbTNCSerialPort.Size = new Size(79, 21);
            _cmbTNCSerialPort.Sorted = true;
            _cmbTNCSerialPort.TabIndex = 5;
            _ToolTip1.SetToolTip(_cmbTNCSerialPort, "Select TNC serial port ");
            // 
            // btnBrowseConfiguration
            // 
            _btnBrowseConfiguration.Location = new Point(501, 200);
            _btnBrowseConfiguration.Name = "_btnBrowseConfiguration";
            _btnBrowseConfiguration.Size = new Size(51, 23);
            _btnBrowseConfiguration.TabIndex = 16;
            _btnBrowseConfiguration.Text = "Browse";
            _btnBrowseConfiguration.UseVisualStyleBackColor = true;
            // 
            // txtTNCConfigurationFile
            // 
            _txtTNCConfigurationFile.Location = new Point(138, 202);
            _txtTNCConfigurationFile.Name = "_txtTNCConfigurationFile";
            _txtTNCConfigurationFile.Size = new Size(357, 20);
            _txtTNCConfigurationFile.TabIndex = 15;
            _txtTNCConfigurationFile.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtTNCConfigurationFile, "The TNC configuration (.aps) file. (see Examples for templates) ");
            // 
            // Label18
            // 
            _Label18.AutoSize = true;
            _Label18.Location = new Point(16, 205);
            _Label18.Name = "_Label18";
            _Label18.Size = new Size(116, 13);
            _Label18.TabIndex = 136;
            _Label18.Text = "TNC Configuration File:";
            // 
            // cmbTNCBaudRate
            // 
            _cmbTNCBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTNCBaudRate.FormattingEnabled = true;
            _cmbTNCBaudRate.Location = new Point(251, 126);
            _cmbTNCBaudRate.Name = "_cmbTNCBaudRate";
            _cmbTNCBaudRate.Size = new Size(79, 21);
            _cmbTNCBaudRate.TabIndex = 7;
            _ToolTip1.SetToolTip(_cmbTNCBaudRate, "Select TNC baud rate (not on air baud rate) ");
            // 
            // Label21
            // 
            _Label21.AutoSize = true;
            _Label21.Location = new Point(184, 129);
            _Label21.Name = "_Label21";
            _Label21.Size = new Size(61, 13);
            _Label21.TabIndex = 126;
            _Label21.Text = "Baud Rate:";
            // 
            // nudActivityTimeout
            // 
            _nudActivityTimeout.Location = new Point(501, 127);
            _nudActivityTimeout.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            _nudActivityTimeout.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _nudActivityTimeout.Name = "_nudActivityTimeout";
            _nudActivityTimeout.Size = new Size(37, 20);
            _nudActivityTimeout.TabIndex = 6;
            _nudActivityTimeout.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudActivityTimeout, "Select maximum inactivity time before auto disconnect");
            _nudActivityTimeout.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // nudScriptTimeout
            // 
            _nudScriptTimeout.Location = new Point(687, 206);
            _nudScriptTimeout.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            _nudScriptTimeout.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            _nudScriptTimeout.Name = "_nudScriptTimeout";
            _nudScriptTimeout.Size = new Size(42, 20);
            _nudScriptTimeout.TabIndex = 14;
            _nudScriptTimeout.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudScriptTimeout, "Timeout for each script line");
            _nudScriptTimeout.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // nudPriority
            // 
            _nudPriority.Location = new Point(104, 63);
            _nudPriority.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            _nudPriority.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudPriority.Name = "_nudPriority";
            _nudPriority.Size = new Size(37, 20);
            _nudPriority.TabIndex = 4;
            _nudPriority.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudPriority, "Select priority 1-5, 1=highest (default=3)");
            _nudPriority.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // chkFirstUseOnly
            // 
            _chkFirstUseOnly.AutoSize = true;
            _chkFirstUseOnly.CheckAlign = ContentAlignment.MiddleRight;
            _chkFirstUseOnly.Location = new Point(80, 93);
            _chkFirstUseOnly.Name = "_chkFirstUseOnly";
            _chkFirstUseOnly.Size = new Size(233, 17);
            _chkFirstUseOnly.TabIndex = 12;
            _chkFirstUseOnly.Text = "Do a full TNC configuration only on first use:";
            _ToolTip1.SetToolTip(_chkFirstUseOnly, " ");
            _chkFirstUseOnly.UseVisualStyleBackColor = true;
            // 
            // rdoV24
            // 
            _rdoV24.AutoSize = true;
            _rdoV24.Location = new Point(67, 19);
            _rdoV24.Name = "_rdoV24";
            _rdoV24.Size = new Size(61, 17);
            _rdoV24.TabIndex = 1;
            _rdoV24.TabStop = true;
            _rdoV24.Text = "RS-232";
            _ToolTip1.SetToolTip(_rdoV24, "Select TTL or RS232 Levels for PTCIIpro and PTCIIusb models");
            _rdoV24.UseVisualStyleBackColor = true;
            // 
            // rdoTTL
            // 
            _rdoTTL.AutoSize = true;
            _rdoTTL.Location = new Point(12, 19);
            _rdoTTL.Name = "_rdoTTL";
            _rdoTTL.Size = new Size(45, 17);
            _rdoTTL.TabIndex = 0;
            _rdoTTL.TabStop = true;
            _rdoTTL.Text = "TTL";
            _ToolTip1.SetToolTip(_rdoTTL, "Select TTL or RS232 Levels for PTCIIpro and PTCIIusb models");
            _rdoTTL.UseVisualStyleBackColor = true;
            // 
            // rdoViaPTCII
            // 
            _rdoViaPTCII.AutoSize = true;
            _rdoViaPTCII.Location = new Point(55, 42);
            _rdoViaPTCII.Name = "_rdoViaPTCII";
            _rdoViaPTCII.Size = new Size(129, 17);
            _rdoViaPTCII.TabIndex = 1;
            _rdoViaPTCII.TabStop = true;
            _rdoViaPTCII.Text = "Via PTC II, IIpro, IIusb";
            _ToolTip1.SetToolTip(_rdoViaPTCII, "Use Radio control via a PTC II, IIpro or IIusb (not available in  IIe or IIex mod" + "els)");
            _rdoViaPTCII.UseVisualStyleBackColor = true;
            // 
            // txtRadioAdd
            // 
            _txtRadioAdd.Enabled = false;
            _txtRadioAdd.Location = new Point(623, 19);
            _txtRadioAdd.Name = "_txtRadioAdd";
            _txtRadioAdd.Size = new Size(25, 20);
            _txtRadioAdd.TabIndex = 6;
            _txtRadioAdd.Text = "01";
            _txtRadioAdd.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtRadioAdd, "Hex Address for Icom C-IV Radios");
            // 
            // cmbRadioModel
            // 
            _cmbRadioModel.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRadioModel.FormattingEnabled = true;
            _cmbRadioModel.Location = new Point(368, 19);
            _cmbRadioModel.Name = "_cmbRadioModel";
            _cmbRadioModel.Size = new Size(120, 21);
            _cmbRadioModel.TabIndex = 5;
            _ToolTip1.SetToolTip(_cmbRadioModel, "Select Radio type here.");
            // 
            // rdoSerial
            // 
            _rdoSerial.AutoSize = true;
            _rdoSerial.Location = new Point(55, 112);
            _rdoSerial.Name = "_rdoSerial";
            _rdoSerial.Size = new Size(124, 17);
            _rdoSerial.TabIndex = 2;
            _rdoSerial.Text = "Direct via Serial Port:";
            _ToolTip1.SetToolTip(_rdoSerial, "Select this if the Radio control is direct using a Serial or USB port.");
            _rdoSerial.UseVisualStyleBackColor = true;
            // 
            // rdoManual
            // 
            _rdoManual.AutoSize = true;
            _rdoManual.Checked = true;
            _rdoManual.Location = new Point(55, 24);
            _rdoManual.Name = "_rdoManual";
            _rdoManual.Size = new Size(93, 17);
            _rdoManual.TabIndex = 0;
            _rdoManual.TabStop = true;
            _rdoManual.Text = "Manual (none)";
            _ToolTip1.SetToolTip(_rdoManual, "Select if manual radio control.");
            _rdoManual.UseVisualStyleBackColor = true;
            // 
            // cmbRadioPort
            // 
            _cmbRadioPort.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRadioPort.FormattingEnabled = true;
            _cmbRadioPort.Location = new Point(368, 75);
            _cmbRadioPort.Name = "_cmbRadioPort";
            _cmbRadioPort.Size = new Size(68, 21);
            _cmbRadioPort.TabIndex = 3;
            _ToolTip1.SetToolTip(_cmbRadioPort, "If you selected Direct via Serial this is where you select the radio control port" + "");
            // 
            // cmbRadioBaud
            // 
            _cmbRadioBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRadioBaud.FormattingEnabled = true;
            _cmbRadioBaud.Location = new Point(368, 48);
            _cmbRadioBaud.Name = "_cmbRadioBaud";
            _cmbRadioBaud.Size = new Size(69, 21);
            _cmbRadioBaud.TabIndex = 4;
            _ToolTip1.SetToolTip(_cmbRadioBaud, "Baud rate for the radio control port (8N1 assumed) ");
            // 
            // txtFreqMHz
            // 
            _txtFreqMHz.CharacterCasing = CharacterCasing.Upper;
            _txtFreqMHz.Location = new Point(368, 104);
            _txtFreqMHz.Name = "_txtFreqMHz";
            _txtFreqMHz.Size = new Size(93, 20);
            _txtFreqMHz.TabIndex = 171;
            _txtFreqMHz.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtFreqMHz, "Enter the remote target callsign");
            // 
            // cmbOnAirBaud
            // 
            _cmbOnAirBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbOnAirBaud.FormattingEnabled = true;
            _cmbOnAirBaud.Location = new Point(466, 63);
            _cmbOnAirBaud.Name = "_cmbOnAirBaud";
            _cmbOnAirBaud.Size = new Size(59, 21);
            _cmbOnAirBaud.TabIndex = 146;
            _ToolTip1.SetToolTip(_cmbOnAirBaud, "Baud rate for the radio control port (8N1 assumed) ");
            // 
            // nudTNCPort
            // 
            _nudTNCPort.Location = new Point(569, 71);
            _nudTNCPort.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            _nudTNCPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudTNCPort.Name = "_nudTNCPort";
            _nudTNCPort.Size = new Size(36, 20);
            _nudTNCPort.TabIndex = 3;
            _nudTNCPort.TextAlign = HorizontalAlignment.Center;
            _nudTNCPort.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblTNCPort
            // 
            _lblTNCPort.AutoSize = true;
            _lblTNCPort.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _lblTNCPort.Location = new Point(509, 73);
            _lblTNCPort.Name = "_lblTNCPort";
            _lblTNCPort.Size = new Size(54, 13);
            _lblTNCPort.TabIndex = 143;
            _lblTNCPort.Text = "TNC Port:";
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(608, 376);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(129, 30);
            _btnHelp.TabIndex = 23;
            _btnHelp.Text = "Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // Label9
            // 
            _Label9.AutoSize = true;
            _Label9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label9.Location = new Point(159, 9);
            _Label9.Name = "_Label9";
            _Label9.Size = new Size(481, 13);
            _Label9.TabIndex = 144;
            _Label9.Text = "To create a new channel type a new channel name in the Channel Name text box...";
            // 
            // grpRadioControl
            // 
            _grpRadioControl.Controls.Add(_Label11);
            _grpRadioControl.Controls.Add(_txtFreqMHz);
            _grpRadioControl.Controls.Add(_grpPTCLevels);
            _grpRadioControl.Controls.Add(_Label15);
            _grpRadioControl.Controls.Add(_rdoViaPTCII);
            _grpRadioControl.Controls.Add(_lblRadioAddress);
            _grpRadioControl.Controls.Add(_txtRadioAdd);
            _grpRadioControl.Controls.Add(_Label10);
            _grpRadioControl.Controls.Add(_cmbRadioModel);
            _grpRadioControl.Controls.Add(_lblTNCPort);
            _grpRadioControl.Controls.Add(_Label16);
            _grpRadioControl.Controls.Add(_nudTNCPort);
            _grpRadioControl.Controls.Add(_rdoSerial);
            _grpRadioControl.Controls.Add(_rdoManual);
            _grpRadioControl.Controls.Add(_cmbRadioPort);
            _grpRadioControl.Controls.Add(_cmbRadioBaud);
            _grpRadioControl.Location = new Point(25, 232);
            _grpRadioControl.Name = "_grpRadioControl";
            _grpRadioControl.Size = new Size(771, 138);
            _grpRadioControl.TabIndex = 145;
            _grpRadioControl.TabStop = false;
            _grpRadioControl.Text = "Optional VHF/UHF Radio Control";
            // 
            // Label11
            // 
            _Label11.AutoSize = true;
            _Label11.Location = new Point(227, 107);
            _Label11.Name = "_Label11";
            _Label11.Size = new Size(135, 13);
            _Label11.TabIndex = 172;
            _Label11.Text = "Channel frequency in MHz:";
            // 
            // grpPTCLevels
            // 
            _grpPTCLevels.Controls.Add(_rdoV24);
            _grpPTCLevels.Controls.Add(_rdoTTL);
            _grpPTCLevels.Location = new Point(71, 65);
            _grpPTCLevels.Name = "_grpPTCLevels";
            _grpPTCLevels.Size = new Size(134, 41);
            _grpPTCLevels.TabIndex = 170;
            _grpPTCLevels.TabStop = false;
            _grpPTCLevels.Text = "PTC Levels to Radio";
            // 
            // Label15
            // 
            _Label15.AutoSize = true;
            _Label15.Location = new Point(304, 78);
            _Label15.Name = "_Label15";
            _Label15.Size = new Size(58, 13);
            _Label15.TabIndex = 168;
            _Label15.Text = "Serial Port:";
            // 
            // lblRadioAddress
            // 
            _lblRadioAddress.AutoSize = true;
            _lblRadioAddress.Location = new Point(512, 21);
            _lblRadioAddress.Name = "_lblRadioAddress";
            _lblRadioAddress.Size = new Size(105, 13);
            _lblRadioAddress.TabIndex = 163;
            _lblRadioAddress.Text = "Radio Address (hex):";
            // 
            // Label10
            // 
            _Label10.AutoSize = true;
            _Label10.Location = new Point(292, 22);
            _Label10.Name = "_Label10";
            _Label10.Size = new Size(70, 13);
            _Label10.TabIndex = 12;
            _Label10.Text = "Radio Model:";
            // 
            // Label16
            // 
            _Label16.AutoSize = true;
            _Label16.Location = new Point(300, 51);
            _Label16.Name = "_Label16";
            _Label16.Size = new Size(61, 13);
            _Label16.TabIndex = 10;
            _Label16.Text = "Baud Rate:";
            // 
            // Label12
            // 
            _Label12.AutoSize = true;
            _Label12.Location = new Point(367, 66);
            _Label12.Name = "_Label12";
            _Label12.Size = new Size(93, 13);
            _Label12.TabIndex = 147;
            _Label12.Text = "On-Air Baud Rate:";
            // 
            // btnUpdateChannelList
            // 
            _btnUpdateChannelList.Location = new Point(168, 165);
            _btnUpdateChannelList.Name = "_btnUpdateChannelList";
            _btnUpdateChannelList.Size = new Size(173, 23);
            _btnUpdateChannelList.TabIndex = 148;
            _btnUpdateChannelList.Text = "Update Channel List";
            _btnUpdateChannelList.UseVisualStyleBackColor = true;
            // 
            // cmbRemoteCallsign
            // 
            _cmbRemoteCallsign.FormattingEnabled = true;
            _cmbRemoteCallsign.Location = new Point(426, 33);
            _cmbRemoteCallsign.Name = "_cmbRemoteCallsign";
            _cmbRemoteCallsign.Size = new Size(116, 21);
            _cmbRemoteCallsign.TabIndex = 149;
            // 
            // lblFrequency
            // 
            _lblFrequency.AutoSize = true;
            _lblFrequency.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _lblFrequency.Location = new Point(570, 36);
            _lblFrequency.Name = "_lblFrequency";
            _lblFrequency.Size = new Size(70, 13);
            _lblFrequency.TabIndex = 150;
            _lblFrequency.Text = "Frequency:";
            // 
            // cmbFreqs
            // 
            _cmbFreqs.FormattingEnabled = true;
            _cmbFreqs.Location = new Point(646, 33);
            _cmbFreqs.Name = "_cmbFreqs";
            _cmbFreqs.Size = new Size(124, 21);
            _cmbFreqs.TabIndex = 151;
            _ToolTip1.SetToolTip(_cmbFreqs, "Frequency and baud rate");
            // 
            // DialogPacketTNCChannels
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(814, 428);
            Controls.Add(_cmbFreqs);
            Controls.Add(_lblFrequency);
            Controls.Add(_cmbRemoteCallsign);
            Controls.Add(_btnUpdateChannelList);
            Controls.Add(_Label12);
            Controls.Add(_cmbOnAirBaud);
            Controls.Add(_grpRadioControl);
            Controls.Add(_Label9);
            Controls.Add(_chkFirstUseOnly);
            Controls.Add(_btnHelp);
            Controls.Add(_nudPriority);
            Controls.Add(_nudScriptTimeout);
            Controls.Add(_nudActivityTimeout);
            Controls.Add(_btnBrowseConfiguration);
            Controls.Add(_txtTNCConfigurationFile);
            Controls.Add(_Label18);
            Controls.Add(_cmbTNCBaudRate);
            Controls.Add(_Label21);
            Controls.Add(_Label22);
            Controls.Add(_cmbTNCSerialPort);
            Controls.Add(_Label14);
            Controls.Add(_cmbTNCType);
            Controls.Add(_Label8);
            Controls.Add(_Label7);
            Controls.Add(_txtScript);
            Controls.Add(_Label6);
            Controls.Add(_Label4);
            Controls.Add(_chkEnabled);
            Controls.Add(_cmbChannelName);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_Label1);
            Controls.Add(_btnClose);
            Controls.Add(_btnUpdate);
            Controls.Add(_btnRemove);
            Controls.Add(_btnAdd);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogPacketTNCChannels";
            RightToLeft = RightToLeft.No;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Packet TNC Channels";
            ((System.ComponentModel.ISupportInitialize)_nudActivityTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudScriptTimeout).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudTNCPort).EndInit();
            _grpRadioControl.ResumeLayout(false);
            _grpRadioControl.PerformLayout();
            _grpPTCLevels.ResumeLayout(false);
            _grpPTCLevels.PerformLayout();
            Load += new EventHandler(PacketTNCChannels_Load);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label _Label8;

        internal Label Label8
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label8;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label8 != null)
                {
                }

                _Label8 = value;
                if (_Label8 != null)
                {
                }
            }
        }

        private Label _Label7;

        internal Label Label7
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label7;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label7 != null)
                {
                }

                _Label7 = value;
                if (_Label7 != null)
                {
                }
            }
        }

        private TextBox _txtScript;

        internal TextBox txtScript
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtScript;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtScript != null)
                {
                }

                _txtScript = value;
                if (_txtScript != null)
                {
                }
            }
        }

        private Label _Label6;

        internal Label Label6
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label6;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label6 != null)
                {
                }

                _Label6 = value;
                if (_Label6 != null)
                {
                }
            }
        }

        private Label _Label4;

        internal Label Label4
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label4;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label4 != null)
                {
                }

                _Label4 = value;
                if (_Label4 != null)
                {
                }
            }
        }

        private CheckBox _chkEnabled;

        internal CheckBox chkEnabled
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkEnabled;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkEnabled != null)
                {
                }

                _chkEnabled = value;
                if (_chkEnabled != null)
                {
                }
            }
        }

        private ComboBox _cmbChannelName;

        internal ComboBox cmbChannelName
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbChannelName;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbChannelName != null)
                {
                    _cmbChannelName.SelectedIndexChanged -= cmbChannelName_SelectedIndexChanged;
                    _cmbChannelName.TextChanged -= cmbChannelName_TextChanged;
                }

                _cmbChannelName = value;
                if (_cmbChannelName != null)
                {
                    _cmbChannelName.SelectedIndexChanged += cmbChannelName_SelectedIndexChanged;
                    _cmbChannelName.TextChanged += cmbChannelName_TextChanged;
                }
            }
        }

        private Label _Label3;

        internal Label Label3
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label3;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label3 != null)
                {
                }

                _Label3 = value;
                if (_Label3 != null)
                {
                }
            }
        }

        private Label _Label2;

        internal Label Label2
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label2;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label2 != null)
                {
                }

                _Label2 = value;
                if (_Label2 != null)
                {
                }
            }
        }

        private Label _Label1;

        internal Label Label1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label1 != null)
                {
                }

                _Label1 = value;
                if (_Label1 != null)
                {
                }
            }
        }

        private Button _btnClose;

        internal Button btnClose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnClose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnClose != null)
                {
                    _btnClose.Click -= btnClose_Click;
                }

                _btnClose = value;
                if (_btnClose != null)
                {
                    _btnClose.Click += btnClose_Click;
                }
            }
        }

        private Button _btnUpdate;

        internal Button btnUpdate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdate != null)
                {
                    _btnUpdate.Click -= btnUpdate_Click;
                }

                _btnUpdate = value;
                if (_btnUpdate != null)
                {
                    _btnUpdate.Click += btnUpdate_Click;
                }
            }
        }

        private Button _btnRemove;

        internal Button btnRemove
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnRemove;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnRemove != null)
                {
                    _btnRemove.Click -= btnRemove_Click;
                }

                _btnRemove = value;
                if (_btnRemove != null)
                {
                    _btnRemove.Click += btnRemove_Click;
                }
            }
        }

        private Button _btnAdd;

        internal Button btnAdd
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAdd;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAdd != null)
                {
                    _btnAdd.Click -= btnAdd_Click;
                }

                _btnAdd = value;
                if (_btnAdd != null)
                {
                    _btnAdd.Click += btnAdd_Click;
                }
            }
        }

        private Label _Label14;

        internal Label Label14
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label14;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label14 != null)
                {
                }

                _Label14 = value;
                if (_Label14 != null)
                {
                }
            }
        }

        private ComboBox _cmbTNCType;

        internal ComboBox cmbTNCType
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbTNCType;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbTNCType != null)
                {
                    _cmbTNCType.SelectedIndexChanged -= cmbTNCtype_SelectedIndexChanged;
                    _cmbTNCType.TextChanged -= cmbTNCtype_TextChanged;
                }

                _cmbTNCType = value;
                if (_cmbTNCType != null)
                {
                    _cmbTNCType.SelectedIndexChanged += cmbTNCtype_SelectedIndexChanged;
                    _cmbTNCType.TextChanged += cmbTNCtype_TextChanged;
                }
            }
        }

        private Label _Label22;

        internal Label Label22
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label22;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label22 != null)
                {
                }

                _Label22 = value;
                if (_Label22 != null)
                {
                }
            }
        }

        private ComboBox _cmbTNCSerialPort;

        internal ComboBox cmbTNCSerialPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbTNCSerialPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbTNCSerialPort != null)
                {
                }

                _cmbTNCSerialPort = value;
                if (_cmbTNCSerialPort != null)
                {
                }
            }
        }

        private Button _btnBrowseConfiguration;

        internal Button btnBrowseConfiguration
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnBrowseConfiguration;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnBrowseConfiguration != null)
                {
                    _btnBrowseConfiguration.Click -= btnBrowseConfiguration_Click;
                }

                _btnBrowseConfiguration = value;
                if (_btnBrowseConfiguration != null)
                {
                    _btnBrowseConfiguration.Click += btnBrowseConfiguration_Click;
                }
            }
        }

        private TextBox _txtTNCConfigurationFile;

        internal TextBox txtTNCConfigurationFile
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtTNCConfigurationFile;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtTNCConfigurationFile != null)
                {
                }

                _txtTNCConfigurationFile = value;
                if (_txtTNCConfigurationFile != null)
                {
                }
            }
        }

        private Label _Label18;

        internal Label Label18
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label18;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label18 != null)
                {
                }

                _Label18 = value;
                if (_Label18 != null)
                {
                }
            }
        }

        private ComboBox _cmbTNCBaudRate;

        internal ComboBox cmbTNCBaudRate
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbTNCBaudRate;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbTNCBaudRate != null)
                {
                }

                _cmbTNCBaudRate = value;
                if (_cmbTNCBaudRate != null)
                {
                }
            }
        }

        private Label _Label21;

        internal Label Label21
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label21;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label21 != null)
                {
                }

                _Label21 = value;
                if (_Label21 != null)
                {
                }
            }
        }

        private ToolTip _ToolTip1;

        internal ToolTip ToolTip1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _ToolTip1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_ToolTip1 != null)
                {
                }

                _ToolTip1 = value;
                if (_ToolTip1 != null)
                {
                }
            }
        }

        private NumericUpDown _nudTNCPort;

        internal NumericUpDown nudTNCPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudTNCPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudTNCPort != null)
                {
                }

                _nudTNCPort = value;
                if (_nudTNCPort != null)
                {
                }
            }
        }

        private Label _lblTNCPort;

        internal Label lblTNCPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblTNCPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblTNCPort != null)
                {
                }

                _lblTNCPort = value;
                if (_lblTNCPort != null)
                {
                }
            }
        }

        private NumericUpDown _nudActivityTimeout;

        internal NumericUpDown nudActivityTimeout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudActivityTimeout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudActivityTimeout != null)
                {
                }

                _nudActivityTimeout = value;
                if (_nudActivityTimeout != null)
                {
                }
            }
        }

        private NumericUpDown _nudScriptTimeout;

        internal NumericUpDown nudScriptTimeout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudScriptTimeout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudScriptTimeout != null)
                {
                }

                _nudScriptTimeout = value;
                if (_nudScriptTimeout != null)
                {
                }
            }
        }

        private NumericUpDown _nudPriority;

        internal NumericUpDown nudPriority
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudPriority;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudPriority != null)
                {
                }

                _nudPriority = value;
                if (_nudPriority != null)
                {
                }
            }
        }

        private Button _btnHelp;

        internal Button btnHelp
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnHelp;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnHelp != null)
                {
                    _btnHelp.Click -= btnHelp_Click;
                }

                _btnHelp = value;
                if (_btnHelp != null)
                {
                    _btnHelp.Click += btnHelp_Click;
                }
            }
        }

        private CheckBox _chkFirstUseOnly;

        internal CheckBox chkFirstUseOnly
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkFirstUseOnly;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkFirstUseOnly != null)
                {
                }

                _chkFirstUseOnly = value;
                if (_chkFirstUseOnly != null)
                {
                }
            }
        }

        private Label _Label9;

        internal Label Label9
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label9;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label9 != null)
                {
                }

                _Label9 = value;
                if (_Label9 != null)
                {
                }
            }
        }

        private GroupBox _grpRadioControl;

        internal GroupBox grpRadioControl
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _grpRadioControl;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_grpRadioControl != null)
                {
                }

                _grpRadioControl = value;
                if (_grpRadioControl != null)
                {
                }
            }
        }

        private GroupBox _grpPTCLevels;

        internal GroupBox grpPTCLevels
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _grpPTCLevels;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_grpPTCLevels != null)
                {
                }

                _grpPTCLevels = value;
                if (_grpPTCLevels != null)
                {
                }
            }
        }

        private RadioButton _rdoV24;

        internal RadioButton rdoV24
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoV24;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoV24 != null)
                {
                }

                _rdoV24 = value;
                if (_rdoV24 != null)
                {
                }
            }
        }

        private RadioButton _rdoTTL;

        internal RadioButton rdoTTL
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoTTL;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoTTL != null)
                {
                }

                _rdoTTL = value;
                if (_rdoTTL != null)
                {
                }
            }
        }

        private Label _Label15;

        internal Label Label15
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label15;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label15 != null)
                {
                }

                _Label15 = value;
                if (_Label15 != null)
                {
                }
            }
        }

        private RadioButton _rdoViaPTCII;

        internal RadioButton rdoViaPTCII
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoViaPTCII;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoViaPTCII != null)
                {
                    _rdoViaPTCII.CheckedChanged -= rdoViaPTCII_CheckedChanged;
                }

                _rdoViaPTCII = value;
                if (_rdoViaPTCII != null)
                {
                    _rdoViaPTCII.CheckedChanged += rdoViaPTCII_CheckedChanged;
                }
            }
        }

        private Label _lblRadioAddress;

        internal Label lblRadioAddress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblRadioAddress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblRadioAddress != null)
                {
                }

                _lblRadioAddress = value;
                if (_lblRadioAddress != null)
                {
                }
            }
        }

        private TextBox _txtRadioAdd;

        internal TextBox txtRadioAdd
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtRadioAdd;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtRadioAdd != null)
                {
                }

                _txtRadioAdd = value;
                if (_txtRadioAdd != null)
                {
                }
            }
        }

        private Label _Label10;

        internal Label Label10
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label10;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label10 != null)
                {
                }

                _Label10 = value;
                if (_Label10 != null)
                {
                }
            }
        }

        private ComboBox _cmbRadioModel;

        internal ComboBox cmbRadioModel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbRadioModel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbRadioModel != null)
                {
                    _cmbRadioModel.TextChanged -= cmbRadioModel_TextChanged;
                }

                _cmbRadioModel = value;
                if (_cmbRadioModel != null)
                {
                    _cmbRadioModel.TextChanged += cmbRadioModel_TextChanged;
                }
            }
        }

        private Label _Label16;

        internal Label Label16
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label16;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label16 != null)
                {
                }

                _Label16 = value;
                if (_Label16 != null)
                {
                }
            }
        }

        private RadioButton _rdoSerial;

        internal RadioButton rdoSerial
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoSerial;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoSerial != null)
                {
                    _rdoSerial.CheckedChanged -= rdoSerial_CheckedChanged;
                }

                _rdoSerial = value;
                if (_rdoSerial != null)
                {
                    _rdoSerial.CheckedChanged += rdoSerial_CheckedChanged;
                }
            }
        }

        private RadioButton _rdoManual;

        internal RadioButton rdoManual
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoManual;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoManual != null)
                {
                    _rdoManual.CheckedChanged -= rdoManual_CheckedChanged;
                }

                _rdoManual = value;
                if (_rdoManual != null)
                {
                    _rdoManual.CheckedChanged += rdoManual_CheckedChanged;
                }
            }
        }

        private ComboBox _cmbRadioPort;

        internal ComboBox cmbRadioPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbRadioPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbRadioPort != null)
                {
                }

                _cmbRadioPort = value;
                if (_cmbRadioPort != null)
                {
                }
            }
        }

        private ComboBox _cmbRadioBaud;

        internal ComboBox cmbRadioBaud
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbRadioBaud;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbRadioBaud != null)
                {
                }

                _cmbRadioBaud = value;
                if (_cmbRadioBaud != null)
                {
                }
            }
        }

        private Label _Label11;

        internal Label Label11
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label11;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label11 != null)
                {
                }

                _Label11 = value;
                if (_Label11 != null)
                {
                }
            }
        }

        private TextBox _txtFreqMHz;

        internal TextBox txtFreqMHz
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtFreqMHz;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtFreqMHz != null)
                {
                    _txtFreqMHz.TextChanged -= txtFreqMHz_TextChanged;
                }

                _txtFreqMHz = value;
                if (_txtFreqMHz != null)
                {
                    _txtFreqMHz.TextChanged += txtFreqMHz_TextChanged;
                }
            }
        }

        private ComboBox _cmbOnAirBaud;

        internal ComboBox cmbOnAirBaud
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbOnAirBaud;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbOnAirBaud != null)
                {
                    _cmbOnAirBaud.SelectedIndexChanged -= cmbOnAirBaud_SelectedIndexChanged;
                }

                _cmbOnAirBaud = value;
                if (_cmbOnAirBaud != null)
                {
                    _cmbOnAirBaud.SelectedIndexChanged += cmbOnAirBaud_SelectedIndexChanged;
                }
            }
        }

        private Label _Label12;

        internal Label Label12
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label12;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label12 != null)
                {
                }

                _Label12 = value;
                if (_Label12 != null)
                {
                }
            }
        }

        private Button _btnUpdateChannelList;

        internal Button btnUpdateChannelList
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnUpdateChannelList;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnUpdateChannelList != null)
                {
                    _btnUpdateChannelList.Click -= btnUpdateChannelList_Click;
                }

                _btnUpdateChannelList = value;
                if (_btnUpdateChannelList != null)
                {
                    _btnUpdateChannelList.Click += btnUpdateChannelList_Click;
                }
            }
        }

        private ComboBox _cmbRemoteCallsign;

        internal ComboBox cmbRemoteCallsign
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbRemoteCallsign;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbRemoteCallsign != null)
                {
                    _cmbRemoteCallsign.SelectedIndexChanged -= cmbRemoteCallsign_SelectedIndexChanged;
                }

                _cmbRemoteCallsign = value;
                if (_cmbRemoteCallsign != null)
                {
                    _cmbRemoteCallsign.SelectedIndexChanged += cmbRemoteCallsign_SelectedIndexChanged;
                }
            }
        }

        private Label _lblFrequency;

        internal Label lblFrequency
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblFrequency;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblFrequency != null)
                {
                }

                _lblFrequency = value;
                if (_lblFrequency != null)
                {
                }
            }
        }

        private ComboBox _cmbFreqs;

        internal ComboBox cmbFreqs
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbFreqs;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbFreqs != null)
                {
                    _cmbFreqs.SelectedIndexChanged -= cmbFreqs_SelectedIndexChanged;
                }

                _cmbFreqs = value;
                if (_cmbFreqs != null)
                {
                    _cmbFreqs.SelectedIndexChanged += cmbFreqs_SelectedIndexChanged;
                }
            }
        }
    }
}