using FinanceTracker.Models;
using FinanceTracker.ViewModels;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                btnAddSubscription.Visibility = Visibility.Visible;
                btnUpdateSubscription.Visibility = Visibility.Collapsed;
                Subscription selectedSubscription = (Subscription)lvSubscriptions.SelectedItem;
                string source = txtSource.Text;
                DateOnly date = selectedSubscription.Date;
                var viewModel = (SubscriptionViewModel)DataContext;
                viewModel.UpdateSubscription(selectedSubscription, amount, source, date);
                viewModel.RemoveFromTotalCost(selectedSubscription);
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
        private void btnRemoveSubscription_Click(object sender, RoutedEventArgs e)
        {
            if (lvSubscriptions.SelectedItem != null)
            {
                var viewModel = (SubscriptionViewModel)DataContext;
                Subscription selectedSubscription = (Subscription)lvSubscriptions.SelectedItem;
                viewModel.RemoveSubscription(selectedSubscription);
                viewModel.RemoveFromTotalCost(selectedSubscription);
                txtAmount.Text = string.Empty;
                txtSource.Text = string.Empty;
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
        private void lvSubscriptions_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvSubscriptions.SelectedItems.Count == 1)
            {
                var subscription = (Subscription)lvSubscriptions.SelectedItem;
                txtAmount.Text = subscription.Amount.ToString();
                txtSource.Text = subscription.Source.ToString();
                btnAddSubscription.Visibility = Visibility.Collapsed;
                btnUpdateSubscription.Visibility = Visibility.Visible;
            }
            else
            {
                return;
            }
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(lvSubscriptions.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
    }
}
