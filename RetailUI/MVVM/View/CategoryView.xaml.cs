using Microsoft.Extensions.DependencyInjection;
using RetailUI.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RetailUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        CategoryViewModel viewModel;
        public CategoryView()
        {
            InitializeComponent();
            viewModel = App.ServiceProvider.GetRequiredService<CategoryViewModel>();
            this.DataContext = viewModel;
        }
    }
}
