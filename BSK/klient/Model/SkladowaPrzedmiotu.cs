using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class SkladowaPrzedmiotu
    {
        public int Id;
        public string Nazwa;
        public Przedmiot PrzedmiotMacierzysty;
        public Prowadzacy OsobaOdpowiedzialna;
        public SkladowaPrzedmiotu(int id, string nazwa, Przedmiot przedmiot, Prowadzacy odpowiedzialny)
        {
            Id = id;
            Nazwa = nazwa;
            PrzedmiotMacierzysty = przedmiot;
            OsobaOdpowiedzialna = odpowiedzialny;
        }
    }
}
