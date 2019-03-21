using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace BurnSoft.Database.SQLite
{
    /// <summary>
    /// Class SQLiteDataManagement handles the ability to read and write data to the SQLite Database.
    /// </summary>
    public class SQLiteDataManagement
    {

        #region "Exception Error Handling"        
        /// <summary>
        /// The class location
        /// </summary>
        private static string ClassLocation = "BurnSoft.Database.SQLite.SQLiteDataManagement";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, Exception e) => "{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, InvalidCastException e) => "{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
        /// <summary>
        /// Errors the message.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="FunctionName">Name of the function.</param>
        /// <param name="e">The e.</param>
        /// <returns>System.String.</returns>
        private static string ErrorMessage(string location, string FunctionName, ArgumentNullException e) => "{ClassLocation}.{FunctionName} - {e.Message.ToString()}";
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
        public bool ConnectDB(string dbName, out string errOut)
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
                errOut = ErrorMessage(ClassLocation, "ConnectDB", e);
            }
            return bAns;
        }
        /// <summary>
        /// Closes the database.
        /// </summary>
        public void CloseDb()
        {
            if (ConnObject.State != System.Data.ConnectionState.Closed)
            {
                ConnObject.Close();
            }
            ConnObject = null;
        }
        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            ConnObject?.Dispose();
        }

        /// <summary>
        /// Runs the query.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="SQL">The SQL.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="Exception"></exception>
        public static bool RunQuery(string dbName, string SQL, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                SQLiteDataManagement obj = new SQLiteDataManagement();
                if (obj.ConnectDB(dbName, out errOut))
                {
                    SQLiteCommand cmd = new SQLiteCommand
                    {
                        CommandText = SQL,
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
                errOut = ErrorMessage(ClassLocation, "RunQuery", e);
            }
            return bAns;
        }
        /// <summary>
        /// Determines whether the specified database name has data.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="SQl">The s ql.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if the specified database name has data; otherwise, <c>false</c>.</returns>
        /// <exception cref="Exception"></exception>
        public static bool HasData(string dbName, string SQl , out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                SQLiteDataManagement obj = new SQLiteDataManagement();
                if (obj.ConnectDB(dbName, out errOut))
                {
                    SQLiteCommand cmd = new SQLiteCommand(SQl, obj.ConnObject);
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
                errOut = ErrorMessage(ClassLocation, "HasData", e);
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
        public static DataTable GetDataBySQL(string dbname, string sql, out string errOut)
        {
            DataTable dtAns = new DataTable();
            errOut = @"";
            try
            {
                SQLiteDataManagement obj = new SQLiteDataManagement();
                if (obj.ConnectDB(dbname, out errOut))
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
                errOut = ErrorMessage(ClassLocation, "GetDataBySQL", e);
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
        public static bool CleanDB(string dbname, out string errOut)
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
                errOut = ErrorMessage(ClassLocation, "CleanDb", e);
            }
            return bAns;
        }
        /// <summary>
        /// Gets the single value from a T-SQL querey, granted it is working in just strings, but the public functions
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
        private static string GetSingleValue(string dbname, string sql, string sColName, string defaultValue, out string errOut)
        {
            string sAns = defaultValue;
            errOut = @"";
            try
            {
                SQLiteDataManagement obj = new SQLiteDataManagement();
                if (obj.ConnectDB(dbname, out errOut))
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
                errOut = ErrorMessage(ClassLocation, "GetSingleValue", e);
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
                errOut = ErrorMessage(ClassLocation, "GetSingleValueFromDatabase", e);
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
                errOut = ErrorMessage(ClassLocation, "GetSingleValueFromDatabase", e);
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
        public static double GetSingleValueFromDatabase(string dbname, string sql, string sColName, double defaultValue, out string errOut)
        {
            double dAns = 0;
            errOut = @"";
            try
            {
                dAns = Convert.ToDouble(GetSingleValue(dbname, sql, sColName, Convert.ToString(defaultValue), out errOut));
                if (errOut.Length > 0) throw new Exception(errOut);
            }
            catch (Exception e)
            {
                errOut = ErrorMessage(ClassLocation, "GetSingleValueFromDatabase", e);
            }
            return dAns;
        }
    }
}
