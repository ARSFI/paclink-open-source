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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSiteProperties));
            _Label26 = new Label();
            _txtPOP3PortNumber = new TextBox();
            _Label23 = new Label();
            _txtSMTPPortNumber = new TextBox();
            _Label21 = new Label();
            _txtSecureLoginPassword = new TextBox();
            _Label2 = new Label();
            _Label3 = new Label();
            _txtGridSquare = new TextBox();
            _txtSiteCallsign = new TextBox();
            _chkLANAccessable = new CheckBox();
            _Label9 = new Label();
            _txtSizeLimit = new TextBox();
            _Label1 = new Label();
            _Label11 = new Label();
            _txtPrefix = new TextBox();
            _Label12 = new Label();
            _txtSuffix = new TextBox();
            _OK_Button = new Button();
            _OK_Button.Click += new EventHandler(OK_Button_Click);
            _Cancel_Button = new Button();
            _Cancel_Button.Click += new EventHandler(Cancel_Button_Click);
            _Label8 = new Label();
            _chkAddToOutlookExpress = new CheckBox();
            _ToolTip1 = new ToolTip(components);
            _cmbLocalIPAddress = new ComboBox();
            _chkEnableRadar = new CheckBox();
            _txtPOP3Password = new TextBox();
            _txtRMSRelayIPPath = new TextBox();
            _btnHelp = new Button();
            _btnHelp.Click += new EventHandler(btnHelp_Click);
            _Label4 = new Label();
            _Label5 = new Label();
            _GroupBox1 = new GroupBox();
            _Label10 = new Label();
            _txtRMSRelayPort = new TextBox();
            _Label6 = new Label();
            _rdoUseRMSRelay = new RadioButton();
            _rdoUseRMSRelay.CheckedChanged += new EventHandler(rdoUseRMSRelay_CheckedChanged);
            _rdoUseCMS = new RadioButton();
            _rdoUseCMS.CheckedChanged += new EventHandler(rdoUseCMS_CheckedChanged);
            _Label7 = new Label();
            _txtServiceCodes = new TextBox();
            _chkForceHFRouting = new CheckBox();
            _Label13 = new Label();
            _chkAutoupdateTest = new CheckBox();
            _Label14 = new Label();
            _GroupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // Label26
            // 
            _Label26.Location = new Point(53, 342);
            _Label26.Name = "_Label26";
            _Label26.Size = new Size(540, 31);
            _Label26.TabIndex = 3;
            _Label26.Text = resources.GetString("Label26.Text");
            // 
            // txtPOP3PortNumber
            // 
            _txtPOP3PortNumber.CharacterCasing = CharacterCasing.Upper;
            _txtPOP3PortNumber.Location = new Point(174, 196);
            _txtPOP3PortNumber.Name = "_txtPOP3PortNumber";
            _txtPOP3PortNumber.Size = new Size(68, 20);
            _txtPOP3PortNumber.TabIndex = 6;
            _txtPOP3PortNumber.Text = "110";
            _txtPOP3PortNumber.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtPOP3PortNumber, "POP3 Port number, change only if confilct with other program or service (default " + "= 110)");
            // 
            // Label23
            // 
            _Label23.AutoSize = true;
            _Label23.Location = new Point(70, 199);
            _Label23.Name = "_Label23";
            _Label23.Size = new Size(100, 13);
            _Label23.TabIndex = 51;
            _Label23.Text = "POP3 Port Number:";
            // 
            // txtSMTPPortNumber
            // 
            _txtSMTPPortNumber.CharacterCasing = CharacterCasing.Upper;
            _txtSMTPPortNumber.Location = new Point(174, 170);
            _txtSMTPPortNumber.Name = "_txtSMTPPortNumber";
            _txtSMTPPortNumber.Size = new Size(68, 20);
            _txtSMTPPortNumber.TabIndex = 5;
            _txtSMTPPortNumber.Text = "25";
            _txtSMTPPortNumber.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtSMTPPortNumber, "SMTP Port number, change only if confilct with other program or service (default " + "= 25)");
            // 
            // Label21
            // 
            _Label21.AutoSize = true;
            _Label21.Location = new Point(68, 173);
            _Label21.Name = "_Label21";
            _Label21.Size = new Size(102, 13);
            _Label21.TabIndex = 48;
            _Label21.Text = "SMTP Port Number:";
            // 
            // txtSecureLoginPassword
            // 
            _txtSecureLoginPassword.Location = new Point(174, 66);
            _txtSecureLoginPassword.Name = "_txtSecureLoginPassword";
            _txtSecureLoginPassword.Size = new Size(111, 20);
            _txtSecureLoginPassword.TabIndex = 1;
            _txtSecureLoginPassword.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtSecureLoginPassword, "Winlink Password for this callsign and -ssid");
            // 
            // Label2
            // 
            _Label2.AutoSize = true;
            _Label2.Location = new Point(39, 69);
            _Label2.Name = "_Label2";
            _Label2.Size = new Size(56, 13);
            _Label2.TabIndex = 3;
            _Label2.Text = "Password:";
            // 
            // Label3
            // 
            _Label3.AutoSize = true;
            _Label3.Location = new Point(87, 121);
            _Label3.Name = "_Label3";
            _Label3.Size = new Size(83, 13);
            _Label3.TabIndex = 70;
            _Label3.Text = "Site grid square:";
            // 
            // txtGridSquare
            // 
            _txtGridSquare.CharacterCasing = CharacterCasing.Upper;
            _txtGridSquare.Location = new Point(174, 118);
            _txtGridSquare.Name = "_txtGridSquare";
            _txtGridSquare.Size = new Size(111, 20);
            _txtGridSquare.TabIndex = 3;
            _txtGridSquare.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtGridSquare, "Grid square location (6 character) ");
            // 
            // txtSiteCallsign
            // 
            _txtSiteCallsign.CharacterCasing = CharacterCasing.Upper;
            _txtSiteCallsign.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _txtSiteCallsign.Location = new Point(174, 37);
            _txtSiteCallsign.Name = "_txtSiteCallsign";
            _txtSiteCallsign.Size = new Size(111, 20);
            _txtSiteCallsign.TabIndex = 0;
            _txtSiteCallsign.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtSiteCallsign, "Valid radio callsign (including optional -SSID) for this site.");
            // 
            // chkLANAccessable
            // 
            _chkLANAccessable.AutoSize = true;
            _chkLANAccessable.CheckAlign = ContentAlignment.MiddleRight;
            _chkLANAccessable.Location = new Point(522, 133);
            _chkLANAccessable.Name = "_chkLANAccessable";
            _chkLANAccessable.Size = new Size(101, 17);
            _chkLANAccessable.TabIndex = 12;
            _chkLANAccessable.Text = "LAN Accessible";
            _ToolTip1.SetToolTip(_chkLANAccessable, "Check if this Paclink Instance will be accessed by other computers on the local L" + "AN");
            _chkLANAccessable.UseVisualStyleBackColor = true;
            // 
            // Label9
            // 
            _Label9.AutoSize = true;
            _Label9.Location = new Point(10, 147);
            _Label9.Name = "_Label9";
            _Label9.Size = new Size(160, 13);
            _Label9.TabIndex = 75;
            _Label9.Text = "Message size limit (compressed):";
            // 
            // txtSizeLimit
            // 
            _txtSizeLimit.Location = new Point(174, 144);
            _txtSizeLimit.Name = "_txtSizeLimit";
            _txtSizeLimit.Size = new Size(111, 20);
            _txtSizeLimit.TabIndex = 4;
            _ToolTip1.SetToolTip(_txtSizeLimit, "Attachment limit in bytes. 0 removes all attachments. (Default=120000)");
            // 
            // Label1
            // 
            _Label1.AutoSize = true;
            _Label1.Location = new Point(75, 40);
            _Label1.Name = "_Label1";
            _Label1.Size = new Size(95, 13);
            _Label1.TabIndex = 68;
            _Label1.Text = "Callsign (no SSID):";
            // 
            // Label11
            // 
            _Label11.AutoSize = true;
            _Label11.Location = new Point(355, 69);
            _Label11.Name = "_Label11";
            _Label11.Size = new Size(89, 13);
            _Label11.TabIndex = 80;
            _Label11.Text = "Callsign ID Prefix:";
            // 
            // txtPrefix
            // 
            _txtPrefix.Location = new Point(450, 66);
            _txtPrefix.Name = "_txtPrefix";
            _txtPrefix.Size = new Size(61, 20);
            _txtPrefix.TabIndex = 8;
            _ToolTip1.SetToolTip(_txtPrefix, "Prefix ID. Does not affect callsign or addresses (Default = blank)");
            // 
            // Label12
            // 
            _Label12.AutoSize = true;
            _Label12.Location = new Point(355, 96);
            _Label12.Name = "_Label12";
            _Label12.Size = new Size(89, 13);
            _Label12.TabIndex = 82;
            _Label12.Text = "Callsign ID Suffix:";
            // 
            // txtSuffix
            // 
            _txtSuffix.Location = new Point(450, 92);
            _txtSuffix.Name = "_txtSuffix";
            _txtSuffix.Size = new Size(61, 20);
            _txtSuffix.TabIndex = 9;
            _ToolTip1.SetToolTip(_txtSuffix, "Suffix ID. Does not affect callsign or addresses (Default = blank)");
            // 
            // OK_Button
            // 
            _OK_Button.Location = new Point(115, 379);
            _OK_Button.Name = "_OK_Button";
            _OK_Button.Size = new Size(132, 26);
            _OK_Button.TabIndex = 16;
            _OK_Button.Text = "Update";
            // 
            // Cancel_Button
            // 
            _Cancel_Button.DialogResult = DialogResult.Cancel;
            _Cancel_Button.Location = new Point(258, 379);
            _Cancel_Button.Name = "_Cancel_Button";
            _Cancel_Button.Size = new Size(132, 26);
            _Cancel_Button.TabIndex = 17;
            _Cancel_Button.Text = "Close";
            // 
            // Label8
            // 
            _Label8.AutoSize = true;
            _Label8.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(0));
            _Label8.Location = new Point(171, 9);
            _Label8.Name = "_Label8";
            _Label8.Size = new Size(311, 16);
            _Label8.TabIndex = 74;
            _Label8.Text = "Enter the properties for this Paclink instance";
            // 
            // chkAddToOutlookExpress
            // 
            _chkAddToOutlookExpress.AutoSize = true;
            _chkAddToOutlookExpress.CheckAlign = ContentAlignment.MiddleRight;
            _chkAddToOutlookExpress.Location = new Point(425, 169);
            _chkAddToOutlookExpress.Name = "_chkAddToOutlookExpress";
            _chkAddToOutlookExpress.Size = new Size(198, 17);
            _chkAddToOutlookExpress.TabIndex = 14;
            _chkAddToOutlookExpress.Text = "Add this account to Outlook Express";
            _ToolTip1.SetToolTip(_chkAddToOutlookExpress, "Check to add this account automatically to Outlook Express (accounts must be adde" + "d manually to other E-mail clients)");
            _chkAddToOutlookExpress.UseVisualStyleBackColor = true;
            // 
            // cmbLocalIPAddress
            // 
            _cmbLocalIPAddress.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbLocalIPAddress.FormattingEnabled = true;
            _cmbLocalIPAddress.Location = new Point(451, 38);
            _cmbLocalIPAddress.Name = "_cmbLocalIPAddress";
            _cmbLocalIPAddress.Size = new Size(174, 21);
            _cmbLocalIPAddress.TabIndex = 7;
            _ToolTip1.SetToolTip(_cmbLocalIPAddress, "Select default Local IP Addresss. Set to \"default\" unless multihomed.");
            // 
            // chkEnableRadar
            // 
            _chkEnableRadar.AutoSize = true;
            _chkEnableRadar.CheckAlign = ContentAlignment.MiddleRight;
            _chkEnableRadar.Location = new Point(468, 151);
            _chkEnableRadar.Name = "_chkEnableRadar";
            _chkEnableRadar.Size = new Size(155, 17);
            _chkEnableRadar.TabIndex = 13;
            _chkEnableRadar.Text = "Range and Bearing Display";
            _ToolTip1.SetToolTip(_chkEnableRadar, "Check to enable range & Bearing \"Radar\" display upon connect");
            _chkEnableRadar.UseVisualStyleBackColor = true;
            // 
            // txtPOP3Password
            // 
            _txtPOP3Password.Location = new Point(174, 92);
            _txtPOP3Password.Name = "_txtPOP3Password";
            _txtPOP3Password.Size = new Size(111, 20);
            _txtPOP3Password.TabIndex = 2;
            _txtPOP3Password.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtPOP3Password, "Grid square location (6 character) ");
            // 
            // txtRMSRelayIPPath
            // 
            _txtRMSRelayIPPath.Enabled = false;
            _txtRMSRelayIPPath.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(0));
            _txtRMSRelayIPPath.Location = new Point(240, 41);
            _txtRMSRelayIPPath.Name = "_txtRMSRelayIPPath";
            _txtRMSRelayIPPath.Size = new Size(140, 20);
            _txtRMSRelayIPPath.TabIndex = 129;
            _txtRMSRelayIPPath.Text = "localhost";
            _txtRMSRelayIPPath.TextAlign = HorizontalAlignment.Center;
            _ToolTip1.SetToolTip(_txtRMSRelayIPPath, "Valid radio callsign (including optional -SSID) for this site.");
            // 
            // btnHelp
            // 
            _btnHelp.DialogResult = DialogResult.Cancel;
            _btnHelp.Location = new Point(400, 379);
            _btnHelp.Name = "_btnHelp";
            _btnHelp.Size = new Size(132, 26);
            _btnHelp.TabIndex = 18;
            _btnHelp.Text = "Help";
            // 
            // Label4
            // 
            _Label4.AutoSize = true;
            _Label4.Location = new Point(355, 44);
            _Label4.Name = "_Label4";
            _Label4.Size = new Size(90, 13);
            _Label4.TabIndex = 86;
            _Label4.Text = "Local IP Address:";
            // 
            // Label5
            // 
            _Label5.AutoSize = true;
            _Label5.Location = new Point(27, 99);
            _Label5.Name = "_Label5";
            _Label5.Size = new Size(143, 13);
            _Label5.TabIndex = 125;
            _Label5.Text = "Password (for POP3/SMTP):";
            // 
            // GroupBox1
            // 
            _GroupBox1.Controls.Add(_Label10);
            _GroupBox1.Controls.Add(_txtRMSRelayPort);
            _GroupBox1.Controls.Add(_Label6);
            _GroupBox1.Controls.Add(_txtRMSRelayIPPath);
            _GroupBox1.Controls.Add(_rdoUseRMSRelay);
            _GroupBox1.Controls.Add(_rdoUseCMS);
            _GroupBox1.Location = new Point(39, 256);
            _GroupBox1.Name = "_GroupBox1";
            _GroupBox1.Size = new Size(574, 73);
            _GroupBox1.TabIndex = 127;
            _GroupBox1.TabStop = false;
            // 
            // Label10
            // 
            _Label10.AutoSize = true;
            _Label10.Location = new Point(435, 21);
            _Label10.Name = "_Label10";
            _Label10.Size = new Size(83, 13);
            _Label10.TabIndex = 132;
            _Label10.Text = "RMS Relay Port";
            // 
            // txtRMSRelayPort
            // 
            _txtRMSRelayPort.Enabled = false;
            _txtRMSRelayPort.Location = new Point(412, 41);
            _txtRMSRelayPort.Name = "_txtRMSRelayPort";
            _txtRMSRelayPort.Size = new Size(130, 20);
            _txtRMSRelayPort.TabIndex = 131;
            // 
            // Label6
            // 
            _Label6.AutoSize = true;
            _Label6.Location = new Point(251, 21);
            _Label6.Name = "_Label6";
            _Label6.Size = new Size(129, 13);
            _Label6.TabIndex = 130;
            _Label6.Text = "IP address of RMS Relay:";
            // 
            // rdoUseRMSRelay
            // 
            _rdoUseRMSRelay.AutoSize = true;
            _rdoUseRMSRelay.CheckAlign = ContentAlignment.MiddleRight;
            _rdoUseRMSRelay.Location = new Point(17, 42);
            _rdoUseRMSRelay.Name = "_rdoUseRMSRelay";
            _rdoUseRMSRelay.Size = new Size(189, 17);
            _rdoUseRMSRelay.TabIndex = 128;
            _rdoUseRMSRelay.Text = "Connect via RMS Relay telnet port";
            _rdoUseRMSRelay.UseVisualStyleBackColor = true;
            // 
            // rdoUseCMS
            // 
            _rdoUseCMS.AutoSize = true;
            _rdoUseCMS.CheckAlign = ContentAlignment.MiddleRight;
            _rdoUseCMS.Checked = true;
            _rdoUseCMS.Location = new Point(17, 19);
            _rdoUseCMS.Name = "_rdoUseCMS";
            _rdoUseCMS.Size = new Size(189, 17);
            _rdoUseCMS.TabIndex = 127;
            _rdoUseCMS.TabStop = true;
            _rdoUseCMS.Text = "Connect directly to CMS telnet port";
            _rdoUseCMS.UseVisualStyleBackColor = true;
            // 
            // Label7
            // 
            _Label7.AutoSize = true;
            _Label7.Location = new Point(85, 225);
            _Label7.Name = "_Label7";
            _Label7.Size = new Size(85, 13);
            _Label7.TabIndex = 129;
            _Label7.Text = "Service Code(s):";
            // 
            // txtServiceCodes
            // 
            _txtServiceCodes.CharacterCasing = CharacterCasing.Upper;
            _txtServiceCodes.Location = new Point(174, 224);
            _txtServiceCodes.Name = "_txtServiceCodes";
            _txtServiceCodes.Size = new Size(197, 20);
            _txtServiceCodes.TabIndex = 130;
            // 
            // chkForceHFRouting
            // 
            _chkForceHFRouting.AutoSize = true;
            _chkForceHFRouting.CheckAlign = ContentAlignment.MiddleRight;
            _chkForceHFRouting.Location = new Point(405, 187);
            _chkForceHFRouting.Name = "_chkForceHFRouting";
            _chkForceHFRouting.Size = new Size(218, 17);
            _chkForceHFRouting.TabIndex = 131;
            _chkForceHFRouting.Text = "Send messages via radio-only forwarding";
            _chkForceHFRouting.UseVisualStyleBackColor = true;
            // 
            // Label13
            // 
            _Label13.AutoSize = true;
            _Label13.Location = new Point(511, 95);
            _Label13.Name = "_Label13";
            _Label13.Size = new Size(117, 13);
            _Label13.TabIndex = 132;
            _Label13.Text = "(Optional country code)";
            // 
            // chkAutoupdateTest
            // 
            _chkAutoupdateTest.AutoSize = true;
            _chkAutoupdateTest.CheckAlign = ContentAlignment.MiddleRight;
            _chkAutoupdateTest.Location = new Point(406, 205);
            _chkAutoupdateTest.Name = "_chkAutoupdateTest";
            _chkAutoupdateTest.Size = new Size(217, 17);
            _chkAutoupdateTest.TabIndex = 320;
            _chkAutoupdateTest.TabStop = false;
            _chkAutoupdateTest.Text = "Install field-test (beta) versions of Paclink";
            _chkAutoupdateTest.UseVisualStyleBackColor = true;
            // 
            // Label14
            // 
            _Label14.AutoSize = true;
            _Label14.Location = new Point(92, 69);
            _Label14.Name = "_Label14";
            _Label14.Size = new Size(81, 13);
            _Label14.TabIndex = 321;
            _Label14.Text = "(Case sensitive)";
            // 
            // DialogSiteProperties
            // 
            AutoScaleDimensions = new SizeF(6.0F, 13.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(653, 426);
            Controls.Add(_Label14);
            Controls.Add(_chkAutoupdateTest);
            Controls.Add(_Label13);
            Controls.Add(_chkForceHFRouting);
            Controls.Add(_txtServiceCodes);
            Controls.Add(_Label7);
            Controls.Add(_GroupBox1);
            Controls.Add(_Label5);
            Controls.Add(_txtPOP3Password);
            Controls.Add(_chkEnableRadar);
            Controls.Add(_Label4);
            Controls.Add(_cmbLocalIPAddress);
            Controls.Add(_chkLANAccessable);
            Controls.Add(_Label3);
            Controls.Add(_Label2);
            Controls.Add(_Label23);
            Controls.Add(_txtGridSquare);
            Controls.Add(_txtPOP3PortNumber);
            Controls.Add(_txtSecureLoginPassword);
            Controls.Add(_Label21);
            Controls.Add(_btnHelp);
            Controls.Add(_txtSMTPPortNumber);
            Controls.Add(_chkAddToOutlookExpress);
            Controls.Add(_Label9);
            Controls.Add(_txtSizeLimit);
            Controls.Add(_Label8);
            Controls.Add(_txtSiteCallsign);
            Controls.Add(_Cancel_Button);
            Controls.Add(_OK_Button);
            Controls.Add(_Label26);
            Controls.Add(_Label1);
            Controls.Add(_txtPrefix);
            Controls.Add(_Label11);
            Controls.Add(_txtSuffix);
            Controls.Add(_Label12);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogSiteProperties";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Paclink Site Properties";
            _GroupBox1.ResumeLayout(false);
            _GroupBox1.PerformLayout();
            Activated += new EventHandler(Properties_Activated);
            Load += new EventHandler(Properties_Load);
            ResumeLayout(false);
            PerformLayout();
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

        private CheckBox _chkAddToOutlookExpress;

        internal CheckBox chkAddToOutlookExpress
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkAddToOutlookExpress;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkAddToOutlookExpress != null)
                {
                }

                _chkAddToOutlookExpress = value;
                if (_chkAddToOutlookExpress != null)
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

        private CheckBox _chkAutoupdateTest;

        internal CheckBox chkAutoupdateTest
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _chkAutoupdateTest;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_chkAutoupdateTest != null)
                {
                }

                _chkAutoupdateTest = value;
                if (_chkAutoupdateTest != null)
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