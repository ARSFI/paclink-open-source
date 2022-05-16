using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogPactorTNCChannels : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPactorTNCChannels));
            this._btnClose = new System.Windows.Forms.Button();
            this._btnUpdate = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._cmbRadioModel = new System.Windows.Forms.ComboBox();
            this._cmbRadioPort = new System.Windows.Forms.ComboBox();
            this._cmbRadioBaud = new System.Windows.Forms.ComboBox();
            this._txtAudioCenter = new System.Windows.Forms.TextBox();
            this._cmbTNCBaudRate = new System.Windows.Forms.ComboBox();
            this._cmbTNCSerialPort = new System.Windows.Forms.ComboBox();
            this._txtTNCConfigurationFile = new System.Windows.Forms.TextBox();
            this._cmbTNCType = new System.Windows.Forms.ComboBox();
            this._cmbChannelName = new System.Windows.Forms.ComboBox();
            this._txtRadioAddress = new System.Windows.Forms.TextBox();
            this._chkNarrowFilter = new System.Windows.Forms.CheckBox();
            this._rdoViaPTCII = new System.Windows.Forms.RadioButton();
            this._nudFSKLevel = new System.Windows.Forms.NumericUpDown();
            this._nudPSKLevel = new System.Windows.Forms.NumericUpDown();
            this._rdoSerial = new System.Windows.Forms.RadioButton();
            this._rdoManual = new System.Windows.Forms.RadioButton();
            this._btnBrowseConfiguration = new System.Windows.Forms.Button();
            this._grpChannelSetting = new System.Windows.Forms.GroupBox();
            this._btnUpdateChannelList = new System.Windows.Forms.Button();
            this._chkBusyHold = new System.Windows.Forms.CheckBox();
            this._chkIDEnabled = new System.Windows.Forms.CheckBox();
            this._chkChannelEnabled = new System.Windows.Forms.CheckBox();
            this._chkAutoforwardEnabled = new System.Windows.Forms.CheckBox();
            this._Label3 = new System.Windows.Forms.Label();
            this._Label10 = new System.Windows.Forms.Label();
            this._Label5 = new System.Windows.Forms.Label();
            this._cmbFreqs = new System.Windows.Forms.ComboBox();
            this._cmbCallSigns = new System.Windows.Forms.ComboBox();
            this._nudPriority = new System.Windows.Forms.NumericUpDown();
            this._nudActivityTimeout = new System.Windows.Forms.NumericUpDown();
            this._Label4 = new System.Windows.Forms.Label();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label1 = new System.Windows.Forms.Label();
            this._chkFirstUseOnly = new System.Windows.Forms.CheckBox();
            this._Label13 = new System.Windows.Forms.Label();
            this._rdoV24 = new System.Windows.Forms.RadioButton();
            this._rdoTTL = new System.Windows.Forms.RadioButton();
            this._chkNMEA = new System.Windows.Forms.CheckBox();
            this._grpRadioControl = new System.Windows.Forms.GroupBox();
            this._grpPTCLevels = new System.Windows.Forms.GroupBox();
            this._Label15 = new System.Windows.Forms.Label();
            this._lblRadioAddress = new System.Windows.Forms.Label();
            this._Label8 = new System.Windows.Forms.Label();
            this._Label9 = new System.Windows.Forms.Label();
            this._grpTNCSettings = new System.Windows.Forms.GroupBox();
            this._lblPSKLevel = new System.Windows.Forms.Label();
            this._lblFSKLevel = new System.Windows.Forms.Label();
            this._Label14 = new System.Windows.Forms.Label();
            this._Label18 = new System.Windows.Forms.Label();
            this._Label7 = new System.Windows.Forms.Label();
            this._Label21 = new System.Windows.Forms.Label();
            this._Label22 = new System.Windows.Forms.Label();
            this._btnHelp = new System.Windows.Forms.Button();
            this._Label16 = new System.Windows.Forms.Label();
            this.chkLongPath = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._nudFSKLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPSKLevel)).BeginInit();
            this._grpChannelSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudActivityTimeout)).BeginInit();
            this._grpRadioControl.SuspendLayout();
            this._grpPTCLevels.SuspendLayout();
            this._grpTNCSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnClose
            // 
            this._btnClose.Location = new System.Drawing.Point(928, 879);
            this._btnClose.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(258, 69);
            this._btnClose.TabIndex = 7;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // _btnUpdate
            // 
            this._btnUpdate.Location = new System.Drawing.Point(654, 879);
            this._btnUpdate.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnUpdate.Name = "_btnUpdate";
            this._btnUpdate.Size = new System.Drawing.Size(258, 69);
            this._btnUpdate.TabIndex = 6;
            this._btnUpdate.Text = "Update The Channel";
            this._btnUpdate.UseVisualStyleBackColor = true;
            this._btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // _btnRemove
            // 
            this._btnRemove.Location = new System.Drawing.Point(380, 879);
            this._btnRemove.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.Size = new System.Drawing.Size(258, 69);
            this._btnRemove.TabIndex = 5;
            this._btnRemove.Text = "Remove This Channel";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // _btnAdd
            // 
            this._btnAdd.Location = new System.Drawing.Point(106, 879);
            this._btnAdd.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.Size = new System.Drawing.Size(258, 69);
            this._btnAdd.TabIndex = 4;
            this._btnAdd.Text = "Add New Channel";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // _cmbRadioModel
            // 
            this._cmbRadioModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRadioModel.FormattingEnabled = true;
            this._cmbRadioModel.Location = new System.Drawing.Point(48, 464);
            this._cmbRadioModel.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbRadioModel.Name = "_cmbRadioModel";
            this._cmbRadioModel.Size = new System.Drawing.Size(254, 38);
            this._cmbRadioModel.TabIndex = 5;
            this._ToolTip1.SetToolTip(this._cmbRadioModel, "Select Radio type here.");
            this._cmbRadioModel.TextChanged += new System.EventHandler(this.cmbRadioModel_TextChanged);
            // 
            // _cmbRadioPort
            // 
            this._cmbRadioPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRadioPort.FormattingEnabled = true;
            this._cmbRadioPort.Location = new System.Drawing.Point(170, 300);
            this._cmbRadioPort.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbRadioPort.Name = "_cmbRadioPort";
            this._cmbRadioPort.Size = new System.Drawing.Size(132, 38);
            this._cmbRadioPort.TabIndex = 3;
            this._ToolTip1.SetToolTip(this._cmbRadioPort, "If you selected Direct via Serial this is where you select the radio control port" +
        "");
            // 
            // _cmbRadioBaud
            // 
            this._cmbRadioBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRadioBaud.FormattingEnabled = true;
            this._cmbRadioBaud.Location = new System.Drawing.Point(170, 362);
            this._cmbRadioBaud.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbRadioBaud.Name = "_cmbRadioBaud";
            this._cmbRadioBaud.Size = new System.Drawing.Size(134, 38);
            this._cmbRadioBaud.TabIndex = 4;
            this._ToolTip1.SetToolTip(this._cmbRadioBaud, "Baud rate for the radio control port (8N1 assumed) ");
            // 
            // _txtAudioCenter
            // 
            this._txtAudioCenter.Location = new System.Drawing.Point(422, 88);
            this._txtAudioCenter.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._txtAudioCenter.Name = "_txtAudioCenter";
            this._txtAudioCenter.Size = new System.Drawing.Size(204, 35);
            this._txtAudioCenter.TabIndex = 1;
            this._txtAudioCenter.Text = "1500";
            this._txtAudioCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtAudioCenter, "Center of TNC audio tones in Hz (used to calculate dial frequencies) default = 15" +
        "00 ");
            // 
            // _cmbTNCBaudRate
            // 
            this._cmbTNCBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTNCBaudRate.FormattingEnabled = true;
            this._cmbTNCBaudRate.Location = new System.Drawing.Point(830, 85);
            this._cmbTNCBaudRate.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbTNCBaudRate.Name = "_cmbTNCBaudRate";
            this._cmbTNCBaudRate.Size = new System.Drawing.Size(138, 38);
            this._cmbTNCBaudRate.TabIndex = 3;
            this._ToolTip1.SetToolTip(this._cmbTNCBaudRate, "Select TNC baud rate (not on air baud rate) ");
            // 
            // _cmbTNCSerialPort
            // 
            this._cmbTNCSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTNCSerialPort.FormattingEnabled = true;
            this._cmbTNCSerialPort.Location = new System.Drawing.Point(660, 85);
            this._cmbTNCSerialPort.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbTNCSerialPort.Name = "_cmbTNCSerialPort";
            this._cmbTNCSerialPort.Size = new System.Drawing.Size(138, 38);
            this._cmbTNCSerialPort.Sorted = true;
            this._cmbTNCSerialPort.TabIndex = 2;
            this._ToolTip1.SetToolTip(this._cmbTNCSerialPort, "Select TNC serial port ");
            // 
            // _txtTNCConfigurationFile
            // 
            this._txtTNCConfigurationFile.Location = new System.Drawing.Point(264, 284);
            this._txtTNCConfigurationFile.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._txtTNCConfigurationFile.Name = "_txtTNCConfigurationFile";
            this._txtTNCConfigurationFile.Size = new System.Drawing.Size(670, 35);
            this._txtTNCConfigurationFile.TabIndex = 7;
            this._txtTNCConfigurationFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtTNCConfigurationFile, "The TNC configuration (.aps) file. (see Examples for templates) ");
            // 
            // _cmbTNCType
            // 
            this._cmbTNCType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbTNCType.FormattingEnabled = true;
            this._cmbTNCType.Location = new System.Drawing.Point(140, 85);
            this._cmbTNCType.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbTNCType.MaxDropDownItems = 24;
            this._cmbTNCType.Name = "_cmbTNCType";
            this._cmbTNCType.Size = new System.Drawing.Size(254, 38);
            this._cmbTNCType.TabIndex = 0;
            this._ToolTip1.SetToolTip(this._cmbTNCType, "Select TNC type");
            this._cmbTNCType.SelectedIndexChanged += new System.EventHandler(this.cmbTNCtype_SelectedIndexChanged);
            this._cmbTNCType.TextChanged += new System.EventHandler(this.cmbTNCtype_TextChanged);
            // 
            // _cmbChannelName
            // 
            this._cmbChannelName.FormattingEnabled = true;
            this._cmbChannelName.Location = new System.Drawing.Point(194, 42);
            this._cmbChannelName.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbChannelName.MaxDropDownItems = 24;
            this._cmbChannelName.Name = "_cmbChannelName";
            this._cmbChannelName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmbChannelName.Size = new System.Drawing.Size(466, 38);
            this._cmbChannelName.Sorted = true;
            this._cmbChannelName.TabIndex = 0;
            this._ToolTip1.SetToolTip(this._cmbChannelName, "Enter Channel Name");
            this._cmbChannelName.SelectedIndexChanged += new System.EventHandler(this.cmbChannelName_SelectedIndexChanged);
            this._cmbChannelName.TextChanged += new System.EventHandler(this.cmbChannelName_TextChanged);
            // 
            // _txtRadioAddress
            // 
            this._txtRadioAddress.Enabled = false;
            this._txtRadioAddress.Location = new System.Drawing.Point(260, 533);
            this._txtRadioAddress.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._txtRadioAddress.Name = "_txtRadioAddress";
            this._txtRadioAddress.Size = new System.Drawing.Size(46, 35);
            this._txtRadioAddress.TabIndex = 6;
            this._txtRadioAddress.Text = "01";
            this._txtRadioAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtRadioAddress, "Hex Address for Icom C-IV Radios");
            // 
            // _chkNarrowFilter
            // 
            this._chkNarrowFilter.AutoSize = true;
            this._chkNarrowFilter.Location = new System.Drawing.Point(48, 665);
            this._chkNarrowFilter.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkNarrowFilter.Name = "_chkNarrowFilter";
            this._chkNarrowFilter.Size = new System.Drawing.Size(22, 21);
            this._chkNarrowFilter.TabIndex = 8;
            this._chkNarrowFilter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this._ToolTip1.SetToolTip(this._chkNarrowFilter, "Use Narrow Filter for Pactor I or II");
            this._chkNarrowFilter.UseVisualStyleBackColor = true;
            // 
            // _rdoViaPTCII
            // 
            this._rdoViaPTCII.AutoSize = true;
            this._rdoViaPTCII.Location = new System.Drawing.Point(30, 85);
            this._rdoViaPTCII.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._rdoViaPTCII.Name = "_rdoViaPTCII";
            this._rdoViaPTCII.Size = new System.Drawing.Size(236, 34);
            this._rdoViaPTCII.TabIndex = 1;
            this._rdoViaPTCII.TabStop = true;
            this._rdoViaPTCII.Text = "Via PTC II, IIpro, IIusb";
            this._ToolTip1.SetToolTip(this._rdoViaPTCII, "Use Radio control via a PTC II, IIpro or IIusb (not available in  IIe or IIex mod" +
        "els)");
            this._rdoViaPTCII.UseVisualStyleBackColor = true;
            this._rdoViaPTCII.CheckedChanged += new System.EventHandler(this.rdoViaPTCII_CheckedChanged);
            // 
            // _nudFSKLevel
            // 
            this._nudFSKLevel.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nudFSKLevel.Location = new System.Drawing.Point(398, 155);
            this._nudFSKLevel.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._nudFSKLevel.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this._nudFSKLevel.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nudFSKLevel.Name = "_nudFSKLevel";
            this._nudFSKLevel.Size = new System.Drawing.Size(118, 35);
            this._nudFSKLevel.TabIndex = 4;
            this._ToolTip1.SetToolTip(this._nudFSKLevel, "Set FSK level for Pactor I. Level should NOT generate ALC action.");
            this._nudFSKLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _nudPSKLevel
            // 
            this._nudPSKLevel.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nudPSKLevel.Location = new System.Drawing.Point(824, 155);
            this._nudPSKLevel.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._nudPSKLevel.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this._nudPSKLevel.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this._nudPSKLevel.Name = "_nudPSKLevel";
            this._nudPSKLevel.Size = new System.Drawing.Size(116, 35);
            this._nudPSKLevel.TabIndex = 5;
            this._ToolTip1.SetToolTip(this._nudPSKLevel, "Set PSK Level for P2 & P3. Level should NOT generate ALC action!");
            this._nudPSKLevel.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // _rdoSerial
            // 
            this._rdoSerial.AutoSize = true;
            this._rdoSerial.Location = new System.Drawing.Point(30, 247);
            this._rdoSerial.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._rdoSerial.Name = "_rdoSerial";
            this._rdoSerial.Size = new System.Drawing.Size(229, 34);
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
            this._rdoManual.Location = new System.Drawing.Point(30, 44);
            this._rdoManual.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._rdoManual.Name = "_rdoManual";
            this._rdoManual.Size = new System.Drawing.Size(173, 34);
            this._rdoManual.TabIndex = 0;
            this._rdoManual.TabStop = true;
            this._rdoManual.Text = "Manual (none)";
            this._ToolTip1.SetToolTip(this._rdoManual, "Select if manual radio control.");
            this._rdoManual.UseVisualStyleBackColor = true;
            this._rdoManual.CheckedChanged += new System.EventHandler(this.rdoManual_CheckedChanged);
            // 
            // _btnBrowseConfiguration
            // 
            this._btnBrowseConfiguration.Location = new System.Drawing.Point(950, 279);
            this._btnBrowseConfiguration.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnBrowseConfiguration.Name = "_btnBrowseConfiguration";
            this._btnBrowseConfiguration.Size = new System.Drawing.Size(120, 53);
            this._btnBrowseConfiguration.TabIndex = 8;
            this._btnBrowseConfiguration.Text = "Browse";
            this._ToolTip1.SetToolTip(this._btnBrowseConfiguration, "Browse to select custom configuration file.");
            this._btnBrowseConfiguration.UseVisualStyleBackColor = true;
            this._btnBrowseConfiguration.Click += new System.EventHandler(this.btnBrowseConfiguration_Click);
            // 
            // _grpChannelSetting
            // 
            this._grpChannelSetting.Controls.Add(this._btnUpdateChannelList);
            this._grpChannelSetting.Controls.Add(this._chkBusyHold);
            this._grpChannelSetting.Controls.Add(this._chkIDEnabled);
            this._grpChannelSetting.Controls.Add(this._chkChannelEnabled);
            this._grpChannelSetting.Controls.Add(this._chkAutoforwardEnabled);
            this._grpChannelSetting.Controls.Add(this._Label3);
            this._grpChannelSetting.Controls.Add(this._Label10);
            this._grpChannelSetting.Controls.Add(this._Label5);
            this._grpChannelSetting.Controls.Add(this._cmbFreqs);
            this._grpChannelSetting.Controls.Add(this._cmbCallSigns);
            this._grpChannelSetting.Controls.Add(this._nudPriority);
            this._grpChannelSetting.Controls.Add(this._nudActivityTimeout);
            this._grpChannelSetting.Controls.Add(this._Label4);
            this._grpChannelSetting.Controls.Add(this._Label2);
            this._grpChannelSetting.Controls.Add(this._cmbChannelName);
            this._grpChannelSetting.Controls.Add(this._Label1);
            this._grpChannelSetting.Location = new System.Drawing.Point(26, 76);
            this._grpChannelSetting.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpChannelSetting.Name = "_grpChannelSetting";
            this._grpChannelSetting.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpChannelSetting.Size = new System.Drawing.Size(1106, 376);
            this._grpChannelSetting.TabIndex = 1;
            this._grpChannelSetting.TabStop = false;
            this._grpChannelSetting.Text = "Pactor Channel Settings";
            this._ToolTip1.SetToolTip(this._grpChannelSetting, "Check to enable Pactor 1 FEC ID on Disconnect.");
            // 
            // _btnUpdateChannelList
            // 
            this._btnUpdateChannelList.Location = new System.Drawing.Point(418, 295);
            this._btnUpdateChannelList.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnUpdateChannelList.Name = "_btnUpdateChannelList";
            this._btnUpdateChannelList.Size = new System.Drawing.Size(308, 62);
            this._btnUpdateChannelList.TabIndex = 184;
            this._btnUpdateChannelList.Text = "Update Channel List";
            this._btnUpdateChannelList.UseVisualStyleBackColor = true;
            this._btnUpdateChannelList.Click += new System.EventHandler(this.btnUpdateChannelList_Click);
            // 
            // _chkBusyHold
            // 
            this._chkBusyHold.AutoSize = true;
            this._chkBusyHold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkBusyHold.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkBusyHold.Location = new System.Drawing.Point(684, 175);
            this._chkBusyHold.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkBusyHold.Name = "_chkBusyHold";
            this._chkBusyHold.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._chkBusyHold.Size = new System.Drawing.Size(281, 34);
            this._chkBusyHold.TabIndex = 183;
            this._chkBusyHold.Text = "Enable busy channel hold:";
            this._ToolTip1.SetToolTip(this._chkBusyHold, "Check for autoforwarding on this HF channel (normally limited to Emergency Use ON" +
        "LY!)");
            this._chkBusyHold.UseVisualStyleBackColor = true;
            // 
            // _chkIDEnabled
            // 
            this._chkIDEnabled.AutoSize = true;
            this._chkIDEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkIDEnabled.Checked = true;
            this._chkIDEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkIDEnabled.Location = new System.Drawing.Point(422, 175);
            this._chkIDEnabled.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkIDEnabled.Name = "_chkIDEnabled";
            this._chkIDEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._chkIDEnabled.Size = new System.Drawing.Size(209, 34);
            this._chkIDEnabled.TabIndex = 181;
            this._chkIDEnabled.Text = "Pactor ID Enabled:";
            this._ToolTip1.SetToolTip(this._chkIDEnabled, "Check to enable Pactor 1 FEC ID ");
            this._chkIDEnabled.UseVisualStyleBackColor = true;
            // 
            // _chkChannelEnabled
            // 
            this._chkChannelEnabled.AutoSize = true;
            this._chkChannelEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkChannelEnabled.Location = new System.Drawing.Point(166, 175);
            this._chkChannelEnabled.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkChannelEnabled.Name = "_chkChannelEnabled";
            this._chkChannelEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._chkChannelEnabled.Size = new System.Drawing.Size(200, 34);
            this._chkChannelEnabled.TabIndex = 180;
            this._chkChannelEnabled.Text = "Channel Enabled:";
            this._ToolTip1.SetToolTip(this._chkChannelEnabled, "Enable this channel ");
            this._chkChannelEnabled.UseVisualStyleBackColor = true;
            // 
            // _chkAutoforwardEnabled
            // 
            this._chkAutoforwardEnabled.AutoSize = true;
            this._chkAutoforwardEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkAutoforwardEnabled.Location = new System.Drawing.Point(730, 231);
            this._chkAutoforwardEnabled.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkAutoforwardEnabled.Name = "_chkAutoforwardEnabled";
            this._chkAutoforwardEnabled.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._chkAutoforwardEnabled.Size = new System.Drawing.Size(240, 34);
            this._chkAutoforwardEnabled.TabIndex = 182;
            this._chkAutoforwardEnabled.Text = "Autoforward Enabled:";
            this._ToolTip1.SetToolTip(this._chkAutoforwardEnabled, "Check for autoforwarding on this HF channel (normally limited to Emergency Use ON" +
        "LY!)");
            this._chkAutoforwardEnabled.UseVisualStyleBackColor = true;
            this._chkAutoforwardEnabled.CheckedChanged += new System.EventHandler(this.chkAutoforwardEnabled_CheckedChanged);
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Location = new System.Drawing.Point(420, 235);
            this._Label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(88, 30);
            this._Label3.TabIndex = 179;
            this._Label3.Text = "Minutes";
            // 
            // _Label10
            // 
            this._Label10.AutoSize = true;
            this._Label10.Location = new System.Drawing.Point(484, 115);
            this._Label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label10.Name = "_Label10";
            this._Label10.Size = new System.Drawing.Size(202, 30);
            this._Label10.TabIndex = 178;
            this._Label10.Text = "RF Center Freq (kHz)";
            // 
            // _Label5
            // 
            this._Label5.AutoSize = true;
            this._Label5.Location = new System.Drawing.Point(678, 53);
            this._Label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label5.Name = "_Label5";
            this._Label5.Size = new System.Drawing.Size(167, 30);
            this._Label5.TabIndex = 177;
            this._Label5.Text = "Remote Callsign:";
            // 
            // _cmbFreqs
            // 
            this._cmbFreqs.FormattingEnabled = true;
            this._cmbFreqs.Location = new System.Drawing.Point(712, 108);
            this._cmbFreqs.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbFreqs.Name = "_cmbFreqs";
            this._cmbFreqs.Size = new System.Drawing.Size(348, 38);
            this._cmbFreqs.TabIndex = 3;
            this._ToolTip1.SetToolTip(this._cmbFreqs, "This is a list of the center frequencies for the selected remote callsign. ...Or " +
        "you can enter one directly (KHz).");
            // 
            // _cmbCallSigns
            // 
            this._cmbCallSigns.FormattingEnabled = true;
            this._cmbCallSigns.Location = new System.Drawing.Point(864, 42);
            this._cmbCallSigns.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._cmbCallSigns.Name = "_cmbCallSigns";
            this._cmbCallSigns.Size = new System.Drawing.Size(196, 38);
            this._cmbCallSigns.TabIndex = 1;
            this._ToolTip1.SetToolTip(this._cmbCallSigns, "You can type in a specific call or select one from the PMBO/RMS Type of freq list" +
        "");
            this._cmbCallSigns.SelectedIndexChanged += new System.EventHandler(this.cmbCallSigns_SelectedIndexChanged);
            // 
            // _nudPriority
            // 
            this._nudPriority.Location = new System.Drawing.Point(370, 111);
            this._nudPriority.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
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
            this._nudPriority.Size = new System.Drawing.Size(78, 35);
            this._nudPriority.TabIndex = 2;
            this._nudPriority.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudPriority, "Select channel priority 1-5, 1=highest (default = 5)");
            this._nudPriority.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _nudActivityTimeout
            // 
            this._nudActivityTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nudActivityTimeout.Location = new System.Drawing.Point(344, 228);
            this._nudActivityTimeout.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._nudActivityTimeout.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this._nudActivityTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._nudActivityTimeout.Name = "_nudActivityTimeout";
            this._nudActivityTimeout.Size = new System.Drawing.Size(64, 35);
            this._nudActivityTimeout.TabIndex = 9;
            this._nudActivityTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._nudActivityTimeout, "Select activity timeout 1 - 4 minutes, (default = 1)");
            this._nudActivityTimeout.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(168, 235);
            this._Label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(168, 30);
            this._Label4.TabIndex = 161;
            this._Label4.Text = "Activity Timeout:";
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(194, 115);
            this._Label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(165, 30);
            this._Label2.TabIndex = 160;
            this._Label2.Text = "Channel priority:";
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._Label1.Location = new System.Drawing.Point(30, 48);
            this._Label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(146, 25);
            this._Label1.TabIndex = 148;
            this._Label1.Text = "Channel name:";
            // 
            // _chkFirstUseOnly
            // 
            this._chkFirstUseOnly.AutoSize = true;
            this._chkFirstUseOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkFirstUseOnly.Location = new System.Drawing.Point(36, 221);
            this._chkFirstUseOnly.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkFirstUseOnly.Name = "_chkFirstUseOnly";
            this._chkFirstUseOnly.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._chkFirstUseOnly.Size = new System.Drawing.Size(453, 34);
            this._chkFirstUseOnly.TabIndex = 6;
            this._chkFirstUseOnly.Text = "Do a full TNC configuration only on first use:";
            this._ToolTip1.SetToolTip(this._chkFirstUseOnly, " ");
            this._chkFirstUseOnly.UseVisualStyleBackColor = true;
            // 
            // _Label13
            // 
            this._Label13.Location = new System.Drawing.Point(90, 642);
            this._Label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label13.Name = "_Label13";
            this._Label13.Size = new System.Drawing.Size(236, 90);
            this._Label13.TabIndex = 169;
            this._Label13.Text = "Enable narrow filters on Pactor 1 and 2 (when available)";
            this._ToolTip1.SetToolTip(this._Label13, "Select Sideband (Default=USB)");
            // 
            // _rdoV24
            // 
            this._rdoV24.AutoSize = true;
            this._rdoV24.Location = new System.Drawing.Point(134, 44);
            this._rdoV24.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._rdoV24.Name = "_rdoV24";
            this._rdoV24.Size = new System.Drawing.Size(103, 34);
            this._rdoV24.TabIndex = 1;
            this._rdoV24.TabStop = true;
            this._rdoV24.Text = "RS-232";
            this._ToolTip1.SetToolTip(this._rdoV24, "Select TTL or RS232 Levels for PTCIIpro and PTCIIusb models");
            this._rdoV24.UseVisualStyleBackColor = true;
            // 
            // _rdoTTL
            // 
            this._rdoTTL.AutoSize = true;
            this._rdoTTL.Location = new System.Drawing.Point(24, 44);
            this._rdoTTL.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._rdoTTL.Name = "_rdoTTL";
            this._rdoTTL.Size = new System.Drawing.Size(70, 34);
            this._rdoTTL.TabIndex = 0;
            this._rdoTTL.TabStop = true;
            this._rdoTTL.Text = "TTL";
            this._ToolTip1.SetToolTip(this._rdoTTL, "Select TTL or RS232 Levels for PTCIIpro and PTCIIusb models");
            this._rdoTTL.UseVisualStyleBackColor = true;
            // 
            // _chkNMEA
            // 
            this._chkNMEA.AutoSize = true;
            this._chkNMEA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkNMEA.Location = new System.Drawing.Point(48, 595);
            this._chkNMEA.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._chkNMEA.Name = "_chkNMEA";
            this._chkNMEA.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._chkNMEA.Size = new System.Drawing.Size(250, 34);
            this._chkNMEA.TabIndex = 181;
            this._chkNMEA.Text = "Use NMEA Commands";
            this._ToolTip1.SetToolTip(this._chkNMEA, "Enable this channel ");
            this._chkNMEA.UseVisualStyleBackColor = true;
            // 
            // _grpRadioControl
            // 
            this._grpRadioControl.Controls.Add(this._chkNMEA);
            this._grpRadioControl.Controls.Add(this._grpPTCLevels);
            this._grpRadioControl.Controls.Add(this._Label13);
            this._grpRadioControl.Controls.Add(this._Label15);
            this._grpRadioControl.Controls.Add(this._rdoViaPTCII);
            this._grpRadioControl.Controls.Add(this._chkNarrowFilter);
            this._grpRadioControl.Controls.Add(this._lblRadioAddress);
            this._grpRadioControl.Controls.Add(this._txtRadioAddress);
            this._grpRadioControl.Controls.Add(this._Label8);
            this._grpRadioControl.Controls.Add(this._cmbRadioModel);
            this._grpRadioControl.Controls.Add(this._Label9);
            this._grpRadioControl.Controls.Add(this._rdoSerial);
            this._grpRadioControl.Controls.Add(this._rdoManual);
            this._grpRadioControl.Controls.Add(this._cmbRadioPort);
            this._grpRadioControl.Controls.Add(this._cmbRadioBaud);
            this._grpRadioControl.Location = new System.Drawing.Point(1166, 104);
            this._grpRadioControl.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpRadioControl.Name = "_grpRadioControl";
            this._grpRadioControl.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpRadioControl.Size = new System.Drawing.Size(356, 750);
            this._grpRadioControl.TabIndex = 3;
            this._grpRadioControl.TabStop = false;
            this._grpRadioControl.Text = "Optional Radio Control";
            // 
            // _grpPTCLevels
            // 
            this._grpPTCLevels.Controls.Add(this._rdoV24);
            this._grpPTCLevels.Controls.Add(this._rdoTTL);
            this._grpPTCLevels.Location = new System.Drawing.Point(44, 138);
            this._grpPTCLevels.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpPTCLevels.Name = "_grpPTCLevels";
            this._grpPTCLevels.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpPTCLevels.Size = new System.Drawing.Size(268, 95);
            this._grpPTCLevels.TabIndex = 170;
            this._grpPTCLevels.TabStop = false;
            this._grpPTCLevels.Text = "PTC Levels to Radio";
            // 
            // _Label15
            // 
            this._Label15.AutoSize = true;
            this._Label15.Location = new System.Drawing.Point(42, 307);
            this._Label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label15.Name = "_Label15";
            this._Label15.Size = new System.Drawing.Size(111, 30);
            this._Label15.TabIndex = 168;
            this._Label15.Text = "Serial Port:";
            // 
            // _lblRadioAddress
            // 
            this._lblRadioAddress.AutoSize = true;
            this._lblRadioAddress.Location = new System.Drawing.Point(44, 540);
            this._lblRadioAddress.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._lblRadioAddress.Name = "_lblRadioAddress";
            this._lblRadioAddress.Size = new System.Drawing.Size(202, 30);
            this._lblRadioAddress.TabIndex = 163;
            this._lblRadioAddress.Text = "Radio Address (hex):";
            // 
            // _Label8
            // 
            this._Label8.AutoSize = true;
            this._Label8.Location = new System.Drawing.Point(42, 425);
            this._Label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label8.Name = "_Label8";
            this._Label8.Size = new System.Drawing.Size(136, 30);
            this._Label8.TabIndex = 12;
            this._Label8.Text = "Radio Model:";
            // 
            // _Label9
            // 
            this._Label9.AutoSize = true;
            this._Label9.Location = new System.Drawing.Point(42, 369);
            this._Label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label9.Name = "_Label9";
            this._Label9.Size = new System.Drawing.Size(113, 30);
            this._Label9.TabIndex = 10;
            this._Label9.Text = "Baud Rate:";
            // 
            // _grpTNCSettings
            // 
            this._grpTNCSettings.Controls.Add(this.chkLongPath);
            this._grpTNCSettings.Controls.Add(this._chkFirstUseOnly);
            this._grpTNCSettings.Controls.Add(this._nudPSKLevel);
            this._grpTNCSettings.Controls.Add(this._nudFSKLevel);
            this._grpTNCSettings.Controls.Add(this._lblPSKLevel);
            this._grpTNCSettings.Controls.Add(this._lblFSKLevel);
            this._grpTNCSettings.Controls.Add(this._Label14);
            this._grpTNCSettings.Controls.Add(this._cmbTNCType);
            this._grpTNCSettings.Controls.Add(this._btnBrowseConfiguration);
            this._grpTNCSettings.Controls.Add(this._txtTNCConfigurationFile);
            this._grpTNCSettings.Controls.Add(this._Label18);
            this._grpTNCSettings.Controls.Add(this._Label7);
            this._grpTNCSettings.Controls.Add(this._txtAudioCenter);
            this._grpTNCSettings.Controls.Add(this._cmbTNCBaudRate);
            this._grpTNCSettings.Controls.Add(this._Label21);
            this._grpTNCSettings.Controls.Add(this._Label22);
            this._grpTNCSettings.Controls.Add(this._cmbTNCSerialPort);
            this._grpTNCSettings.Location = new System.Drawing.Point(26, 478);
            this._grpTNCSettings.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpTNCSettings.Name = "_grpTNCSettings";
            this._grpTNCSettings.Padding = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._grpTNCSettings.Size = new System.Drawing.Size(1106, 376);
            this._grpTNCSettings.TabIndex = 2;
            this._grpTNCSettings.TabStop = false;
            this._grpTNCSettings.Text = "TNC Settings";
            // 
            // _lblPSKLevel
            // 
            this._lblPSKLevel.AutoSize = true;
            this._lblPSKLevel.Location = new System.Drawing.Point(592, 159);
            this._lblPSKLevel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._lblPSKLevel.Name = "_lblPSKLevel";
            this._lblPSKLevel.Size = new System.Drawing.Size(211, 30);
            this._lblPSKLevel.TabIndex = 171;
            this._lblPSKLevel.Text = "PTC II PSK Level (mv):";
            // 
            // _lblFSKLevel
            // 
            this._lblFSKLevel.AutoSize = true;
            this._lblFSKLevel.Location = new System.Drawing.Point(166, 159);
            this._lblFSKLevel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._lblFSKLevel.Name = "_lblFSKLevel";
            this._lblFSKLevel.Size = new System.Drawing.Size(209, 30);
            this._lblFSKLevel.TabIndex = 170;
            this._lblFSKLevel.Text = "PTC II FSK Level (mv):";
            // 
            // _Label14
            // 
            this._Label14.AutoSize = true;
            this._Label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._Label14.Location = new System.Drawing.Point(134, 51);
            this._Label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label14.Name = "_Label14";
            this._Label14.Size = new System.Drawing.Size(110, 25);
            this._Label14.TabIndex = 169;
            this._Label14.Text = "TNC Type:";
            // 
            // _Label18
            // 
            this._Label18.AutoSize = true;
            this._Label18.Location = new System.Drawing.Point(36, 291);
            this._Label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label18.Name = "_Label18";
            this._Label18.Size = new System.Drawing.Size(228, 30);
            this._Label18.TabIndex = 164;
            this._Label18.Text = "TNC Configuration File:";
            // 
            // _Label7
            // 
            this._Label7.AutoSize = true;
            this._Label7.Location = new System.Drawing.Point(422, 51);
            this._Label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label7.Name = "_Label7";
            this._Label7.Size = new System.Drawing.Size(199, 30);
            this._Label7.TabIndex = 161;
            this._Label7.Text = "Audio Tones Center:";
            // 
            // _Label21
            // 
            this._Label21.AutoSize = true;
            this._Label21.Location = new System.Drawing.Point(824, 51);
            this._Label21.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label21.Name = "_Label21";
            this._Label21.Size = new System.Drawing.Size(113, 30);
            this._Label21.TabIndex = 155;
            this._Label21.Text = "Baud Rate:";
            // 
            // _Label22
            // 
            this._Label22.AutoSize = true;
            this._Label22.Location = new System.Drawing.Point(654, 53);
            this._Label22.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label22.Name = "_Label22";
            this._Label22.Size = new System.Drawing.Size(112, 30);
            this._Label22.TabIndex = 154;
            this._Label22.Text = "Serial port:";
            // 
            // _btnHelp
            // 
            this._btnHelp.Location = new System.Drawing.Point(1202, 879);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(258, 69);
            this._btnHelp.TabIndex = 0;
            this._btnHelp.Text = "Help";
            this._btnHelp.UseVisualStyleBackColor = true;
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // _Label16
            // 
            this._Label16.AutoSize = true;
            this._Label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label16.Location = new System.Drawing.Point(302, 21);
            this._Label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this._Label16.Name = "_Label16";
            this._Label16.Size = new System.Drawing.Size(808, 25);
            this._Label16.TabIndex = 145;
            this._Label16.Text = "To create a new channel type a new channel name in the Channel Name text box...";
            // 
            // chkLongPath
            // 
            this.chkLongPath.AutoSize = true;
            this.chkLongPath.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLongPath.Location = new System.Drawing.Point(607, 221);
            this.chkLongPath.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.chkLongPath.Name = "chkLongPath";
            this.chkLongPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkLongPath.Size = new System.Drawing.Size(443, 34);
            this.chkLongPath.TabIndex = 172;
            this.chkLongPath.Text = "Use Long Path for high-latency connections";
            this._ToolTip1.SetToolTip(this.chkLongPath, " ");
            this.chkLongPath.UseVisualStyleBackColor = true;
            // 
            // DialogPactorTNCChannels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1566, 992);
            this.Controls.Add(this._Label16);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._grpChannelSetting);
            this.Controls.Add(this._grpTNCSettings);
            this.Controls.Add(this._grpRadioControl);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnUpdate);
            this.Controls.Add(this._btnRemove);
            this.Controls.Add(this._btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPactorTNCChannels";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pactor TNC Channels";
            this.Load += new System.EventHandler(this.PactorTNCChannels_Load);
            ((System.ComponentModel.ISupportInitialize)(this._nudFSKLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudPSKLevel)).EndInit();
            this._grpChannelSetting.ResumeLayout(false);
            this._grpChannelSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._nudPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._nudActivityTimeout)).EndInit();
            this._grpRadioControl.ResumeLayout(false);
            this._grpRadioControl.PerformLayout();
            this._grpPTCLevels.ResumeLayout(false);
            this._grpPTCLevels.PerformLayout();
            this._grpTNCSettings.ResumeLayout(false);
            this._grpTNCSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private GroupBox _grpTNCSettings;

        internal GroupBox grpTNCSettings
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _grpTNCSettings;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_grpTNCSettings != null)
                {
                }

                _grpTNCSettings = value;
                if (_grpTNCSettings != null)
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

        private TextBox _txtAudioCenter;

        internal TextBox txtAudioCenter
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtAudioCenter;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtAudioCenter != null)
                {
                }

                _txtAudioCenter = value;
                if (_txtAudioCenter != null)
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

        private GroupBox _grpChannelSetting;

        internal GroupBox grpChannelSetting
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _grpChannelSetting;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_grpChannelSetting != null)
                {
                }

                _grpChannelSetting = value;
                if (_grpChannelSetting != null)
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

        private TextBox _txtRadioAddress;

        internal TextBox txtRadioAddress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtRadioAddress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtRadioAddress != null)
                {
                }

                _txtRadioAddress = value;
                if (_txtRadioAddress != null)
                {
                }
            }
        }

        private CheckBox _chkNarrowFilter;

        internal CheckBox chkNarrowFilter
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkNarrowFilter;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkNarrowFilter != null)
                {
                }

                _chkNarrowFilter = value;
                if (_chkNarrowFilter != null)
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

        private Label _lblPSKLevel;

        internal Label lblPSKLevel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblPSKLevel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblPSKLevel != null)
                {
                }

                _lblPSKLevel = value;
                if (_lblPSKLevel != null)
                {
                }
            }
        }

        private Label _lblFSKLevel;

        internal Label lblFSKLevel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblFSKLevel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblFSKLevel != null)
                {
                }

                _lblFSKLevel = value;
                if (_lblFSKLevel != null)
                {
                }
            }
        }

        private NumericUpDown _nudFSKLevel;

        internal NumericUpDown nudFSKLevel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudFSKLevel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudFSKLevel != null)
                {
                }

                _nudFSKLevel = value;
                if (_nudFSKLevel != null)
                {
                }
            }
        }

        private NumericUpDown _nudPSKLevel;

        internal NumericUpDown nudPSKLevel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _nudPSKLevel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_nudPSKLevel != null)
                {
                }

                _nudPSKLevel = value;
                if (_nudPSKLevel != null)
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

        private ComboBox _cmbCallSigns;

        internal ComboBox cmbCallSigns
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbCallSigns;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbCallSigns != null)
                {
                    _cmbCallSigns.SelectedIndexChanged -= cmbCallSigns_SelectedIndexChanged;
                }

                _cmbCallSigns = value;
                if (_cmbCallSigns != null)
                {
                    _cmbCallSigns.SelectedIndexChanged += cmbCallSigns_SelectedIndexChanged;
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
                }

                _cmbFreqs = value;
                if (_cmbFreqs != null)
                {
                }
            }
        }

        private Label _Label5;

        internal Label Label5
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label5;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label5 != null)
                {
                }

                _Label5 = value;
                if (_Label5 != null)
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

        private Label _Label13;

        internal Label Label13
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label13;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label13 != null)
                {
                }

                _Label13 = value;
                if (_Label13 != null)
                {
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

        private CheckBox _chkBusyHold;

        internal CheckBox chkBusyHold
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkBusyHold;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkBusyHold != null)
                {
                }

                _chkBusyHold = value;
                if (_chkBusyHold != null)
                {
                }
            }
        }

        private CheckBox _chkIDEnabled;

        internal CheckBox chkIDEnabled
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkIDEnabled;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkIDEnabled != null)
                {
                }

                _chkIDEnabled = value;
                if (_chkIDEnabled != null)
                {
                }
            }
        }

        private CheckBox _chkChannelEnabled;

        internal CheckBox chkChannelEnabled
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkChannelEnabled;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkChannelEnabled != null)
                {
                }

                _chkChannelEnabled = value;
                if (_chkChannelEnabled != null)
                {
                }
            }
        }

        private CheckBox _chkAutoforwardEnabled;

        internal CheckBox chkAutoforwardEnabled
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkAutoforwardEnabled;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkAutoforwardEnabled != null)
                {
                    _chkAutoforwardEnabled.CheckedChanged -= chkAutoforwardEnabled_CheckedChanged;
                }

                _chkAutoforwardEnabled = value;
                if (_chkAutoforwardEnabled != null)
                {
                    _chkAutoforwardEnabled.CheckedChanged += chkAutoforwardEnabled_CheckedChanged;
                }
            }
        }

        private CheckBox _chkNMEA;

        internal CheckBox chkNMEA
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkNMEA;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkNMEA != null)
                {
                }

                _chkNMEA = value;
                if (_chkNMEA != null)
                {
                }
            }
        }

        private Button _btnUpdateChannelList;
        private CheckBox chkLongPath;

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
    }
}