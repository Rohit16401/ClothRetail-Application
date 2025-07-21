using Microsoft.Extensions.DependencyInjection;
using RetailUI.MVVM.ViewModel;
using System.Windows.Controls;

namespace RetailUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerViewModel viewModel;
        public CustomerView()
        {
            InitializeComponent();
            viewModel = App.ServiceProvider.GetRequiredService<CustomerViewModel>();
            this.DataContext = viewModel;
        }
    }
}
