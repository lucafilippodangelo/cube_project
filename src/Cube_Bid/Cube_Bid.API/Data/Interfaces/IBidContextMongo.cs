using Cube_Bid.API.Entities;
using MongoDB.Driver;

namespace Cube_Bid.API.Data.Interfaces
{
    public interface IBidContextMongo
    {
        IMongoCollection<Bid> Bids { get; }
    }
}
