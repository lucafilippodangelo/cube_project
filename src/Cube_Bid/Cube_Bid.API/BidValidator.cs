using Cube_Bid.API.Entities;
using Cube_Bid.API.Repositories;
using System;
using System.Linq;

namespace Cube_Bid.API
{
    public interface IBidValidator
    {
        int ValidateInputBid(Bid aBid);
    }

    
    public class BidValidator : IBidValidator
    {

        private readonly IAuctionsHistoryRepositoryRedis _auctionsHistoryRepositoryRedis;

        //LD constructor
        public BidValidator(IAuctionsHistoryRepositoryRedis auctionsHistoryRepositoryRedis) {
            _auctionsHistoryRepositoryRedis = auctionsHistoryRepositoryRedis ?? throw new ArgumentNullException(nameof(_auctionsHistoryRepositoryRedis));
        }


        //NEED UT
        public int ValidateInputBid(Bid aBid)
        {
            var auctionEventsList = _auctionsHistoryRepositoryRedis.GetAuctionsHistoriesBYAuctionIdAndEventId(aBid.AuctionId, 3); 
            //LD 001 -> need to split the string



            //if both date an milliseconds
            //if (aBid.DateTime )
            //THE ABOVE METHOD will need to return a structure
            //need to do a parse of the info and then check the date

            return 2;
        }
    }
}
