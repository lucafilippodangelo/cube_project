using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Repositories.Interfaces
{
    public interface IAuctionReposirory
    {
        List<string> AuctionInsertTest();
        void AuctionFlushTest();
    }
}
