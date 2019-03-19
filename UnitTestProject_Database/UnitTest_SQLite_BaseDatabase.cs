using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.SQLite;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_SQLite_BaseDatabase
    {
        /// <summary>
        /// Defines the test method TestMethod_ConnectionString.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string value = BaseDatabase.ConnectionString(Settings.SQLiteDatabase.DatabaseNameAndPath);
            General.HasValue(value);
        }
    }
}
