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
    public class PiecesViewModel : INotifyPropertyChanged
    {
        private PieceDAL _pieceDal = new PieceDAL();
        private FournisseurDAL _fournisseurDal = new FournisseurDAL();

        // liste pour affichage
        public ObservableCollection<Piece> ListePieces { get; set; }
        public ObservableCollection<Fournisseur> ListeFournisseurs { get; set; }

        // formulaire
        private string _nouveauNom;
        public string NouveauNom { get { return _nouveauNom; } set { _nouveauNom = value; OnPropertyChanged(); } }

        private string _nouvelleDescription;
        public string NouvelleDescription { get { return _nouvelleDescription; } set { _nouvelleDescription = value; OnPropertyChanged(); } }

        private decimal _nouveauPrix;
        public decimal NouveauPrix { get { return _nouveauPrix; } set { _nouveauPrix = value; OnPropertyChanged(); } }

        private int _nouvelleQuantite;
        public int NouvelleQuantite { get { return _nouvelleQuantite; } set { _nouvelleQuantite = value; OnPropertyChanged(); } }

        private int _nouveauSeuil;
        public int NouveauSeuil { get { return _nouveauSeuil; } set { _nouveauSeuil = value; OnPropertyChanged(); } }

        // stocke le fournissue r sélectionné dans la ComboBox
        private Fournisseur _fournisseurSelectionne;
        public Fournisseur FournisseurSelectionne { get { return _fournisseurSelectionne; } set { _fournisseurSelectionne = value; OnPropertyChanged(); } }

        public ICommand AjouterPieceCommand { get; set; }

        public PiecesViewModel()
        {
            ChargerDonnees();
            AjouterPieceCommand = new RelayCommand(o => AjouterNouvellePiece(), o => FournisseurSelectionne != null);
            // ajout impossible si fournisseur non sélectionné
        }

        private void ChargerDonnees()
        {
            ListePieces = new ObservableCollection<Piece>(_pieceDal.GetLesPieces());
            ListeFournisseurs = new ObservableCollection<Fournisseur>(_fournisseurDal.GetLesFournisseurs());

            OnPropertyChanged(nameof(ListePieces));
            OnPropertyChanged(nameof(ListeFournisseurs));
        }

        private void AjouterNouvellePiece()
        {
            Piece nouvellePiece = new Piece
            {
                Nom = NouveauNom,
                Description = NouvelleDescription,
                PrixAchat = NouveauPrix,
                QuantiteStock = NouvelleQuantite,
                SeuilAlerte = NouveauSeuil,
                IdFournisseur = FournisseurSelectionne.IdFournisseur
            };

            _pieceDal.AjouterPiece(nouvellePiece);
            ListePieces = new ObservableCollection<Piece>(_pieceDal.GetLesPieces());
            OnPropertyChanged(nameof(ListePieces));

            NouveauNom = "";
            NouvelleDescription = "";
            NouveauPrix = 0;
            NouvelleQuantite = 0;
            NouveauSeuil = 0;
            FournisseurSelectionne = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
