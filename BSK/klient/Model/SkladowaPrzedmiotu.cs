using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class SkladowaPrzedmiotu
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        //public Przedmiot PrzedmiotMacierzysty;
        //public Prowadzacy OsobaOdpowiedzialna;
        public int IloscGodzin { get; set; }
        public string Przedmiot { get; set; }
        public int IdOdpowiedzialnego { get; set; }
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
