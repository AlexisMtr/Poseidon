using System;

namespace Poseidon.Dtos
{
    public class AlarmDto
    {
        public int PoolId { get; set; }
        public string Description { get; set; }
        public string AlarmType { get; set; }
        public bool IsAck { get; set; }
        public DateTimeOffset OccuredAt { get; set; }
    }
}
