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
        }

        private void btnIncome_Click(object sender, RoutedEventArgs e)
        {
            IncomeWindow incomeWindow = new IncomeWindow();
            incomeWindow.Show();
            this.Close();
        }

        private void btnBills_Click(object sender, RoutedEventArgs e)
        {
            BillsWindow billsWindow = new BillsWindow();
            billsWindow.Show();
            this.Close();
        }

        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            SubscriptionsWindow subscriptionsWindow = new SubscriptionsWindow();
            subscriptionsWindow.Show();
            this.Close();
        }

        private void btnOtherExpenses_Click(object sender, RoutedEventArgs e)
        {
            OtherExpensesWindow otherExpensesWindow = new OtherExpensesWindow();
            otherExpensesWindow.Show();
            this.Close();
        }
    }
}