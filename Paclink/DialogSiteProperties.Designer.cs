using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogSiteProperties : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components is object)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSiteProperties));
            this._Label26 = new System.Windows.Forms.Label();
            this._txtPOP3PortNumber = new System.Windows.Forms.TextBox();
            this._Label23 = new System.Windows.Forms.Label();
            this._txtSMTPPortNumber = new System.Windows.Forms.TextBox();
            this._Label21 = new System.Windows.Forms.Label();
            this._txtSecureLoginPassword = new System.Windows.Forms.TextBox();
            this._Label2 = new System.Windows.Forms.Label();
            this._Label3 = new System.Windows.Forms.Label();
            this._txtGridSquare = new System.Windows.Forms.TextBox();
            this._txtSiteCallsign = new System.Windows.Forms.TextBox();
            this._chkLANAccessable = new System.Windows.Forms.CheckBox();
            this._Label9 = new System.Windows.Forms.Label();
            this._txtSizeLimit = new System.Windows.Forms.TextBox();
            this._Label1 = new System.Windows.Forms.Label();
            this._Label11 = new System.Windows.Forms.Label();
            this._txtPrefix = new System.Windows.Forms.TextBox();
            this._Label12 = new System.Windows.Forms.Label();
            this._txtSuffix = new System.Windows.Forms.TextBox();
            this._OK_Button = new System.Windows.Forms.Button();
            this._Cancel_Button = new System.Windows.Forms.Button();
            this._Label8 = new System.Windows.Forms.Label();
            this._ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._cmbLocalIPAddress = new System.Windows.Forms.ComboBox();
            this._chkEnableRadar = new System.Windows.Forms.CheckBox();
            this._txtPOP3Password = new System.Windows.Forms.TextBox();
            this._txtRMSRelayIPPath = new System.Windows.Forms.TextBox();
            this._btnHelp = new System.Windows.Forms.Button();
            this._Label4 = new System.Windows.Forms.Label();
            this._Label5 = new System.Windows.Forms.Label();
            this._GroupBox1 = new System.Windows.Forms.GroupBox();
            this._Label10 = new System.Windows.Forms.Label();
            this._txtRMSRelayPort = new System.Windows.Forms.TextBox();
            this._Label6 = new System.Windows.Forms.Label();
            this._rdoUseRMSRelay = new System.Windows.Forms.RadioButton();
            this._rdoUseCMS = new System.Windows.Forms.RadioButton();
            this._Label7 = new System.Windows.Forms.Label();
            this._txtServiceCodes = new System.Windows.Forms.TextBox();
            this._chkForceHFRouting = new System.Windows.Forms.CheckBox();
            this._Label13 = new System.Windows.Forms.Label();
            this._Label14 = new System.Windows.Forms.Label();
            this._GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _Label26
            // 
            this._Label26.Location = new System.Drawing.Point(62, 395);
            this._Label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label26.Name = "_Label26";
            this._Label26.Size = new System.Drawing.Size(630, 36);
            this._Label26.TabIndex = 3;
            this._Label26.Text = resources.GetString("_Label26.Text");
            // 
            // _txtPOP3PortNumber
            // 
            this._txtPOP3PortNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtPOP3PortNumber.Location = new System.Drawing.Point(203, 226);
            this._txtPOP3PortNumber.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtPOP3PortNumber.Name = "_txtPOP3PortNumber";
            this._txtPOP3PortNumber.Size = new System.Drawing.Size(79, 23);
            this._txtPOP3PortNumber.TabIndex = 6;
            this._txtPOP3PortNumber.Text = "110";
            this._txtPOP3PortNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtPOP3PortNumber, "POP3 Port number, change only if confilct with other program or service (default " +
        "= 110)");
            // 
            // _Label23
            // 
            this._Label23.AutoSize = true;
            this._Label23.Location = new System.Drawing.Point(82, 230);
            this._Label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label23.Name = "_Label23";
            this._Label23.Size = new System.Drawing.Size(111, 15);
            this._Label23.TabIndex = 51;
            this._Label23.Text = "POP3 Port Number:";
            // 
            // _txtSMTPPortNumber
            // 
            this._txtSMTPPortNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtSMTPPortNumber.Location = new System.Drawing.Point(203, 196);
            this._txtSMTPPortNumber.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtSMTPPortNumber.Name = "_txtSMTPPortNumber";
            this._txtSMTPPortNumber.Size = new System.Drawing.Size(79, 23);
            this._txtSMTPPortNumber.TabIndex = 5;
            this._txtSMTPPortNumber.Text = "25";
            this._txtSMTPPortNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtSMTPPortNumber, "SMTP Port number, change only if confilct with other program or service (default " +
        "= 25)");
            // 
            // _Label21
            // 
            this._Label21.AutoSize = true;
            this._Label21.Location = new System.Drawing.Point(79, 200);
            this._Label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label21.Name = "_Label21";
            this._Label21.Size = new System.Drawing.Size(112, 15);
            this._Label21.TabIndex = 48;
            this._Label21.Text = "SMTP Port Number:";
            // 
            // _txtSecureLoginPassword
            // 
            this._txtSecureLoginPassword.Location = new System.Drawing.Point(203, 76);
            this._txtSecureLoginPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtSecureLoginPassword.Name = "_txtSecureLoginPassword";
            this._txtSecureLoginPassword.Size = new System.Drawing.Size(129, 23);
            this._txtSecureLoginPassword.TabIndex = 1;
            this._txtSecureLoginPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtSecureLoginPassword, "Winlink Password for this callsign and -ssid");
            // 
            // _Label2
            // 
            this._Label2.AutoSize = true;
            this._Label2.Location = new System.Drawing.Point(46, 80);
            this._Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label2.Name = "_Label2";
            this._Label2.Size = new System.Drawing.Size(60, 15);
            this._Label2.TabIndex = 3;
            this._Label2.Text = "Password:";
            // 
            // _Label3
            // 
            this._Label3.AutoSize = true;
            this._Label3.Location = new System.Drawing.Point(102, 140);
            this._Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label3.Name = "_Label3";
            this._Label3.Size = new System.Drawing.Size(91, 15);
            this._Label3.TabIndex = 70;
            this._Label3.Text = "Site grid square:";
            // 
            // _txtGridSquare
            // 
            this._txtGridSquare.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtGridSquare.Location = new System.Drawing.Point(203, 136);
            this._txtGridSquare.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtGridSquare.Name = "_txtGridSquare";
            this._txtGridSquare.Size = new System.Drawing.Size(129, 23);
            this._txtGridSquare.TabIndex = 3;
            this._txtGridSquare.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtGridSquare, "Grid square location (6 character) ");
            // 
            // _txtSiteCallsign
            // 
            this._txtSiteCallsign.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtSiteCallsign.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._txtSiteCallsign.Location = new System.Drawing.Point(203, 43);
            this._txtSiteCallsign.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtSiteCallsign.Name = "_txtSiteCallsign";
            this._txtSiteCallsign.Size = new System.Drawing.Size(129, 20);
            this._txtSiteCallsign.TabIndex = 0;
            this._txtSiteCallsign.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtSiteCallsign, "Valid radio callsign (including optional -SSID) for this site.");
            // 
            // _chkLANAccessable
            // 
            this._chkLANAccessable.AutoSize = true;
            this._chkLANAccessable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkLANAccessable.Location = new System.Drawing.Point(609, 153);
            this._chkLANAccessable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkLANAccessable.Name = "_chkLANAccessable";
            this._chkLANAccessable.Size = new System.Drawing.Size(107, 19);
            this._chkLANAccessable.TabIndex = 12;
            this._chkLANAccessable.Text = "LAN Accessible";
            this._ToolTip1.SetToolTip(this._chkLANAccessable, "Check if this Paclink Instance will be accessed by other computers on the local L" +
        "AN");
            this._chkLANAccessable.UseVisualStyleBackColor = true;
            // 
            // _Label9
            // 
            this._Label9.AutoSize = true;
            this._Label9.Location = new System.Drawing.Point(12, 170);
            this._Label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label9.Name = "_Label9";
            this._Label9.Size = new System.Drawing.Size(180, 15);
            this._Label9.TabIndex = 75;
            this._Label9.Text = "Message size limit (compressed):";
            // 
            // _txtSizeLimit
            // 
            this._txtSizeLimit.Location = new System.Drawing.Point(203, 166);
            this._txtSizeLimit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtSizeLimit.Name = "_txtSizeLimit";
            this._txtSizeLimit.Size = new System.Drawing.Size(129, 23);
            this._txtSizeLimit.TabIndex = 4;
            this._ToolTip1.SetToolTip(this._txtSizeLimit, "Attachment limit in bytes. 0 removes all attachments. (Default=120000)");
            // 
            // _Label1
            // 
            this._Label1.AutoSize = true;
            this._Label1.Location = new System.Drawing.Point(88, 46);
            this._Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label1.Name = "_Label1";
            this._Label1.Size = new System.Drawing.Size(103, 15);
            this._Label1.TabIndex = 68;
            this._Label1.Text = "Callsign (no SSID):";
            // 
            // _Label11
            // 
            this._Label11.AutoSize = true;
            this._Label11.Location = new System.Drawing.Point(414, 80);
            this._Label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label11.Name = "_Label11";
            this._Label11.Size = new System.Drawing.Size(99, 15);
            this._Label11.TabIndex = 80;
            this._Label11.Text = "Callsign ID Prefix:";
            // 
            // _txtPrefix
            // 
            this._txtPrefix.Location = new System.Drawing.Point(525, 76);
            this._txtPrefix.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtPrefix.Name = "_txtPrefix";
            this._txtPrefix.Size = new System.Drawing.Size(70, 23);
            this._txtPrefix.TabIndex = 8;
            this._ToolTip1.SetToolTip(this._txtPrefix, "Prefix ID. Does not affect callsign or addresses (Default = blank)");
            // 
            // _Label12
            // 
            this._Label12.AutoSize = true;
            this._Label12.Location = new System.Drawing.Point(414, 111);
            this._Label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label12.Name = "_Label12";
            this._Label12.Size = new System.Drawing.Size(99, 15);
            this._Label12.TabIndex = 82;
            this._Label12.Text = "Callsign ID Suffix:";
            // 
            // _txtSuffix
            // 
            this._txtSuffix.Location = new System.Drawing.Point(525, 106);
            this._txtSuffix.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtSuffix.Name = "_txtSuffix";
            this._txtSuffix.Size = new System.Drawing.Size(70, 23);
            this._txtSuffix.TabIndex = 9;
            this._ToolTip1.SetToolTip(this._txtSuffix, "Suffix ID. Does not affect callsign or addresses (Default = blank)");
            // 
            // _OK_Button
            // 
            this._OK_Button.Location = new System.Drawing.Point(134, 437);
            this._OK_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._OK_Button.Name = "_OK_Button";
            this._OK_Button.Size = new System.Drawing.Size(154, 30);
            this._OK_Button.TabIndex = 16;
            this._OK_Button.Text = "Update";
            this._OK_Button.Click += new System.EventHandler(this.OK_Button_Click);
            // 
            // _Cancel_Button
            // 
            this._Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._Cancel_Button.Location = new System.Drawing.Point(301, 437);
            this._Cancel_Button.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._Cancel_Button.Name = "_Cancel_Button";
            this._Cancel_Button.Size = new System.Drawing.Size(154, 30);
            this._Cancel_Button.TabIndex = 17;
            this._Cancel_Button.Text = "Close";
            this._Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
            // 
            // _Label8
            // 
            this._Label8.AutoSize = true;
            this._Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._Label8.Location = new System.Drawing.Point(200, 10);
            this._Label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label8.Name = "_Label8";
            this._Label8.Size = new System.Drawing.Size(310, 16);
            this._Label8.TabIndex = 74;
            this._Label8.Text = "Enter the properties for this Paclink instance";
            // 
            // _cmbLocalIPAddress
            // 
            this._cmbLocalIPAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbLocalIPAddress.FormattingEnabled = true;
            this._cmbLocalIPAddress.Location = new System.Drawing.Point(526, 44);
            this._cmbLocalIPAddress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._cmbLocalIPAddress.Name = "_cmbLocalIPAddress";
            this._cmbLocalIPAddress.Size = new System.Drawing.Size(202, 23);
            this._cmbLocalIPAddress.TabIndex = 7;
            this._ToolTip1.SetToolTip(this._cmbLocalIPAddress, "Select default Local IP Addresss. Set to \"default\" unless multihomed.");
            // 
            // _chkEnableRadar
            // 
            this._chkEnableRadar.AutoSize = true;
            this._chkEnableRadar.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkEnableRadar.Location = new System.Drawing.Point(550, 174);
            this._chkEnableRadar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkEnableRadar.Name = "_chkEnableRadar";
            this._chkEnableRadar.Size = new System.Drawing.Size(166, 19);
            this._chkEnableRadar.TabIndex = 13;
            this._chkEnableRadar.Text = "Range and Bearing Display";
            this._ToolTip1.SetToolTip(this._chkEnableRadar, "Check to enable range & Bearing \"Radar\" display upon connect");
            this._chkEnableRadar.UseVisualStyleBackColor = true;
            // 
            // _txtPOP3Password
            // 
            this._txtPOP3Password.Location = new System.Drawing.Point(203, 106);
            this._txtPOP3Password.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtPOP3Password.Name = "_txtPOP3Password";
            this._txtPOP3Password.Size = new System.Drawing.Size(129, 23);
            this._txtPOP3Password.TabIndex = 2;
            this._txtPOP3Password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtPOP3Password, "Grid square location (6 character) ");
            // 
            // _txtRMSRelayIPPath
            // 
            this._txtRMSRelayIPPath.Enabled = false;
            this._txtRMSRelayIPPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._txtRMSRelayIPPath.Location = new System.Drawing.Point(280, 47);
            this._txtRMSRelayIPPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtRMSRelayIPPath.Name = "_txtRMSRelayIPPath";
            this._txtRMSRelayIPPath.Size = new System.Drawing.Size(163, 20);
            this._txtRMSRelayIPPath.TabIndex = 129;
            this._txtRMSRelayIPPath.Text = "localhost";
            this._txtRMSRelayIPPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this._ToolTip1.SetToolTip(this._txtRMSRelayIPPath, "Valid radio callsign (including optional -SSID) for this site.");
            // 
            // _btnHelp
            // 
            this._btnHelp.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnHelp.Location = new System.Drawing.Point(467, 437);
            this._btnHelp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._btnHelp.Name = "_btnHelp";
            this._btnHelp.Size = new System.Drawing.Size(154, 30);
            this._btnHelp.TabIndex = 18;
            this._btnHelp.Text = "Help";
            this._btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // _Label4
            // 
            this._Label4.AutoSize = true;
            this._Label4.Location = new System.Drawing.Point(414, 51);
            this._Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label4.Name = "_Label4";
            this._Label4.Size = new System.Drawing.Size(96, 15);
            this._Label4.TabIndex = 86;
            this._Label4.Text = "Local IP Address:";
            // 
            // _Label5
            // 
            this._Label5.AutoSize = true;
            this._Label5.Location = new System.Drawing.Point(31, 114);
            this._Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label5.Name = "_Label5";
            this._Label5.Size = new System.Drawing.Size(153, 15);
            this._Label5.TabIndex = 125;
            this._Label5.Text = "Password (for POP3/SMTP):";
            // 
            // _GroupBox1
            // 
            this._GroupBox1.Controls.Add(this._Label10);
            this._GroupBox1.Controls.Add(this._txtRMSRelayPort);
            this._GroupBox1.Controls.Add(this._Label6);
            this._GroupBox1.Controls.Add(this._txtRMSRelayIPPath);
            this._GroupBox1.Controls.Add(this._rdoUseRMSRelay);
            this._GroupBox1.Controls.Add(this._rdoUseCMS);
            this._GroupBox1.Location = new System.Drawing.Point(46, 295);
            this._GroupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._GroupBox1.Name = "_GroupBox1";
            this._GroupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._GroupBox1.Size = new System.Drawing.Size(670, 84);
            this._GroupBox1.TabIndex = 127;
            this._GroupBox1.TabStop = false;
            // 
            // _Label10
            // 
            this._Label10.AutoSize = true;
            this._Label10.Location = new System.Drawing.Point(507, 24);
            this._Label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label10.Name = "_Label10";
            this._Label10.Size = new System.Drawing.Size(87, 15);
            this._Label10.TabIndex = 132;
            this._Label10.Text = "RMS Relay Port";
            // 
            // _txtRMSRelayPort
            // 
            this._txtRMSRelayPort.Enabled = false;
            this._txtRMSRelayPort.Location = new System.Drawing.Point(481, 47);
            this._txtRMSRelayPort.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtRMSRelayPort.Name = "_txtRMSRelayPort";
            this._txtRMSRelayPort.Size = new System.Drawing.Size(151, 23);
            this._txtRMSRelayPort.TabIndex = 131;
            // 
            // _Label6
            // 
            this._Label6.AutoSize = true;
            this._Label6.Location = new System.Drawing.Point(293, 24);
            this._Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label6.Name = "_Label6";
            this._Label6.Size = new System.Drawing.Size(135, 15);
            this._Label6.TabIndex = 130;
            this._Label6.Text = "IP address of RMS Relay:";
            // 
            // _rdoUseRMSRelay
            // 
            this._rdoUseRMSRelay.AutoSize = true;
            this._rdoUseRMSRelay.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._rdoUseRMSRelay.Location = new System.Drawing.Point(20, 48);
            this._rdoUseRMSRelay.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoUseRMSRelay.Name = "_rdoUseRMSRelay";
            this._rdoUseRMSRelay.Size = new System.Drawing.Size(204, 19);
            this._rdoUseRMSRelay.TabIndex = 128;
            this._rdoUseRMSRelay.Text = "Connect via RMS Relay telnet port";
            this._rdoUseRMSRelay.UseVisualStyleBackColor = true;
            this._rdoUseRMSRelay.CheckedChanged += new System.EventHandler(this.rdoUseRMSRelay_CheckedChanged);
            // 
            // _rdoUseCMS
            // 
            this._rdoUseCMS.AutoSize = true;
            this._rdoUseCMS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._rdoUseCMS.Checked = true;
            this._rdoUseCMS.Location = new System.Drawing.Point(20, 22);
            this._rdoUseCMS.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._rdoUseCMS.Name = "_rdoUseCMS";
            this._rdoUseCMS.Size = new System.Drawing.Size(212, 19);
            this._rdoUseCMS.TabIndex = 127;
            this._rdoUseCMS.TabStop = true;
            this._rdoUseCMS.Text = "Connect directly to CMS telnet port";
            this._rdoUseCMS.UseVisualStyleBackColor = true;
            this._rdoUseCMS.CheckedChanged += new System.EventHandler(this.rdoUseCMS_CheckedChanged);
            // 
            // _Label7
            // 
            this._Label7.AutoSize = true;
            this._Label7.Location = new System.Drawing.Point(99, 260);
            this._Label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label7.Name = "_Label7";
            this._Label7.Size = new System.Drawing.Size(91, 15);
            this._Label7.TabIndex = 129;
            this._Label7.Text = "Service Code(s):";
            // 
            // _txtServiceCodes
            // 
            this._txtServiceCodes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this._txtServiceCodes.Location = new System.Drawing.Point(203, 258);
            this._txtServiceCodes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._txtServiceCodes.Name = "_txtServiceCodes";
            this._txtServiceCodes.Size = new System.Drawing.Size(229, 23);
            this._txtServiceCodes.TabIndex = 130;
            // 
            // _chkForceHFRouting
            // 
            this._chkForceHFRouting.AutoSize = true;
            this._chkForceHFRouting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._chkForceHFRouting.Location = new System.Drawing.Point(473, 196);
            this._chkForceHFRouting.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this._chkForceHFRouting.Name = "_chkForceHFRouting";
            this._chkForceHFRouting.Size = new System.Drawing.Size(243, 19);
            this._chkForceHFRouting.TabIndex = 131;
            this._chkForceHFRouting.Text = "Send messages via radio-only forwarding";
            this._chkForceHFRouting.UseVisualStyleBackColor = true;
            // 
            // _Label13
            // 
            this._Label13.AutoSize = true;
            this._Label13.Location = new System.Drawing.Point(596, 110);
            this._Label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label13.Name = "_Label13";
            this._Label13.Size = new System.Drawing.Size(134, 15);
            this._Label13.TabIndex = 132;
            this._Label13.Text = "(Optional country code)";
            // 
            // _Label14
            // 
            this._Label14.AutoSize = true;
            this._Label14.Location = new System.Drawing.Point(107, 80);
            this._Label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._Label14.Name = "_Label14";
            this._Label14.Size = new System.Drawing.Size(88, 15);
            this._Label14.TabIndex = 321;
            this._Label14.Text = "(Case sensitive)";
            // 
            // DialogSiteProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 492);
            this.Controls.Add(this._Label14);
            this.Controls.Add(this._Label13);
            this.Controls.Add(this._chkForceHFRouting);
            this.Controls.Add(this._txtServiceCodes);
            this.Controls.Add(this._Label7);
            this.Controls.Add(this._GroupBox1);
            this.Controls.Add(this._Label5);
            this.Controls.Add(this._txtPOP3Password);
            this.Controls.Add(this._chkEnableRadar);
            this.Controls.Add(this._Label4);
            this.Controls.Add(this._cmbLocalIPAddress);
            this.Controls.Add(this._chkLANAccessable);
            this.Controls.Add(this._Label3);
            this.Controls.Add(this._Label2);
            this.Controls.Add(this._Label23);
            this.Controls.Add(this._txtGridSquare);
            this.Controls.Add(this._txtPOP3PortNumber);
            this.Controls.Add(this._txtSecureLoginPassword);
            this.Controls.Add(this._Label21);
            this.Controls.Add(this._btnHelp);
            this.Controls.Add(this._txtSMTPPortNumber);
            this.Controls.Add(this._Label9);
            this.Controls.Add(this._txtSizeLimit);
            this.Controls.Add(this._Label8);
            this.Controls.Add(this._txtSiteCallsign);
            this.Controls.Add(this._Cancel_Button);
            this.Controls.Add(this._OK_Button);
            this.Controls.Add(this._Label26);
            this.Controls.Add(this._Label1);
            this.Controls.Add(this._txtPrefix);
            this.Controls.Add(this._Label11);
            this.Controls.Add(this._txtSuffix);
            this.Controls.Add(this._Label12);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogSiteProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Paclink Site Properties";
            this.Activated += new System.EventHandler(this.Properties_Activated);
            this.Load += new System.EventHandler(this.Properties_Load);
            this._GroupBox1.ResumeLayout(false);
            this._GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label _Label26;

        internal Label Label26
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label26;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label26 != null)
                {
                }

                _Label26 = value;
                if (_Label26 != null)
                {
                }
            }
        }

        private TextBox _txtPOP3PortNumber;

        internal TextBox txtPOP3PortNumber
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtPOP3PortNumber;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtPOP3PortNumber != null)
                {
                }

                _txtPOP3PortNumber = value;
                if (_txtPOP3PortNumber != null)
                {
                }
            }
        }

        private Label _Label23;

        internal Label Label23
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Label23;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Label23 != null)
                {
                }

                _Label23 = value;
                if (_Label23 != null)
                {
                }
            }
        }

        private TextBox _txtSMTPPortNumber;

        internal TextBox txtSMTPPortNumber
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSMTPPortNumber;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtSMTPPortNumber != null)
                {
                }

                _txtSMTPPortNumber = value;
                if (_txtSMTPPortNumber != null)
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

        private TextBox _txtSecureLoginPassword;

        internal TextBox txtSecureLoginPassword
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSecureLoginPassword;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtSecureLoginPassword != null)
                {
                }

                _txtSecureLoginPassword = value;
                if (_txtSecureLoginPassword != null)
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

        private TextBox _txtSiteCallsign;

        internal TextBox txtSiteCallsign
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSiteCallsign;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtSiteCallsign != null)
                {
                }

                _txtSiteCallsign = value;
                if (_txtSiteCallsign != null)
                {
                }
            }
        }

        private Button _OK_Button;

        internal Button OK_Button
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _OK_Button;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_OK_Button != null)
                {
                    _OK_Button.Click -= OK_Button_Click;
                }

                _OK_Button = value;
                if (_OK_Button != null)
                {
                    _OK_Button.Click += OK_Button_Click;
                }
            }
        }

        private Button _Cancel_Button;

        internal Button Cancel_Button
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Cancel_Button;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Cancel_Button != null)
                {
                    _Cancel_Button.Click -= Cancel_Button_Click;
                }

                _Cancel_Button = value;
                if (_Cancel_Button != null)
                {
                    _Cancel_Button.Click += Cancel_Button_Click;
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

        private TextBox _txtGridSquare;

        internal TextBox txtGridSquare
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtGridSquare;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtGridSquare != null)
                {
                }

                _txtGridSquare = value;
                if (_txtGridSquare != null)
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

        private CheckBox _chkLANAccessable;

        internal CheckBox chkLANAccessable
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkLANAccessable;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkLANAccessable != null)
                {
                }

                _chkLANAccessable = value;
                if (_chkLANAccessable != null)
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

        private TextBox _txtSizeLimit;

        internal TextBox txtSizeLimit
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSizeLimit;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtSizeLimit != null)
                {
                }

                _txtSizeLimit = value;
                if (_txtSizeLimit != null)
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

        private TextBox _txtPrefix;

        internal TextBox txtPrefix
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtPrefix;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtPrefix != null)
                {
                }

                _txtPrefix = value;
                if (_txtPrefix != null)
                {
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

        private TextBox _txtSuffix;

        internal TextBox txtSuffix
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtSuffix;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtSuffix != null)
                {
                }

                _txtSuffix = value;
                if (_txtSuffix != null)
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

        private ComboBox _cmbLocalIPAddress;

        internal ComboBox cmbLocalIPAddress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbLocalIPAddress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbLocalIPAddress != null)
                {
                }

                _cmbLocalIPAddress = value;
                if (_cmbLocalIPAddress != null)
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

        private CheckBox _chkEnableRadar;

        internal CheckBox chkEnableRadar
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkEnableRadar;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkEnableRadar != null)
                {
                }

                _chkEnableRadar = value;
                if (_chkEnableRadar != null)
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

        private TextBox _txtPOP3Password;

        internal TextBox txtPOP3Password
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtPOP3Password;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtPOP3Password != null)
                {
                }

                _txtPOP3Password = value;
                if (_txtPOP3Password != null)
                {
                }
            }
        }

        private GroupBox _GroupBox1;

        internal GroupBox GroupBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _GroupBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_GroupBox1 != null)
                {
                }

                _GroupBox1 = value;
                if (_GroupBox1 != null)
                {
                }
            }
        }

        private RadioButton _rdoUseRMSRelay;

        internal RadioButton rdoUseRMSRelay
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoUseRMSRelay;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoUseRMSRelay != null)
                {
                    _rdoUseRMSRelay.CheckedChanged -= rdoUseRMSRelay_CheckedChanged;
                }

                _rdoUseRMSRelay = value;
                if (_rdoUseRMSRelay != null)
                {
                    _rdoUseRMSRelay.CheckedChanged += rdoUseRMSRelay_CheckedChanged;
                }
            }
        }

        private RadioButton _rdoUseCMS;

        internal RadioButton rdoUseCMS
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdoUseCMS;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdoUseCMS != null)
                {
                    _rdoUseCMS.CheckedChanged -= rdoUseCMS_CheckedChanged;
                }

                _rdoUseCMS = value;
                if (_rdoUseCMS != null)
                {
                    _rdoUseCMS.CheckedChanged += rdoUseCMS_CheckedChanged;
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

        private TextBox _txtRMSRelayIPPath;

        internal TextBox txtRMSRelayIPPath
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtRMSRelayIPPath;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtRMSRelayIPPath != null)
                {
                }

                _txtRMSRelayIPPath = value;
                if (_txtRMSRelayIPPath != null)
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

        private TextBox _txtServiceCodes;

        internal TextBox txtServiceCodes
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtServiceCodes;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtServiceCodes != null)
                {
                }

                _txtServiceCodes = value;
                if (_txtServiceCodes != null)
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

        private TextBox _txtRMSRelayPort;

        internal TextBox txtRMSRelayPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtRMSRelayPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtRMSRelayPort != null)
                {
                }

                _txtRMSRelayPort = value;
                if (_txtRMSRelayPort != null)
                {
                }
            }
        }

        private CheckBox _chkForceHFRouting;

        internal CheckBox chkForceHFRouting
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkForceHFRouting;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkForceHFRouting != null)
                {
                }

                _chkForceHFRouting = value;
                if (_chkForceHFRouting != null)
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
    }
}