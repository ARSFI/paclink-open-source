using System.Collections.Generic;

namespace Paclink.UI.Common
{
    public interface ITacticalAccountsBacking : IFormBacking
    {
        string SiteRootDirectory { get; }
        bool IsValidFileName(string text);
        string GetEmailPassword(string text);
        bool ChangeTacticalPassword(string account, string password);
        bool AccountExists(string account);
        void RemoveAccount(string accountName);
        bool ChangePassword(string account, string oldPassword, string newPassword);
        bool ValidatePassword(string tacticalAddress, string password);
        void AddTacticalAddress(string tacticalAddress, string password);
        void RefreshAccountsList();
        AccountRecord GetUserAccount(string accountName);
        void SaveEmailPassword(string account, string newPassword);
        void UpdateAccountDirectories();
        string GetAccountNames();
        void SaveAccountNames(string accountList);
        List<string> GetTacticalAccountNames();
    }
}
