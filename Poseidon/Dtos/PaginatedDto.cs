using Poseidon.Models;
using System.Collections.Generic;

namespace Poseidon.Dtos
{
    public interface IPaginatedDto : IPaginatedResource
    {
        string NextPageUrl { get; set; }
        string PreviousPageUrl { get; set; }
    }

    public class PaginatedDto<T> : IPaginatedDto
    {
        public int PageCount { get; set; }
        public int TotalElementCount { get; set; }
        public IEnumerable<T> Elements { get; set; }
        public string NextPageUrl { get; set; }
        public string PreviousPageUrl { get; set; }
    }
}
