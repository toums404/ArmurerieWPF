using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class CommandeManager
    {
        private CommandeFournisseurDAL _enteteDAL = new CommandeFournisseurDAL();
        private LigneCommandeFDAL _ligneDAL = new LigneCommandeFDAL();
        private PieceDAL _pieceDAL = new PieceDAL();

        public string PasserCommande(int idFournisseur, int idPiece, int quantite, string nomPiece, string nomFournisseur)
        {
            if (quantite <= 0) return "Erreur : La quantité à commander doit être supérieure à zéro.";

            int idNouvelleCommande = _enteteDAL.CreerEnteteCommande(idFournisseur);//on crée l'entête de la commande et on récupère son ID

            _ligneDAL.AjouterLigneCommande(idNouvelleCommande, idPiece, quantite);//on ajoute la ligne de commande avec l'ID de l'entête, l'ID de la pièce et la quantité

            _pieceDAL.AugmenterStock(idPiece, quantite);//on met à jour le stock de la pièce en augmentant la quantité commandée


            string bordereau = $"===== BORDEREAU DE COMMANDE =====\n" +
                               $"Commande N° : {idNouvelleCommande}\n" +
                               $"Date : {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}\n" +
                               $"Fournisseur : {nomFournisseur} (ID: {idFournisseur})\n" +
                               $"---------------------------------\n" +
                               $"DÉTAIL DES ARTICLES :\n" +
                               $"[+] {quantite}x {nomPiece}\n" +
                               $"---------------------------------\n" +
                               $"Statut : Envoyée\n" +
                               $"=================================";

            return bordereau;
        }
        public void ExporterBordereauTexte(int idCommande, string cheminFichier)
        {
            //recup l'entete de commande pour afficher les infos générales
            CommandeFournisseur commande = _enteteDAL.GetLesCommandesFournisseur().Find(c => c.IdCommandeF == idCommande);
            if (commande == null) return;

            var lignesDeCetteCommande = _ligneDAL.GetLesLignesCommandesF().FindAll(lc => lc.IdCommandeF == idCommande);

            string contenuBordereau = $"===== BORDEREAU DE COMMANDE =====\n";
            contenuBordereau += $"Commande N° : {commande.IdCommandeF}\n";
            contenuBordereau += $"Date : {commande.DateCommande.ToString("dd/MM/yyyy HH:mm")}\n";
            contenuBordereau += $"Fournisseur : {commande.NomFournisseurAffichage} (ID: {commande.IdFournisseur})\n";
            contenuBordereau += $"Statut : {commande.Statut}\n";
            contenuBordereau += $"-------------------------------\n";

            foreach (var ligne in lignesDeCetteCommande)
            {
                Piece piece = _pieceDAL.GetLesPieces().Find(p => p.IdPiece == ligne.IdPiece);
                string nomPiece = piece != null ? piece.Nom : "Pièce inconnue";

                contenuBordereau += $"Article : {nomPiece}\n";
                contenuBordereau += $"Quantité : {ligne.QuantiteCommande}\n";
                contenuBordereau += $"-------------------------------\n";
            }

            contenuBordereau += $"===============================\n";
            contenuBordereau += $"TomCorp - Document Interne";

            File.WriteAllText(cheminFichier, contenuBordereau);
        }
    }
}
