using System.Collections.Generic;

namespace Poseidon.Dtos
{
    public class PaginatedDto<T>
    {
        public int PageCount { get; set; }
        public int TotalElementCount { get; set; }
        public IEnumerable<T> Elements { get; set; }
        public string NextPageUrl { get; set; }
    }
}
