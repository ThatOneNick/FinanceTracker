using FinanceTracker.Models;
using System.Windows;

namespace FinanceTracker
{
    /// <summary>
    /// Interaction logic for AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        public AddIncomeWindow()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            IncomeData incomeData = new IncomeData
            {
                Amount = double.Parse(txtAmount.Text),
                Source = txtSource.Text
            };
        }
    }
}
