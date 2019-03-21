using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.SQLite;

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
        /*
         [TestMethod]
        public void TestMethod_()
        {
        }
         */
    }
}
