using System;

namespace CIS424_API.Models
{
    public class FundTransfer
    {
        public int fID { get; set; }
        public int vID { get; set; } //The vID in the actual table corresponds to the foreign key verifiedBy
        public string name { get; set; } 
        public DateTime date { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string status { get; set; }
        public decimal total { get; set; }
        public string verifiedBy { get; set; } //When this object is returned, it represents the name of the user who verified the deposit.
        public DateTime? verifiedOn { get; set; } //Use DateTime? instead of DateTime because null values can be returned from the database for dates 
    }
}