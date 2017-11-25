using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Authentication;

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

            MongoClientSettings clientSettings = MongoClientSettings.FromUrl(new MongoUrl(Settings.DefaultConnectionString));
            clientSettings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

            Client = new MongoClient(clientSettings);
            Database = Client.GetDatabase(Settings.DefaultDbName);
        }
    }
}
