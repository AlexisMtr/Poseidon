using Poseidon.Filters;
using Poseidon.Models;
using System.Collections.Generic;

namespace Poseidon.Repositories
{
    public interface IPoolRepository : IRepository<Pool>
    {
        IEnumerable<Pool> Get(IFilter<Pool> filter, int rowsPerPage, int pageNumber);
    }
}
