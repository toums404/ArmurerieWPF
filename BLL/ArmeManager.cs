using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD_Armurerie_TT_25_26.BLL
{
    public class ArmeManager
    {
        private ArmeDAL _armeDAL = new ArmeDAL();//private pour éviter les accès directs depuis l'extérieur de la classe
        public string CreerArme(Arme a)
        {
            if (string.IsNullOrWhiteSpace(a.Nom)) return "Erreur : Le nom de l'arme est requis.";
            if (a.PrixVente <= 0) return "Erreur : Le prix doit être supérieur à 0.";

            _armeDAL.AjouterArme(a);
            return "Arme ajoutée au catalogue !";
        }
    }
}
