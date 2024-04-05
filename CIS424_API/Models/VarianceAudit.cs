using System;

namespace CIS424_API.Models
{
    public class VarianceAudit
    {
        public int storeID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal cashTendered { get; set; }
        public decimal cashBuys { get; set; }
        public decimal pettyCash { get; set; }
        public decimal mastercard { get; set; }
        public decimal visa { get; set; }
        public decimal americanExpress { get; set; }
        public decimal discover { get; set; }
        public decimal debit { get; set; }
        public decimal other { get; set; }
    }
}