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
            if(Session["role"] == null)
            {
                Role = new ObservableCollection<Rola>(Baza.pobierzRole());
                Session["role"] = Role;
            }
            else
            {
                Role = (ObservableCollection<Rola>)Session["role"];
            }
            if(Session["Uzytkownik"]!=null)
            {
                zalogowanyUzytkownik = (Uzytkownik)Session["Uzytkownik"];
                ViewBag.Uzytkownik = zalogowanyUzytkownik;
            }
            if(Session["Operacje"]!=null)
            {
                Operacje = (List<Operacja>)Session["Operacje"];
                ViewBag.Operacje = Operacje;
            }
            if(Session["Uzytkownicy"]!=null)
            {
                Uzytkownicy = (ObservableCollection<Uzytkownik>)Session["Uzytkownicy"];
            }
            if(Session["AktualnaRola"]!=null)
            {
                aktualnaRola = (Rola)Session["AktualnaRola"];
                ViewBag.Rola = aktualnaRola;
            }
            if(Session["Blad"]!=null)
            {
                ViewBag.Blad = (string)Session["Blad"];
            }
            if (Session["DostepneTabele"] != null)
            {
                ViewBag.DostepneTabele = Session["DostepneTabele"];
            }
        }

        public ActionResult Index(string name, string par1 = "brak", int par2 = 1)
        {
            Init();
            ViewBag.Role = Role;
            string nazwaTabeli = par1;
            ViewBag.NumerStrony = par2;
            switch (nazwaTabeli)
            {
                case "Studenci":
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Studenci"));
                    ViewBag.DaneTabeli = Baza.pobierzStudentow();
                    break;
                case "Prowadzacy":
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Prowadzacy"));
                    ViewBag.DaneTabeli = Baza.pobierzProwadzacych();
                    break;
                case "Wyniki":
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Wyniki"));
                    ViewBag.DaneTabeli = Baza.pobierzWyniki();
                    break;
                case "Przedmioty":
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Przedmioty"));
                    ViewBag.DaneTabeli = Baza.pobierzPrzedmioty();
                    break;
                case "ProwadzacySkladowych":
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Prowadzacy_Skladowych"));
                    ViewBag.DaneTabeli = Baza.pobierzProwadzacychSkladowych();
                    break;
                case "SkladowePrzedmiotow":
                    ViewBag.NazwaWybranejTabeli = nazwaTabeli;
                    ViewBag.PrzywilejeDanejTabeliTabela = Operacje.FindAll((o) => o.NazwaOperacji.Contains("t_Skladowe_Przedmiotow"));
                    ViewBag.DaneTabeli = Baza.pobierzSkladowePrzedmiotow();
                    break;
            }
            return View();
        }

        public ActionResult Wylogowywanie(string name)
        {
            Init();
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
            string login = par1.Trim();
            string haslo = PobierzHashHasla(par2);
            string rola = par3.ToLower();
            Init();
            if (CzyIstniejeUzytkownikODanymLoginie(login) &&
               CzyHasloJestPrawidlowe(login, haslo))
            {
                Uzytkownik uzytkownik = Baza.pobierzUzytkownikow(" WHERE C_LOGIN = '" + login + "'").First();
                if ((uzytkownik.Grupa >> (Role.IndexOf(Role.First((r)=>r.Nazwa.ToLower().Equals(rola))))) % 2 == 1)
                {
                    zalogowanyUzytkownik = uzytkownik;
                    aktualnaRola = Baza.pobierzRole(" WHERE C_ROLA = '" + rola + "'").First();
                    if (Baza.createSession(zalogowanyUzytkownik.IdUzytkownika, aktualnaRola.Id))
                    {
                        Session["Uzytkownik"] = zalogowanyUzytkownik;
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
                    ViewBag.Blad = "Użytkownik nie posiada wybranej roli";
                    Session["Blad"] = (string)ViewBag.Blad;
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