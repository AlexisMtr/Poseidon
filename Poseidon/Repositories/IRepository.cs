using System.Linq;

namespace Poseidon.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        T GetById(int id);
        void Add(T entity);
        void Delete(T entity);

        void SaveChanges();
    }
}
