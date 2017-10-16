﻿using MongoDB.Driver;
using Poseidon.Configuration;
using Poseidon.Models;
using System.Linq;
using System;
using AlexisMtrTools.DateTime;

namespace Poseidon.Repositories
{
    public class MongoDbMeasuresRepository : IMeasuresRepository<Measure>
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


            UpdateDefinition<Pool> update = Builders<Pool>.Update.Set($"Measures.{index}", model);
            this.PoolsCollection.UpdateOne(Builders<Pool>.Filter.Eq(p => p.Id, model.PoolId), update);
        }

        public IQueryable<Measure> GetByPoolId(string poolId)
        {
            return this.PoolsCollection.AsQueryable()
                .Where(p => p.Id.Equals(poolId))
                .SelectMany(p => p.Measures);
        }

        public IQueryable<Measure> GetByPoolIdBetween(string poolId, DateTime min, DateTime max)
        {
            return this.GetByPoolId(poolId)
                .Where(m => m.Timestamp.ToDateTime().IsBetween(min, max));
        }
    }
}
