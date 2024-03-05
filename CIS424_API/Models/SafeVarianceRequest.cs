using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class SafeVarianceRequest
    {
        public int storeID { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
}