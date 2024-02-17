using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class CashCount
    {
        public int ID { get; set; }
        public int employeeID { get; set; }
        public int registerID { get; set; }
        public int amountExpected { get; set; }
        public DateTime date { get; set; }
    }
}