using FinanceTracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace FinanceTracker.ViewModels
{
    public class IncomeViewModel
    {
        public ObservableCollection<Income> IncomeItems { get; } = new();
        public ObservableCollection<double> IncomeAmounts { get; } = new();
        public double totalAmount;
        public double netIncome;
        public void AddIncome(double amount, string source, DateOnly date)
        {
            if (amount > 0)
            {
                Income income = new Income
                {
                    Amount = amount,
                    Source = source,
                    Date = date
                };

                IncomeItems.Add(income);

                string file = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                              "incomedata.json");
                string json = JsonSerializer.Serialize(IncomeItems);
                File.WriteAllText(file, json); 
            } else
            {
                MessageBox.Show("Amount must be a positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void UpdateIncome(Income selectedIncome, double amount, string source, DateOnly date)
        {
            if (amount > 0)
            {
                Income income = new Income
                {
                    Amount = amount,
                    Source = source,
                    Date = date
                };
                IncomeItems.Add(income);
                int originalIndex = IncomeItems.IndexOf(selectedIncome);
                int index = IncomeItems.IndexOf(income);
                IncomeItems.Move(index, originalIndex);
                IncomeItems.Remove(selectedIncome);

                string file = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                              "incomedata.json");
                string json = JsonSerializer.Serialize(IncomeItems);
                File.WriteAllText(file, json);
            }
            else
            {
                MessageBox.Show("Amount must be a positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void RemoveIncome(Income selectedIncome)
        {
            IncomeItems.Remove(selectedIncome);
            string file = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                              "incomedata.json");
            string json = JsonSerializer.Serialize(IncomeItems);
            File.WriteAllText(file, json);
        }
        public void AddToTotalIncome(double amount)
        {
            IncomeAmounts.Add(amount);
            totalAmount = IncomeAmounts.Sum();
            TotalIncome totalIncome = new TotalIncome
            {
                TotalAmount = totalAmount
            };
        }
        public void RemoveFromTotalIncome(Income selectedIncome)
        {
            IncomeAmounts.Remove(selectedIncome.Amount);
            totalAmount = IncomeAmounts.Sum();
        }
        public void LoadIncome()
        {
            string file = Path.Combine( Environment.GetFolderPath(
                          Environment.SpecialFolder.MyDocuments),
                          "incomedata.json");
            if (!File.Exists(file))
            {
                File.WriteAllText(file, string.Empty);
            }
            string json = File.ReadAllText(file);
            if (json.Length != 0)
            {
                ObservableCollection<Income>? income =
                    JsonSerializer.Deserialize<ObservableCollection<Income>?>(json);

                if (income != null)
                {
                    foreach (var incomeItem in income)
                    {
                        IncomeItems.Add(incomeItem);
                        double amount = incomeItem.Amount;
                        AddToTotalIncome(amount);
                    }
                }
                BillViewModel billViewModel = new BillViewModel();
                billViewModel.LoadBill();
                SubscriptionViewModel subscriptionViewModel = new SubscriptionViewModel();
                subscriptionViewModel.LoadSubscription();
                ExpenseViewModel expenseViewModel = new ExpenseViewModel();
                expenseViewModel.LoadExpense();
                netIncome = totalAmount;
                netIncome -= billViewModel.totalCost;
                netIncome -= subscriptionViewModel.totalCost;
                netIncome -= expenseViewModel.totalCost;
            }
        }
    }
}
