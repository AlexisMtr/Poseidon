namespace Poseidon.Dtos
{
    public class PoolCreationDto
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double TemperatureMinValue { get; set; }
        public double TemperatureMaxValue { get; set; }
        public double PhMinValue { get; set; }
        public double PhMaxValue { get; set; }
        public double WaterLevelMinValue { get; set; }
        public double WaterLevelMaxValue { get; set; }
    }
}
