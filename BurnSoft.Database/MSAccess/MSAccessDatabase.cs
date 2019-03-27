using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;

namespace BurnSoft.Database.MSAccess
{
    /// <summary>
    /// Class MSAccessDatabase, Helps connect and manage MS Access Databases
    /// </summary>
    public class MSAccessDatabase
    {
        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MSAccess.MSAccessDatabase";
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
        #region "Class Vars"        
        /// <summary>
        /// The connection
        /// </summary>
        OdbcConnection Conn;
        #endregion
        #region "Connection Strings"
        /// <summary>
        /// Connection String Format Used to Connect to MS Access Databases using the Microsoft Access Driver
        /// </summary>
        /// <param name="DatabasePath"></param>
        /// <param name="databaseName"></param>
        /// <param name="password"></param>
        /// <param name="errOur"></param>
        /// <returns>string</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// string value = MSAccessDatabase.ConnectionString("C:\\test", "test.mdb", out errOut);
        /// <br/>
        /// <b>Results</b><br/>
        /// Driver={Microsoft Access Driver (*.mdb)};dbq=C:\test\test.mdb
        /// </example>
        public static string ConnectionString(string DatabasePath, string databaseName,out string errOut, string password = "")
        {
            string sAns = "";
            errOut = @"";
            try
            {
                if (password.Length > 0)
                {
                    sAns = $"Driver={{Microsoft Access Driver (*.mdb)}};dbq={DatabasePath}\\{databaseName};Pwd={password}";
                } else
                {
                    sAns = $"Driver={{Microsoft Access Driver (*.mdb)}};dbq={DatabasePath}\\{databaseName}";
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "ConnectionString", e);
            }
            return sAns;
        }
        /// <summary>
        /// Connections to the MS Access Database using the string OLE.
        /// </summary>
        /// <param name="DatabasePath">The database path.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="password">The password.</param>
        /// <param name="errOur">The error our.</param>
        /// <returns>System.String.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// string value = MSAccessDatabase.ConnectionStringOLE("C:\\test", "test.mdb", out errOut);<br/>
        /// <br/>
        /// <b>Results</b><br/>
        /// Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source="C:\test\test.mdb";
        /// </example>
        public static string ConnectionStringOLE(string DatabasePath, string databaseName, out string errOut, string password = "")
        {
            string sAns = "";
            errOut = @"";
            try
            {
                if (password.Length > 0)
                {
                    sAns = $"Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=\"{DatabasePath}\\{databaseName}\";Jet OLEDB:Database Password={password};";
                }
                else
                {
                    sAns = $"Provider=Microsoft.Jet.OLEDB.4.0;Persist Security Info=False;Data Source=\"{DatabasePath}\\{databaseName}\";";
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "ConnectionString", e);
            }
            return sAns;
        }
        #endregion


        /// <summary>
        /// Connects the database using the connection string.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// SEE UNIT TEST @ UnitTest_MSAccess <br/>
        /// <br/>
        /// MSAccessDatabase obj = new MSAccessDatabase(); <br/>
        /// bool value = obj.ConnectDB(ConnString, out errOut); <br/>
        /// obj.Close(out errOut); <br/>
        /// </example>
        public bool ConnectDB(string ConnectionString, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Conn = new OdbcConnection(ConnectionString);
                Conn.Open();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "ConnectDB", e);
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
                if(Conn.State != System.Data.ConnectionState.Closed)
                {
                    Conn.Close();
                }
                Conn = null;
            }
            catch (Exception e)
            {
                errMsg = ErrorMessage(ClassLocation, "CloseDB", e);
            }
            return bAns;
        }
        /// <summary>
        /// Connect to the database and execture the SQL statement that you passed.
        /// In this function, we set the connection objecto null instead of using the Close Function
        /// because you might be using that object for something else, and this will take out the connection
        /// from right uder neath you, so we just set that to null instead of a hard close.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="SQL">The SQL.</param>
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
        public bool ConnExec(string ConnectionString, string SQL, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                if (ConnectDB(ConnectionString, out errOut))
                {
                    OdbcCommand CMD = new OdbcCommand(SQL, Conn);
                    CMD.ExecuteNonQuery();
                    CMD.Connection.Close();
                    CMD = null;
                    Conn = null;
                    bAns = true;
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "ConnExec", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="SQL">The SQL.</param>
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
        public DataTable GetData(string ConnectionString, string SQL, out string errOut)
        {
            DataTable Table = new DataTable();
            errOut = @"";
            try
            {
                Table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                if (ConnectDB(ConnectionString, out errOut))
                {
                    OdbcCommand CMD = new OdbcCommand(SQL, Conn);
                    OdbcDataAdapter RS = new OdbcDataAdapter();
                    RS.SelectCommand = CMD;
                    RS.Fill(Table);
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "GetData", e);
            }
            return Table;
        }
    }
}
