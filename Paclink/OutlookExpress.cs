using Microsoft.Win32;
using System.Windows.Forms;

namespace Paclink
{
    public class OutlookExpress
    {
        public static void AddOutlookExpressAccount(string strAccount)
        {
            // Add an account to Outlook Express in this machine (only)...

            strAccount = strAccount.ToUpper();
            string strPartialKey = @"Software\Microsoft\Internet Account Manager\Accounts\";
            RegistryKey objKey;
            // First check all keys 1 to 100 for possible duplcate Account name
            for (int intIndex = 1; intIndex <= 100; intIndex++)
            {
                objKey = Registry.CurrentUser.OpenSubKey(strPartialKey + intIndex.ToString("X").PadLeft(8, '0'));
                if (objKey != null)
                {
                    if ((strAccount ?? "") == (objKey.GetValue("Account Name", "").ToString() ?? ""))
                    {
                        MessageBox.Show("Account " + strAccount + " already exists in the local Outlook Express!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
            }
            // no duplicats so put new account into the first empty space.
            for (int intIndex = 1; intIndex <= 100; intIndex++)
            {
                objKey = Registry.CurrentUser.OpenSubKey(strPartialKey + intIndex.ToString("X").PadLeft(8, '0'));
                if (objKey == null)
                {
                    objKey = Registry.CurrentUser.CreateSubKey(strPartialKey + intIndex.ToString("X").PadLeft(8, '0'));
                    objKey.SetValue("Account Name", strAccount);
                    objKey.SetValue("Connection Type", 0);
                    objKey.SetValue("SMTP User Name", strAccount);
                    objKey.SetValue("SMTP Display Name", strAccount);
                    objKey.SetValue("SMTP Email Address", strAccount + "@Winlink.org");
                    objKey.SetValue("SMTP Server", "Localhost");
                    objKey.SetValue("SMTP Use Sicily", 2);
                    objKey.SetValue("POP3 User Name", strAccount);
                    objKey.SetValue("POP3 Server", "Localhost");
                    objKey.SetValue("POP3 Prompt for Password", 0);
                    objKey.SetValue("POP3 Skip Account", 0);
                    MessageBox.Show("Account " + strAccount + " has been added to your local Outlook Express." + Globals.CR + "Shut down and restart Outlook Express and be sure to set" + Globals.CR + " the account password in Outlook Express before using the account." + Globals.CR + "If your OE is not updated or you are using another Client" + Globals.CR + "you may add the account manually.", "Account Added", MessageBoxButtons.OK, MessageBoxIcon.Information);



                    return;
                }
            }
        } // AddOutlookExpressAccount

        public static void RemoveOutlookExpressAccount(string strAccount)
        {
            string strKey = @"Software\Microsoft\Internet Account Manager\Accounts\";
            var objKey = Registry.CurrentUser.OpenSubKey(strKey, true);
            if (objKey is object)
            {
                var strSubKeys = objKey.GetSubKeyNames();
                foreach (string sSubKey in strSubKeys)
                {
                    int temp = 0;
                    if (int.TryParse(sSubKey, out temp))
                    {
                        if (KeyTest(strKey + sSubKey, strAccount))
                        {
                            DeleteKey(strKey, sSubKey);
                        }
                    }
                }
            }
        } // RemoveOutlookExpressAccount

        private static bool KeyTest(string strKey, string strAccount)
        {
            var objKey = Registry.CurrentUser.OpenSubKey(strKey);
            if ((strAccount ?? "") == (objKey.GetValue("Account Name", "").ToString() ?? ""))
                return true;
            return false;
        } // KeyTest

        private static void DeleteKey(string strKey, string sSubKey)
        {
            var objKey = Registry.CurrentUser.OpenSubKey(strKey, true);
            objKey.DeleteSubKey(sSubKey);
        } // DeleteKey
    }
}