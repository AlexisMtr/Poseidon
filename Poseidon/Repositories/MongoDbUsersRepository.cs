using MongoDB.Driver;
using Poseidon.Models;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq;

namespace Poseidon.Repositories
{
    public class MongoDbUsersRepository : IRepository<User>
    {
        private MongoDbService DbService { get; set; }
        private IMongoCollection<User> UsersCollection { get; set; }

        public MongoDbUsersRepository(MongoDbService service)
        {
            this.DbService = service;
            this.UsersCollection = this.DbService.Database.GetCollection<User>("users");
        }

        public void Add(User model)
        {
            this.UsersCollection.InsertOne(model);
        }

        public void Delete(User model)
        {
            this.UsersCollection.DeleteOne(Builders<User>.Filter.Eq(u => u.Id, model.Id));
        }

        public IQueryable<User> Get()
        {
            return this.UsersCollection.AsQueryable();
        }

        public User GetById(string id)
        {
            return this.UsersCollection.AsQueryable()
                .FirstOrDefault(u => u.Id.Equals(id));
        }

        public void Update(string id, User model)
        {
            UpdateDefinition<User> update = Builders<User>.Update.Set(u => u, model);
            this.UsersCollection.UpdateOne(Builders<User>.Filter.Eq(u => u.Id, id), update);
        }

        public IEnumerable<Pool> GetPools(string id)
        {
            IMongoCollection<Pool> pools = this.DbService.Database.GetCollection<Pool>("pools");
            IEnumerable<string> poolsId = this.UsersCollection.AsQueryable()
                .FirstOrDefault(u => u.Id.Equals(id))
                .PoolsId;

            FilterDefinition<Pool> filter = Builders<Pool>.Filter.In(p => p.Id, poolsId);

            return pools.Find(filter).ToList();
        }

        public void RemovePoolReference(string poolId)
        {
            IEnumerable<User> usersWithPoolReference = this.UsersCollection.AsQueryable()
                .Where(u => u.PoolsId.Contains(poolId));

            FilterDefinition<User> filter;
            UpdateDefinition<User> update;
            foreach (User user in usersWithPoolReference)
            {
                filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                update = Builders<User>.Update.Pull(u => u.PoolsId, poolId);
                this.UsersCollection.UpdateOne(filter, update);
            }
        }

        public void AddPoolReference(string id, string poolId)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Id, id);
            UpdateDefinition<User> update = Builders<User>.Update.Push(u => u.PoolsId, poolId);
            this.UsersCollection.UpdateOne(filter, update);
        }
    }
}
