using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    

    class Przywilej
    {
        public int IdRoli;
        public int IdOperacji;
	    public bool Aktywny;
        public Przywilej(int idRoli, int idOperacji, bool aktywny)
        {
            IdRoli = idRoli;
            IdOperacji = idOperacji;
            Aktywny = aktywny;
        }
    }
}
