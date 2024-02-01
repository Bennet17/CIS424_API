using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class cashCount
    {
        public int cashCountID { get; set; }
        public int employeeID { get; set; }
        public int amountExpected { get; set; }
        public DateTime date { get; set; }
    }
}