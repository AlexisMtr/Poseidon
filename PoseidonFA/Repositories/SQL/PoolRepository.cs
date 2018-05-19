using System.Linq;
using PoseidonFA.Configuration;
using PoseidonFA.Models;

namespace PoseidonFA.Repositories.SQL
{
    public class PoolRepository : IPoolRepository
    {
        private readonly PoseidonContext context;

        public PoolRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public Pool Get(int id)
        {
            return context.Pools.FirstOrDefault(e => e.Id.Equals(id));
        }
    }
}
