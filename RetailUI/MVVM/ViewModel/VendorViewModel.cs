using Domain.Entities;
using Domain.Repositories;
using RetailUI.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RetailUI.MVVM.ViewModel
{
    public class VendorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ISQLGenericRepository<Vendor> _isqlVendor;
        private readonly IVendorRepository _vendors;
        public VendorViewModel(
            ISQLGenericRepository<Vendor> isqlVendor,
            IVendorRepository vendors
            )
        {
            _isqlVendor = isqlVendor;
            _vendors = vendors;
            SubmitCommand = new RelayCommand(Submit);
            UpdateCommand = new RelayCommand(Update);
            ClosePopupCommand = new RelayCommand(ClosePopup);
            VendorType = new ObservableCollection<string>
            {
                "Retailer",
                "WholeSaler"
            };
            PopulateDataGrid();
        }
        public ICommand SubmitCommand { get; }
        public ICommand UpdateCommand { get; }

        public ObservableCollection<string> VendorType { get; set; }
        //Populating Datagrid with customer info
        public ObservableCollection<Vendor> Vendors { get; set; } = new ObservableCollection<Vendor>();
        public async void PopulateDataGrid()
        {
            var data = await _vendors.GetAll();
            Vendors.Clear();
            if (data != null)
            {
                foreach (var item in data)
                    Vendors.Add(item);
            }
        }


        //populating form when user select any data from data grid
        public async void PopulateForm(Vendor selectedcustomer)
        {
            Name = selectedcustomer.Name;
            Type = selectedcustomer.Type;
            SelectedVendorType = selectedcustomer.Type;
            Phone = selectedcustomer.Phone_No;
            City = selectedcustomer.City;

            IsAddButtonVisible = false;
            IsUpdateButtonVisible = true;
        }




        private bool _isAddButtonVisible = true;
        public bool IsAddButtonVisible
        {
            get => _isAddButtonVisible;
            set
            {
                _isAddButtonVisible = value;
                OnPropertyChanged(nameof(IsAddButtonVisible));
            }
        }

        private bool _isUpdateButtonVisible = false;
        public bool IsUpdateButtonVisible
        {
            get => _isUpdateButtonVisible;
            set
            {
                _isUpdateButtonVisible = value;
                OnPropertyChanged(nameof(IsUpdateButtonVisible));
            }
        }

        private string _selectedVendorType;
        public string SelectedVendorType
        {
            get => _selectedVendorType;
            set
            {
                _selectedVendorType = value;
                OnPropertyChanged(nameof(SelectedVendorType));

            }
        }

        private Vendor _selectedVendor;
        public Vendor SelectedVendor
        {
            get => _selectedVendor;
            set
            {
                _selectedVendor = value;
                OnPropertyChanged(nameof(SelectedVendor));
                PopulateForm(_selectedVendor);
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }
        private Vendor _vendor;
        public Vendor Vendor
        {
            get { return _vendor; }
            set
            {
                if (_vendor != value)
                { _vendor = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool IsFormValid()
        {
            return

                   !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(City) &&
                   SelectedVendorType != null;
        }

        private async void Submit(object parameter)
        {
            if (!IsFormValid())
            {
                Debug.WriteLine("All fields must be filled out.");
                return;
            }
            var vendor = new Vendor
            {
                Name = Name,
                Phone_No = Phone,
                City = City,
                Type = SelectedVendorType
            };

            try
            {
                var abc = await _vendors.Add(vendor);
                Debug.WriteLine("Inserted Successfully ");
                PopupMessage = "Item added successfully!";
                IsPopupOpen = true;
                Name = "";
                Phone = "";
                City = "";
                SelectedVendorType = "";
                if(abc != null)
                { PopulateDataGrid(); }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }
        }
        private async void Update(object parameter)
        {
            var vendor = new Vendor()
            {
                Type = SelectedVendorType,
                Name = Name,
                Phone_No = Phone,
                City = City,
            };

            if (vendor != null)
            {
                var data = _vendors.Update(vendor);
                if (data != null)
                {
                    PopupMessage = "Item updated successfully!";
                    IsPopupOpen = true;
                    Debug.WriteLine("Updated Successfully");
                }
            }
        }


        //Popup Notification Purpose code
        private bool _isPopupOpen;
        private string _popupMessage;

        public bool IsPopupOpen
        {
            get => _isPopupOpen;
            set
            {
                _isPopupOpen = value;
                OnPropertyChanged();
            }
        }

        public string PopupMessage
        {
            get => _popupMessage;
            set
            {
                _popupMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClosePopupCommand { get; }


        private void ClosePopup(object parameter)
        {
            IsPopupOpen = false;
        }
    }
}
