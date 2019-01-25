using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class IdentityDeviceFilter : IFilter<Device>
    {
        private readonly IFilter<Device> filter;
        public User User { get; set; }

        public IdentityDeviceFilter(IFilter<Device> filter)
        {
            this.filter = filter;
        }

        public IdentityDeviceFilter(IFilter<Device> filter, User user)
            : this(filter)
        {
            this.User = user;
        }

        public IQueryable<Device> Filter(IQueryable<Device> source)
        {
            if (User == null) return filter.Filter(source);

            return filter.Filter(source)
                .Where(e => e.Pool.Users.Select(i => i.User.Id).Contains(User.Id));
        }
    }
}
