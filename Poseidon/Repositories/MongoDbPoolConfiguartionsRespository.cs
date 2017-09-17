using MongoDB.Driver;
using Poseidon.Configuration;
using Poseidon.Models;
using System.Linq;

namespace Poseidon.Repositories
{
    public class MongoDbPoolConfiguartionsRespository : IPoolConfigurationsRepository<PoolConfiguration>
    {
        private readonly MongoDbContext Context;
        private readonly IMongoCollection<PoolConfiguration> ConfigurationsCollection;

        public MongoDbPoolConfiguartionsRespository(MongoDbContext context)
        {
            this.Context = context;
            this.ConfigurationsCollection = this.Context.Database.GetCollection<PoolConfiguration>("configurations");
        }

        public void Add(PoolConfiguration model)
        {
            this.ConfigurationsCollection.InsertOne(model);
        }

        public void Delete(PoolConfiguration model)
        {
            this.ConfigurationsCollection.DeleteOne(Builders<PoolConfiguration>.Filter.Eq(p => p.Id, model.Id));
        }

        public IQueryable<PoolConfiguration> Get()
        {
            return this.ConfigurationsCollection.AsQueryable();
        }

        public PoolConfiguration GetById(string id)
        {
            return this.ConfigurationsCollection.AsQueryable()
                .FirstOrDefault(c => c.Id.Equals(id));
        }

        public PoolConfiguration GetByPoolId(string id)
        {
            return this.ConfigurationsCollection.AsQueryable()
                .FirstOrDefault(c => c.PoolId.Equals(id));
        }

        public void Update(string id, PoolConfiguration model)
        {
            var oldConf = this.GetByPoolId(id);
            model.Id = oldConf.Id;
            model.PoolId = oldConf.PoolId;
            model.ObjectId = oldConf.ObjectId;

            this.ConfigurationsCollection.ReplaceOne(Builders<PoolConfiguration>.Filter.Eq(p => p.Id, oldConf.Id), model);
        }
    }
}
