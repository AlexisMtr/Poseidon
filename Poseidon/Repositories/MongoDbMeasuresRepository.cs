using MongoDB.Driver;
using Poseidon.Models;
using Poseidon.Services;
using System;
using System.Linq;

namespace Poseidon.Repositories
{
    public class MongoDbMeasuresRepository : IRepository<Measure>
    {
        private MongoDbService DbService { get; set; }
        private IMongoCollection<Pool> PoolsCollection { get; set; }

        public MongoDbMeasuresRepository(MongoDbService service)
        {
            this.DbService = service;
            this.PoolsCollection = this.DbService.Database.GetCollection<Pool>("pools");
        }

        public void Add(Measure model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Push(p => p.Measures, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public void Delete(Measure model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Pull(p => p.Measures, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public IQueryable<Measure> Get()
        {
            return this.PoolsCollection.AsQueryable()
                .SelectMany(p => p.Measures);
        }

        public Measure GetById(string id)
        {
            return this.PoolsCollection.AsQueryable()
                .SelectMany(p => p.Measures)
                .FirstOrDefault(m => m.Id.Equals(id));
        }

        public void Update(string id, Measure model)
        {
            Pool pool = this.PoolsCollection.AsQueryable()
                .FirstOrDefault(p => p.Id.Equals(model.PoolId));

            Measure measure = this.PoolsCollection.AsQueryable()
                .SelectMany(p => p.Measures)
                .FirstOrDefault(m => m.Id.Equals(id) && m.PoolId.Equals(model.PoolId));

            int index = pool.Measures.ToList().IndexOf(measure);


            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set(p => p.Measures.ToList()[index], model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public IQueryable<Measure> GetByPoolId(string poolId)
        {
            return this.PoolsCollection.AsQueryable()
                .Where(p => p.Id.Equals(poolId))
                .SelectMany(p => p.Measures);
        }
    }
}
