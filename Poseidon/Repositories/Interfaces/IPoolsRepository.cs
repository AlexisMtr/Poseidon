using Poseidon.Models;

namespace Poseidon.Repositories
{
    public interface IPoolsRepository<T> : IRepository<T> where T : Pool
    {
    }
}
