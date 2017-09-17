using System.Collections.Generic;

namespace Poseidon.Payload
{
    public class UserPayload
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> PoolsId { get; set; }
    }
}
