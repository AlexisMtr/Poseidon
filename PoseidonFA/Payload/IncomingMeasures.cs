using PoseidonFA.Models;

namespace PoseidonFA.Payload
{
    public class IncomingMeasures
    {
        public string PoolId { get; set; }
        public Measure Ph { get; set; }
        public Measure Level { get; set; }
        public Measure Temperature { get; set; }
        public Measure Battery { get; set; }
    }
}
