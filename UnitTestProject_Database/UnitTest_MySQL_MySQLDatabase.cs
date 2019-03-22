using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MySQL;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MySQL_MySQLDatabase
    {
        private string errOut;
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string value = MySQLDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out errOut);
            General.HasValue(value, errOut);
        }

        [TestMethod]
        public void TestMethod_ConnectDB()
        {
            MySQLDatabase obj = new MySQLDatabase();
            string connString = MySQLDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out errOut);
            bool value = obj.ConnectDB(connString, out errOut);
            obj.Close();
            General.HasTrueValue(value, errOut);
        }
        /*
        [TestMethod]
        public void TestMethod_()
        {
        }
        */
    }
}
