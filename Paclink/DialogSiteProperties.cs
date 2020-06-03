using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using nsoftware.IPWorks;

namespace Paclink
{
    public partial class DialogSiteProperties
    {
        public DialogSiteProperties()
        {
            objTestPort = new Ipdaemon();
            InitializeComponent();
            _Label26.Name = "Label26";
            _txtPOP3PortNumber.Name = "txtPOP3PortNumber";
            _Label23.Name = "Label23";
            _txtSMTPPortNumber.Name = "txtSMTPPortNumber";
            _Label21.Name = "Label21";
            _txtSecureLoginPassword.Name = "txtSecureLoginPassword";
            _Label2.Name = "Label2";
            _Label3.Name = "Label3";
            _txtGridSquare.Name = "txtGridSquare";
            _txtSiteCallsign.Name = "txtSiteCallsign";
            _chkLANAccessable.Name = "chkLANAccessable";
            _Label9.Name = "Label9";
            _txtSizeLimit.Name = "txtSizeLimit";
            _Label1.Name = "Label1";
            _Label11.Name = "Label11";
            _txtPrefix.Name = "txtPrefix";
            _Label12.Name = "Label12";
            _txtSuffix.Name = "txtSuffix";
            _OK_Button.Name = "OK_Button";
            _Cancel_Button.Name = "Cancel_Button";
            _Label8.Name = "Label8";
            _chkAddToOutlookExpress.Name = "chkAddToOutlookExpress";
            _cmbLocalIPAddress.Name = "cmbLocalIPAddress";
            _chkEnableRadar.Name = "chkEnableRadar";
            _txtPOP3Password.Name = "txtPOP3Password";
            _txtRMSRelayIPPath.Name = "txtRMSRelayIPPath";
            _btnHelp.Name = "btnHelp";
            _Label4.Name = "Label4";
            _Label5.Name = "Label5";
            _GroupBox1.Name = "GroupBox1";
            _Label10.Name = "Label10";
            _txtRMSRelayPort.Name = "txtRMSRelayPort";
            _Label6.Name = "Label6";
            _rdoUseRMSRelay.Name = "rdoUseRMSRelay";
            _rdoUseCMS.Name = "rdoUseCMS";
            _Label7.Name = "Label7";
            _txtServiceCodes.Name = "txtServiceCodes";
            _chkForceHFRouting.Name = "chkForceHFRouting";
            _Label13.Name = "Label13";
            _chkAutoupdateTest.Name = "chkAutoupdateTest";
            _Label14.Name = "Label14";
        }

        private Ipdaemon _objTestPort;

        private Ipdaemon objTestPort
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _objTestPort;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_objTestPort != null)
                {
                }

                _objTestPort = value;
                if (_objTestPort != null)
                {
                }
            }
        }

        public static bool IsValid()
        {
            if (!Globals.IsValidRadioCallsign(Globals.SiteCallsign))
                return false;
            if (Globals.SiteGridSquare.Length != 4 & Globals.SiteGridSquare.Length != 6)
                return false;
            return true;
        } // IsValid

        private void Properties_Activated(object sender, EventArgs e)
        {
            txtSiteCallsign.Focus();
        } // Properties_Activated

        private void Properties_Load(object sender, EventArgs e)
        {
            // 
            // Opens the profile dialog box.
            // 
            rdoUseCMS.Checked = true;
            txtRMSRelayIPPath.Text = Globals.strRMSRelayIPPath;
            txtRMSRelayPort.Text = Globals.intRMSRelayPort.ToString();
            txtRMSRelayIPPath.Enabled = false;
            txtRMSRelayPort.Enabled = false;
            rdoUseRMSRelay.Checked = Globals.blnUseRMSRelay;
            txtSiteCallsign.Text = Globals.SiteCallsign;
            txtServiceCodes.Text = Globals.strServiceCodes;
            txtSecureLoginPassword.Text = Globals.SecureLoginPassword;
            txtPOP3Password.Text = Globals.POP3Password;
            txtGridSquare.Text = Globals.SiteGridSquare;
            txtSMTPPortNumber.Text = Globals.intSMTPPortNumber.ToString();
            txtPOP3PortNumber.Text = Globals.intPOP3PortNumber.ToString();
            chkLANAccessable.Checked = Globals.blnLAN;
            chkEnableRadar.Checked = Globals.blnEnableRadar;
            chkForceHFRouting.Checked = Globals.blnForceHFRouting;
            chkAutoupdateTest.Checked = Globals.blnAutoupdateTest;
            txtPrefix.Text = Globals.objINIFile.GetString("Properties", "Prefix", "");
            txtSuffix.Text = Globals.objINIFile.GetString("Properties", "Suffix", "");
            txtSizeLimit.Text = Globals.objINIFile.GetString("Properties", "Size Limit", "120000");
            txtSecureLoginPassword.Enabled = true;
            if (txtSecureLoginPassword.Enabled == false)
                txtSecureLoginPassword.Text = "";

            // Initialize the local IP addresses 
            cmbLocalIPAddress.Items.Clear();
            foreach (string strAddress in Globals.strLocalIPAddresses)
            {
                if (strAddress is object)
                    cmbLocalIPAddress.Items.Add(strAddress);
            }

            try
            {
                cmbLocalIPAddress.SelectedIndex = Globals.objINIFile.GetInteger("Properties", "Default Local IP Address Index", 0);
            }
            catch
            {
                if (cmbLocalIPAddress.Items.Count > 0)
                {
                    cmbLocalIPAddress.Text = Conversions.ToString(cmbLocalIPAddress.Items[0]);
                    cmbLocalIPAddress.SelectedIndex = 0;
                }
            }
        } // Properties_Load

        private void OK_Button_Click(object sender, EventArgs e)
        {
            // Save the profile data...
            if (!Information.IsNumeric(txtSizeLimit.Text))
            {
                Interaction.MsgBox("Attachment limit must be between 0 and 120000. 0 blocks all attachments.", MsgBoxStyle.Exclamation, "Attachment Limit Error");
                return;
            }
            else if (Conversions.ToInteger(txtSizeLimit.Text) < 0 | Conversions.ToInteger(txtSizeLimit.Text) > 120000)
            {
                Interaction.MsgBox("Attachment limit must be between 0 and 120000. 0 blocks all attachments.", MsgBoxStyle.Exclamation, "Attachment Limit Error");
                return;
            }

            if (!Information.IsNumeric(txtPOP3PortNumber.Text))
            {
                Interaction.MsgBox("POP3 port number must be between 0 and 65535.", MsgBoxStyle.Exclamation, "Port Number Error");
                return;
            }

            if (!Information.IsNumeric(txtSMTPPortNumber.Text))
            {
                Interaction.MsgBox("SMTP port number must be between 0 and 65535.", MsgBoxStyle.Exclamation, "Port Number Error");
                return;
            }

            string strExistingCallsign = "";
            bool blnNewInstallation = false;
            txtSiteCallsign.Text = txtSiteCallsign.Text.ToUpper().Trim();
            txtSecureLoginPassword.Text = txtSecureLoginPassword.Text.Trim();
            txtPOP3Password.Text = txtPOP3Password.Text.Trim();
            txtGridSquare.Text = txtGridSquare.Text.ToUpper().Trim();
            txtServiceCodes.Text = txtServiceCodes.Text.ToUpper().Trim();
            bool blnValidSSID = true;
            var strTokens = txtSiteCallsign.Text.Split('-');
            if (strTokens.Length == 2)
            {
                if (Information.IsNumeric(strTokens[1]))
                {
                    int intSSID = Conversions.ToInteger(strTokens[1]);
                    if (intSSID >= 1 & intSSID <= 15)
                    {
                        blnValidSSID = true;
                    }
                    else
                    {
                        blnValidSSID = false;
                    }
                }
                else
                {
                    blnValidSSID = false;
                }
            }

            if (!blnValidSSID)
            {
                Interaction.MsgBox("A callsign without SSID or any SSID value of 1 through 15 is acceptable.", MsgBoxStyle.Information, "Incorrect SSID");
                txtSiteCallsign.Focus();
                return;
            }

            if (!Globals.IsValidRadioCallsign(txtSiteCallsign.Text))
            {
                Interaction.MsgBox("A valid radio callsign is required...");
                txtSiteCallsign.Focus();
                return;
            }

            if (txtSecureLoginPassword.Text.Trim().Length < 6)
            {
                Interaction.MsgBox("You must enter a password that is at least 6 characters long.");
                txtSecureLoginPassword.Focus();
                return;
            }

            if (txtPOP3Password.Text.Length < 4)
            {
                Interaction.MsgBox("A valid POP3/SMTP password is required...");
                txtPOP3Password.Focus();
                return;
            }

            if (txtGridSquare.Text.Trim().Length < 6)
            {
                Interaction.MsgBox("A 6 character grid square entry is required...");
                txtGridSquare.Focus();
                return;
            }

            Globals.SiteCallsign = txtSiteCallsign.Text;
            Globals.SecureLoginPassword = txtSecureLoginPassword.Text.Trim();
            Globals.POP3Password = txtPOP3Password.Text.ToUpper().Trim();
            Globals.SiteGridSquare = txtGridSquare.Text.Trim();
            Globals.strServiceCodes = txtServiceCodes.Text.ToUpper().Trim();
            Globals.intSMTPPortNumber = Conversions.ToInteger(txtSMTPPortNumber.Text);
            Globals.intPOP3PortNumber = Conversions.ToInteger(txtPOP3PortNumber.Text);
            Globals.blnLAN = chkLANAccessable.Checked;
            Globals.blnEnableRadar = chkEnableRadar.Checked;
            Globals.blnUseRMSRelay = rdoUseRMSRelay.Checked;
            Globals.strRMSRelayIPPath = txtRMSRelayIPPath.Text;
            Globals.intRMSRelayPort = Convert.ToInt32(txtRMSRelayPort.Text);
            Globals.blnForceHFRouting = chkForceHFRouting.Checked;
            Globals.blnAutoupdateTest = chkAutoupdateTest.Checked;

            // Tell WinlinkInterop what our callsign is.
            if (Globals.objWL2KInterop is object)
                Globals.objWL2KInterop.SetCallsign(Globals.SiteCallsign);

            // Test the POP3 and SMTP Port numbers
            objTestPort.LocalPort = Globals.intPOP3PortNumber;
            try
            {
                objTestPort.Listening = true;
                objTestPort.Listening = false;
            }
            catch
            {
                Interaction.MsgBox("Conflict on POP3 port number " + Globals.intPOP3PortNumber.ToString() + ".  Port is probably in use by another service or program. Try port number " + (Globals.intPOP3PortNumber + 1).ToString() + ".", MsgBoxStyle.Exclamation, "POP3 Port Conflict");
                txtPOP3PortNumber.Focus();
                return;
            }

            objTestPort.LocalPort = Globals.intSMTPPortNumber;
            try
            {
                objTestPort.Listening = true;
                objTestPort.Listening = false;
            }
            catch
            {
                Interaction.MsgBox("Conflict on SMTP port number " + Globals.intSMTPPortNumber.ToString() + ".  Port is probably in use by another service or program. Try port number " + (Globals.intSMTPPortNumber + 1).ToString() + ".", MsgBoxStyle.Exclamation, "SMTP Port Conflict");
                txtSMTPPortNumber.Focus();
                return;
            }

            Globals.objINIFile.WriteString("Properties", "Program Directory", Globals.SiteBinDirectory);
            Globals.objINIFile.WriteString("Properties", "Site Callsign", Globals.SiteCallsign);
            Globals.objINIFile.WriteString("Properties", "Site Password", "");
            Globals.objINIFile.WriteString("Properties", "Secure Login Password", Globals.SecureLoginPassword);
            Globals.objINIFile.WriteString("Properties", "EMail Password", Globals.POP3Password);
            Globals.objINIFile.WriteString("Properties", "ServiceCodes", Globals.strServiceCodes);
            Globals.objINIFile.WriteString("Properties", "Grid Square", Globals.SiteGridSquare);
            Globals.objINIFile.WriteInteger("Properties", "SMTP Port Number", Globals.intSMTPPortNumber);
            Globals.objINIFile.WriteInteger("Properties", "POP3 Port Number", Globals.intPOP3PortNumber);
            Globals.objINIFile.WriteBoolean("Properties", "LAN Connection", Globals.blnLAN);
            Globals.objINIFile.WriteBoolean("Properties", "Enable Radar", Globals.blnEnableRadar);
            Globals.objINIFile.WriteString("Properties", "Prefix", txtPrefix.Text.Trim());
            Globals.objINIFile.WriteString("Properties", "Suffix", txtSuffix.Text.Trim());
            Globals.objINIFile.WriteString("Properties", "Size Limit", txtSizeLimit.Text.Trim());
            Globals.objINIFile.WriteInteger("Properties", "Default Local IP Address Index", cmbLocalIPAddress.SelectedIndex);
            Globals.objINIFile.WriteBoolean("Properties", "Use RMS Relay", Globals.blnUseRMSRelay);
            Globals.objINIFile.WriteString("Properties", "Local IP Path", Globals.strRMSRelayIPPath);
            Globals.objINIFile.WriteInteger("Properties", "RMS Relay Port", Globals.intRMSRelayPort);
            Globals.objINIFile.WriteBoolean("Properties", "Force radio-only", Globals.blnForceHFRouting);
            Globals.objINIFile.WriteBoolean(Application.ProductName, "Start Minimized", Globals.blnStartMinimized);
            Globals.objINIFile.WriteBoolean("Main", "Test Autoupdate", Globals.blnAutoupdateTest);
            try
            {
                Globals.strLocalIPAddress = Globals.strLocalIPAddresses[cmbLocalIPAddress.SelectedIndex];
            }
            catch
            {
            }

            if (!string.IsNullOrEmpty(strExistingCallsign))
            {
                if (strExistingCallsign.IndexOf("-") > -1)
                {
                    RemoveRadioAccount(strExistingCallsign);
                }
            }

            AddRadioAccount(Globals.SiteCallsign, Globals.SecureLoginPassword);
            My.MyProject.Forms.Main.Text = "Paclink - " + Globals.SiteCallsign;
            Globals.UpdateAccountDirectories();
            Globals.objINIFile.Flush();
            DialogResult = DialogResult.OK;
            Cursor = Cursors.Default;
            Close();
        } // OK_Button_Click

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            // Closes the profile dialog without updating any profile data...

            DialogResult = DialogResult.Cancel;
            Close();
        } // Cancel_Button_Click

        private void AddRadioAccount(string strCallsign, string strPassword)
        {
            // Create a new callsign account directory...
            Directory.CreateDirectory(Globals.SiteRootDirectory + @"Accounts\" + strCallsign + "_Account");
            string strAccountList = Globals.objINIFile.GetString("Properties", "Account Names", "");
            strAccountList = strAccountList + strCallsign + "|";
            Globals.objINIFile.WriteString("Properties", "Site Callsign", strCallsign);
            Globals.objINIFile.WriteString("Properties", "Site Password", "");
            Globals.objINIFile.WriteString("Properties", "Secure Login Password", Globals.SecureLoginPassword);
            Globals.objINIFile.WriteString("Properties", "Account Names", strAccountList);
            Globals.objINIFile.WriteString(strCallsign, "EMail Password", Globals.POP3Password);
            Accounts.RefreshAccountsList();

            // Add new callsign account to the local Outlook Express...
            if (chkAddToOutlookExpress.Checked)
                OutlookExpress.AddOutlookExpressAccount(strCallsign);
        } // AddRadioAccount

        private bool RemoveRadioAccount(string strCallsign)
        {
            string strAccountList = Globals.objINIFile.GetString("Properties", "Account Names", "");
            strAccountList = Strings.Replace(strAccountList, strCallsign + "|", "");
            Globals.objINIFile.WriteString("Properties", "Site Callsign", "");
            Globals.objINIFile.WriteString("Properties", "Site Password", "");
            Globals.objINIFile.WriteString("Properties", "Secure Login Password", "");
            Globals.objINIFile.WriteString("Properties", "EMail Password", "");
            Globals.objINIFile.WriteString("Properties", "Account Names", strAccountList);
            Globals.objINIFile.DeleteSection(strCallsign);

            // Remove callsign account directory...
            try
            {
                if (Directory.Exists(Globals.SiteRootDirectory + @"Accounts\" + strCallsign + "_Account"))
                {
                    Directory.Delete(Globals.SiteRootDirectory + @"Accounts\" + strCallsign + "_Account", true);
                }
            }
            catch
            {
                Logs.Exception("[Properties.RemoveRadioAccount] " + Information.Err().Description);
            }

            // Remove callsign account from local Outlook Express...
            if (Interaction.MsgBox("Remove account " + strCallsign + " from local Outlook Express?", MsgBoxStyle.YesNo, "Remove OE Account") == MsgBoxResult.Yes)
            {
                OutlookExpress.RemoveOutlookExpressAccount(strCallsign);
            }

            return default;
        } // RemoveRadioAccount

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs40.htm");
        } // btnHelp_Click

        private void rdoUseRMSRelay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUseRMSRelay.Checked)
            {
                txtRMSRelayIPPath.Text = Globals.objINIFile.GetString("Properties", "Local IP Path", "localhost");
                txtRMSRelayPort.Text = Globals.intRMSRelayPort.ToString();
                txtRMSRelayIPPath.Enabled = true;
                txtRMSRelayPort.Enabled = true;
            }
            else
            {
                txtRMSRelayIPPath.Enabled = false;
                txtRMSRelayPort.Enabled = false;
            }
        } // rdoUseRMSRelay_CheckedChanged

        private void rdoUseCMS_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUseCMS.Checked)
            {
                txtRMSRelayIPPath.Enabled = false;
                txtRMSRelayPort.Enabled = false;
            }
            else
            {
                txtRMSRelayIPPath.Text = Globals.objINIFile.GetString("Properties", "Local IP Path", "localhost");
                txtRMSRelayPort.Text = Globals.intRMSRelayPort.ToString();
                txtRMSRelayIPPath.Enabled = true;
                txtRMSRelayPort.Enabled = true;
            }
        } // rdoUseCMS_CheckedChanged
    }
} // SiteDialogProperties