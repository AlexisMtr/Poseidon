using Poseidon.Filters;
using System.Linq;

namespace Poseidon.Repositories
{
    public interface IRepository<T, TId>
    {
        IQueryable<T> Get(IFilter<T> filter);
        T GetById(TId id, IFilter<T> filter);
        void Add(T entity);
        void Delete(T entity);
        int Count(IFilter<T> filter);

        void SaveChanges();
    }
}
