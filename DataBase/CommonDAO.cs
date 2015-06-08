using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndicateurAirAtlantique.DataBase
{
    partial class CommonDAO
    {
        protected AirAtlantiqueEntities db
        {
            get { return Singleton.Instance.db; }
        }
        public void Commit() { Singleton.Commit(); }
    }
}
