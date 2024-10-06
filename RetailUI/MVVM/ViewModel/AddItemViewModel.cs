using Domain.Entities;
using Domain.Repositories;
using RetailUI.Core;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RetailUI.MVVM.ViewModel
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        private readonly ISQLGenericRepository<Items> _repository;
       private readonly IItemsRepository _itemsRepository;

        // Constructor with Dependency Injection
        public AddItemViewModel(IItemsRepository _itemsRepository )
        {
            _repository = _itemsRepository ?? throw new ArgumentNullException(nameof(_itemsRepository));
            Item = new Items();
            SubmitCommand = new RelayCommand(Submit);
            SubmitCommand1 = new RelayCommand(Save);
        }

        public AddItemViewModel()
        {
        }

        // Remove parameterless constructor as it will not initialize _repository
        // public AddItemViewModel() { }

        private string _itemId;
        private string _description;
        private string _colour;
        private string _size;
        private string _categoryA;
        private string _categoryB;
        private string _categoryC;
        private string _rackId;
        private Items _item;

        public Items Item
        {
            get => _item;
            set
            {
                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        public string ItemId
        {
            get => _itemId;
            set
            {
                _itemId = value;
                OnPropertyChanged(nameof(ItemId)); // Corrected from Description to ItemId
            }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public string Colour
        {
            get => _colour;
            set { _colour = value; OnPropertyChanged(nameof(Colour)); }
        }

        public string Size
        {
            get => _size;
            set { _size = value; OnPropertyChanged(nameof(Size)); }
        }

        public string CategoryA
        {
            get => _categoryA;
            set { _categoryA = value; OnPropertyChanged(nameof(CategoryA)); }
        }

        public string CategoryB
        {
            get => _categoryB;
            set { _categoryB = value; OnPropertyChanged(nameof(CategoryB)); }
        }

        public string CategoryC
        {
            get => _categoryC;
            set { _categoryC = value; OnPropertyChanged(nameof(CategoryC)); }
        }

        public string RackId
        {
            get => _rackId;
            set { _rackId = value; OnPropertyChanged(nameof(RackId)); }
        }

        // Command for Submit button
        public ICommand SubmitCommand { get; }
        public ICommand SubmitCommand1 { get; }

        private async void Save(object parameter)
        {
            Debug.WriteLine("Save");
        }
        private async void Submit(object parameter)
        {
            try
            {
                // Debug statements to check initialization
                Debug.WriteLine(_repository == null ? "Repository is null" : "Repository is initialized");

                // Ensure all fields are filled
                if (string.IsNullOrWhiteSpace(ItemId) ||
                    string.IsNullOrWhiteSpace(Description) ||
                    string.IsNullOrWhiteSpace(Colour) ||
                    string.IsNullOrWhiteSpace(Size) ||
                    string.IsNullOrWhiteSpace(CategoryA) ||
                    string.IsNullOrWhiteSpace(CategoryB) ||
                    string.IsNullOrWhiteSpace(CategoryC) ||
                    string.IsNullOrWhiteSpace(RackId))
                {
                    Debug.WriteLine("All fields must be filled out.");
                    return;
                }

                // Create a new Item object using form fields
                var item = new Items
                {
                    Item_Id = this.ItemId,
                    Description = this.Description,
                    Colour = this.Colour,
                    Size = this.Size,
                    CatL1_Id = this.CategoryA,
                    CatL2_Id = this.CategoryB,
                    CatL3_Id = this.CategoryC,
                    Rack_Id = this.RackId
                };

                Debug.WriteLine($"CatL1_Id: {item.CatL1_Id} is the data inside form.");

                // Attempt to add the item to the repository
                bool isAdded = await _repository.Add(item);
                if (isAdded)
                {
                    Debug.WriteLine("Successfully Added");
                }
                else
                {
                    Debug.WriteLine("Failed to add the item.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Occurred: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine("Button has hitted ");

        }

        public void SaveItem()
        {
            Debug.WriteLine("I am inside SaveItem");
            var item = new Items
            {
                Item_Id = this.ItemId,
                Description = this.Description,
                Colour = this.Colour,
                Size = this.Size,
                CatL1_Id = this.CategoryA,
                CatL2_Id = this.CategoryB,
                CatL3_Id = this.CategoryC,
                Rack_Id = this.RackId
            };

            _repository.Add(item);

        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

//using Domain.Entities;
//using Domain.Repositories;
//using Domain.Services.EntityServices;
//using Domain.Services.HelperServices;
//using RetailUI.Core;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Windows.Input;

//namespace RetailUI.MVVM.ViewModel
//{
//    public class AddItemViewModel : INotifyPropertyChanged
//    {
//        private readonly ISQLGenericRepository<Items> _repository;

//        // Constructor with Dependency Injection
//        public AddItemViewModel(ISQLGenericRepository<Items> repository)
//        {
//            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
//            Item = new Items();
//            SubmitCommand = new RelayCommand(Submit);
//        }
//        public AddItemViewModel() : this(new SQLGenericRepository<Items>(new ConnectionStringBuilder())) // Ensure to pass a valid connection string builder
//        {
//            // This allows XAML to instantiate the ViewModel
//        }
//        // Properties representing form fields
//        private Items _item;
//        private string _itemId;
//        private string _description;
//        private string _colour;
//        private string _size;
//        private string _categoryA;
//        private string _categoryB;
//        private string _categoryC;
//        private string _rackId;

//        public Items Item
//        {
//            get => _item;
//            set
//            {
//                _item = value;
//                OnPropertyChanged(nameof(Item)); // Notifies UI of the change (if bound)
//            }
//        }
//        public string ItemId
//        {
//            get => _itemId;
//            set { _itemId = value; OnPropertyChanged(); }
//        }

//        public string Description
//        {
//            get => _description;
//            set { _description = value; OnPropertyChanged(); }
//        }

//        public string Colour
//        {
//            get => _colour;
//            set { _colour = value; OnPropertyChanged(); }
//        }

//        public string Size
//        {
//            get => _size;
//            set { _size = value; OnPropertyChanged(); }
//        }

//        public string CategoryA
//        {
//            get => _categoryA;
//            set { _categoryA = value; OnPropertyChanged(); }
//        }

//        public string CategoryB
//        {
//            get => _categoryB;
//            set { _categoryB = value; OnPropertyChanged(); }
//        }

//        public string CategoryC
//        {
//            get => _categoryC;
//            set { _categoryC = value; OnPropertyChanged(); }
//        }

//        public string RackId
//        {
//            get => _rackId;
//            set { _rackId = value; OnPropertyChanged(); }
//        }

//        // Command for Submit button
//        public ICommand SubmitCommand { get; }

//        // Submit method that handles adding the item
//        private async void Submit(object parameter)
//        {
//            try
//            {
//                // Ensure all fields are filled
//                if (string.IsNullOrWhiteSpace(ItemId) || string.IsNullOrWhiteSpace(Description) ||
//                    string.IsNullOrWhiteSpace(Colour) || string.IsNullOrWhiteSpace(Size) ||
//                    string.IsNullOrWhiteSpace(CategoryA) || string.IsNullOrWhiteSpace(CategoryB) ||
//                    string.IsNullOrWhiteSpace(CategoryC) || string.IsNullOrWhiteSpace(RackId))
//                {
//                    Debug.WriteLine("All fields must be filled out.");
//                    return;
//                }

//                // Create a new Item object using form fields
//                var item = new Items
//                {
//                    Item_Id = this.ItemId,
//                    Description = this.Description,
//                    Colour = this.Colour,
//                    Size = this.Size,
//                    CatL1_Id = this.CategoryA,
//                    CatL2_Id = this.CategoryB,
//                    CatL3_Id = this.CategoryC,
//                    Rack_Id = this.RackId
//                };

//                // Attempt to add the item to the repository
//                bool isAdded = await _repository.Add(item);
//                if (isAdded)
//                {
//                    Debug.WriteLine("Item successfully added");
//                }
//                else
//                {
//                    Debug.WriteLine("Failed to add the item");
//                }
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"Error occurred: {ex.Message}");
//            }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//public void SaveItem()
//{
//    Debug.WriteLine("I am inside SaveItem");
//    var item = new Items
//    {
//        Item_Id = this.ItemId,
//        Description = this.Description,
//        Colour = this.Colour,
//        Size = this.Size,
//        CatL1_Id = this.CategoryA,
//        CatL2_Id = this.CategoryB,
//        CatL3_Id = this.CategoryC,
//        Rack_Id = this.RackId
//    };

//    _repository.Add(item);

//}
//        // Method to raise PropertyChanged event
//        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }   
//}
