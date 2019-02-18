using System;
using System.Linq;

namespace Poseidon.Filters
{
    public class TimeFilter<T> : IFilter<T> where T : TimeObject
    {
        public enum Sort
        {
            Asc = 0,
            Desc = 1
        }

        public DateTime? Before { get; set; }
        public DateTime? After { get; set; }
        public Sort? Order { get; set; }
        
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
            if (Order.HasValue)
            {
                source = Order.Value == Sort.Asc ? source.OrderBy(e => e.DateTime) : source.OrderByDescending(e => e.DateTime);
            }

            return source;
        }
    }
}
