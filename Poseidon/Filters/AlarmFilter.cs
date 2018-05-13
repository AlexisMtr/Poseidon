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
}
