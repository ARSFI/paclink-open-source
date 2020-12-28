using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Paclink.Data
{
    public class MessageStore : IMessageStore
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _database;

        public MessageStore(IDatabase db)
        {
            _database = db;
        }

        public void AddMessageAddress(string messageId, string address)
        {
            throw new NotImplementedException();
        }

        public void AddMessageIdSeen(string messageId)
        {
            _database.NonQuery(string.Format(@"INSERT INTO `MessageIdsSeen` (`SeenDate`, `MessageId`) VALUES (datetime('now'), '{0}')", messageId));
        }

        public string GetMessage(string messageId)
        {
            throw new NotImplementedException();
        }

        public List<string> GetMessageList()
        {
            throw new NotImplementedException();
        }

        public bool IsMessageIdSeen(string messageId)
        {
            var sql = "SELECT `MessageId` FROM `MessageIdsSeen` WHERE `MessageId`='{0}'";
            var idList = _database.FillDataSet(string.Format(sql, messageId), "MessageIdsSeen");
            return idList.Tables[0].Rows.Count > 0;
        }

        public bool MessageExists(string messageId)
        {
            throw new NotImplementedException();
        }

        public void PurgeMessageIdsSeen()
        {
            _database.NonQuery(@"DELETE FROM `MessageIdsSeen` WHERE `SeenDate` <= datetime('now', '-30 days')");
        }

        public void SaveMessage(string messageId, string message, bool processed)
        {
            throw new NotImplementedException();
        }

        public byte[] GetTemporaryInboundMessage(string messageId)
        {
            var sql = "SELECT `MessageData` FROM `TempInboundMessage` WHERE `MessageId`='{0}'";
            var messages = _database.FillDataSet(string.Format(sql, messageId), "TempInboundMessage");
            
            if (messages.Tables[0].Rows.Count == 0)
            {
                return new byte[0];
            }
            else
            {
                var base64String = messages.Tables[0].Rows[0]["MessageData"].ToString();
                return Convert.FromBase64String(base64String);
            }
        }

        public void DeleteTemporaryInboundMessage(string messageId)
        {
            _database.NonQuery(string.Format(@"DELETE FROM `TempInboundMessage` WHERE `MessageId`=='{0}'", messageId));
        }

        public void SaveTemporaryInboundMessage(string messageId, byte[] message)
        {
            try
            {
                _database.NonQuery("BEGIN");
                DeleteTemporaryInboundMessage(messageId);

                var base64String = Convert.ToBase64String(message);
                _database.NonQuery(string.Format(@"INSERT INTO `TempInboundMessage` (`MessageId`, `Timestamp`, `MessageData`) VALUES ('{0}', datetime('now'), '{1}')", messageId, base64String));
                _database.NonQuery("COMMIT");
            } 
            finally
            {
                _database.NonQuery("ROLLBACK");
            }
        }

        public void PurgeOldTemporaryInboundMessages()
        {
            _database.NonQuery(@"DELETE FROM `TempInboundMessage` WHERE `Timestamp` <= datetime('now', '-1 days')");
        }
    }
}
