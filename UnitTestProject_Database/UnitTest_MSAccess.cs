using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database;

namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_MSAccess
    {
        private string errOut;
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringWithPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionStringWithPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionString(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, Settings.AccessDatabase.DatabasePassword, out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringWithOutPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionStringWithOutPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionString(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName,"", out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionOLEStringWithPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionOLEStringWithPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionStringOLE(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, Settings.AccessDatabase.DatabasePassword, out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionOLEStringWithOutPassword.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionOLEStringWithOutPassword()
        {
            string value = BurnSoft.Database.MSAccess.MSAccessDatabase.ConnectionStringOLE(Settings.AccessDatabase.DatabasePath, Settings.AccessDatabase.DatabaseName, "", out errOut);
            General.HasValue(value, errOut);
        }
    }
}
