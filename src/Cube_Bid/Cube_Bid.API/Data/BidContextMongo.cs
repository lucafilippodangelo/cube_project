using Cube_Bid.API.Data.Interfaces;
using Cube_Bid.API.Entities;
using Cube_Bid.API.Settings;
using MongoDB.Driver;

namespace Cube_Bid.API.Data
{
    public class BidContextMongo : IBidContextMongo
    {
        public IMongoCollection<Bid> Bids { get; }
        public BidContextMongo(IBidMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Bids = database.GetCollection<Bid>(settings.CollectionName);
            //CatalogContextSeed.SeedData(Products);
        }

    }
}
