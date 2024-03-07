using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class Store
    {
        public int ID { get; set; }
        public string location { get; set; }

        public bool enabled { get; set; }
    }
}