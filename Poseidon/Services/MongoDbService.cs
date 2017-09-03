using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Services
{
    public class MongoDbService
    {
        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; set; }

        public MongoDbService()
        {
            this.Client = new MongoClient("mongodb://127.0.0.1:27017");
            this.Database = this.Client.GetDatabase("poseidon") as MongoDatabaseBase;
        }
    }
}
