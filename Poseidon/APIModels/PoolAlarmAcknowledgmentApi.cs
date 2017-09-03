using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.APIModels
{
    public class PoolAlarmAcknowledgmentApi
    {
        public string PoolId { get; set; }
        public string AlarmId { get; set; }
        public long AlarmTimestamp { get; set; }
        public long AlarmAcknowledgmentTimestamp { get; set; }
    }
}
