using MongoDB.Driver;
using Poseidon.Models;
using Poseidon.Services;
using System.Linq;

namespace Poseidon.Repositories
{
    public class MongoDbPoolRespository : IRepository<Pool>
    {
        private MongoDbService DbService { get; set; }
        private IMongoCollection<Pool> PoolsCollection { get; set; }

        public MongoDbPoolRespository(MongoDbService service)
        {
            this.DbService = service;
            this.PoolsCollection = this.DbService.Database.GetCollection<Pool>("pools");
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
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set(p => p, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, id), update);
        }
    }
}
