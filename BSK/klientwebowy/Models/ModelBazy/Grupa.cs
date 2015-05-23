using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    public class Grupa
    {
        public int Id;
        public string NazwaGrupy { get; set; }

        public Grupa(int id, string nazwaGrupy)
        {
            Id = id;
            NazwaGrupy = nazwaGrupy;
        }
    }
}
