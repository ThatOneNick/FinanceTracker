using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceTracker.Models
{
    public class IncomeData
    {
        public double Amount { get; set; }
        
        public string Source { get; set; } = string.Empty;
    }
}
