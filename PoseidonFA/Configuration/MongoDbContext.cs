using MongoDB.Driver;

namespace PoseidonFA.Configuration
{
    public class MongoDbContext
    {
        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;

        public MongoDbContext(string connectionString, string dbName)
        {
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(dbName);
        }
    }
}
