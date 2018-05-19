using System;

namespace PoseidonFA.Models
{
    public class Alarm
    {
        public string Id { get; set; }
        public Pool Pool { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public bool Ack { get; set; }
        public DateTime? AcknowledgmentDateTime { get; set; }
        public AlarmType AlarmType { get; set; }
    }
}
