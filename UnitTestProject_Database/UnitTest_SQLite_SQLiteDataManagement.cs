using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.SQLite;
using System.Data;
using System.Diagnostics;

namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_SQLite_SQLiteDataManagement
    {
        private string errOut;
        [TestInitialize]
        public void TestInitialize()
        {
            bool value = BaseDatabase.CreateStarterDatabase(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, out errOut);
            if (errOut.Length > 0)
            {
                General.HasTrueValue(value, errOut);
            }
        }

        [TestMethod]
        public void TestMethod_ConnectDB()
        {
            SQLiteDataManagement obj = new SQLiteDataManagement();
            bool value = obj.ConnectDB(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, out errOut);
            General.HasTrueValue(value, errOut);
        }

        [TestMethod]
        public void TestMethod_RunQuery()
        {
            string sql = "INSERT INTO DB_Version (version) VALUES (0);";
            bool value = SQLiteDataManagement.RunQuery(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, out errOut);
            General.HasTrueValue(value, errOut);
        }

        [TestMethod]
        public void TestMethod_HasData()
        {
            string sql = "select * DB_Version;";
            bool value = SQLiteDataManagement.HasData(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, sql, out errOut);
            General.HasTrueValue(value, errOut);
        }

        [TestMethod]
        public void TestMethod_GetDataBySQL()
        {
            string sql = "select * DB_Version;";
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
                }
            }
            General.HasTrueValue(HasData, errOut);
        }

        [TestMethod]
        public void TestMethod_()
        {
        }
        /*
         [TestMethod]
        public void TestMethod_()
        {
        }
         */
    }
}
