using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Models
{
    public class Subscription
    {
        public double Amount { get; set; }
        public string Source { get; set; }
        public DateOnly Date { get; set; }
    }
}
