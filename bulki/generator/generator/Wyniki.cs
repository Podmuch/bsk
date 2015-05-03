using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Wyniki
    {
        public int indeksstudenta;
        public int idprzedmiotu;
        public string nazwaprzedmiotu;        
        public float wynik;
        public Wyniki(int _idprzed,string _nazwa, int _ind, float _w)
        {
            idprzedmiotu = _idprzed;
            nazwaprzedmiotu = _nazwa;
            indeksstudenta = _ind;
            wynik = _w;
        }
        public String toString2()
        {
            int jednosci = (int)wynik;
            int po_przecinku = (int)((wynik - jednosci)*10);
            return indeksstudenta.ToString() + ";" + nazwaprzedmiotu + ";" + jednosci + "." + po_przecinku + "\r\n";
        }
    }
}
