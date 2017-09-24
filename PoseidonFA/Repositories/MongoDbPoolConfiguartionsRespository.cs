using MongoDB.Driver;
using PoseidonFA.Models;
using PoseidonFA.Configuration;
using System.Linq;

namespace PoseidonFA.Repositories
{
    public class MongoDbPoolConfiguartionsRespository : IConfigurationRepository<PoolConfiguration>
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
            UpdateDefinition<PoolConfiguration> update = Builders<PoolConfiguration>.Update.Set(p => p, model);
            this.ConfigurationsCollection.UpdateOne(Builders<PoolConfiguration>.Filter.Eq(p => p.Id, id), update);
        }
    }
}
