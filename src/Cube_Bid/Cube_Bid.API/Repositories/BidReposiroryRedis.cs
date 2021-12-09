using Cube_Bid.API.Data.Interfaces;
using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Repositories
{


    public class BidReposiroryRedis : IBidRepositoryRedis
    {
        private readonly IBidContextRedis _context;

        public BidReposiroryRedis(IBidContextRedis context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public List<string> GetBidsByPattern(string searchPattern)
        {
            var time001 = DateTime.UtcNow;
            List<string> aListPOfStrings = new List<string>();

            //this takes 2 seconds to query and read 1000 records ----------------------
            var allValues2 = _context.Server.Keys(pattern: searchPattern);

            var time002 = DateTime.UtcNow;
            foreach (var key in allValues2)
            {
                string currentString = (key.ToString() + " - " + _context.Redis.StringGet(key).ToString());
                aListPOfStrings.Add(currentString);
            }
            //this takes 2 seconds END--------------------------------------------------

            var time004 = DateTime.UtcNow;

            //ordering by string itself
            var ordered = aListPOfStrings.OrderBy(a => a).ToList();
            var count = ordered.Count();
            var time003 = DateTime.UtcNow;

            return ordered;
        }

        public string? InsertBid(string key, string value)
        {
            _context.Redis.StringSet(key, value);

            var val = _context.Redis.StringGet(key);
            var toBeReturned = ("StringGet({0}) value is {1}", key, val);
            return toBeReturned.ToString();


            /*
            public async Task<IEnumerable<Bid>> GetBidsByAuction(string userName)
            {
                var basket = await _context
                                    .Redis
                                    .StringGetAsync(userName);
                if (basket.IsNullOrEmpty)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<BasketCart>(basket);
            }
            */

            /*
            public async Task<BasketCart> UpdateBasket(BasketCart basket)
            {
                var updated = await _context
                                  .Redis
                                  .StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
                if (!updated)
                {
                    return null;
                }
                return await GetBasket(basket.UserName);
            }
            */

        }

        public void AuctionFlushTest()
        {
            //get the specific key
            var allValues2 = _context.Server.Keys(pattern: "a*");

            List<string> aListPOfStrings = new List<string>();
            foreach (var key in allValues2)
            {
                _context.Redis.KeyDelete(key);
            }
        }


    }
}