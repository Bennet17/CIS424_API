namespace CIS424_API.Models
{
    public class CreateCashCount
    {
		public int usrID { get; set; }
		public int storeID { get; set; }
		public string itemCounted { get; set; }
		public decimal amountExpected { get; set; }
		public decimal total { get; set; }
		public string type { get; set; }
		public int hundred	{ get; set; }
		public int fifty { get; set; }
		public int twenty { get; set; }
		public int ten { get; set; }
		public int five { get; set; }
		public int two { get; set; }
		public int one { get; set; }
		public int dollarCoin { get; set; }
		public int halfDollar { get; set; }
		public int quarter	{ get; set; }
		public int dime { get; set; }
		public int nickel { get; set; }
		public int penny { get; set; }
		public int quarterRoll { get; set; }
		public int dimeRoll { get; set; }
		public int nickelRoll { get; set; }
		public int pennyRoll { get; set; }
		
		public decimal creditExpected { get; set; }
		public decimal creditActual { get; set; }
		public decimal cashToSafeTotal { get; set;}
		public int hundredToSafe { get; set; }
		public int fiftyToSafe { get; set; }
		public int twentyToSafe { get; set; }

		// could add in another attribute for if it was an open, mid, close cashCount so never a null if not open or close would be mid, during shift
		// could add in a boolean that is for open day and close day, default is false, and when user selects open day on the GUI param sent as open and boolean would be set to true for that cash count and that could be used to stay open for the day until close day is used
		// close day boolean would be true until the open day is clicked, and will change the GUI for the user
		// could use the Store opened attribute as the boolean for this becasue store isnt open until open day is complete, so would be like an opposite check, if opened = false then it would be an open cash count, for close day it would be like if opened = true so when closed day is clicked on the GUI it could send a closed boolean and when this is sent we know it is a closed, 
		// other cash counts for the day would not send the boolean for store table opened unless the user selects the option,
		// normal cash counts would not sent that boolean so overloading is used to have multiple constructors for cash count or a check to see if that paraamter is sent nad then do the appropriate join with the extra boolean attribute for the opened
    }
}