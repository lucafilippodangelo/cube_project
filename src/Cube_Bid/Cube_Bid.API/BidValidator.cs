using Cube_Bid.API.Entities;

namespace Cube_Bid.API
{
    public interface IBidValidator
    {
        int ValidateInputBid(Bid aBid);
    }

    public class BidValidator : IBidValidator
    {
        //LD constructor
        public BidValidator() { }


        //LD TO BE PERFORMANT WILL NEED TO query redis about the specific validity of auction. At the moment I do run a rest request
        public int ValidateInputBid(Bid aBid)
        {
            //Option one: cache a rest call will query history in auction service
            //Option two: query a redis any time is needed. the redis will need to load history of auctions and listen for any update
            return 2;
        }
    }
}
