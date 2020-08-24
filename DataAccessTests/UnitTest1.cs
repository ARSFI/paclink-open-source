using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paclink.Data;

namespace DataAccessTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine(DataAccess.GetVersion());
        }
    }
}
