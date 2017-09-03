﻿using MongoDB.Driver;
using Poseidon.Models;
using Poseidon.Services;
using System;
using System.Linq;
using AlexisMtrTools.DateTime;
using System.Collections.Generic;

namespace Poseidon.Repositories
{
    public class MongoDbAlarmsRespository : IRepository<Alarm>
    {
        private MongoDbService DbService { get; set; }
        private IMongoCollection<Pool> PoolsCollection { get; set; }

        public MongoDbAlarmsRespository(MongoDbService service)
        {
            this.DbService = service;
            this.PoolsCollection = this.DbService.Database.GetCollection<Pool>("pools");
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
            (string PoolId, int AlarmIndex) = this.GetPoolIdAndAlarmIndex(id);

            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set(p => p.Alarms.ToList()[AlarmIndex], model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id,PoolId), update);
        }

        public void Ack(string id)
        {
            (string PoolId, int AlarmIndex) = this.GetPoolIdAndAlarmIndex(id);

            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set(p => p.Alarms.ToList()[AlarmIndex].Ack, true)
                .Set(p => p.Alarms.ToList()[AlarmIndex].AcknowledgmentTimestamp, DateTime.Now.ToTimestamp());
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, PoolId), update);
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

        private (string PoolId, int AlarmIndex) GetPoolIdAndAlarmIndex(string alarmId)
        {
            Pool pool = this.PoolsCollection.AsQueryable()
                .FirstOrDefault(p => p.Alarms.Select(a => a.Id).Contains(alarmId));

            Alarm alarm = this.PoolsCollection.AsQueryable()
                .FirstOrDefault(p => p.Alarms.Select(a => a.Id).Contains(alarmId))
                .Alarms.FirstOrDefault(a => a.Id.Equals(alarmId));

            return (pool.Id, pool.Alarms.ToList().IndexOf(alarm));
        }
    }
}
