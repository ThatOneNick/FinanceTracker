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
    /// Interaction logic for OtherExpenseWindow.xaml
    /// </summary>
    public partial class OtherExpenseWindow : Window
    {
        public OtherExpenseWindow()
        {
            InitializeComponent();
            DataContext = new ExpenseViewModel();
            var viewModel = (ExpenseViewModel)DataContext;
            viewModel.LoadExpense();
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            lblTotalCost.Content = "Total Cost: " + viewModel.totalCost.ToString("C", culture);
        }
        private void btnAddExpense_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                var date = DateOnly.FromDateTime(DateTime.Now);
                var viewModel = (ExpenseViewModel)DataContext;
                viewModel.AddExpense(amount, txtSource.Text, date);
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
        private void btnUpdateExpense_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount))
            {
                btnAddExpense.Visibility = Visibility.Visible;
                btnUpdateExpense.Visibility = Visibility.Collapsed;
                Expense selectedExpense = (Expense)lvExpenses.SelectedItem;
                string source = txtSource.Text;
                DateOnly date = selectedExpense.Date;
                var viewModel = (ExpenseViewModel)DataContext;
                viewModel.UpdateExpense(selectedExpense, amount, source, date);
                viewModel.RemoveFromTotalCost(selectedExpense);
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
        private void btnRemoveExpense_Click(object sender, RoutedEventArgs e)
        {
            if (lvExpenses.SelectedItem != null)
            {
                var viewModel = (ExpenseViewModel)DataContext;
                Expense selectedExpense = (Expense)lvExpenses.SelectedItem;
                viewModel.RemoveExpense(selectedExpense);
                viewModel.RemoveFromTotalCost(selectedExpense);
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
        private void lvExpenses_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvExpenses.SelectedItems.Count == 1)
            {
                var expense = (Expense)lvExpenses.SelectedItem;
                txtAmount.Text = expense.Amount.ToString();
                txtSource.Text = expense.Source.ToString();
                btnAddExpense.Visibility = Visibility.Collapsed;
                btnUpdateExpense.Visibility = Visibility.Visible;
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
              CollectionViewSource.GetDefaultView(lvExpenses.ItemsSource);

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