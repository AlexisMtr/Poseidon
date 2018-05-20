using Poseidon.Filters;
using System;

namespace Poseidon.Models
{
    public class Telemetry : TimeObject
    {
        public int Id { get; set; }
        public Pool Pool { get; set; }
        public TelemetryType Type { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
    }
}
