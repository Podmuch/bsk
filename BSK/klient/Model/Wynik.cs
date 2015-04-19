using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Wynik
    {
        public Prowadzacy ProwadzacyKtoryWstawilOcene;
        public Student StudentKtoryOtrzymalOcene;
        public SkladowaPrzedmiotu SkladowaPrzedmiotuZKtoregoOcenaZostalaWstawiona;
        public float Wynik;
        public Wynik(Prowadzacy prowadzacy, Student student, SkladowaPrzedmiotu skladowaPrzedmiotu, float wynik)
        {
            ProwadzacyKtoryWstawilOcene = prowadzacy;
            StudentKtoryOtrzymalOcene = student;
            SkladowaPrzedmiotuZKtoregoOcenaZostalaWstawiona = skladowaPrzedmiotu;
            Wynik = wynik;
        }
    }
}
