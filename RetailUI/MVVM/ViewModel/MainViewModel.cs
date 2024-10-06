using Domain.Entities;
using Domain.Services.EntityServices;
using RetailUI.Core;
using System.Diagnostics;
using System.Windows.Input;
using Domain.Repositories;
using Domain.Entities;
using Domain.Services;

namespace RetailUI.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private readonly ISQLGenericRepository<Items> _repository;
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand AddItemViewCommand { get; set; }
        public RelayCommand RackCommand { get; set; }
        public RelayCommand GoodsReceivedCommand { get; set; }
        public ICommand ItemSaveCommand { get; set; }

        private object _currentView;
        public HomeViewModel HomeVm { get; set; }
        public AddItemViewModel AddItemVm { get; set; }
        public RackViewModel RackVM { get; set; }    
        public GoodsReceivedViewModel GoodsReceivedVm { get; set; }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            AddItemVm = new AddItemViewModel();
            RackVM= new RackViewModel();
            GoodsReceivedVm = new GoodsReceivedViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVm;
                Debug.WriteLine(CurrentView);
            });

            AddItemViewCommand = new RelayCommand(o =>
            {
                CurrentView = AddItemVm;
                Debug.WriteLine(CurrentView);
            });
            RackCommand = new RelayCommand(o =>
            {
                CurrentView = RackVM;
                Debug.WriteLine(CurrentView);
            });
            GoodsReceivedCommand = new RelayCommand(o =>
            {
                CurrentView = GoodsReceivedVm;
                Debug.WriteLine(CurrentView);
            });

        }
    }
}
