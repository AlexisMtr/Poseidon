using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Poseidon.Configuration;
using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;

namespace Poseidon.Repositories.SQL
{
    public class PoolRepository : IPoolRepository
    {
        private readonly PoseidonContext context;

        public PoolRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public void Add(Pool entity)
        {
            context.Pools.Add(entity);
        }

        public int Count(IFilter<Pool> filter)
        {
            return context.Pools
                .Count(filter ?? new PoolFilter());
        }

        public void Delete(Pool entity)
        {
            context.Pools.Remove(entity);
        }

        public IEnumerable<Pool> Get(IFilter<Pool> filter, int rowsPerPage, int pageNumber)
        {
            int skip = Math.Max(0, pageNumber - 1) * rowsPerPage;

            return context.Pools
                .Include(e => e.Device)
                .Include(e => e.Alarms)
                .Where(filter ?? new PoolFilter())
                .OrderBy(e => e.Name)
                .Skip(skip)
                .Take(rowsPerPage);
        }

        public IQueryable<Pool> Get(IFilter<Pool> filter)
        {
            return context.Pools
                .Include(e => e.Device)
                .Include(e => e.Alarms)
                .Where(filter ?? new PoolFilter());
        }

        public Pool GetById(int id, IFilter<Pool> filter)
        {
            return context.Pools
                .Include(e => e.Device)
                .Include(e => e.Alarms)
                .Where(filter ?? new PoolFilter())
                .Include(e => e.Telemetries)
                .FirstOrDefault(e => e.Id == id);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
