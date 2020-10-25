using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public partial class DialogSiteProperties
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DialogSiteProperties()
        {
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
            _Label14.Name = "Label14";
        }

        private Socket _objTestPort;

        private Socket objTestPort
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
            txtPrefix.Text = Globals.Settings.Get("Properties", "Prefix", "");
            txtSuffix.Text = Globals.Settings.Get("Properties", "Suffix", "");
            txtSizeLimit.Text = Globals.Settings.Get("Properties", "Size Limit", "120000");
            txtSecureLoginPassword.Enabled = true;
            if (txtSecureLoginPassword.Enabled == false)
                txtSecureLoginPassword.Text = "";

            // Initialize the local IP addresses 
            cmbLocalIPAddress.Items.Clear();
            foreach (string strAddress in Globals.strLocalIPAddresses)
            {
                if (strAddress != null)
                    cmbLocalIPAddress.Items.Add(strAddress);
            }

            try
            {
                cmbLocalIPAddress.SelectedIndex = Globals.Settings.Get("Properties", "Default Local IP Address Index", 0);
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
            int sizeLimit = 0;
            if (!int.TryParse(txtSizeLimit.Text, out sizeLimit))
            {
                MessageBox.Show("Attachment limit must be between 0 and 120000. 0 blocks all attachments.", "Attachment Limit Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (sizeLimit < 0 || sizeLimit > 120000)
            {
                MessageBox.Show("Attachment limit must be between 0 and 120000. 0 blocks all attachments.", "Attachment Limit Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

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

            if (!Globals.IsValidRadioCallsign(txtSiteCallsign.Text))
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

            if (txtPOP3Password.Text.Length < 4)
            {
                MessageBox.Show("A valid POP3/SMTP password is required...");
                txtPOP3Password.Focus();
                return;
            }

            if (txtGridSquare.Text.Trim().Length < 6)
            {
                MessageBox.Show("A 6 character grid square entry is required...");
                txtGridSquare.Focus();
                return;
            }

            Globals.SiteCallsign = txtSiteCallsign.Text;
            Globals.SecureLoginPassword = txtSecureLoginPassword.Text.Trim();
            Globals.POP3Password = txtPOP3Password.Text.ToUpper().Trim();
            Globals.SiteGridSquare = txtGridSquare.Text.Trim();
            Globals.strServiceCodes = txtServiceCodes.Text.ToUpper().Trim();
            Globals.intSMTPPortNumber = Convert.ToInt32(txtSMTPPortNumber.Text);
            Globals.intPOP3PortNumber = Convert.ToInt32(txtPOP3PortNumber.Text);
            Globals.blnLAN = chkLANAccessable.Checked;
            Globals.blnEnableRadar = chkEnableRadar.Checked;
            Globals.blnUseRMSRelay = rdoUseRMSRelay.Checked;
            Globals.strRMSRelayIPPath = txtRMSRelayIPPath.Text;
            Globals.intRMSRelayPort = Convert.ToInt32(txtRMSRelayPort.Text);
            Globals.blnForceHFRouting = chkForceHFRouting.Checked;

            // Test the POP3 and SMTP Port numbers
            objTestPort = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                objTestPort.Bind(new IPEndPoint(IPAddress.Loopback, Globals.intPOP3PortNumber));
                objTestPort.Listen(10);
                objTestPort.Close();
            }
            catch
            {
                MessageBox.Show(
                    "Conflict on POP3 port number " + Globals.intPOP3PortNumber.ToString() + 
                    ".  Port is probably in use by another service or program. Try port number " + 
                    (Globals.intPOP3PortNumber + 1).ToString() + ".", "POP3 Port Conflict", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtPOP3PortNumber.Focus();
                return;
            }
            finally 
            {
                objTestPort = null;
            }

            objTestPort = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                objTestPort.Bind(new IPEndPoint(IPAddress.Loopback, Globals.intSMTPPortNumber));
                objTestPort.Listen(10);
                objTestPort.Close();
            }
            catch
            {
                MessageBox.Show(
                    "Conflict on SMTP port number " + Globals.intSMTPPortNumber.ToString() +
                    ".  Port is probably in use by another service or program. Try port number " +
                    (Globals.intSMTPPortNumber + 1).ToString() + ".",
                    "SMTP Port Conflict",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                txtSMTPPortNumber.Focus();
                return;
            }
            finally 
            {
                objTestPort = null;
            }

            Globals.Settings.Save("Properties", "Program Directory", Globals.SiteBinDirectory);
            Globals.Settings.Save("Properties", "Site Callsign", Globals.SiteCallsign);
            Globals.Settings.Save("Properties", "Site Password", "");
            Globals.Settings.Save("Properties", "Secure Login Password", Globals.SecureLoginPassword);
            Globals.Settings.Save("Properties", "EMail Password", Globals.POP3Password);
            Globals.Settings.Save("Properties", "ServiceCodes", Globals.strServiceCodes);
            Globals.Settings.Save("Properties", "Grid Square", Globals.SiteGridSquare);
            Globals.Settings.Save("Properties", "SMTP Port Number", Globals.intSMTPPortNumber);
            Globals.Settings.Save("Properties", "POP3 Port Number", Globals.intPOP3PortNumber);
            Globals.Settings.Save("Properties", "LAN Connection", Globals.blnLAN);
            Globals.Settings.Save("Properties", "Enable Radar", Globals.blnEnableRadar);
            Globals.Settings.Save("Properties", "Prefix", txtPrefix.Text.Trim());
            Globals.Settings.Save("Properties", "Suffix", txtSuffix.Text.Trim());
            Globals.Settings.Save("Properties", "Size Limit", txtSizeLimit.Text.Trim());
            Globals.Settings.Save("Properties", "Default Local IP Address Index", cmbLocalIPAddress.SelectedIndex);
            Globals.Settings.Save("Properties", "Use RMS Relay", Globals.blnUseRMSRelay);
            Globals.Settings.Save("Properties", "Local IP Path", Globals.strRMSRelayIPPath);
            Globals.Settings.Save("Properties", "RMS Relay Port", Globals.intRMSRelayPort);
            Globals.Settings.Save("Properties", "Force radio-only", Globals.blnForceHFRouting);
            Globals.Settings.Save(Application.ProductName, "Start Minimized", Globals.blnStartMinimized);
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
            MyApplication.Forms.Main.Text = "Paclink - " + Globals.SiteCallsign;
            Globals.UpdateAccountDirectories();
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
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            strAccountList = strAccountList + strCallsign + "|";
            Globals.Settings.Save("Properties", "Site Callsign", strCallsign);
            Globals.Settings.Save("Properties", "Site Password", "");
            Globals.Settings.Save("Properties", "Secure Login Password", Globals.SecureLoginPassword);
            Globals.Settings.Save("Properties", "Account Names", strAccountList);
            Globals.Settings.Save(strCallsign, "EMail Password", Globals.POP3Password);
            Accounts.RefreshAccountsList();
        } // AddRadioAccount

        private bool RemoveRadioAccount(string strCallsign)
        {
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            strAccountList = strAccountList.Replace(strCallsign + "|", "");
            Globals.Settings.Save("Properties", "Site Callsign", "");
            Globals.Settings.Save("Properties", "Site Password", "");
            Globals.Settings.Save("Properties", "Secure Login Password", "");
            Globals.Settings.Save("Properties", "EMail Password", "");
            Globals.Settings.Save("Properties", "Account Names", strAccountList);
            Globals.Settings.DeleteGroup(strCallsign);

            // Remove callsign account directory...
            try
            {
                if (Directory.Exists(Globals.SiteRootDirectory + @"Accounts\" + strCallsign + "_Account"))
                {
                    Directory.Delete(Globals.SiteRootDirectory + @"Accounts\" + strCallsign + "_Account", true);
                }
            }
            catch (Exception ex)
            {
                Log.Error("[Properties.RemoveRadioAccount] " + ex.Message);
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
                txtRMSRelayIPPath.Text = Globals.Settings.Get("Properties", "Local IP Path", "localhost");
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
                txtRMSRelayIPPath.Text = Globals.Settings.Get("Properties", "Local IP Path", "localhost");
                txtRMSRelayPort.Text = Globals.intRMSRelayPort.ToString();
                txtRMSRelayIPPath.Enabled = true;
                txtRMSRelayPort.Enabled = true;
            }
        } // rdoUseCMS_CheckedChanged
    }
} // SiteDialogProperties