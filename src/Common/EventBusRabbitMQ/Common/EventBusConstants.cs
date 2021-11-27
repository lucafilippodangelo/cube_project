namespace EventBusRabbitMQ.Common
{
    public static class EventBusConstants
    {
        public const string AuctionCreationQueue = "auctionCreationQueue";
        public const string BidCreationQueue_Redis = "bidCreationQueue_Redis";
        public const string BidCreationQueue_Mongo = "bidCreationQueue_Mongo";

        public const string BasketCheckoutQueue = "basketCheckoutQueue";
        public const string SecondConsumerQueue = "secondConsumerQueue";
    }
}
