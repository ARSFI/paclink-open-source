using System.Collections.Generic;

namespace Paclink.UI.Common
{
    public interface ICallsignAccountsBacking : IFormBacking
    {
        string SiteRootDirectory { get; }
        string SiteCallsign { get; }

        bool AddUserAccount(string accountName, string password);
        List<string> GetAccountNames();
        object GetUserAccount(string account);
        bool IsAccount(string accountName);
        bool IsChannel(string channelName);
        bool IsValidFileName(string text);
        void RefreshAccountsList();
        void RemoveAccount(string strAccount);
        bool IsValidRadioCallsign(string callsign);
        void UpdateAccountDirectories();
        string GetAccountPassword(string account);
        void SaveAccountPassword(string account, string v);
    }
}
