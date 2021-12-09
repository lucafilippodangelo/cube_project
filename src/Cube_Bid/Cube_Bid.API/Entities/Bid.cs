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
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }        
        public string AuctionName { get; set; }
        public double Amount { get; set; }  
        public DateTime DateTime { get; set; } 
    }
}
