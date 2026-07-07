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
            var incomeViewModel = (IncomeViewModel)DataContext;
            incomeViewModel.LoadIncome();
            lblTotalIncome.Content = "Total Income: " + incomeViewModel.totalAmount.ToString("C");
        }
        private void btnIncome_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow();
            incomeWindow.Show();
            this.Close();
        }
        private void btnBills_Click(object sender, RoutedEventArgs e)
        {
            BillWindow billWindow = new BillWindow();
            billWindow.Show();
            this.Close();
        }
    }
}