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
    public class AuctionStatusHistoryRepository : Repository<AuctionStatusHistory>, IAuctionStatusHistoryRepository
    {
        public AuctionStatusHistoryRepository(AuctionContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<AuctionStatusHistory>> GetAuctionHistory(Auction anAuction)
        {
            var auctionHistoryList = await _dbContext.AuctionStatusHistory
                      .Where(o => o.Auction.Name == anAuction.Name)
                      .ToListAsync();

            return auctionHistoryList;
        }
    }
}
