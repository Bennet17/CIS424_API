﻿using System;

namespace CIS424_API.Models
{
    public class VarianceResponse
    {
        public float amountExpected { get; set; }

        public float total {  get; set; }

        public float Variance {  get; set; }

        public DateTime Date {  get; set; }
   }
}