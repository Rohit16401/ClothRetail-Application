using Domain.Entities;
using Domain.Repositories;
using RetailUI.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RetailUI.MVVM.ViewModel
{
    public class CustomerViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ISQLGenericRepository<Customer> _isqlCustomer;
        private readonly ICustomersRepository _customers;
        public CustomerViewModel(
            ISQLGenericRepository<Customer> isqlCustomer,
            ICustomersRepository customers
            ) 
        { 
            _isqlCustomer = isqlCustomer;
          _customers = customers;
            SubmitCommand = new RelayCommand(Submit);
            UpdateCommand = new RelayCommand(Update);
            CustomerType = new ObservableCollection<string>
            {
                "Retailer",
                "WholeSaler"
            };
            PopulateDataGrid();
        }
        public ICommand SubmitCommand { get; }
        public ICommand UpdateCommand { get; }

        public ObservableCollection<string> CustomerType { get; set; }
        //Populating Datagrid with customer info
        public ObservableCollection<Customer> Customers {  get; set; } = new ObservableCollection<Customer>();
        public async void PopulateDataGrid()
        {
            var data = await _customers.GetAll();
            Customers.Clear();
            if (data != null)
            {
                foreach (var item in data)
                Customers.Add(item);
            }
        }


        //populating form when user select any data from data grid
        public async void PopulateForm(Customer selectedcustomer)
        {
            Name = selectedcustomer.Name;
            Type = selectedcustomer.Type;
            SelectedCustomerType = selectedcustomer.Type;
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

        private string _selectedCustomerType;
        public string SelectedCustomerType
        {
            get => _selectedCustomerType;
            set
            {
                _selectedCustomerType = value;
                OnPropertyChanged(nameof(SelectedCustomerType));
               
            }
        }

        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
                PopulateForm(_selectedCustomer);
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
        private Customer _customer;
        public Customer Customer
        {
            get { return _customer; }
            set {
                  if (_customer != value)
                {  _customer = value;}
              }
        }
        private bool IsFormValid()
        {
            return
          
                   !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(City) &&
                   SelectedCustomerType != null;
        }

        private async void Submit(object parameter)
        {
            if (!IsFormValid())
            {
                Debug.WriteLine("All fields must be filled out.");
                return;
            }
            var customer = new Customer
            {
                Name = Name,
                Phone_No = Phone,
                City = City,
                Type = SelectedCustomerType
            };

            try
            {
                var abc = await _customers.Add(customer);
                Debug.WriteLine("Inserted Successfully ");
                PopupMessage = "Item added successfully!";
                IsPopupOpen = true;
                Name = "";
                Phone = "";
                City = "";
                SelectedCustomerType = "";
            }
            catch (Exception ex)
            { 
              Debug.WriteLine($"{ex.Message}");
            }
        }
        private async void Update(object parameter)
        {
            var cutomer = new Customer()
            { 
             Type = SelectedCustomerType,
             Name = Name,
             Phone_No = Phone,
             City = City,
            };

            if(cutomer != null)
            {
              var data =  _customers.Update(cutomer);
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
