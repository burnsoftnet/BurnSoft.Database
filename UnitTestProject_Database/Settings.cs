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
            public const string StarterDatabaseNameAndPath = "C:\\test\\unittest.db";
            /// <summary>
            /// The database version
            /// </summary>
            public const double DBVersion = 1.1;
            /// <summary>
            /// The database version update
            /// </summary>
            public const double DBVersionUpdate = 2.0;
        }
    }
}
