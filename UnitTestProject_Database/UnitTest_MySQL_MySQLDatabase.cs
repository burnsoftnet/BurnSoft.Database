using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MySQL;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MySQL_MySQLDatabase
    {
        /// <summary>
        /// The error out
        /// </summary>
        private string errOut;
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string value = MySQLDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectDB.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectDB()
        {
            MySQLDatabase obj = new MySQLDatabase();
            string connString = MySQLDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out errOut);
            bool value = obj.ConnectDB(connString, out errOut);
            obj.Close();
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_RunQuery.
        /// </summary>
        [TestMethod]
        public void TestMethod_RunQuery()
        {
            string sql = "CREATE TABLE `DB_Version` (`ID` int(11) NOT NULL AUTO_INCREMENT,`verNo` varchar(45) DEFAULT NULL,`dtUpdated` timestamp NULL DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(`ID`),  UNIQUE KEY `ID_UNIQUE` (`ID`)) ENGINE = MyISAM AUTO_INCREMENT = 5 DEFAULT CHARSET = latin1;";
            string connString = MySQLDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out errOut);
            bool value = MySQLDatabase.RunQuery(connString, sql, out errOut);
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
