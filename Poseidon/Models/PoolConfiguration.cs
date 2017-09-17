using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    public class PoolConfiguration
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId ObjectId { get; set; }

        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public string PoolId { get; set; }
        [BsonElement]
        public double TemperatureMinValue { get; set; }
        [BsonElement]
        public double TemperatureMaxValue { get; set; }
        [BsonElement]
        public double PhMinValue { get; set; }
        [BsonElement]
        public double PhMaxValue { get; set; }
        [BsonElement]
        public double WaterLevelMinValue { get; set; }
        [BsonElement]
        public double WaterLevelMaxValue { get; set; }
    }
}
