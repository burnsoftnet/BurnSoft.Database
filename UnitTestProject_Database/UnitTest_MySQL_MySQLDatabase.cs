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
           string value = MySQLDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD)
        }

        /*
        [TestMethod]
        public void TestMethod_()
        {
        }
        */
    }
}
