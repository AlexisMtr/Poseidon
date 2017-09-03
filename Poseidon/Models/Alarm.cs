using MongoDB.Bson.Serialization.Attributes;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    public class Alarm
    {
        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public string PoolId { get; set; }
        [BsonElement]
        public long Timestamp { get; set; }
        [BsonElement]
        public string Description { get; set; }
        [BsonElement]
        public bool Ack { get; set; }
        [BsonElement]
        public long AcknowledgmentTimestamp { get; set; }
        [BsonElement]
        public AlarmType AlarmType { get; set; }
    }
}
