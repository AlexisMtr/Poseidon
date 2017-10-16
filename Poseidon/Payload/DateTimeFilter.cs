using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Payload
{
    public class DateTimeFilter
    {
        public long MinDateTimestamp { get; set; }
        public long MaxDateTimestamp { get; set; }
    }
}
