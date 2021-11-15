using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Data.Interfaces
{
    public interface IAuctionContext
    {
        IDatabase Redis { get; }
        public IServer Server { get; }
    }
}
