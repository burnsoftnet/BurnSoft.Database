using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MSSQL;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MSSQL_MSSQLDatabase
    {
        /// <summary>
        /// The error out
        /// </summary>
        private string errOut;
        /// <summary>
        /// Defines the test method TestMethod_ConnectionString.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string connString = MssqlDatabase.ConnectionString(Settings.MSSQLDatabase.server, "", Settings.MSSQLDatabase.database, Settings.MSSQLDatabase.UID, Settings.MSSQLDatabase.pwd);
            General.HasValue(connString);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringInstance.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionStringInstance()
        {
            string connString = MssqlDatabase.ConnectionString(Settings.MSSQLDatabase.server, "testinstance", Settings.MSSQLDatabase.database, Settings.MSSQLDatabase.UID, Settings.MSSQLDatabase.pwd);
            General.HasValue(connString);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectToDb.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectToDb()
        {
            MssqlDatabase obj = new MssqlDatabase();
            string connString = MssqlDatabase.ConnectionString(Settings.MSSQLDatabase.server, "", Settings.MSSQLDatabase.database, Settings.MSSQLDatabase.UID, Settings.MSSQLDatabase.pwd);
            bool value = obj.ConnectToDb(connString, out errOut);
            obj.Close();
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_RunExec.
        /// </summary>
        [TestMethod]
        public void TestMethod_RunExec()
        {
            string connString = MssqlDatabase.ConnectionString(Settings.MSSQLDatabase.server, "", Settings.MSSQLDatabase.database, Settings.MSSQLDatabase.UID, Settings.MSSQLDatabase.pwd);
            string SQL = "UPDATE test set value=1;";
            bool value = MssqlDatabase.RunExec(connString, SQL,out errOut);
            General.HasTrueValue(value, errOut);
        }
    }
}
