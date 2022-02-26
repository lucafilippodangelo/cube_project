using Cube_Auction.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cube_Auction.Infrastructure.Data
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
        {
        }

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<AuctionHistory> AuctionHistory { get; set; }
    }

   


}
