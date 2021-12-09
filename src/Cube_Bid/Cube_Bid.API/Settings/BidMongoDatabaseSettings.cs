namespace Cube_Bid.API.Settings
{
    public class BidMongoDatabaseSettings : IBidMongoDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
