using PoseidonFA.Configuration;
using PoseidonFA.Helpers;
using PoseidonFA.Models;
using PoseidonFA.Payload;
using PoseidonFA.Repositories;
using System;

namespace PoseidonFA.Services
{
    public class ProcessDataService
    {
        private readonly IRepository<Measure> MeasuresRepository;
        private readonly IRepository<Alarm> AlarmsRepository;
        private readonly IConfigurationRepository<PoolConfiguration> ConfigurationRepository;
        
        public readonly PoseidonSettings PoseidonSettings;

        public ProcessDataService(IRepository<Measure> measureRepository, IRepository<Alarm> alarmRepository, 
            IConfigurationRepository<PoolConfiguration> configurationRepository, PoseidonSettings poseidonSettings)
        {
            this.MeasuresRepository = measureRepository;
            this.AlarmsRepository = alarmRepository;
            this.ConfigurationRepository = configurationRepository;
            this.PoseidonSettings = poseidonSettings;
        }

        public void Process(string poolId, IncomingMeasures data)
        {
            if (string.IsNullOrEmpty(poolId))
                throw new ArgumentNullException($"Argument {nameof(poolId)} cannot be null");

            var configuration = this.ConfigurationRepository.GetByPoolId(poolId);

            if(IsNumeric(data.Ph.Value, out double ph))
            {
                if(!ph.IsBetween(configuration.PhMinValue, configuration.PhMaxValue))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Ph.Timestamp,
                        AlarmType = AlarmType.Ph,
                        Description = (ph > configuration.PhMaxValue ? "Solution Saline" : "Solution Acide") + $" (ph: {ph})"
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

            if(IsNumeric(data.Level.Value, out double level))
            {
                if(!level.IsBetween(configuration.WaterLevelMinValue, configuration.WaterLevelMaxValue))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Level.Timestamp,
                        AlarmType = AlarmType.WaterLevel,
                        Description = level > configuration.WaterLevelMaxValue ? "Débordement" : "Niveau d'eau trop faible, arrêter le système de pompage"
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

            if (IsNumeric(data.Temperature.Value, out double temperature))
            {
                if (!temperature.IsBetween(configuration.TemperatureMinValue, configuration.TemperatureMaxValue))
                {
                    this.AlarmsRepository.Add(new Alarm
                    {
                        PoolId = poolId,
                        Timestamp = data.Temperature.Timestamp,
                        AlarmType = AlarmType.Temperature,
                        Description = temperature > configuration.TemperatureMaxValue ? "La température de l'eau a dépassée le seuil maximal" : "Température trop basse pour se baigner"
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

            if (IsNumeric(data.Battery.Value, out double battery))
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

        private bool IsNumeric(object value, out double valueAsDouble)
        {
            if(value is double d)
            {
                valueAsDouble = d;
                return true;
            }
            if(value is int i)
            {
                valueAsDouble = i;
                return true;
            }
            if (value is long l)
            {
                valueAsDouble = l;
                return true;
            }

            valueAsDouble = 0.00;
            return false;
        }
    }
}
