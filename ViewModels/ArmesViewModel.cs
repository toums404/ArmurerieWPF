using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace ArmurerieWPF.ViewModels
{
    public class ArmesViewModel : INotifyPropertyChanged
    {
        private ArmeDAL _armeDal = new ArmeDAL();

        public ObservableCollection<Arme> ListeArmes { get; set; }

        // champs du formulaire
        private string _nouveauNom;
        public string NouveauNom { get { return _nouveauNom; } set { _nouveauNom = value; OnPropertyChanged(); } }

        private decimal _nouveauPrix;
        public decimal NouveauPrix { get { return _nouveauPrix; } set { _nouveauPrix = value; OnPropertyChanged(); } }

        private int _nouvelleQuantite;
        public int NouvelleQuantite { get { return _nouvelleQuantite; } set { _nouvelleQuantite = value; OnPropertyChanged(); } }

        private int _nouveauSeuil;
        public int NouveauSeuil { get { return _nouveauSeuil; } set { _nouveauSeuil = value; OnPropertyChanged(); } }

        // bouton
        public ICommand AjouterArmeCommand { get; set; }

        public ArmesViewModel()
        {
            ChargerArmes();
            AjouterArmeCommand = new RelayCommand(o => AjouterNouvelleArme());
        }

        private void ChargerArmes()
        {
            ListeArmes = new ObservableCollection<Arme>(_armeDal.GetLesArmes());
            OnPropertyChanged(nameof(ListeArmes)); // Notifie l'interface si on recharge la liste
        }

        private void AjouterNouvelleArme()
        {
            // creation d'une nouvelle arme à partir des champs du formulaire
            Arme nouvelleArme = new Arme
            {
                Nom = NouveauNom,
                PrixVente = NouveauPrix,
                QuantiteStock = NouvelleQuantite,
                SeuilAlerte = NouveauSeuil
            };

            // envoi a la bdd
            _armeDal.AjouterArme(nouvelleArme);

            // rechargeme,t liste
            ChargerArmes();

            NouveauNom = "";
            NouveauPrix = 0;
            NouvelleQuantite = 0;
            NouveauSeuil = 0;
        }

        // --- INOTIFYPROPERTYCHANGED ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
