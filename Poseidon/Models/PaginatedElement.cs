using System.Collections.Generic;

namespace Poseidon.Models
{
    public interface IPaginatedResource
    {
        int PageCount { get; }
    }

    public class PaginatedElement<T> : IPaginatedResource
    {
        public int PageCount { get; set; }
        public int TotalElementCount { get; set; }
        public IEnumerable<T> Elements { get; set; }
    }
}
