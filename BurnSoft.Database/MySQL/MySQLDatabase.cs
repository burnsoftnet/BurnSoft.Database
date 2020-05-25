
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
using MySql.Data.MySqlClient;
using System.Data;

namespace BurnSoft.Database.MySQL
{
    /// <summary>
    /// Class MySQLDatabase handles connections to the Mysql database as well as some simple queries to the database as needed
    /// </summary>
    public class MySQLDatabase
    {
        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MySQL.MySQLDatabase";
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
        private static string ErrorMessage(string location, string FunctionName, InvalidCastException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, ArgumentNullException e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        #endregion        
        public MySqlConnection Conn;
        /// <summary>
        /// Connections the string to connect to a MySQL Server
        /// </summary>
        /// <param name="hostname">The hostname.</param>
        /// <param name="uid">The uid.</param>
        /// <param name="pwd">The password.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.String.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_MySQL_MySQLDatabase</b><br/>
        /// <br/>
        /// string value = MySQLDatabase.ConnectionString("192.168.1.5", "testuser", "test.user", "testdb", out errOut);
        /// <br/>
        /// <b>Result</b><br/>
        /// Server=192.168.1.5;user id=testuser;password=test.user;persist security info=true;database=testdb
        /// </example>
        public static string ConnectionString(string hostname, string uid, string pwd, string databaseName, out string errOut)
        {
            string sAns = @"";
            errOut = @"";
            try
            {
                sAns = $"Server={hostname};user id={uid};password={pwd};persist security info=true;database={databaseName}";
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "Connectionstring", e);
            }
            return sAns;
        }
        /// <summary>
        /// Connects the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_MySQL_MySQLDatabase</b><br/>
        /// <br/>
        ///  MySQLDatabase obj = new MySQLDatabase();<br/>
        /// string connString = MySQLDatabase.ConnectionString("192.168.1.5", "testuser", "test.user", "testdb", out errOut);<br/>
        /// bool value = obj.ConnectDB(connString, out errOut);<br/>
        /// obj.Close();<br/>
        /// </example>
        public bool ConnectDB(string connectionString, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                Conn = new MySqlConnection(connectionString);
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
        /// Closes this instance.
        /// </summary>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_MySQL_MySQLDatabase</b><br/>
        /// <br/>
        ///  MySQLDatabase obj = new MySQLDatabase();<br/>
        /// string connString = MySQLDatabase.ConnectionString("192.168.1.5", "testuser", "test.user", "testdb", out errOut);<br/>
        /// obj.Close();<br/>
        /// </example>
        public void Close()
        {
            if (Conn.State != ConnectionState.Closed)
            {
                Conn.Close();
            }
            Conn = null;
        }
        /// <summary>
        /// Runs the query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_MySQL_MySQLDatabase</b><br/>
        /// <br/>
        /// string sql = "CREATE TABLE `DB_Version` (`ID` int(11) NOT NULL AUTO_INCREMENT,`verNo` varchar(45) DEFAULT NULL,`dtUpdated` timestamp NULL DEFAULT CURRENT_TIMESTAMP, PRIMARY KEY(`ID`),  UNIQUE KEY `ID_UNIQUE` (`ID`)) ENGINE = MyISAM AUTO_INCREMENT = 5 DEFAULT CHARSET = latin1;";<br/>
        /// string connString = MySQLDatabase.ConnectionString("192.168.1.5", "testuser", "test.user", "testdb", out errOut);<br/>
        /// bool value = MySQLDatabase.RunQuery(connString, sql, out errOut);<br/>
        /// </example>
        public static bool RunQuery(string connectionString, string sql, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                MySQLDatabase obj = new MySQLDatabase();
                if (obj.ConnectDB(connectionString, out errOut))
                {
                    MySqlCommand cmd = new MySqlCommand()
                    {
                        CommandText = sql,
                        Connection = obj.Conn
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    obj.Close();
                    bAns = true;
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "RunQuery", e);
            }
            return bAns;
        }

        public static DataTable GetData(string connectionString, string sql, out string errOut)
        {
            DataTable dt = new DataTable();
            errOut = @"";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation,"GetData", e);
            }
            return dt;
        }
    }
}
