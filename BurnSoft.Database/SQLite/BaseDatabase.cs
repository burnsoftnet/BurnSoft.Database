
/* ------------------------------------------------------------------------------------------------
* 
* BurnSoft
* www.burnsoft.net
* Owenton, Kentucky
* Copyright (C) 2019. All Rights Reserved.
* 
* ------------------------------------------------------------------------------------------------
* Original Designer(s):
*                      Joe M.
* Original Author(s):
*      03/27/2019      Joe M.
*      
* Revision:
* 
* ----------------------------------------------------------------------------------------------- */
using System;
using System.Data.SQLite;
using System.IO;
// ReSharper disable UnusedMember.Local

namespace BurnSoft.Database.SQLite
{
    /// <summary>
    /// Class BaseDatabase is used for creating the base database if it doesn't exist and the ability to put in the version of the
    /// database in ia table or update the versions.
    /// </summary>
    public class BaseDatabase
    {

        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.SQLite.BaseDatabase";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, Exception e) => $"{ClassLocation}.{functionName} - {e.Message}";

        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, InvalidCastException e) => $"{ClassLocation}.{functionName} - {e.Message}";

        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, ArgumentNullException e) => $"{ClassLocation}.{functionName} - {e.Message}";
        #endregion

        /// <summary>
        /// Connections the string.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <returns>System.String.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// string value = BaseDatabase.ConnectionString("C:\\test\\unittest.db");<br/>
        /// <br/>
        /// <b>Results</b><br/>
        /// Data Source=C:\test\unittest.db;Version=3
        /// </example>
        public static string ConnectionString(string dbname)
        {
            return $"Data Source={dbname};Version=3";
        }
        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// bool value = BaseDatabase.CreateDB("C:\\test\\unittest.db", out errOut);
        /// </example>
        public static bool CreateDb(string dbName, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                SQLiteConnection.CreateFile(dbName);
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("CreateDB", e);
            }
            return bAns;
        }
        /// <summary>
        /// Databases the version exists.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="myVer">My ver.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// bool value = BaseDatabase.DBVersionExists("C:\\test\\unittest.db", 1.1, out errOut);
        /// </example>
        public static bool DbVersionExists(string dbName, double myVer, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                string sql = $"SELECT * from DB_Version where version={myVer}";
                bAns = SQLiteDataManagement.HasData(dbName, sql, out errOut);
                if (errOut.Length > 0) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage( "DBVersionExists", e);
            }
            return bAns;
        }
        /// <summary>
        /// Updates the database version.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="dbversion">The database version.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// bool value = BaseDatabase.UpdateDbVersion("C:\\test\\unittest.db", 1.1, out errOut);
        /// </example>
        public static bool UpdateDbVersion(string dbname, double dbversion, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                string sql = $"INSERT INTO DB_Version (version) VALUES ({dbversion});";
                bAns = SQLiteDataManagement.RunQuery(dbname, sql, out errOut);
                if (!bAns) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("UpdateDbVersion", e);
            }
            return bAns;
        }
        /// <summary>
        /// Creates the database version.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="errOut">The error out.</param>
        /// <param name="version">The version.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// bool value = BaseDatabase.CreateDatabaseVersion("C:\\test\\unittest.db", out errOut, 1.2);
        /// </example>
        public static bool CreateDatabaseVersion(string dbname, out string errOut, double version =1.0)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                string sql = "create table IF NOT EXISTS DB_Version (id integer primary key autoincrement, version DOUBLE DEFAULT 0, dt DATETIME DEFAULT CURRENT_TIMESTAMP);";
                if (SQLiteDataManagement.RunQuery(dbname, sql, out errOut))
                {
                    if (!DbVersionExists(dbname, version, out errOut))
                    {
                        if (UpdateDbVersion(dbname, version, out errOut))
                        {
                            bAns = true;
                        } else
                        {
                            throw new Exception(errOut);
                        }
                    } else
                    {
                        bAns = true;
                    }
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("CreateDatabaseVersion", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the database version.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.Double.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// double value = BaseDatabase.GetDatabaseVersion("C:\\test\\unittest.db", out errOut);
        /// </example>
        public static double GetDatabaseVersion(string dbname, out string errOut)
        {
            double dAns = 0;
            errOut = @"";
            try
            {
                string sql = "select version from DB_Version order by id desc limit 1;";
                SQLiteDataManagement obj = new SQLiteDataManagement();
                obj.ConnectDB(dbname, out errOut);
                SQLiteCommand cmd = new SQLiteCommand(sql, obj.ConnObject);
                using (SQLiteDataReader rs = cmd.ExecuteReader())
                {
                    while (rs.Read())
                    {
                        dAns = rs.GetDouble(0);
                    }
                    rs.Close();
                }
                cmd.Dispose();
                obj.CloseDb();
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetDatabaseVersion", e);
            }
            return dAns;
        }

        /// <summary>
        /// Creates the starter database with the table DB_Version set to version 1.0 and the date and time it was created.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="errOut">The error out.</param>
        /// <param name="dbversion">The dbversion.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception">
        /// </exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_BaseDatabase</b><br/>
        /// <br/>
        /// bool value = BaseDatabase.CreateStarterDatabase("C:\\test\\unittest.db", out errOut);
        /// </example>
        public static bool CreateStarterDatabase(string dbname, out string errOut, double dbversion = 1.0)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                bool dbExists = File.Exists(dbname);
                if (!dbExists)
                {
                    if (CreateDb(dbname, out errOut))
                    {
                        if (CreateDatabaseVersion(dbname, out errOut, dbversion))
                        {
                            bAns = true;
                        } else
                        {
                            throw new Exception(errOut);
                        }
                    } else
                    {
                        if (CreateDatabaseVersion(dbname, out errOut))
                        {
                            bAns = true;
                        } else
                        {
                            throw new Exception(errOut);
                        }
                    }
                } else
                {
                    bAns = true;
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("CreateStarterDatabase", e);
            }
            return bAns;
        }
    }
}
