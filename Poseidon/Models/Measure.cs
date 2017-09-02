using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Models
{
    public class Measure
    {
        public string Id { get; set; }
        public long Timestamp { get; set; }
        public DateTime Date { get; set; }
        public MeasureType Type { get; set; }
        public object Value { get; set; }
    }
}
