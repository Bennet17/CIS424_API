using System;

namespace CIS424_API.Models
{
    public class FundTransfer
    {
        public int fID { get; set; }
        public string name { get; set; } 
        public DateTime date { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string status { get; set; }
        public decimal total { get; set; }
        public int verifiedBy { get; set; }
        public DateTime verifiedOn { get; set; }
    }
}