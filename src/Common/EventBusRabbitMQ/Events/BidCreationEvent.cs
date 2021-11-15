using System;

namespace EventBusRabbitMQ.Events
{
    public class BidCreationEvent
    {
        public string Id { get; set; }
        public string AuctionName { get; set; }
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string AuctionSubscriberName { get; set; } //this will be an ID
    }
}
