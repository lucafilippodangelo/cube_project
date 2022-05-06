namespace EventBusRabbitMQ.Common
{
    public static class EventBusConstants
    {
        public const string QUEUE_AuctionEvent = "QUEUE_AuctionEvent";
        //public const string BidCreationQueue_Redis = "bidCreationQueue_Redis"; //LD not used anymore
        public const string QUEUE_BidCreation = "QUEUE_BidCreation";
        public const string QUEUE_BidFinalization = "QUEUE_BidFinalization";
    }
}
