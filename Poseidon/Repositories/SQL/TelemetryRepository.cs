using Poseidon.Configuration;
using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Poseidon.Repositories.SQL
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly PoseidonContext context;

        public TelemetryRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public void Add(Telemetry entity)
        {
            context.Telemetries.Add(entity);
        }

        public int Count(IFilter<Telemetry> filter)
        {
            return context.Telemetries
                .Count(filter ?? new TelemetryFilter());
        }

        public int CountByPool(int poolId, IFilter<Telemetry> filter)
        {
            return context.Telemetries
                .Where(e => e.Pool.Id == poolId)
                .Count(filter ?? new TelemetryFilter());
        }

        public void Delete(Telemetry entity)
        {
            context.Telemetries.Remove(entity);
        }

        public IQueryable<Telemetry> Get(IFilter<Telemetry> filter)
        {
            return context.Telemetries
                .Where(filter ?? new TelemetryFilter());
        }

        public Telemetry GetById(int id)
        {
            return context.Telemetries
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Telemetry> GetByPool(int poolId, IFilter<Telemetry> filter, int rowsPerPage, int pageNumber, params Expression<Func<Telemetry, object>>[] includes)
        {
            int skip = Math.Max(0, pageNumber - 1) * rowsPerPage;

            return context.Telemetries
                .Where(filter ?? new TelemetryFilter())
                .Where(e => e.Pool.Id == poolId)
                .OrderBy(e => e.DateTime)
                .Skip(skip)
                .Take(rowsPerPage);
        }

        public Telemetry GetLastTelemetry(int poolId, IFilter<Telemetry> filter)
        {
            return context.Telemetries
                .Where(filter ?? new TelemetryFilter())
                .Where(e => e.Pool.Id == poolId)
                .OrderBy(e => e.DateTime)
                .LastOrDefault();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
