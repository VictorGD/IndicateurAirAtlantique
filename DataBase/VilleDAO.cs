using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndicateurAirAtlantique.DataBase
{
    class VilleDAO : CommonDAO
    {
        public IEnumerable<string> GetAll()
        {
            return db.Ville.Select(x => x.nom);
        }
        public double Distance(double lat1, double lat2, double lon1, double lon2)
        {
            var rlat1 = Math.PI * lat1 / 180;
            var rlat2 = Math.PI * lat2 / 180;
            var rlon1 = Math.PI * lon1 / 180;
            var rlon2 = Math.PI * lon2 / 180;

            var theta = lon1 - lon2;
            var rtheta = Math.PI * theta / 180;

            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.8477;

            return dist;
        }
    }
}
