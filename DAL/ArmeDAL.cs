using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.DAL
{
    public class ArmeDAL
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\PC\Ecole\IS3\CplProgra\ArmurerieWPF\ArmurerieDB.mdf"";Integrated Security = True";
        
        public List<Arme> GetLesArmes()
        { 
            List<Arme> listeArmes = new List<Arme>();
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            //using pour s'assurer que la connexion est fermée même en cas d'erreur
            {
                string requete = "SELECT IdArme, Nom, PrixVente, QuantiteStock, SeuilAlerte  FROM T_Arme";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            Arme a = new Arme();
                            a.IdArme = Convert.ToInt32(lecteur["IdArme"]);
                            a.Nom = lecteur["Nom"].ToString();
                            a.PrixVente = Convert.ToDecimal(lecteur["PrixVente"]);
                            a.QuantiteStock = Convert.ToInt32(lecteur["QuantiteStock"]);
                            a.SeuilAlerte = Convert.ToInt32(lecteur["SeuilAlerte"]);
                            listeArmes.Add(a);
                        }
                    }
                }
            }
            return listeArmes;
        }
        public void AugmenterStock(int idArme, int quantiteFabriquee)
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                //maj du stock
                string requete = "UPDATE T_Arme SET QuantiteStock = QuantiteStock + @qte WHERE IdArme = @id";
                //@ pour plus de sécurité, on évite les injections SQL
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@qte", quantiteFabriquee);
                    commande.Parameters.AddWithValue("@id", idArme);

                    connexion.Open();
                    commande.ExecuteNonQuery();//execute sans renvoyer qqch
                }
            }
        }
        public void DiminuerStock(int idArme, int quantiteVendue)
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "UPDATE T_Arme SET QuantiteStock = QuantiteStock - @qte WHERE IdArme = @id";

                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@qte", quantiteVendue);
                    commande.Parameters.AddWithValue("@id", idArme);

                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
            }
        }
        public void AjouterArme(Arme a)
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "INSERT INTO T_Arme (Nom, QuantiteStock, SeuilAlerte, PrixVente) " +
                                 "VALUES (@nom, @stock, @seuil, @prix)";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@nom", a.Nom);
                    commande.Parameters.AddWithValue("@stock", a.QuantiteStock);
                    commande.Parameters.AddWithValue("@seuil", a.SeuilAlerte);
                    commande.Parameters.AddWithValue("@prix", a.PrixVente);
                    //remplace les parametre par les valeurs recup dans l'interface
                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
            }
        }
    }
}
