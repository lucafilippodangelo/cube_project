using Cube_Bid.API.Data.Interfaces;
using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Repositories
{


    public class BidRepositoryMongo : IBidRepositoryMongo
    {
        private readonly IBidContextMongo _context;

        public BidRepositoryMongo(IBidContextMongo bidContextMongo)
        {
            _context = bidContextMongo ?? throw new ArgumentNullException(nameof(bidContextMongo));
        }

        public async Task<IEnumerable<Bid>> GetAllBids()
        {
            var toBeReturned = await _context
                            .Bids
                            .Find(p => true)
                            .ToListAsync();

            return toBeReturned.OrderBy(d=>d.DateTime);
        }

        public async Task<IEnumerable<Bid>> GetBidsByAuctionId(Guid aGuid)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(p => p.AuctionId, aGuid);



            return await _context
                          .Bids
                          .Find(filter)
                          .ToListAsync();
        }

        public async Task Create(Bid aBid)
        {
            try
            {
                aBid.Id = Guid.NewGuid(); //MongoDB.Bson.ObjectId.GenerateNewId();
                await _context.Bids.InsertOneAsync(aBid);

            }
            catch (Exception message)
            {
                var info = message;
            }
        }

        public async Task<bool> Update(Bid aBid)
        {
            var updateResult = await _context
                                        .Bids
                                        .ReplaceOneAsync(filter: g => g.Id == aBid.Id, replacement: aBid);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(Guid id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult = await _context
                                                .Bids
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }


        public async Task<bool> DeleteAll()
        {
            
            DeleteResult deleteResult = await _context.Bids.DeleteManyAsync(new BsonDocument());


            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }



        /*

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                            .Products
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name, name);

            return await _context
                          .Products
                          .Find(filter)
                          .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _context
                          .Products
                          .Find(filter)
                          .ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        */


    }
}
