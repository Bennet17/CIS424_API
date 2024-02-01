using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class Register
    {
        public int registerID { get; set; }
        public int storeID { get; set; }
        public int amount { get; set; }
    }
}