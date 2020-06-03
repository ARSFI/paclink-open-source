using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

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
            _btnOutlookExpress.Name = "btnOutlookExpress";
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
            txtPassword.Text = Globals.objINIFile.GetString(cmbAccount.Text, "EMail Password", "");
            strOldPassword = txtPassword.Text;
        } // cmbAccount_TextChanged

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            // Displays instructions for entering a callsign address...

            Interaction.MsgBox("A callsign account name must consist of a valid ham or MARS radio callsign." + Constants.vbCrLf + "examples:  W1ABC, WA9DEF-12" + Constants.vbCrLf + Constants.vbCrLf + "The <account name>@Winlink.org will be the email address of the account user." + Constants.vbCrLf + "If the account name uses a -ssid extension then you must also enter the " + Constants.vbCrLf + "password of the base callsign in the field provided." + Constants.vbCrLf + Constants.vbCrLf + "To enter a tactical account use the Tactical Accounts dialog box.");




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
                Interaction.MsgBox("This account name is already an account...");
                cmbAccount.Focus();
                return;
            }
            else if (IsChannel(cmbAccount.Text))
            {
                Interaction.MsgBox("This account name is already in use as a channel name...");
                cmbAccount.Focus();
                return;
            }

            if (txtPassword.Text.Trim().Length < 3)
            {
                Interaction.MsgBox("Password must be at least three characters...");
                txtPassword.Focus();
                return;
            }
            else if (txtPassword.Text.Trim().Length > 12)
            {
                Interaction.MsgBox("Password must be twelve characters or less long...");
                txtPassword.Focus();
                return;
            }

            btnAdd.Enabled = false;
            if (Interaction.MsgBox("Are you sure you want all messages to " + strAccountName + " to be delivered to this site?", MsgBoxStyle.Question | MsgBoxStyle.YesNoCancel, "New Account") == MsgBoxResult.Yes)
            {
                if (AddNewAccount())
                {
                    Interaction.MsgBox("The new account '" + strAccountName + "' has been added. ", MsgBoxStyle.Information, "Account Added");
                }
            }
            else
            {
                Interaction.MsgBox("Callsign entry cancelled", MsgBoxStyle.Information, "New Account");
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
                Interaction.MsgBox("The site callsign account cannot be removed from the Accounts menu!" + Constants.vbCr + "Use the Properties menu to change the site callsign.", MsgBoxStyle.Information, "Remove Account");
                return;
            }

            if (Interaction.MsgBox("Confirm removal of '" + strAccount + "' from the this site?", MsgBoxStyle.OkCancel, "Confirm Remove") == MsgBoxResult.Cancel)
                return;
            string strAccountList = Globals.objINIFile.GetString("Properties", "Account Names", "");
            while (strAccountList.IndexOf(strAccount + "|") != -1) // This added to delete multiples if they exist
                strAccountList = Strings.Replace(strAccountList, strAccount + "|", "");
            Globals.objINIFile.WriteString("Properties", "Account Names", strAccountList);
            Directory.Delete(Globals.SiteRootDirectory + @"Accounts\" + strAccount + "_Account", true);
            Globals.objINIFile.DeleteSection(strAccount);
            OutlookExpress.RemoveOutlookExpressAccount(strAccount);
            Accounts.RefreshAccountsList();
            FillAccountSelection();
        } // btnRemove_Click

        private void btnOutlookExpress_Click(object sender, EventArgs e)
        {
            // 
            // Add this selected account to Outlook Express in this machine.
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
            string strCallsign = cmbAccount.Text;
            if (strCallsign != "<Add new account>" && string.Compare(txtPassword.Text.Trim(), strOldPassword, false) != 0)
            {
                // Update the password for this callsign.
                Globals.objINIFile.WriteString(strCallsign, "EMail Password", txtPassword.Text.Trim());
            }

            Globals.UpdateAccountDirectories();
            Close();
        } // btnClose_Click

        private void FillAccountSelection()
        {
            cmbAccount.Items.Clear();
            cmbAccount.Sorted = true;
            foreach (Accounts.TAccount UserAccount in Accounts.AccountsList)
            {
                if (Globals.IsValidRadioCallsign(UserAccount.Name))
                {
                    if (!string.IsNullOrEmpty(UserAccount.Name))
                        cmbAccount.Items.Add(UserAccount.Name);
                }
            }

            cmbAccount.Items.Add("<Add new account>");
            cmbAccount.Text = Conversions.ToString(cmbAccount.Items[0]);
            cmbAccount.Focus();
        } // FillAccountSelection

        private void EntryErrorMessage()
        {
            // 
            // Display an account entry error message box...
            // 
            Interaction.MsgBox("Callsign account names must be a valid ham or MARS radio callsign with an optional -SSID between 1 and 15.", MsgBoxStyle.Information);
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
            if (!Information.IsNumeric(strTokens[1]))
                return false;
            int intSSID = Conversions.ToInteger(strTokens[1]);
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
            string strChannelNames = Globals.objINIFile.GetString("Properties", "Channel Names", "");
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
            // If RegisterRadioCallsign(cmbAccount.Text, txtPassword.Text, txtBasePassword.Text.Trim) Then
            // 
            // Make a new sub-directory for this account.
            // 
            Directory.CreateDirectory(Globals.SiteRootDirectory + @"Accounts\" + cmbAccount.Text + "_Account");
            string strAccountList = Globals.objINIFile.GetString("Properties", "Account Names", "");
            // 
            // Add the account name to the registry list.
            // 
            strAccountList = strAccountList + cmbAccount.Text + "|";
            Globals.objINIFile.WriteString("Properties", "Account Names", strAccountList);
            Globals.objINIFile.WriteString(cmbAccount.Text, "EMail Password", txtPassword.Text.Trim());
            strOldPassword = txtPassword.Text;
            blnReturn = true;
            // 
            // Restore the dialog box window.
            // 
            btnOutlookExpress.Enabled = true;
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