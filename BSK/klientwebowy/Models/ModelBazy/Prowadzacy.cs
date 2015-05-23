using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Prowadzacy
    {
        public int Id { get; set; }
        public int Wiek { get; set; }
        public int Staz { get; set; }
        public string Pesel { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Katedra { get; set; }
        public string Tytul { get; set; }
        public string Wydzial { get; set; }
        public Prowadzacy(int id, int wiek, int staz, string pesel, string imie, string nazwisko, string katedra, string tytul, string wydzial)
        {
            Id = id;
            Wiek = wiek;
            Staz = staz;
            Pesel = pesel;
            //Login = login;
            //Haslo = haslo;
            Imie = imie;
            Nazwisko = nazwisko;
            Katedra = katedra;
            Tytul = tytul;
            Wydzial = wydzial;
            //Role = new List<Rola>();
        }

        public void DodajRole(List<Rola> role)
        {
            //Role.AddRange(role);
        }
    }
}
