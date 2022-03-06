using System;
using System.Collections.Generic;

namespace Cube_Bid.API.Repositories
{
    public interface IAuctionsHistoryRepositoryRedis
    {
        void AuctionFlushTest();
        List<string> GetAuctionsHistoriesBYAuctionIdAndEventId(Guid auctionId, int eventId);
        string InsertAuctionEvent(string key, string value);
    }
}