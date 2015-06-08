using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndicateurAirAtlantique.DataBase
{
    public sealed class Singleton
    {
        private static Singleton instance = null;
        private static readonly object padlock = new object();

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }

                    return instance;
                }
            }
        }
        public AirAtlantiqueEntities db = new AirAtlantiqueEntities();

        public static void Commit()
        {
            Instance.db.SaveChanges();
        }
    }
}
