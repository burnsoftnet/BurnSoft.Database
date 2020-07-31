
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
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using BurnSoft.Database.MySQL.Type;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable ConstantConditionalAccessQualifier

namespace BurnSoft.Database.MySQL
{
    /// <summary>
    /// Class MySQLDatabase handles connections to the Mysql database as well as some simple queries to the database as needed
    /// </summary>
    public class MySqlDatabase
    {
        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.MySQL.MySQLDatabase";
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
        /// MySQl Connection Object
        /// </summary>
        public MySqlConnection Conn;
        /// <summary>
        /// Connections the string to connect to a MySQL Server
        /// </summary>
        /// <param name="hostname">The host name.</param>
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
                errOut = ErrorMessage("ConnectionString", (ArgumentNullException) e);
            }
            return sAns;
        }
        /// <summary>
        /// Parses the connection string from the web.config connection string section.
        /// </summary>
        /// <param name="webConfigConnectionStringName">Name of the web configuration connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>List&lt;ConString&gt;.</returns>
        private static List<ConString> ParseConnectionString(string webConfigConnectionStringName, out string errOut)
        {
            List<ConString> sAns = new List<ConString>();
            errOut = @"";
            try
            {
                string[] lst = webConfigConnectionStringName.Split(';');
                List<ConString> cs = new List<ConString>();
                string server = @"";
                string database = @"";
                string persistSecurityInfo = @"";
                string userId = @"";
                string password = @"";

                foreach (string l in lst)
                {
                    string[] newLst = l.Split('=');

                    if (newLst[0].Trim().ToLower().Equals("server"))
                    {
                        server = newLst[1].Trim();
                    }
                    else if (newLst[0].Trim().ToLower().Equals("password"))
                    {
                        password = newLst[1].Trim();
                    }
                    else if (newLst[0].Trim().ToLower().Equals("persistsecurityinfo"))
                    {
                        persistSecurityInfo = newLst[1].Trim();
                    }
                    else if (newLst[0].Trim().ToLower().Equals("database"))
                    {
                        database = newLst[1].Trim();
                    }
                    else if (newLst[0].Trim().ToLower().Equals("user id"))
                    {
                        userId = newLst[1].Trim();
                    }
                }

                cs.Add(new ConString()
                {
                    Server = server,
                    Database = database,
                    UserID = userId,
                    Password = password,
                    PersistSecurityInfo = persistSecurityInfo
                });

                sAns = cs;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("ParseConnectionString", (ArgumentNullException)e);
            }
            return sAns;
        }
        /// <summary>
        /// Pass the connection string from the web.config file and have it return back a connection string that the mysql libraries like.
        /// </summary>
        /// <param name="webConfigConnectionStringName">Name of the web configuration connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.String.</returns>
        /// <example>
        /// string conString = MySQLDatabase.ConnectionString(ConfigurationManager.ConnectionString["MySQLConnection"].ToString, out var err); <br/>
        ///  <br/>
        /// <b>Results</b> <br/>
        ///  <br/>
        ///  Server=serverName;user id=userName;password=password;persist security info=true;database=Northwind
        /// </example>
        public static string ConnectionString(string webConfigConnectionStringName, out string errOut)
        {
            string sAns = @"";
            errOut = @"";
            try
            {
               
                List<ConString> cs = ParseConnectionString(webConfigConnectionStringName, out errOut);
                
                foreach (ConString s in cs)
                {
                    sAns = ConnectionString(s.Server, s.UserID, s.Password, s.Database, out errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("ConnectionString", (ArgumentNullException)e);
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
        public bool ConnectDb(string connectionString, out string errOut)
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
                errOut = ErrorMessage("ConnectDB", (ArgumentNullException) e);
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
                MySqlDatabase obj = new MySqlDatabase();
                if (obj.ConnectDb(connectionString, out errOut))
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
                errOut = ErrorMessage("RunQuery", (ArgumentNullException) e);
            }
            return bAns;
        }
        /// <summary>
        /// pass the connection string and SQL-T statement and get a return in a DataTable Format
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>DataTable.</returns>
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
                errOut = ErrorMessage("GetData", (ArgumentNullException) e);
            }
            return dt;
        }
        /// <summary>
        /// Gets the data and returns it in a data set format.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>DataSet.</returns>
        public static DataSet GetData(string connection, string sql, string tableName, out string errOut)
        {
            DataSet ds = new DataSet();
            errOut = @"";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connection))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                        {
                            sda.Fill(ds, tableName);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetData", e);
            }

            return ds;
        }
        /// <summary>
        /// Pass a SQL Statement to see if rows exist in the table  with those requirements.
        /// If it doesn't exist, it will return false, if one or more row exists, it will return true
        /// </summary>
        /// <param name="connectionString">MySQL Connections string</param>
        /// <param name="sql">T-SQL query with the requirements</param>
        /// <param name="errOut">If an exception occurred, this will contain the error message.</param>
        /// <returns></returns>
        public static bool ValueExists(string connectionString, string sql, out string errOut)
        {
            bool bAns = false;

            try
            {
                DataTable dt = GetData(connectionString, sql, out errOut);
                if (errOut?.Length > 0) throw  new Exception(errOut);
                bAns = (dt.Rows?.Count > 0);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("ValueExists", e);
            }

            return bAns;
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
                //TODO: Add this to Unit Test
                DataTable dt = GetData(connection, sql, out errOut);
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
