namespace Poseidon.Models
{
    public class Device
    {
        public string DeviceId { get; set; }
        public string Version { get; set; }
        public DeviceConfiguration Configuration { get; set; }

        public virtual Pool Pool { get; set; }
    }
}
