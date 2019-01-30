using Poseidon.Filters;
using System;

namespace Poseidon.Models
{
    public class Alarm : TimeObject
    {
        public int Id { get; set; }
        public Pool Pool { get; set; }
        public string Description { get; set; }
        public bool Ack { get; set; }
        public DateTimeOffset? AcknowledgmentDateTime { get; set; }
        public AlarmType AlarmType { get; set; }
    }
}
