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
using klient.Model;

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
            }
            catch (SqlException)
            {

            }
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

        public List<Operacja> pobierzOperacje(string where = "")
        {
            string query = "SELECT * FROM T_OPERACJE";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Operacja> data = (from item in dane.AsEnumerable() 
                            select new Operacja(
                                (int)item["c_id_operacji"],
                                (bool)item["c_active"],
                                (string)item["c_operacja"]
                            )
                      ).ToList<Operacja>();
            return data;
        }

        public List<Rola> pobierzRole(string where = "")
        {
            string query = "SELECT * FROM T_ROLE";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Rola> data = (from item in dane.AsEnumerable()
                                    select new Rola(
                                           (int)item["c_id_roli"],
                                           (string)item["c_nazwa"],
                                           (bool)item["c_aktywna"],
                                           (int)item["c_grupy_ktorych_dotyczy"]  
                                       )
                      ).ToList<Rola>();
            return data;
        }
        public List<Przywilej> pobierzPrzywileje(string where = "")
        {
            string query = "SELECT * FROM T_PRZYWILEJE";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Przywilej> data = (from item in dane.AsEnumerable()
                                    select new Przywilej(
                                      (int)item["c_Fk_id_roli"],
                                      (int)item["c_id_operacji"],
                                      (bool)(item["c_aktywny"])
                                  )
                      ).ToList<Przywilej>();
            return data;
        }

        public List<Prowadzacy> pobierzProwadzacych(string where = "")
        {
            string query = "SELECT * FROM T_PROWADZACY";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Prowadzacy> data = (from item in dane.AsEnumerable()
                                         select new Prowadzacy(
                                           (int)item["c_Id_pracownika"],
                                           (int)item["c_wiek"],
                                           (int)item["c_staz"],
                                           (string)item["c_pesel"],
                                           (string)item["c_imie"],
                                           (string)item["c_nazwisko"],
                                           (string)item["c_katedra"],
                                           (string)item["c_tytul"],
                                           (string)item["c_wydzial"]
                                       )
                      ).ToList<Prowadzacy>();
            return data;
        }

        public List<Student> pobierzStudentow(string where = "")
        {
            string query = "SELECT * FROM T_STUDENCI";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Student> data = (from item in dane.AsEnumerable()
                                  select new Student(
                                       (int)item["c_Nr_indeksu"],
                                       (string)item["c_Imie"],
                                       (string)item["c_Nazwisko"],
                                       (string)item["c_Pesel"],
                                       (int)item["c_Dlug_ects"],
                                       (int)item["c_Rok"],
                                       (int)item["c_Semestr"]
                                   )
                      ).ToList<Student>();
            return data;
        }

        public List<Przedmiot> pobierzPrzedmioty(string where = "")
        {
            string query = "SELECT * FROM T_PRZEDMIOTY";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Przedmiot> data = (from item in dane.AsEnumerable()
                                    select new Przedmiot(
                                       (string)item["c_Nazwa"],
                                       (int)item["c_Fk_gl_prowadz"],
                                       (int)item["c_Semestr"],
                                       (int)item["c_Ilosc_ects"],
                                       (int)item["c_Ilosc_godzin"]
                                   )
                      ).ToList<Przedmiot>();
            return data;
        }

        public List<Wynik> pobierzWyniki(string where = "")
        {
            string query = "SELECT * FROM T_WYNIKI";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Wynik> data = (from item in dane.AsEnumerable()
                                select new Wynik(2.5, "asd", 3
                                      // (float)item["c_Wynik"],
                                      // (string)item["c_Fk_Przedmiot"],
                                       //(int)item["c_Fk_Student"]
                                   )
                      ).ToList<Wynik>();
            return data;
        }
        public List<SkladowaPrzedmiotu> pobierzSkladowePrzedmiotow(string where = "")
        {
            string query = "SELECT * FROM T_SKLADOWE_PRZEDMIOTOW";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<SkladowaPrzedmiotu> data = (from item in dane.AsEnumerable()
                                select new SkladowaPrzedmiotu(
                                       (int)item["c_Id_skladowej"],
                                       (string)item["c_Fk_skladowa"],
                                       (int)item["c_Ilosc_godzin"],
                                       (string)item["c_Fk_Przedmiot"],
                                       (int)item["c_Fk_osoby_odp"]
                                   )
                      ).ToList<SkladowaPrzedmiotu>();
            return data;
        }
        public List<Uzytkownik> pobierzUzytkownikow(string where = "")
        {
            string query = "SELECT * FROM T_UZYTKOWNICY";
            if (!string.IsNullOrEmpty(where))
                query += " WHERE " + where;
            DataTable dane = pobierz_dane(query);
            List<Uzytkownik> data = (from item in dane.AsEnumerable()
                                            select new Uzytkownik(
                                                    (int)item["c_id_uzytkownika"],
                                                    (int)(item["c_Fk_nr_indeksu"] == System.DBNull.Value ? 0 : item["c_Fk_nr_indeksu"]),
                                                    (int)(item["c_Fk_id_pracownika"] == System.DBNull.Value ? 0 : item["c_Fk_id_pracownika"]),
                                                    (string)item["c_nazwa"],
                                                    (int)item["c_grupa"],
                                                    (string)item["c_haslo"]
                                                )
                      ).ToList<Uzytkownik>();
            return data;
        }

    }
}
