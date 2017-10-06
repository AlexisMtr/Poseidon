using PoseidonFA.Models;
using System.Collections.Generic;

namespace PoseidonFA.Repositories
{
    public interface IConfigurationRepository<T> : IRepository<T> where T : PoolConfiguration
    {
        T GetByPoolId(string id);
        IEnumerable<string> GetUsersByPoolId(string poolId);
    }
}
