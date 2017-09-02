using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Repositories
{
    public class MongoDbAlarmsRespository : IRepository<Alarm>
    {
        public MongoDbAlarmsRespository()
        {
        }

        public void Add(Alarm model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Alarm model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Alarm> Get()
        {
            throw new NotImplementedException();
        }

        public Alarm GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(Alarm model)
        {
            throw new NotImplementedException();
        }
    }
}
