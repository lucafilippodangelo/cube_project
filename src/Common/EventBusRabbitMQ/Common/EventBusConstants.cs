namespace EventBusRabbitMQ.Common
{
    public static class EventBusConstants
    {
        public const string AuctionEventQueue = "auctionEventQueue";
        //public const string BidCreationQueue_Redis = "bidCreationQueue_Redis"; //LD not used anymore
        public const string BidCreationQueue_Mongo = "bidCreationQueue_Mongo";

        public const string BasketCheckoutQueue = "basketCheckoutQueue";
        public const string SecondConsumerQueue = "secondConsumerQueue";
    }
}
