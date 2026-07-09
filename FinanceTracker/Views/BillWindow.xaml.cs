using FinanceTracker.Models;
using FinanceTracker.ViewModels;
using System.Globalization;
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
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C", culture);
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
                CultureInfo culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;
                lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C", culture);
            }
            else if (string.IsNullOrWhiteSpace(txtAmount.Text) || string.IsNullOrWhiteSpace(txtSource.Text))
            {
                MessageBox.Show("Please enter a value for amount and source.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Amount must be numbers only.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnRemoveBill_Click(object sender, RoutedEventArgs e)
        {
            if (lvBills.SelectedItem != null)
            {
                var viewModel = (BillViewModel)DataContext;
                Bill selectedBill = (Bill)lvBills.SelectedItem;
                viewModel.RemoveBill(selectedBill);
                viewModel.RemoveFromTotalCost(selectedBill);
                CultureInfo culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;
                lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C", culture); 
            } else
            {
                MessageBox.Show("An item must be selected to remove.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnUpdateBill_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                btnAddBill.Visibility = Visibility.Visible;
                btnUpdateBill.Visibility = Visibility.Collapsed;
                Bill selectedBill = (Bill)lvBills.SelectedItem;
                string source = txtSource.Text;
                DateOnly date = selectedBill.Date;
                var viewModel = (BillViewModel)DataContext;
                viewModel.UpdateBill(selectedBill, amount, source, date);
                viewModel.RemoveFromTotalCost(selectedBill);
                viewModel.AddToTotalCost(amount);
                txtAmount.Text = string.Empty;
                txtSource.Text = string.Empty;
                CultureInfo culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;
                lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C", culture);
            }
            else if (string.IsNullOrWhiteSpace(txtAmount.Text) || string.IsNullOrWhiteSpace(txtSource.Text))
            {
                MessageBox.Show("Please enter a value for amount and source.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Amount must be numbers only.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lvBills_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvBills.SelectedItems.Count == 1)
            {
                var bill = (Bill)lvBills.SelectedItem;
                txtAmount.Text = bill.Amount.ToString();
                txtSource.Text = bill.Source.ToString();
                btnAddBill.Visibility = Visibility.Collapsed;
                btnUpdateBill.Visibility = Visibility.Visible; 
            } else
            {
                return;
            }
        }
    }
}