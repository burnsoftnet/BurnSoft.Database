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

            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "DBVersionExists", e);
            }
            return bAns;
        }
    }
}
