using Poseidon.Models;

namespace Poseidon.Payload
{
    public class AlarmPayload
    {
        public string Description { get; set; }
        public string PoolId { get; set; }
        public AlarmType AlarmType { get; set; }
    }
}
