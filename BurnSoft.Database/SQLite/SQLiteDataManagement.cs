using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

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
        public SQLiteConnection ConnObject;

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

        public void CloseDb()
        {
            if (ConnObject.State != System.Data.ConnectionState.Closed)
            {
                ConnObject.Close();
            }
            ConnObject = null;
        }

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
    }
}
