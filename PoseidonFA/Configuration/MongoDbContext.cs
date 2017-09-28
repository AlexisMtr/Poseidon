using Autofac.Core;
using MongoDB.Driver;

namespace PoseidonFA.Configuration
{
    public class MongoDbContext : Service
    {
        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;

        public MongoDbContext(string connectionString, string dbName)
        {
            Client = new MongoClient(connectionString);
            Database = Client.GetDatabase(dbName);
        }

        public override string Description => "";
    }
}
