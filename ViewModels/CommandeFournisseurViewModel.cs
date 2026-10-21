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
    public class CommandeFournisseurViewModel : INotifyPropertyChanged
    {
        private FournisseurDAL _fournisseurDal = new FournisseurDAL();
        private PieceDAL _pieceDal = new PieceDAL();
        private LigneCommandeFDAL _ligneCmdDal = new LigneCommandeFDAL();
        private CommandeFournisseurDAL _commandeDal = new CommandeFournisseurDAL();

        public ObservableCollection<Fournisseur> ListeFournisseurs { get; set; }

        private ObservableCollection<Piece> _listePieces;
        public ObservableCollection<Piece> ListePieces
        {
            get { return _listePieces; }
            set { _listePieces = value; OnPropertyChanged(); }
        }

        private Fournisseur _fournisseurSelectionne;
        public Fournisseur FournisseurSelectionne
        {
            get { return _fournisseurSelectionne; }
            set
            {
                _fournisseurSelectionne = value;
                OnPropertyChanged();
                FiltrerPieces();
            }
        }

        private Piece _pieceSelectionnee;
        public Piece PieceSelectionnee { get { return _pieceSelectionnee; } set { _pieceSelectionnee = value; OnPropertyChanged(); } }

        private int _quantite = 1;
        public int Quantite { get { return _quantite; } set { _quantite = value; OnPropertyChanged(); } }

        public ICommand CommanderCommand { get; set; }

        public CommandeFournisseurViewModel()
        {
            ListeFournisseurs = new ObservableCollection<Fournisseur>(_fournisseurDal.GetLesFournisseurs());
            ListePieces = new ObservableCollection<Piece>();

            CommanderCommand = new RelayCommand(o => Commander(), o => PieceSelectionnee != null && Quantite > 0);
        }

        private void FiltrerPieces()
        {
            if (FournisseurSelectionne != null)
            {
                var toutesLesPieces = _pieceDal.GetLesPieces();
                var piecesDuFournisseur = toutesLesPieces.Where(p => p.IdFournisseur == FournisseurSelectionne.IdFournisseur);
                ListePieces = new ObservableCollection<Piece>(piecesDuFournisseur);
            }
            else
            {
                ListePieces.Clear();
            }
        }

        private void Commander()
        {
            // creation d'une nouvelle commande pour le fournisseur sélectionné
            int idNouvelleCommande = _commandeDal.CreerEnteteCommande(FournisseurSelectionne.IdFournisseur);

            _ligneCmdDal.AjouterLigneCommande(idNouvelleCommande, PieceSelectionnee.IdPiece, Quantite);

            // augmenter le stock de la pièce commandée
            _pieceDal.AugmenterStock(PieceSelectionnee.IdPiece, Quantite);

            FournisseurSelectionne = null;
            PieceSelectionnee = null;
            Quantite = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
