using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    public class Rola
    {
        public int Id;
        public string Nazwa  { get; set; }
        public bool Aktywna;
        public int Grupy_ktorych_dotyczy;
        public Rola(int id, string nazwa, bool aktywna, int grupy_ktorych_dotyczy)
        {
            Id = id;
            Nazwa = nazwa;
            Aktywna = aktywna;
            Grupy_ktorych_dotyczy = grupy_ktorych_dotyczy;
        }
    }
}
