using System.Collections.Generic;

namespace Poseidon.Models
{
    public class Pool
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> UsersId { get; set; }
        public Location Location { get; set; }
        public IEnumerable<Measure> Measures { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
    }
}
