using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Models
{
    [BsonIgnoreExtraElements]
    public class Location
    {
        [BsonElement]
        public double Latitude { get; set; }
        [BsonElement]
        public double Longitude { get; set; }
    }
}
