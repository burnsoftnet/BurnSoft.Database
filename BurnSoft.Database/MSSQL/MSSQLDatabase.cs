
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
using System.Data.SqlClient;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression

namespace BurnSoft.Database.MSSQL
{
    /// <summary>
    /// Class MSSQLDatabase handles connections and data for Microsoft SQL Server based database.
    /// </summary>
    public class MssqlDatabase
    {
        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MSSQL.MSSQLDatabase";
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
            string sAns;
            string ending = $"Initial Catalog={database}; Integrated Security=false; Pooling=false;UID={uid};PWD={pwd}";
            if (instance?.Length > 0)
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
                MssqlDatabase obj = new MssqlDatabase();
                bAns = obj.ConnectToDb(connString, out errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage( "ConnectToDatabase", e);
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
        /// MSSQLDatabase obj = new MSSQLDatabase();<br/>
        /// string connString = MSSQLDatabase.ConnectionString("192.168.1.6", "", "test", "test", "test");<br/>
        /// bool value = obj.ConnectToDb(connString, out errOut);<br/>
        /// obj.Close();<br/>
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
                errOut = ErrorMessage("ConnectToDb", e);
            }
            return bAns;
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSSQL_MSSQLDatabase <br/>
        /// <br/>
        /// MSSQLDatabase obj = new MSSQLDatabase();<br/>
        /// string connString = MSSQLDatabase.ConnectionString("192.168.1.6", "", "test", "test", "test");<br/>
        /// bool value = obj.ConnectToDb(connString, out errOut);<br/>
        /// obj.Close();<br/>
        /// </example>
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
        /// string connString = MSSQLDatabase.ConnectionString("192.168.1.6", "", "test", "test", "test");<br/>
        /// string SQL = "UPDATE test set value=1;";<br/>
        /// bool value = MSSQLDatabase.RunExec(connString, SQL, out errOut);<br/>
        /// </example>
        public static bool RunExec(string connString, string sql, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                MssqlDatabase obj = new MssqlDatabase();
                
                if (obj.ConnectToDb(connString, out errOut))
                {
                    SqlCommand cmd = new SqlCommand()
                    {
                        CommandText = sql,
                        Connection = obj.Conn
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                } else
                {
                    throw new Exception(errOut);
                }

                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage( "RunExec", e);
            }
            return bAns;
        }
    }
}
