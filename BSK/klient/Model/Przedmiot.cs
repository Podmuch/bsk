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
        public Prowadzacy GlownyProwadzacy;
        public int IloscEcts;
        public int IloscGodzin;
        public Przedmiot(string nazwa, Prowadzacy prowadzacy, int ects, int godziny)
        {
            Nazwa = nazwa;
            GlownyProwadzacy = prowadzacy;
            IloscEcts = ects;
            IloscGodzin = godziny;
        }
    }
}
