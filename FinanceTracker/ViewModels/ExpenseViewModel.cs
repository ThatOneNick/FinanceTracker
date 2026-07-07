using FinanceTracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace FinanceTracker.ViewModels
{
    public class ExpenseViewModel
    {
        public ObservableCollection<Expense> ExpenseItems { get; } = new();
        public ObservableCollection<double> ExpenseAmounts { get; } = new();
        public double totalCost;
        public void AddExpense(double amount, string source, DateOnly date)
        {
            Expense expense = new Expense
            {
                Amount = amount,
                Source = source,
                Date = date
            };

            ExpenseItems.Add(expense);

            string file = Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                          "expensedata.json");
            string json = JsonSerializer.Serialize(ExpenseItems);
            File.WriteAllText(file, json);

        }
        public void RemoveExpense(Expense selectedExpense)
        {
            ExpenseItems.Remove(selectedExpense);
            string file = Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                          "expensedata.json");
            string json = JsonSerializer.Serialize(ExpenseItems);
            File.WriteAllText(file, json);
        }

        public void AddToTotalCost(double amount)
        {
            ExpenseAmounts.Add(amount);
            totalCost = ExpenseAmounts.Sum();
            ExpensesTotalCost expensesTotalCost = new ExpensesTotalCost
            {
                TotalCost = totalCost
            };
        }

        public void RemoveFromTotalCost(Expense selectedExpense)
        {
            ExpenseAmounts.Remove(selectedExpense.Amount);
            totalCost = ExpenseAmounts.Sum();
        }

        public void LoadExpense()
        {
            string file = Path.Combine( Environment.GetFolderPath(
                          Environment.SpecialFolder.MyDocuments),
                          "expensedata.json");
            if (!File.Exists(file))
            {
                File.WriteAllText(file, string.Empty);
            }
            string json = File.ReadAllText(file);
            if (json.Length != 0)
            {
                ObservableCollection<Expense>? expense =
                    JsonSerializer.Deserialize<ObservableCollection<Expense>?>(json);

                if (expense != null)
                {
                    foreach (var expenseItem in expense)
                    {
                        ExpenseItems.Add(expenseItem);
                        double amount = expenseItem.Amount;
                        AddToTotalCost(amount);
                    }
                }
            }
        }
    }
}
