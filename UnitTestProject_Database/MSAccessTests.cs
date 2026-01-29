using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.MSAccess;
using System.Data;

namespace UnitTestProject_Database
{
    [TestClass]
    public class MSAccessTests
    {
        private string errOut;
        private string ConnString;
        private string _dbPath;
        private string _dbName;
        private string _dbPassword;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbPath = Settings.AccessDatabase.DatabasePath;
            _dbName = Settings.AccessDatabase.DatabaseName;
            _dbPassword = Settings.AccessDatabase.DatabasePassword;
            ConnString = MsAccessDatabase.ConnectionString(_dbPath, _dbName, out errOut, _dbPassword);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringWithPassword.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_ConnectionStringWithPassword()
        {
            string value = BurnSoft.Database.MSAccess.MsAccessDatabase.ConnectionString(_dbPath, _dbName, out errOut, _dbPassword);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionStringWithOutPassword.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_ConnectionStringWithOutPassword()
        {
            string value = MsAccessDatabase.ConnectionString(_dbPath, _dbName, out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionOLEStringWithPassword.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_ConnectionOLEStringWithPassword()
        {
            string value = MsAccessDatabase.ConnectionStringOle(_dbPath, _dbName, out errOut, _dbPassword);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectionOLEStringWithOutPassword.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_ConnectionOLEStringWithOutPassword()
        {
            string value = MsAccessDatabase.ConnectionStringOle(_dbPath, _dbName, out errOut);
            General.HasValue(value, errOut);
        }

        /// <summary>
        /// Defines the test method TestMethod_ConnectDB.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_ConnectDB()
        {
            MsAccessDatabase obj = new MsAccessDatabase();
            bool value = obj.ConnectDb(ConnString, out errOut);
            obj.Close(out errOut);
            General.HasTrueValue(value, errOut);

        }
        /// <summary>
        /// Defines the test method TestMethod_ConnExec.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_ConnExec()
        {
            string SQL = "INSERT INTO Gun_Cal(Cal) VALUES('TEST');";
            MsAccessDatabase obj = new MsAccessDatabase();
            bool value = obj.ConnExec(ConnString, SQL, out errOut);
            General.HasTrueValue(value, errOut);
        }

        /// <summary>
        /// Defines the test method TestMethod_GetData.
        /// </summary>
        [TestMethod, TestCategory("MSAccess")]
        public void TestMethod_GetData()
        {
            String SQL = "Select * from Gun_Cal";
            MsAccessDatabase obj = new MsAccessDatabase();
            DataTable table = obj.GetData(ConnString, SQL, out errOut);
            string TestValue = @"";
            foreach(DataRow row in table.Rows)
            {
                TestValue += String.Format("{0}{1}",row["Cal"].ToString(),Environment.NewLine);
            }

            General.HasValue(TestValue, errOut);
        }
    }
}
