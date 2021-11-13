using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Application.Queries
{
    public class GetAuctionByNameQuery : IRequest<IEnumerable<AuctionResponse>>
    {
        public string Name { get; set; }

        public GetAuctionByNameQuery(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}