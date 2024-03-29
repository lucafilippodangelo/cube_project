﻿using Cube_Bid.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube_Bid.API.Repositories.Interfaces
{
    public interface IBidRepositoryMongo
    {
        Task Create(Bid aBid);
        Task<bool> Delete(Guid id);
        Task<bool> DeleteAll();
        Task<IEnumerable<Bid>> GetAllBids();
        Task<IEnumerable<Bid>> GetBidsByAuctionId(Guid aGuid);
        Task<bool> Update(Bid aBid);
    }
}