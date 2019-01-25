using Poseidon.Filters;
using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Poseidon.Repositories
{
    public interface IAlarmRepository : IRepository<Alarm, int>
    {
        IEnumerable<Alarm> GetByPool(int poolId, IFilter<Alarm> filter, int rowsPerPage, int pageNumber, params Expression<Func<Alarm, object>>[] includes);
        int CountByPool(int poolId, IFilter<Alarm> filter);
    }
}
