using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    public class Season
    {
        [BsonId]
        private ObjectId ObjectId { get; set; }

        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public string PoolId { get; set; }
        [BsonElement]
        public long SeasonStart { get; set; }
        [BsonElement]
        public long? SeasonEnd { get; set; }
    }
}
