using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Entities
{
    public class BidCommand
    {
        public string Id { get; set; } //this will be a guid
        public string AuctionName { get; set; }
        public string AuctionSubscriberName { get; set; }
        public double Amount { get; set; }  
        public DateTime DateTime { get; set; } 
    }
}
