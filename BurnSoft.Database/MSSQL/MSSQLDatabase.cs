using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BurnSoft.Database.MSSQL
{
    /// <summary>
    /// Class MSSQLDatabase handles connections and data for Microsoft SQL Server based database.
    /// </summary>
    public class MSSQLDatabase
    {
        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MSSQL.MSSQLDatabase";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, Exception e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, AccessViolationException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, InvalidCastException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, ArgumentException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, ArgumentNullException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        #endregion

        /// <summary>
        /// The SQL connection Object.
        /// </summary>
        public SqlConnection Conn;

        /// <summary>
        /// Connections the string.
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="database">The database.</param>
        /// <param name="uid">The uid.</param>
        /// <param name="pwd">The password.</param>
        /// <returns>System.String.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSSQL_MSSQLDatabase <br/>
        /// <br/>
        /// string connString = MSSQLDatabase.ConnectionString("192.168.1.6", "", "test", "test", "test");<br/>
        /// <br/>
        /// <b>Result</b><br/>
        /// Data Source=192.168.1.6;Initial Catalog=test; Integrated Security=false; Pooling=false;UID=test;PWD=test<br/>
        /// 
        /// </example>
        public static string ConnectionString(string hostname, string instance, string database, string uid, string pwd)
        {
            string sAns = @"";
            string ending = $"Initial Catalog={database}; Integrated Security=false; Pooling=false;UID={uid};PWD={pwd}";
            if (instance.Length > 0)
            {
                sAns = $"Data Source={hostname}\\{instance};{ending}";
            } else
            {
                sAns = $"Data Source={hostname};{ending}";
            }
            return sAns;
        }

        /// <summary>
        /// Converts to database.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool ConnectToDatabase(string connString, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                MSSQLDatabase obj = new MSSQLDatabase();
                bAns = obj.ConnectToDb(connString, out errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "ConnectToDatabase", e);
            }
            return bAns;
        }

        /// <summary>
        /// Converts to db.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSSQL_MSSQLDatabase <br/>
        /// <br/>
        /// </example>
        public bool ConnectToDb(string connString, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Conn = new SqlConnection(connString);
                Conn.Open();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "ConnectToDb", e);
            }
            return bAns;
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            if (Conn.State != System.Data.ConnectionState.Closed)
            {
                while (Conn.State != System.Data.ConnectionState.Closed)
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
            Conn = null;
        }

        /// <summary>
        /// Runs and executes the SQL statement.
        /// </summary>
        /// <param name="connString">The connection string.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSSQL_MSSQLDatabase <br/>
        /// <br/>
        /// </example>
        public static bool RunExec(string connString, string sql, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                MSSQLDatabase obj = new MSSQLDatabase();
                
                if (obj.ConnectToDb(connString, out errOut))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = sql,
                        Connection = obj.Conn
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd = null;
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "RunExec", e);
            }
            return bAns;
        }
    }
}
