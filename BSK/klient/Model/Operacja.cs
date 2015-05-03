using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Operacja
    {
        public int IdOperacji;
        public bool Aktywna;
        public string NazwaOperacji;
        public Operacja(int idOperacji, bool aktywna, string nazwaOperacji)
        {
            IdOperacji = idOperacji;
            Aktywna = aktywna;
            NazwaOperacji = nazwaOperacji;
        }
    }
}
