using Poseidon.Filters;

namespace Poseidon.Models
{
    public class Telemetry : TimeObject
    {
        public string Id { get; set; }
        public Pool Pool { get; set; }
        public long Timestamp { get; set; }
        public TelemetryType Type { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
    }
}
