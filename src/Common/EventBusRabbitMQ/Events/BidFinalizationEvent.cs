using System;

namespace EventBusRabbitMQ.Events
{
    public class BidFinalizationEvent
    {
        public string BasicLog { get; set; }

        public string Status { get; set; }
    }
}
