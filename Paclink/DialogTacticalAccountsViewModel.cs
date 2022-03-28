using Paclink.Data;
using Paclink.UI.Common;
using System.Collections.Generic;
using winlink;

namespace Paclink
{
    internal class DialogTacticalAccountsViewModel : ITacticalAccountsBacking
    {
        public string SiteRootDirectory => Globals.SiteRootDirectory;

        public bool AccountExists(string account)
        {
            return WinlinkWebServices.AccountExists(account);
        }

        public void AddTacticalAddress(string tacticalAddress, string password)
        {
            WinlinkWebServices.AddTacticalAddress(tacticalAddress, password);
        }

        public bool ChangePassword(string account, string oldPassword, string newPassword)
        {
            try
            {
                WinlinkWebServices.ChangePassword(account, oldPassword, newPassword);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetAccountNames()
        {
            return Globals.Settings.Get("Properties", "Account Names", "");
        }

        public string GetEmailPassword(string accountName)
        {
            return Globals.Settings.Get(accountName, "EMail Password", "");
        }

        public AccountRecord GetUserAccount(string accountName)
        {
            return Accounts.GetUserAccount(accountName);
        }

        public List<string> GetTacticalAccountNames()
        {
            var accountNames = new List<string>();
            foreach (AccountRecord UserAccount in Accounts.AccountsList.Values)
            {
                if (!Globals.IsValidRadioCallsign(UserAccount.Name))
                {
                    if (!string.IsNullOrEmpty(UserAccount.Name))
                        accountNames.Add(UserAccount.Name);
                }
            }
            return accountNames;
        }

        public bool IsValidFileName(string text)
        {
            return Globals.IsValidFileName(text);
        }

        public void RefreshAccountsList()
        {
            Accounts.RefreshAccountsList();
        }

        public void RefreshWindow()
        {
            // empty
        }

        public void SaveAccountNames(string accountList)
        {
            Globals.Settings.Save("Properties", "Account Names", accountList);
        }

        public void RemoveAccount(string accountName)
        {
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            while (strAccountList.IndexOf(accountName + "|") != -1) // This added to delete multiples if they exist
                strAccountList = strAccountList.Replace(accountName + "|", "");
            Globals.Settings.Save("Properties", "Account Names", strAccountList);

            var messageStore = new MessageStore(DatabaseFactory.Get());
            messageStore.DeleteAccountEmails(accountName);

            Globals.Settings.DeleteGroup(accountName);
            Accounts.RefreshAccountsList();
        }

        public void SaveEmailPassword(string account, string newPassword)
        {
            Globals.Settings.Save(account, "EMail Password", newPassword);
        }

        public bool ChangeTacticalPassword(string account, string oldPassword)
        {
            DialogChangePasswordViewModel vm = new DialogChangePasswordViewModel();
            vm.OldPassword = oldPassword;
            UserInterfaceFactory.GetUiSystem().DisplayForm(AvailableForms.ChangePassword, vm);
            if (vm.DialogResult == DialogFormResult.OK)
            {
                var newPassword = vm.NewPassword;
                // Update password locally
                SaveEmailPassword(account, newPassword);
                return true;
            }
            return false;
        }

        public void UpdateAccountDirectories()
        {
            Globals.UpdateAccountDirectories();
        }

        public bool ValidatePassword(string tacticalAddress, string password)
        {
            return WinlinkWebServices.ValidatePassword(tacticalAddress, password);
        }

        public void CloseWindow()
        {
            // empty
        }

        public void FormClosed()
        {
            // empty
        }

        public void FormClosing()
        {
            // empty
        }

        public void FormLoaded()
        {
            // empty
        }

        public void FormLoading()
        {
            // empty
        }

    }
}
