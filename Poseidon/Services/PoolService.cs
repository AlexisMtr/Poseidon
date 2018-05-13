using Poseidon.Models;
using Poseidon.Repositories;
using System;

namespace Poseidon.Services
{
    public class PoolService
    {
        private readonly IPoolRepository<Pool> poolRepository;

        public PoolService(IPoolRepository<Pool> poolRepository)
        {
            this.poolRepository = poolRepository;
        }

        public Pool Get(string poolId)
        {
            return poolRepository.GetById(poolId);
        }

        public Pool Add(string name, double latitude, double longitude)
        {
            Pool pool = new Pool
            {
                Id = Guid.NewGuid().ToString(),
                Location = new Location { Latitude = latitude, Longitude = longitude },
                Name = name
            };

            poolRepository.Add(pool);
            poolRepository.SaveChanges();

            return pool;
        }
    }
}
