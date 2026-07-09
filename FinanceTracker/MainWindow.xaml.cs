using FinanceTracker.ViewModels;
using System.Globalization;
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
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            lblTotalIncome.Content = "Net Income: " + incomeViewModel.netIncome.ToString("C", culture);
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
        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionWindow subscriptionWindow = new SubscriptionWindow();
            subscriptionWindow.Show();
            this.Close();
        }
        private void btnOtherExpenses_Click(object sender, RoutedEventArgs e)
        {
            OtherExpenseWindow otherExpenseWindow = new OtherExpenseWindow();
            otherExpenseWindow.Show();
            this.Close();
        }
    }
}