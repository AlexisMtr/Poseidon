using Poseidon.Models;

namespace Poseidon.Repositories
{
    public interface IPoolConfigurationsRepository<T> : IRepository<T> where T : PoolConfiguration
    {
        T GetByPoolId(string id);
    }
}
