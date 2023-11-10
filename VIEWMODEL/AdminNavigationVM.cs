using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Ya_.UTILITIES;

namespace Ya_.VIEWMODEL
{
   public class AdminNavigationVM: ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

       
        public ICommand StatisticsCommand { get; set; }
        public ICommand SongManagerCommand { get; set; }
        public ICommand ComplitationManagerCommand { get; set; }

       
        private void Statistics(object obj) => CurrentView = new StatisticsVM();
        private void SongManager(object obj) => CurrentView = new SongManagerVM();
        private void ComplitationManager(object obj) => CurrentView = new ComplitationVM();
        public AdminNavigationVM()
        {
          
            StatisticsCommand = new RelayCommand(Statistics);
            SongManagerCommand = new RelayCommand(SongManager);
            ComplitationManagerCommand = new RelayCommand(ComplitationManager);
            // Startup Page
            CurrentView = new StatisticsVM();
        }
    }
}
