using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndicateurAirAtlantique.DataBase
{
    class VolDAO : CommonDAO
    {
        public IEnumerable<string> GetAll()
        {
            return db.Vol.Select(x => x.villeDepart + x.villeArrive);
        }

        public IEnumerable<string> TopUsed()
        {
            return db.Frequentation.Select(x => x.villeDepart +" - "+ x.villeArrive +" \t: "+ SqlFunctions.StringConvert(x.tauxRemplissage*100)+"%");

        }

        public IEnumerable<string> GetRecherche(int ID)
        {
            return db.Vol.Where(x => x.id == ID).
                Select(x => new{ x.villeDepart, x.villeArrive, x.dateVol}).ToList().Select(t=>string.Format("{0} - {1} \t :\t {2:f}", t.villeDepart, t.villeArrive, t.dateVol));
        }

        public IEnumerable<int> GetID(string villeD, string villeA)
        {
            return db.Vol.Where(x => x.villeDepart == villeD && x.villeArrive == villeA).
                Select(x => x.id);
        }

        public string Afficher(int ID)
        {
            string VilleDepart = "";
            foreach (string vD in db.Vol.Where(x => x.id == ID).Select(x => x.villeDepart)) { VilleDepart = VilleDepart+vD;};

            string VilleArrive = "";
            foreach (string vD in db.Vol.Where(x => x.id == ID).Select(x => x.villeArrive)) { VilleArrive = VilleArrive + vD; };

            DateTime dateVol = db.Vol.FirstOrDefault(x => x.id == ID).dateVol;

            //places economiques
            int nbPlaceEco= 0;
            foreach (int a in db.Capacite.Where(x => x.idAvion == db.Vol.FirstOrDefault(a => a.id == ID).idAvion && x.idTypePlace == 1).Select(x => x.nbPlace))
            { nbPlaceEco = nbPlaceEco+a; }

            int nbPlaceEcoVendues = 0;
            foreach (int a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 1).Select(x => x.nbPlace))
            { nbPlaceEcoVendues = nbPlaceEcoVendues + a; }

            float placeEcoPrix = 0;
            foreach (float a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 1).Select(x => x.prix))
            { placeEcoPrix = placeEcoPrix + a; }

            float ecoCA = nbPlaceEcoVendues * placeEcoPrix;

            //places ecoPremiums
            int nbPlaceEcoP= 0;
            foreach (int nb in db.Capacite.Where(x => x.idAvion == db.Vol.FirstOrDefault(a => a.id == ID).idAvion && x.idTypePlace == 2).Select(x => x.nbPlace))
            { nbPlaceEcoP = nbPlaceEcoP+nb; }

            int nbPlaceEcoPVendues = 0;
            foreach (int a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 2).Select(x => x.nbPlace))
            { nbPlaceEcoPVendues = nbPlaceEcoPVendues + a; }

            float placeEcoPPrix = 0;
            foreach (float a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 2).Select(x => x.prix))
            { placeEcoPPrix = placeEcoPPrix + a; }

            float ecoPCA = nbPlaceEcoPVendues * placeEcoPPrix;

            //places affaires
            int nbPlaceA= 0;
            foreach (int nb in db.Capacite.Where(x => x.idAvion == db.Vol.FirstOrDefault(a => a.id == ID).idAvion && x.idTypePlace == 3).Select(x => x.nbPlace))
            { nbPlaceA = nbPlaceA+nb; }

            int nbPlaceAVendues = 0;
            foreach (int a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 3).Select(x => x.nbPlace))
            { nbPlaceAVendues = nbPlaceAVendues + a; }

            float placeAPrix = 0;
            foreach (float a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 3).Select(x => x.prix))
            { placeAPrix = placeAPrix + a; }

            float AffCA = nbPlaceAVendues * placeAPrix;

            //places première classe
            int nbPlaceP= 0;
            foreach (int nb in db.Capacite.Where(x => x.idAvion == db.Vol.FirstOrDefault(a => a.id == ID).idAvion && x.idTypePlace == 4).Select(x => x.nbPlace))
            { nbPlaceP = nbPlaceP+nb; }

            int nbPlacePVendues = 0;
            foreach (int a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 4).Select(x => x.nbPlace))
            { nbPlacePVendues = nbPlacePVendues + a; }

            float placePPrix = 0;
            foreach (float a in db.VentesPlaces.Where(x => x.idVol == ID && x.idTypePlace == 4).Select(x => x.prix))
            { placePPrix = placePPrix + a; }

            float PreCA = nbPlacePVendues * placePPrix;

            //info sur le vol
            float TauxRemplissage = (nbPlaceEcoVendues+nbPlaceEcoPVendues+nbPlaceAVendues+nbPlacePVendues)*100/(nbPlaceEco+nbPlaceEcoP+nbPlaceA+nbPlaceP);
            float CA_total = ecoCA + ecoPCA + AffCA + PreCA;


            int idAvion = db.Vol.FirstOrDefault(x=> x.id == ID).idAvion;

            return string.Format(
            "Ville de Départ : \t{0}"+
            "\nVille Arrivée : \t{1}"+
            "\nDate du vol : \t{2:f}\n"+
            "\nClasse Economique : "+
                "\n\tNb de Places : \t{3}" +
                "\n\tPlaces Vendues : \t{4}" +
                "\n\tPrix de la place : \t{5}" +"€"+
                "\n\tChiffre d'Affaire : \t{6}" + "€\n" +
            "\nClasse EcoPremium : " +
                "\n\tNb de Places : \t{7}" +
                "\n\tPlaces Vendues : \t{8}" +
                "\n\tPrix de la place : \t{9}" + "€" +
                "\n\tChiffre d'Affaire : \t{10}" + "€\n" +
            "\nClasse Affaire : " +
                "\n\tNb de Places : \t{11}" +
                "\n\tPlaces Vendues : \t{12}" +
                "\n\tPrix de la place : \t{13}" + "€" +
                "\n\tChiffre d'Affaire : \t{14}" + "€\n" +
            "\nPremière Classe : " +
                "\n\tNb de Places : \t{15}" +
                "\n\tPlaces Vendues : \t{16}" +
                "\n\tPrix de la place : \t{17}" + "€" +
                "\n\tChiffre d'Affaire : \t{18}" + "€" +
            "\n"+
            "\nTaux de remplissage : \t{19}"+"%"+
            "\nChiffre d'Affaire généré : \t{20}" + "€" +
            "\nBénéfices Approximatif : \t{21}€",
            VilleDepart , VilleArrive, dateVol,
            //places Economiques
            nbPlaceEco, nbPlaceEcoVendues, placeEcoPrix, ecoCA,
            //places EcoPremium
            nbPlaceEcoP, nbPlaceEcoPVendues, placeEcoPPrix,ecoPCA,
            //places affaires
            nbPlaceA, nbPlaceAVendues, placeAPrix, AffCA,
            //place premiere classe
            nbPlaceP, nbPlacePVendues, placePPrix, PreCA,
            //info sur le vol
            TauxRemplissage, CA_total, this.GetBenefices(ID, idAvion,VilleDepart,VilleArrive)
            );
            
        }

        public double GetBenefices(double idVol, int idAvion,string VilleDepart, string VilleArrive)
        {
            VilleDAO ville = new VilleDAO();
            double VilleD_Latitude = db.Ville.FirstOrDefault(x => x.nom == VilleDepart).latitude;
            double VilleD_Longitude = db.Ville.FirstOrDefault(x => x.nom == VilleDepart).longitude;
            double VilleA_Latitude = db.Ville.FirstOrDefault(x => x.nom == VilleArrive).latitude;
            double VilleA_Longitude = db.Ville.FirstOrDefault(x => x.nom == VilleArrive).longitude;
            double dist = Math.Round(ville.Distance(
                db.Ville.FirstOrDefault(x => x.nom == VilleDepart).latitude,
                db.Ville.FirstOrDefault(x => x.nom == VilleArrive).latitude,
                db.Ville.FirstOrDefault(x => x.nom == VilleDepart).longitude,
                db.Ville.FirstOrDefault(x => x.nom == VilleArrive).longitude
                ), 2);
            double benefices = dist * db.Avion.FirstOrDefault(x => x.id == idAvion).conso;
            return Math.Round(benefices,2);
        }

        public double GetTotalBenefices()
        {
            VolDAO v = new VolDAO();
            double beneficeTotal = 0;
            foreach (int idVol in db.Vol.Select(x=>x.id))
            {
                beneficeTotal = beneficeTotal +
                    v.GetBenefices(
                    idVol, 
                    db.Vol.FirstOrDefault(x => x.id ==idVol).idAvion, 
                    db.Vol.FirstOrDefault(x=>x.id==idVol).villeDepart,
                    db.Vol.FirstOrDefault(x => x.id == idVol).villeArrive);
            }
            return Math.Round(beneficeTotal,2);
        }

        

    }
}
