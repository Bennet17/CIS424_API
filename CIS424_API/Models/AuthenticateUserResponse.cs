﻿namespace CIS424_API.Models
{
    public class AuthenticateUserResponse
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public int storeID { get; set; }

        public bool IsValid { get; set; }
    }
}