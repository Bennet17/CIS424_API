namespace CIS424_API.Models
{
    public class Deposit
    {
        public int fID { get; set; }
        public string status { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public decimal total { get; set; }
    }
}