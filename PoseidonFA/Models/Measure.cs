using MongoDB.Bson.Serialization.Attributes;

namespace PoseidonFA.Models
{
    [BsonIgnoreExtraElements]
    [BsonNoId]
    public class Measure
    {
        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public string PoolId { get; set; }
        [BsonElement]
        public long Timestamp { get; set; }
        [BsonElement]
        public MeasureType MeasureType { get; set; }
        [BsonElement]
        public object Value { get; set; }
        [BsonElement]
        public string Unit { get; set; }
    }
}
