using FinanceTracker.ViewModels;
using System.Windows;

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new IncomeViewModel();
            var viewModel = (IncomeViewModel)DataContext;
            viewModel.LoadIncome();
            lblTotalIncome.Content = "Total Income: " + viewModel.totalAmount.ToString("C");
        }

        private void btnIncome_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow();
            incomeWindow.Show();
            this.Close();
        }

    }
}