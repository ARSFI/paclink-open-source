using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paclink.Data;

namespace DataAccessTests
{
    [TestClass]
    public class FunctionalTests
    {
        private IDatabase _db;
        private Properties _properties;

        [TestInitialize]
        public void Init()
        {
            _db = new SQLiteDatabase();
            _properties=new Properties(_db);
        }

        [TestMethod]
        public void CreateDatabaseTest()
        {
            _db.CreateDatabase();
        }

        [TestMethod]
        public void FillDataSetTest()
        {
            var ds = _db.FillDataSet("SELECT * FROM Properties", "Properties");
            Console.WriteLine($"Record count: {ds.Tables[0].Rows.Count}");
        }

        [TestMethod]
        public void GetPropertyTest()
        {
            var p = _properties.GetProperty("Schema Version", "");
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetPropertyBoolTest()
        {
            var p = _properties.GetProperty("Some Bool Prop", false);
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetPropertyIntTest()
        {
            var p = _properties.GetProperty("Some Other Prop", 0);
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetSingleValueTest()
        {
            var p = _db.GetSingleValue("SELECT Property FROM Properties LIMIT 1");
            Console.WriteLine($"Property from Properties table: {p}");
        }

        [TestMethod]
        public void GetSingleValueDateTimeTest()
        {
            var dt = _db.GetSingleValueDateTime("SELECT Timestamp FROM Properties LIMIT 1");
            Console.WriteLine($"Timestamp from Properties table: {dt:O}");
        }

        [TestMethod]
        public void GetSingleValueLongTest()
        {
            var count = _db.GetSingleValueLong("SELECT COUNT(*) FROM Properties");
            Console.WriteLine($"Properties table has {count} rows");
        }

    }
}
