using Paclink.Data;
using System;
using System.IO;
using System.Windows.Forms;

namespace Paclink
{
    public partial class DialogCallsignAccounts
    {
        public DialogCallsignAccounts()
        {
            InitializeComponent();
            _btnClose.Name = "btnClose";
            _txtPassword.Name = "txtPassword";
            _cmbAccount.Name = "cmbAccount";
            _Label2.Name = "Label2";
            _Label1.Name = "Label1";
            _btnRemove.Name = "btnRemove";
            _btnAdd.Name = "btnAdd";
            _btnInstructions.Name = "btnInstructions";
            _btnHelp.Name = "btnHelp";
            _Label9.Name = "Label9";
        }

        private string strOldPassword = "";

        private void CallsignAccounts_Load(object sender, EventArgs e)
        {
            FillAccountSelection();
        } // CallsignAccounts_Load

        private void cmbAccount_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Text = Globals.Settings.Get(cmbAccount.Text, "EMail Password", "");
            strOldPassword = txtPassword.Text;
        } // cmbAccount_TextChanged

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            // Displays instructions for entering a callsign address...

            MessageBox.Show(
                "A callsign account name must consist of a valid ham or MARS radio callsign." + Globals.CRLF + 
                "examples:  W1ABC, WA9DEF-12" + Globals.CRLF + Globals.CRLF +
                "The <account name>@Winlink.org will be the email address of the account user." + Globals.CRLF + 
                "If the account name uses a -ssid extension then you must also enter the " + Globals.CRLF + 
                "password of the base callsign in the field provided." + Globals.CRLF + Globals.CRLF +
                "To enter a tactical account use the Tactical Accounts dialog box.");




        } // btnInstructions_Click

        private void btnAdd_Click(object s, EventArgs e)
        {
            string strPassword = txtPassword.Text.Trim();
            cmbAccount.Text = cmbAccount.Text.Trim().ToUpper();
            txtPassword.Text = strPassword;
            string strAccountName = cmbAccount.Text;
            bool blnIsHamCall = Globals.IsValidRadioCallsign(strAccountName);
            bool blnIsDashSSID = blnIsHamCall & strAccountName.IndexOf("-") != -1;
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
            else if (!Globals.IsValidRadioCallsign(cmbAccount.Text))
            {
                EntryErrorMessage();
                cmbAccount.Focus();
                return;
            }
            else if (IsAccount(cmbAccount.Text))
            {
                MessageBox.Show("This account name is already an account...");
                cmbAccount.Focus();
                return;
            }
            else if (IsChannel(cmbAccount.Text))
            {
                MessageBox.Show("This account name is already in use as a channel name...");
                cmbAccount.Focus();
                return;
            }

            if (txtPassword.Text.Trim().Length < 3)
            {
                MessageBox.Show("Password must be at least three characters...");
                txtPassword.Focus();
                return;
            }
            else if (txtPassword.Text.Trim().Length > 12)
            {
                MessageBox.Show("Password must be twelve characters or less long...");
                txtPassword.Focus();
                return;
            }

            btnAdd.Enabled = false;
            if (MessageBox.Show(
                "Are you sure you want all messages to " + strAccountName + " to be delivered to this site?",
                "New Account",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (AddNewAccount())
                {
                    MessageBox.Show(
                        "The new account '" + strAccountName + "' has been added. ", "Account Added",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(
                    "Callsign entry cancelled", "New Account",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            strOldPassword = strPassword;
            btnAdd.Enabled = true;
            FillAccountSelection();
        } // btnAdd_Click

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // 
            // Remove the selected callsign account from the local site.
            // 
            string strAccount = cmbAccount.Text.ToUpper().Trim();
            if ((Globals.SiteCallsign ?? "") == (strAccount ?? ""))
            {
                MessageBox.Show(
                    "The site callsign account cannot be removed from the Accounts menu!" + Globals.CR +
                    "Use the Properties menu to change the site callsign.", "Remove Account",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                "Confirm removal of '" + strAccount + "' from the this site?",
                "Confirm Remove",
                MessageBoxButtons.YesNoCancel) == DialogResult.Cancel)
                return;
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            while (strAccountList.IndexOf(strAccount + "|") != -1) // This added to delete multiples if they exist
                strAccountList = strAccountList.Replace(strAccount + "|", "");
            Globals.Settings.Save("Properties", "Account Names", strAccountList);

            var messageStore = new MessageStore(DatabaseFactory.Get());
            messageStore.DeleteAccountEmails(strAccount);

            Globals.Settings.DeleteGroup(strAccount);
            Accounts.RefreshAccountsList();
            FillAccountSelection();
        } // btnRemove_Click

        private void btnClose_Click(object sender, EventArgs e)
        {
            // 
            // Closes this dialog box.
            // 
            string strCallsign = cmbAccount.Text;
            if (strCallsign != "<Add new account>" && string.Compare(txtPassword.Text.Trim(), strOldPassword, false) != 0)
            {
                // Update the password for this callsign.
                Globals.Settings.Save(strCallsign, "EMail Password", txtPassword.Text.Trim());
            }

            Globals.UpdateAccountDirectories();
            Close();
        } // btnClose_Click

        private void FillAccountSelection()
        {
            cmbAccount.Items.Clear();
            cmbAccount.Sorted = true;
            foreach (Accounts.AccountRecord UserAccount in Accounts.AccountsList.Values)
            {
                if (Globals.IsValidRadioCallsign(UserAccount.Name))
                {
                    if (!string.IsNullOrEmpty(UserAccount.Name))
                        cmbAccount.Items.Add(UserAccount.Name);
                }
            }

            cmbAccount.Items.Add("<Add new account>");
            cmbAccount.Text = Convert.ToString(cmbAccount.Items[0]);
            cmbAccount.Focus();
        } // FillAccountSelection

        private void EntryErrorMessage()
        {
            // 
            // Display an account entry error message box...
            // 
            MessageBox.Show(
                "Callsign account names must be a valid ham or MARS radio callsign with an optional -SSID between 1 and 15.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        } // EntryErrorMessage

        public bool IsValidAccountName(string strCallsign)
        {
            // 
            // Returns True if strCallsign is a valid callsign with a valid SSID.
            // 
            if (!Globals.IsValidRadioCallsign(strCallsign))
                return false;
            var strTokens = strCallsign.Split('-');
            if (strTokens.Length != 2)
                return false;
            int intSSID = 0;
            if (!int.TryParse(strTokens[1], out intSSID))
                return false;
            if (intSSID >= 1 & intSSID <= 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        } // IsValidAccountName

        private bool IsAccount(string strAccountName)
        {
            // 
            // Returns True if the indicated account name is registered locally.
            // 
            var Account = Accounts.GetUserAccount(strAccountName);
            if ((Account.Name ?? "") == (strAccountName ?? ""))
                return true;
            return false;
        } // IsAccountNameInUseHere

        public static bool IsChannel(string strChannelName)
        {
            string strChannelNames = Globals.Settings.Get("Properties", "Channel Names", "");
            if (strChannelNames.IndexOf(strChannelName + "|") != -1)
                return true;
            else
                return false;
        } // IsChannel

        private bool AddNewAccount()
        {
            // 
            // Adds a new account name to the system.
            // 
            bool blnReturn;
            cmbAccount.Text = cmbAccount.Text.ToUpper().Trim();

            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            // 
            // Add the account name to the registry list.
            // 
            strAccountList = strAccountList + cmbAccount.Text + "|";
            Globals.Settings.Save("Properties", "Account Names", strAccountList);
            Globals.Settings.Save(cmbAccount.Text, "EMail Password", txtPassword.Text.Trim());
            strOldPassword = txtPassword.Text;
            blnReturn = true;
            // 
            // Restore the dialog box window.
            // 
            btnRemove.Enabled = true;
            Accounts.RefreshAccountsList();
            FillAccountSelection();
            return blnReturn;
        } // AddNewAccount

        private void cmbAccount_Leave(object sender, EventArgs e)
        {
            if (cmbAccount.Text == "<Add new account>")
                return;
            if (!Globals.IsValidFileName(cmbAccount.Text))
                cmbAccount.Focus();
        } // cmbAccount_Leave

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Globals.SiteRootDirectory + @"Help\Paclink.chm", HelpNavigator.Topic, @"html\hs90.htm");
        } // btnHelp_Click

        private void cmbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
} // CallsignAccounts