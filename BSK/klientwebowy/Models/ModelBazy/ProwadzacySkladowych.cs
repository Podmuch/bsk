using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace klient.Model
{
    public class ProwadzacySkladowych
    {
        public int ID_Skladowej { get; set; }
        public int ID_Pracownika { get; set; }
        public ProwadzacySkladowych(int idSkladowej, int idPracownika)
        {
            ID_Skladowej = idSkladowej;
            ID_Pracownika = idPracownika;
        }
    }
}
