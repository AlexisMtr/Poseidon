using System.Collections.Generic;

namespace Poseidon.Models
{
    public class Pool
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double TemperatureMinValue { get; set; }
        public double TemperatureMaxValue { get; set; }
        public double PhMinValue { get; set; }
        public double PhMaxValue { get; set; }
        public double WaterLevelMinValue { get; set; }
        public double WaterLevelMaxValue { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public IEnumerable<Telemetry> Measures { get; set; }
        public IEnumerable<Alarm> Alarms { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
