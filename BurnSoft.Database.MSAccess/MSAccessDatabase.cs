
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
using System.Data.Odbc;
using System.Data;
// ReSharper disable UnusedMember.Local

namespace BurnSoft.Database.MSAccess
{
    /// <summary>
    /// Class MSAccessDatabase, Helps connect and manage MS Access Databases
    /// </summary>
    public class MsAccessDatabase
    {
        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MSAccess.MSAccessDatabase";
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
        #region "Class Vars"        
        /// <summary>
        /// The connection
        /// </summary>
        OdbcConnection _conn;
        #endregion
        #region "Connection Strings"

        /// <summary>
        /// Connection String Format Used to Connect to MS Access Databases using the Microsoft Access Driver
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="databaseName"></param>
        /// <param name="errOut"></param>
        /// <param name="password"></param>
        /// <returns>string</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// string value = MSAccessDatabase.ConnectionString("C:\\test", "test.mdb", out errOut);
        /// <br/>
        /// <b>Results</b><br/>
        /// Driver={Microsoft Access Driver (*.mdb)};dbq=C:\test\test.mdb
        /// </example>
        public static string ConnectionString(string databasePath, string databaseName,out string errOut, string password = "")
        {
            string sAns = "";
            errOut = @"";
            try
            {
                sAns = password?.Length > 0 ? $"Driver={{Microsoft Access Driver (*.mdb)}};dbq={databasePath}\\{databaseName};Pwd={password}" : $"Driver={{Microsoft Access Driver (*.mdb)}};dbq={databasePath}\\{databaseName}";
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("ConnectionString", e);
            }
            return sAns;
        }

        /// <summary>
        /// Connections to the MS Access Database using the string OLE.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="errOut"></param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// string value = MSAccessDatabase.ConnectionStringOLE("C:\\test", "test.mdb", out errOut);<br/>
        /// <br/>
        /// <b>Results</b><br/>
        /// Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source="C:\test\test.mdb";
        /// </example>
        public static string ConnectionStringOle(string databasePath, string databaseName, out string errOut, string password = "")
        {
            string sAns = "";
            errOut = @"";
            try
            {
                sAns = password?.Length > 0 ? $"Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=\"{databasePath}\\{databaseName}\";Jet OLEDB:Database Password={password};" : $"Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=\"{databasePath}\\{databaseName}\";";
            }
            catch (Exception e)
            {
                errOut = ErrorMessage( "ConnectionString", e);
            }
            return sAns;
        }
        #endregion


        /// <summary>
        /// Connects the database using the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// MSAccessDatabase obj = new MSAccessDatabase(); <br/>
        /// bool value = obj.ConnectDB(ConnString, out errOut); <br/>
        /// obj.Close(out errOut); <br/>
        /// </example>
        public bool ConnectDb(string connectionString, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                _conn = new OdbcConnection(connectionString);
                _conn.Open();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("ConnectDB", e);
            }
            return bAns;
        }
        /// <summary>
        /// Closes the specified error MSG.
        /// </summary>
        /// <param name="errMsg">The error MSG.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// MSAccessDatabase obj = new MSAccessDatabase(); <br/>
        /// obj.Close(out errOut); <br/>
        /// </example>
        public bool Close(out string errMsg)
        {
            bool bAns = false;
            errMsg = @"";
            try
            {
                if(_conn.State != ConnectionState.Closed)
                {
                    _conn.Close();
                }
                _conn = null;
                bAns = true;
            }
            catch (Exception e)
            {
                errMsg = ErrorMessage( "CloseDB", e);
            }
            return bAns;
        }
        /// <summary>
        /// Connect to the database and execute the SQL statement that you passed.
        /// In this function, we set the connection object null instead of using the Close Function
        /// because you might be using that object for something else, and this will take out the connection
        /// from right under neath you, so we just set that to null instead of a hard close.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.Exception"></exception>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// string SQL = "INSERT INTO Gun_Cal(Cal) VALUES('TEST');"; <br/>
        /// MSAccessDatabase obj = new MSAccessDatabase(); <br/>
        /// bool value = obj.ConnExec(ConnString, SQL, out errOut); <br/>
        /// </example>
        public bool ConnExec(string connectionString, string sql, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                if (ConnectDb(connectionString, out errOut))
                {
                    OdbcCommand cmd = new OdbcCommand(sql, _conn);
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    _conn = null;
                    bAns = true;
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("ConnExec", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>DataTable.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        ///  String SQL = "Select * from Gun_Cal"; <br/>
        /// MSAccessDatabase obj = new MSAccessDatabase(); <br/>
        /// DataTable table = obj.GetData(ConnString, SQL, out errOut); <br/>
        /// string TestValue = @""; <br/>
        ///    foreach(DataRow row in table.Rows) <br/>
        ///    { <br/>
        ///        TestValue += String.Format("{0}{1}",row["Cal"].ToString(),Environment.NewLine); <br/>
        ///    } <br/>
        /// </example>
        public DataTable GetData(string connectionString, string sql, out string errOut)
        {
            DataTable table = new DataTable();
            errOut = @"";
            try
            {
                table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                if (ConnectDb(connectionString, out errOut))
                {
                    OdbcCommand cmd = new OdbcCommand(sql, _conn);
                    OdbcDataAdapter rs = new OdbcDataAdapter {SelectCommand = cmd};
                    rs.Fill(table);
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetData", e);
            }
            return table;
        }

        /// <summary>
        /// Get the Identity seed from the table base on your T SQl statement.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="identitySeedColumnName"></param>
        /// <param name="errOut"></param>
        /// <returns>number</returns>
        /// <example>
        /// string sql = "select id from sometable where something='something'"; <br/>
        /// int value = GetIDFromTableBasedOnTSQL(SomeConnectionString, sql, "id", out var errOut);
        /// 
        /// </example>
        public static int GetIDFromTableBasedOnTSQL(string connection, string sql, string identitySeedColumnName, out string errOut)
        {
            int iAns = 0;
            errOut = @"";
            try
            {
                MsAccessDatabase obj = new MsAccessDatabase();
                //TODO: Add this to Unit Test
                DataTable dt = obj.GetData(connection, sql, out errOut);
                if (errOut?.Length > 0) throw new Exception(errOut);
                foreach (DataRow dr in dt.Rows)
                {
                    iAns = Convert.ToInt32(dr[identitySeedColumnName]);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetIDFromTableBasedOnTSQL", e);
            }
            return iAns;
        }

    }
}
