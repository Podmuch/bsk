using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Przedmiot
    {
        public string Nazwa;
        //public Prowadzacy GlownyProwadzacy;
        public int IdProwadzacego;
        public int Semestr;
        public int IloscEcts;
        public int IloscGodzin;
        public Przedmiot(string nazwa, int idProwadzacego, int semestr, int ects, int godziny)
        {
            Nazwa = nazwa;
            //GlownyProwadzacy = prowadzacy;
            IdProwadzacego = idProwadzacego;
            Semestr = semestr;
            IloscEcts = ects;
            IloscGodzin = godziny;
        }
    }
}
