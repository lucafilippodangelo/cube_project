using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Application
{
    public class AuctionResponse
    {
        public string Name { get; set; }
        public DateTime ExpirationDateTime { get; set; }  //DeliveryStandByDuration is this + 3h
    }
}
