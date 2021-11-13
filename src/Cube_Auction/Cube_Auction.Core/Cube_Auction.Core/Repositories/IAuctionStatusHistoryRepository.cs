using Cube_Auction.Core.Entities;
using Cube_Auction.Core.Repositories.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube_Auction.Core.Repositories
{
    public interface IAuctionStatusHistoryRepository : IRepository<AuctionStatusHistory>
    {
        Task<IEnumerable<AuctionStatusHistory>> GetAuctionHistory(Auction anAuction);
    }
}

