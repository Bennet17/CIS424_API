using System;

namespace CIS424_API.Models
{
    public class CashCount
    {
        public int ID { get; set; }
        public int usrID { get; set; } // changed to usrID, was employeeID, usrID is used everywhere else
        public int registerID { get; set; }
        public int amountExpected { get; set; }
        public DateTime date { get; set; }
    }
}