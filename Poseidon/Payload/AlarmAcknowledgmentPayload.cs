namespace Poseidon.Payload
{
    public class AlarmAcknowledgmentPayload
    {
        public string PoolId { get; set; }
        public string AlarmId { get; set; }
        public long AlarmTimestamp { get; set; }
        public long AlarmAcknowledgmentTimestamp { get; set; }
    }
}
