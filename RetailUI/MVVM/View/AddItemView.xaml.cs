using Domain.Entities;
using Domain.Repositories;
using Domain.Services.EntityServices;
using Domain.Services.HelperServices;
using RetailUI.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RetailUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddItemView.xaml
    /// </summary>
    public partial class AddItemView : UserControl
    {
        public AddItemView()
        {
            InitializeComponent();
            DataContext = new AddItemViewModel();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (AddItemViewModel)this.DataContext;
            viewModel.SaveItem();
        }

    }
}
