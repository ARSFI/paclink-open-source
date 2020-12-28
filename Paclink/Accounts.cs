using System.Collections.Generic;

namespace Paclink
{
    public class Accounts
    {
        public struct AccountRecord
        {
            public string Name;               // The account name as set up on the users POP3 Client setup
            public string Password;           // The user Password as set up on the users POP3 Client setup
            public string MimePathIn;         // The directory of the MIME files for messages from this user
            public string MimePathOut;        // The directory of the MIME files for messages to this user
            public string AlternateAddress;   // an alternate (non winlink address for forwarding)
            public string Prefix;             // Optional callsign prefix for ID
            public string Suffix;             // Optional callsign Suffix for ID 
        } // AccountRecord

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
                        newAccount.MimePathOut = Globals.SiteRootDirectory + @"Accounts\" + strEachAccount + @"_Account\";
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