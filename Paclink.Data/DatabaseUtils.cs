using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using NLog;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities

namespace Paclink.Data
{
    public static class DatabaseUtils
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private const string DatabaseName = "paclink.db";
        private static readonly Version DatabaseVersion = new Version("1.0.0");
        private static readonly string ConnectionString;

        public const string SchemaVersionProperty = "Schema Version";

        static DatabaseUtils()
        {
            try
            {
                //validate path and create if necessary
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Paclink");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //update connection string
                var filepath = Path.Combine(path, DatabaseName);
                ConnectionString = $"URI=file:{filepath}";
                Log.Trace($"Database connection string: {ConnectionString}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }

        public static void CreateDatabase()
        {
            try
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();

                var sql = "CREATE TABLE IF NOT EXISTS Properties (Timestamp TEXT, Property TEXT NOT NULL, Value TEXT, PRIMARY KEY(Property))";
                using var cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Accounts(Callsign STRING PRIMARY KEY, Password STRING, Tactical BOOLEAN)";
                cmd.ExecuteNonQuery();

                //add db version property
                Properties.SaveProperty(SchemaVersionProperty, DatabaseVersion.ToString());
                Log.Trace($"Database version: {DatabaseVersion}");

                //:TODO
                //throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }

        public static DataSet FillDataSet(string sql, string tableName)
        {
            try
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();

                using SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                var count = adapter.Fill(ds, tableName);
                Log.Trace($"Loaded {count} rows from table: {tableName} using query: {sql}");
                return ds;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw;
            }
        }

        public static Version GetSchemaVersion()
        {
            var v = Properties.GetProperty(SchemaVersionProperty, DatabaseVersion.ToString());
            return new Version(v);
        }

        public static string GetSingleValue(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                var value = reader.GetString(0);
                Log.Trace($"Retrieved string value '{value}' using query: {sql}");
                return value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return string.Empty;
            }
        }

        public static DateTime GetSingleValueDateTime(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                var value = reader.GetDateTime(0);
                Log.Trace($"Retrieved DateTime value '{value}' using query: {sql}");
                return value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return DateTime.MinValue;
            }
        }

        public static long GetSingleValueLong(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                var value = reader.GetInt64(0);
                Log.Trace($"Retrieved long value '{value}' using query: {sql}");
                return value;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return long.MinValue;
            }
        }

        public static void MigrateDatabase(string schemaChanges, Version newVersion)
        {
            throw new NotImplementedException();
        }

        public static void NonQuery(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(ConnectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                var count = cmd.ExecuteNonQuery();
                Log.Trace($"{count} records were impacted running script: {sql}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
        }

    }
}
