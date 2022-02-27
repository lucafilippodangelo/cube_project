using Cube_Auction.Core.Entities;
using Cube_Auction.Core.Repositories;
using Cube_Auction.Infrastructure.Data;
using Cube_Auction.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube_Auction.Infrastructure.Repository
{
    public class AuctionHistoryRepository : Repository<AuctionHistory>, IAuctionHistoryRepository
    {
        public AuctionHistoryRepository(AuctionContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<AuctionHistory>> GetAuctionHistory(Auction anAuction)
        {
            var auctionHistoryList = await _dbContext.AuctionHistory
                      .Where(o => o.AuctionId == anAuction.Id)
                      .ToListAsync();

            return auctionHistoryList;
        }
    }
}
