using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.DAL
{
    public class LigneCommandeFDAL
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\PC\Ecole\IS3\CplProgra\ArmurerieWPF\ArmurerieDB.mdf"";Integrated Security = True";
        public List<LigneCommandeF> GetLesLignesCommandesF()
        {
            List<LigneCommandeF> listeCommandeF = new List<LigneCommandeF>();
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "SELECT IdLigneF, IdCommandeF, IdPiece, QuantiteCommande FROM T_LigneCommandeF";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            LigneCommandeF lc = new LigneCommandeF();
                            lc.IdLigneF = Convert.ToInt32(lecteur["IdLigneF"]);
                            lc.IdCommandeF = Convert.ToInt32(lecteur["IdCommandeF"]);
                            lc.IdPiece = Convert.ToInt32(lecteur["IdPiece"]);
                            lc.QuantiteCommande = Convert.ToInt32(lecteur["QuantiteCommande"]);
                            listeCommandeF.Add(lc);
                        }
                    }
                }
            }
            return listeCommandeF;
        }
        public void AjouterLigneCommande(int idCommande, int idPiece, int quantite)
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "INSERT INTO T_LigneCommandeF (IdCommandeF, IdPiece, QuantiteCommande) " +
                                 "VALUES (@idC, @idP, @qte)";

                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@idC", idCommande);
                    commande.Parameters.AddWithValue("@idP", idPiece);
                    commande.Parameters.AddWithValue("@qte", quantite);

                    connexion.Open();
                    commande.ExecuteNonQuery();
                }
            }
        }
    }
}
