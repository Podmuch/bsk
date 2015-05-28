using klient;
using klient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace klientwebowy.Controllers
{
    public class HomeController : Controller
    {
        private DataBase Baza;
        public ObservableCollection<Rola> Role;
        public ObservableCollection<Uzytkownik> Uzytkownicy;
        public List<Operacja> Operacje;
        public ObservableCollection<Operacja> PozostaleOperacje;
        public ObservableCollection<Operacja> WybraneOperacje;
        public ObservableCollection<Rola> WybraneRole;
        public ObservableCollection<Rola> PozostaleRole;
        private Rola aktualnaRola;
        private Uzytkownik zalogowanyUzytkownik;

        private void Init()
        {
            if (Session["baza"] == null)
            {
                Baza = new DataBase("user123", "haslo123", "localhost", "szkola");
                Session["baza"] = Baza;
            }
            else
            {
                Baza = (DataBase)Session["baza"];
            }
            Role = new ObservableCollection<Rola>(Baza.pobierzRole());
            if(Session["Uzytkownik"]!=null)
            {
                zalogowanyUzytkownik = (Uzytkownik)Session["Uzytkownik"];
                zalogowanyUzytkownik = Baza.pobierzUzytkownikow(" where c_id_uzytkownika = " + zalogowanyUzytkownik.IdUzytkownika).First();
                Session["Uzytkownik"] = zalogowanyUzytkownik;
                ViewBag.Uzytkownik = zalogowanyUzytkownik;
            }
            if (Session["AktualnaRola"] != null)
            {
                aktualnaRola = (Rola)Session["AktualnaRola"];
                ViewBag.Rola = aktualnaRola;
            }
            if(Session["Operacje"]!=null)
            {
                if (aktualnaRola.Nazwa.ToLower() == "administrator")
                {
                    Operacje = Baza.pobierzOperacje();
                }
                else
                {
                    Operacje = Baza.pobierzOperacje(" join t_przywileje on c_Fk_id_operacji = c_id_operacji where c_Fk_id_roli = " + aktualnaRola.Id);
                }
                Session["Operacje"] = Operacje;
                ViewBag.Operacje = Operacje;
            }
            if(Session["Uzytkownicy"]!=null)
            {
                Uzytkownicy = new ObservableCollection<Uzytkownik>(Baza.pobierzUzytkownikow());
                Session["Uzytkownicy"] = Uzytkownicy;
            }
            if(Session["Blad"]!=null)
            {
                ViewBag.Blad = (string)Session["Blad"];
            }
            if (Session["DostepneTabele"] != null)
            {
                Session["DostepneTabele"] = WybierzDostepneTabele();
                ViewBag.DostepneTabele = WybierzDostepneTabele();
            }
        }

        public ActionResult Index(string name, string par1 = "brak", int par2 = 1, string par3 = "brak", string par4 = "brak", string par5 = "brak")
        {
            Init();
            ViewBag.Role = Role;
            if (zalogowanyUzytkownik != null)
            {
                if (aktualnaRola != null)
                {
                    Baza.keepSession(zalogowanyUzytkownik.IdUzytkownika, aktualnaRola.Id);
                }
                else
                {
                    ObservableCollection<Rola> roleUzytkownika = new ObservableCollection<Rola>();
                    for (int i = 0; i < Role.Count; i++ )
                    {
                        if ((zalogowanyUzytkownik.Grupa >> i)% 2 == 1)
                        {
                            roleUzytkownika.Add(Role[i]);
                        }
                    }
                    ViewBag.Role = roleUzytkownika;
                }
            }
            string nazwaTabeli = par1;
            ViewBag.NumerStrony = par2;
            //ViewBag.Blad = null;
            switch (nazwaTabeli)
            {
                case "Studenci":
                    if (par3 == "Delete" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Studenci_delete")))
                    {
                        if (par4 != "brak")
                        {
                            if (!Baza.executeQuery("DELETE FROM T_STUDENCI WHERE c_Nr_indeksu = " + par3))
                            {
                                ViewBag.Blad = "Nie można usunąć rekordu, gdyż istnieją inne z nim powiązane";
                            }
                        }
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Studenci"));
                    ViewBag.DaneTabeli = Baza.pobierzStudentow();
                    break;
                case "Prowadzacy":
                    if (par3 == "Delete" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Prowadzacy_delete")))
                    {
                        if (par4 != "brak")
                        {
                            if (!Baza.executeQuery("DELETE FROM T_PROWADZACY WHERE c_Id_pracownika = " + par4))
                            {
                                ViewBag.Blad = "Nie można usunąć rekordu, gdyż istnieją inne z nim powiązane";
                            }
                        }
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Prowadzacy"));
                    ViewBag.DaneTabeli = Baza.pobierzProwadzacych();
                    break;
                case "Wyniki":
                    if (par3 == "Delete" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Wyniki_delete")))
                    {
                        if (par4 != "brak" && par5 != "brak")
                        {
                            if (!Baza.executeQuery("DELETE FROM T_WYNIKI WHERE c_Fk_Student = " + par4 + " AND c_Fk_Przedmiot = '" + par5 + "'"))
                            {
                                ViewBag.Blad = "Nie można usunąć rekordu, gdyż istnieją inne z nim powiązane";
                            }
                        }
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Wyniki"));
                    ViewBag.DaneTabeli = Baza.pobierzWyniki();
                    break;
                case "Przedmioty":
                    if (par3 == "Delete" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Przedmioty_delete")))
                    {
                        if (par4 != "brak")
                        {
                            if (!Baza.executeQuery("DELETE FROM T_PRZEDMIOTY WHERE c_Nazwa = '" + par4 + "'"))
                            {
                                ViewBag.Blad = "Nie można usunąć rekordu, gdyż istnieją inne z nim powiązane";
                            }
                        }
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Przedmioty"));
                    ViewBag.DaneTabeli = Baza.pobierzPrzedmioty();
                    break;
                case "ProwadzacySkladowych":
                    if (par3 == "Delete" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych_delete")))
                    {
                        if (par4 != "brak" && par5 != "brak")
                        {
                            if (!Baza.executeQuery("DELETE FROM t_Prowadzacy_skladowych_czesci WHERE c_Fk_id_skladowej = " + par4 + " AND c_Fk_id_pracownika = " + par5))
                            {
                                ViewBag.Blad = "Nie można usunąć rekordu, gdyż istnieją inne z nim powiązane";
                            }
                        }
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych"));
                    ViewBag.DaneTabeli = Baza.pobierzProwadzacychSkladowych();
                    break;
                case "SkladowePrzedmiotow":
                    if (par3 == "Delete" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow_delete")))
                    {
                        if (par4 != "brak")
                        {
                            if (!Baza.executeQuery("DELETE FROM t_Skladowe_przedmiotow WHERE c_Id_skladowej = " + par4))
                            {
                                ViewBag.Blad = "Nie można usunąć rekordu, gdyż istnieją inne z nim powiązane";
                            }
                        }
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow"));
                    ViewBag.DaneTabeli = Baza.pobierzSkladowePrzedmiotow();
                    break;
            }
            return View();
        }

        public ActionResult Modyfikuj()
        {
            Init();
            return View();
        }

        public ActionResult Dodaj()
        {
            Init();
            return View();
        }

        public ActionResult Dodawanie(string name, string par1 = "brak", string par2 = "brak", string par3 = "brak")
        {
            Init();
            if(zalogowanyUzytkownik != null)
            {
                Baza.keepSession(zalogowanyUzytkownik.IdUzytkownika, aktualnaRola.Id);
            }
            ViewBag.Role = Role;
            ViewBag.Baza = Baza;
            ViewBag.par1 = par2;
            ViewBag.par2 = par3;
            string nazwaTabeli = par1;
            ViewBag.Blad = null;
            switch (nazwaTabeli)
            {
                case "Studenci":
                    if (par2 == "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Studenci_insert")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Studenci_insert"));
                        ViewBag.Dana = null;
                    }
                    else if(par2 != "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Studenci_update")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Studenci_update"));
                        ViewBag.Dana = Baza.pobierzStudentow(" WHERE c_Nr_indeksu = " + par2).First();
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    break;
                case "Prowadzacy":
                    if (par2 == "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Prowadzacy_insert")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Prowadzacy_insert"));
                        ViewBag.Dana = null;
                    }
                    else if (par2 != "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Prowadzacy_update")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Prowadzacy_update"));
                        ViewBag.Dana = Baza.pobierzProwadzacych(" WHERE c_Id_pracownika = " + par2).First();
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    break;
                case "Wyniki":
                    if (par2 == "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Wyniki_insert")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Wyniki_insert"));
                        ViewBag.Dana = null;
                    }
                    else if (par2 != "brak" && par3 != "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Wyniki_update")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Wyniki_update"));
                        ViewBag.Dana = Baza.pobierzWyniki(" WHERE c_Fk_Student = " + par2 + " AND c_Fk_Przedmiot = '" + par3 + "'").First();
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    break;
                case "Przedmioty":
                    if (par2 == "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Przedmioty_insert")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Przedmioty_insert"));
                        ViewBag.Dana = null;
                    }
                    else if (par2 != "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Przedmioty_update")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Przedmioty_update"));
                        ViewBag.Dana = Baza.pobierzPrzedmioty(" WHERE c_Nazwa = '" + par2 + "'").First();
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    break;
                case "ProwadzacySkladowych":
                    if (par2 == "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych_insert")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych_insert"));
                        ViewBag.Dana = null;
                    }
                    else if (par2 != "brak" && par3 != "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych_update")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych_update"));
                        ViewBag.Dana = Baza.pobierzProwadzacychSkladowych(" WHERE c_Fk_id_skladowej = " + par2 + " AND c_Fk_id_pracownika = " + par3).First();
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    break;
                case "SkladowePrzedmiotow":
                    if (par2 == "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow_insert")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow_insert"));
                        ViewBag.Dana = null;
                    }
                    else if (par2 != "brak" && Operacje.Exists((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow_update")))
                    {
                        ViewBag.Przywilej = Operacje.Find((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow_update"));
                        ViewBag.Dana = Baza.pobierzSkladowePrzedmiotow(" WHERE c_Id_skladowej = " + par2).First();
                    }
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    break;
            }
            return View();
        }
        public ActionResult Wylogowywanie(string name)
        {
            Init();
            if(zalogowanyUzytkownik != null)
            {
                if (aktualnaRola != null)
                {
                    Baza.destroySession(zalogowanyUzytkownik.IdUzytkownika, aktualnaRola.Id);
                }
            }
            Session["Uzytkownik"] = null;
            Session["AktualnaRola"] = null;
            Session["Operacje"] = null;
            Session["Uzytkownicy"] = null;
            Session["Blad"] = null;
            Session["DostepneTabele"] = null;
            return View();
        }

        public ActionResult WeryfikujLogowanie(string name, string par1 = "brak", string par2 = "brak", string par3 = "brak")
        {
            Init();
            string login = "", haslo = "", rola = "";
            if (zalogowanyUzytkownik == null)
            {
                if (!string.IsNullOrEmpty(Request["login"]))
                    login = Request["login"].Trim();
                if (!string.IsNullOrEmpty(Request["haslo"]))
                    haslo = PobierzHashHasla(Request["haslo"]);
            }
            else
            {
                login = zalogowanyUzytkownik.NazwaUzytkownika;
                haslo = zalogowanyUzytkownik.Haslo;
            }
            if (!string.IsNullOrEmpty(Request["rola"]))
                rola = Request["rola"].ToLower();
            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(haslo) && CzyIstniejeUzytkownikODanymLoginie(login) &&
               CzyHasloJestPrawidlowe(login, haslo))
            {
                Uzytkownik uzytkownik = Baza.pobierzUzytkownikow(" WHERE C_LOGIN = '" + login + "'").First();
                Session["Uzytkownik"] = uzytkownik;
                if (!string.IsNullOrEmpty(rola) && (uzytkownik.Grupa >> (Role.IndexOf(Role.First((r)=>r.Nazwa.ToLower().Equals(rola))))) % 2 == 1)
                {
                    zalogowanyUzytkownik = uzytkownik;
                    aktualnaRola = Baza.pobierzRole(" WHERE C_ROLA = '" + rola + "'").First();
                    if (Baza.createSession(zalogowanyUzytkownik.IdUzytkownika, aktualnaRola.Id))
                    {
                        Session["AktualnaRola"] = aktualnaRola;
                        if (rola == "administrator")
                        {
                            Operacje = Baza.pobierzOperacje();
                            Session["Operacje"] = Operacje;
                            Uzytkownicy = new ObservableCollection<Uzytkownik>(Baza.pobierzUzytkownikow());
                            Session["Uzytkownicy"] = Uzytkownicy;
                        }
                        else
                        {
                            Operacje = Baza.pobierzOperacje(" join t_przywileje on c_Fk_id_operacji = c_id_operacji where c_Fk_id_roli = " + aktualnaRola.Id);
                            Session["Operacje"] = Operacje;
                            Session["DostepneTabele"] = WybierzDostepneTabele();
                        }
                        ViewBag.Blad = null;
                        Session["Blad"] = (string)ViewBag.Blad;
                    }
                    else
                    {
                        ViewBag.Blad= "Istnieje aktywna sesja dla tego użytkownika.";
                        Session["Blad"] = (string)ViewBag.Blad;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(rola))
                    { 
                        ViewBag.Blad = "Użytkownik nie posiada wybranej roli";
                        Session["Blad"] = (string)ViewBag.Blad;
                    }
                }
            }
            else
            {
                ViewBag.Blad = "Błędne hasło lub login";
                Session["Blad"] = (string)ViewBag.Blad;
            }
            return View();
        }

        private string PobierzHashHasla(string haslo)
        {
            byte[] result = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(haslo));
            StringBuilder s = new StringBuilder();
            foreach (byte b in result)
                s.Append(b.ToString("x2").ToLower());
            return s.ToString();
        }

        private bool CzyIstniejeUzytkownikODanymLoginie(string Login)
        {
            DataTable table = Baza.pobierz_dane("SELECT C_LOGIN FROM T_UZYTKOWNICY");
            bool czyIstnieje = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][0].Equals(Login))
                    czyIstnieje = true;
            }
            return czyIstnieje;
        }

        private bool CzyHasloJestPrawidlowe(string Login, string Password)
        {
            DataTable table = Baza.pobierz_dane("SELECT C_HASLO FROM T_UZYTKOWNICY WHERE C_LOGIN = '" + Login + "'");
            bool czyIstnieje = false;
            if (table.Rows.Count>0)
            {
                czyIstnieje = table.Rows[0][0].Equals(Password);
            } 
            return czyIstnieje;
        }

        private List<Tabela> WybierzDostepneTabele()
        {
            List<Tabela> listaTabel = new List<Tabela>();
            var operacjeSelect = Operacje.FindAll((o) => o.NazwaOperacji.Contains("select"));
            for (int i = 0; i < operacjeSelect.Count; i++)
            {
                listaTabel.Add(new Tabela(operacjeSelect[i].NazwaOperacji.Contains("Prowadzacy_Skladowych") ? "ProwadzacySkladowych" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Przedmioty") ? "Przedmioty" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Studenci") ? "Studenci" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Wyniki") ? "Wyniki" :
                                          operacjeSelect[i].NazwaOperacji.Contains("Prowadzacy") ? "Prowadzacy" : "SkladowePrzedmiotow"));
            }
            return listaTabel;
        }
    }
}