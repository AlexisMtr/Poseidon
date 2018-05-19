using Poseidon.Filters;
using System.Linq;

namespace Poseidon.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get(IFilter<T> filter);
        T GetById(int id);
        void Add(T entity);
        void Delete(T entity);
        int Count(IFilter<T> filter);

        void SaveChanges();
    }
}
