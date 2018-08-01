using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class IdentityPoolFilter : IFilter<Pool>
    {
        private readonly IFilter<Pool> filter;
        public User User { get; set; }

        public IdentityPoolFilter(IFilter<Pool> filter)
        {
            this.filter = filter;
        }
        public IdentityPoolFilter(IFilter<Pool> filter, User user)
            : this(filter)
        {
            this.User = user;
        }
        
        public IQueryable<Pool> Filter(IQueryable<Pool> source)
        {
            if (User == null) return filter.Filter(source);

            return filter.Filter(source)
                .Where(e => e.Users.Select(u => u.User.Id).Contains(User.Id));
        }
    }
}
