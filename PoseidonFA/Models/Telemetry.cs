using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoseidonFA.Models
{
    public class Telemetry
    {
        public int Id { get; set; }
        public DateTimeOffset DateTime { get; set; }
        [ForeignKey(nameof(PoolId))]
        public Pool Pool { get; set; }
        public TelemetryType Type { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }

        public int PoolId { get; set; }
    }
}
