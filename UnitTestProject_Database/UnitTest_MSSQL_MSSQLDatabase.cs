using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MSSQL;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MSSQL_MSSQLDatabase
    {
        /// <summary>
        /// Defines the test method TestMethod_ConnectionString.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string connString = MSSQLDatabase.ConnectionString(Settings.MSSQLDatabase.server, "", Settings.MSSQLDatabase.database, Settings.MSSQLDatabase.UID, Settings.MSSQLDatabase.pwd);
            General.HasValue(connString);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringInstance.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionStringInstance()
        {
            string connString = MSSQLDatabase.ConnectionString(Settings.MSSQLDatabase.server, "testinstance", Settings.MSSQLDatabase.database, Settings.MSSQLDatabase.UID, Settings.MSSQLDatabase.pwd);
            General.HasValue(connString);
        }
    }
}
