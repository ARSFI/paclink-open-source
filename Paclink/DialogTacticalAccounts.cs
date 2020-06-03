using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Paclink
{
    public partial class DialogTacticalAccounts
    {
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
            txtPassword.Text = Globals.objINIFile.GetString(cmbAccount.Text, "EMail Password", "");
        } // cmbAccount_TextChanged

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            // 
            // Displays instructions for entering a tactical address...
            // 
            Interaction.MsgBox("A tactical account name may consist of a tactical address of alpha characters" + Constants.vbCrLf + "ONLY or alpha characters ONLY, followed by a dash, followed by alphanumeric" + Constants.vbCrLf + "characters . A name may not exceed 12 characters. " + Constants.vbCrLf + Constants.vbCrLf + "Valid account name examples:  MLBSHELTER, REDCROSS-12, POLICE-9A, FLDADEEOC-1" + Constants.vbCrLf + "The <account name>@Winlink.org will be the E-mail address of the account user." + Constants.vbCrLf + Constants.vbCrLf + "To enter a ham or MARS radio callsign account use the Callsign Accounts dialog box.");




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
                Interaction.MsgBox("This account name is already in use...");
                cmbAccount.Focus();
                return;
            }

            if (txtPassword.Text.Length < 3)
            {
                Interaction.MsgBox("Password must be at least three characters...");
                txtPassword.Focus();
                return;
            }
            else if (txtPassword.Text.Length > 12)
            {
                Interaction.MsgBox("Password must be twelve characters or less long...");
                txtPassword.Focus();
                return;
            }

            btnAdd.Enabled = false;
            if (AddNewAccount())
            {
                Interaction.MsgBox("The new account '" + sAccountName + "' has been added. ", MsgBoxStyle.Information, "Account Added");
            }

            btnAdd.Enabled = true;
            FillAccountSelection();
        } // btnAdd_Click

        private void btnRemove_Click(object s, EventArgs e)
        {
            // 
            // Removes a tactical account from the system...
            // 
            string strAccount = Strings.Trim(cmbAccount.Text);
            if (Interaction.MsgBox("Confirm removal of " + strAccount + " from the Winlink database?", MsgBoxStyle.OkCancel, "Confirm Remove") != MsgBoxResult.Cancel)
            {
                Cursor = Cursors.WaitCursor;
                // 
                // Remove the account from the system.
                // 
                string strResponse = Globals.objWL2KInterop.RemoveTacticalAddress(strAccount);
                if (!string.IsNullOrEmpty(strResponse))
                {
                    Cursor = Cursors.Default;
                    Interaction.MsgBox("Error removing tactical account " + strAccount + ": " + strResponse, MsgBoxStyle.Exclamation);
                    return;
                }
                // 
                // Remove account from registry list.
                // 
                string strAccountList = Globals.objINIFile.GetString("Properties", "Account Names", "");
                strAccountList = Strings.Replace(strAccountList, strAccount + "|", "");
                Globals.objINIFile.WriteString("Properties", "Account Names", strAccountList);
                // 
                // Remove account directory.
                // 
                if (Directory.Exists(Globals.SiteRootDirectory + @"Accounts\" + strAccount + "_Account"))
                {
                    Directory.Delete(Globals.SiteRootDirectory + @"Accounts\" + strAccount + "_Account", true);
                }

                Globals.objINIFile.DeleteSection(strAccount);
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
                    Interaction.MsgBox("The address/account has been previously used and the entered old password" + "does not match the password for '" + strAccount + "' in the Winlink database" + Constants.vbCr + Constants.vbCr + "To use this address/account " + "name you must have the previously assigned password.", MsgBoxStyle.Critical, "Validating Password");



                    return;
                }
                // 
                // Set the new password in the system table.
                // 
                string strReply = Globals.objWL2KInterop.SetPassword(strAccount, strOldPassword, strNewPassword);
                if (!string.IsNullOrEmpty(strReply))
                {
                    Cursor = Cursors.Default;
                    Interaction.MsgBox("Error while setting password: " + strReply, MsgBoxStyle.Exclamation);
                    return;
                }
            }
            else
            {
                // 
                // This account is not know by the system.
                // 
                Cursor = Cursors.Default;
                Interaction.MsgBox("The tactical address " + strAccount + " has not been registered with the Winlink system.", MsgBoxStyle.Critical, "Checking address");
            }
            // 
            // Update the password in the .ini file.
            // 
            Globals.objINIFile.WriteString(strAccount, "EMail Password", strNewPassword);
            // 
            // Finished
            // 
            Accounts.RefreshAccountsList();
            FillAccountSelection();
            Cursor = Cursors.Default;
            Interaction.MsgBox("The password for " + strAccount + " has been changed to " + strNewPassword, MsgBoxStyle.Information);
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
            foreach (Accounts.TAccount UserAccount in Accounts.AccountsList)
            {
                if (!Globals.IsValidRadioCallsign(UserAccount.Name))
                {
                    if (!string.IsNullOrEmpty(UserAccount.Name))
                        cmbAccount.Items.Add(UserAccount.Name);
                }
            }

            cmbAccount.Items.Add("<Add new account>");
            cmbAccount.Text = Conversions.ToString(cmbAccount.Items[0]);
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
            Interaction.MsgBox("Account names must be tactical addresses at least 3 characters long," + "not longer than 12 characters, and be in a valid format. See instructions for more information...", MsgBoxStyle.Information);
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
                string strAccountList = Globals.objINIFile.GetString("Properties", "Account Names", "");
                strAccountList = strAccountList + cmbAccount.Text + "|";
                Globals.objINIFile.WriteString("Properties", "Account Names", strAccountList);
                Globals.objINIFile.WriteString(cmbAccount.Text, "EMail Password", txtPassword.Text);
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
                    Interaction.MsgBox("The address/account has been previously used and the entered password" + "does not match the password for '" + strTacticalAddress + "' in the Winlink database" + Constants.vbCr + Constants.vbCr + "To use this address/account " + "name you must have the previously assigned password.", MsgBoxStyle.Critical, "Validating Password");



                    return false;
                }

                if (Interaction.MsgBox("The account name '" + strTacticalAddress + "' is already in use or otherwise " + "not available." + Constants.vbCrLf + "Do you want to add this account to this Paclink site anyway?", MsgBoxStyle.Question | MsgBoxStyle.YesNo, "Existing Tactical Address Found") == MsgBoxResult.Yes)

                {
                    Interaction.MsgBox("Using the same Tactical address from different Paclink sites must be done carefully." + Constants.vbCr + "Once mail is retrieved by a Paclink site it is considered delivered by the WL2K system " + Constants.vbCr + "and will not longer be accessable to other sites.", MsgBoxStyle.Information, "Caution!");

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
                Interaction.MsgBox("Error adding tactical account " + strTacticalAddress + ": " + strResult, MsgBoxStyle.Exclamation);
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
            catch
            {
                Logs.Exception("[TacticalAccounts.btnHelp_Click] " + Information.Err().Description);
            }
        } // btnHelp_Click
    }
} // TacticalAccounts