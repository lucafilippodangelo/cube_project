using System.Collections.Generic;

namespace Cube_Bid.API.Repositories
{
    public interface IAuctionsHistoryRepositoryRedis
    {
        void AuctionFlushTest();
        List<string> GetAuctionsHistoriesBYAuctionId(string AuctionId);
        string InsertAuctionEvent(string key, string value);
    }
}