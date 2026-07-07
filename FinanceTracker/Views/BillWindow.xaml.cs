using FinanceTracker.Models;
using FinanceTracker.ViewModels;
using System.Windows;

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for BillsWindow.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        public BillWindow()
        {
            InitializeComponent();
            DataContext = new BillViewModel();
            var viewModel = (BillViewModel)DataContext;
            viewModel.LoadBill();
            lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C");
        }
        private void btnAddBill_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var viewModel = (BillViewModel)DataContext;
                viewModel.AddBill(amount, txtSource.Text, date);
                viewModel.AddToTotalCost(amount);
                txtAmount.Text = string.Empty;
                txtSource.Text = string.Empty;
                lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C");
            }
            else if (string.IsNullOrWhiteSpace(txtAmount.Text) || string.IsNullOrWhiteSpace(txtSource.Text))
            {
                MessageBox.Show("Please enter an amount and source.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Amount must be numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnRemoveBill_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (BillViewModel)DataContext;
            Bill selectedBill = (Bill)lvBills.SelectedItem;
            viewModel.RemoveBill(selectedBill);
            viewModel.RemoveFromTotalCost(selectedBill);
            lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C");
        }
    }
}
