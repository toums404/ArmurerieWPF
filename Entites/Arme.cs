using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.Entites
{
    public class Arme
    {
        public int IdArme { get; set; }//get autorise le programme a lire et set a ecrire
        public string Nom { get; set; }
        public decimal PrixVente { get; set; }
        public int QuantiteStock { get; set; }
        public int SeuilAlerte { get; set; }

    }
}
