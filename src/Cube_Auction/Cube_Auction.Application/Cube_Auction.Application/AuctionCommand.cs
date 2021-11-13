using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Application
{
    public class AuctionCommand : IRequest<AuctionResponse>
    {
        public string Name { get; set; }
        //public DateTime ExpirationDateTime { get; set; }  
    }
}
