using BDD_Armurerie_TT_25_26.DAL;
using BDD_Armurerie_TT_25_26.Entites;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ArmurerieWPF.ViewModels
{
    public class NouvelleVenteViewModel : INotifyPropertyChanged
    {
        private ClientDAL _clientDal = new ClientDAL();
        private ArmeDAL _armeDal = new ArmeDAL();
        private VenteDAL _venteDal = new VenteDAL();

        // stock id client quand validé
        private int _idClientCourant = 0;

        public ObservableCollection<Arme> ListeArmes { get; set; }

        // client
        private string _nomClient;
        public string NomClient { get { return _nomClient; } set { _nomClient = value; OnPropertyChanged(); } }

        private string _prenomClient;
        public string PrenomClient { get { return _prenomClient; } set { _prenomClient = value; OnPropertyChanged(); } }

        private string _adresseClient;
        public string AdresseClient { get { return _adresseClient; } set { _adresseClient = value; OnPropertyChanged(); } }

        private string _emailClient;
        public string EmailClient { get { return _emailClient; } set { _emailClient = value; OnPropertyChanged(); } }

        // vente
        private Arme _armeSelectionnee;
        public Arme ArmeSelectionnee { get { return _armeSelectionnee; } set { _armeSelectionnee = value; OnPropertyChanged(); } }

        private int _quantite = 1;
        public int Quantite { get { return _quantite; } set { _quantite = value; OnPropertyChanged(); } }

        // commande
        public ICommand ValiderClientCommand { get; set; }
        public ICommand ValiderVenteCommand { get; set; }

        public NouvelleVenteViewModel()
        {
            ListeArmes = new ObservableCollection<Arme>(_armeDal.GetLesArmes());

            // Actif si Nom et Prénom sont remplis
            ValiderClientCommand = new RelayCommand(o => TraiterClient(), o => !string.IsNullOrEmpty(NomClient) && !string.IsNullOrEmpty(PrenomClient));

            // Actif si un client est validé arme choisie et quantité > 0
            ValiderVenteCommand = new RelayCommand(o => TraiterVente(), o => _idClientCourant > 0 && ArmeSelectionnee != null && Quantite > 0);
        }

        private void TraiterClient()
        {
            // client existant ? 
            Client clientExistant = _clientDal.GetClientParNomEtPrenom(NomClient, PrenomClient);

            if (clientExistant != null)
            {
                _idClientCourant = clientExistant.IdClient;
                AdresseClient = clientExistant.Adresse; // remplit adresse
                EmailClient = clientExistant.Email;     // remplit email
            }
            else
            {
                // création nv client
                Client nouveauClient = new Client
                {
                    Nom = NomClient,
                    Prenom = PrenomClient,
                    Adresse = AdresseClient ?? "",
                    Email = EmailClient ?? ""
                };
                _idClientCourant = _clientDal.AjouterClient(nouveauClient);
            }
        }

        private void TraiterVente()
        {
            // calcul montant
            int montantTotal = (int)(ArmeSelectionnee.PrixVente * Quantite);

            // créatio, vente
            _venteDal.AjouterVente(_idClientCourant, DateTime.Now, "Facture", montantTotal);

            // maj stock arme
            _armeDal.DiminuerStock(ArmeSelectionnee.IdArme, Quantite);

            EnvoyerEmailFacture(EmailClient, NomClient, PrenomClient, ArmeSelectionnee, Quantite, montantTotal);

            NomClient = ""; PrenomClient = ""; AdresseClient = ""; EmailClient = "";
            ArmeSelectionnee = null; Quantite = 1;
            _idClientCourant = 0;

            // rehcarcher la liste d'arme pour maj stock
            ListeArmes = new ObservableCollection<Arme>(_armeDal.GetLesArmes());
            OnPropertyChanged(nameof(ListeArmes));

            MessageBox.Show("Vente validée et e-mail envoyé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void EnvoyerEmailFacture(string emailClient, string nom, string prenom, Arme arme, int quantiteVendu, int montant)
        {
            // si pas de mail on fait pas 
            if (string.IsNullOrWhiteSpace(emailClient)) return;

            try
            {
                string expediteur = "tomthunus2@gmail.com";
                string motDePasse = "phuy meoz gnru qfxw";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(expediteur);
                message.To.Add(emailClient);
                message.Subject = "Votre facture - Armurerie TomCorp";

                message.Body = $"Bonjour {prenom} {nom},\n\n" +
                               $"Nous vous remercions pour votre achat chez Armurerie TomCorp.\n\n" +
                               $"Voici le récapitulatif de votre commande :\n" +
                               $"- Article : {arme.Nom}\n" +
                               $"- Quantité : {quantiteVendu}\n" +
                               $"- Montant Total : {montant} €\n\n" +
                               $"À très bientôt dans notre boutique !" +
                               $"\n\nCordialement,\n----Armurerie TomCorp----";

                // config serv stmp
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(expediteur, motDePasse),
                    EnableSsl = true
                };

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("La vente a été enregistrée, mais l'envoi de l'email a échoué.\nErreur : " + ex.Message, "Erreur d'envoi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
