using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    public class Operacja : IEquatable<Operacja>
    {
        public int IdOperacji;
        public bool Aktywna;
        public string NazwaOperacji { get; set; }
        public Operacja(int idOperacji, bool aktywna, string nazwaOperacji)
        {
            IdOperacji = idOperacji;
            Aktywna = aktywna;
            NazwaOperacji = nazwaOperacji;
        }

        public bool Equals(Operacja other)
        {
            return IdOperacji == other.IdOperacji;
        }
    }
}
