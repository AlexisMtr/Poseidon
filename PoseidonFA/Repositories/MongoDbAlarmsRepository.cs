using MongoDB.Driver;
using PoseidonFA.Configuration;
using PoseidonFA.Models;
using System.Linq;

namespace PoseidonFA.Repositories
{
    public class MongoDbAlarmsRepository : IRepository<Alarm>
    {
        private readonly MongoDbContext Context;
        private readonly IMongoCollection<Pool> PoolsCollection;

        public MongoDbAlarmsRepository(MongoDbContext context)
        {
            this.Context = context;
            this.PoolsCollection = this.Context.Database.GetCollection<Pool>("pools");
        }

        public void Add(Alarm model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Push(p => p.Alarms, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public void Update(string id, Alarm model)
        {
            Alarm alarm = this.GetById(id);
            int alarmIndex = this.GetIndexOfAlarm(alarm.PoolId, alarm);

            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set($"Alarms.{alarmIndex}", model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, alarm.PoolId), update);
        }

        private Alarm GetById(string id)
        {
            return this.PoolsCollection.AsQueryable()
                .SelectMany(p => p.Alarms)
                .FirstOrDefault(a => a.Id.Equals(id));
        }

        private int GetIndexOfAlarm(string poolId, Alarm alarm)
        {
            Pool pool = this.PoolsCollection.AsQueryable()
                .FirstOrDefault(p => p.Id.Equals(poolId));

            return pool.Alarms.ToList().IndexOf(alarm);
        }
    }
}
