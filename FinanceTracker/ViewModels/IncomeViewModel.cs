using FinanceTracker.Models;
using System.Collections.ObjectModel;

namespace FinanceTracker.ViewModels
{
    public class IncomeViewModel
    {
        public ObservableCollection<Income> IncomeItems { get; } = new();
        public ObservableCollection<double> IncomeAmounts { get; } = new();
        public double totalAmount;
        public void AddIncome(double amount, string source)
        {
            Income income = new Income
            {
                Amount = amount,
                Source = source
            };

            IncomeItems.Add(income);
        }
        public void RemoveIncome(Income selectedIncome)
        {
            IncomeItems.Remove(selectedIncome);
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
    }
}
