using System.Collections.Generic;

namespace Cube_Bid.API.Repositories.Interfaces
{
    public interface IBidRepositoryRedis
    {
        List<string> GetBidsByPattern(string pattern);

        public string? InsertBid(string key, string value);

    }
}
