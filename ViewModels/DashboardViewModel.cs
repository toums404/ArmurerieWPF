using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ArmurerieWPF.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private string _chiffreAffairesSemaine;
        public string ChiffreAffairesSemaine
        {
            get { return _chiffreAffairesSemaine; }
            set { _chiffreAffairesSemaine = value; OnPropertyChanged(); }
        }

        public ObservableCollection<AlerteItem> ListeAlertes { get; set; }

        public DashboardViewModel()
        {
            ListeAlertes = new ObservableCollection<AlerteItem>();
            ChargerAlertesStock();
            CalculerChiffreAffaires();
        }

        private void ChargerAlertesStock()
        {
            ArmeDAL armeDal = new ArmeDAL();
            var toutesLesArmes = armeDal.GetLesArmes();
            var armesEnAlerte = toutesLesArmes.Where(a => a.QuantiteStock <= a.SeuilAlerte);

            foreach (var arme in armesEnAlerte)
            {
                ListeAlertes.Add(new AlerteItem
                {
                    Type = "Arme",
                    Article = arme.Nom,
                    Quantite = arme.QuantiteStock,
                    Seuil = arme.SeuilAlerte
                });
            }
        }

        private void CalculerChiffreAffaires()
        {
            VenteDAL venteDal = new VenteDAL();
            var toutesLesVentes = venteDal.GetLesVentes();

            // Date d'il y a 7 jours
            DateTime dateSemaineDerniere = DateTime.Now.AddDays(-7);

            // Somme du MontantTotal pour les ventes récentes
            var total = toutesLesVentes
                .Where(v => v.DateVente >= dateSemaineDerniere)
                .Sum(v => v.MontantTotal);

            ChiffreAffairesSemaine = $"{total} €";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AlerteItem
    {
        public string Type { get; set; }
        public string Article { get; set; }
        public int Quantite { get; set; }
        public int Seuil { get; set; }
    }

}
