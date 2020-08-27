using System;
using System.Data;

namespace Paclink.Data
{
    public interface IDatabase
    {
        public void CreateDatabase();
        public bool ExistsQuery(string sql);
        public DataSet FillDataSet(string sql, string tableName);
        public string GetSingleValue(string sql);
        public DateTime GetSingleValueDateTime(string sql);
        public long GetSingleValueLong(string sql);
        public void MigrateDatabase(string schemaChanges, Version newVersion);
        public void NonQuery(string sql);
    }
}
