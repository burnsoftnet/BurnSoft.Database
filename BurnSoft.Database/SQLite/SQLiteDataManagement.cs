
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
using System.Data.SQLite;
using System.Data;
using System.Globalization;
// ReSharper disable UnusedMember.Local

namespace BurnSoft.Database.SQLite
{
    /// <summary>
    /// Class SQLiteDataManagement handles the ability to read and write data to the SQLite Database.
    /// </summary>
    public class SqLiteDataManagement
    {

        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.SQLite.SQLiteDataManagement";
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
        /// The connection object
        /// </summary>
        public SQLiteConnection ConnObject;
        /// <summary>
        /// Connects the database.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// SQLiteDataManagement obj = new SQLiteDataManagement();
        /// value = obj.ConnectDB("C:\\test\\unittest.db", out errOut);
        /// obj.Close();
        /// </example>
        public bool ConnectDb(string dbName, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                ConnObject = new SQLiteConnection(BaseDatabase.ConnectionString(dbName));
                ConnObject.Open();
                bAns = true;
            }
            catch (Exception e)
            {
                errOut = ErrorMessage( "ConnectDB", e);
            }
            return bAns;
        }
        /// <summary>
        /// Closes the database.
        /// </summary>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// SQLiteDataManagement obj = new SQLiteDataManagement();
        /// value = obj.ConnectDB("C:\\test\\unittest.db", out errOut);
        /// obj.Close();
        /// </example>
        public void CloseDb()
        {
            if (ConnObject.State != ConnectionState.Closed)
            {
                ConnObject.Close();
            }
            ConnObject = null;
        }
        /// <summary>
        /// Disposes this instance.
        /// </summary>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// SQLiteDataManagement obj = new SQLiteDataManagement();
        /// value = obj.ConnectDB("C:\\test\\unittest.db", out errOut);
        /// obj.Dispose();
        /// </example>
        // ReSharper disable once UnusedMember.Global
        public void Dispose()
        {
            ConnObject?.Dispose();
        }

        /// <summary>
        /// Runs the query.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// string sql = "INSERT INTO DB_Version (version) VALUES (2.0);";
        /// bool value = SQLiteDataManagement.RunQuery("C:\\test\\unittest.db", sql, out errOut);
        /// </example>
        public static bool RunQuery(string dbName, string sql, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                SqLiteDataManagement obj = new SqLiteDataManagement();
                if (obj.ConnectDb(dbName, out errOut))
                {
                    SQLiteCommand cmd = new SQLiteCommand
                    {
                        CommandText = sql,
                        Connection = obj.ConnObject
                    };
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    obj.CloseDb();
                    bAns = true;
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("RunQuery", e);
            }
            return bAns;
        }
        /// <summary>
        /// Determines whether the specified database name has data.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="sQl">The s ql.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if the specified database name has data; otherwise, <c>false</c>.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// string sql = "select * from DB_Version;";
        /// bool value = SQLiteDataManagement.HasData("C:\\test\\unittest.db", sql, out errOut);
        /// </example>
        public static bool HasData(string dbName, string sQl , out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                SqLiteDataManagement obj = new SqLiteDataManagement();
                if (obj.ConnectDb(dbName, out errOut))
                {
                    SQLiteCommand cmd = new SQLiteCommand(sQl, obj.ConnObject);
                    using (SQLiteDataReader rs = cmd.ExecuteReader())
                    {
                        bAns = rs.HasRows;
                        rs.Close();
                    }
                    cmd.Dispose();
                    obj.CloseDb();
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("HasData", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the data by SQL.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>DataTable.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// string sql = "select * from DB_Version;";<br/>
        /// DataTable dt = SQLiteDataManagement.GetDataBySQL("C:\\test\\unittest.db", sql, out errOut);<br/>
        /// bool HasData = false;<br/>
        ///    if (errOut.Length == 0)<br/>
        ///    {<br/>
        ///        HasData = (dt.Rows.Count > 0);<br/>
        ///        foreach (DataRow dr in dt.Rows)<br/>
        ///        {<br/>
        ///            Debug.Print("{0}", dr["id"].ToString());<br/>
        ///            Debug.Print("{0}", dr["version"].ToString());<br/>
        ///            Debug.Print("{0}", dr["dt"].ToString());<br/>
        ///            Debug.Print("");<br/>
        ///        }<br/>
        ///}<br/>
        /// </example>
        public static DataTable GetDataBySql(string dbname, string sql, out string errOut)
        {
            DataTable dtAns = new DataTable();
            errOut = @"";
            try
            {
                SqLiteDataManagement obj = new SqLiteDataManagement();
                if (obj.ConnectDb(dbname, out errOut))
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql, obj.ConnObject);
                    using (SQLiteDataAdapter da  = new SQLiteDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dtAns = ds.Tables[0];
                    }
                    cmd.Dispose();
                    obj.CloseDb();
                }
                else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetDataBySQL", e);
            }
            return dtAns;
        }
        /// <summary>
        /// Cleans the database.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// bool value = SQLiteDataManagement.CleanDB("C:\\test\\unittest.db", out errOut);
        /// </example>
        public static bool CleanDb(string dbname, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                bAns = RunQuery(dbname, "vacuum", out errOut);
                if (!bAns) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("CleanDb", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the single value from a T-SQL query, granted it is working in just strings, but the public functions
        /// will contain the conversions for integer and double values as needed.  This was just a generic interface for a quick
        /// and easy pull from the database.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="sColName">Name of the s col.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// string sql = "select * from DB_Version order by dt desc limit 1;";<br/>
        /// string value = GetSingleValue("C:\\test\\unittest.db", sql, "dt", "", out errOut);
        /// </example>
        private static string GetSingleValue(string dbname, string sql, string sColName, string defaultValue, out string errOut)
        {
            string sAns = defaultValue;
            errOut = @"";
            try
            {
                SqLiteDataManagement obj = new SqLiteDataManagement();
                if (obj.ConnectDb(dbname, out errOut))
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql, obj.ConnObject);
                    using (SQLiteDataReader rs = cmd.ExecuteReader())
                    {
                        while (rs.Read())
                        {
                            if (rs.GetValue(rs.GetOrdinal(sColName)) != DBNull.Value)
                            {
                                sAns = Convert.ToString(rs.GetValue(rs.GetOrdinal(sColName)));
                            }
                        }
                        rs.Close();
                    }
                    cmd.Dispose();
                    obj.CloseDb();
                } else
                {
                    throw new Exception(errOut);
                }
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetSingleValue", e);
            }
            return sAns;
        }
        /// <summary>
        /// Gets the single value from database.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="sColName">Name of the s col.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// string sql = "select * from DB_Version order by dt desc limit 1;";<br/>
        /// string value = SQLiteDataManagement.GetSingleValueFromDatabase("C:\\test\\unittest.db", sql, "dt", "", out errOut);<br/>
        /// </example>
        public static string GetSingleValueFromDatabase(string dbname, string sql, string sColName, string defaultValue, out string errOut)
        {
            string sAns = @"";
            errOut = @"";
            try
            {
                sAns = GetSingleValue(dbname, sql, sColName, defaultValue, out errOut);
                if (errOut.Length > 0) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetSingleValueFromDatabase", e);
            }
            return sAns;
        }
        /// <summary>
        /// Gets the single value from database.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="sColName">Name of the s col.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        /// string sql = "select * from DB_Version order by dt desc limit 1;";<br/>
        /// int value = SQLiteDataManagement.GetSingleValueFromDatabase("C:\\test\\unittest.db", sql, "id", 0, out errOut);<br/>
        /// </example>
        public static int GetSingleValueFromDatabase(string dbname, string sql, string sColName, int defaultValue, out string errOut)
        {
            int iAns = 0;
            errOut = @"";
            try
            {
                iAns = Convert.ToInt32(GetSingleValue(dbname, sql, sColName, Convert.ToString(defaultValue), out errOut));
                if (errOut.Length > 0) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage( "GetSingleValueFromDatabase", e);
            }
            return iAns;
        }
        /// <summary>
        /// Gets the single value from database.
        /// </summary>
        /// <param name="dbname">The dbname.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="sColName">Name of the s col.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns>System.Double.</returns>
        /// <exception cref="Exception"></exception>
        /// <example>
        /// <b>SEE UNIT TESTS @ UnitTest_SQLite_SQLiteDataManagement</b><br/>
        /// <br/>
        ///  string sql = "select * from DB_Version order by dt desc limit 1;"; <br/>
        /// double value = SQLiteDataManagement.GetSingleValueFromDatabase("C:\\test\\unittest.db", sql, "version", 0.0, out errOut); <br/>
        /// </example>
        public static double GetSingleValueFromDatabase(string dbname, string sql, string sColName, double defaultValue, out string errOut)
        {
            double dAns = 0;
            errOut = @"";
            try
            {
                dAns = Convert.ToDouble(GetSingleValue(dbname, sql, sColName, Convert.ToString(defaultValue, CultureInfo.InvariantCulture), out errOut));
                if (errOut.Length > 0) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage("GetSingleValueFromDatabase", e);
            }
            return dAns;
        }
    }
}
