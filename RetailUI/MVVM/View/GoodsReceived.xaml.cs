using RetailUI.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace RetailUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for GoodsReceived.xaml
    /// </summary>
    public partial class GoodsReceived : UserControl
    {
        public GoodsReceived()
        {
            InitializeComponent();
            DataContext = new GoodsReceivedViewModel();

        }
        private void AddNewRow_Click(object sender, RoutedEventArgs e)
        {
            // Create a new Grid row for dynamic input fields
            Grid newRow = new Grid
            {
                Margin = new Thickness(0, 10, 0, 0) // Adding some spacing between rows
            };

            // Create the same column structure for the new row
            for (int i = 0; i < 14; i++) // 13 for fields + 1 for Remove button
            {
                newRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Add TextBoxes and DatePicker for each column as before
            newRow.Children.Add(CreateTextBox(0));
            newRow.Children.Add(CreateTextBox(1));
            newRow.Children.Add(CreateDatePicker(2)); // For DatePicker column
            newRow.Children.Add(CreateTextBox(3));
            newRow.Children.Add(CreateTextBox(4));
            newRow.Children.Add(CreateTextBox(5));
            newRow.Children.Add(CreateTextBox(6));
            newRow.Children.Add(CreateTextBox(7));
            newRow.Children.Add(CreateTextBox(8));
            newRow.Children.Add(CreateTextBox(9));
            newRow.Children.Add(CreateTextBox(10));
            newRow.Children.Add(CreateTextBox(11));
            newRow.Children.Add(CreateTextBox(12));

            // Add the Remove button to the last column (13th index)
            Button removeButton = new Button
            {
                Content = "Remove",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = System.Windows.Media.Brushes.Red,
                Foreground = System.Windows.Media.Brushes.White,
                Padding = new Thickness(5)
            };

            // Set the removeButton click event handler to remove the row
            removeButton.Click += (s, e) => RemoveRow(newRow);

            // Place the button in the last column of the grid
            Grid.SetColumn(removeButton, 13);
            newRow.Children.Add(removeButton);

            // Add the new row to the StackPanel (DynamicRowsPanel)
            DynamicRowsPanel.Children.Add(newRow);
        }

        // Event handler for removing the non-dynamic initial row
        private void RemoveNonDynamicRow_Click(object sender, RoutedEventArgs e)
        {
            // Remove the non-dynamic row's controls (initial row is at index 1)
            DynamicRowsPanel.Children.Remove(ItemIdTextBox);
            DynamicRowsPanel.Children.Remove(VendorIdTextBox);
            DynamicRowsPanel.Children.Remove(InDatePicker);
            DynamicRowsPanel.Children.Remove(QuantityTextBox);
            DynamicRowsPanel.Children.Remove(StockTypeIDTextBox);
            DynamicRowsPanel.Children.Remove(RackIdTextBox);
            DynamicRowsPanel.Children.Remove(BuyPriceTextBox);
            DynamicRowsPanel.Children.Remove(StaticChargeTextBox);
            DynamicRowsPanel.Children.Remove(GSTTextBox);
            DynamicRowsPanel.Children.Remove(SizeTextBox);
            DynamicRowsPanel.Children.Remove(MRPTextBox);
            DynamicRowsPanel.Children.Remove(DiscountTextBox);
            DynamicRowsPanel.Children.Remove(FinalSalePriceTextBox);

            // Add removal for all other controls in the non-dynamic row (QuantityTextBox, etc.)
        }

        // Event handler for removing a dynamic row
        private void RemoveRow(Grid row)
        {
            DynamicRowsPanel.Children.Remove(row);
        }

        // Helper methods for creating TextBox and DatePicker
        TextBox CreateTextBox(int column)
        {
            TextBox textBox = new TextBox
            {
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            // Set the Grid column for the TextBox
            Grid.SetColumn(textBox, column);
            return textBox;
        }

        DatePicker CreateDatePicker(int column)
        {
            DatePicker datePicker = new DatePicker
            {
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center
            };

            // Set the Grid column for the DatePicker
            Grid.SetColumn(datePicker, column);
            return datePicker;
        }

    }
}
