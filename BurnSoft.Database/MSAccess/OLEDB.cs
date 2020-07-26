using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
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
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string FunctionName, Exception e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message for access violations
        /// </summary>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string FunctionName, AccessViolationException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message for invalid cast exception
        /// </summary>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string FunctionName, InvalidCastException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message argument exception
        /// </summary>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string FunctionName, ArgumentException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message for argument null exception.
        /// </summary>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string FunctionName, ArgumentNullException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        #endregion
        //End Snippet

        public static bool AddPasswordToDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                ADODB.Connection conn = DoConnection(path, password,true);
                conn.Open();
                conn.Execute($"ALTER DATABASE PASSWORD {password} NULL", out _);
                conn.Close();
            }
            catch (Exception e)
            {
                errOut = e.Message;
            }
            return bAns;
        }

        private static ADODB.Connection DoConnection(string path, string password = @"", bool runAsAdmin = false)
        {
            ADODB.Connection conn = new ADODB.Connection();
            conn.Provider = "Microsoft.Jet.OLEDB.4.0";
            conn.ConnectionString = $"Data Source={path}";
            conn.Mode = ADODB.ConnectModeEnum.adModeShareExclusive;
            if (password?.Length > 0)
            {
                conn.Properties["Jet OLEDB:Database Password"].Value = password;
            }
            return conn;
        }

        public static bool RemovePasswordToDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                ADODB.Connection conn = DoConnection(path, password,true);
                conn.Open();
                conn.Execute($"ALTER DATABASE PASSWORD NULL {password}", out _);
                conn.Close();
            }
            catch (Exception e)
            {
                errOut = e.Message;
            }
            return bAns;
        }
        public static bool RunSQL(string path,string sql, out string errOut, bool runAsAdmin = false, string password=@"")
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                ADODB.Connection conn = DoConnection(path, password, runAsAdmin);
                conn.Open();
                conn.Execute(sql, out _);
                conn.Close();
            }
            catch (Exception e)
            {
                errOut = e.Message;
            }
            return bAns;
        }
    }
}
