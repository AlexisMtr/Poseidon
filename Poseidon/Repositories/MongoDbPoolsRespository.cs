using MongoDB.Driver;
using Poseidon.Configuration;
using Poseidon.Models;
using System.Linq;

namespace Poseidon.Repositories
{
    public class MongoDbPoolsRespository : IPoolsRepository<Pool>
    {
        private readonly MongoDbContext Context;
        private readonly IMongoCollection<Pool> PoolsCollection;

        public MongoDbPoolsRespository(MongoDbContext context)
        {
            this.Context = context;
            this.PoolsCollection = this.Context.Database.GetCollection<Pool>("pools");
        }

        public void Add(Pool model)
        {
            this.PoolsCollection.InsertOne(model);
        }

        public void Delete(Pool model)
        {
            this.PoolsCollection.DeleteOne(Builders<Pool>.Filter.Eq(p => p.Id, model.Id));
        }

        public IQueryable<Pool> Get()
        {
            return this.PoolsCollection.AsQueryable();
        }

        public Pool GetById(string id)
        {
            return this.PoolsCollection.AsQueryable()
                .FirstOrDefault(p => p.Id.Equals(id));
        }

        public void Update(string id, Pool model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set(p => p.Location, model.Location)
                .Set(p => p.Name, model.Name)
                .Set(p => p.UsersId, model.UsersId);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, id), update);
        }
    }
}
