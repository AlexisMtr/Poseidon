using Newtonsoft.Json;
using System;

namespace Poseidon.Dtos
{
    public class AlarmDto
    {
        public int Id { get; set; }
        public int PoolId { get; set; }
        public string Description { get; set; }
        public string AlarmType { get; set; }
        public bool IsAck { get; set; }
        public DateTimeOffset OccuredAt { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AcknowledgmentUri { get; set; }
    }
}
