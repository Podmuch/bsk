using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace klient
{
    class DataBase
    {
        private SqlConnection connect;
        public DataBase(string user, string pass, string instance, string dbdir)
        {
            try
            {
                connect = new SqlConnection();
                connect.ConnectionString = "user id=" + user + ";" + "password=" + pass +
                        ";Data Source=" + instance + ";" + "Trusted_Connection=no;" +
                        "database=" + dbdir + "; " + "connection timeout=3";
                connect.Open();
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
            dr = sqlc.ExecuteReader();
            dt.Load(dr); 
            return dt;

        }
    }
}
