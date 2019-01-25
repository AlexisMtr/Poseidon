using System;

namespace Poseidon.Dtos
{
    public class DeviceConfigurationDto
    {
        public string Version { get; set; }
        public DateTime SleepModeStart { get; set; }
        public TimeSpan PublicationDelay { get; set; }
        public TimeSpan ConfigurationUpdateCheckDelay { get; set; }
    }
}
