using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        T GetById(string id);
        void Add(T model);
        void Update(T model);
        void Delete(T model);
    }
}
