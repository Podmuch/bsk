using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    public class Tabela
    {
        public string NazwaTabeli { get; set; }
        public Tabela(string nazwa)
        {
            NazwaTabeli = nazwa;
        }
    }
}
