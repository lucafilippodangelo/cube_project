using Cube_Auction.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Core.Entities
{
    public class Auction : Entity
    {
        public Guid Id { get; set; }  
        public string Name { get; set; }

    }
}
