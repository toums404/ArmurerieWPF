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
    public class FournisseursViewModel : INotifyPropertyChanged
    {
        private FournisseurDAL _fournisseurDal = new FournisseurDAL();

        public ObservableCollection<Fournisseur> ListeFournisseurs { get; set; }

        //formulaire 
        private string _nouveauNomSociete;
        public string NouveauNomSociete { get { return _nouveauNomSociete; } set { _nouveauNomSociete = value; OnPropertyChanged(); } }

        private string _nouveauContact;
        public string NouveauContact { get { return _nouveauContact; } set { _nouveauContact = value; OnPropertyChanged(); } }

        private string _nouveauTelephone;
        public string NouveauTelephone { get { return _nouveauTelephone; } set { _nouveauTelephone = value; OnPropertyChanged(); } }

        public ICommand AjouterFournisseurCommand { get; set; }

        public FournisseursViewModel()
        {
            ChargerFournisseurs();
            AjouterFournisseurCommand = new RelayCommand(o => AjouterNouveauFournisseur());
        }

        private void ChargerFournisseurs()
        {
            ListeFournisseurs = new ObservableCollection<Fournisseur>(_fournisseurDal.GetLesFournisseurs());
            OnPropertyChanged(nameof(ListeFournisseurs));
        }

        private void AjouterNouveauFournisseur()
        {
            Fournisseur nouveau = new Fournisseur
            {
                NomSociete = NouveauNomSociete,
                ContactNom = NouveauContact,
                Telephone = NouveauTelephone
            };

            _fournisseurDal.AjouterFournisseur(nouveau);
            ChargerFournisseurs();

            NouveauNomSociete = "";
            NouveauContact = "";
            NouveauTelephone = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
