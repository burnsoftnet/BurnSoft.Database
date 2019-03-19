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
    }
}
