using FinanceTracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

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
                netIncome = totalAmount - billViewModel.totalCost;
            }
        }
    }
}
