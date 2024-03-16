using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class User
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string position { get; set; }
        public string storeCSV { get; set; }
        public bool enabled { get; set; }
    }
}