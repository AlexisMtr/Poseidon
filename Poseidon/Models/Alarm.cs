namespace Poseidon.Models
{
    public class Alarm
    {
        public string Id { get; set; }
        public string PoolId { get; set; }
        public long Timestamp { get; set; }
        public string Description { get; set; }
        public bool Ack { get; set; }
        public long AcknowledgmentTimestamp { get; set; }
        public AlarmType AlarmType { get; set; }
    }
}
