using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IndicateurAirAtlantique.DataBase;
using System.Text.RegularExpressions;

namespace IndicateurAirAtlantique
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[] idVolRecherche = new int[0];

        public MainWindow()
        {
            InitializeComponent();

            //initialisation de villeDAO : instance ????
            VilleDAO villeDAO = new VilleDAO();
            //Remplissage des ComboBox pour la recherche
            foreach (string ville in villeDAO.GetAll())
            {
                CbVilleD.Items.Add(ville);
                CbVilleA.Items.Add(ville);
            }

            //Remplissage du top taux de remplissages sur les lignes
            VolDAO volDAO = new VolDAO();

            foreach (string vol in volDAO.TopUsed())
            {
                ListFrequentation.Items.Add(vol);
            }

            //Remplissage du label avec les indicateurs globaux de la compagnie
            RemplissageIndicateursGlobaux();

            
        }

        private void BtRecherche_Click(object sender, RoutedEventArgs e)
        {
            ListRecherche.Items.Clear();
            idVolRecherche = new int[0];
            
            VolDAO volDAO = new VolDAO();
            foreach (int vol in volDAO.GetID(CbVilleD.SelectedItem.ToString(), CbVilleA.SelectedItem.ToString()))
            {
                foreach (string id in volDAO.GetRecherche(vol))
                {
                    ListRecherche.Items.Add(id);
                }
                Array.Resize(ref idVolRecherche, idVolRecherche.Length + 1);
                idVolRecherche[idVolRecherche.Length-1] = vol;
            }
        }

        private void ListRecherche_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //not used function
        }

        private void ListRecherche_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VolDAO volSelected = new VolDAO();
            try
            {
                InfoVol.Content = volSelected.Afficher(idVolRecherche[ListRecherche.SelectedIndex]);
            }
            catch
            {
                InfoVol.Content = "";
            }
        }

        public void RemplissageIndicateursGlobaux()
        {
            
            VentesPlacesDAO v = new VentesPlacesDAO();
            VolDAO vo = new VolDAO();
            LbIndicateursGlobaux.Content = string.Format(
                "Nombre de Places Vendues sur tous les Vols : \t{0}\n"+
                "Chiffre d'Affaire généré par la compagnie : \t\t{1}€\n"+
                "Bénéfices Approximatifs de la compagnie : \t\t{2}€",
                v.GetTotalPlacesVendues(), v.GetTotalCA(), vo.GetTotalBenefices()
                );
        }
    }
}
