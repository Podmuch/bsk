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
                query += where;
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
                query += where;
            DataTable dane = pobierz_dane(query);
            List<Rola> data = (from item in dane.AsEnumerable()
                                    select new Rola(
                                           (int)item["c_id_roli"],
                                           (string)item["c_rola"],
                                           (bool)item["c_aktywna"],
                                           (int)item["c_grupy_ktorych_dotyczy"]  
                                       )
                      ).ToList<Rola>();
            return data;
        }

        public List<ProwadzacySkladowych> pobierzProwadzacychSkladowych(string where ="")
        {
            string query = "SELECT * FROM t_Prowadzacy_skladowych_czesci";
            if (!string.IsNullOrEmpty(where))
                query += where;
            DataTable dane = pobierz_dane(query);
            List<ProwadzacySkladowych> data = (from item in dane.AsEnumerable()
                                    select new ProwadzacySkladowych(
                                      (int)item["c_Fk_id_skladowej"],
                                      (int)item["c_Fk_id_pracownika"]
                                  )
                      ).ToList<ProwadzacySkladowych>();
            return data;
        }
        public List<Przywilej> pobierzPrzywileje(string where = "")
        {
            string query = "SELECT * FROM T_PRZYWILEJE";
            if (!string.IsNullOrEmpty(where))
                query += where;
            DataTable dane = pobierz_dane(query);
            List<Przywilej> data = (from item in dane.AsEnumerable()
                                    select new Przywilej(
                                      (int)item["c_Fk_id_roli"],
                                      (int)item["c_Fk_id_operacji"],
                                      (bool)(item["c_aktywny"])
                                  )
                      ).ToList<Przywilej>();
            return data;
        }

        public List<Prowadzacy> pobierzProwadzacych(string where = "")
        {
            string query = "SELECT * FROM T_PROWADZACY";
            if (!string.IsNullOrEmpty(where))
                query += where;
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
                query += where;
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
                query +=  where;
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
                query += where;
            DataTable dane = pobierz_dane(query);
            List<Wynik> data = (from item in dane.AsEnumerable()
                                select new Wynik((double)item["c_Wynik"],
                                       (string)item["c_Fk_Przedmiot"],
                                       (int)item["c_Fk_Student"]
                                   )
                      ).ToList<Wynik>();
            return data;
        }
        public List<SkladowaPrzedmiotu> pobierzSkladowePrzedmiotow(string where = "")
        {
            string query = "SELECT * FROM T_SKLADOWE_PRZEDMIOTOW";
            if (!string.IsNullOrEmpty(where))
                query += where;
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
                query += where;
            DataTable dane = pobierz_dane(query);
            List<Uzytkownik> data = (from item in dane.AsEnumerable()
                                            select new Uzytkownik(
                                                    (int)item["c_id_uzytkownika"],
                                                    (int)(item["c_Fk_nr_indeksu"] == System.DBNull.Value ? 0 : item["c_Fk_nr_indeksu"]),
                                                    (int)(item["c_Fk_id_pracownika"] == System.DBNull.Value ? 0 : item["c_Fk_id_pracownika"]),
                                                    (string)item["c_login"],
                                                    (int)item["c_grupa"],
                                                    (string)item["c_haslo"]
                                                )
                      ).ToList<Uzytkownik>();
            return data;
        }

        public List<Operacja> pobierzOperacjeDlaDanejRoli(int idRoli) 
        {
            List<Operacja> data = pobierzOperacje(" join t_przywileje on c_id_operacji = c_Fk_id_operacji where c_Fk_id_roli = " + idRoli);
            return data;
        }

        public void dodajNowaRole(string nazwaRoli, List<Operacja> listaOperacji)
        {
            var role = pobierzRole(" WHERE c_rola = '" + nazwaRoli + "'");
            if (role.Count > 0 || string.IsNullOrEmpty(nazwaRoli))
                return;
            Rola rola = new Rola(-1, nazwaRoli, true, 0);
            insertNewRole(rola);

            var stworzonaRola = pobierzRole(" WHERE c_rola = '" + nazwaRoli + "'");
            if(stworzonaRola.Count > 0)
            {
                foreach (Operacja o in listaOperacji)
                {
                    var przywilej = new Przywilej(stworzonaRola[0].Id, o.IdOperacji, true);
                    insertNewPrivilege(przywilej);
                }
            }            
        }

        public void dodajNowegoUzytkownika(string login, List<Rola> listaRol, List<Rola> Role)
        {
            var uzytkownicy = pobierzUzytkownikow(" WHERE c_login = '" + login + "'");
            if (uzytkownicy.Count > 0 || string.IsNullOrEmpty(login))
                return;
            int posiadaneRole = 0;
            foreach (Rola r in listaRol)
            {
                posiadaneRole += (int)Math.Pow(2, Role.IndexOf(r));
            }
            Uzytkownik uzytkownik = new Uzytkownik(-1, -1,-1, login, posiadaneRole, "domyslne");
            insertNewUser(uzytkownik);         
        }
        public void insertNewRole(Rola r)
        {
            string query = "INSERT INTO T_ROLE(c_rola, c_grupy_ktorych_dotyczy) VALUES ('" 
                + r.Nazwa + "', " + r.Grupy_ktorych_dotyczy + ")";
            pobierz_dane(query); // pobierz_dane :d
        }

        public void insertNewUser(Uzytkownik u)
        {
            string query = "INSERT INTO t_Uzytkownicy(c_Fk_nr_indeksu, c_Fk_id_pracownika, c_login, c_haslo, c_grupa) values (NULL, NULL, '" +
                u.NazwaUzytkownika + "', '04c72343945e2a6ef09221862164ac3a9e914373'," + u.Grupa + ")";
            pobierz_dane(query); // pobierz_dane :d
        }
        public void insertNewPrivilege(Przywilej p)
        {
            string query = "INSERT INTO t_Przywileje(c_Fk_id_roli, c_Fk_id_operacji) VALUES ("
                + p.IdRoli + ", " + p.IdOperacji + ")";
            pobierz_dane(query); // pobierz_dane :d
        }

        private void DeleteAllPrivilegesWithThisRole(Rola rola)
        {
            string query = "DELETE FROM t_Przywileje WHERE c_Fk_id_roli='" + rola.Id + "';";
            pobierz_dane(query); // pobierz_dane :d
        }
        public void ModyfikujUyztkownika(Uzytkownik wybrany, string Login, List<Rola> wybraneRole, List<Rola> Role)
        {
            int posiadaneRole = 0;
            foreach (Rola r in wybraneRole)
            {
                posiadaneRole += (int)Math.Pow(2, Role.IndexOf(r));
            }
            string query = "UPDATE t_Uzytkownicy Set c_login='" + Login + "', c_grupa='"+posiadaneRole+"' Where c_login='" + wybrany.NazwaUzytkownika + "';";
            pobierz_dane(query);
        }

        public void ModyfikujRole(string nazwaRoli, Rola rola, List<Operacja> listaOperacji)
        {
            string query = "UPDATE T_ROLE Set c_rola='" + nazwaRoli + "' Where c_rola='" + rola.Nazwa + "';";
            pobierz_dane(query);

            DeleteAllPrivilegesWithThisRole(rola);
            foreach (Operacja o in listaOperacji)
            {
                var przywilej = new Przywilej(rola.Id, o.IdOperacji, true);
                insertNewPrivilege(przywilej);
            }    
        }
    }
}
