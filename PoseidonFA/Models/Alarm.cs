using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PoseidonFA.Models
{
    public class Alarm
    {
        public int Id { get; set; }
        [ForeignKey(nameof(PoolId))]
        public Pool Pool { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Description { get; set; }
        public bool Ack { get; set; }
        public DateTimeOffset? AcknowledgmentDateTime { get; set; }
        public AlarmType AlarmType { get; set; }

        public int PoolId { get; set; }
    }
}
