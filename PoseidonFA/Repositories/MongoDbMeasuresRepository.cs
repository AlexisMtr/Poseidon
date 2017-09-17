using MongoDB.Driver;
using PoseidonFA.Configuration;
using PoseidonFA.Models;
using System.Linq;

namespace PoseidonFA.Repositories
{
    class MongoDbMeasuresRepository : IRepository<Measure>
    {
        private readonly MongoDbContext Context;
        private readonly IMongoCollection<Pool> PoolsCollection;

        public MongoDbMeasuresRepository(MongoDbContext context)
        {
            this.Context = context;
            this.PoolsCollection = this.Context.Database.GetCollection<Pool>("pools");
        }

        public void Add(Measure model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Push(p => p.Measures, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
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
    }
}
