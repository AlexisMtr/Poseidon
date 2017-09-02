using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Models
{
    public class Alarm
    {
        public string Id { get; set; }
        public string PoolId { get; set; }
        public long Timestapm { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
