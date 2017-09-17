using PoseidonFA.Configuration;
using PoseidonFA.Helpers;
using PoseidonFA.Models;
using PoseidonFA.Payload;
using PoseidonFA.Repositories;

namespace PoseidonFA.Services
{
    public class ProcessDataService
    {
        private readonly IRepository<Measure> MeasuresRepository;
        private readonly IRepository<Alarm> AlarmsRepository;

        public readonly PoolConfiguration Configuration;
        public readonly PoseidonSettings PoseidonSettings;

        public ProcessDataService(IRepository<Measure> measureRepository, IRepository<Alarm> alarmRepository, PoolConfiguration configuration, PoseidonSettings poseidonSettings)
        {
            this.MeasuresRepository = measureRepository;
            this.AlarmsRepository = alarmRepository;
            this.Configuration = configuration;
            this.PoseidonSettings = poseidonSettings;
        }

        public void Process(string poolId, IncomingMeasures data)
        {
            if(data.Ph.Value is double ph)
            {
                if(!ph.IsBetween(Configuration.PhMinValue, Configuration.PhMaxValue))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Ph.Timestamp,
                        AlarmType = AlarmType.Ph,
                        Description = (ph > Configuration.PhMaxValue ? "Solution Saline" : "Solution Acide") + $" (ph: {ph})"
                    });
                }

                this.MeasuresRepository.Add(new Measure
                {
                    Id = $"M{data.Ph.Timestamp}{(int)MeasureType.Ph}",
                    Timestamp = data.Ph.Timestamp,
                    MeasureType = MeasureType.Ph,
                    PoolId = poolId,
                    Value = ph,
                    Unit = ""
                });
            }

            if(data.Level.Value is double level)
            {
                if(!level.IsBetween(Configuration.WaterLevelMinValue, Configuration.WaterLevelMaxValue))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Level.Timestamp,
                        AlarmType = AlarmType.WaterLevel,
                        Description = level > Configuration.WaterLevelMaxValue ? "Débordement" : "Niveau d'eau trop faible, arrêter le système de pompage"
                    });
                }

                this.MeasuresRepository.Add(new Measure
                {
                    Id = $"M{data.Level.Timestamp}{(int)MeasureType.Level}",
                    Timestamp = data.Level.Timestamp,
                    MeasureType = MeasureType.Level,
                    PoolId = poolId,
                    Value = level,
                    Unit = "m3"
                });
            }

            if (data.Temperature.Value is double temperature)
            {
                if (!temperature.IsBetween(Configuration.TemperatureMinValue, Configuration.TemperatureMaxValue))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Temperature.Timestamp,
                        AlarmType = AlarmType.Temperature,
                        Description = temperature > Configuration.TemperatureMaxValue ? "La température de l'eau a dépassée le seuil maximal" : "Température trop basse pour se baigner"
                    });
                }

                this.MeasuresRepository.Add(new Measure
                {
                    Id = $"M{data.Temperature.Timestamp}{(int)MeasureType.Temperature}",
                    Timestamp = data.Temperature.Timestamp,
                    MeasureType = MeasureType.Temperature,
                    PoolId = poolId,
                    Value = temperature,
                    Unit = "°C"
                });
            }

            if (data.Battery.Value is int battery)
            {
                if (battery.IsBetween(0, PoseidonSettings.BatteryLevelAlarm))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Temperature.Timestamp,
                        AlarmType = AlarmType.BatteryLow,
                        Description = $"Niveau de batterie faible ({battery}%)"
                    });
                }

                this.MeasuresRepository.Add(new Measure
                {
                    Id = $"M{data.Battery.Timestamp}{(int)MeasureType.Battery}",
                    Timestamp = data.Battery.Timestamp,
                    MeasureType = MeasureType.Battery,
                    PoolId = poolId,
                    Value = battery,
                    Unit = "%"
                });
            }
        }
    }
}
