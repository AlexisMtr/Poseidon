﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        private ObjectId ObjectId { get; set; }

        [BsonElement]
        public string Id { get; set; }
        [BsonElement]
        public string LastName { get; set; }
        [BsonElement]
        public string FirstName { get; set; }
        [BsonElement]
        public string Login { get; set; }
        [BsonElement]
        public string Password { get; set; }
        [BsonElement]
        public IEnumerable<string> PoolsId { get; set; }
        [BsonElement]
        public string Role { get; set; }
    }
}
