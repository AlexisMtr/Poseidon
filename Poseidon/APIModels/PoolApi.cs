using Poseidon.Models;

namespace Poseidon.APIModels
{
    public class PoolApi
    {
        public string PoolId { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public long LastUpdate { get; set; }
        public int AlarmsCount { get; set; }
    }
}
