using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class PieceManager
    {
        private PieceDAL _pieceDAL = new PieceDAL();

        public string CreerPiece(Piece p)
        {
            if (string.IsNullOrWhiteSpace(p.Nom)) return "Erreur : Le nom de la pièce est requis.";
            if (p.IdFournisseur <= 0) return "Erreur : Vous devez sélectionner un fournisseur valide.";
            if (p.PrixAchat < 0) return "Erreur : Le prix ne peut pas être négatif.";

            _pieceDAL.AjouterPiece(p);
            return "Pièce ajoutée avec succès !";
        }
    }
}
