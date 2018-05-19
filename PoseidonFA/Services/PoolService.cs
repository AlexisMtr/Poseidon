using PoseidonFA.Models;
using PoseidonFA.Repositories;

namespace PoseidonFA.Services
{
    public class PoolService
    {
        private readonly IPoolRepository poolRepository;

        public PoolService(IPoolRepository poolRepository)
        {
            this.poolRepository = poolRepository;
        }

        public Pool Get(int id)
        {
            return poolRepository.Get(id);
        }
    }
}
