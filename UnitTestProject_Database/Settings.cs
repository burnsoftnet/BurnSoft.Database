using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject_Database
{
    /// <summary>
    /// Class Settings for Unit Tests
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Class AccessDatabase Settings for unit tests
        /// </summary>
        public class AccessDatabase
        {
            /// <summary>
            /// The database name
            /// </summary>
            public const string DatabaseName = "MGC.mdb";
            /// <summary>
            /// The database path
            /// </summary>
            public const string DatabasePath = "C:\\test";
            /// <summary>
            /// The database password
            /// </summary>
            public const string DatabasePassword = "";

        }
        /// <summary>
        /// Class SQLiteDatabase.
        /// </summary>
        public class SQLiteDatabase
        {
            /// <summary>
            /// The database name and path
            /// </summary>
            public const string DatabaseNameAndPath = "C:\\test\\unittest.db";
            /// <summary>
            /// The starter database name and path
            /// </summary>
            public const string StarterDatabaseNameAndPath = "C:\\test\\unittest_full.db";
            /// <summary>
            /// The database version
            /// </summary>
            public const double DBVersion = 1.1;
            /// <summary>
            /// The database version update
            /// </summary>
            public const double DBVersionUpdate = 2.0;
        }

        /// <summary>
        /// Class MySQLDatabase Settings for the MySQl Unit tests
        /// </summary>
        public class MySQLDatabase
        {
            /// <summary>
            /// The hostname
            /// </summary>
            public const string HOSTNAME = "192.168.1.5";
            /// <summary>
            /// The uid
            /// </summary>
            public const string UID = "testuser";
            /// <summary>
            /// The password
            /// </summary>
            public const string PWD = "test.user";
            /// <summary>
            /// The database
            /// </summary>
            public const string Database = "testdb";


        }
        /// <summary>
        /// Class MSSQLDatabase.
        /// </summary>
        public class MSSQLDatabase
        {
            public static string server = "192.168.1.6";
            public static string UID = "test";
            public static string pwd = "test";
            public static string database = "test";
        }
    }
}
