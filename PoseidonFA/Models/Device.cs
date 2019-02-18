namespace PoseidonFA.Models
{
    public class Device
    {
        public string DeviceId { get; set; }
        public string Version { get; set; }
        public DeviceConfiguration Configuration { get; set; }
    }
}