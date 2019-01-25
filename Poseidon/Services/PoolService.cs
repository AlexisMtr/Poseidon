using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Exceptions;
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
        private readonly DeviceService deviceService;

        public PoolService(IPoolRepository poolRepository, IMapper mapper, DeviceService deviceService)
        {
            this.poolRepository = poolRepository;
            this.mapper = mapper;
            this.deviceService = deviceService;
        }

        public Pool Get(int poolId, User user = null)
        {
            IFilter<Pool> filter = new PoolFilter();
            if (user != null)
            {
                filter = new IdentityPoolFilter(filter, user);
            }

            Pool pool = poolRepository.GetById(poolId, filter);
            if (pool == null) throw new NotFoundException(typeof(Pool));

            return pool;
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

        public Pool Update(int poolId, PoolCreationDto model, User user)
        {
            Pool pool = Get(poolId, user);

            pool.Name = model.Name;
            pool.Latitude = model.Latitude;
            pool.Longitude = model.Longitude;
            pool.PhMaxValue = model.PhMaxValue;
            pool.PhMinValue = model.PhMinValue;
            pool.WaterLevelMaxValue = model.WaterLevelMaxValue;
            pool.WaterLevelMinValue = model.WaterLevelMinValue;
            pool.TemperatureMaxValue = model.TemperatureMaxValue;
            pool.TemperatureMinValue = model.TemperatureMinValue;

            poolRepository.SaveChanges();
            return pool;
        }

        public PaginatedElement<Pool> Get(IFilter<Pool> filter, int rowsPerPage, int pageNumber, User user = null)
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

        public void Associate(int poolId, string deviceId, User user)
        {
            Device device = deviceService.Get(deviceId);
            Pool pool = Get(poolId, user);

            pool.Device = device;
            poolRepository.SaveChanges();
        }

        public void Dissociate(int poolId, User user)
        {
            Pool pool = Get(poolId, user);
            pool.Device = null;
            poolRepository.SaveChanges();
        }
    }
}
