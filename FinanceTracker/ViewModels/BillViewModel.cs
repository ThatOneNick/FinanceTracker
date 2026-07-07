using FinanceTracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace FinanceTracker.ViewModels
{
    public class BillViewModel
    {
        public ObservableCollection<Bill> BillItems { get; } = new();
        public ObservableCollection<double> BillAmounts { get; } = new();
        public double totalCost;
        public void AddBill(double amount, string source, DateOnly date)
        {
            Bill bill = new Bill
            {
                Amount = amount,
                Source = source,
                Date = date
            };

            BillItems.Add(bill);

            string file = Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                          "billdata.json");
            string json = JsonSerializer.Serialize(BillItems);
            File.WriteAllText(file, json);

        }
        public void RemoveBill(Bill selectedBill)
        {
            BillItems.Remove(selectedBill);
            string file = Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                          "billdata.json");
            string json = JsonSerializer.Serialize(BillItems);
            File.WriteAllText(file, json);
        }

        public void AddToTotalCost(double amount)
        {
            BillAmounts.Add(amount);
            totalCost = BillAmounts.Sum();
            BillsTotalCost billsTotalCost = new BillsTotalCost
            {
                TotalCost = totalCost
            };
        }

        public void RemoveFromTotalCost(Bill selectedBill)
        {
            BillAmounts.Remove(selectedBill.Amount);
            totalCost = BillAmounts.Sum();
        }

        public void LoadBill()
        {
            string file = Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                          "billdata.json");
            if (!File.Exists(file))
            {
                File.Create(file);
            }
            else
            {
                string json = File.ReadAllText(file);
                ObservableCollection<Bill>? bill =
                    JsonSerializer.Deserialize<ObservableCollection<Bill>>(json);
                if (bill != null)
                {
                    foreach (var billItem in bill)
                    {
                        BillItems.Add(billItem);
                        double amount = billItem.Amount;
                        AddToTotalCost(amount);
                    }
                }
            }
        }
    }
}
