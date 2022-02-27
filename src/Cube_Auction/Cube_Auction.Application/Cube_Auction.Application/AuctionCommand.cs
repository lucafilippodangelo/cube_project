using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Cube_Auction.Application
{
    public class AuctionCommand : IRequest<AuctionResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }    
        public string Name { get; set; }

    }
}
