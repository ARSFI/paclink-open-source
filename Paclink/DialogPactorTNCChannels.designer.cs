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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPactorTNCChannels));
            _btnClose = new Button();
            _btnClose.Click += new EventHandler(btnClose_Click);
            _btnUpdate = new Button();
            _btnUpdate.Click += new EventHandler(btnUpdate_Click);
            _btnRemove = new Button();
            _btnRemove.Click += new EventHandler(btnRemove_Click);
            _btnAdd = new Button();
            _btnAdd.Click += new EventHandler(btnAdd_Click);
            _ToolTip1 = new ToolTip(components);
            _cmbRadioModel = new ComboBox();
            _cmbRadioModel.TextChanged += new EventHandler(cmbRadioModel_TextChanged);
            _cmbRadioPort = new ComboBox();
            _cmbRadioBaud = new ComboBox();
            _txtAudioCenter = new TextBox();
            _cmbTNCBaudRate = new ComboBox();
            _cmbTNCSerialPort = new ComboBox();
            _txtTNCConfigurationFile = new TextBox();
            _cmbTNCType = new ComboBox();
            _cmbTNCType.SelectedIndexChanged += new EventHandler(cmbTNCtype_SelectedIndexChanged);
            _cmbTNCType.TextChanged += new EventHandler(cmbTNCtype_TextChanged);
            _cmbChannelName = new ComboBox();
            _cmbChannelName.SelectedIndexChanged += new EventHandler(cmbChannelName_SelectedIndexChanged);
            _cmbChannelName.TextChanged += new EventHandler(cmbChannelName_TextChanged);
            _txtRadioAddress = new TextBox();
            _chkNarrowFilter = new CheckBox();
            _rdoViaPTCII = new RadioButton();
            _rdoViaPTCII.CheckedChanged += new EventHandler(rdoViaPTCII_CheckedChanged);
            _nudFSKLevel = new NumericUpDown();
            _nudPSKLevel = new NumericUpDown();
            _rdoSerial = new RadioButton();
            _rdoSerial.CheckedChanged += new EventHandler(rdoSerial_CheckedChanged);
            _rdoManual = new RadioButton();
            _rdoManual.CheckedChanged += new EventHandler(rdoManual_CheckedChanged);
            _btnBrowseConfiguration = new Button();
            _btnBrowseConfiguration.Click += new EventHandler(btnBrowseConfiguration_Click);
            _grpChannelSetting = new GroupBox();
            _btnUpdateChannelList = new Button();
            _btnUpdateChannelList.Click += new EventHandler(btnUpdateChannelList_Click);
            _chkBusyHold = new CheckBox();
            _chkIDEnabled = new CheckBox();
            _chkChannelEnabled = new CheckBox();
            _chkAutoforwardEnabled = new CheckBox();
            _chkAutoforwardEnabled.CheckedChanged += new EventHandler(chkAutoforwardEnabled_CheckedChanged);
            _Label3 = new Label();
            _Label10 = new Label();
            _Label5 = new Label();
            _cmbFreqs = new ComboBox();
            _cmbCallSigns = new ComboBox();
            _cmbCallSigns.SelectedIndexChanged += new EventHandler(cmbCallSigns_SelectedIndexChanged);
            _nudPriority = new NumericUpDown();
            _nudActivityTimeout = new NumericUpDown();
            _Label4 = new Label();
            _Label2 = new Label();
            _Label1 = new Label();
            _chkFirstUseOnly = new CheckBox();
            _Label13 = new Label();
            _rdoV24 = new RadioButton();
            _rdoTTL = new RadioButton();
            _chkNMEA = new CheckBox();
            _grpRadioControl = new GroupBox();
            _grpPTCLevels = new GroupBox();
            _Label15 = new Label();
            _lblRadioAddress = new Label();
            _Label8 = new Label();
            _Label9 = new Label();
            _grpTNCSettings = new GroupBox();
            _lblPSKLevel = new Label();
            _lblFSKLevel = new Label();
            _Label14 = new Label();
            _Label18 = new Label();
            _Label7 = new Label();
            _Label21 = new Label();
            _Label22 = new Label();
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            _Label16 = new Label();
            ((System.ComponentModel.ISupportInitialize)_nudFSKLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudPSKLevel).BeginInit();
            _grpChannelSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_nudActivityTimeout).BeginInit();
            _grpRadioControl.SuspendLayout();
            _grpPTCLevels.SuspendLayout();
            _grpTNCSettings.SuspendLayout();
            SuspendLayout();
            // 
            // btnClose
            // 
            _btnClose.Location = new Point(464, 381);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(129, 30);
            _btnClose.TabIndex = 7;
            _btnClose.Text = "Close";
            _btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            _btnUpdate.Location = new Point(327, 381);
            _btnUpdate.Name = "_btnUpdate";
            _btnUpdate.Size = new Size(129, 30);
            _btnUpdate.TabIndex = 6;
            _btnUpdate.Text = "Update The Channel";
            _btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            _btnRemove.Location = new Point(190, 381);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Size = new Size(129, 30);
            _btnRemove.TabIndex = 5;
            _btnRemove.Text = "Remove This Channel";
            _btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            _btnAdd.Location = new Point(53, 381);
            _btnAdd.Name = "_btnAdd";
            _btnAdd.Size = new Size(129, 30);
            _btnAdd.TabIndex = 4;
            _btnAdd.Text = "Add New Channel";
            _btnAdd.UseVisualStyleBackColor = true;
            // 
            // cmbRadioModel
            // 
            _cmbRadioModel.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRadioModel.FormattingEnabled = true;
            _cmbRadioModel.Location = new Point(24, 201);
            _cmbRadioModel.Name = "_cmbRadioModel";
            _cmbRadioModel.Size = new Size(129, 21);
            _cmbRadioModel.TabIndex = 5;
            _ToolTip1.SetToolTip(_cmbRadioModel, "Select Radio type here.");
            // 
            // cmbRadioPort
            // 
            _cmbRadioPort.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRadioPort.FormattingEnabled = true;
            _cmbRadioPort.Location = new Point(85, 130);
            _cmbRadioPort.Name = "_cmbRadioPort";
            _cmbRadioPort.Size = new Size(68, 21);
            _cmbRadioPort.TabIndex = 3;
            _ToolTip1.SetToolTip(_cmbRadioPort, "If you selected Direct via Serial this is where you select the radio control port" + "");
            // 
            // cmbRadioBaud
            // 
            _cmbRadioBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRadioBaud.FormattingEnabled = true;
            _cmbRadioBaud.Location = new Point(85, 157);
            _cmbRadioBaud.Name = "_cmbRadioBaud";
            _cmbRadioBaud.Size = new Size(69, 21);
            _cmbRadioBaud.TabIndex = 4;
            _ToolTip1.SetToolTip(_cmbRadioBaud, "Baud rate for the radio control port (8N1 assumed) ");
            // 
            // txtAudioCenter
            // 
            _txtAudioCenter.Location = new Point(211, 38);
            _txtAudioCenter.Name = "_txtAudioCenter";
            _txtAudioCenter.Size = new Size(104, 20);
            _txtAudioCenter.TabIndex = 1;
            _txtAudioCenter.Text = "1500";
            _txtAudioCenter.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtAudioCenter, "Center of TNC audio tones in Hz (used to calculate dial frequencies) default = 15" + "00 ");
            // 
            // cmbTNCBaudRate
            // 
            _cmbTNCBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTNCBaudRate.FormattingEnabled = true;
            _cmbTNCBaudRate.Location = new Point(415, 37);
            _cmbTNCBaudRate.Name = "_cmbTNCBaudRate";
            _cmbTNCBaudRate.Size = new Size(71, 21);
            _cmbTNCBaudRate.TabIndex = 3;
            _ToolTip1.SetToolTip(_cmbTNCBaudRate, "Select TNC baud rate (not on air baud rate) ");
            // 
            // cmbTNCSerialPort
            // 
            _cmbTNCSerialPort.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTNCSerialPort.FormattingEnabled = true;
            _cmbTNCSerialPort.Location = new Point(330, 37);
            _cmbTNCSerialPort.Name = "_cmbTNCSerialPort";
            _cmbTNCSerialPort.Size = new Size(71, 21);
            _cmbTNCSerialPort.Sorted = true;
            _cmbTNCSerialPort.TabIndex = 2;
            _ToolTip1.SetToolTip(_cmbTNCSerialPort, "Select TNC serial port ");
            // 
            // txtTNCConfigurationFile
            // 
            _txtTNCConfigurationFile.Location = new Point(132, 123);
            _txtTNCConfigurationFile.Name = "_txtTNCConfigurationFile";
            _txtTNCConfigurationFile.Size = new Size(337, 20);
            _txtTNCConfigurationFile.TabIndex = 7;
            _txtTNCConfigurationFile.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtTNCConfigurationFile, "The TNC configuration (.aps) file. (see Examples for templates) ");
            // 
            // cmbTNCType
            // 
            _cmbTNCType.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTNCType.FormattingEnabled = true;
            _cmbTNCType.Location = new Point(70, 37);
            _cmbTNCType.MaxDropDownItems = 24;
            _cmbTNCType.Name = "_cmbTNCType";
            _cmbTNCType.Size = new Size(129, 21);
            _cmbTNCType.TabIndex = 0;
            _ToolTip1.SetToolTip(_cmbTNCType, "Select TNC type");
            // 
            // cmbChannelName
            // 
            _cmbChannelName.FormattingEnabled = true;
            _cmbChannelName.Location = new Point(97, 18);
            _cmbChannelName.MaxDropDownItems = 24;
            _cmbChannelName.Name = "_cmbChannelName";
            _cmbChannelName.RightToLeft = RightToLeft.No;
            _cmbChannelName.Size = new Size(235, 21);
            _cmbChannelName.Sorted = true;
            _cmbChannelName.TabIndex = 0;
            _ToolTip1.SetToolTip(_cmbChannelName, "Enter Channel Name");
            // 
            // txtRadioAddress
            // 
            _txtRadioAddress.Enabled = false;
            _txtRadioAddress.Location = new Point(130, 231);
            _txtRadioAddress.Name = "_txtRadioAddress";
            _txtRadioAddress.Size = new Size(25, 20);
            _txtRadioAddress.TabIndex = 6;
            _txtRadioAddress.Text = "01";
            _txtRadioAddress.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtRadioAddress, "Hex Address for Icom C-IV Radios");
            // 
            // chkNarrowFilter
            // 
            _chkNarrowFilter.AutoSize = true;
            _chkNarrowFilter.Location = new Point(24, 288);
            _chkNarrowFilter.Name = "_chkNarrowFilter";
            _chkNarrowFilter.Size = new Size(15, 14);
            _chkNarrowFilter.TabIndex = 8;
            _chkNarrowFilter.TextAlign = ContentAlignment.TopRight;
            _ToolTip1.SetToolTip(_chkNarrowFilter, "Use Narrow Filter for Pactor I or II");
            _chkNarrowFilter.UseVisualStyleBackColor = true;
            // 
            // rdoViaPTCII
            // 
            _rdoViaPTCII.AutoSize = true;
            _rdoViaPTCII.Location = new Point(15, 37);
            _rdoViaPTCII.Name = "_rdoViaPTCII";
            _rdoViaPTCII.Size = new Size(129, 17);
            _rdoViaPTCII.TabIndex = 1;
            _rdoViaPTCII.TabStop = true;
            _rdoViaPTCII.Text = "Via PTC II, IIpro, IIusb";
            _ToolTip1.SetToolTip(_rdoViaPTCII, "Use Radio control via a PTC II, IIpro or IIusb (not available in  IIe or IIex mod" + "els)");
            _rdoViaPTCII.UseVisualStyleBackColor = true;
            // 
            // nudFSKLevel
            // 
            _nudFSKLevel.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _nudFSKLevel.Location = new Point(199, 67);
            _nudFSKLevel.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            _nudFSKLevel.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _nudFSKLevel.Name = "_nudFSKLevel";
            _nudFSKLevel.Size = new Size(59, 20);
            _nudFSKLevel.TabIndex = 4;
            _ToolTip1.SetToolTip(_nudFSKLevel, "Set FSK level for Pactor I. Level should NOT generate ALC action.");
            _nudFSKLevel.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // nudPSKLevel
            // 
            _nudPSKLevel.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _nudPSKLevel.Location = new Point(412, 67);
            _nudPSKLevel.Maximum = new decimal(new int[] { 1009, 0, 0, 0 });
            _nudPSKLevel.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _nudPSKLevel.Name = "_nudPSKLevel";
            _nudPSKLevel.Size = new Size(58, 20);
            _nudPSKLevel.TabIndex = 5;
            _ToolTip1.SetToolTip(_nudPSKLevel, "Set PSK Level for P2 & P3. Level should NOT generate ALC action!");
            _nudPSKLevel.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // rdoSerial
            // 
            _rdoSerial.AutoSize = true;
            _rdoSerial.Location = new Point(15, 107);
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
            _rdoManual.Location = new Point(15, 19);
            _rdoManual.Name = "_rdoManual";
            _rdoManual.Size = new Size(93, 17);
            _rdoManual.TabIndex = 0;
            _rdoManual.TabStop = true;
            _rdoManual.Text = "Manual (none)";
            _ToolTip1.SetToolTip(_rdoManual, "Select if manual radio control.");
            _rdoManual.UseVisualStyleBackColor = true;
            // 
            // btnBrowseConfiguration
            // 
            _btnBrowseConfiguration.Location = new Point(475, 121);
            _btnBrowseConfiguration.Name = "_btnBrowseConfiguration";
            _btnBrowseConfiguration.Size = new Size(60, 23);
            _btnBrowseConfiguration.TabIndex = 8;
            _btnBrowseConfiguration.Text = "Browse";
            _ToolTip1.SetToolTip(_btnBrowseConfiguration, "Browse to select custom configuration file.");
            _btnBrowseConfiguration.UseVisualStyleBackColor = true;
            // 
            // grpChannelSetting
            // 
            _grpChannelSetting.Controls.Add(_btnUpdateChannelList);
            _grpChannelSetting.Controls.Add(_chkBusyHold);
            _grpChannelSetting.Controls.Add(_chkIDEnabled);
            _grpChannelSetting.Controls.Add(_chkChannelEnabled);
            _grpChannelSetting.Controls.Add(_chkAutoforwardEnabled);
            _grpChannelSetting.Controls.Add(_Label3);
            _grpChannelSetting.Controls.Add(_Label10);
            _grpChannelSetting.Controls.Add(_Label5);
            _grpChannelSetting.Controls.Add(_cmbFreqs);
            _grpChannelSetting.Controls.Add(_cmbCallSigns);
            _grpChannelSetting.Controls.Add(_nudPriority);
            _grpChannelSetting.Controls.Add(_nudActivityTimeout);
            _grpChannelSetting.Controls.Add(_Label4);
            _grpChannelSetting.Controls.Add(_Label2);
            _grpChannelSetting.Controls.Add(_cmbChannelName);
            _grpChannelSetting.Controls.Add(_Label1);
            _grpChannelSetting.Location = new Point(13, 33);
            _grpChannelSetting.Name = "_grpChannelSetting";
            _grpChannelSetting.Size = new Size(553, 163);
            _grpChannelSetting.TabIndex = 1;
            _grpChannelSetting.TabStop = false;
            _grpChannelSetting.Text = "Pactor Channel Settings";
            _ToolTip1.SetToolTip(_grpChannelSetting, "Check to enable Pactor 1 FEC ID on Disconnect.");
            // 
            // btnUpdateChannelList
            // 
            _btnUpdateChannelList.Location = new Point(209, 128);
            _btnUpdateChannelList.Name = "_btnUpdateChannelList";
            _btnUpdateChannelList.Size = new Size(154, 27);
            _btnUpdateChannelList.TabIndex = 184;
            _btnUpdateChannelList.Text = "Update Channel List";
            _btnUpdateChannelList.UseVisualStyleBackColor = true;
            // 
            // chkBusyHold
            // 
            _chkBusyHold.AutoSize = true;
            _chkBusyHold.CheckAlign = ContentAlignment.MiddleRight;
            _chkBusyHold.ImageAlign = ContentAlignment.MiddleRight;
            _chkBusyHold.Location = new Point(342, 76);
            _chkBusyHold.Name = "_chkBusyHold";
            _chkBusyHold.RightToLeft = RightToLeft.No;
            _chkBusyHold.Size = new Size(151, 17);
            _chkBusyHold.TabIndex = 183;
            _chkBusyHold.Text = "Enable busy channel hold:";
            _ToolTip1.SetToolTip(_chkBusyHold, "Check for autoforwarding on this HF channel (normally limited to Emergency Use ON" + "LY!)");
            _chkBusyHold.UseVisualStyleBackColor = true;
            // 
            // chkIDEnabled
            // 
            _chkIDEnabled.AutoSize = true;
            _chkIDEnabled.CheckAlign = ContentAlignment.MiddleRight;
            _chkIDEnabled.Checked = true;
            _chkIDEnabled.CheckState = CheckState.Checked;
            _chkIDEnabled.Location = new Point(211, 76);
            _chkIDEnabled.Name = "_chkIDEnabled";
            _chkIDEnabled.RightToLeft = RightToLeft.No;
            _chkIDEnabled.Size = new Size(116, 17);
            _chkIDEnabled.TabIndex = 181;
            _chkIDEnabled.Text = "Pactor ID Enabled:";
            _ToolTip1.SetToolTip(_chkIDEnabled, "Check to enable Pactor 1 FEC ID ");
            _chkIDEnabled.UseVisualStyleBackColor = true;
            // 
            // chkChannelEnabled
            // 
            _chkChannelEnabled.AutoSize = true;
            _chkChannelEnabled.CheckAlign = ContentAlignment.MiddleRight;
            _chkChannelEnabled.Location = new Point(83, 76);
            _chkChannelEnabled.Name = "_chkChannelEnabled";
            _chkChannelEnabled.RightToLeft = RightToLeft.No;
            _chkChannelEnabled.Size = new Size(110, 17);
            _chkChannelEnabled.TabIndex = 180;
            _chkChannelEnabled.Text = "Channel Enabled:";
            _ToolTip1.SetToolTip(_chkChannelEnabled, "Enable this channel ");
            _chkChannelEnabled.UseVisualStyleBackColor = true;
            // 
            // chkAutoforwardEnabled
            // 
            _chkAutoforwardEnabled.AutoSize = true;
            _chkAutoforwardEnabled.CheckAlign = ContentAlignment.MiddleRight;
            _chkAutoforwardEnabled.Location = new Point(365, 100);
            _chkAutoforwardEnabled.Name = "_chkAutoforwardEnabled";
            _chkAutoforwardEnabled.RightToLeft = RightToLeft.No;
            _chkAutoforwardEnabled.Size = new Size(128, 17);
            _chkAutoforwardEnabled.TabIndex = 182;
            _chkAutoforwardEnabled.Text = "Autoforward Enabled:";
            _ToolTip1.SetToolTip(_chkAutoforwardEnabled, "Check for autoforwarding on this HF channel (normally limited to Emergency Use ON" + "LY!)");
            _chkAutoforwardEnabled.UseVisualStyleBackColor = true;
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Location = new Point(210, 102);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(44, 13);
            _Label3.TabIndex = 179;
            _Label3.Text = "Minutes";
            // 
            // Label10
            // 
            _Label10.AutoSize = true;
            _Label10.Location = new Point(242, 50);
            _Label10.Name = "_Label10";
            _Label10.Size = new Size(107, 13);
            _Label10.TabIndex = 178;
            _Label10.Text = "RF Center Freq (kHz)";
            // 
            // Label5
            // 
            _Label5.AutoSize = true;
            _Label5.Location = new Point(339, 23);
            _Label5.Name = "_Label5";
            _Label5.Size = new Size(86, 13);
            _Label5.TabIndex = 177;
            _Label5.Text = "Remote Callsign:";
            // 
            // cmbFreqs
            // 
            _cmbFreqs.FormattingEnabled = true;
            _cmbFreqs.Location = new Point(356, 47);
            _cmbFreqs.Name = "_cmbFreqs";
            _cmbFreqs.Size = new Size(176, 21);
            _cmbFreqs.TabIndex = 3;
            _ToolTip1.SetToolTip(_cmbFreqs, "This is a list of the center frequencies for the selected remote callsign. ...Or " + "you can enter one directly (KHz).");
            // 
            // cmbCallSigns
            // 
            _cmbCallSigns.FormattingEnabled = true;
            _cmbCallSigns.Location = new Point(432, 18);
            _cmbCallSigns.Name = "_cmbCallSigns";
            _cmbCallSigns.Size = new Size(100, 21);
            _cmbCallSigns.TabIndex = 1;
            _ToolTip1.SetToolTip(_cmbCallSigns, "You can type in a specific call or select one from the PMBO/RMS Type of freq list" + "");
            // 
            // nudPriority
            // 
            _nudPriority.Location = new Point(185, 48);
            _nudPriority.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            _nudPriority.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudPriority.Name = "_nudPriority";
            _nudPriority.Size = new Size(39, 20);
            _nudPriority.TabIndex = 2;
            _nudPriority.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudPriority, "Select channel priority 1-5, 1=highest (default = 5)");
            _nudPriority.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // nudActivityTimeout
            // 
            _nudActivityTimeout.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _nudActivityTimeout.Location = new Point(172, 99);
            _nudActivityTimeout.Maximum = new decimal(new int[] { 5, 0, 0, 0 });
            _nudActivityTimeout.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _nudActivityTimeout.Name = "_nudActivityTimeout";
            _nudActivityTimeout.Size = new Size(32, 20);
            _nudActivityTimeout.TabIndex = 9;
            _nudActivityTimeout.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_nudActivityTimeout, "Select activity timeout 1 - 4 minutes, (default = 1)");
            _nudActivityTimeout.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(84, 102);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(85, 13);
            _Label4.TabIndex = 161;
            _Label4.Text = "Activity Timeout:";
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(97, 50);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(82, 13);
            _Label2.TabIndex = 160;
            _Label2.Text = "Channel priority:";
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _Label1.Location = new Point(15, 21);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(78, 13);
            _Label1.TabIndex = 148;
            _Label1.Text = "Channel name:";
            // 
            // chkFirstUseOnly
            // 
            _chkFirstUseOnly.AutoSize = true;
            _chkFirstUseOnly.CheckAlign = ContentAlignment.MiddleRight;
            _chkFirstUseOnly.Location = new Point(160, 96);
            _chkFirstUseOnly.Name = "_chkFirstUseOnly";
            _chkFirstUseOnly.RightToLeft = RightToLeft.No;
            _chkFirstUseOnly.Size = new Size(233, 17);
            _chkFirstUseOnly.TabIndex = 6;
            _chkFirstUseOnly.Text = "Do a full TNC configuration only on first use:";
            _ToolTip1.SetToolTip(_chkFirstUseOnly, " ");
            _chkFirstUseOnly.UseVisualStyleBackColor = true;
            // 
            // Label13
            // 
            _Label13.Location = new Point(45, 278);
            _Label13.Name = "_Label13";
            _Label13.Size = new Size(118, 39);
            _Label13.TabIndex = 169;
            _Label13.Text = "Enable narrow filters on Pactor 1 and 2 (when available)";
            _ToolTip1.SetToolTip(_Label13, "Select Sideband (Default=USB)");
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
            // chkNMEA
            // 
            _chkNMEA.AutoSize = true;
            _chkNMEA.CheckAlign = ContentAlignment.MiddleRight;
            _chkNMEA.Location = new Point(24, 258);
            _chkNMEA.Name = "_chkNMEA";
            _chkNMEA.RightToLeft = RightToLeft.Yes;
            _chkNMEA.Size = new Size(134, 17);
            _chkNMEA.TabIndex = 181;
            _chkNMEA.Text = "Use NMEA Commands";
            _ToolTip1.SetToolTip(_chkNMEA, "Enable this channel ");
            _chkNMEA.UseVisualStyleBackColor = true;
            // 
            // grpRadioControl
            // 
            _grpRadioControl.Controls.Add(_chkNMEA);
            _grpRadioControl.Controls.Add(_grpPTCLevels);
            _grpRadioControl.Controls.Add(_Label13);
            _grpRadioControl.Controls.Add(_Label15);
            _grpRadioControl.Controls.Add(_rdoViaPTCII);
            _grpRadioControl.Controls.Add(_chkNarrowFilter);
            _grpRadioControl.Controls.Add(_lblRadioAddress);
            _grpRadioControl.Controls.Add(_txtRadioAddress);
            _grpRadioControl.Controls.Add(_Label8);
            _grpRadioControl.Controls.Add(_cmbRadioModel);
            _grpRadioControl.Controls.Add(_Label9);
            _grpRadioControl.Controls.Add(_rdoSerial);
            _grpRadioControl.Controls.Add(_rdoManual);
            _grpRadioControl.Controls.Add(_cmbRadioPort);
            _grpRadioControl.Controls.Add(_cmbRadioBaud);
            _grpRadioControl.Location = new Point(583, 45);
            _grpRadioControl.Name = "_grpRadioControl";
            _grpRadioControl.Size = new Size(178, 325);
            _grpRadioControl.TabIndex = 3;
            _grpRadioControl.TabStop = false;
            _grpRadioControl.Text = "Optional Radio Control";
            // 
            // grpPTCLevels
            // 
            _grpPTCLevels.Controls.Add(_rdoV24);
            _grpPTCLevels.Controls.Add(_rdoTTL);
            _grpPTCLevels.Location = new Point(22, 60);
            _grpPTCLevels.Name = "_grpPTCLevels";
            _grpPTCLevels.Size = new Size(134, 41);
            _grpPTCLevels.TabIndex = 170;
            _grpPTCLevels.TabStop = false;
            _grpPTCLevels.Text = "PTC Levels to Radio";
            // 
            // Label15
            // 
            _Label15.AutoSize = true;
            _Label15.Location = new Point(21, 133);
            _Label15.Name = "_Label15";
            _Label15.Size = new Size(58, 13);
            _Label15.TabIndex = 168;
            _Label15.Text = "Serial Port:";
            // 
            // lblRadioAddress
            // 
            _lblRadioAddress.AutoSize = true;
            _lblRadioAddress.Location = new Point(22, 234);
            _lblRadioAddress.Name = "_lblRadioAddress";
            _lblRadioAddress.Size = new Size(105, 13);
            _lblRadioAddress.TabIndex = 163;
            _lblRadioAddress.Text = "Radio Address (hex):";
            // 
            // Label8
            // 
            _Label8.AutoSize = true;
            _Label8.Location = new Point(21, 184);
            _Label8.Name = "_Label8";
            _Label8.Size = new Size(70, 13);
            _Label8.TabIndex = 12;
            _Label8.Text = "Radio Model:";
            // 
            // Label9
            // 
            _Label9.AutoSize = true;
            _Label9.Location = new Point(21, 160);
            _Label9.Name = "_Label9";
            _Label9.Size = new Size(61, 13);
            _Label9.TabIndex = 10;
            _Label9.Text = "Baud Rate:";
            // 
            // grpTNCSettings
            // 
            _grpTNCSettings.Controls.Add(_chkFirstUseOnly);
            _grpTNCSettings.Controls.Add(_nudPSKLevel);
            _grpTNCSettings.Controls.Add(_nudFSKLevel);
            _grpTNCSettings.Controls.Add(_lblPSKLevel);
            _grpTNCSettings.Controls.Add(_lblFSKLevel);
            _grpTNCSettings.Controls.Add(_Label14);
            _grpTNCSettings.Controls.Add(_cmbTNCType);
            _grpTNCSettings.Controls.Add(_btnBrowseConfiguration);
            _grpTNCSettings.Controls.Add(_txtTNCConfigurationFile);
            _grpTNCSettings.Controls.Add(_Label18);
            _grpTNCSettings.Controls.Add(_Label7);
            _grpTNCSettings.Controls.Add(_txtAudioCenter);
            _grpTNCSettings.Controls.Add(_cmbTNCBaudRate);
            _grpTNCSettings.Controls.Add(_Label21);
            _grpTNCSettings.Controls.Add(_Label22);
            _grpTNCSettings.Controls.Add(_cmbTNCSerialPort);
            _grpTNCSettings.Location = new Point(13, 207);
            _grpTNCSettings.Name = "_grpTNCSettings";
            _grpTNCSettings.Size = new Size(553, 163);
            _grpTNCSettings.TabIndex = 2;
            _grpTNCSettings.TabStop = false;
            _grpTNCSettings.Text = "TNC Settings";
            // 
            // lblPSKLevel
            // 
            _lblPSKLevel.AutoSize = true;
            _lblPSKLevel.Location = new Point(296, 69);
            _lblPSKLevel.Name = "_lblPSKLevel";
            _lblPSKLevel.Size = new Size(116, 13);
            _lblPSKLevel.TabIndex = 171;
            _lblPSKLevel.Text = "PTC II PSK Level (mv):";
            // 
            // lblFSKLevel
            // 
            _lblFSKLevel.AutoSize = true;
            _lblFSKLevel.Location = new Point(83, 69);
            _lblFSKLevel.Name = "_lblFSKLevel";
            _lblFSKLevel.Size = new Size(115, 13);
            _lblFSKLevel.TabIndex = 170;
            _lblFSKLevel.Text = "PTC II FSK Level (mv):";
            // 
            // Label14
            // 
            _Label14.AutoSize = true;
            _Label14.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _Label14.Location = new Point(67, 22);
            _Label14.Name = "_Label14";
            _Label14.Size = new Size(59, 13);
            _Label14.TabIndex = 169;
            _Label14.Text = "TNC Type:";
            // 
            // Label18
            // 
            _Label18.AutoSize = true;
            _Label18.Location = new Point(18, 126);
            _Label18.Name = "_Label18";
            _Label18.Size = new Size(116, 13);
            _Label18.TabIndex = 164;
            _Label18.Text = "TNC Configuration File:";
            // 
            // Label7
            // 
            _Label7.AutoSize = true;
            _Label7.Location = new Point(211, 22);
            _Label7.Name = "_Label7";
            _Label7.Size = new Size(104, 13);
            _Label7.TabIndex = 161;
            _Label7.Text = "Audio Tones Center:";
            // 
            // Label21
            // 
            _Label21.AutoSize = true;
            _Label21.Location = new Point(412, 22);
            _Label21.Name = "_Label21";
            _Label21.Size = new Size(61, 13);
            _Label21.TabIndex = 155;
            _Label21.Text = "Baud Rate:";
            // 
            // Label22
            // 
            _Label22.AutoSize = true;
            _Label22.Location = new Point(327, 23);
            _Label22.Name = "_Label22";
            _Label22.Size = new Size(57, 13);
            _Label22.TabIndex = 154;
            _Label22.Text = "Serial port:";
            // 
            // btnHelp
            // 
            _btnHelp.Location = new Point(601, 381);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(129, 30);
            _btnHelp.TabIndex = 0;
            _btnHelp.Text = "Help";
            _btnHelp.UseVisualStyleBackColor = true;
            // 
            // Label16
            // 
            _Label16.AutoSize = true;
            _Label16.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label16.Location = new Point(151, 9);
            _Label16.Name = "_Label16";
            _Label16.Size = new Size(481, 13);
            _Label16.TabIndex = 145;
            _Label16.Text = "To create a new channel type a new channel name in the Channel Name text box...";
            // 
            // DialogPactorTNCChannels
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(783, 430);
            Controls.Add(_Label16);
            Controls.Add(_btnHelp);
            Controls.Add(_grpChannelSetting);
            Controls.Add(_grpTNCSettings);
            Controls.Add(_grpRadioControl);
            Controls.Add(_btnClose);
            Controls.Add(_btnUpdate);
            Controls.Add(_btnRemove);
            Controls.Add(_btnAdd);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogPactorTNCChannels";
            RightToLeft = RightToLeft.No;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pactor TNC Channels";
            ((System.ComponentModel.ISupportInitialize)_nudFSKLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudPSKLevel).EndInit();
            _grpChannelSetting.ResumeLayout(false);
            _grpChannelSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_nudPriority).EndInit();
            ((System.ComponentModel.ISupportInitialize)_nudActivityTimeout).EndInit();
            _grpRadioControl.ResumeLayout(false);
            _grpRadioControl.PerformLayout();
            _grpPTCLevels.ResumeLayout(false);
            _grpPTCLevels.PerformLayout();
            _grpTNCSettings.ResumeLayout(false);
            _grpTNCSettings.PerformLayout();
            Load += new EventHandler(PactorTNCChannels_Load);
            ResumeLayout(false);
            PerformLayout();
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