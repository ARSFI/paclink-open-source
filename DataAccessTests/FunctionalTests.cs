using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paclink.Data;

namespace DataAccessTests
{
    [TestClass]
    public class FunctionalTests
    {
        private IDatabase _db;
        private IProperties _properties;

        [TestInitialize]
        public void TestInitialize()
        {
            _db = DatabaseFactory.Get();
            _properties = new Properties(_db);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //_properties.DeleteGroup("");
            _properties.Delete("", "Some Bool Prop");
            _properties.Delete("", "Some Other Prop");
        }

        [TestMethod]
        public void CreateDatabaseTest()
        {
            _db.CreateDatabase();
        }

        [TestMethod]
        public void ExistsQueryTest()
        {
            var exists = _db.ExistsQuery("SELECT * FROM Properties LIMIT 1");
            Console.WriteLine($"Record exists: {exists}");
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
            var p = _properties.Get("System", "Schema Version", "");
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetPropertyBoolTest()
        {
            var p = _properties.Get("", "Some Bool Prop", false);
            Console.WriteLine($"Property value: {p}");
        }

        [TestMethod]
        public void GetPropertyIntTest()
        {
            var p = _properties.Get("", "Some Other Prop", 0);
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

        [TestMethod]
        public void ToIniFileFormatTest()
        {
            var s = _properties.ToIniFileFormat();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(s));
            Console.WriteLine(s);
        }

        [TestMethod]
        public void FromIniFileFormatTest()
        {
            var testEntry = DateTime.Now.ToString();
            _properties.FromIniFileFormat($"[Test]\r\nTestEntry1={testEntry}\r\nTestEntry2={testEntry}");
            var s = _properties.Get("Test", "TestEntry1", "");
            Assert.IsTrue(testEntry == s, $"{s} should be the same as {testEntry}");
            Console.WriteLine($"{s} should be the same as {testEntry}");
        }

    }
}
