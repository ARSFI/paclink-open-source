using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paclink.Data;

namespace DataAccessTests
{
    [TestClass]
    public class FunctionalTests
    {
        [TestMethod]
        public void CreateDatabaseTest()
        {
            DatabaseUtils.CreateDatabase();
        }

        [TestMethod]
        public void FillDataSetTest()
        {
            var ds = DatabaseUtils.FillDataSet("SELECT * FROM Properties", "Properties");
            Console.WriteLine($"Record count: {ds.Tables[0].Rows.Count}");
        }

        [TestMethod]
        public void GetPropertyTest()
        {
            var p = Properties.GetProperty(DatabaseUtils.SchemaVersionProperty, "");
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetPropertyBoolTest()
        {
            var p = Properties.GetProperty("Some Bool Prop", false);
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetPropertyIntTest()
        {
            var p = Properties.GetProperty("Some Other Prop", 0);
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetSchemaVersionTest()
        {
            var v = DatabaseUtils.GetSchemaVersion();
            Console.WriteLine($"Database version: {v}");
        }

        [TestMethod]
        public void GetSingleValueTest()
        {
            var p = DatabaseUtils.GetSingleValue("SELECT Property FROM Properties LIMIT 1");
            Console.WriteLine($"Property from Properties table: {p}");
        }

        [TestMethod]
        public void GetSingleValueDateTimeTest()
        {
            var dt = DatabaseUtils.GetSingleValueDateTime("SELECT Timestamp FROM Properties LIMIT 1");
            Console.WriteLine($"Timestamp from Properties table: {dt:O}");
        }

        [TestMethod]
        public void GetSingleValueLongTest()
        {
            var count = DatabaseUtils.GetSingleValueLong("SELECT COUNT(*) FROM Properties");
            Console.WriteLine($"Properties table has {count} rows");
        }

    }
}
