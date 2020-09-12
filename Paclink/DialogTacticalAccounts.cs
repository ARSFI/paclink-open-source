using System;
using System.IO;
using System.Windows.Forms;
using NLog;

namespace Paclink
{
    public partial class DialogTacticalAccounts
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DialogTacticalAccounts()
        {
            InitializeComponent();
            _btnClose.Name = "btnClose";
            _txtPassword.Name = "txtPassword";
            _cmbAccount.Name = "cmbAccount";
            _Label2.Name = "Label2";
            _Label1.Name = "Label1";
            _btnOutlookExpress.Name = "btnOutlookExpress";
            _btnPassword.Name = "btnPassword";
            _btnRemove.Name = "btnRemove";
            _btnAdd.Name = "btnAdd";
            _btnInstructions.Name = "btnInstructions";
            _Label3.Name = "Label3";
            _btnHelp.Name = "btnHelp";
            _Label9.Name = "Label9";
        }

        private bool blnNoCMSConnection = false;

        private void TacticalAccounts_Load(object sender, EventArgs e)
        {
            // 
            // Initializes this form.
            // Verify that we can connect to a CMS.
            // 
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            FillAccountSelection();
        } // TacticalAccounts_Load

        private void cmbAccount_Leave(object sender, EventArgs e)
        {
            if (cmbAccount.Text == "<Add new account>")
                return;
            if (!Globals.IsValidFileName(cmbAccount.Text))
                cmbAccount.Focus();
        } // cmbAccount_Leave

        private void cmbAccount_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Text = Globals.Settings.Get(cmbAccount.Text, "EMail Password", "");
        } // cmbAccount_TextChanged

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            // 
            // Displays instructions for entering a tactical address...
            // 
            MessageBox.Show("A tactical account name may consist of a tactical address of alpha characters" + Globals.CRLF + "ONLY or alpha characters ONLY, followed by a dash, followed by alphanumeric" + Globals.CRLF + "characters . A name may not exceed 12 characters. " + Globals.CRLF + Globals.CRLF + "Valid account name examples:  MLBSHELTER, REDCROSS-12, POLICE-9A, FLDADEEOC-1" + Globals.CRLF + "The <account name>@Winlink.org will be the E-mail address of the account user." + Globals.CRLF + Globals.CRLF + "To enter a ham or MARS radio callsign account use the Callsign Accounts dialog box.");




        } // btnInstructions_Click

        private void btnAdd_Click(object s, EventArgs e)
        {
            // 
            // Adds a new tactical address to this site...
            // 
            string strPassword = txtPassword.Text.Trim();
            cmbAccount.Text = cmbAccount.Text.Trim().ToUpper();
            txtPassword.Text = strPassword;
            string sAccountName = cmbAccount.Text;
            if (cmbAccount.Text.Length < 3)
            {
                EntryErrorMessage();
                cmbAccount.Focus();
                return;
            }
            else if (cmbAccount.Text.Length > 12)
            {
                EntryErrorMessage();
                cmbAccount.Focus();
                return;
            }
            else if (!IsValidAccountName(cmbAccount.Text))
            {
                EntryErrorMessage();
                cmbAccount.Focus();
                return;
            }
            else if (IsAccountNameInUseHere(cmbAccount.Text))
            {
                MessageBox.Show("This account name is already in use...");
                cmbAccount.Focus();
                return;
            }

            if (txtPassword.Text.Length < 3)
            {
                MessageBox.Show("Password must be at least three characters...");
                txtPassword.Focus();
                return;
            }
            else if (txtPassword.Text.Length > 12)
            {
                MessageBox.Show("Password must be twelve characters or less long...");
                txtPassword.Focus();
                return;
            }

            btnAdd.Enabled = false;
            if (AddNewAccount())
            {
                MessageBox.Show("The new account '" + sAccountName + "' has been added. ", "Account Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            btnAdd.Enabled = true;
            FillAccountSelection();
        } // btnAdd_Click

        private void btnRemove_Click(object s, EventArgs e)
        {
            // 
            // Removes a tactical account from the system...
            // 
            string strAccount = cmbAccount.Text.Trim();
            if (MessageBox.Show("Confirm removal of " + strAccount + " from the Winlink database?", "Confirm Remove", MessageBoxButtons.OKCancel) != DialogResult.Cancel)
            {
                Cursor = Cursors.WaitCursor;
                // 
                // Remove the account from the system.
                // 
                string strResponse = Globals.objWL2KInterop.RemoveTacticalAddress(strAccount);
                if (!string.IsNullOrEmpty(strResponse))
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Error removing tactical account " + strAccount + ": " + strResponse, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                // 
                // Remove account from registry list.
                // 
                string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
                strAccountList = strAccountList.Replace(strAccount + "|", "");
                Globals.Settings.Save("Properties", "Account Names", strAccountList);
                // 
                // Remove account directory.
                // 
                if (Directory.Exists(Globals.SiteRootDirectory + @"Accounts\" + strAccount + "_Account"))
                {
                    Directory.Delete(Globals.SiteRootDirectory + @"Accounts\" + strAccount + "_Account", true);
                }

                Globals.Settings.DeleteGroup(strAccount);
                // 
                // Remove account from Outlook Express.
                // 
                OutlookExpress.RemoveOutlookExpressAccount(strAccount);
                // 
                // Restore the dialog box window.
                // 
                Accounts.RefreshAccountsList();
                FillAccountSelection();
                Cursor = Cursors.Default;
            }
        } // btnRemove_Click

        private void btnPassword_Click(object s, EventArgs e)
        {
            // 
            // Changes the password for the tactical address.
            // 
            // Prompt for the old and new passwords.
            // 
            string strOldPassword = txtPassword.Text.Trim();
            var objDialogChangePassword = new DialogChangePassword(strOldPassword);
            var enmResult = objDialogChangePassword.ShowDialog();
            if (enmResult == DialogResult.Cancel)
                return;
            // 
            // They confirmed that they want to make the change.
            // 
            Cursor = Cursors.WaitCursor;
            strOldPassword = objDialogChangePassword.GetOldPassword();
            string strNewPassword = objDialogChangePassword.GetNewPassword();
            string strAccount = cmbAccount.Text.Trim();
            // 
            // Test for existence of the tactical address in the CMS database.
            // 
            if (Globals.objWL2KInterop.AccountRegistered(strAccount))
            {
                // 
                // This account is already registered.  Check the old password.
                // 
                if (Globals.objWL2KInterop.ValidatePassword(strAccount, strOldPassword) == false)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(
                        "The address/account has been previously used and the entered old password" + 
                        " does not match the password for '" + strAccount + "' in the Winlink database" +
                        Globals.CR + Globals.CR + "To use this address/account " +
                        "name you must have the previously assigned password.", "Validating Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
                // 
                // Set the new password in the system table.
                // 
                string strReply = Globals.objWL2KInterop.SetPassword(strAccount, strOldPassword, strNewPassword);
                if (!string.IsNullOrEmpty(strReply))
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show("Error while setting password: " + strReply, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else
            {
                // 
                // This account is not know by the system.
                // 
                Cursor = Cursors.Default;
                MessageBox.Show(
                    "The tactical address " + strAccount + " has not been registered with the Winlink system.", 
                    "Checking address",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            // 
            // Update the password in the .ini file.
            // 
            Globals.Settings.Save(strAccount, "EMail Password", strNewPassword);
            // 
            // Finished
            // 
            Accounts.RefreshAccountsList();
            FillAccountSelection();
            Cursor = Cursors.Default;
            MessageBox.Show(
                "The password for " + strAccount + " has been changed to " + strNewPassword, 
                "Password Changed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        } // btnPassword_Click

        private void btnOutlookExpress_Click(object s, EventArgs e)
        {
            // 
            // Adds this account to the Outlook Express program located on this machine...
            // 
            if (cmbAccount.Text.ToLower() != "<add new account>")
            {
                OutlookExpress.AddOutlookExpressAccount(cmbAccount.Text);
            }
        } // btnOutlookExpress_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 
            // Closes this dialog box.
            // 
            Globals.UpdateAccountDirectories();
            Close();
        } // btnClose_Click

        private void FillAccountSelection()
        {
            // 
            // Fill the cmbAccount combo box with currently used tactical addresses.
            // 
            cmbAccount.Items.Clear();
            cmbAccount.Sorted = true;
            foreach (Accounts.AccountRecord UserAccount in Accounts.AccountsList.Values)
            {
                if (!Globals.IsValidRadioCallsign(UserAccount.Name))
                {
                    if (!string.IsNullOrEmpty(UserAccount.Name))
                        cmbAccount.Items.Add(UserAccount.Name);
                }
            }

            cmbAccount.Items.Add("<Add new account>");
            cmbAccount.Text = Convert.ToString(cmbAccount.Items[0]);
            cmbAccount.Focus();
        } // FillAccountSelection

        private bool IsValidAccountName(string sName)
        {
            // 
            // Checks for valid Tactical Account name:
            // 3-12 characters
            // Characters before an optional "-" must be alpha (A-Z)
            // Characters after an optional "-" may be alpha (A-Z) or numberic (0-9)
            // 
            bool bDigitsOk = false;
            sName = sName.ToUpper().Trim();
            if (sName.Length < 3 | sName.Length > 12)
                return false;
            var ch = sName.ToCharArray();
            foreach (char c in ch)
            {
                if (c.CompareTo('-') == 0)
                    bDigitsOk = true;
                if (!bDigitsOk)
                {
                    if (c.CompareTo('A') < 0 | c.CompareTo('Z') > 0)
                        return false;
                }
                else if (!(c.CompareTo('A') >= 0 & c.CompareTo('Z') <= 0 | c.CompareTo('0') >= 0 & c.CompareTo('9') <= 0 | c.CompareTo('-') == 0))

                    return false;
            }

            return true;
        } // IsValidAccountName

        private void EntryErrorMessage()
        {
            // 
            // Display an account entry error message box...
            // 
            MessageBox.Show(
                "Account names must be tactical addresses at least 3 characters long ," +
                "not longer than 12 characters, and be in a valid format. See instructions for more information...",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        } // EntryErrorMessage

        private bool IsAccountNameInUseHere(string strAccountName)
        {
            // 
            // Returns True if the indicated account name is registered locally...
            // 
            var Account = Accounts.GetUserAccount(strAccountName);
            if ((Account.Name ?? "") == (strAccountName ?? ""))
                return true;
            return false;
        } // IsAccountNameInUseHere

        private bool AddNewAccount()
        {
            // 
            // Adds a new account name to the system.
            // 
            bool blnReturn;
            if (AddTacticalAccountToWinlink(cmbAccount.Text, txtPassword.Text))
            {
                // Make a new sub-directory for this account...
                Directory.CreateDirectory(Globals.SiteRootDirectory + @"Accounts\" + cmbAccount.Text + "_Account");

                // Add the account name to the properties file...
                string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
                strAccountList = strAccountList + cmbAccount.Text + "|";
                Globals.Settings.Save("Properties", "Account Names", strAccountList);
                Globals.Settings.Save(cmbAccount.Text, "EMail Password", txtPassword.Text);
                blnReturn = true;
            }
            else
            {
                blnReturn = false;
            }

            // Restore the dialog box window...
            btnOutlookExpress.Enabled = true;
            btnRemove.Enabled = true;
            btnPassword.Enabled = true;
            btnAdd.Enabled = false;
            Accounts.RefreshAccountsList();
            FillAccountSelection();
            return blnReturn;
        } // AddNewAccount

        public bool AddTacticalAccountToWinlink(string strTacticalAddress, string strPassword)
        {
            // 
            // Registers a tactical address and password if the address is not
            // already in use. Returns True if successful, false if the address is
            // already in use.
            // 
            // Test for existence of the tactical address in the CMS database.
            // 
            if (Globals.objWL2KInterop.AccountRegistered(strTacticalAddress))
            {
                // 
                // This account is already registered.  Check the password.
                // 
                if (Globals.objWL2KInterop.ValidatePassword(strTacticalAddress, strPassword) == false)
                {
                    MessageBox.Show(
                        "The address/account has been previously used and the entered password " +
                        "does not match the password for '" + strTacticalAddress + "' in the Winlink database" +
                        Globals.CR + Globals.CR + "To use this address/account " + 
                        "name you must have the previously assigned password.", 
                        "Validating Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return false;
                }

                if (MessageBox.Show(
                    "The account name '" + strTacticalAddress + "' is already in use or otherwise " +
                    "not available." + Globals.CRLF + "Do you want to add this account to this Paclink site anyway?",
                    "Existing Tactical Address Found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)

                {
                    MessageBox.Show(
                        "Using the same Tactical address from different Paclink sites must be done carefully." + 
                        Globals.CR + "Once mail is retrieved by a Paclink site it is considered delivered by the WL2K system " +
                        Globals.CR + "and will not longer be accessable to other sites.",
                        "Caution!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return true;
                }
            }
            // 
            // Add the new tactical address to the CMS database.
            // 
            string strResult = Globals.objWL2KInterop.AddTacticalAddress(strTacticalAddress, strPassword);
            if (!string.IsNullOrEmpty(strResult))
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    "Error adding tactical account " + strTacticalAddress + ": " + strResult, 
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }
            // 
            // Finished
            // 
            Cursor = Cursors.Default;
            return true;
        } // AddTacticalAccountToWinlink

        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs100.htm");
            }
            catch (Exception ex)
            {
                Log.Error("[TacticalAccounts.btnHelp_Click] " + ex.Message);
            }
        } // btnHelp_Click
    }
} // TacticalAccounts