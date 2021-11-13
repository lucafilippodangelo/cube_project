using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Entities
{
    public class Auction 
    {
        public string Name { get; set; }

        public DateTime ExpirationDateTime { get; set; }  //DeliveryStandByDuration is this + 3h

    }
}
