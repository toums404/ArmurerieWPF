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
    public class FabricationViewModel : INotifyPropertyChanged
    {
        private ArmeDAL _armeDal = new ArmeDAL();
        private PieceDAL _pieceDal = new PieceDAL();
        private CompoArme_liaisonDAL _compoDal = new CompoArme_liaisonDAL();

        public ObservableCollection<Arme> ListeArmes { get; set; }

        // liste pour afficher piece
        private ObservableCollection<DetailRecette> _piecesNecessaires;
        public ObservableCollection<DetailRecette> PiecesNecessaires
        {
            get { return _piecesNecessaires; }
            set { _piecesNecessaires = value; OnPropertyChanged(); }
        }

        private Arme _armeSelectionnee;
        public Arme ArmeSelectionnee
        {
            get { return _armeSelectionnee; }
            set
            {
                _armeSelectionnee = value;
                OnPropertyChanged();
                CalculerPiecesNecessaires();
            }
        }

        private int _quantite = 1;
        public int Quantite
        {
            get { return _quantite; }
            set
            {
                _quantite = value;
                OnPropertyChanged();
                CalculerPiecesNecessaires();
            }
        }

        public ICommand FabriquerCommand { get; set; }

        public FabricationViewModel()
        {
            ListeArmes = new ObservableCollection<Arme>(_armeDal.GetLesArmes());
            PiecesNecessaires = new ObservableCollection<DetailRecette>();

            // btn actif si une arme est sélectionnée, quantité > 0, et que l'arme a bien une recette
            FabriquerCommand = new RelayCommand(o => Fabriquer(), o => ArmeSelectionnee != null && Quantite > 0 && PiecesNecessaires.Count > 0);
        }

        private void CalculerPiecesNecessaires()
        {
            PiecesNecessaires.Clear();
            if (ArmeSelectionnee != null && Quantite > 0)
            {
                var recetteBase = _compoDal.GetCompositionArme(ArmeSelectionnee.IdArme);
                var toutesLesPieces = _pieceDal.GetLesPieces(); 

                foreach (var ligne in recetteBase)
                {
                    var piece = toutesLesPieces.FirstOrDefault(p => p.IdPiece == ligne.IdPiece);
                    if (piece != null)
                    {
                        PiecesNecessaires.Add(new DetailRecette
                        {
                            IdPiece = piece.IdPiece,
                            NomPiece = piece.Nom,
                            QuantiteTotale = ligne.QuantiteRequise * Quantite
                        });
                    }
                }
            }
        }

        private void Fabriquer()
        {
            //diminution du stock des pièces nécessaires
            foreach (var piece in PiecesNecessaires)
            {
                _pieceDal.DiminuerStock(piece.IdPiece, piece.QuantiteTotale);
            }

            //augmentation stock arme finie
            _armeDal.AugmenterStock(ArmeSelectionnee.IdArme, Quantite);

            ArmeSelectionnee = null;
            Quantite = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DetailRecette
    {
        public int IdPiece { get; set; }
        public string NomPiece { get; set; }
        public int QuantiteTotale { get; set; }
    }
}
