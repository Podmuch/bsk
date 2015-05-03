using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    class Uzytkownik
    {
        /*
         * 	c_id_uzytkownika integer IDENTITY(1,1) PRIMARY KEY, 
	c_Fk_nr_indeksu integer REFERENCES t_Studenci(c_Nr_indeksu),
	c_Fk_id_pracownika integer REFERENCES t_Prowadzacy(c_Id_pracownika),
	c_nazwa varchar(30) not null,
	c_grupa integer not null default 1,  -- do jakiej grupy nalezy uzytkownik
	--c_status integer not null default 1, -- czy user jest aktywny/usuniety/wylaczony itd
	c_haslo varchar(50) not null, -- sha1	
         * 
         */

        public int IdUzytkownika;
        public int NrIndeksu;
        public int IdProwadzacego;
        public string NazwaUzytkownika;
        public int Grupa;
        public string Haslo;

        public Uzytkownik(int idUzytkownika, int nrIndeksu, int idProwadzacego, string nazwaUzytkownika, int grupa, string haslo)
        {
            IdUzytkownika = idUzytkownika;
            NrIndeksu = nrIndeksu;
            IdProwadzacego = idProwadzacego;
            NazwaUzytkownika = nazwaUzytkownika;
            Grupa = grupa;
            Haslo = haslo;
        }
    }
}
