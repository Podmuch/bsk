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
        //public Przedmiot PrzedmiotMacierzysty;
        //public Prowadzacy OsobaOdpowiedzialna;
        public int IloscGodzin;
        public string Przedmiot;
        public int IdOdpowiedzialnego;
        public SkladowaPrzedmiotu(int id, string nazwa, int iloscGodzin, string przedmiot, int idOdpowiedzialnego)
        {
            Id = id;
            Nazwa = nazwa;
            //PrzedmiotMacierzysty = przedmiot;
            //OsobaOdpowiedzialna = odpowiedzialny;
            IloscGodzin = iloscGodzin;
            Przedmiot = przedmiot;
            IdOdpowiedzialnego = idOdpowiedzialnego;
        }
    }
}
