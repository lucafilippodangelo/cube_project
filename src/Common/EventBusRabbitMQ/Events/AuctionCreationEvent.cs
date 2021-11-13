using System;

namespace EventBusRabbitMQ.Events
{
    public class AuctionCreationEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDateTime { get; set; }

        public Guid RequestId { get; set; } //LD for rabbit and logging purposes
    }
}
