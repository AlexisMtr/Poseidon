using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class IdentityPoolFilter : PoolFilter, IFilter<Pool>
    {
        public User User { get; set; }
        public string Role { get; set; }
        
        public override object Clone()
        {
            IdentityPoolFilter filter = base.Clone() as IdentityPoolFilter;
            filter.User = User;
            filter.Role = Role;
            return filter;
        }

        public override IQueryable<Pool> Filter(IQueryable<Pool> source)
        {
            source = base.Filter(source);
            switch(Role)
            {
                case Roles.User:
                    source = source.Where(e => e.Users.Select(u => u.User.Id).Contains(User.Id));
                    break;
                default:
                    break;
            }
            return source;
        }
    }
}
