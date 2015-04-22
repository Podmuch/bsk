using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security;
using System.Runtime.InteropServices;

namespace klient
{
    class DataBase
    {
        public static String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }


        private SqlConnection connect;
        public DataBase(string user, string pass, string instance, string dbdir)
        {
            try
            {
                connect = new SqlConnection();
                connect.ConnectionString = "user id=" + user + ";" + "password=" + pass +
                        ";Data Source=" + instance + ";" + "Trusted_Connection=yes;" +
                        "database=" + dbdir + "; " + "connection timeout=3";
                //connect.Open();
            }
            catch(SqlException ex) { }
        }

        public DataTable pobierz_dane(string q)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr;
            SqlCommand sqlc;
            sqlc = new SqlCommand(q);
            sqlc.Connection = this.connect;
            sqlc.Connection.Open();
            dr = sqlc.ExecuteReader();
            dt.Load(dr);
            sqlc.Connection.Close();
            return dt;
        }

    }
}
