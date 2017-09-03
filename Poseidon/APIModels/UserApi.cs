using System.Collections.Generic;

namespace Poseidon.APIModels
{
    public class UserApi
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> PoolsId { get; set; }
    }
}
