using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.Entites
{
    public class CommandeFournisseur
    {
        public int IdCommandeF { get; set; }
        public int IdFournisseur { get; set; }
        public DateTime DateCommande { get; set; }
        public string Statut { get; set; }
        public string NomFournisseurAffichage { get; set; }
    }
}
