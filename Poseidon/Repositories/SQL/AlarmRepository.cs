using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Poseidon.Configuration;
using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;

namespace Poseidon.Repositories.SQL
{
    public class AlarmRepository : IAlarmRepository
    {
        private readonly PoseidonContext context;

        public AlarmRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public void Add(Alarm entity)
        {
            context.Alarms.Add(entity);
        }

        public int Count(IFilter<Alarm> filter)
        {
            return context.Alarms
                .Count(filter ?? new AlarmFilter());
        }

        public int CountByPool(int poolId, IFilter<Alarm> filter)
        {
            return context.Alarms
                .Where(filter ?? new AlarmFilter())
                .Where(e => e.Pool.Id.Equals(poolId))
                .Count();
        }

        public void Delete(Alarm entity)
        {
            context.Alarms.Remove(entity);
        }

        public IQueryable<Alarm> Get(IFilter<Alarm> filter)
        {
            return context.Alarms.Where(filter ?? new AlarmFilter());
        }

        public Alarm GetById(int id)
        {
            return context.Alarms.FirstOrDefault(e => e.Id.Equals(id));
        }

        public IEnumerable<Alarm> GetByPool(int poolId, IFilter<Alarm> filter, int rowsPerPage, int pageNumber, params Expression<Func<Alarm, object>>[] includes)
        {
            int skip = Math.Max(0, pageNumber - 1) * rowsPerPage;

            return context.Alarms
                .Where(filter ?? new AlarmFilter())
                .Where(e => e.Pool.Id.Equals(poolId))
                .OrderBy(e => e.DateTime)
                .Skip(skip)
                .Take(rowsPerPage);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
