using System;
using System.Linq;

namespace Poseidon.Filters
{
    public class TimeFilter<T> : IFilter<T> where T : TimeObject
    {
        public DateTime? Before { get; set; }
        public DateTime? After { get; set; }
        
        public virtual IQueryable<T> Filter(IQueryable<T> source)
        {
            if (Before.HasValue)
            {
                source = source.Where(e => e.DateTime < Before.Value);
            }
            if (After.HasValue)
            {
                source = source.Where(e => e.DateTime > After.Value);
            }

            return source;
        }
    }
}
