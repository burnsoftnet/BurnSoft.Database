using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MySQL;
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MySQL_MySQLDatabase
    {
        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        /// <value>The test context.</value>
        public TestContext TestContext { get; set; }
        /// <summary>
        /// The error out
        /// </summary>
        private string _errOut;
        /// <summary>
        /// Defines the test method TestMethod_ConnectionString.
        /// </summary>
        [TestMethod, TestCategory("MySQL")]
        public void TestMethod_ConnectionString()
        {
            string value = MySqlDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out _errOut);
            General.HasValue(value, _errOut);
        }
        [TestMethod, TestCategory("MySQL")]
        public void ConnectionStringWebTest()
        {
            string connectionsExmaple = "server=serverName;database=Northwind;persistsecurityinfo=True;User ID=userName;Password=password";
            string value = MySqlDatabase.ConnectionString(connectionsExmaple, out _);
            General.HasValue(value, _errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectDB.
        /// </summary>
        [TestMethod, TestCategory("MySQL")]
        public void TestMethod_ConnectDB()
        {
            MySqlDatabase obj = new MySqlDatabase();
            string connString = MySqlDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out _errOut);
            bool value = obj.ConnectDb(connString, out _errOut);
            obj.Close();
            General.HasTrueValue(value, _errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_RunQuery.
        /// </summary>
        [TestMethod, TestCategory("MySQL")]
        public void TestMethod_RunQuery()
        {
            string sql = "CREATE TABLE `DB_Version` (`ID` int(11) NOT NULL AUTO_INCREMENT,`verNo` varchar(45) DEFAULT NULL,`dtUpdated` timestamp NULL DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(`ID`),  UNIQUE KEY `ID_UNIQUE` (`ID`)) ENGINE = MyISAM AUTO_INCREMENT = 5 DEFAULT CHARSET = latin1;";
            string connString = MySqlDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out _errOut);
            bool value = MySqlDatabase.RunQuery(connString, sql, out _errOut);
            General.HasTrueValue(value, _errOut);
        }
        /// <summary>
        /// Defines the test method ValueExistsTests.
        /// </summary>
        [TestMethod, TestCategory("MySQL")]
        public void ValueExistsTests()
        {
            string sql = "select * from `DB_Version`";
            string connString = MySqlDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out _errOut);
            bool value = MySqlDatabase.ValueExists(connString, sql, out _errOut);
            General.HasTrueValue(value, _errOut);
        }
        /// <summary>
        /// Defines the test method GetDataSetTest.
        /// </summary>
        [TestMethod, TestCategory("MySQL - Get Data")]
        public void GetDataSetTest()
        {
            string sql = "select * from `DB_Version`";
            string connString = MySqlDatabase.ConnectionString(Settings.MySQLDatabase.HOSTNAME, Settings.MySQLDatabase.UID, Settings.MySQLDatabase.PWD, Settings.MySQLDatabase.Database, out _errOut);
            DataSet value = MySqlDatabase.GetData(connString, sql, "dbversion", out _errOut);
            bool didPass = false;

            foreach (DataTable table in value.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    TestContext.WriteLine($"Version #: {row["VerNo"]}");
                    TestContext.WriteLine($"Date Created: {row["dtUpdated"]}");
                    didPass = true;
                }
            }

            General.HasTrueValue(didPass, _errOut);
        }
        /*
        [TestMethod]
        public void TestMethod_()
        {
        }
        */
    }
}
