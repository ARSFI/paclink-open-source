using NLog;
using Paclink.UI.Common;
using System;
using System.Windows.Forms;
using R = Paclink.Resources.Properties.Resources;


namespace Paclink.UI.Windows
{
    public partial class DialogTacticalAccounts : ITacticalAccountsWindow
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();
        private ITacticalAccountsBacking _backingObject = null;
        public ITacticalAccountsBacking BackingObject => _backingObject;

        public DialogTacticalAccounts(ITacticalAccountsBacking backingObject)
        {
            _backingObject = backingObject;

            InitializeComponent();
            _btnClose.Name = "btnClose";
            _txtPassword.Name = "txtPassword";
            _cmbAccount.Name = "cmbAccount";
            _Label2.Name = "Label2";
            _Label1.Name = "Label1";
            _btnPassword.Name = "btnPassword";
            _btnAdd.Name = "btnAdd";
            _btnInstructions.Name = "btnInstructions";
            _btnHelp.Name = "btnHelp";
            _Label9.Name = "Label9";
        }

        private void TacticalAccounts_Load(object sender, EventArgs e)
        {
            // 
            // Initializes this form.
            // Verify that we can connect to a CMS.
            // 
            /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            FillAccountSelection();
        }

        private void cmbAccount_Leave(object sender, EventArgs e)
        {
            if (cmbAccount.Text == "<Add new account>")
                return;
            if (!BackingObject.IsValidFileName(cmbAccount.Text))
                cmbAccount.Focus();
        }

        private void cmbAccount_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Text = BackingObject.GetEmailPassword(cmbAccount.Text);
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            // 
            // Displays instructions for entering a tactical address...
            // 
            MessageBox.Show("A tactical account name may consist of a tactical address of alpha characters\r\n" +
                "ONLY or alpha characters ONLY, followed by a dash, followed by alphanumeric\r\n" +
                "characters . A name may not exceed 12 characters. \r\n\r\n" +
                "Valid account name examples:  MLBSHELTER, REDCROSS-12, POLICE-9A, FLDADEEOC-1\r\n" +
                "The <account name>@Winlink.org will be the E-mail address of the account user.\r\n\r\n" +
                "To enter a ham or MARS radio callsign account use the Callsign Accounts dialog box.");
        }

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

            if (txtPassword.Text.Length < 6 || txtPassword.Text.Length > 12)
            {
                UserInterfaceFactory.GetUiSystem().DisplayModalError(R.Entry_Error, R.Password_Requirements);
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
        }

        private void btnPassword_Click(object s, EventArgs e)
        {
            // 
            // Changes the password for the tactical address.
            // 

            // 
            // Test for existence of the tactical address
            // 
            string account = cmbAccount.Text.Trim();
            if (!BackingObject.AccountExists(account))
            {
                MessageBox.Show(
                    "The tactical address is not registered with the Winlink system.",
                    "Checking address", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //remove from local database - cms overrides local
                BackingObject.RemoveAccount(account);
                BackingObject.RefreshAccountsList();
                return;
            }

            string oldPassword = txtPassword.Text.Trim();
            if (BackingObject.ChangeTacticalPassword(account, oldPassword))
            {
                var newPassword = BackingObject.GetEmailPassword(account);
                MessageBox.Show(
                    "The password for " + account + " has been changed to '" + newPassword + "'",
                    "Password Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The password was not changed.", "Password Change", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BackingObject.RefreshAccountsList();
            FillAccountSelection();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 
            // Closes this dialog box.
            // 
            BackingObject.UpdateAccountDirectories();
            Close();
        }

        private void FillAccountSelection()
        {
            // 
            // Fill the cmbAccount combo box with currently used tactical addresses.
            // 
            cmbAccount.Items.Clear();
            cmbAccount.Sorted = true;
            var tacticalAccountNames = BackingObject.GetTacticalAccountNames();
            foreach (var tacticalAccountName in tacticalAccountNames)
            {
                cmbAccount.Items.Add(tacticalAccountName);
            }

            cmbAccount.Items.Add("<Add new account>");
            cmbAccount.Text = Convert.ToString(cmbAccount.Items[0]);
            cmbAccount.Focus();
        }

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
        }

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
        }

        private bool IsAccountNameInUseHere(string strAccountName)
        {
            // 
            // Returns True if the indicated account name is registered locally...
            // 
            var Account = BackingObject.GetUserAccount(strAccountName);
            if ((Account.Name ?? "") == (strAccountName ?? ""))
                return true;
            return false;
        }

        private bool AddNewAccount()
        {
            // 
            // Adds a new account name to the system.
            // 
            bool blnReturn;
            if (AddTacticalAccountToWinlink(cmbAccount.Text, txtPassword.Text))
            {
                // Add the account name to the properties file...
                string strAccountList = BackingObject.GetAccountNames();
                strAccountList = strAccountList + cmbAccount.Text + "|";
                BackingObject.SaveAccountNames(strAccountList);
                BackingObject.SaveEmailPassword(cmbAccount.Text, txtPassword.Text);
                blnReturn = true;
            }
            else
            {
                blnReturn = false;
            }

            // Restore the dialog box window...
            btnPassword.Enabled = true;
            btnAdd.Enabled = false;
            BackingObject.RefreshAccountsList();
            FillAccountSelection();
            return blnReturn;
        }

        public bool AddTacticalAccountToWinlink(string strTacticalAddress, string strPassword)
        {
            // 
            // Registers a tactical address and password if the address is not
            // already in use. Returns True if successful, false if the address is
            // already in use.
            // 
            // Test for existence of the tactical address in the CMS database.
            // 
            if (BackingObject.AccountExists(strTacticalAddress))
            {
                // 
                // This account is already registered.  Check the password.
                // 
                if (!BackingObject.ValidatePassword(strTacticalAddress, strPassword))
                {
                    MessageBox.Show(
                        "The address/account has been previously used and the entered password " +
                        "does not match the password for '" + strTacticalAddress + "' in the Winlink database\r\n\r\n" +
                        "To use this address/account name you must have the previously assigned password.",
                        "Validating Password",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return false;
                }

                if (MessageBox.Show(
                    "The account name '" + strTacticalAddress + "' is already in use or otherwise " +
                    "not available.\r\nDo you want to add this account to this Paclink site anyway?",
                    "Existing Tactical Address Found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.OK)

                {
                    MessageBox.Show(
                        "Using the same Tactical address from different Paclink sites must be done carefully.\r\n" +
                        "Once mail is retrieved by a Paclink site it is considered delivered by the WL2K system \r\n" +
                         "and will not longer be accessable to other sites.",
                        "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
            }

            // 
            // Add the new tactical address to the CMS database.
            // 
            try
            {
                BackingObject.AddTacticalAddress(strTacticalAddress, strPassword);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error adding tactical account " + strTacticalAddress + ": " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            try
            {
                Help.ShowHelp(this, BackingObject.SiteRootDirectory + @"\Paclink.chm", HelpNavigator.Topic, @"html\hs100.htm");
            }
            catch (Exception ex)
            {
                Log.Error("[TacticalAccounts.btnHelp_Click] " + ex.Message);
            }
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void CloseWindow()
        {
            // empty
        }

        public UiDialogResult ShowModal()
        {
            throw new NotImplementedException();
        }
    }
}