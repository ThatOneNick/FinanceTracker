using FinanceTracker.Models;
using FinanceTracker.ViewModels;
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
            lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C");
        }
        private void btnAddIncome_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                var viewModel = (IncomeViewModel)DataContext;
                viewModel.AddIncome(amount, txtSource.Text);
                viewModel.AddToTotalIncome(amount);
                txtAmount.Text = string.Empty;
                txtSource.Text = string.Empty;
                lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C");
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
        private void btnRemoveIncome_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (IncomeViewModel)DataContext;
            Income selectedIncome = (Income)lvIncome.SelectedItem;
            viewModel.RemoveIncome(selectedIncome);
            viewModel.RemoveFromTotalIncome(selectedIncome);
            lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C");
        }
    }
}
