﻿using System;
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
        
        private static string ConnectionString(string dbname)
        {
            return $"Data Source={dbname};Version=3";
        }

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
