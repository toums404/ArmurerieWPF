using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class FabricationManager
    {
        private ArmeDAL _armeDAL = new ArmeDAL();
        private PieceDAL _pieceDAL = new PieceDAL();
        private CompoArme_liaisonDAL _compoDAL = new CompoArme_liaisonDAL();

        public string AssemblerArme(int IdArme, int QuantiteAFabriquer)
        {
            if (QuantiteAFabriquer <= 0){return "La quantité à fabriquer doit être supérieure à zéro.";}
            
            List<CompoArme_liaison> recette = _compoDAL.GetCompositionArme(IdArme);

            if (recette.Count == 0){return "Aucune pièce n'est définie pour cette arme."; }

            foreach (var compo in recette)
            {
                Piece piece = _pieceDAL.GetLesPieces().Find(p => p.IdPiece == compo.IdPiece);
                int besoinTotal = compo.QuantiteRequise * QuantiteAFabriquer;
                if ( piece.QuantiteStock < besoinTotal)
                {
                    return $"Stock insuffisant pour la pièce '{piece.Nom}'. Besoin : {besoinTotal}, Stock disponible : {piece.QuantiteStock}.";
                }
            }
            foreach (var compo in recette)
            {
                int quantiteUtilisee = compo.QuantiteRequise * QuantiteAFabriquer;
                _pieceDAL.DiminuerStock(compo.IdPiece, quantiteUtilisee);
            }
            _armeDAL.AugmenterStock(IdArme, QuantiteAFabriquer);
            return $"Fabrication de {QuantiteAFabriquer} unité(s) de l'arme ID {IdArme} réussie. Stock mis a jour avec succès.";
        }
    }
}
