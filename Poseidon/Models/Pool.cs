﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    public class Pool
    {
        [BsonId]
        [JsonIgnore]
        public ObjectId ObjectId { get; set; }

        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public IEnumerable<string> UsersId { get; set; }
        [BsonElement]
        public Location Location { get; set; }
        [BsonElement]
        public IEnumerable<Measure> Measures { get; set; }
        [BsonElement]
        public IEnumerable<Alarm> Alarms { get; set; }
    }
}
