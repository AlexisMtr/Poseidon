using Poseidon.Models;
using System;
using System.Linq;

namespace Poseidon.Repositories
{
    public interface IMeasuresRepository<T> : IRepository<T> where T : Measure
    {
        IQueryable<Measure> GetByPoolId(string poolId);
        IQueryable<Measure> GetByPoolIdBetween(string poolId, DateTime min, DateTime max);
    }
}
