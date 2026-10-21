using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.Entites
{
    public class Piece
    {
        public int IdPiece { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public int QuantiteStock { get; set; }
        public int SeuilAlerte { get; set; }
        public decimal PrixAchat { get; set; }
        public int IdFournisseur { get; set; }

    }
}
