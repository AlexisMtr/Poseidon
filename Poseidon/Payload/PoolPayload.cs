using Poseidon.Models;
using System.Collections.Generic;

namespace Poseidon.Payload
{
    public class PoolPayload
    {
        public string Name { get; set; }
        public IEnumerable<string> Users { get; set; }
        public Location Location { get; set; }
    }
}
