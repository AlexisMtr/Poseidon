using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class AlarmFilter : TimeFilter<Alarm>
    {
        public AlarmState? State { get; set; }
        public AlarmType? Type { get; set; }
        
        public override IQueryable<Alarm> Filter(IQueryable<Alarm> source)
        {
            if (State.HasValue)
            {
                source = source.Where(e => State.Value.Equals(AlarmState.All) ||
                    (State.Value.Equals(AlarmState.Done) && e.Ack) ||
                    (State.Value.Equals(AlarmState.Pending) && !e.Ack));
            }
            if (Type.HasValue)
            {
                source = source.Where(e => e.AlarmType.Equals(Type.Value));
            }

            return base.Filter(source);
        }
    }

    public class IdentityAlarmFilter : IFilter<Alarm>
    {
        private readonly IFilter<Alarm> filter;
        public User User { get; set; }

        public IdentityAlarmFilter(IFilter<Alarm> filter)
        {
            this.filter = filter;
        }

        public IdentityAlarmFilter(IFilter<Alarm> filter, User user)
            : this (filter)
        {
            this.User = user;
        }
        public IQueryable<Alarm> Filter(IQueryable<Alarm> source)
        {
            if (User == null) return filter.Filter(source);

            return filter.Filter(source)
                .Where(e => e.Pool.Users.Select(u => u.User.Id).Contains(User.Id));
        }
    }
}
