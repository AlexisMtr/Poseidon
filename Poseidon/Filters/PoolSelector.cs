using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class PoolSelector : IFilter<Pool>
    {
        private readonly string userId;

        public PoolSelector(string userId)
        {
            this.userId = userId;
        }
        
        public IQueryable<Pool> Filter(IQueryable<Pool> source)
        {
            return source.Where(e => e.Users.Select(u => u.User.Id).Contains(userId));
        }
    }
}
