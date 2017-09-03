using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Repositories
{
    public class MongoDbPoolRespository : IRepository<Pool>
    {
        public MongoDbPoolRespository()
        {
        }

        public void Add(Pool model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Pool model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Pool> Get()
        {
            throw new NotImplementedException();
        }

        public Pool GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Pool model)
        {
            throw new NotImplementedException();
        }
    }
}
