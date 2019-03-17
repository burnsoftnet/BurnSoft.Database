﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace BurnSoft.Database.MSAccess
{
    /// <summary>
    /// Class MSAccessDatabase, Helps connect and manage MS Access Databases
    /// </summary>
    public class MSAccessDatabase
    {
        #region "Error Handling"        
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

        #endregion
        #region "Class Vars"        
        /// <summary>
        /// The connection
        /// </summary>
        OdbcConnection Conn;
        /// <summary>
        /// Connection String Format Used to Connect to MS Access Databases using the Microsoft Access Driver
        /// </summary>
        /// <param name="DatabasePath"></param>
        /// <param name="databaseName"></param>
        /// <param name="password"></param>
        /// <param name="errOur"></param>
        /// <returns></returns>
        #endregion
        public static string ConnectionString(string DatabasePath, string databaseName, string password,out string errOur)
        {
            //sAns = "Driver={Microsoft Access Driver (*.mdb)};dbq=" & APPLICATION_PATH_DATA & "\" & DATABASE_NAME & ";Pwd=14un0t2n0"
            string sAns = "";
            errOur = @"";
            try
            {
                if (password.Length > 0)
                {
                    sAns = $"Driver={{Microsoft Access Driver (*.mdb)}};dbq={DatabasePath}\\{databaseName};Pwd={password}";
                } else
                {
                    sAns = $"Driver={{Microsoft Access Driver (*.mdb)}};dbq={DatabasePath}\\{databaseName};Pwd={password}";
                }
            }
            catch (Exception e)
            {
                errOur = ErrorMessage(ClassLocation, "ConnectionString", e);
            }
            return sAns;
        }

        /// <summary>
        /// Connects the database using the connection string.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        /// <param name="errOut">The error out.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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

    }
}
