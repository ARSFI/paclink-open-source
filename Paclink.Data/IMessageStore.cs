using System.Collections.Generic;

namespace Paclink.Data
{
    public interface IMessageStore
    {
        void AddMessageAddress(string messageId, string address);
        string GetMessage(string messageId);
        List<string> GetMessageList();
        bool MessageExists(string messageId);
        void SaveMessage(string messageId, string message, bool processed);
    }
}
