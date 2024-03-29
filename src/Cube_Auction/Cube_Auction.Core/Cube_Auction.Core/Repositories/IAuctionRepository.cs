﻿using Cube_Auction.Application;
using Cube_Auction.Core.Entities;
using Cube_Auction.Core.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Cube_Auction.Core.Repositories
{
    public interface IAuctionRepository : IRepository<Auction>
    {
        Task<IEnumerable<Auction>> GetAuctions();
        Task<IEnumerable<AuctionHistory>> GetAuctionsHistory();
        Task<IEnumerable<Auction>> GetAuctionByName(string name);
        Task<Auction> PostAuction(AuctionCommand command);
        Task<AuctionHistory> PostAuctionHistory(AuctionHistoryCommand command);
    }
}
