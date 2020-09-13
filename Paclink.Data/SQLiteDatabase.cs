using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using NLog;

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities

namespace Paclink.Data
{
    public class SQLiteDatabase : IDatabase
    {
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
        private const string DatabaseName = "paclink.db";
        private readonly Version _databaseVersion = new Version("1.0.0");
        private readonly string _connectionString;

        public const string SchemaVersionProperty = "Schema Version";

        public SQLiteDatabase()
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
                _connectionString = $"URI=file:{filepath}";
                _log.Trace($"Database connection string: {_connectionString}");

                //create the database if it does not already exist
                CreateDatabase();
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                throw;
            }
        }

        public void CreateDatabase()
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                var sql = "CREATE TABLE IF NOT EXISTS Properties (Timestamp TEXT,`Group` TEXT DEFAULT '',`Property` TEXT NOT NULL,`Value` TEXT DEFAULT '',PRIMARY KEY(`Group`,`Property`))";
                using var cmd = new SQLiteCommand(sql, connection);
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Accounts(`Callsign` STRING PRIMARY KEY, `Password` STRING, `Tactical` BOOLEAN)";
                cmd.ExecuteNonQuery();

                //add db version property
                cmd.CommandText = $"REPLACE INTO Properties (`Timestamp`,`Group`,`Property`,`Value`) VALUES (datetime('now'),'System','{SchemaVersionProperty}','{_databaseVersion}')";
                cmd.ExecuteNonQuery();
                _log.Trace($"Database version: {_databaseVersion}");

                //:TODO
                //throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                throw;
            }
        }

        public bool ExistsQuery(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                using var cmd = new SQLiteCommand(sql, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                bool result = reader.HasRows;
                _log.Trace($"Query: {sql} Returned one or more rows: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                throw;
            }
        }

        public DataSet FillDataSet(string sql, string tableName)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                using SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                var count = adapter.Fill(ds, tableName);
                _log.Trace($"Loaded {count} rows from table: {tableName} using query: {sql}");
                return ds;
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                throw;
            }
        }

        public string GetSingleValue(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                var value = cmd.ExecuteScalar();
                if (value == null) return string.Empty;
                var stringValue = Convert.ToString(value);
                _log.Trace($"Retrieved string value '{stringValue}' using query: {sql}");
                return stringValue;
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                return string.Empty;
            }
        }

        public DateTime GetSingleValueDateTime(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (!reader.HasRows) return DateTime.MinValue;
                var value = reader.GetDateTime(0);
                _log.Trace($"Retrieved DateTime value '{value}' using query: {sql}");
                return value;
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                return DateTime.MinValue;
            }
        }

        public long GetSingleValueLong(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                using SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (!reader.HasRows) return long.MinValue;
                var value = reader.GetInt64(0);
                _log.Trace($"Retrieved long value '{value}' using query: {sql}");
                return value;
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
                return long.MinValue;
            }
        }

        public void NonQuery(string sql)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();
                using var cmd = new SQLiteCommand(sql, connection);
                var count = cmd.ExecuteNonQuery();
                _log.Trace($"{count} records were inserted/updated running script: {sql}");
            }
            catch (Exception ex)
            {
                _log.Error(ex, ex.Message);
            }
        }

    }
}
