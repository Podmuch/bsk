using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Wynik
    {
        //public Prowadzacy ProwadzacyKtoryWstawilOcene;
        //public Student StudentKtoryOtrzymalOcene;
        //public SkladowaPrzedmiotu SkladowaPrzedmiotuZKtoregoOcenaZostalaWstawiona;
        public int IdStudenta;
        public string Przedmiot;
        public double Ocena;
        public Wynik(double ocena, string przedmiot, int idStudenta)
        {
            //ProwadzacyKtoryWstawilOcene = prowadzacy;
            //StudentKtoryOtrzymalOcene = student;
            //SkladowaPrzedmiotuZKtoregoOcenaZostalaWstawiona = skladowaPrzedmiotu;
            Przedmiot = przedmiot;
            IdStudenta = idStudenta;
            Ocena = ocena;
        }
    }
}
