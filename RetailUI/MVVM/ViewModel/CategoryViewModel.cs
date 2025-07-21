using Domain.Entities;
using Domain.Entities.StoredProcudureEntities;
using Domain.Repositories;
using Domain.Repositories.SP_Repositories;
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
    public class CategoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private readonly ISQLGenericRepository<CategoryL0> _dbcatL0;
        private readonly ISQLGenericRepository<CategoryL1> _dbcatL1;
        private readonly ISQLGenericRepository<CategoryL2> _dbcatL2;
        private readonly ISQLGenericRepository<CategoryL3> _dbcatL3;
        private readonly ICategoryL0Repository _categoryL0Repository;
        private readonly ICategoryL1Repository _categoryL1Repository;
        private readonly ICategoryL2Repository _categoryL2Repository;
        private readonly ICategoryL3Repository _categoryL3Repository;
        private readonly ISQLGenericRepository<Sp_CategoryL0> _sqlcatl0;
        private readonly ISQLGenericRepository<Sp_CategoryL1> _sqlcatl1;
        private readonly ISQLGenericRepository<Sp_CategoryL2> _sqlcatl2;
        private readonly ISQLGenericRepository<Sp_CategoryL3> _sqlcatl3;
        private readonly ISPCategoryL0Repository _catL0;
        private readonly ISPCategoryL1Repository _catL1;
        private readonly ISPCategoryL2Repository _catL2;
        private readonly ISPCategoryL3Repository _catL3;
        public CategoryViewModel
         (
           ISQLGenericRepository<CategoryL0> dbcatL0,
           ISQLGenericRepository<CategoryL1> dbcatL1,
           ISQLGenericRepository<CategoryL2> dbcatL2,
           ISQLGenericRepository <CategoryL3> dbcatL3,
           ICategoryL0Repository categoryL0Repository,
           ICategoryL1Repository categoryL1Repository,
           ICategoryL2Repository categoryL2Repository,
           ICategoryL3Repository categoryL3Repository,
           ISQLGenericRepository<Sp_CategoryL0> sqlcatl0,
           ISQLGenericRepository<Sp_CategoryL1> sqlcatl1,
           ISQLGenericRepository<Sp_CategoryL2> sqlcatl2,
           ISQLGenericRepository<Sp_CategoryL3> sqlcatl3,
           ISPCategoryL0Repository catL0,
           ISPCategoryL1Repository catL1,
           ISPCategoryL2Repository catL2,
           ISPCategoryL3Repository catL3  ) 
        {
            _dbcatL0 = dbcatL0;
            _dbcatL1 = dbcatL1;
            _dbcatL2 = dbcatL2;
            _dbcatL3 = dbcatL3;
            _categoryL0Repository =categoryL0Repository;
            _categoryL1Repository =categoryL1Repository;
            _categoryL2Repository =categoryL2Repository;
            _categoryL3Repository =categoryL3Repository;
          _sqlcatl0 = sqlcatl0;
            _sqlcatl1 = sqlcatl1;
            _sqlcatl2 = sqlcatl2;
            _sqlcatl3 = sqlcatl3;
            _catL0 = catL0;
            _catL1 = catL1;
            _catL2 = catL2;
            _catL3 = catL3;
            AddCategoryL0Command = new RelayCommand(Submit);
            AddCategoryL1Command = new RelayCommand(Submit);
            AddCategoryL2Command = new RelayCommand(Submit);
            AddCategoryL3Command = new RelayCommand(Submit);
            LoadComboBoxData();
            PopulateDataGrid();
        }

        public ICommand AddCategoryL0Command { get; }
        public ICommand AddCategoryL1Command { get; }
        public ICommand AddCategoryL2Command { get; }
        public ICommand AddCategoryL3Command { get; }


        // ComboBox data collections CategoryL0Data
        public ObservableCollection<CategoryL1> CategoriesL1 { get; set; } = new ObservableCollection<CategoryL1>();
        public ObservableCollection<CategoryL2> CategoriesL2 { get; set; } = new ObservableCollection<CategoryL2>();
        public ObservableCollection<CategoryL0> CategoriesL0 { get; set; } = new ObservableCollection<CategoryL0>();
        public ObservableCollection<string> CategoryType { get; set; } = new ObservableCollection<string> {"CatL0", "CatL1", "CatL2", "CatL3" };
        public ObservableCollection<CategoryL1> CategoryL1Data { get; set; } = new ObservableCollection<CategoryL1>();
        public ObservableCollection<CategoryL2> CategoryL2Data { get; set; } = new ObservableCollection<CategoryL2>();
        public ObservableCollection<CategoryL0> CategoryL0Data { get; set; } = new ObservableCollection<CategoryL0>();
        public ObservableCollection<CategoryL3> CategoryL3Data { get; set; } = new ObservableCollection<CategoryL3>();

        private async void PopulateDataGrid()
        { 
            try {
                var categoriesL0 = await _categoryL0Repository.GetAll();
                var categoriesL1 = await _categoryL1Repository.GetAll();
                var categoriesL2 = await _categoryL2Repository.GetAll();
                var categoriesL3 = await _categoryL3Repository.GetAll();
                CategoryL0Data.Clear();
                CategoryL1Data.Clear();
                CategoryL2Data.Clear();
                CategoryL3Data.Clear();


               
                foreach (var catL0 in categoriesL0)
                {
                    Debug.WriteLine(catL0);
                    CategoryL0Data.Add(catL0);

                }

                foreach (var catL1 in categoriesL1)
                {
                    Debug.WriteLine(catL1);
                    CategoryL1Data.Add(catL1);

                }

                foreach (var catL2 in categoriesL2)
                {
                    Debug.WriteLine(catL2);
                    CategoryL2Data.Add(catL2);

                }
                foreach(var catL3 in categoriesL3)
                {
                    Debug.WriteLine(catL3);
                    CategoryL3Data.Add(catL3);
                }

            }
            catch { }
        }

        private async void LoadComboBoxData()
        {
            try
            {
                // Fetch data for each category and populate ObservableCollections
                var categoriesL0 = await _categoryL0Repository.GetAll();
                var categoriesL1 = await _categoryL1Repository.GetAll();
                var categoriesL2 = await _categoryL2Repository.GetAll();
                
                CategoriesL0.Clear();
                CategoriesL1.Clear();
                CategoriesL2.Clear();

                

                foreach (var catL0 in categoriesL0)
                {
                    Debug.WriteLine(catL0);
                    CategoriesL0.Add(catL0);
                   
                }

                foreach (var catL1 in categoriesL1)
                {
                    Debug.WriteLine(catL1);
                    CategoriesL1.Add(catL1);
                   
                }

                foreach (var catL2 in categoriesL2)
                {
                    Debug.WriteLine(catL2);
                    CategoriesL2.Add(catL2);
                  
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading ComboBox data: {ex.Message}");
            }
        }

        private string _selectedCategoryType;
        public string SelectedCategoryType
        {
            get => _selectedCategoryType;
            set
            {
                _selectedCategoryType = value;
                OnPropertyChanged();
                OnSelectedCategoryChanged(_selectedCategoryType);
            }
        }
         public async void OnSelectedCategoryChanged(string data)
        {
          if(data.Equals("CatL0"))
            {
                PopulateDataGrid();
            }
          else if(data.Equals("CatL1"))
            {

            }
          else if(data.Equals("CatL2"))
            {

            }
            else
            {

            }
        }

        private string _selectedCategoryL0Id;
        public string SelectedCategoryL0Id
        {
            get { return _selectedCategoryL0Id; }
            set
            {
                if (_selectedCategoryL0Id != value)
                {
                    _selectedCategoryL0Id = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedCategoryL2Id;
        public string SelectedCategoryL2Id
        {
            get { return _selectedCategoryL2Id; }
            set
            {
                if (_selectedCategoryL2Id != value)
                {
                    _selectedCategoryL2Id = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedCategoryL1Id;
        public string SelectedCategoryL1Id
        {
            get { return _selectedCategoryL1Id; }
            set
            {
                if (_selectedCategoryL1Id != value)
                {
                    _selectedCategoryL1Id = value;
                    OnPropertyChanged();
                }
            }
        }




        private CategoryL1 _selectedCategoryL1;
        public CategoryL1 SelectedCategoryL1
        {
            get => _selectedCategoryL1;
            set
            {
                _selectedCategoryL1 = value;
                OnPropertyChanged();
                SelectedCategoryL1Id = _selectedCategoryL1?.CatL1_Id;
               
            }
        }

        private CategoryL2 _selectedCategoryL2;
        public CategoryL2 SelectedCategoryL2
        {
            get => _selectedCategoryL2;
            set
            {
                _selectedCategoryL2 = value;
                OnPropertyChanged();
                SelectedCategoryL2Id = _selectedCategoryL2?.CatL2_Id;
              
            }
        }

        private CategoryL0 _selectedCategoryL0;
        public CategoryL0 SelectedCategoryL0
        {
            get => _selectedCategoryL0;
            set
            {
                _selectedCategoryL0 = value;
                OnPropertyChanged();
                SelectedCategoryL0Id = _selectedCategoryL0?.CatL0_Id;
            }
        }



        private string _catL0Id;
        public string CategoryL0Id
        {
            get => _catL0Id;
            set { if (_catL0Id != value) { _catL0Id = value; OnPropertyChanged(); } }
        }
        private string _catL1Id;
        public string CategoryL1Id
        {
            get => _catL1Id ;
            set
            {
                if (_catL1Id != null) { _catL1Id = value; OnPropertyChanged(); }
            }
        }
        private string _catL2Id;
        public string CategoryL2Id
        {
            get => _catL2Id;
            set
            {
                if (_catL2Id != null) { _catL2Id = value; OnPropertyChanged(); }
            }
        }
        private string _catL3Id;
        public string CategoryL3Id
        {
            get => _catL3Id;
            set
            {
                if (_catL3Id != null) { _catL3Id = value; OnPropertyChanged(); }
            }
        }
        private string _categoryL0;
        public string CategoryL0
        {
            get => _categoryL0;
            set
            {
                if (_categoryL0 != value) { _categoryL0 = value; OnPropertyChanged(); }
            }
        }
        private string _categoryL1;
        public string CategoryL1
        {
            get => _categoryL1;
            set
            {
                if (_categoryL1 != value) { _categoryL1 = value; OnPropertyChanged(); }
            }
        }
        private string _categoryL2;
        public string CategoryL2
        {
            get => _categoryL2;
            set
            {
                if (_categoryL2 != value) { _categoryL2 = value; OnPropertyChanged(); }
            }
        }
        private string _categoryL3;
        public string CategoryL3
        {
            get => _categoryL3;
            set
            {
                if (_categoryL3 != value) { _categoryL3 = value; OnPropertyChanged(); }
            }
        }




        private async void Submit(object parameter)
        {
            var cat0 = new Sp_CategoryL0()
            {
                Category_L0 = CategoryL0,
            };
            if (cat0 != null) 
            {
                var result = await _catL0.QueryStoredProcedureAsynccc(cat0);
                if(result != null)
                {
                    PopupMessage = "Added Category L0 Successfully";
                    IsPopupOpen = true;

                }
            }


            var cat1 = new Sp_CategoryL1()
            {
                CatL0_Id = CategoryL0Id,
                CategoryL1 = CategoryL1,
            };
            if(cat1 != null)
            {
                var result = await _catL1.QueryStoredProcedureAsynccc(cat1);
                if (result != null)
                {
                    PopupMessage = "Added Category L1 Successfully";
                    IsPopupOpen = true;

                }
            }

            var cat2 = new Sp_CategoryL2() 
            {
              CatL1_Id = CategoryL1Id,
              Category_L2 = CategoryL2,
            };
            if( cat2 != null)
            {
                var result = await _catL2.QueryStoredProcedureAsynccc(cat2);
                if (result != null)
                {
                    PopupMessage = "Added Category L2 Successfully";
                    IsPopupOpen = true;

                }
            }

            var cat3 = new Sp_CategoryL3()
            {
                CatL2_Id = CategoryL2Id,
                Category_L3 = CategoryL3,
            };
            if(cat3 != null)
            {
                var result = _catL3.QueryStoredProcedureAsynccc(cat3);
                if (result != null)
                {
                    PopupMessage = "Added Category L3 Successfully";
                    IsPopupOpen = true;

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
