using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.DAL
{
    public class CommandeFournisseurDAL
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\PC\Ecole\IS3\CplProgra\ArmurerieWPF\ArmurerieDB.mdf"";Integrated Security = True";

        public List<CommandeFournisseur> GetLesCommandesFournisseur()
        {
        List<CommandeFournisseur> listeCommandesFournisseur = new List<CommandeFournisseur>();
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = @"
            SELECT cmd.IdCommandeF, cmd.DateCommande, cmd.IdFournisseur, cmd.Statut, f.NomSociete 
            FROM T_CommandeFournisseur cmd
            INNER JOIN T_Fournisseurs f ON cmd.IdFournisseur = f.IdFournisseur 
            ORDER BY cmd.DateCommande DESC";
                //inner join pour récupérer le nom du fournisseur lié à la commande
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read())
                        {
                            CommandeFournisseur cf = new CommandeFournisseur();
                            cf.IdCommandeF = Convert.ToInt32(lecteur["IdCommandeF"]);
                            cf.IdFournisseur = Convert.ToInt32(lecteur["IdFournisseur"]);
                            cf.DateCommande = Convert.ToDateTime(lecteur["DateCommande"]);
                            cf.Statut = lecteur["Statut"].ToString();

                            cf.NomFournisseurAffichage = lecteur["NomSociete"].ToString();
                            listeCommandesFournisseur.Add(cf);
                        }
                    }
                }
            }
            return listeCommandesFournisseur;
        }
        public int CreerEnteteCommande(int idFournisseur)
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                // On recup l'id générée par le serveur après insertion
                string requete = "INSERT INTO T_CommandeFournisseur (DateCommande, IdFournisseur, Statut) " +
                                 "VALUES (@date, @idF, @statut); " +
                                 "SELECT SCOPE_IDENTITY();";

                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@date", DateTime.Now);
                    commande.Parameters.AddWithValue("@idF", idFournisseur);
                    commande.Parameters.AddWithValue("@statut", "Envoyée");

                    connexion.Open();
                    //on récupère le nouvel ID
                    int nouvelId = Convert.ToInt32(commande.ExecuteScalar());
                    return nouvelId;
                }
            }
        }
    }
}
