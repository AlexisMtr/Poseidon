using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.APIModels
{
    public class PoolApi
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public long LastUpdate { get; set; }
    }
}
