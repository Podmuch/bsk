using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class RodzajSkladowych
    {
        public string nazwa;

        public RodzajSkladowych(string _nazwa)
        {
            nazwa = _nazwa;
        }
        public String toString2()
        {
            return nazwa + "\r\n";
        }
    }
}
