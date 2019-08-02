using PoseidonFA.Helpers;
using PoseidonFA.Models;
using PoseidonFA.Dtos;
using System.Collections.Generic;
using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace PoseidonFA.Services
{
    public class ProcessDataService
    {
        private readonly PoolService poolService;
        private readonly AlarmService alarmService;
        private readonly TelemetryService telemetryService;
        private readonly ILogger log;
        private readonly double batteryLevelAlarm;

        public ProcessDataService(PoolService poolService, AlarmService alarmService, TelemetryService telemetryService, ILogger log)
        {
            this.poolService = poolService;
            this.alarmService = alarmService;
            this.telemetryService = telemetryService;
            this.log = log;
            this.batteryLevelAlarm = double.Parse(Environment.GetEnvironmentVariable("BatteryLevelAlarm"));
        }

        public void Process(string deviceId, TelemetriesSetDto data)
        {
            if (!data.Telemetries.Any()) return;

            Pool pool = poolService.GetByDeviceId(deviceId);
            if (pool == null)
            {
                log.LogError($"No pool associated to device {deviceId}");
                throw new Exception($"Pool not found");
            }

            IEnumerable<Telemetry> telemetries = Mapper.Map<IEnumerable<Telemetry>>(data.Telemetries);
            foreach(Telemetry telemetry in telemetries)
            {
                ProcessTelemetry(telemetry, pool);
            }
        }

        private void ProcessTelemetry(Telemetry telemetry, Pool pool)
        {
            double minValue = 0.00;
            double maxValue = 0.00;
            AlarmType alarmType;

            switch(telemetry.Type)
            {
                case TelemetryType.Level:
                    minValue = pool.WaterLevelMinValue;
                    maxValue = pool.WaterLevelMaxValue;
                    alarmType = AlarmType.WaterLevel;
                    break;
                case TelemetryType.Battery:
                    minValue = batteryLevelAlarm;
                    maxValue = 100;
                    alarmType = AlarmType.BatteryLow;
                    break;
                case TelemetryType.Ph:
                    minValue = pool.PhMinValue;
                    maxValue = pool.PhMaxValue;
                    alarmType = AlarmType.Ph;
                    break;
                case TelemetryType.Temperature:
                    minValue = pool.TemperatureMinValue;
                    maxValue = pool.TemperatureMaxValue;
                    alarmType = AlarmType.Temperature;
                    break;
                case TelemetryType.Other:
                    alarmType = AlarmType.DeviceWarning;
                    break;
                default:
                    return;
            }

            CheckForAlarm(pool, telemetry, minValue, maxValue, alarmType);

            telemetry.Pool = pool;
            telemetry.DateTime = DateTimeOffset.UtcNow;
            telemetryService.Add(telemetry);
        }

        private void CheckForAlarm(Pool pool, Telemetry telemetry, double minValue, double maxValue, AlarmType type)
        {
            // Check if the telemetry value is under respectable values
            if (telemetry.Value is double value && value.IsBetween(minValue, maxValue, false)) return;

            alarmService.Add(new Alarm
            {
                Pool = pool,
                DateTime = DateTimeOffset.UtcNow,
                Description = $"Generated on {DateTime.UtcNow} by Poseidon",
                AlarmType = type,
                Ack = false
            });
        }
    }
}
