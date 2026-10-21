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
    public class AssemblageViewModel : INotifyPropertyChanged
    {
        private ArmeDAL _armeDal = new ArmeDAL();
        private PieceDAL _pieceDal = new PieceDAL();
        private CompoArme_liaisonDAL _compoDal = new CompoArme_liaisonDAL();

        // liste pour les cmb
        public ObservableCollection<Arme> ListeArmes { get; set; }
        public ObservableCollection<Piece> ListePieces { get; set; }

        // panier temp
        public ObservableCollection<LignePanier> Panier { get; set; }

        // champs de saisie
        private Arme _armeSelectionnee;
        public Arme ArmeSelectionnee { get { return _armeSelectionnee; } set { _armeSelectionnee = value; OnPropertyChanged(); } }

        private Piece _pieceSelectionnee;
        public Piece PieceSelectionnee { get { return _pieceSelectionnee; } set { _pieceSelectionnee = value; OnPropertyChanged(); } }

        private int _quantiteSaisie = 1;
        public int QuantiteSaisie { get { return _quantiteSaisie; } set { _quantiteSaisie = value; OnPropertyChanged(); } }

        // comamnde
        public ICommand AjouterPanierCommand { get; set; }
        public ICommand ValiderPanierCommand { get; set; }

        public AssemblageViewModel()
        {
            ListeArmes = new ObservableCollection<Arme>(_armeDal.GetLesArmes());
            ListePieces = new ObservableCollection<Piece>(_pieceDal.GetLesPieces());
            Panier = new ObservableCollection<LignePanier>();

            // btn ajt pas actif si piece selectionnée = 0
            AjouterPanierCommand = new RelayCommand(o => AjouterLigne(), o => PieceSelectionnee != null && QuantiteSaisie > 0);

            //btn valider actif que si arme selectionnée et panier pas vide
            ValiderPanierCommand = new RelayCommand(o => ValiderRecette(), o => ArmeSelectionnee != null && Panier.Count > 0);
        }

        private void AjouterLigne()
        {
            Panier.Add(new LignePanier
            {
                IdPiece = PieceSelectionnee.IdPiece,
                NomPiece = PieceSelectionnee.Nom,
                Quantite = QuantiteSaisie
            });

            PieceSelectionnee = null;
            QuantiteSaisie = 1;
        }

        private void ValiderRecette()
        {
            // boucle sur chaque  ligne pour inserer dans la bdd
            foreach (var ligne in Panier)
            {
                _compoDal.AjouterLigneComposition(ArmeSelectionnee.IdArme, ligne.IdPiece, ligne.Quantite);
            }

            Panier.Clear();
            ArmeSelectionnee = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // affichage opti
    public class LignePanier
    {
        public int IdPiece { get; set; }
        public string NomPiece { get; set; }
        public int Quantite { get; set; }
    }
}
