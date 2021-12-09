using Cube_Bid.API.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Data
{
    public class BidContextRedis : IBidContextRedis
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IConfiguration _configuration;

        public BidContextRedis(ConnectionMultiplexer redisConnection, IConfiguration configuration)
        {
            _redisConnection = redisConnection;
            _configuration = configuration;

            Redis = redisConnection.GetDatabase();
            var dd = _configuration.GetConnectionString("Redis");//var ff = Redis.Multiplexer.Configuration;

            Server = redisConnection.GetServer(dd);
        }

        public IDatabase Redis { get; }
        public IServer Server { get; }

    }
}

