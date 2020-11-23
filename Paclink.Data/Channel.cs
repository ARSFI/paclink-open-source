using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Paclink.Data
{
    public class Channel : IChannel
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _database;

        public Channel(IDatabase db)
        {
            _database = db;
        }

        public void BeginUpdateSession()
        {
            _database.NonQuery("BEGIN");
        }

        public void CommitUpdateSession()
        {
            _database.NonQuery("COMMIT");
        }

        public void RollbackUpdateSession()
        {
            _database.NonQuery("ROLLBACK");

        }
        public void AddChannelRecord(bool isPacket, string callsign, int frequency, string gridSquare, int mode, string operatingHours, string serviceCode)
        {
            var sql = @"
                INSERT INTO `Channel` (`IsPacket`, `Callsign`, `Frequency`, `GridSquare`, `Mode`, `OperatingHours`, `ServiceCode`)
                VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
            _database.NonQuery(string.Format(sql, isPacket, callsign, frequency, gridSquare, mode, operatingHours, serviceCode));
        }

        public void ClearChannelList(bool isPacket)
        {
            var sql = "DELETE FROM `Channel` WHERE `IsPacket`= {0}";
            _database.NonQuery(string.Format(sql, isPacket));
        }

        public bool ContainsChannelList(bool isPacket)
        {
            var sql = "SELECT `IsPacket`, `Callsign`, `Frequency`, `GridSquare`, `Mode`, `OperatingHours`, `ServiceCode` FROM `Channel`";
            var channelList = _database.FillDataSet(sql, "Channel");
            return channelList.Tables[0].Rows.Count > 0;
        }

        public SortedDictionary<string, string> GetChannelList(bool isPacket)
        {
            var result = new SortedDictionary<string, string>();
            var sql = "SELECT `IsPacket`, `Callsign`, `Frequency`, `GridSquare`, `Mode`, `OperatingHours`, `ServiceCode` FROM `Channel`";
            var channelList = _database.FillDataSet(sql, "Channel");

            foreach (DataRow row in channelList.Tables[0].Rows)
            {
                var entry =
                    row["Frequency"].ToString() + "|" +
                    row["GridSquare"].ToString() + "|" +
                    row["Mode"].ToString() + "|" +
                    row["OperatingHours"].ToString() + "|" +
                    row["ServiceCode"].ToString();

                var callsign = row["Callsign"].ToString();

                if (result.ContainsKey(callsign))
                {
                    result[callsign] += "," + entry;
                }
                else 
                {
                    result[callsign] = entry;
                }
            }

            return result;
        }
    }
}
