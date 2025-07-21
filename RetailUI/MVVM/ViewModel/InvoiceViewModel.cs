using Domain.Entities;
using Domain.Entities.StoredProcudureEntities;
using Domain.Repositories;
using Domain.Repositories.SP_Repositories;
using RetailUI.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RetailUI.MVVM.ViewModel
{

    public class InvoiceViewModel : INotifyPropertyChanged
    {
        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand SubmitCommand { get; }
        private readonly ISQLGenericRepository<Cart_View> _cartRepositories;
        private readonly ICartRepository _cartRepository;
        private readonly ISQLGenericRepository<Items> _itemRepositories;
        private readonly ISQLGenericRepository<Retail_Invoice> _retailRepositories;
        private readonly ISQLGenericRepository<WholeSale_Invoice> _wholeSaleRepositories;
        private readonly IWholeSaleInvoiceRepository _wholeSaleInvoiceRepository;
        private readonly IRetailInvoiceRepository _retailInvoiceRepository;
        private readonly ISpRetailInvoiceRepository _spRetail;
        private readonly ISpWholeSaleInvoiceRepository _spWholesale;
        private readonly ISQLGenericRepository<Sp_RetailInvoiceItem> _spRetailInvoiceItemRepository;
        private readonly ISpRetailInvoiceItemRepository _spRetailItemRepository;
        private readonly ISQLGenericRepository<Sp_WholesaleInvoiceItem> _spWholesaleInvoiceItemRepository;
        private readonly ISPWholeSaleInvoiceItemRepository _spWholesaleItemRepository;
        public InvoiceViewModel(
            ISQLGenericRepository<Sp_RetailInvoiceItem> spRetailInvoiceItemRepository,
            ISQLGenericRepository<Cart_View> cartRepositories,
            ICartRepository cartRepository,
            ISQLGenericRepository<Items> itemRepositories,
            ISQLGenericRepository<Retail_Invoice> retailRepositories,
            ISQLGenericRepository<WholeSale_Invoice> wholesaleRepositories,
            IWholeSaleInvoiceRepository wholeSaleInvoiceRepository,
            IRetailInvoiceRepository retailInvoiceRepository,
            ISpRetailInvoiceRepository spRetail,
            ISpWholeSaleInvoiceRepository spWholesale,
            ISpRetailInvoiceItemRepository spRetailItemRepository,
            ISQLGenericRepository<Sp_WholesaleInvoiceItem> spWholesaleInvoiceItemRepository,
            ISPWholeSaleInvoiceItemRepository spWholesaleItemRepository
            )
        {
            _spWholesaleInvoiceItemRepository = spWholesaleInvoiceItemRepository;
            _spWholesaleItemRepository = spWholesaleItemRepository;
            _spRetailInvoiceItemRepository = spRetailInvoiceItemRepository;
            _spRetailItemRepository = spRetailItemRepository;
            _spRetail = spRetail;
            _spWholesale = spWholesale;
            _cartRepositories = cartRepositories;
            _cartRepository = cartRepository;
            _itemRepositories = itemRepositories ?? throw new ArgumentNullException( nameof( itemRepositories ) );
            _retailRepositories = retailRepositories ?? throw new ArgumentNullException(nameof( retailRepositories ) );
            _wholeSaleRepositories = wholesaleRepositories ?? throw new ArgumentNullException(nameof ( wholesaleRepositories ) );
            _wholeSaleInvoiceRepository = wholeSaleInvoiceRepository ?? throw new ArgumentNullException(nameof(wholeSaleInvoiceRepository));
            _retailInvoiceRepository = retailInvoiceRepository ?? throw new ArgumentNullException(nameof(retailInvoiceRepository) );
            SubmitCommand = new RelayCommand(Submit);
            CustomerType = new ObservableCollection<string>
            {
                "Retailer",
                "WholeSaler"
            };
        }

        public ObservableCollection<Cart_View> InvoiceGrid { get; set; } = new ObservableCollection<Cart_View>();
        public ObservableCollection<string> CustomerType { get; set; }

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


        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName  = value;
                OnPropertyChanged( nameof( CustomerName ) );
            }
        }
        private string _contactNumber;
        public string ContactNumber
        {
            get => _contactNumber;
            set {
                _contactNumber = value;
                OnPropertyChanged( nameof( ContactNumber ) );
            }
        }
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }
        private string _customerType;
      
        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof( Customer ) );
            }
        }

        private string _barcodeInput;
        public string BarcodeInput
        {
            get =>_barcodeInput;
            set
            {
                _barcodeInput = value;
                OnPropertyChanged(nameof(BarcodeInput));
            }
        }

        private double _totalAmount;
        public double totalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(totalAmount));
                UpdateFinalAmount();
            }
        }

        private double _discount;
        public double discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged(nameof(discount));
                UpdateFinalAmount();
                UpdateFinalPrices(_discount);
            }
        }

        private double _finalAmount;
        public double finalAmount
        {
            get => _finalAmount;
            set
            {
                _finalAmount = value;
                OnPropertyChanged(nameof(finalAmount));
            }
        }
        private void UpdateFinalAmount()
        {
            finalAmount =  totalAmount - discount;
        }

        public void RefreshCollection()
        {
            var itemsCopy = InvoiceGrid.ToList();    // Make a copy of current items
            InvoiceGrid.Clear();                    // Clear the collection
            foreach (var item in itemsCopy)        // Re-add each item
            {
                InvoiceGrid.Add(item);
            }
        }
        public async void UpdateFinalPrices(double discountPercentage)
        {
           
            foreach (var item in InvoiceGrid)
           {
                var data = await _cartRepository.GetById(item.Item_Id); // IT WILL TAKE VALUE FROM DATABASE ITSELF AND UPDATE VALUE IN INVOICEGRID
                var existingItem = InvoiceGrid.FirstOrDefault(i => i.Item_Id == data.Item_Id);
                double dis = (discount * data.FinalSale_Price * existingItem.Qty) / totalAmount;
                double value = data.FinalSale_Price * (1 - (discountPercentage / totalAmount)) * existingItem.Qty;
             
                if (value - Math.Floor(value) >= 0.50)   //  Check if the decimal portion is 0.50 or greater
                {
                    value = (int)Math.Ceiling(value);  // Round down to the original integer
                }
                else
                {                    
                    value = (int)Math.Floor(value);   // Round down to the original integer
                }
                if (dis - Math.Floor(dis) >= 0.50)   //  Check if the decimal portion is 0.50 or greater
                {
                    dis = (int)Math.Ceiling(dis);  // Round down to the original integer
                }
                else
                {
                    dis = (int)Math.Floor(dis);   // Round down to the original integer
                }
                item.Discount = dis;
                item.FinalSale_Price = value;
            }
            RefreshCollection(); 
        }
        
        
        public async void ProcessScannedBarcode(string barcode)
        {
            var data =  await _cartRepository.GetById(barcode);
           
            var existingItem = InvoiceGrid.FirstOrDefault(i => i.Item_Id == data.Item_Id);
            if (data != null)
            {
                if (existingItem != null)
                { 

                    existingItem.Qty += 1;
                    existingItem.FinalSale_Price = existingItem.Qty * existingItem.FinalSale_Price;
                    RefreshCollection();
                    Debug.WriteLine("ttttttttttttttt", existingItem);
                }
                else
                {
                    data.Discount = 0;
                     data.Qty = 1;
                    InvoiceGrid.Add(data);

                }

                totalAmount = InvoiceGrid.Sum(item => item.FinalSale_Price);

            }
           
        }

        private async  void Submit(object parameter )
        {
            if(SelectedCustomerType == "Retailer")
            {
                var invoice = new Sp_RetailInvoice
                {
                    Date = DateTime.Today,
                    PhoneNo = "8530122036",
                    GrandTotal = totalAmount,
                    Type = "Retailer",
                    Discount = discount,
                    

                };
                var invoiceid = await _spRetail.QueryStoredProcedureAsynccc(invoice);
                if (invoiceid != null) 
                {
                    for (int i = 0; i < InvoiceGrid.Count(); i++) 
                    {
                        Cart_View inv = InvoiceGrid[i];
                        var itemdetails = new Sp_RetailInvoiceItem
                        {
                            Invoice_Id =invoiceid,
                           
                            Date = DateTime.Today,
                            Qty=inv.Qty,
                            SubDiscount = inv.Discount,
                            FinalSale_Price=inv.FinalSale_Price * (1-(discount/totalAmount)),
                            Item_Id=inv.Item_Id,
                            MRP=inv.MRP,
                            
                        };
                        var a = await _spRetailItemRepository.QueryStoredProcedureAsync(itemdetails);

                    }
                  
                    InvoiceGrid.Clear();
                    CustomerName = "";
                    ContactNumber = "";
                    City = "";
                    SelectedCustomerType = "";
                    totalAmount = 0;
                    discount = 0;
                    finalAmount = 0;
                    
                }

                Debug.WriteLine("Invoice ID :- ", invoiceid);
            }
            else if(SelectedCustomerType == "WholeSaler")
            {
                var wholesale = new Sp_WholeSaleInvoice
                {
                    Date = DateTime.Today,
                    PhoneNo ="8530122036",
                    Type = "WholeSaler",
                    Discount = discount,
                    GST = "120",
                    Grand_Total = totalAmount,
                };
                var invoiceid = await _spWholesale.QueryStoredProcedureAsynccc(wholesale);
                if(invoiceid != null)
                {
                    for (int i = 0; i < InvoiceGrid.Count(); i++)
                    {
                        Cart_View inv = InvoiceGrid[i];
                        var itemdetails = new Sp_WholesaleInvoiceItem
                        {
                            Invoice_Id = invoiceid,
                            Qty = inv.Qty,
                            SubDiscount = inv.Discount,
                            Final_Price = inv.FinalSale_Price * (1 - (discount / totalAmount)),
                            Item_Id = inv.Item_Id,
                            MRP = inv.MRP,

                        };
                        var a = await _spWholesaleItemRepository.QueryStoredProcedureAsync(itemdetails);

                    }
                    InvoiceGrid.Clear();
                    CustomerName = "";
                    ContactNumber = "";
                    City = "";
                    SelectedCustomerType = "";
                    totalAmount = 0;
                    discount = 0;
                    finalAmount = 0;
                }
                //_spWholesaleItemRepository   _spWholesale;



            }



        }
    }
}
