using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class FournisseurManager
    {
    private FournisseurDAL _fournisseurDAL = new FournisseurDAL();
        public string CreerFournisseur(Fournisseur f)
        {
            if (string.IsNullOrWhiteSpace(f.NomSociete))
                return "Le nom de la société est obligatoire.";

            _fournisseurDAL.AjouterFournisseur(f);
            return "Fournisseur ajouté avec succès !";
        }
    }
}
