using Poseidon.Models;
using System;
using System.Linq;

namespace Poseidon.Repositories
{
    public class MongoDbMeasuresRepository : IRepository<Measure>
    {
        public MongoDbMeasuresRepository()
        {
        }

        public void Add(Measure model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Measure model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Measure> Get()
        {
            throw new NotImplementedException();
        }

        public Measure GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Measure model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Measure> GetByPoolId(string poolId)
        {
            throw new NotImplementedException();
        }
    }
}
