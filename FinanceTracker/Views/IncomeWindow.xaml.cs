using FinanceTracker.Models;
using FinanceTracker.ViewModels;
using System.Globalization;
using System.Windows;

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for IncomeWindow.xaml
    /// </summary>
    public partial class IncomeWindow : Window
    {
        public IncomeWindow()
        {
            InitializeComponent();
            DataContext = new IncomeViewModel();
            var viewModel = (IncomeViewModel)DataContext;
            viewModel.LoadIncome();
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C", culture);
        }
        private void btnAddIncome_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var viewModel = (IncomeViewModel)DataContext;
                viewModel.AddIncome(amount, txtSource.Text, date);
                viewModel.AddToTotalIncome(amount);
                txtAmount.Text = string.Empty;
                txtSource.Text = string.Empty;
                CultureInfo culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;
                lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C", culture);
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
        private void btnRemoveIncome_Click(object sender, RoutedEventArgs e)
        {
            if (lvIncome.SelectedItem != null)
            {
                var viewModel = (IncomeViewModel)DataContext;
                Income selectedIncome = (Income)lvIncome.SelectedItem;
                viewModel.RemoveIncome(selectedIncome);
                viewModel.RemoveFromTotalIncome(selectedIncome);
                CultureInfo culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;
                lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C", culture); 
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

        private void btnUpdateIncome_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                btnAddIncome.Visibility = Visibility.Visible;
                btnUpdateIncome.Visibility = Visibility.Collapsed;
                Income selectedIncome = (Income)lvIncome.SelectedItem;
                string source = txtSource.Text;
                DateOnly date = selectedIncome.Date;
                var viewModel = (IncomeViewModel)DataContext;
                viewModel.UpdateIncome(selectedIncome, amount, source, date);
                viewModel.RemoveFromTotalIncome(selectedIncome);
                viewModel.AddToTotalIncome(amount);
                txtAmount.Text = string.Empty;
                txtSource.Text = string.Empty;
                CultureInfo culture = new CultureInfo("en-US");
                culture.NumberFormat.CurrencyNegativePattern = 1;
                lblTotalIncome.Content = "Total Cost: " + viewModel.totalAmount.ToString("C", culture);
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
        private void lvIncome_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvIncome.SelectedItems.Count == 1)
            {
                var income = (Income)lvIncome.SelectedItem;
                txtAmount.Text = income.Amount.ToString();
                txtSource.Text = income.Source.ToString();
                btnAddIncome.Visibility = Visibility.Collapsed;
                btnUpdateIncome.Visibility = Visibility.Visible;
            }
            else
            {
                return;
            }
        }
    }
}
