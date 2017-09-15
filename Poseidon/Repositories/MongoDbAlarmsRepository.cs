using MongoDB.Driver;
using Poseidon.Models;
using System;
using System.Linq;
using AlexisMtrTools.DateTime;
using System.Collections.Generic;
using Poseidon.Configuration;

namespace Poseidon.Repositories
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
        
        public IQueryable<Alarm> Get()
        {
            return this.Get(AlarmState.All);
        }

        public IQueryable<Alarm> Get(AlarmState state)
        {
            return this.PoolsCollection.AsQueryable()
                      .SelectMany(p => p.Alarms)
                      .Where(a => this.MatchState(a, state));
        }

        public void Add(Alarm model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Push(p => p.Alarms, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public void Delete(Alarm model)
        {
            UpdateDefinition<Pool> update = Builders<Pool>.Update.Pull(p => p.Alarms, model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public Alarm GetById(string id)
        {
            return this.PoolsCollection.AsQueryable()
                .SelectMany(p => p.Alarms)
                .FirstOrDefault(a => a.Id.Equals(id));
        }

        public void Update(string id, Alarm model)
        {
            Alarm alarm = this.GetById(id);
            int alarmIndex = this.GetIndexOfAlarm(alarm.PoolId, alarm);

            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set($"Alarms.{alarmIndex}", model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, alarm.PoolId), update);
        }

        public void Ack(string id)
        {
            Alarm alarm = this.GetById(id);
            int alarmIndex = this.GetIndexOfAlarm(alarm.PoolId, alarm);

            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set($"Alarms.{alarmIndex}.Ack", true)
                .Set($"Alarms.{alarmIndex}.AcknowledgmentTimestamp", DateTime.Now.ToTimestamp());
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, alarm.PoolId), update);
        }

        public IEnumerable<Alarm> GetByPoolId(string poolId, AlarmState filter)
        {
            List<Alarm> alarms = this.PoolsCollection.AsQueryable()
                .Where(p => p.Id.Equals(poolId))
                .SelectMany(p => p.Alarms)
                .ToList();

            return alarms.Where(a => this.MatchState(a, filter));
        }

        private bool MatchState(Alarm item, AlarmState filter)
        {
            return filter.Equals(AlarmState.All) ||
                (filter.Equals(AlarmState.Done) && item.Ack) ||
                (filter.Equals(AlarmState.Pending) && !item.Ack);
        }

        private int GetIndexOfAlarm(string poolId, Alarm alarm)
        {
            Pool pool = this.PoolsCollection.AsQueryable()
                .FirstOrDefault(p => p.Id.Equals(poolId));

            return pool.Alarms.ToList().IndexOf(alarm);
        }
    }
}
