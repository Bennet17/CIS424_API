using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIS424_API.Models
{
    public class MaximumDenominations
    {
        public int storeId { get; set; }
        public bool enabled { get; set; }
        public bool opened { get; set; }
        public int Hundred_Register { get; set; }
        public int Fifty_Register { get; set; }
        public int Twenty_Register { get; set; }
        public int Hundred { get; set; }
        public int Fifty { get; set; }
        public int Twenty { get; set; }
        public int Ten { get; set; }
        public int Five { get; set; }
        public int Two { get; set; }
        public int One { get; set; }
        public int QuarterRoll { get; set; }
        public int DimeRoll { get; set; }
        public int NickelRoll { get; set; }
        public int PennyRoll { get; set; }
    }
}