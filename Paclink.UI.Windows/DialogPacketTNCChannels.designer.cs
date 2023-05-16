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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPacketTNCChannels));
            this._Label8 = new System.Windows.Forms.Label();
            this._Label7 = new System.Windows.Forms.Label();
            this._txtScript = new System.Windows.Forms.TextBox();
            this._Label6 = new System.Windows.Forms.Label();
            this._Label4 = new System.Windows.Forms.Label();
            this._chkEnabled = new System.Windows.Forms.CheckBox();
            this._cmbChannelName = new System.Windows.Forms.ComboBox();
            this._Label3 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label1 = new System.Windows.Forms.Label();
            this._btnClose = new System.Windows.Forms.Button();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._Label14 = new System.Windows.Forms.Label();
            this._cmbTNCType = new System.Windows.Forms.ComboBox();
            this._Label22 = new System.Windows.Forms.Label();
            this._cmbTNCSerialPort = new System.Windows.Forms.ComboBox();
            this._btnBrowseConfiguration = new System.Windows.Forms.Button();
            this._txtTNCConfigurationFile = new System.Windows.Forms.TextBox();
            this._Label18 = new System.Windows.Forms.Label();
            this._cmbTNCBaudRate = new System.Windows.Forms.ComboBox();
            this._Label21 = new System.Windows.Forms.Label();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._nudActivityTimeout = new System.Windows.Forms.NumericUpDown();
            this._nudScriptTimeout = new System.Windows.Forms.NumericUpDown();
            this._nudPriority = new System.Windows.Forms.NumericUpDown();
            this._chkFirstUseOnly = new System.Windows.Forms.CheckBox();
            this._rdoV24 = new System.Windows.Forms.RadioButton();
            this._rdoTTL = new System.Windows.Forms.RadioButton();
            this._rdoViaPTCII = new System.Windows.Forms.RadioButton();
            this._txtRadioAdd = new System.Windows.Forms.TextBox();
            this._cmbRadioModel = new System.Windows.Forms.ComboBox();
            this._rdoSerial = new System.Windows.Forms.RadioButton();
            this._rdoManual = new System.Windows.Forms.RadioButton();
            this._cmbRadioPort = new System.Windows.Forms.ComboBox();
            this._cmbRadioBaud = new System.Windows.Forms.ComboBox();
            this._txtFreqMHz = new System.Windows.Forms.TextBox();
            this._cmbOnAirBaud = new System.Windows.Forms.ComboBox();
            this._cmbFreqs = new System.Windows.Forms.ComboBox();
            this._nudTNCPort = new System.Windows.Forms.NumericUpDown();
            this._lblTNCPort = new System.Windows.Forms.Label();
            this._btnHelp = new System.Windows.Forms.Button();
            this._Label9 = new System.Windows.Forms.Label();
            this._grpRadioControl = new System.Windows.Forms.GroupBox();
            this._Label11 = new System.Windows.Forms.Label();
            this._grpPTCLevels = new System.Windows.Forms.GroupBox();
            this._Label15 = new System.Windows.Forms.Label();
            this._lblRadioAddress = new System.Windows.Forms.Label();
            this._Label10 = new System.Windows.Forms.Label();
            this._Label16 = new System.Windows.Forms.Label();
            this._Label12 = new System.Windows.Forms.Label();
            this._btnUpdateChannelList = new System.Windows.Forms.Button();
            this._cmbRemoteCallsign = new System.Windows.Forms.ComboBox();
            this._lblFrequency = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._nudActivityTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudScriptTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudTNCPort)).BeginInit();
            this._grpRadioControl.SuspendLayout();
            this._grpPTCLevels.SuspendLayout();
            this.SuspendLayout();
            // 
            // _Label8
            // 
            this._Label8.AutoSize = true;
            this._Label8.CausesValidation = false;
            this._Label8.Location = new System.Drawing.Point(858, 240);
            this._Label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label8.Name = "_Label8";
            this._Label8.Size = new System.Drawing.Size(50, 15);
            this._Label8.TabIndex = 118;
            this._Label8.Text = "seconds";
            // 
            // _Label7
            // 
            this._Label7.AutoSize = true;
            this._Label7.CausesValidation = false;
            this._Label7.Location = new System.Drawing.Point(657, 240);
            this._Label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label7.Name = "_Label7";
            this._Label7.Size = new System.Drawing.Size(136, 15);
            this._Label7.TabIndex = 117;
            this._Label7.Text = "Script inactivity timeout:";
            // 
            // _txtScript
            // 
            this._txtScript.CausesValidation = false;
            this._txtScript.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtScript.Location = new System.Drawing.Point(659, 90);
            this._txtScript.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtScript.Multiline = true;
            this._txtScript.Name = "_txtScript";
            this._txtScript.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._txtScript.Size = new System.Drawing.Size(269, 138);
            this._txtScript.TabIndex = 13;
            this._ToolTip1.SetToolTip(this._txtScript, "Enter optional connect scrip. One line for simple vias or multiline.");
            // 
            // _Label6
            // 
            this._Label6.AutoSize = true;
            this._Label6.CausesValidation = false;
            this._Label6.Location = new System.Drawing.Point(712, 72);
            this._Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label6.Name = "_Label6";
            this._Label6.Size = new System.Drawing.Size(134, 15);
            this._Label6.TabIndex = 116;
            this._Label6.Text = "Optional connect script:";
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(418, 149);
            this._Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(162, 15);
            this._Label4.TabIndex = 111;
            this._Label4.Text = "Activity timeout (in minutes):";
            // 
            // _chkEnabled
            // 
            this._chkEnabled.AutoSize = true;
            this._chkEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkEnabled.Location = new System.Drawing.Point(432, 107);
            this._chkEnabled.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkEnabled.Name = "_chkEnabled";
            this._chkEnabled.Size = new System.Drawing.Size(118, 19);
            this._chkEnabled.TabIndex = 9;
            this._chkEnabled.Text = "Channel enabled:";
            this._ToolTip1.SetToolTip(this._chkEnabled, "Enable this channel ");
            this._chkEnabled.UseVisualStyleBackColor = true;
            // 
            // _cmbChannelName
            // 
            this._cmbChannelName.FormattingEnabled = true;
            this._cmbChannelName.Location = new System.Drawing.Point(130, 38);
            this._cmbChannelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbChannelName.MaxDropDownItems = 24;
            this._cmbChannelName.Name = "_cmbChannelName";
            this._cmbChannelName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmbChannelName.Size = new System.Drawing.Size(221, 23);
            this._cmbChannelName.Sorted = true;
            this._cmbChannelName.TabIndex = 0;
            this._ToolTip1.SetToolTip(this._cmbChannelName, "Enter Channel Name");
            this._cmbChannelName.SelectedIndexChanged += new System.EventHandler(this.cmbChannelName_SelectedIndexChanged);
            this._cmbChannelName.TextChanged += new System.EventHandler(this.cmbChannelName_TextChanged);
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label3.Location = new System.Drawing.Point(377, 42);
            this._Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(101, 13);
            this._Label3.TabIndex = 110;
            this._Label3.Text = "Remote callsign:";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(19, 77);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(95, 15);
            this._Label2.TabIndex = 109;
            this._Label2.Text = "Channel priority:";
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label1.Location = new System.Drawing.Point(19, 42);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(91, 13);
            this._Label1.TabIndex = 108;
            this._Label1.Text = "Channel name:";
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(552, 434);
            this._btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(150, 35);
            this._btnClose.TabIndex = 22;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Location = new System.Drawing.Point(392, 434);
            this._btnUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size(150, 35);
            this._btnUpdate.TabIndex = 21;
            this._btnUpdate.Text = "Update The Channel";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point(234, 434);
            this._btnRemove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(150, 35);
            this._btnRemove.TabIndex = 20;
            this._btnRemove.Text = "Remove This Channel";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(71, 434);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(150, 35);
            this._btnAdd.TabIndex = 19;
            this._btnAdd.Text = "Add New Channel";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // _Label14
            // 
            this._Label14.AutoSize = true;
            this._Label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._Label14.Location = new System.Drawing.Point(178, 77);
            this._Label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label14.Name = "_Label14";
            this._Label14.Size = new System.Drawing.Size(59, 13);
            this._Label14.TabIndex = 122;
            this._Label14.Text = "TNC Type:";
            // 
            // _cmbTNCType
            // 
            this._cmbTNCType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTNCType.FormattingEnabled = true;
            this._cmbTNCType.Location = new System.Drawing.Point(254, 73);
            this._cmbTNCType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbTNCType.MaxDropDownItems = 24;
            this._cmbTNCType.Name = "_cmbTNCType";
            this._cmbTNCType.Size = new System.Drawing.Size(143, 23);
            this._cmbTNCType.TabIndex = 2;
            this._ToolTip1.SetToolTip(this._cmbTNCType, "Select TNC type");
            this._cmbTNCType.SelectedIndexChanged += new System.EventHandler(this.cmbTNCtype_SelectedIndexChanged);
            this._cmbTNCType.TextChanged += new System.EventHandler(this.cmbTNCtype_TextChanged);
            // 
            // _Label22
            // 
            this._Label22.AutoSize = true;
            this._Label22.Location = new System.Drawing.Point(22, 149);
            this._Label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label22.Name = "_Label22";
            this._Label22.Size = new System.Drawing.Size(63, 15);
            this._Label22.TabIndex = 125;
            this._Label22.Text = "Serial port:";
            // 
            // _cmbTNCSerialPort
            // 
            this._cmbTNCSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTNCSerialPort.FormattingEnabled = true;
            this._cmbTNCSerialPort.Location = new System.Drawing.Point(96, 145);
            this._cmbTNCSerialPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbTNCSerialPort.Name = "_cmbTNCSerialPort";
            this._cmbTNCSerialPort.Size = new System.Drawing.Size(92, 23);
            this._cmbTNCSerialPort.Sorted = true;
            this._cmbTNCSerialPort.TabIndex = 5;
            this._ToolTip1.SetToolTip(this._cmbTNCSerialPort, "Select TNC serial port ");
            // 
            // _btnBrowseConfiguration
            // 
            this._btnBrowseConfiguration.Location = new System.Drawing.Point(584, 231);
            this._btnBrowseConfiguration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnBrowseConfiguration.Name = "_btnBrowseConfiguration";
            this._btnBrowseConfiguration.Size = new System.Drawing.Size(59, 27);
            this._btnBrowseConfiguration.TabIndex = 16;
            this._btnBrowseConfiguration.Text = "Browse";
            this._btnBrowseConfiguration.UseVisualStyleBackColor = true;
            this._btnBrowseConfiguration.Click += new System.EventHandler(this.btnBrowseConfiguration_Click);
            // 
            // _txtTNCConfigurationFile
            // 
            this._txtTNCConfigurationFile.Location = new System.Drawing.Point(161, 233);
            this._txtTNCConfigurationFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtTNCConfigurationFile.Name = "_txtTNCConfigurationFile";
            this._txtTNCConfigurationFile.Size = new System.Drawing.Size(416, 23);
            this._txtTNCConfigurationFile.TabIndex = 15;
            this._txtTNCConfigurationFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtTNCConfigurationFile, "The TNC configuration (.aps) file. (see Examples for templates) ");
            // 
            // _Label18
            // 
            this._Label18.AutoSize = true;
            this._Label18.Location = new System.Drawing.Point(19, 237);
            this._Label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label18.Name = "_Label18";
            this._Label18.Size = new System.Drawing.Size(131, 15);
            this._Label18.TabIndex = 136;
            this._Label18.Text = "TNC Configuration File:";
            // 
            // _cmbTNCBaudRate
            // 
            this._cmbTNCBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTNCBaudRate.FormattingEnabled = true;
            this._cmbTNCBaudRate.Location = new System.Drawing.Point(293, 145);
            this._cmbTNCBaudRate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbTNCBaudRate.Name = "_cmbTNCBaudRate";
            this._cmbTNCBaudRate.Size = new System.Drawing.Size(92, 23);
            this._cmbTNCBaudRate.TabIndex = 7;
            this._ToolTip1.SetToolTip(this._cmbTNCBaudRate, "Select TNC baud rate (not on air baud rate) ");
            // 
            // _Label21
            // 
            this._Label21.AutoSize = true;
            this._Label21.Location = new System.Drawing.Point(215, 149);
            this._Label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label21.Name = "_Label21";
            this._Label21.Size = new System.Drawing.Size(63, 15);
            this._Label21.TabIndex = 126;
            this._Label21.Text = "Baud Rate:";
            // 
            // _nudActivityTimeout
            // 
            this._nudActivityTimeout.Location = new System.Drawing.Point(584, 147);
            this._nudActivityTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudActivityTimeout.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this._nudActivityTimeout.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nudActivityTimeout.Name = "_nudActivityTimeout";
            this._nudActivityTimeout.Size = new System.Drawing.Size(43, 23);
            this._nudActivityTimeout.TabIndex = 6;
            this._nudActivityTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudActivityTimeout, "Select maximum inactivity time before auto disconnect");
            this._nudActivityTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // _nudScriptTimeout
            // 
            this._nudScriptTimeout.Location = new System.Drawing.Point(802, 238);
            this._nudScriptTimeout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudScriptTimeout.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this._nudScriptTimeout.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this._nudScriptTimeout.Name = "_nudScriptTimeout";
            this._nudScriptTimeout.Size = new System.Drawing.Size(49, 23);
            this._nudScriptTimeout.TabIndex = 14;
            this._nudScriptTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudScriptTimeout, "Timeout for each script line");
            this._nudScriptTimeout.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // _nudPriority
            // 
            this._nudPriority.Location = new System.Drawing.Point(121, 73);
            this._nudPriority.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudPriority.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._nudPriority.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudPriority.Name = "_nudPriority";
            this._nudPriority.Size = new System.Drawing.Size(43, 23);
            this._nudPriority.TabIndex = 4;
            this._nudPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudPriority, "Select priority 1-5, 1=highest (default=3)");
            this._nudPriority.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _chkFirstUseOnly
            // 
            this._chkFirstUseOnly.AutoSize = true;
            this._chkFirstUseOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkFirstUseOnly.Location = new System.Drawing.Point(93, 107);
            this._chkFirstUseOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkFirstUseOnly.Name = "_chkFirstUseOnly";
            this._chkFirstUseOnly.Size = new System.Drawing.Size(261, 19);
            this._chkFirstUseOnly.TabIndex = 12;
            this._chkFirstUseOnly.Text = "Do a full TNC configuration only on first use:";
            this._ToolTip1.SetToolTip(this._chkFirstUseOnly, " ");
            this._chkFirstUseOnly.UseVisualStyleBackColor = true;
            // 
            // _rdoV24
            // 
            this._rdoV24.AutoSize = true;
            this._rdoV24.Location = new System.Drawing.Point(78, 22);
            this._rdoV24.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoV24.Name = "_rdoV24";
            this._rdoV24.Size = new System.Drawing.Size(61, 19);
            this._rdoV24.TabIndex = 1;
            this._rdoV24.TabStop = true;
            this._rdoV24.Text = "RS-232";
            this._ToolTip1.SetToolTip(this._rdoV24, "Select TTL or RS232 Levels for PTCIIpro and PTCIIusb models");
            this._rdoV24.UseVisualStyleBackColor = true;
            // 
            // _rdoTTL
            // 
            this._rdoTTL.AutoSize = true;
            this._rdoTTL.Location = new System.Drawing.Point(14, 22);
            this._rdoTTL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoTTL.Name = "_rdoTTL";
            this._rdoTTL.Size = new System.Drawing.Size(43, 19);
            this._rdoTTL.TabIndex = 0;
            this._rdoTTL.TabStop = true;
            this._rdoTTL.Text = "TTL";
            this._ToolTip1.SetToolTip(this._rdoTTL, "Select TTL or RS232 Levels for PTCIIpro and PTCIIusb models");
            this._rdoTTL.UseVisualStyleBackColor = true;
            // 
            // _rdoViaPTCII
            // 
            this._rdoViaPTCII.AutoSize = true;
            this._rdoViaPTCII.Location = new System.Drawing.Point(64, 48);
            this._rdoViaPTCII.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoViaPTCII.Name = "_rdoViaPTCII";
            this._rdoViaPTCII.Size = new System.Drawing.Size(134, 19);
            this._rdoViaPTCII.TabIndex = 1;
            this._rdoViaPTCII.TabStop = true;
            this._rdoViaPTCII.Text = "Via PTC II, IIpro, IIusb";
            this._ToolTip1.SetToolTip(this._rdoViaPTCII, "Use Radio control via a PTC II, IIpro or IIusb (not available in  IIe or IIex mod" +
        "els)");
            this._rdoViaPTCII.UseVisualStyleBackColor = true;
            this._rdoViaPTCII.CheckedChanged += new System.EventHandler(this.rdoViaPTCII_CheckedChanged);
            // 
            // _txtRadioAdd
            // 
            this._txtRadioAdd.Enabled = false;
            this._txtRadioAdd.Location = new System.Drawing.Point(727, 22);
            this._txtRadioAdd.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtRadioAdd.Name = "_txtRadioAdd";
            this._txtRadioAdd.Size = new System.Drawing.Size(28, 23);
            this._txtRadioAdd.TabIndex = 6;
            this._txtRadioAdd.Text = "01";
            this._txtRadioAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtRadioAdd, "Hex Address for Icom C-IV Radios");
            // 
            // _cmbRadioModel
            // 
            this._cmbRadioModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRadioModel.FormattingEnabled = true;
            this._cmbRadioModel.Location = new System.Drawing.Point(429, 22);
            this._cmbRadioModel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbRadioModel.Name = "_cmbRadioModel";
            this._cmbRadioModel.Size = new System.Drawing.Size(139, 23);
            this._cmbRadioModel.TabIndex = 5;
            this._ToolTip1.SetToolTip(this._cmbRadioModel, "Select Radio type here.");
            this._cmbRadioModel.TextChanged += new System.EventHandler(this.cmbRadioModel_TextChanged);
            // 
            // _rdoSerial
            // 
            this._rdoSerial.AutoSize = true;
            this._rdoSerial.Location = new System.Drawing.Point(64, 129);
            this._rdoSerial.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoSerial.Name = "_rdoSerial";
            this._rdoSerial.Size = new System.Drawing.Size(133, 19);
            this._rdoSerial.TabIndex = 2;
            this._rdoSerial.Text = "Direct via Serial Port:";
            this._ToolTip1.SetToolTip(this._rdoSerial, "Select this if the Radio control is direct using a Serial or USB port.");
            this._rdoSerial.UseVisualStyleBackColor = true;
            this._rdoSerial.CheckedChanged += new System.EventHandler(this.rdoSerial_CheckedChanged);
            // 
            // _rdoManual
            // 
            this._rdoManual.AutoSize = true;
            this._rdoManual.Checked = true;
            this._rdoManual.Location = new System.Drawing.Point(64, 28);
            this._rdoManual.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoManual.Name = "_rdoManual";
            this._rdoManual.Size = new System.Drawing.Size(103, 19);
            this._rdoManual.TabIndex = 0;
            this._rdoManual.TabStop = true;
            this._rdoManual.Text = "Manual (none)";
            this._ToolTip1.SetToolTip(this._rdoManual, "Select if manual radio control.");
            this._rdoManual.UseVisualStyleBackColor = true;
            this._rdoManual.CheckedChanged += new System.EventHandler(this.rdoManual_CheckedChanged);
            // 
            // _cmbRadioPort
            // 
            this._cmbRadioPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRadioPort.FormattingEnabled = true;
            this._cmbRadioPort.Location = new System.Drawing.Point(429, 87);
            this._cmbRadioPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbRadioPort.Name = "_cmbRadioPort";
            this._cmbRadioPort.Size = new System.Drawing.Size(79, 23);
            this._cmbRadioPort.TabIndex = 3;
            this._ToolTip1.SetToolTip(this._cmbRadioPort, "If you selected Direct via Serial this is where you select the radio control port" +
        "");
            // 
            // _cmbRadioBaud
            // 
            this._cmbRadioBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRadioBaud.FormattingEnabled = true;
            this._cmbRadioBaud.Location = new System.Drawing.Point(429, 55);
            this._cmbRadioBaud.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbRadioBaud.Name = "_cmbRadioBaud";
            this._cmbRadioBaud.Size = new System.Drawing.Size(80, 23);
            this._cmbRadioBaud.TabIndex = 4;
            this._ToolTip1.SetToolTip(this._cmbRadioBaud, "Baud rate for the radio control port (8N1 assumed) ");
            // 
            // _txtFreqMHz
            // 
            this._txtFreqMHz.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtFreqMHz.Location = new System.Drawing.Point(429, 120);
            this._txtFreqMHz.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtFreqMHz.Name = "_txtFreqMHz";
            this._txtFreqMHz.Size = new System.Drawing.Size(108, 23);
            this._txtFreqMHz.TabIndex = 171;
            this._txtFreqMHz.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtFreqMHz, "Enter the remote target callsign");
            this._txtFreqMHz.TextChanged += new System.EventHandler(this.txtFreqMHz_TextChanged);
            // 
            // _cmbOnAirBaud
            // 
            this._cmbOnAirBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbOnAirBaud.FormattingEnabled = true;
            this._cmbOnAirBaud.Location = new System.Drawing.Point(544, 73);
            this._cmbOnAirBaud.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbOnAirBaud.Name = "_cmbOnAirBaud";
            this._cmbOnAirBaud.Size = new System.Drawing.Size(68, 23);
            this._cmbOnAirBaud.TabIndex = 146;
            this._ToolTip1.SetToolTip(this._cmbOnAirBaud, "Baud rate for the radio control port (8N1 assumed) ");
            this._cmbOnAirBaud.SelectedIndexChanged += new System.EventHandler(this.cmbOnAirBaud_SelectedIndexChanged);
            // 
            // _cmbFreqs
            // 
            this._cmbFreqs.FormattingEnabled = true;
            this._cmbFreqs.Location = new System.Drawing.Point(754, 38);
            this._cmbFreqs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbFreqs.Name = "_cmbFreqs";
            this._cmbFreqs.Size = new System.Drawing.Size(144, 23);
            this._cmbFreqs.TabIndex = 151;
            this._ToolTip1.SetToolTip(this._cmbFreqs, "Frequency and baud rate");
            this._cmbFreqs.SelectedIndexChanged += new System.EventHandler(this.cmbFreqs_SelectedIndexChanged);
            // 
            // _nudTNCPort
            // 
            this._nudTNCPort.Location = new System.Drawing.Point(664, 82);
            this._nudTNCPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._nudTNCPort.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this._nudTNCPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudTNCPort.Name = "_nudTNCPort";
            this._nudTNCPort.Size = new System.Drawing.Size(42, 23);
            this._nudTNCPort.TabIndex = 3;
            this._nudTNCPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._nudTNCPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _lblTNCPort
            // 
            this._lblTNCPort.AutoSize = true;
            this._lblTNCPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._lblTNCPort.Location = new System.Drawing.Point(594, 84);
            this._lblTNCPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblTNCPort.Name = "_lblTNCPort";
            this._lblTNCPort.Size = new System.Drawing.Size(54, 13);
            this._lblTNCPort.TabIndex = 143;
            this._lblTNCPort.Text = "TNC Port:";
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(709, 434);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(150, 35);
            this._btnHelp.TabIndex = 23;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // _Label9
            // 
            this._Label9.AutoSize = true;
            this._Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label9.Location = new System.Drawing.Point(186, 10);
            this._Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label9.Name = "_Label9";
            this._Label9.Size = new System.Drawing.Size(481, 13);
            this._Label9.TabIndex = 144;
            this._Label9.Text = "To create a new channel type a new channel name in the Channel Name text box...";
            // 
            // _grpRadioControl
            // 
            this._grpRadioControl.Controls.Add(this._Label11);
            this._grpRadioControl.Controls.Add(this._txtFreqMHz);
            this._grpRadioControl.Controls.Add(this._grpPTCLevels);
            this._grpRadioControl.Controls.Add(this._Label15);
            this._grpRadioControl.Controls.Add(this._rdoViaPTCII);
            this._grpRadioControl.Controls.Add(this._lblRadioAddress);
            this._grpRadioControl.Controls.Add(this._txtRadioAdd);
            this._grpRadioControl.Controls.Add(this._Label10);
            this._grpRadioControl.Controls.Add(this._cmbRadioModel);
            this._grpRadioControl.Controls.Add(this._lblTNCPort);
            this._grpRadioControl.Controls.Add(this._Label16);
            this._grpRadioControl.Controls.Add(this._nudTNCPort);
            this._grpRadioControl.Controls.Add(this._rdoSerial);
            this._grpRadioControl.Controls.Add(this._rdoManual);
            this._grpRadioControl.Controls.Add(this._cmbRadioPort);
            this._grpRadioControl.Controls.Add(this._cmbRadioBaud);
            this._grpRadioControl.Location = new System.Drawing.Point(29, 268);
            this._grpRadioControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._grpRadioControl.Name = "_grpRadioControl";
            this._grpRadioControl.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._grpRadioControl.Size = new System.Drawing.Size(899, 159);
            this._grpRadioControl.TabIndex = 145;
            this._grpRadioControl.TabStop = false;
            this._grpRadioControl.Text = "Optional VHF/UHF Radio Control";
            // 
            // _Label11
            // 
            this._Label11.AutoSize = true;
            this._Label11.Location = new System.Drawing.Point(265, 123);
            this._Label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label11.Name = "_Label11";
            this._Label11.Size = new System.Drawing.Size(151, 15);
            this._Label11.TabIndex = 172;
            this._Label11.Text = "Channel frequency in MHz:";
            // 
            // _grpPTCLevels
            // 
            this._grpPTCLevels.Controls.Add(this._rdoV24);
            this._grpPTCLevels.Controls.Add(this._rdoTTL);
            this._grpPTCLevels.Location = new System.Drawing.Point(83, 75);
            this._grpPTCLevels.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._grpPTCLevels.Name = "_grpPTCLevels";
            this._grpPTCLevels.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._grpPTCLevels.Size = new System.Drawing.Size(156, 47);
            this._grpPTCLevels.TabIndex = 170;
            this._grpPTCLevels.TabStop = false;
            this._grpPTCLevels.Text = "PTC Levels to Radio";
            // 
            // _Label15
            // 
            this._Label15.AutoSize = true;
            this._Label15.Location = new System.Drawing.Point(355, 90);
            this._Label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label15.Name = "_Label15";
            this._Label15.Size = new System.Drawing.Size(63, 15);
            this._Label15.TabIndex = 168;
            this._Label15.Text = "Serial Port:";
            // 
            // _lblRadioAddress
            // 
            this._lblRadioAddress.AutoSize = true;
            this._lblRadioAddress.Location = new System.Drawing.Point(597, 24);
            this._lblRadioAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblRadioAddress.Name = "_lblRadioAddress";
            this._lblRadioAddress.Size = new System.Drawing.Size(115, 15);
            this._lblRadioAddress.TabIndex = 163;
            this._lblRadioAddress.Text = "Radio Address (hex):";
            // 
            // _Label10
            // 
            this._Label10.AutoSize = true;
            this._Label10.Location = new System.Drawing.Point(341, 25);
            this._Label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label10.Name = "_Label10";
            this._Label10.Size = new System.Drawing.Size(77, 15);
            this._Label10.TabIndex = 12;
            this._Label10.Text = "Radio Model:";
            // 
            // _Label16
            // 
            this._Label16.AutoSize = true;
            this._Label16.Location = new System.Drawing.Point(350, 59);
            this._Label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label16.Name = "_Label16";
            this._Label16.Size = new System.Drawing.Size(63, 15);
            this._Label16.TabIndex = 10;
            this._Label16.Text = "Baud Rate:";
            // 
            // _Label12
            // 
            this._Label12.AutoSize = true;
            this._Label12.Location = new System.Drawing.Point(428, 76);
            this._Label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label12.Name = "_Label12";
            this._Label12.Size = new System.Drawing.Size(102, 15);
            this._Label12.TabIndex = 147;
            this._Label12.Text = "On-Air Baud Rate:";
            // 
            // _btnUpdateChannelList
            // 
            this._btnUpdateChannelList.Location = new System.Drawing.Point(196, 190);
            this._btnUpdateChannelList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnUpdateChannelList.Name = "_btnUpdateChannelList";
            this._btnUpdateChannelList.Size = new System.Drawing.Size(202, 27);
            this._btnUpdateChannelList.TabIndex = 148;
            this._btnUpdateChannelList.Text = "Update Channel List";
            this._btnUpdateChannelList.UseVisualStyleBackColor = true;
            this._btnUpdateChannelList.Click += new System.EventHandler(this.btnUpdateChannelList_Click);
            // 
            // _cmbRemoteCallsign
            // 
            this._cmbRemoteCallsign.FormattingEnabled = true;
            this._cmbRemoteCallsign.Location = new System.Drawing.Point(497, 38);
            this._cmbRemoteCallsign.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbRemoteCallsign.Name = "_cmbRemoteCallsign";
            this._cmbRemoteCallsign.Size = new System.Drawing.Size(135, 23);
            this._cmbRemoteCallsign.TabIndex = 149;
            this._cmbRemoteCallsign.SelectedIndexChanged += new System.EventHandler(this.cmbRemoteCallsign_SelectedIndexChanged);
            // 
            // _lblFrequency
            // 
            this._lblFrequency.AutoSize = true;
            this._lblFrequency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._lblFrequency.Location = new System.Drawing.Point(665, 42);
            this._lblFrequency.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblFrequency.Name = "_lblFrequency";
            this._lblFrequency.Size = new System.Drawing.Size(70, 13);
            this._lblFrequency.TabIndex = 150;
            this._lblFrequency.Text = "Frequency:";
            // 
            // DialogPacketTNCChannels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(950, 494);
            this.Controls.Add(this._cmbFreqs);
            this.Controls.Add(this._lblFrequency);
            this.Controls.Add(this._cmbRemoteCallsign);
            this.Controls.Add(this._btnUpdateChannelList);
            this.Controls.Add(this._Label12);
            this.Controls.Add(this._cmbOnAirBaud);
            this.Controls.Add(this._grpRadioControl);
            this.Controls.Add(this._Label9);
            this.Controls.Add(this._chkFirstUseOnly);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._nudPriority);
            this.Controls.Add(this._nudScriptTimeout);
            this.Controls.Add(this._nudActivityTimeout);
            this.Controls.Add(this._btnBrowseConfiguration);
            this.Controls.Add(this._txtTNCConfigurationFile);
            this.Controls.Add(this._Label18);
            this.Controls.Add(this._cmbTNCBaudRate);
            this.Controls.Add(this._Label21);
            this.Controls.Add(this._Label22);
            this.Controls.Add(this._cmbTNCSerialPort);
            this.Controls.Add(this._Label14);
            this.Controls.Add(this._cmbTNCType);
            this.Controls.Add(this._Label8);
            this.Controls.Add(this._Label7);
            this.Controls.Add(this._txtScript);
            this.Controls.Add(this._Label6);
            this.Controls.Add(this._Label4);
            this.Controls.Add(this._chkEnabled);
            this.Controls.Add(this._cmbChannelName);
            this.Controls.Add(this._Label3);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnUpdate);
            this.Controls.Add(this._btnRemove);
            this.Controls.Add(this._btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPacketTNCChannels";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Packet TNC Channels";
            this.Load += new System.EventHandler(this.PacketTNCChannels_Load);
            ((System.ComponentModel.ISupportInitialize)(this._nudActivityTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudScriptTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudTNCPort)).EndInit();
            this._grpRadioControl.ResumeLayout(false);
            this._grpRadioControl.PerformLayout();
            this._grpPTCLevels.ResumeLayout(false);
            this._grpPTCLevels.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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