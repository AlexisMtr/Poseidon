﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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
                .Include(e => e.Pool)
                .Where(filter ?? new AlarmFilter())
                .Where(e => e.Pool.Id == poolId)
                .Count();
        }

        public void Delete(Alarm entity)
        {
            context.Alarms.Remove(entity);
        }

        public IQueryable<Alarm> Get(IFilter<Alarm> filter)
        {
            return context.Alarms
                .Include(e => e.Pool)
                .Where(filter ?? new AlarmFilter());
        }

        public Alarm GetById(int id, IFilter<Alarm> filter)
        {
            return context.Alarms
                .Include(e => e.Pool)
                .Where(filter ?? new AlarmFilter())
                .FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Alarm> GetByPool(int poolId, IFilter<Alarm> filter, int rowsPerPage, int pageNumber, params Expression<Func<Alarm, object>>[] includes)
        {
            int skip = Math.Max(0, pageNumber - 1) * rowsPerPage;

            return context.Alarms
                .Include(e => e.Pool)
                .Where(filter ?? new AlarmFilter())
                .Where(e => e.Pool.Id == poolId)
                .Skip(skip)
                .Take(rowsPerPage);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
