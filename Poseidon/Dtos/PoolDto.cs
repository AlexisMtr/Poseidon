namespace Poseidon.Dtos
{
    public class PoolDto
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
        public string DeviceId { get; set; }

        public bool HasAlarm { get; set; }
        public int AlarmCount { get; set; }

        public double LastTemperature { get; set; }
        public double LastPh { get; set; }
        public double LastWaterLevel { get; set; }
        public double LastBatteryLevel { get; set; }
    }
}
