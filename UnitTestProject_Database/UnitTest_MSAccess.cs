using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MSAccess;

namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MSAccess
    {
        private string errOut;
        private string ConnString;

        [TestInitialize]
        public void TestInitialize()
        {
            ConnString = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionString(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, out errOut, Settings.AccessDatabase.DatabasePassword);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringWithPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionStringWithPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionString(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, out errOut, Settings.AccessDatabase.DatabasePassword);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringWithOutPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionStringWithOutPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionString(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionOLEStringWithPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionOLEStringWithPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionStringOLE(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, out errOut, Settings.AccessDatabase.DatabasePassword);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionOLEStringWithOutPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionOLEStringWithOutPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionStringOLE(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, out errOut);
            General.HasValue(value, errOut);
        }

        /// <summary>
        /// Defines the test method TestMethod_ConnectDB.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectDB()
        {
            BurnSoft.Database.MSAccess.MSAccessDatabase obj = new BurnSoft.Database.MSAccess.MSAccessDatabase();
            bool value = obj.ConnectDB(ConnString, out errOut);
            obj.Close(out errOut);
            General.HasTrueValue(value, errOut);

        }
        /// <summary>
        /// Defines the test method TestMethod_ConnExec.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnExec()
        {
            string SQL = "INSERT INTO Gun_Cal(Cal) VALUES('TEST');";
            BurnSoft.Database.MSAccess.MSAccessDatabase obj = new BurnSoft.Database.MSAccess.MSAccessDatabase();
            bool value = obj.ConnExec(ConnString, SQL, out errOut);
            General.HasTrueValue(value, errOut);
        }
    }
}
