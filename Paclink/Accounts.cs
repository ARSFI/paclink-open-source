using System;
using System.Collections.Generic;

namespace Paclink
{
    public class Accounts
    {
        public struct TAccount
        {
            public string Name;               // The account name as set up on the users POP3 Client setup
            public string Password;           // The user Password as set up on the users POP3 Client setup
            public string MimePathIn;         // The directory of the MIME files for messages from this user
            public string MimePathOut;        // The directory of the MIME files for messages to this user
            public int AttachmentLimit;   // The attachment limit in bytes 0 = no attachments
            public bool WhitelistNotice;   // Flag set to send notification on whitelist block
            public string AlternateAddress;   // an alternate (non winlink address for fowarding)
            public string Prefix;             // Optional callsign prefix for ID
            public string Suffix;             // Optoinal callsign Suffix for ID 
        } // TAccount

        public static Dictionary<string, TAccount> AccountsList;
        public static string AccountsString;

        public static void RefreshAccountsList()
        {
            // Clear user list...
            AccountsList = null;
            AccountsList = new Dictionary<string, TAccount>();
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
                        TAccount NewAccount;
                        NewAccount.Name = strEachAccount;
                        NewAccount.Password = Globals.Settings.Get(strEachAccount, "EMail Password", "");
                        NewAccount.AlternateAddress = Globals.Settings.Get(strEachAccount, "Alternate Address", "");
                        NewAccount.AttachmentLimit = Globals.Settings.Get(strEachAccount, "Size Limit", 120000);
                        NewAccount.Prefix = Globals.Settings.Get(strEachAccount, "Prefix", "");
                        NewAccount.Suffix = Globals.Settings.Get(strEachAccount, "Suffix", "");
                        NewAccount.WhitelistNotice = Globals.Settings.Get(strEachAccount, "Whitelist Notice", false);
                        NewAccount.MimePathIn = Globals.SiteRootDirectory + @"To Winlink\";
                        NewAccount.MimePathOut = Globals.SiteRootDirectory + @"Accounts\" + strEachAccount + @"_Account\";
                        try
                        {
                            AccountsList.Add(strEachAccount, NewAccount);
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

        public static TAccount GetUserAccount(string strAccountName)
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