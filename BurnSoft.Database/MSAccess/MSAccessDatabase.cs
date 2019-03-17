using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace BurnSoft.Database.MSAccess
{
    public class MSAccessDatabase
    {
        #region "Error Handling"
        private static string ClassLocation = "BurnSoft.Database.MSAccess.MSAccessDatabase";
        private static string ErrorMessage(string location, string FunctionName, Exception e) => $"{ClassLocation}.{FunctionName} - {e.Message.ToString()}";

        #endregion
        #region "Class Vars"
        OdbcConnection Conn;
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

    }
}
