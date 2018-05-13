using System.Collections.Generic;

namespace Poseidon.Models
{
    public class PaginatedElement<T>
    {
        public int PageCount { get; set; }
        public int TotalElementCount { get; set; }
        public IEnumerable<T> Elements { get; set; }
    }
}
