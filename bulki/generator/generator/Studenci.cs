using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace generator
{
    class Studenci
    {
        public int nrindeksu;
        public string pesel;
        public string imie;
        public string nazwisko;
        public int rok;
        public int semestr;
        public int dlugects;
        public List<Wyniki> wyniki; 
        public Studenci(int _nr, string _pesel, string _imie, string _nazwisko, int _rok, int _semestr, int _dlugects)
        {
            nrindeksu = _nr;
            pesel = _pesel;
            imie = _imie;
            nazwisko = _nazwisko;
            rok = _rok;
            semestr = _semestr;
            dlugects = _dlugects;
            wyniki = new List<Wyniki>();
        }
        public String toString2()
        {
            return nrindeksu.ToString() + ";" + pesel + ";" + imie + ";" + nazwisko + ";" + rok.ToString() + ";" + semestr.ToString() + ";" + dlugects.ToString() + "\r\n";
        }
    }
}
