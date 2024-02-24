using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class Register
    {
        public int ID { get; set; }
        public string name { get; set; }
        public int storeID { get; set; }

        public bool enabled { get; set; }

        public bool opened { get; set; }

    }
}