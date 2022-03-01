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


    public class AuctionReposirory : IAuctionReposirory
    {
        private readonly IAuctionContext _context;

        public AuctionReposirory(IAuctionContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<string> AuctionInsertTest()
        {
            List<string> aListPOfStrings = new List<string>();

            //mset and msetnx
            KeyValuePair<RedisKey, RedisValue>[] values = {
               new KeyValuePair<RedisKey, RedisValue>("a:1", "a uno"),
               new KeyValuePair<RedisKey, RedisValue>("a:2", "a due"),
               new KeyValuePair<RedisKey, RedisValue>("b:1", "b due")
            };

            if (_context.Redis.StringSet(values))
            {

                //get the specific key
                RedisKey[] myKeys = { "a:1" };
                var allValues = _context.Redis.StringGet(myKeys);
                var ld = string.Join(",", allValues);

                //get the specific key
                RedisKey[] myKeys2 = { "a*" };
                var allValues2 = _context.Server.Keys(pattern: "a*");
                
                
                foreach (var key in allValues2)
                {
                    string currentString = ( key.ToString() + " - " + _context.Redis.StringGet(key).ToString ());
                    aListPOfStrings.Add(currentString);
                }

                

            }
            return aListPOfStrings;

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
            var allValues2 = _context.Server.Keys(pattern: "*");

            List<string> aListPOfStrings = new List<string>();
            foreach (var key in allValues2)
            {
                _context.Redis.KeyDelete(key);
            }
        }

    }
}