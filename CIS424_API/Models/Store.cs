﻿namespace CIS424_API.Models
{
    public class Store
    {
        public int ID { get; set; }
        public string location { get; set; }
        public bool enabled { get; set; }

        public bool opened { get; set; }
    }
}