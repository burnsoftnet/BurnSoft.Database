using System;
using ADODB;

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
        private static string ErrorMessage(string functionName, Exception e) => $"{ClassLocation}.{functionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message for access violations
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, AccessViolationException e) => $"{ClassLocation}.{functionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message for invalid cast exception
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, InvalidCastException e) => $"{ClassLocation}.{functionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message argument exception
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string functionName, ArgumentException e) => $"{ClassLocation}.{functionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message for argument null exception.
        /// </summary>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string FunctionName, ArgumentNullException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        #endregion
        //End Snippet        
        /// <summary>
        /// Adds the password to database.
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
                Connection conn = DoConnection(path, password,true);
                conn.Open();
                conn.Execute($"ALTER DATABASE PASSWORD {password} NULL", out _);
                conn.Close();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("AddPasswordToDatabase",e);
            }
            return bAns;
        }

        private static Connection DoConnection(string path, string password = @"", bool runAsAdmin = false)
        {
            Connection conn = new ADODB.Connection();
            conn.Provider = "Microsoft.Jet.OLEDB.4.0";
            conn.ConnectionString = $"Data Source={path}";
            conn.Mode = ConnectModeEnum.adModeShareExclusive;
            if (password?.Length > 0)
            {
                conn.Properties["Jet OLEDB:Database Password"].Value = password;
            }
            return conn;
        }

        public static bool RemovePasswordFromDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Connection conn = DoConnection(path, password,true);
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
        public static bool RunSQL(string path,string sql, out string errOut, bool runAsAdmin = false, string password=@"")
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
    }
}
