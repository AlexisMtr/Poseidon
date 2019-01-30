using System;

namespace Poseidon.Models
{
    public  class DeviceConfiguration
    {
        public int Id { get; set; }
        public bool IsPublished { get; set; }
        public string Version { get; set; }
        public DateTimeOffset SleepModeStart { get; set; }
        public TimeSpan PublicationDelay { get; set; }
        public TimeSpan ConfigurationUpdateCheckDelay { get; set; }
    }
}