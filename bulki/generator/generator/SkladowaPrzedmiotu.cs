using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class SkladowaPrzedmiotu
    {
        public int idskladowej;
        public string nazwaprzedmiotu;
        public string rodzajskladowej;
        public int iloscgodzin;
        public int idosobyodpowiedzialnej;

        public SkladowaPrzedmiotu(int _id, string _nazwa, string _rodzaj, int _godz, int _idodp)
        {
            idskladowej = _id;
            nazwaprzedmiotu = _nazwa;
            rodzajskladowej = _rodzaj;
            iloscgodzin = _godz;
            idosobyodpowiedzialnej = _idodp;
        }
        public String toString2()
        {
            return idskladowej.ToString() + ";" + nazwaprzedmiotu + ";" + rodzajskladowej + ";" + iloscgodzin.ToString() + ";" + idosobyodpowiedzialnej.ToString() + "\r\n";
        }
    }
}
