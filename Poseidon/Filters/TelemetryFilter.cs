using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class TelemetryFilter : TimeFilter<Telemetry>
    {
        public TelemetryType? Type { get; set; }

        public override object Clone()
        {
            TelemetryFilter filter = base.Clone() as TelemetryFilter;
            filter.Type = Type;

            return filter;
        }

        public override IQueryable<Telemetry> Filter(IQueryable<Telemetry> source)
        {
            if(Type.HasValue)
            {
                source = source.Where(e => e.Type.Equals(Type.Value));
            }

            return base.Filter(source);
        }
    }
}
