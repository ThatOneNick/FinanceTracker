namespace FinanceTracker.Models
{
    public class Income
    {
        public double Amount { get; set; }
        public string Source { get; set; }
        public DateOnly Date {  get; set; }
    }
}
