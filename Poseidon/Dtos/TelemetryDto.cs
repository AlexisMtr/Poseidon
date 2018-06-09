using Poseidon.Models;
using System;

namespace Poseidon.Dtos
{
    public class TelemetryDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int PoolId { get; set; }
        public TelemetryType Type { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
    }
}
