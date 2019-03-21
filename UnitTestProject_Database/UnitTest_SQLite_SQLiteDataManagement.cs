using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.SQLite;
using System.Data;
using System.Diagnostics;
/// <summary>
/// The UnitTestProject_Database namespace.
/// </summary>
namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_SQLite_SQLiteDataManagement
    {
        private string errOut;
        /// <summary>
        /// Tests the initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            bool value = BaseDatabase.CreateStarterDatabase(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, out errOut);
            if (errOut.Length > 0)
            {
                General.HasTrueValue(value, errOut);
            }
        }
        /// <summary>
        /// Defines the test method TestMethod_ConnectDB.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectDB()
        {
            SQLiteDataManagement obj = new SQLiteDataManagement();
            bool value = obj.ConnectDB(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, out errOut);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_RunQuery.
        /// </summary>
        [TestMethod]
        public void TestMethod_RunQuery()
        {
            string sql = "INSERT INTO DB_Version (version) VALUES (0);";
            bool value = SQLiteDataManagement.RunQuery(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, out errOut);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_HasData.
        /// </summary>
        [TestMethod]
        public void TestMethod_HasData()
        {
            string sql = "select * from DB_Version;";
            bool value = SQLiteDataManagement.HasData(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, out errOut);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_GetDataBySQL.
        /// </summary>
        [TestMethod]
        public void TestMethod_GetDataBySQL()
        {
            string sql = "select * from DB_Version;";
            DataTable dt = SQLiteDataManagement.GetDataBySQL(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, out errOut);
            bool HasData = false;
            if (errOut.Length == 0)
            {
                HasData = (dt.Rows.Count > 0);
                foreach (DataRow dr in dt.Rows)
                {
                    Debug.Print("{0}", dr["id"].ToString());
                    Debug.Print("{0}", dr["version"].ToString());
                    Debug.Print("{0}", dr["dt"].ToString());
                    Debug.Print("");
                }
            }
            General.HasTrueValue(HasData, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_CleanDB.
        /// </summary>
        [TestMethod]
        public void TestMethod_CleanDB()
        {
            bool value = SQLiteDataManagement.CleanDB(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, out errOut);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_GetSingleValueFromDatabase_String.
        /// </summary>
        [TestMethod]
        public void TestMethod_GetSingleValueFromDatabase_String()
        {
            string sql = "select * from DB_Version order by dt desc limit 1;";
            string value = SQLiteDataManagement.GetSingleValueFromDatabase(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, "dt", "", out errOut);
            General.HasValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_GetSingleValueFromDatabase_integer.
        /// </summary>
        [TestMethod]
        public void TestMethod_GetSingleValueFromDatabase_integer()
        {
            string sql = "select * from DB_Version order by dt desc limit 1;";
            int value = SQLiteDataManagement.GetSingleValueFromDatabase(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, "id", 0, out errOut);
            bool HasValue = (value > 0);
            Debug.Print("Value Returned is: {0}", value);
            General.HasTrueValue(HasValue, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_GetSingleValueFromDatabase_Double.
        /// </summary>
        [TestMethod]
        public void TestMethod_GetSingleValueFromDatabase_Double()
        {
            string sql = "select * from DB_Version order by dt desc limit 1;";
            double value = SQLiteDataManagement.GetSingleValueFromDatabase(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, "version", 0.0, out errOut);
            bool HasValue = (value > 0);
            Debug.Print("Value Returned is: {0}", value);
            General.HasTrueValue(HasValue, errOut);
        }
        /*
         [TestMethod]
        public void TestMethod_()
        {
        }
         */
    }
}
