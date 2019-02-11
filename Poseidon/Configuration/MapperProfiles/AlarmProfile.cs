using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;

namespace Poseidon.Configuration.MapperProfiles
{
    public class AlarmProfile : Profile
    {
        public AlarmProfile()
        {
            CreateMap<Alarm, AlarmDto>()
                .ForMember(d => d.IsAck, opt => opt.MapFrom(s => s.Ack))
                .ForMember(d => d.OccuredAt, opt => opt.MapFrom(s => s.DateTime))
                .ForMember(d => d.AlarmType, opt => opt.MapFrom(s => s.AlarmType.ToString()))
                .ForMember(d => d.AcknowledgmentUri, opt => opt.ResolveUsing<RestApiResolver>());

            this.CreatePaginatedMap<Alarm, AlarmDto>();
        }
    }
}
