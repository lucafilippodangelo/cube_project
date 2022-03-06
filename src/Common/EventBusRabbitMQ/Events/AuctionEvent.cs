using System;

namespace EventBusRabbitMQ.Events
{
    public class AuctionEvent
    {
        public Guid Id { get; set; }
        public int EventCode { get; set; }
        public DateTime EventDateTime { get; set; }

        public int EventDateTimeMilliseconds { get; set; }

    }
}
