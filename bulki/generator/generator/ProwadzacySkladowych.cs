using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class ProwadzacySkladowych
    {
        public int idskladowej;
        public int idprowadzacego;

        public ProwadzacySkladowych(int _idskl, int _idprow)
        {
            idskladowej = _idskl;
            idprowadzacego = _idprow;
        }
        public String toString2()
        {
            return idskladowej.ToString() + ";" + idprowadzacego.ToString() + "\r\n";
        }
    }
}
