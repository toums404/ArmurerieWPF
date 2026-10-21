using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.DAL
{
    public class ClientDAL
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\PC\Ecole\IS3\CplProgra\ArmurerieWPF\ArmurerieDB.mdf"";Integrated Security = True";
        public List<Client> GetLesClients() 
        {
            List<Client> listeClients = new List<Client>();
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "SELECT IdClient, Nom, Prenom, Adresse, Email FROM T_Client";
                using (SqlCommand commande = new SqlCommand(requete, connexion)) 
                {
                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        while (lecteur.Read()) 
                        {
                            Client c = new Client();
                            c.IdClient = Convert.ToInt32(lecteur["IdClient"]);
                            c.Nom = lecteur["Nom"].ToString();
                            c.Prenom = lecteur["Prenom"].ToString();
                            c.Adresse = lecteur["Adresse"].ToString();
                            c.Email = lecteur["Email"].ToString();
                            listeClients.Add(c);
                        }
                        
                    }
                }

            }
            return listeClients;
        }
        public Client GetClientParNomEtPrenom(string nom, string prenom)//verif si client existe 
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                string requete = "SELECT IdClient, Nom, Prenom, Adresse, Email FROM T_Client WHERE Nom = @nom AND Prenom = @prenom";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@nom", nom);
                    commande.Parameters.AddWithValue("@prenom", prenom);

                    connexion.Open();
                    using (SqlDataReader lecteur = commande.ExecuteReader())
                    {
                        if (lecteur.Read())//si on trouve le client avec le nom on le retourne sinon on retourne null
                        {
                            return new Client
                            {
                                IdClient = Convert.ToInt32(lecteur["IdClient"]),
                                Nom = lecteur["Nom"].ToString(),
                                Prenom = lecteur["Prenom"].ToString(),
                                Adresse = lecteur["Adresse"].ToString(),
                                Email = lecteur["Email"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
        public int AjouterClient(Client c)//ajt un client directement dans la bdd et retourne son ID généré automatiquement
        {
            using (SqlConnection connexion = new SqlConnection(_connectionString))
            {
                // SCOPE_IDENTITY() permet de récupérer l'ID généré automatiquement
                string requete = "INSERT INTO T_Client (Nom, Prenom, Adresse, Email) VALUES (@nom, @prenom, @adr, @mail); SELECT SCOPE_IDENTITY();";
                using (SqlCommand commande = new SqlCommand(requete, connexion))
                {
                    commande.Parameters.AddWithValue("@nom", c.Nom);
                    commande.Parameters.AddWithValue("@prenom", c.Prenom);
                    commande.Parameters.AddWithValue("@adr", c.Adresse);
                    commande.Parameters.AddWithValue("@mail", c.Email);

                    connexion.Open();
                    return Convert.ToInt32(commande.ExecuteScalar());//executeScalar car on attend une seule valeur 
                }
            }
        }
    }
}
