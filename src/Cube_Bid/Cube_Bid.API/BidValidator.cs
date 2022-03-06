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
            //pick the first one with value 3
            var auctionEventsList = _auctionsHistoryRepositoryRedis.GetAuctionsHistoriesBYAuctionIdAndEventId(aBid.AuctionId, 3).First();

            //LD 001 -> need to split the string in a struct. String looks like -> "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174"
            string phrase = "The quick brown fox jumps over the lazy dog.";
            string[] words = auctionEventsList.Split(' ');

            var date = words[2];
            var time = words[3];
            var timeMilliseconds = words[4];


            //if both date an milliseconds
            //var something = aBid.DateTime.CompareTo((DateTime)date);

            //THE ABOVE METHOD will need to return a structure
            //need to do a parse of the info and then check the date

            return 2;
        }
    }
}
