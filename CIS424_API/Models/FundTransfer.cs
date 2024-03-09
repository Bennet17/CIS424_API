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
        public int userID { get; set; } // db table says userID, createFundTransfer uses userID so changed it here
        public DateTime date { get; set; }
    }
}