using Cube_Bid.API.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cube_Bid.API.Repositories
{
    public class AuctionHistoryRepositoryRedis : IAuctionsHistoryRepositoryRedis
    {
        private readonly IAuctionsHistoryContextRedis _context;

        public AuctionHistoryRepositoryRedis(IAuctionsHistoryContextRedis context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //this method will need to return a structure of key/values
        public List<string> GetAuctionsHistoriesBYAuctionIdAndEventId(Guid auctionId, int eventId)
        {
            List<string> aListPOfStrings = new List<string>();
            var allValues2 = _context.Server.Keys(pattern: auctionId.ToString() + " " + eventId.ToString());

            //LD returns in the format -> "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174"
            foreach (var key in allValues2)
            {
                string currentString = (key.ToString() + " " + _context.Redis.StringGet(key));
                aListPOfStrings.Add(currentString);
            }

            //ordering by string itself
            var ordered = aListPOfStrings.OrderBy(a => a).ToList();
            var count = ordered.Count();

            return ordered;
        }

        public string? InsertAuctionEvent(string key, string value)
        {
            _context.Redis.StringSet(key, value);

            var val = _context.Redis.StringGet(key);
            var toBeReturned = ("StringGet({0}) value is {1}", key, val);
            return toBeReturned.ToString();


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