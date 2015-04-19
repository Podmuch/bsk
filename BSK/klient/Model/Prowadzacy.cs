using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Prowadzacy
    {
        public int Id;
        public int Wiek;
        public string Login;
        public string Haslo;
        public string Imie;
        public string Nazwisko;
        public string Katedra;
        public string Tytul;
        public string Wydzial;
        public List<Rola> Role;
        public Prowadzacy(int id, int wiek, string login, string haslo, string imie, string nazwisko, string katedra, string tytul, string wydzial)
        {
            Id = id;
            Wiek = wiek;
            Login = login;
            Haslo = haslo;
            Imie = imie;
            Nazwisko = nazwisko;
            Katedra = katedra;
            Tytul = tytul;
            Wydzial = wydzial;
            Role = new List<Rola>();
        }

        public void DodajRole(List<Rola> role)
        {
            Role.AddRange(role);
        }
    }
}
