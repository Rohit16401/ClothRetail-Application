using Domain.Entities;
using Domain.Repositories;
using Domain.Services.EntityServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailUI.MVVM.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private AddItemViewModel _addItemViewModel;
        private readonly ISQLGenericRepository<Items> _repository;

        public AddItemViewModel AddItemViewModel
        {
            get => _addItemViewModel;
            set
            {
                _addItemViewModel = value;
                OnPropertyChanged(nameof(AddItemViewModel)); // Notify that the property has changed
            }
        }

        public HomeViewModel()
        {
            // Initialize the AddItemViewModel here
            AddItemViewModel = new AddItemViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
