using System;

namespace EventBusRabbitMQ.Events
{
    public class BidCreationEvent
    {
        public int IncrementalId { get; set; } //will be replaced with a GUID
        public Guid AuctionId { get; set; }
        public double Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string AuctionSubscriberName { get; set; } //this will be an ID
    }
}
