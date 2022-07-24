using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube_Bid.API.Entities
{
    public class Bid
    {
        [BsonElement("BidName")]
        public string BidName { get; set; }

        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }        
        public Guid AuctionId { get; set; }
        public double Amount { get; set; }  
        public string confirmed { get; set; } //LD this is "0" at creation time, a routine will confirm if valid "1" or not valid "2"  
        public DateTime DateTime { get; set; }
        public int DateTimeMilliseconds { get; set; }
    }
}
