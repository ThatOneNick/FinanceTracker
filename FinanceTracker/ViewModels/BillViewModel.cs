using FinanceTracker.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace FinanceTracker.ViewModels
{
    public class BillViewModel
    {
        public ObservableCollection<Bill> BillItems { get; } = new();
        public ObservableCollection<double> BillAmounts { get; } = new();
        public double totalCost;
        
        public void AddBill(double amount, string source, DateOnly date)
        {
            if (amount > 0)
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
            } else
            {
                MessageBox.Show("Amount must be a positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void UpdateBill(Bill selectedBill, double amount, string source, DateOnly date)
        {
            if (amount > 0)
            {
                Bill bill = new Bill
                {
                    Amount = amount,
                    Source = source,
                    Date = date
                };
                BillItems.Add(bill);
                int originalIndex = BillItems.IndexOf(selectedBill);
                int index = BillItems.IndexOf(bill);
                BillItems.Move(index, originalIndex);
                BillItems.Remove(selectedBill);

                string file = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                              "billdata.json");
                string json = JsonSerializer.Serialize(BillItems);
                File.WriteAllText(file, json);
            } else
            {
                MessageBox.Show("Amount must be a positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            string file = Path.Combine( Environment.GetFolderPath(
                          Environment.SpecialFolder.MyDocuments),
                          "billdata.json");
            try
            {
                if (!File.Exists(file))
                {
                    File.WriteAllText(file, string.Empty);
                }
                string json = File.ReadAllText(file);
                if (json.Length != 0)
                {
                    ObservableCollection<Bill>? bill =
                        JsonSerializer.Deserialize<ObservableCollection<Bill>?>(json);

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
            catch (Exception)
            {
                MessageBox.Show("Bill data failed to load.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}