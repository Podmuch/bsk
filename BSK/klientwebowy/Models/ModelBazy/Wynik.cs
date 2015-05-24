using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    public class Wynik
    {
        //public Prowadzacy ProwadzacyKtoryWstawilOcene;
        //public Student StudentKtoryOtrzymalOcene;
        //public SkladowaPrzedmiotu SkladowaPrzedmiotuZKtoregoOcenaZostalaWstawiona;
        public int NrIndeksu { get; set; }
        public string Przedmiot { get; set; }
        public double Ocena { get; set; }
        public Wynik(double ocena, string przedmiot, int idStudenta)
        {
            //ProwadzacyKtoryWstawilOcene = prowadzacy;
            //StudentKtoryOtrzymalOcene = student;
            //SkladowaPrzedmiotuZKtoregoOcenaZostalaWstawiona = skladowaPrzedmiotu;
            Przedmiot = przedmiot;
            NrIndeksu = idStudenta;
            Ocena = ocena;
        }
    }
}
