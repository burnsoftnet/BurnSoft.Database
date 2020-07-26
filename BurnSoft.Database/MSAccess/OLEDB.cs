using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using ADODB;

namespace BurnSoft.Database.MSAccess
{
    /// <summary>
    /// Class OLEDB.
    /// </summary>
    public class Oledb
    {
        //TODO Add code code to access oledb database for access databases
        public static bool AddPasswordToDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                ADODB.Connection conn = new ADODB.Connection();
                conn.Provider = "Microsoft.Jet.OLEDB.4.0";
                conn.ConnectionString = $"Data Source={path}";
                //conn.Properties["Jet OLEDB:Database Password"].Value = password;
                conn.Mode = ADODB.ConnectModeEnum.adModeShareExclusive;
                conn.Open();
                string sql = $"ALTER DATABASE PASSWORD {password} NULL";
                conn.Execute(sql, out _);
                conn.Close();
            }
            catch (Exception e)
            {
                errOut = e.Message;
            }
            return bAns;
        }

        public static bool RemovePasswordToDatabase(string path, string password, out string errOut)
        {
            bool bAns = false;
            errOut = @"";
            try
            {
                ADODB.Connection conn = new ADODB.Connection();
                conn.Provider = "Microsoft.Jet.OLEDB.4.0";
                conn.ConnectionString = $"Data Source={path}";
                //conn.Properties["Jet OLEDB:Database Password"].Value = password;
                conn.Mode = ADODB.ConnectModeEnum.adModeShareExclusive;
                conn.Open();
                string sql = $"ALTER DATABASE PASSWORD {password} NULL";
                conn.Execute(sql, out _);
                conn.Close();
            }
            catch (Exception e)
            {
                errOut = e.Message;
            }
            return bAns;
        }
    }
}
