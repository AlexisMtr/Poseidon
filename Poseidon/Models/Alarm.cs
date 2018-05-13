using MongoDB.Bson.Serialization.Attributes;
using Poseidon.Filters;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class Alarm : TimeObject
    {
        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public Pool Pool { get; set; }
        [BsonElement]
        public long Timestamp { get; set; }
        [BsonElement]
        public string Description { get; set; }
        [BsonElement]
        public bool Ack { get; set; }
        [BsonElement]
        public long? AcknowledgmentTimestamp { get; set; }
        [BsonElement]
        public AlarmType AlarmType { get; set; }
    }
}
