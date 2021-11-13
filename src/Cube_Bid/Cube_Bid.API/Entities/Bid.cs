using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Entities
{
    public class Bid
    {
        public string AuctionName { get; set; }
        public double Amount { get; set; }  
        public DateTime DateTime { get; set; } 
    }
}
