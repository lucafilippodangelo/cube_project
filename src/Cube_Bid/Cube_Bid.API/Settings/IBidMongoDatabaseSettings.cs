namespace Cube_Bid.API.Settings
{
    public interface IBidMongoDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
