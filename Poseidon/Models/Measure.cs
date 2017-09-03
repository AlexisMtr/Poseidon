namespace Poseidon.Models
{
    public class Measure
    {
        public string Id { get; set; }
        public string PoolId { get; set; }
        public long Timestamp { get; set; }
        public MeasureType Type { get; set; }
        public object Value { get; set; }
    }
}
