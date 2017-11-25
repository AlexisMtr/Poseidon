using Autofac.Core;
using MongoDB.Driver;
using System.Security.Authentication;

namespace PoseidonFA.Configuration
{
    public class MongoDbContext : Service
    {
        public readonly IMongoClient Client;
        public readonly IMongoDatabase Database;

        public MongoDbContext(string connectionString, string dbName)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            Client = new MongoClient(settings);
            Database = Client.GetDatabase(dbName);
        }

        public override string Description => "";
    }
}
