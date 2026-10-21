using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class ClientManager
    {
        private ClientDAL _clientDAL = new ClientDAL();
        public (string message, int idClient) VerifierOuCreerClient(Client c)
        {

            Client existant = _clientDAL.GetClientParNomEtPrenom(c.Nom, c.Prenom);

            if (existant != null)
            {
                return ("Client déjà existant. Sélection automatique effectuée.", existant.IdClient);
            }

            int nouvelId = _clientDAL.AjouterClient(c);
            return ("Nouveau client créé avec succès !", nouvelId);
        }
    }
}
