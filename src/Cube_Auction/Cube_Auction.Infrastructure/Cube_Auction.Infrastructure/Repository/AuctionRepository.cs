using Cube_Auction.Application;
using Cube_Auction.Core.Entities;
using Cube_Auction.Core.Repositories;
using Cube_Auction.Infrastructure.Data;
using Cube_Auction.Infrastructure.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Auction.Infrastructure.Repository
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        public AuctionRepository(AuctionContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Auction>> GetAuctions()
        {
            var auctionList = await _dbContext.Auctions
                      .ToListAsync();

            return auctionList;
        }

        public async Task<IEnumerable<Auction>> GetAuctionByName(string name)
        {
            var auctionList = await _dbContext.Auctions
                      .Where(o => o.Name == name)
                      .ToListAsync();

            return auctionList;
        }

        public async Task<Auction> PostAuction(AuctionCommand command)
        {
            //LD simulating a mapper at the moment
            Auction aNewAuction = new Auction();
            aNewAuction.Id = Guid.NewGuid();
            aNewAuction.Name = command.Name;

            _ = _dbContext.Auctions.AddAsync(aNewAuction);
            _ = await _dbContext.SaveChangesAsync();

            return aNewAuction;
        }

        public async Task<AuctionHistory> PostAuctionHistory(AuctionHistoryCommand command)
        {
            //LD simulating a mapper at the moment
            AuctionHistory aNewAuctionHistory = new AuctionHistory();
            aNewAuctionHistory.Id = Guid.NewGuid();
            aNewAuctionHistory.AuctionId = command.AuctionId;
            aNewAuctionHistory.DateTimeEvent = command.DateTimeEvent;//at the moment passing it from the controller, there will be a builder or something
            aNewAuctionHistory.AuctionStatus = (Core.Entities.AuctionStatus)command.AuctionStatus;
            _ = _dbContext.AuctionHistory.AddAsync(aNewAuctionHistory);
            _ = await _dbContext.SaveChangesAsync();

            return aNewAuctionHistory;
        }

        Task<Auction> IAuctionRepository.PostAuctionHistory(AuctionHistoryCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
