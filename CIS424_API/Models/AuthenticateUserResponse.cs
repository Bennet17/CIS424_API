﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class AuthenticateUserResponse
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public int storeID { get; set; }

        public string IsValid { get; set; }
    }
}