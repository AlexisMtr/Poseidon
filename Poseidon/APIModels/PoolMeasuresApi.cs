using Poseidon.Models;

namespace Poseidon.APIModels
{
    public class PoolMeasuresApi
    {
        public string PoolId { get; set; }
        public Measure Ph { get; set; }
        public Measure Temperature { get; set; }
        public Measure Level { get; set; }
    }
}
