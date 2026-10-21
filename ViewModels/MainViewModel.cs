using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace ArmurerieWPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;

        //variable pour dire qui est a l'écran 
        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(); //notification du changement
            }
        }

        // La commande pour btn accueil
        public ICommand NaviguerAccueilCommand { get; set; }
        public ICommand NaviguerArmesCommand { get; set; }
        public ICommand NaviguerFournisseursCommand { get; set; }
        public ICommand NaviguerPiecesCommand { get; set; }
        public ICommand NaviguerAssemblageCommand { get; set; }
        public ICommand NaviguerNouvelleVenteCommand { get; set; }
        public ICommand NaviguerCommandeCommand { get; set; }
        public ICommand NaviguerFabricationCommand { get; set; }

        public MainViewModel()
        {
            //relie la commande a la mmethode qui affiche la page d'accueil
            NaviguerAccueilCommand = new RelayCommand(o => AfficherAccueil());
            NaviguerArmesCommand = new RelayCommand(o => CurrentViewModel = new ArmesViewModel());
            NaviguerFournisseursCommand = new RelayCommand(o => CurrentViewModel = new FournisseursViewModel());
            NaviguerPiecesCommand = new RelayCommand(o => CurrentViewModel = new PiecesViewModel());
            NaviguerAssemblageCommand = new RelayCommand(o => CurrentViewModel = new AssemblageViewModel());
            NaviguerNouvelleVenteCommand = new RelayCommand(o => CurrentViewModel = new NouvelleVenteViewModel());
            NaviguerCommandeCommand = new RelayCommand(o => CurrentViewModel = new CommandeFournisseurViewModel());
            NaviguerFabricationCommand = new RelayCommand(o => CurrentViewModel = new FabricationViewModel());
            //on affiche la page d'accueil par défaut
            AfficherAccueil();
        }

        private void AfficherAccueil()
        {
            CurrentViewModel = new DashboardViewModel();
        }

        //code pour le databinding, pour notifier la vue que la propriété a changé
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
