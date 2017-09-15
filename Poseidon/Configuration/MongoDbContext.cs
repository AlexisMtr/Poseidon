using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Poseidon.Configuration
{
    public class MongoDbContext
    {
        public readonly MongoDbSettings Settings;
        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            Settings = settings.Value;

            Client = new MongoClient(Settings.DefaultConnectionString);
            Database = Client.GetDatabase(Settings.DefaultCollectionName);
        }
    }
}
