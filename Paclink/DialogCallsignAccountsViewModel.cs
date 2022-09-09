using Paclink.UI.Common;
using Paclink.Data;
using System.Collections.Generic;

namespace Paclink
{
    internal class DialogCallsignAccountsViewModel : ICallsignAccountsBacking
    {
        public string SiteRootDirectory { get; }
        public string SiteCallsign { get; }

        public bool AddUserAccount(string accountName, string password)
        {
            string strAccountList = Globals.Settings.Get("Properties", "Account Names", "");
            // 
            // Add the account name to the list.
            // 
            strAccountList = strAccountList + accountName + "|";
            Globals.Settings.Save("Properties", "Account Names", strAccountList);
            Globals.Settings.Save(accountName, "EMail Password", password);
            Accounts.RefreshAccountsList();
            return true;
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

        public List<string> GetAccountNames()
        {
            var accountNames = new List<string>();
            foreach (AccountRecord UserAccount in Accounts.AccountsList.Values)
            {
                if (Globals.IsValidRadioCallsign(UserAccount.Name))
                {
                    if (!string.IsNullOrEmpty(UserAccount.Name))
                        accountNames.Add(UserAccount.Name);
                }
            }
            return accountNames;
        }

        public string GetAccountPassword(string accountName)
        {
            return Globals.Settings.Get(accountName, "EMail Password", "");
        }

        public object GetUserAccount(string accountName)
        {
            return Accounts.GetUserAccount(accountName);
        }

        public bool IsAccount(string accountName)
        {
            // 
            // Returns True if the indicated account name is registered locally.
            // 
            var Account = Accounts.GetUserAccount(accountName);
            if ((Account.Name ?? "") == (accountName ?? ""))
                return true;
            return false;
        }

        public bool IsChannel(string channelName)
        {
            string channelNames = Globals.Settings.Get("Properties", "Channel Names", "");
            if (channelNames.IndexOf(channelName + "|") != -1)
                return true;
            else
                return false;
        }

        public bool IsValidFileName(string text)
        {
            return Globals.IsValidFileName(text);
        }

        public bool IsValidRadioCallsign(string callsign)
        {
            return Globals.IsValidRadioCallsign(callsign);
        }

        public void RefreshAccountsList()
        {
            Accounts.RefreshAccountsList();
        }

        public void RefreshWindow()
        {
            // empty
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

        public void SaveAccountPassword(string callsign, string password)
        {
            Globals.Settings.Save(callsign, "EMail Password", password);
        }

        public void UpdateAccountDirectories()
        {
            Globals.UpdateAccountDirectories();
        }
    }
}
