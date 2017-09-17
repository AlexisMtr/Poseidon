using Poseidon.Models;
using System.Collections.Generic;

namespace Poseidon.Repositories
{
    public interface IUsersRepository<T> : IRepository<T> where T : User
    {
        IEnumerable<Pool> GetPools(string id);
        void RemovePoolReference(string poolId);
        void AddPoolReference(string id, string poolId);
        User GetByLoginAndPassword(string login, string passsword);
    }
}
