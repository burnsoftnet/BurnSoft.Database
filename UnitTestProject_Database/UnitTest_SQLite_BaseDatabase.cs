using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.SQLite;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_SQLite_BaseDatabase
    {
        private string errOut;
        /// <summary>
        /// Defines the test method TestMethod_ConnectionString.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string value = BaseDatabase.ConnectionString(Settings.SQLiteDatabase.DatabaseNameAndPath);
            General.HasValue(value);
        }
        /// <summary>
        /// Defines the test method TestMethod_CreateDb.
        /// </summary>
        [TestMethod]
        public void TestMethod_CreateDb()
        {
            bool value = BaseDatabase.CreateDB(Settings.SQLiteDatabase.DatabaseNameAndPath, out errOut);
            General.HasTrueValue(value, errOut);
        }
    }
}
