using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;
using System.Collections.Generic;
using System.Linq;

namespace Poseidon.Configuration.MapperProfiles
{
    public class PoolProfile : Profile
    {
        public PoolProfile()
        {
            CreateMap<Pool, PoolDto>()
                .ForMember(d => d.HasAlarm, opt => opt.MapFrom(s => s.Alarms.Any(e => !e.Ack)))
                .ForMember(d => d.AlarmCount, opt => opt.MapFrom(s => s.Alarms.Count(e => !e.Ack)))
                .ForMember(d => d.LastBatteryLevel, opt => opt.MapFrom(s => s.Telemetries.OrderBy(e => e.DateTime).LastOrDefault(e => e.Type == TelemetryType.Battery).Value))
                .ForMember(d => d.LastWaterLevel, opt => opt.MapFrom(s => s.Telemetries.OrderBy(e => e.DateTime).LastOrDefault(e => e.Type == TelemetryType.Level).Value))
                .ForMember(d => d.LastPh, opt => opt.MapFrom(s => s.Telemetries.OrderBy(e => e.DateTime).LastOrDefault(e => e.Type == TelemetryType.Ph).Value))
                .ForMember(d => d.LastTemperature, opt => opt.MapFrom(s => s.Telemetries.OrderBy(e => e.DateTime).LastOrDefault(e => e.Type == TelemetryType.Temperature).Value));

            CreateMap<PaginatedElement<Pool>, PaginatedDto<PoolDto>>();

            CreateMap<PoolCreationDto, Pool>()
                .ForMember(d => d.Users, opt => opt.UseValue(new List<UserPoolAssociation>()));
        }
    }
}
