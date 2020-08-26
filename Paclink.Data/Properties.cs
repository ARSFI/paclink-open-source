using System;
using NLog;

namespace Paclink.Data
{
    public static class Properties
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void DeleteProperty(string propertyName)
        {
            var sql = $"DELETE IGNORE FROM Properties WHERE Property='{propertyName}'";
            DatabaseUtils.NonQuery(sql);
        }

        public static string GetProperty(string propertyName, string defaultValue)
        {
            try
            {
                var sql = $"SELECT Value FROM Properties WHERE Property='{propertyName}'";
                string result = DatabaseUtils.GetSingleValue(sql);

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
                Log.Error(ex);
                return defaultValue;
            }
        }

        public static bool GetProperty(string propertyName, bool defaultValue)
        {
            return Convert.ToBoolean(GetProperty(propertyName, defaultValue.ToString()));
        }

        public static int GetProperty(string propertyName, int defaultValue)
        {
            return Convert.ToInt32(GetProperty(propertyName, defaultValue.ToString()));
        }

        public static void SaveProperty(string propertyName, string value)
        {
            //var ts = DateTime.UtcNow.ToString("O"); //ISO8601 format
            var sql = $"REPLACE INTO Properties (Timestamp,Property,Value) VALUES (datetime('now'),'{propertyName}','{value}')";
            DatabaseUtils.NonQuery(sql);
        }

    }
}
