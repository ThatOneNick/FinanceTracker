using FinanceTracker.Models;
using FinanceTracker.ViewModels;
using System.Globalization;
using System.Windows;

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for SubscriptionWindow.xaml
    /// </summary>
    public partial class SubscriptionWindow : Window
    {
        public SubscriptionWindow()
        {
            InitializeComponent();
            DataContext = new SubscriptionViewModel();
            var viewModel = (SubscriptionViewModel)DataContext;
            viewModel.LoadSubscription();
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C", culture);
        }
        private void btnAddSubscription_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var viewModel = (SubscriptionViewModel)DataContext;
                viewModel.AddSubscription(amount, txtSource.Text, date);
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
        private void btnUpdateSubscription_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnRemoveSubscription_Click(object sender, RoutedEventArgs e)
        {
            if (lvSubscriptions.SelectedItem != null)
            {
                var viewModel = (SubscriptionViewModel)DataContext;
                Subscription selectedSubscription = (Subscription)lvSubscriptions.SelectedItem;
                viewModel.RemoveSubscription(selectedSubscription);
                viewModel.RemoveFromTotalCost(selectedSubscription);
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
    }
}
