using Poseidon.Models;
using System.Linq;

namespace Poseidon.Filters
{
    public class DeviceFilter : IFilter<Device>
    {
        public string Version { get; set; }

        public IQueryable<Device> Filter(IQueryable<Device> source)
        {
            if(!string.IsNullOrEmpty(Version))
            {
                source = source.Where(e => e.Version.Equals(Version));
            }
            return source;
        }
    }
}
