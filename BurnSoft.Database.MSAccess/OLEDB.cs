using System;
using ADODB;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace BurnSoft.Database.MSAccess
{
    /// <summary>
    /// Class OLEDB.
    /// </summary>
    public class Oledb
    {
        //TODO #9 Add code code to access oledb database for access databases

        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MSAccess.";
        /// <summary>
        /// Errors the message for regular Exceptions
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, Exception e) => $"{ClassLocation}.{functionName} - {e.Message}";
        /// <summary>
        /// Errors the message for access violations
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, AccessViolationException e) => $"{ClassLocation}.{functionName} - {e.Message}";

        /// <summary>
        /// Errors the message argument exception
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, ArgumentException e) => $"{ClassLocation}.{functionName} - {e.Message}";
        /// <summary>
        /// Errors the message for argument null exception.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, ArgumentNullException e) => $"{ClassLocation}.{functionName} - {e.Message}";
        #endregion
        //End Snippet      
        #region "Database Security"
        /// <summary>
        /// Removes the password from the access database.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="password">The password.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool RemovePasswordFromDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Connection conn = DoConnection(path, password, true);
                conn.Open();
                conn.Execute($"ALTER DATABASE PASSWORD NULL {password}", out _);
                conn.Close();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("RemovePasswordFromDatabase", e);
            }
            return bAns;
        }
        /// <summary>
        /// Add a Password to the MSAccess database to prevent people from just opening up the database in access
        /// and looking through the database
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="password">The password.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool AddPasswordToDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Connection conn = DoConnection(path, password, true);
                conn.Open();
                conn.Execute($"ALTER DATABASE PASSWORD {password} NULL", out _);
                conn.Close();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("AddPasswordToDatabase", e);
            }
            return bAns;
        }
        #endregion
        #region "Private Functions"
        /// <summary>
        /// Does the connection to the Access database
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="password">The password to access the database is this database is password protected</param>
        /// <param name="runAsAdmin">if set to <c>true</c> [run as admin].</param>
        /// <returns>Connection.</returns>
        private static Connection DoConnection(string path, string password = @"", bool runAsAdmin = false)
        {
            Connection conn = new Connection
            {
                Provider = "Microsoft.Jet.OLEDB.4.0",
                ConnectionString = $"Data Source={path}"
            };
            if (runAsAdmin)
            {
                conn.Mode = ConnectModeEnum.adModeShareExclusive;
            }

            if (password?.Length > 0)
            {
                conn.Properties["Jet OLEDB:Database Password"].Value = password;
            }
            return conn;
        }
        #endregion
        #region "Run Quereies"
        /// <summary>
        /// Runs the T-SQL statement against the Access Database.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <param name="runAsAdmin">if set to <c>true</c> [run as admin].</param>
        /// <param name="password">The password to access the database is this database is password protected.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool RunSql(string path, string sql, out string errOut, bool runAsAdmin = false, string password = @"")
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Connection conn = DoConnection(path, password, runAsAdmin);
                conn.Open();
                conn.Execute(sql, out _);
                conn.Close();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("RunSQL", e);
            }
            return bAns;
        }
        #endregion
        #region "Adding to Database"        
        /// <summary>
        /// Adds the column to the access database
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <param name="table">The table.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="type">The type.</param>
        /// <param name="errOut">The error out.</param>
        /// <param name="password">The password to access the database is this database is password protected.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool AddColumn(string path, string name, string table, string defaultValue, string type, out string errOut, string password=@"")
        {
            string myDefault = $"[\"{defaultValue}\"]";
            string sql = $"ALTER TABLE {table} ADD COLUMN {name} {type}";
            if (defaultValue?.Length > 0)
            {
                sql += $" [\"{defaultValue}\"];";
            }
            else
            {
                sql += ";";
            }
            return RunSql(path, sql, out errOut, false, password);
        }
        /// <summary>
        /// Creates the a view in the Access Database.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="name">The name.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <param name="password">The password to access the database is this database is password protected.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CreateView(string path, string name, string sql, out string errOut, string password = @"")
        {
            string mySql = $"CREATE VIEW {name} AS {sql}";
            return RunSql(path, mySql, out errOut, false, password);
        }

        #endregion

    }
}
