using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Application
{
    public class AuctionCommand : IRequest<AuctionResponse>
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }

    }
}
