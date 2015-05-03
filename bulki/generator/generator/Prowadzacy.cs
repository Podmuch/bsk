using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    internal class Prowadzacy
    {
        public int id;
        public string pesel;
        public string tytul;
        public int staz;
        public int wiek;
        public string imie;
        public string nazwisko;
        public string katedra;
        public string wydzial;

        public Prowadzacy(int id,string pesel, string tytul, int staz, int wiek, string imie, string nazwisko, string katedra, string wydzial)
        {
            this.id = id;
            this.pesel = pesel;
            this.tytul = tytul;
            this.staz = staz;
            this.wiek = wiek;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.katedra = katedra;
            this.wydzial = wydzial;
        }
        public String toString2()
        {
            return id.ToString() + ";" + pesel + ";" + tytul + ";" + staz.ToString() + ";" + wiek.ToString() + ";" + imie + ";" + nazwisko + ";" + katedra + ";" + wydzial + "\r\n";
        }
    }

}
