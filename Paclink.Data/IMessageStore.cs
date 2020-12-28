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

        // TBD: MidsSeen store -- add to separate interface?
        void PurgeMessageIdsSeen();
        bool IsMessageIdSeen(string messageId);
        void AddMessageIdSeen(string messageId);

        // Replacement for "Temp Inbound" folder.
        byte[] GetTemporaryInboundMessage(string messageId);
        void DeleteTemporaryInboundMessage(string messageId);
        void PurgeOldTemporaryInboundMessages();
        void SaveTemporaryInboundMessage(string messageId, byte[] message);

        // Replacement for "From Winlink" folder.
        byte[] GetFromWinlinkMessage(string messageId);
        void DeleteFromWinlinkMessage(string messageId);
        void SaveFromWinlinkMessage(string messageId, byte[] message);
        List<KeyValuePair<string, byte[]>> GetFromWinlinkMessages();

        // Replacement for "To Winlink" folder.
        byte[] GetToWinlinkMessage(string messageId);
        void DeleteToWinlinkMessage(string messageId);
        void SaveToWinlinkMessage(string messageId, byte[] message);

        void SaveFailedMimeMessage(string messageId, byte[] message);
    }
}
