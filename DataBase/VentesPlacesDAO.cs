using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndicateurAirAtlantique.DataBase
{
    class VentesPlacesDAO : CommonDAO
    {
        public IEnumerable<string> GetAll()
        {
            return db.Ville.Select(x => x.nom);
        }

        public int GetTotalPlacesVendues()
        {
            return db.VentesPlaces.Sum(x => x.nbPlace);
        }
        public double GetTotalCA()
        {
            return db.VentesPlaces.Sum(x => (x.prix * x.nbPlace));
        }
    }
}



