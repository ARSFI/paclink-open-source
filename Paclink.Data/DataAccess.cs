using System;
using System.Data.SQLite;
using System.IO;
using NLog;

namespace Paclink.Data
{
    public static class DataAccess
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static string _databaseLocation;

        static DataAccess()
        {
            _databaseLocation = Environment.SpecialFolder.ApplicationData.ToString();
        }

        public static void AddCallsignAccount(string callsign)
        {
            throw new NotImplementedException();
        }

        public static void AddMimeMessage(string message)
        {
            throw new NotImplementedException();
        }

        public static void AddTacticalAccount(string tactical)
        {
            throw new NotImplementedException();
        }

        public static string DumpConfiguration()
        {
            throw new NotImplementedException();
        }

        public static PaclinkConfiguration GetConfiguration()
        {
            throw new NotImplementedException();
        }
        public static string GetMimeMessage()
        {
            throw new NotImplementedException();
        }

        public static string GetVersion()
        {
            string connectionString = "Data Source=:memory:";
            string sql = "SELECT SQLITE_VERSION()";

            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            using var cmd = new SQLiteCommand(sql, connection);
            string version = cmd.ExecuteScalar().ToString();

            return version;
        }
        public static void RemoveCallsignAccount(string callsign)
        {
            throw new NotImplementedException();
        }

        public static void RemoveTacticalAccount(string tactical)
        {
            throw new NotImplementedException();
        }

        public static void SetConfiguration(PaclinkConfiguration config)
        {
            throw new NotImplementedException();
        }

        public static void SetDatabaseLocation(string path)
        {
            //validate path and create if necessary
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _databaseLocation = path;
            }
            catch (Exception ex)
            {
                Log.Warn(ex, ex.Message);
                throw;
            }
        }

    }
}
