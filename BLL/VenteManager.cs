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
    public class VenteManager
    {
        private ArmeDAL _armeDAL = new ArmeDAL();
        private VenteDAL _venteDAL = new VenteDAL();
        private LigneVenteDAL _ligneVenteDAL = new LigneVenteDAL();
        private ClientDAL _clientDAL = new ClientDAL();

        public string ValiderVente(int IdClient, int IdArme, int QuantiteVendue)
        {
            if (QuantiteVendue <= 0) { return "La quantité vendue doit être supérieure à zéro."; }

            Arme armeAVendre = _armeDAL.GetLesArmes().Find(a => a.IdArme == IdArme);
            
            if (armeAVendre == null) { return "L'arme sélectionnée n'existe pas."; }
            
            if (armeAVendre.QuantiteStock < QuantiteVendue) { return $"Stock insuffisant pour l'arme '{armeAVendre.Nom}'. Besoin : {QuantiteVendue}, Stock disponible : {armeAVendre.QuantiteStock}."; }
            
            
            decimal MontantTotal = armeAVendre.PrixVente * QuantiteVendue;
            _armeDAL.DiminuerStock(IdArme, QuantiteVendue);
            int idNouvelleVente = _venteDAL.AjouterVente(IdClient, DateTime.Now, "Facture", (int) MontantTotal);
            _ligneVenteDAL.AjouterLigneVente(idNouvelleVente, IdArme, QuantiteVendue, armeAVendre.PrixVente);
            string facture = $"===== FACTURE AUTOMATIQUE =====\n" +
                             $"Date : {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}\n" +
                             $"Client ID : {IdClient}\n" +
                             $"-------------------------------\n" +
                             $"Article : {armeAVendre.Nom}\n" +
                             $"Quantité : {QuantiteVendue}\n" +
                             $"Prix unitaire : {armeAVendre.PrixVente} €\n" +
                             $"-------------------------------\n" +
                             $"TOTAL À PAYER : {MontantTotal} €\n" +
                             $"===============================";

            return facture;
        }
        public decimal CalculerChiffreAffairesHebdo()
        {
            List<Vente> toutesLesVentes = _venteDAL.GetLesVentes();
            decimal chiffreAffaires = 0;

            //date limite a 7j
            DateTime dateLimite = DateTime.Now.AddDays(-7);

            foreach (var vente in toutesLesVentes)
            {
                if (vente.DateVente >= dateLimite)
                {
                    
                    chiffreAffaires += vente.MontantTotal; 
                }
            }

            return chiffreAffaires;
        }
        public void ExporterFactureTexte(int idVente, string cheminFichier)
        {

            Vente vente = _venteDAL.GetLesVentes().Find(v => v.IdVente == idVente);
            if (vente == null) return; // Sécurité si la vente n'existe pas

            Client client = _clientDAL.GetLesClients().Find(c => c.IdClient == vente.IdClient);
            string nomClient = client != null ? $"{client.Prenom} {client.Nom}" : "Client Inconnu";

            var lignesDeCetteVente = _ligneVenteDAL.GetLesLignesVente().FindAll(lv => lv.IdVente == idVente);

            string contenuFacture = $"===== FACTURE TOMCORP =====\n";
            contenuFacture += $"Facture N° : {vente.IdVente}\n";
            contenuFacture += $"Date : {vente.DateVente.ToString("dd/MM/yyyy HH:mm")}\n";
            contenuFacture += $"Client : {nomClient} (ID: {vente.IdClient})\n";
            contenuFacture += $"-------------------------------\n";

            foreach (var ligne in lignesDeCetteVente)
            {
                Arme arme = _armeDAL.GetLesArmes().Find(a => a.IdArme == ligne.IdArme);
                string nomArme = arme != null ? arme.Nom : "Article inconnu";

                contenuFacture += $"Article : {nomArme}\n";
                contenuFacture += $"Quantité : {ligne.Quantite}\n";
                contenuFacture += $"Prix unitaire : {ligne.PrixUnitaire} €\n";
                contenuFacture += $"-------------------------------\n";
            }

            contenuFacture += $"TOTAL RÉGLÉ : {vente.MontantTotal} €\n";
            contenuFacture += $"===============================\n";
            contenuFacture += $"Merci de votre confiance !";

            File.WriteAllText(cheminFichier, contenuFacture);
        }
    }

    
}
