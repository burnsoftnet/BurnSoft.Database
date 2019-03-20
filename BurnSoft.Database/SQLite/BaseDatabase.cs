using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace BurnSoft.Database.SQLite
{
    /// <summary>
    /// Class BaseDatabase is used for creating the base database if it doesn't exist and the ability to put in the versioning of the
    /// database in ia table or update the versioning
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
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, Exception e) => "{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, InvalidCastException e) => "{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, ArgumentNullException e) => "{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        #endregion

        /// <summary>
        /// Connections the string.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <returns>System.String.</returns>
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
        public static bool CreateDB(string dbName, out string errOut)
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
                errOut = ErrorMessage(ClassLocation, "CreateDB", e);
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
        public static bool DBVersionExists(string dbName, double myVer, out string errOut)
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
                errOut = ErrorMessage(ClassLocation, "DBVersionExists", e);
            }
            return bAns;
        }
        /// <summary>
        /// Updates the database version.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="dbversion">The dbversion.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
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
                errOut = ErrorMessage(ClassLocation, "UpdateDbVersion", e);
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
        private static bool CreateDatabaseVersion(string dbname, out string errOut, double version =1.0)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                string sql = "create table IF NOT EXISTS DB_Version (id integer primary key autoincrement, version DOUBLE DEFAULT 0, dt DATETIME DEFAULT CURRENT_TIMESTAMP);";
                if (SQLiteDataManagement.RunQuery(dbname, sql, out errOut))
                {
                    if (!DBVersionExists(dbname, version, out errOut))
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
                errOut = ErrorMessage(ClassLocation, "CreateDatabaseVersion", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the database version.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.Double.</returns>
        public static double GetDatabaseVersion(string dbname, out string errOut)
        {
            double dAns = 0;
            errOut = @"";
            try
            {
                string sql = "select version from DB_Version order by id desc limit 1;";
                SQLiteDataManagement obj = new SQLiteDataManagement();
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
                errOut = ErrorMessage(ClassLocation, "GetDatabaseVersion", e);
            }
            return dAns;
        }

        /// <summary>
        /// Creates the starter database.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="errOut">The error out.</param>
        /// <param name="dbversion">The dbversion.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception">
        /// </exception>
        public static bool CreateStarterDatabase(string dbname, out string errOut, double dbversion = 1.0)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                bool dbExists = File.Exists(dbname);
                if (!dbExists)
                {
                    if (CreateDB(dbname, out errOut))
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
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "CreateStarterDatabase", e);
            }
            return bAns;
        }
    }
}
