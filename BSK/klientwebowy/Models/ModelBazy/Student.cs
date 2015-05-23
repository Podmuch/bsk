using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Student
    {
        public int NrIndeksu { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Pesel { get; set; }
        //public string Login;
        //public string Haslo;
        public int DlugEcts { get; set; }
        public int Rok { get; set; }
        public int Semestr { get; set; }
        //public List<Rola> Role;
        //public List<Wynik> Wyniki;
        public Student(int indeks, string imie, string nazwisko, string pesel,/*string login, string haslo,*/ int ects, int rok, int semestr)
        {
            NrIndeksu = indeks;
            Imie = imie;
            Nazwisko = nazwisko;
            Pesel = pesel;
            //Login = login;
            //Haslo = haslo;
            DlugEcts = ects;
            Rok = rok;
            Semestr = semestr;
            //Role = new List<Rola>();
            //Wyniki = new List<Wynik>();
        }

        public void DodajRole(List<Rola> role)
        {
            //Role.AddRange(role);
        }

        public void DodajWyniki(List<Wynik> wyniki)
        {
            //Wyniki = wyniki;
        }
    }
}
