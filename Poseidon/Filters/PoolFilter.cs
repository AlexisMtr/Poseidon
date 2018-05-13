using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class PoolFilter : IFilter<Pool>
    {
        public string Id { get; set; }
        public bool? HasPendingAlarms { get; set; }


        public IQueryable<Pool> Filter(IQueryable<Pool> source)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                source = source.Where(e => e.Id.Equals(Id));
            }
            if(HasPendingAlarms.HasValue)
            {
                if (HasPendingAlarms.Value) source = source.Where(e => e.Alarms.Any(a => a.Ack.Equals(false)));
                else source = source.Where(e => e.Alarms.All(a => a.Ack.Equals(true)));
            }

            return source;
        }
    }
}
