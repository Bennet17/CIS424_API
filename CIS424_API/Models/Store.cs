namespace CIS424_API.Models
{
    public class Store
    {
        public int ID { get; set; }
        public string location { get; set; }
        public bool enabled { get; set; }
        public bool opened { get; set; }
        public int hundredRegisterMax { get; set; }
        public int fiftyRegisterMax { get; set; }
        public int twentyRegisterMax { get; set; }
        public int hundredMax { get; set; }
		public int fiftyMax { get; set; }
		public int twentyMax { get; set; }
		public int tenMax { get; set; }
		public int fiveMax { get; set; }
		public int twoMax { get; set; }
		public int oneMax { get; set; }
		public int quarterRollMax { get; set; }
		public int dimeRollMax { get; set; }
		public int nickelRollMax { get; set; }
		public int pennyRollMax { get; set; }
    }
}