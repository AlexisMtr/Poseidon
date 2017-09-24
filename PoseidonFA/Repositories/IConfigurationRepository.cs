using PoseidonFA.Models;

namespace PoseidonFA.Repositories
{
    public interface IConfigurationRepository<T> : IRepository<T> where T : PoolConfiguration
    {
        T GetByPoolId(string id);
    }
}
