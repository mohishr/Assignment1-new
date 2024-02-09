namespace Assignment1.API.Settings
{
    public class MongoDBSettings
    {
        public string connectionURI { get; set; } = null!;
        public string databaseName { get; set; } = null!;
        public string collectionName { get; set; } = null!;
    }
}
