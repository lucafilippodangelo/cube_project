using System;

namespace EventBusRabbitMQ.Events
{
    public class BidCreationEvent
    {
        public int IncrementalId { get; set; } //will be replaced with a GUID
        public Guid AuctionId { get; set; }
        public double Amount { get; set; }

        //DATE AND TIME ARE ASSIGNED WHEN THE EVENTS ARE  OBSERVED
        //public DateTime DateTime { get; set; }
        //public int DateTimeMilliseconds { get; set; }
        public string AuctionSubscriberName { get; set; } //this will be an ID
    }
}
