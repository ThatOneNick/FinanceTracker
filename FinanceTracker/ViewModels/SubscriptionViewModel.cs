using FinanceTracker.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace FinanceTracker.ViewModels
{
    public class SubscriptionViewModel
    {
        public ObservableCollection<Subscription> SubscriptionItems { get; } = new();
        public ObservableCollection<double> SubscriptionAmounts { get; } = new();
        public double totalCost;
        public void AddSubscription(double amount, string source, DateOnly date)
        {
            if (amount > 0)
            {
                Subscription subscription = new Subscription
                {
                    Amount = amount,
                    Source = source,
                    Date = date
                };

                SubscriptionItems.Add(subscription);

                string file = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                              "subscriptiondata.json");
                string json = JsonSerializer.Serialize(SubscriptionItems);
                File.WriteAllText(file, json); 
            } else
            {
                MessageBox.Show("Amount must be a positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        public void UpdateSubscription(Subscription selectedSubscription, double amount, string source, DateOnly date)
        {
            if (amount > 0)
            {
                Subscription subscription = new Subscription
                {
                    Amount = amount,
                    Source = source,
                    Date = date
                };
                SubscriptionItems.Add(subscription);
                int originalIndex = SubscriptionItems.IndexOf(selectedSubscription);
                int index = SubscriptionItems.IndexOf(subscription);
                SubscriptionItems.Move(index, originalIndex);
                SubscriptionItems.Remove(selectedSubscription);

                string file = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                              "subscriptiondata.json");
                string json = JsonSerializer.Serialize(SubscriptionItems);
                File.WriteAllText(file, json);
            }
            else
            {
                MessageBox.Show("Amount must be a positive number.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void RemoveSubscription(Subscription selectedSubscription)
        {
            SubscriptionItems.Remove(selectedSubscription);
            string file = Path.Combine(
                          Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                          "subscriptiondata.json");
            string json = JsonSerializer.Serialize(SubscriptionItems);
            File.WriteAllText(file, json);
        }
        public void AddToTotalCost(double amount)
        {
            SubscriptionAmounts.Add(amount);
            totalCost = SubscriptionAmounts.Sum();
            SubscriptionsTotalCost subscriptionsTotalCost = new SubscriptionsTotalCost
            {
                TotalCost = totalCost
            };
        }
        public void RemoveFromTotalCost(Subscription selectedSubscription)
        {
            SubscriptionAmounts.Remove(selectedSubscription.Amount);
            totalCost = SubscriptionAmounts.Sum();
        }
        public void LoadSubscription()
        {
            string file = Path.Combine( Environment.GetFolderPath(
                          Environment.SpecialFolder.MyDocuments),
                          "subscriptiondata.json");
            try
            {
                if (!File.Exists(file))
                {
                    File.WriteAllText(file, string.Empty);
                }
                string json = File.ReadAllText(file);
                if (json.Length != 0)
                {
                    ObservableCollection<Subscription>? subscription =
                        JsonSerializer.Deserialize<ObservableCollection<Subscription>?>(json);

                    if (subscription != null)
                    {
                        foreach (var subscriptionItem in subscription)
                        {
                            SubscriptionItems.Add(subscriptionItem);
                            double amount = subscriptionItem.Amount;
                            AddToTotalCost(amount);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Subscription data failed to load.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}
