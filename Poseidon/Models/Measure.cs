using MongoDB.Bson.Serialization.Attributes;

namespace Poseidon.Models
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

        public override bool Equals(object obj)
        {
            if (!(obj is Measure))
                return false;

            return (obj as Measure).Id.Equals(this.Id) && (obj as Measure).PoolId.Equals(this.PoolId);
        }
    }
}
