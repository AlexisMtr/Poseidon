using Poseidon.Models;
using System.Linq;

namespace Poseidon.Repositories
{
    public interface IMeasuresRepository<T> : IRepository<T> where T : Measure
    {
        IQueryable<Measure> GetByPoolId(string poolId);
    }
}
