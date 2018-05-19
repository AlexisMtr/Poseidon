using Poseidon.Dtos;
using Poseidon.Filters;
using Poseidon.Models;
using Poseidon.Repositories;
using System.Collections.Generic;

namespace Poseidon.Services
{
    public class PoolService
    {
        private readonly IPoolRepository poolRepository;

        public PoolService(IPoolRepository poolRepository)
        {
            this.poolRepository = poolRepository;
        }

        public Pool Get(int poolId)
        {
            return poolRepository.GetById(poolId);
        }

        public Pool Add(PoolCreationDto model, User user)
        {
            Pool pool = new Pool
            {
                Name = model.Name,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                DeviceId = model.DeviceId,
                PhMaxValue = model.PhMaxValue,
                PhMinValue = model.PhMinValue,
                WaterLevelMaxValue = model.WaterLevelMaxValue,
                WaterLevelMinValue = model.WaterLevelMinValue,
                TemperatureMaxValue = model.TemperatureMaxValue,
                TemperatureMinValue = model.TemperatureMinValue,
                Users = new List<User> { user }
            };

            poolRepository.Add(pool);
            poolRepository.SaveChanges();

            return pool;
        }

        public Pool Update(int poolId, PoolCreationDto model)
        {
            Pool pool = Get(poolId);

            pool.Name = model.Name;
            pool.Latitude = model.Latitude;
            pool.Longitude = model.Longitude;
            pool.DeviceId = model.DeviceId;
            pool.PhMaxValue = model.PhMaxValue;
            pool.PhMinValue = model.PhMinValue;
            pool.WaterLevelMaxValue = model.WaterLevelMaxValue;
            pool.WaterLevelMinValue = model.WaterLevelMinValue;
            pool.TemperatureMaxValue = model.TemperatureMaxValue;
            pool.TemperatureMinValue = model.TemperatureMinValue;

            poolRepository.SaveChanges();
            return pool;
        }

        public PaginatedElement<Pool> Get(IFilter<Pool> filter, int rowsPerPage, int pageNumber)
        {
            IEnumerable<Pool> alarms = poolRepository.Get(filter, rowsPerPage, pageNumber);
            int totalElementCount = poolRepository.Count(filter);

            return new PaginatedElement<Pool>
            {
                TotalElementCount = totalElementCount,
                Elements = alarms,
                PageCount = 0
            };
        }
    }
}
