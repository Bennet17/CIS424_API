﻿namespace CIS424_API.Models
{
    public class Totals
    {
        public int ID { get; set; }
        public int storeID { get; set; }
        public int cashCountID { get; set; }
        public decimal total { get; set; }
        public int hundred { get; set; }
        public int fifty { get; set; }
        public int twenty { get; set; }
        public int ten { get; set; }
        public int five { get; set;}
        public int two { get; set; }
        public int one { get; set; }
        public int dollarCoin { get; set; }
        public int halfDollar { get; set; }
        public int quarter { get; set; }
        public int dime { get; set; }
        public int nickel { get; set; }
        public int penny { get; set; }
        public int quarterRoll { get; set; }
        public int dimeRoll { get; set; }
        public int nickelRoll { get; set; }
        public int pennyRoll { get; set; }
    }
}