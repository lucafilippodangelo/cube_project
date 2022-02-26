using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cube_Auction.Application
{
    public class AuctionHistoryCommand : IRequest<AuctionResponse>
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public AuctionStatus AuctionStatus { get; set; }
        public DateTime DateTimeEvent { get; set; }
    }

    public enum AuctionStatus
    {
        Created = 1,
        InProgress = 2,
        Finalised = 3
    }
}
