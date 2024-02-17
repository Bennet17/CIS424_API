using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class FundTransfer
    {
        public int ID { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public int employeeID { get; set; }
        public DateTime date { get; set; }
    }
}