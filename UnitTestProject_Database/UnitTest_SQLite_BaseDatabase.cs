using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BurnSoft.Database.SQLite;
using System.Diagnostics;

namespace UnitTestProject_Database
{
    [TestClass]
    public class UnitTest_SQLite_BaseDatabase
    {
        private string errOut;
        /// <summary>
        /// Defines the test method TestMethod_ConnectionString.
        /// </summary>
        [TestMethod]
        public void TestMethod_ConnectionString()
        {
            string value = BaseDatabase.ConnectionString(Settings.SQLiteDatabase.DatabaseNameAndPath);
            General.HasValue(value);
        }
        /// <summary>
        /// Defines the test method TestMethod_CreateDb.
        /// </summary>
        [TestMethod]
        public void TestMethod_CreateDb()
        {
            bool value = BaseDatabase.CreateDB(Settings.SQLiteDatabase.DatabaseNameAndPath, out errOut);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_CreateDatabaseVersion.
        /// </summary>
        [TestMethod]
        public void TestMethod_CreateDatabaseVersion()
        {
            bool value = BaseDatabase.CreateDatabaseVersion(Settings.SQLiteDatabase.DatabaseNameAndPath, out errOut, Settings.SQLiteDatabase.DBVersion);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_DBVersionExists.
        /// </summary>
        [TestMethod]
        public void TestMethod_DBVersionExists()
        {
            bool value = BaseDatabase.DBVersionExists(Settings.SQLiteDatabase.DatabaseNameAndPath, Settings.SQLiteDatabase.DBVersion, out errOut);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_GetDatabaseVersion.
        /// </summary>
        [TestMethod]
        public void TestMethod_GetDatabaseVersion()
        {
            bool DidSetVersion = BaseDatabase.UpdateDbVersion(Settings.SQLiteDatabase.DatabaseNameAndPath, Settings.SQLiteDatabase.DBVersion, out errOut);
            double value = BaseDatabase.GetDatabaseVersion(Settings.SQLiteDatabase.DatabaseNameAndPath, out errOut);
            bool isExpected = (value == Settings.SQLiteDatabase.DBVersion);
            Debug.Print("This Database Version is: {0}", value);
            General.HasTrueValue(isExpected, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_UpdateDbVersion.
        /// </summary>
        [TestMethod]
        public void TestMethod_UpdateDbVersion()
        {
            bool value = BaseDatabase.UpdateDbVersion(Settings.SQLiteDatabase.DatabaseNameAndPath, Settings.SQLiteDatabase.DBVersionUpdate, out errOut);
            double newVersion = BaseDatabase.GetDatabaseVersion(Settings.SQLiteDatabase.DatabaseNameAndPath, out errOut);
            Debug.Print("This Database Version is: {0}", value);
            General.HasTrueValue(value, errOut);
        }
        /// <summary>
        /// Defines the test method TestMethod_CreateStarterDatabase.
        /// </summary>
        [TestMethod]
        public void TestMethod_CreateStarterDatabase()
        {
            bool value = BaseDatabase.CreateStarterDatabase(Settings.SQLiteDatabase.StarterDatabaseNameAndPath, out errOut);
            General.HasTrueValue(value, errOut);
        }
        
    }
}
