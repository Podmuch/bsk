using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Arkusz2
    {
        public int idstudenta;
        public int idterminu;
        public string terminy;
        public float uzyskanywynik;
        public float mozliwepunktydozdobycia;
        public string data;
        //"2012-10-01 00:00:00.000"
        public Arkusz2(int _ids, int _idter, string ter, float uzyskpkt, float mozliwpkt, string _data)
        {
            idstudenta = _ids;
            idterminu = _idter;
            terminy = ter;
            uzyskanywynik = uzyskpkt;
            mozliwepunktydozdobycia = mozliwpkt;
            data = _data;
        }
        public string toString2()
        {
            return idstudenta.ToString() + ";" + idterminu.ToString() + ";" + terminy + ";" + uzyskanywynik.ToString() + ";" + mozliwepunktydozdobycia.ToString() + ";" + data + "\r\n";
        }
    }
}
