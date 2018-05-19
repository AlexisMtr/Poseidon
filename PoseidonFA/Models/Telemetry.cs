namespace PoseidonFA.Models
{
    public class Telemetry
    {
        public string Id { get; set; }
        public Pool Pool { get; set; }
        public TelemetryType Type { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
    }
}
