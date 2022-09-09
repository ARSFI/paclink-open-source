using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NLog;
using Paclink.UI.Common;

namespace Paclink.UI.Windows
{
    public partial class DialogSiteProperties : ISitePropertiesWindow
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();
        private ISitePropertiesBacking _backingObject = null;

        public DialogSiteProperties(ISitePropertiesBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _Label26.Name = "Label26";
            _txtPOP3PortNumber.Name = "txtPOP3PortNumber";
            _Label23.Name = "Label23";
            _txtSMTPPortNumber.Name = "txtSMTPPortNumber";
            _Label21.Name = "Label21";
            txtSecureLoginPassword.Name = "txtSecureLoginPassword";
            _Label2.Name = "Label2";
            _Label3.Name = "Label3";
            _txtGridSquare.Name = "txtGridSquare";
            txtSiteCallsign.Name = "txtSiteCallsign";
            _chkLANAccessable.Name = "chkLANAccessable";
            _Label1.Name = "Label1";
            _Label11.Name = "Label11";
            _txtPrefix.Name = "txtPrefix";
            _Label12.Name = "Label12";
            _txtSuffix.Name = "txtSuffix";
            _OK_Button.Name = "OK_Button";
            _Cancel_Button.Name = "Cancel_Button";
            _Label8.Name = "Label8";
            _cmbLocalIPAddress.Name = "cmbLocalIPAddress";
            _chkEnableRadar.Name = "chkEnableRadar";
            _txtMailSystemPassword.Name = "txtMailSystemPassword";
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
            _Label14.Name = "Label14";
        }

        public ISitePropertiesBacking BackingObject => _backingObject;

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
            txtRMSRelayIPPath.Text = BackingObject.RMSRelayIPPath;
            txtRMSRelayPort.Text = BackingObject.RMSRelayPort.ToString();
            txtRMSRelayIPPath.Enabled = false;
            txtRMSRelayPort.Enabled = false;
            rdoUseRMSRelay.Checked = BackingObject.UseRMSRelay;
            txtSiteCallsign.Text = BackingObject.SiteCallsign;
            txtServiceCodes.Text = BackingObject.ServiceCodes;
            txtSecureLoginPassword.Text = BackingObject.SecureLoginPassword;
            txtMailSystemPassword.Text = BackingObject.MailSystemPassword;
            txtGridSquare.Text = BackingObject.GridSquare;
            txtSMTPPortNumber.Text = BackingObject.SMTPPortNumber.ToString();
            txtPOP3PortNumber.Text = BackingObject.POP3PortNumber.ToString();
            chkLANAccessable.Checked = BackingObject.LanAccessible;
            chkEnableRadar.Checked = BackingObject.RadarEnabled;
            chkForceHFRouting.Checked = BackingObject.ForceHFRouting;
            txtPrefix.Text = BackingObject.Prefix;
            txtSuffix.Text = BackingObject.Suffix;
            txtSecureLoginPassword.Enabled = true;

            // Initialize the local IP addresses 
            cmbLocalIPAddress.Items.Clear();
            foreach (string strAddress in BackingObject.LocalIPAddresses)
            {
                if (strAddress != null)
                    cmbLocalIPAddress.Items.Add(strAddress);
            }

            try
            {
                cmbLocalIPAddress.SelectedIndex = BackingObject.SelectedLocalIPAddress;
            }
            catch
            {
                if (cmbLocalIPAddress.Items.Count > 0)
                {
                    cmbLocalIPAddress.Text = Convert.ToString(cmbLocalIPAddress.Items[0]);
                    cmbLocalIPAddress.SelectedIndex = 0;
                }
            }
        } // Properties_Load

        private void OK_Button_Click(object sender, EventArgs e)
        {
            // Save the profile data...
            ushort pop3Port = 0;
            if (!ushort.TryParse(txtPOP3PortNumber.Text, out pop3Port))
            {
                MessageBox.Show("POP3 port number must be between 0 and 65535.", "Port Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ushort smtpPort = 0;
            if (!ushort.TryParse(txtSMTPPortNumber.Text, out smtpPort))
            {
                MessageBox.Show("SMTP port number must be between 0 and 65535.", "Port Number Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            txtSiteCallsign.Text = txtSiteCallsign.Text.ToUpper().Trim();
            txtSecureLoginPassword.Text = txtSecureLoginPassword.Text.Trim();
            txtMailSystemPassword.Text = txtMailSystemPassword.Text.Trim();
            txtGridSquare.Text = txtGridSquare.Text.ToUpper().Trim();
            txtServiceCodes.Text = txtServiceCodes.Text.ToUpper().Trim();
            bool blnValidSSID = true;
            var strTokens = txtSiteCallsign.Text.Split('-');
            if (strTokens.Length == 2)
            {
                int intSSID = 0;
                if (int.TryParse(strTokens[1], out intSSID))
                {
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
                MessageBox.Show("A callsign without SSID or any SSID value of 1 through 15 is acceptable.", "Incorrect SSID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSiteCallsign.Focus();
                return;
            }

            if (!BackingObject.IsValidRadioCallsign(txtSiteCallsign.Text))
            {
                MessageBox.Show("A valid radio callsign is required...");
                txtSiteCallsign.Focus();
                return;
            }

            if (txtSecureLoginPassword.Text.Trim().Length < 6)
            {
                MessageBox.Show("You must enter a password that is at least 6 characters long.");
                txtSecureLoginPassword.Focus();
                return;
            }

            if (txtMailSystemPassword.Text.Length < 4)
            {
                MessageBox.Show("A valid POP3/SMTP password is required...");
                txtMailSystemPassword.Focus();
                return;
            }

            if (txtGridSquare.Text.Trim().Length < 6)
            {
                MessageBox.Show("A 6 character grid square entry is required...");
                txtGridSquare.Focus();
                return;
            }

            // Test the POP3 and SMTP Port numbers
            if (!BackingObject.IsValidPort(cmbLocalIPAddress.SelectedIndex, Convert.ToInt32(txtPOP3PortNumber.Text)))
            {
                MessageBox.Show(
                    "Conflict on POP3 port number " + txtPOP3PortNumber.Text + 
                    ".  Port is probably in use by another service or program. Try port number " + 
                    (Convert.ToInt32(txtPOP3PortNumber.Text) + 1).ToString() + ".", "POP3 Port Conflict", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPOP3PortNumber.Focus();
                return;
            }

            if (!BackingObject.IsValidPort(cmbLocalIPAddress.SelectedIndex, Convert.ToInt32(txtSMTPPortNumber.Text)))
            {
                MessageBox.Show(
                    "Conflict on SMTP port number " + txtSMTPPortNumber.Text +
                    ".  Port is probably in use by another service or program. Try port number " +
                    (Convert.ToInt32(txtSMTPPortNumber.Text) + 1).ToString() + ".", "SMTP Port Conflict",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPOP3PortNumber.Focus();
                return;
            }

            BackingObject.UpdateSiteCallsign(txtSiteCallsign.Text, txtSecureLoginPassword.Text.Trim());
            BackingObject.UpdateMailPassword(txtMailSystemPassword.Text.Trim());
            BackingObject.UpdateGridSquare(txtGridSquare.Text.Trim());
            BackingObject.UpdateServiceCode(txtServiceCodes.Text.ToUpper().Trim());
            BackingObject.UpdatePop3PortNumber(Convert.ToInt32(txtPOP3PortNumber.Text));
            BackingObject.UpdateSmtpPortNumber(Convert.ToInt32(txtSMTPPortNumber.Text));
            BackingObject.UpdateLanAccessible(chkLANAccessable.Checked);
            BackingObject.UpdateRadarEnabled(chkEnableRadar.Checked);
            BackingObject.UpdateUseRMSRelay(rdoUseRMSRelay.Checked);
            BackingObject.UpdateRMSRelayHost(txtRMSRelayIPPath.Text, Convert.ToInt32(txtRMSRelayPort.Text));
            BackingObject.UpdateForceHFRouting(chkForceHFRouting.Checked);
            BackingObject.UpdateLocalIPAddress(cmbLocalIPAddress.SelectedIndex);
            BackingObject.UpdatePrefixAndSuffix(txtPrefix.Text.Trim(), txtSuffix.Text.Trim());          

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

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs40.htm");
        } // btnHelp_Click

        private void rdoUseRMSRelay_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUseRMSRelay.Checked)
            {
                BackingObject.RefreshRMSRelayIPAddress();
                txtRMSRelayIPPath.Text = BackingObject.RMSRelayIPPath;
                txtRMSRelayPort.Text = BackingObject.RMSRelayPort.ToString();
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
                BackingObject.RefreshRMSRelayIPAddress();
                txtRMSRelayIPPath.Text = BackingObject.RMSRelayIPPath;
                txtRMSRelayPort.Text = BackingObject.RMSRelayPort.ToString();
                txtRMSRelayIPPath.Enabled = true;
                txtRMSRelayPort.Enabled = true;
            }
        } // rdoUseCMS_CheckedChanged

        public void RefreshWindow()
        {
            Refresh();
        }

        public void CloseWindow()
        {
            Close();
        }
    }
} // SiteDialogProperties