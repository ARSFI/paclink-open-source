using System.Collections.Generic;

namespace Paclink
{
    public class Accounts
    {
        public static Dictionary<string, AccountRecord> AccountsList;
        public static string AccountsString;

        public static void RefreshAccountsList()
        {
            // Clear user list...
            AccountsList = null;
            AccountsList = new Dictionary<string, AccountRecord>();
            AccountsString = Globals.Settings.Get("Properties", "Account Names", "");
            string strAccountsListNoDupes = ""; // Added to purge ini file of any duplicate accounts
            if (!string.IsNullOrEmpty(AccountsString))
            {
                var strAccountsList = AccountsString.Split('|');

                // Fill user list...
                foreach (string strEachAccount in strAccountsList)
                {
                    if (!string.IsNullOrEmpty(strEachAccount))
                    {
                        AccountRecord newAccount;
                        newAccount.Name = strEachAccount;
                        newAccount.Password = Globals.Settings.Get(strEachAccount, "EMail Password", "");
                        newAccount.AlternateAddress = Globals.Settings.Get(strEachAccount, "Alternate Address", "");
                        newAccount.Prefix = Globals.Settings.Get(strEachAccount, "Prefix", "");
                        newAccount.Suffix = Globals.Settings.Get(strEachAccount, "Suffix", "");
                        newAccount.MimePathIn = string.Empty;
                        newAccount.MimePathOut = string.Empty;
                        try
                        {
                            AccountsList.Add(strEachAccount, newAccount);
                            strAccountsListNoDupes += strEachAccount + "|";
                        }
                        catch
                        {
                            // Do nothing, user already added...
                        }
                    }
                }

                if ((strAccountsListNoDupes ?? "") != (AccountsString ?? ""))
                {
                    // There were duplicates in the list so clean up the ini file
                    Globals.Settings.Save("Properties", "Account Names", strAccountsListNoDupes);
                }
            }
        }

        public static AccountRecord GetUserAccount(string strAccountName)
        {
            try
            {
                return AccountsList[strAccountName];
            }
            catch
            {
                return default;
            }
        }
    }
}