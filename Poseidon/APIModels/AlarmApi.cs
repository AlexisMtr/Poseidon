using Poseidon.Models;

namespace Poseidon.APIModels
{
    public class AlarmApi
    {
        public string Description { get; set; }
        public string PoolId { get; set; }
        public AlarmType AlarmType { get; set; }
    }
}
