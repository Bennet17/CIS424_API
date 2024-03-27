using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class VarianceResponse
    {

        public DateTime Date {  get; set; }
        public string POSName { get; set; }
        public string OpenerName { get; set; }
        public decimal OpenExpected { get; set; }
        public decimal OpenActual { get; set; }
        public string CloserName { get; set; }
        public decimal CloseExpected { get; set; }
        public decimal CloseActual { get; set; }
        public decimal CashToSafe { get; set; }
        public decimal CloseCreditActual { get; set; }
        public decimal CloseCreditExpected { get; set; }
        public decimal OpenVariance { get; set; }
        public decimal CloseVariance { get; set; }
        public decimal TotalCashVariance { get; set; }
        public decimal CreditVariance { get; set; }
        public decimal TotalVariance { get; set; }
   }
}