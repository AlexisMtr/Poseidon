using MongoDB.Bson.Serialization.Attributes;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
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
        public long? AcknowledgmentTimestamp { get; set; }
        [BsonElement]
        public AlarmType AlarmType { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Alarm))
                return false;

            return (obj as Alarm).Id.Equals(this.Id) && (obj as Alarm).PoolId.Equals(this.PoolId);
        }
    }
}
