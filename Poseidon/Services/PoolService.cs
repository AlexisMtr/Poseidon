using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;
using Poseidon.Repositories;
using System.Collections.Generic;

namespace Poseidon.Services
{
    public class PoolService
    {
        private readonly IPoolRepository poolRepository;
        private readonly IMapper mapper;

        public PoolService(IPoolRepository poolRepository, IMapper mapper)
        {
            this.poolRepository = poolRepository;
            this.mapper = mapper;
        }

        public Pool Get(int poolId)
        {
            return poolRepository.GetById(poolId);
        }

        public Pool Add(PoolCreationDto model, User user)
        {
            Pool pool = mapper.Map<Pool>(model);

            pool.Users.Add(new UserPoolAssociation
            {
                User = user,
                Pool = pool
            });

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

        public PaginatedElement<Pool> Get(IFilter<Pool> filter, int rowsPerPage, int pageNumber, User user = null, string role = null)
        {
            if(user != null)
            {
                filter = new IdentityPoolFilter(filter, user);
            }

            IEnumerable<Pool> alarms = poolRepository.Get(filter, rowsPerPage, pageNumber);
            int totalElementCount = poolRepository.Count(filter);

            return new PaginatedElement<Pool>
            {
                TotalElementCount = totalElementCount,
                Elements = alarms,
                PageCount = RestApiHelper.GetPageCount(totalElementCount, rowsPerPage)
            };
        }
    }
}
