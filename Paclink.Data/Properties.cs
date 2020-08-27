using System;
using NLog;

namespace Paclink.Data
{
    public class Properties : IProperties
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private readonly IDatabase _database;

        public Properties(IDatabase db)
        {
            _database = db;
        }
        
        public void DeleteProperty(string propertyName)
        {
            var sql = $"DELETE IGNORE FROM Properties WHERE Property='{propertyName}'";
            _database.NonQuery(sql);
        }

        public string GetProperty(string propertyName, string defaultValue)
        {
            try
            {
                var sql = $"SELECT Value FROM Properties WHERE Property='{propertyName}'";
                string result = _database.GetSingleValue(sql);

                //  Return result if no default specified
                if (string.IsNullOrWhiteSpace(defaultValue))
                {
                    return result;
                }

                //  Return result if a value was found 
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result;
                }

                //add the default value (if property not found)
                SaveProperty(propertyName, defaultValue);
                return defaultValue;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return defaultValue;
            }
        }

        public bool GetProperty(string propertyName, bool defaultValue)
        {
            return Convert.ToBoolean(GetProperty(propertyName, defaultValue.ToString()));
        }

        public int GetProperty(string propertyName, int defaultValue)
        {
            return Convert.ToInt32(GetProperty(propertyName, defaultValue.ToString()));
        }

        public void SaveProperty(string propertyName, string value)
        {
            //var ts = DateTime.UtcNow.ToString("O"); //ISO8601 format
            var sql = $"REPLACE INTO Properties (Timestamp,Property,Value) VALUES (datetime('now'),'{propertyName}','{value}')";
            _database.NonQuery(sql);
        }

    }
}
