using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Przedmiot
    {
        public string Nazwa { get; set; }
        //public Prowadzacy GlownyProwadzacy;
        public int IdProwadzacego { get; set; }
        public int Semestr { get; set; }
        public int IloscEcts { get; set; }
        public int IloscGodzin { get; set; }
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
