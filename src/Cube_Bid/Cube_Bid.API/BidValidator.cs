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


        //LD TO BE PERFORMANT WILL NEED TO query redis about the specific validity of auction. At the moment I do run a rest request
        public int ValidateInputBid(Bid aBid)
        {
            //Option one: cache a rest call will query history in auction service
            //Option two: query a redis any time is needed. the redis will need to load history of auctions and listen for any update

            var auctionEventsList = _auctionsHistoryRepositoryRedis.GetAuctionsHistoriesBYAuctionIdAndEventId(aBid.AuctionId, new int()); 
            //THE ABOVE METHOD will need to return a structure
            //need to do a parse of the info and then check the date

            return 2;
        }
    }
}
