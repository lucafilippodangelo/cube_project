using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Repositories.Interfaces
{
    public interface IBidReposirory
    {
        List<string> BidGetTest();
        List<string> BidInsertTest();
        void BidFlushTest();
    }
}
