using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Student
    {
        public string NrIndeksu;
        public string Imie;
        public string Nazwisko;
        public string Login;
        public string Haslo;
        public int DlugEcts;
        public int Rok;
        public int Semestr;
        public List<Rola> Role;
        public List<Wynik> Wyniki;
        public Student(string indeks, string imie, string nazwisko, string login, string haslo, int ects, int rok, int semestr)
        {
            NrIndeksu = indeks;
            Imie = imie;
            Nazwisko = nazwisko;
            Login = login;
            Haslo = haslo;
            DlugEcts = ects;
            Rok = rok;
            Semestr = semestr;
            Role = new List<Rola>();
            Wyniki = new List<Wynik>();
        }

        public void DodajRole(List<Rola> role)
        {
            Role.AddRange(role);
        }

        public void DodajWyniki(List<Wynik> wyniki)
        {
            Wyniki = wyniki;
        }
    }
}
