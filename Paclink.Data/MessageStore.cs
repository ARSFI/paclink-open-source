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

        public byte[] GetFromWinlinkMessage(string messageId)
        {
            var sql = "SELECT `MessageData` FROM `FromWinlinkMessage` WHERE `MessageId`='{0}'";
            var messages = _database.FillDataSet(string.Format(sql, messageId), "FromWinlinkMessage");

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

        public void DeleteFromWinlinkMessage(string messageId)
        {
            _database.NonQuery(string.Format(@"DELETE FROM `FromWinlinkMessage` WHERE `MessageId`=='{0}'", messageId));
        }

        public void SaveFromWinlinkMessage(string messageId, byte[] message)
        {
            try
            {
                _database.NonQuery("BEGIN");
                DeleteFromWinlinkMessage(messageId);

                var base64String = Convert.ToBase64String(message);
                _database.NonQuery(string.Format(@"INSERT INTO `FromWinlinkMessage` (`MessageId`, `MessageData`) VALUES ('{0}', '{1}')", messageId, base64String));
                _database.NonQuery("COMMIT");
            }
            finally
            {
                _database.NonQuery("ROLLBACK");
            }
        }

        public List<KeyValuePair<string, byte[]>> GetFromWinlinkMessages()
        {
            var sql = "SELECT `MessageId`, `MessageData` FROM `FromWinlinkMessage`";
            var messages = _database.FillDataSet(sql, "FromWinlinkMessage");

            var result = new List<KeyValuePair<string, byte[]>>();

            for (var index = 0; index < messages.Tables[0].Rows.Count; index++)
            {
                var kvp = new KeyValuePair<string, byte[]>(
                    messages.Tables[0].Rows[index]["MessageId"].ToString(),
                    Convert.FromBase64String(messages.Tables[0].Rows[index]["MessageId"].ToString()));
                result.Add(kvp);
            }

            return result;
        }

        public byte[] GetToWinlinkMessage(string messageId)
        {
            var sql = "SELECT `MessageData` FROM `ToWinlinkMessage` WHERE `MessageId`='{0}'";
            var messages = _database.FillDataSet(string.Format(sql, messageId), "ToWinlinkMessage");

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

        public void DeleteToWinlinkMessage(string messageId)
        {
            _database.NonQuery(string.Format(@"DELETE FROM `ToWinlinkMessage` WHERE `MessageId`=='{0}'", messageId));
        }

        public void SaveToWinlinkMessage(string messageId, byte[] message)
        {
            try
            {
                _database.NonQuery("BEGIN");
                DeleteToWinlinkMessage(messageId);

                var base64String = Convert.ToBase64String(message);
                _database.NonQuery(string.Format(@"INSERT INTO `ToWinlinkMessage` (`MessageId`, `MessageData`) VALUES ('{0}', '{1}')", messageId, base64String));
                _database.NonQuery("COMMIT");
            }
            finally
            {
                _database.NonQuery("ROLLBACK");
            }
        }

        public List<KeyValuePair<string, byte[]>> GetToWinlinkMessages()
        {
            var sql = "SELECT `MessageId`, `MessageData` FROM `ToWinlinkMessage`";
            var messages = _database.FillDataSet(sql, "ToWinlinkMessage");

            var result = new List<KeyValuePair<string, byte[]>>();

            for (var index = 0; index < messages.Tables[0].Rows.Count; index++)
            {
                var kvp = new KeyValuePair<string, byte[]>(
                    messages.Tables[0].Rows[index]["MessageId"].ToString(),
                    Convert.FromBase64String(messages.Tables[0].Rows[index]["MessageId"].ToString()));
                result.Add(kvp);
            }

            return result;
        }

        public void SaveFailedMimeMessage(string messageId, byte[] message)
        {
            var base64String = Convert.ToBase64String(message);
            _database.NonQuery(string.Format(@"INSERT INTO `FailedMimeMessage` (`MessageId`, `MessageData`) VALUES ('{0}', '{1}')", messageId, base64String));
        }
    }
}
