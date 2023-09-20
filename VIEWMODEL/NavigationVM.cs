using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ya_.UTILITIES;
using System.Windows.Input;
using Ya_.VIEWMODEL;

namespace Ya_.VIEWMODEL
{


    public class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand SongCommand { get; set; }
        public ICommand ProductsCommand { get; set; }
        public ICommand OrdersCommand { get; set; }
        public ICommand TransactionsCommand { get; set; }
        public ICommand ShipmentsCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Songs(object obj) => CurrentView = new SongsVM();


        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            SongCommand = new RelayCommand(Songs);

            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}
