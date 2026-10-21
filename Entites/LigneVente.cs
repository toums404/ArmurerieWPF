using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.Entites
{
    public class LigneVente
    {
        public int IdLigneV { get; set; }
        public int IdVente { get; set; }
        public int IdArme { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}
