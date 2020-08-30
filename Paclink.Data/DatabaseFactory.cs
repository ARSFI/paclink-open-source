using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Paclink.Data
{
    public static class DatabaseFactory
    {
        // Assumption: SQLite creates the database in "serialized" mode, which allows multithreaded
        // access without any restrictions.
        private static ThreadLocal<IDatabase> _database = new ThreadLocal<IDatabase>(() =>
        {
            return new SQLiteDatabase();
        });

        public static IDatabase Get()
        {
            return _database.Value;
        }
    }
}
