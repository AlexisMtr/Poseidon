using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Models
{
    public class Pool
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Measure> Measures { get; set; }
        public Location Location { get; set; }
    }
}
