using Cube_Auction.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Core.Entities
{
    public class Auction : Entity
    {
        public string Name { get; set; }

        public DateTime ExpirationDateTime { get; set; }  //DeliveryStandByDuration is this + 3h

    }
}
