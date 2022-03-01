using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Data.Interfaces
{
    public interface IAuctionsHistoryContextRedis
    {
        IDatabase Redis { get; }
        public IServer Server { get; }
    }
}
