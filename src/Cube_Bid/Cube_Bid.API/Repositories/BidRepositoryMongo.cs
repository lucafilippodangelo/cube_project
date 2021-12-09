using Cube_Bid.API.Data.Interfaces;
using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
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
            return await _context
                            .Bids
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task Create(Bid aBid)
        {
            try
            {
                aBid .Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
                await _context.Bids.InsertOneAsync(aBid);

            }
            catch (Exception message)
            {
                var info = message;
            }
            

        }

        public async Task<bool> Delete(string id)
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
