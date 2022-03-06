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
            var auctionEventsList = _auctionsHistoryRepositoryRedis.GetAuctionsHistoriesBYAuctionIdAndEventId(aBid.AuctionId, 3);

            //LD 001 -> need to split the string in a struct. String looks like -> "03359e80-02e2-4dba-9d7d-d941e9d96056 3 06/03/2022 14:43:34 174"
            string[] words = auctionEventsList.First().Split(' ');

            var date = words[2];
            var time = words[3];
            var timeMilliseconds = words[4];
            var dd = int.Parse(timeMilliseconds);

            var parsedTime = Convert.ToDateTime(date + " " + time);

            if (aBid.DateTime.CompareTo(parsedTime) > 0)
            {
                //so if Bid is later than "parsedTime"
                return 2; //"two" means not valid
            }
            else if (aBid.DateTime.CompareTo(parsedTime) < 0)
            {
                return 1; //"one" means valid
            }
            else if (aBid.DateTimeMilliseconds > int.Parse(timeMilliseconds)) //in case it is equal then compare milliseconds
            {
                return 2; //"two" means not valid
            }
            else if (aBid.DateTimeMilliseconds <= int.Parse(timeMilliseconds)) 
            {
                return 1; //"one" means valid
            }
            else {
                return 1; //return not valid by default
            }



            return 2;
        }
    }
}
