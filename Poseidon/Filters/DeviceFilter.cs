using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class DeviceFilter : IFilter<Device>
    {
        public string Version { get; set; }
        public bool? IsAvailable { get; set; }

        public IQueryable<Device> Filter(IQueryable<Device> source)
        {
            if(!string.IsNullOrEmpty(Version))
            {
                source = source.Where(e => e.Version.Equals(Version));
            }

            if (IsAvailable.HasValue)
            {
                source = source.Where(e => (e.Pool == null) == IsAvailable.Value);
            }

            return source;
        }
    }
}
