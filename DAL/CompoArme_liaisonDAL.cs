using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.DAL
{
    public class CompoArme_liaisonDAL
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\PC\Ecole\IS3\CplProgra\ArmurerieWPF\ArmurerieDB.mdf"";Integrated Security = True";
        public List<CompoArme_liaison> GetLesCompoArme_liaisons()//methode pour charger toute la bdd, pas optimisé mais utile pour les tests
        {
            List<CompoArme_liaison> listeCompoArme_liaisons = new List<CompoArme_liaison>();
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "SELECT IdArme, IdPiece, QuantiteRequise FROM T_CompoArme_liaison";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            CompoArme_liaison ca = new CompoArme_liaison();
                            ca.IdArme = Convert.ToInt32(lecteur["IdArme"]);
                            ca.IdPiece = Convert.ToInt32(lecteur["IdPiece"]);
                            ca.QuantiteRequise = Convert.ToInt32(lecteur["QuantiteRequise"]);
                            listeCompoArme_liaisons.Add(ca);
                        }
                    }
                }
            }
            return listeCompoArme_liaisons;
        }
        public List<CompoArme_liaison> GetCompositionArme(int idArme)//methode pour eviter de charger toute la bdd, on prend que l'id de l'arme
        {
            List<CompoArme_liaison> recette = new List<CompoArme_liaison>();
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                
                string requete = "SELECT IdArme, IdPiece, QuantiteRequise FROM T_CompoArme_liaison WHERE IdArme = @id";

                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    
                    commande.Parameters.AddWithValue("@id", idArme);

                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            CompoArme_liaison ca = new CompoArme_liaison();
                            ca.IdArme = Convert.ToInt32(lecteur["IdArme"]);
                            ca.IdPiece = Convert.ToInt32(lecteur["IdPiece"]);
                            ca.QuantiteRequise = Convert.ToInt32(lecteur["QuantiteRequise"]);
                            recette.Add(ca);
                        }
                    }
                }
            }
            return recette;
        }
        public void AjouterLigneComposition(int idArme, int idPiece, int quantiteRequise)
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "INSERT INTO T_CompoArme_liaison (IdArme, IdPiece, QuantiteRequise) VALUES (@idA, @idP, @qte)";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@idA", idArme);
                    commande.Parameters.AddWithValue("@idP", idPiece);
                    commande.Parameters.AddWithValue("@qte", quantiteRequise);

                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
            }
        }
    }
}
