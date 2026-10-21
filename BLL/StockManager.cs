using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class StockManager
    {
        private ArmeDAL _armeDAL = new ArmeDAL();
        private PieceDAL _pieceDAL = new PieceDAL();
        private CompoArme_liaisonDAL _compoArme_LiaisonDAL = new CompoArme_liaisonDAL();
        public List<Arme> ObtenirArmesSeuil()
        {
            List<Arme> toutesArmes = _armeDAL.GetLesArmes();
            List<Arme> ArmesAlertes = new List<Arme>();

            foreach (Arme a in toutesArmes)
            {
                if (a.QuantiteStock <= a.SeuilAlerte)
                {
                    ArmesAlertes.Add(a);
                }
            }
            return ArmesAlertes;
        }

        public List<Piece> ObtenirPiecesSeuil()
        {
            List<Piece> toutesPieces = _pieceDAL.GetLesPieces();
            List<Piece> PiecesAlertes = new List<Piece>();
            foreach (Piece p in toutesPieces)
            {
                if (p.QuantiteStock <= p.SeuilAlerte)
                {
                    PiecesAlertes.Add(p);
                }
            }
            return PiecesAlertes;
        }
        public string AssemblerArme(int idArme, int quantiteAAssembler)
        {
            if (quantiteAAssembler <= 0) return "Erreur : Vous devez assembler au moins 1 arme.";

            List<CompoArme_liaison> recette = _compoArme_LiaisonDAL.GetCompositionArme(idArme);

            int totalPiecesRequises = 0;//min 2 pièces pour une recette valide
            foreach (var element in recette)
            {
                totalPiecesRequises += element.QuantiteRequise;
            }

            if (totalPiecesRequises < 2)
            {
                return "Erreur d'assemblage : Cette arme n'a pas une recette valide. Elle doit être composée d'au minimum 2 pièces !";
            }

            List<Piece> toutesLesPiecesEnStock = _pieceDAL.GetLesPieces();

            foreach (var element in recette)
            {
                int besoinTotal = element.QuantiteRequise * quantiteAAssembler;

                Piece pieceEnMagasin = toutesLesPiecesEnStock.Find(p => p.IdPiece == element.IdPiece);

                if (pieceEnMagasin == null || pieceEnMagasin.QuantiteStock < besoinTotal)
                {
                    return $"Erreur : Stock insuffisant pour la pièce ID {element.IdPiece}. Besoin : {besoinTotal}, En stock : {(pieceEnMagasin == null ? 0 : pieceEnMagasin.QuantiteStock)}.";
                }
            }

            foreach (var element in recette)//assezmblage et maj des stocks
            {
                int quantiteAEnlever = element.QuantiteRequise * quantiteAAssembler;
                _pieceDAL.DiminuerStock(element.IdPiece, quantiteAEnlever);
            }

            _armeDAL.AugmenterStock(idArme, quantiteAAssembler);

            return $"Succès : {quantiteAAssembler} arme(s) assemblée(s) avec succès ! Les stocks ont été mis à jour.";
        }
        public List<string> ObtenirTexteRecette(int idArme)
        {
            List<string> lignesAffichage = new List<string>();

            List<CompoArme_liaison> recette = _compoArme_LiaisonDAL.GetCompositionArme(idArme);//recup id

            List<Piece> toutesLesPieces = _pieceDAL.GetLesPieces();

            foreach (var element in recette)
            {
                // on regarde quel piece a quel id pour afficher le nom de la pièce plutot que son id
                Piece p = toutesLesPieces.Find(x => x.IdPiece == element.IdPiece);

                string nomPiece = p != null ? p.Nom : "Pièce inconnue";

                lignesAffichage.Add($"{element.QuantiteRequise}x {nomPiece}");
            }

            return lignesAffichage;
        }

        public string CreerRecetteComplete(int idArme, List<CompoArme_liaison> nouvelleRecette)
        {
            if (nouvelleRecette.Count < 2)
            {
                return "Erreur : Une arme doit être composée d'au moins 2 pièces distinctes.";
            }

            // On enregistre chaque pièce de la liste
            foreach (var ligne in nouvelleRecette)
            {
                _compoArme_LiaisonDAL.AjouterLigneComposition(idArme, ligne.IdPiece, ligne.QuantiteRequise);
            }

            return "Succès : La recette a été enregistrée dans la base de données !";
        }
    }
    
}
