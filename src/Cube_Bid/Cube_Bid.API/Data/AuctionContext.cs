using Cube_Bid.API.Data.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Data
{
    public class AuctionContext : IAuctionContext
    {
        private readonly ConnectionMultiplexer _redisConnection;

        public AuctionContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = redisConnection.GetDatabase();

            var ff = Redis.Multiplexer.Configuration;

            Server = redisConnection.GetServer(ff);
        }

        public IDatabase Redis { get; }
        public IServer Server { get; }

    }
}

