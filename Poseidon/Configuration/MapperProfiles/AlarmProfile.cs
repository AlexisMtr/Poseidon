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
            CreateMap<PaginatedElement<Alarm>, PaginatedDto<AlarmDto>>()
                .ForMember(d => d.NextPageUrl, opt =>
                {
                    opt.PreCondition(e => e.PageCount > 1);
                    opt.ResolveUsing<RestApiResolver, string>(e => opt.DestinationMember.Name);
                })
                .ForMember(d => d.PreviousPageUrl, opt =>
                 {
                     opt.PreCondition(e => e.PageCount > 1);
                     opt.ResolveUsing<RestApiResolver, string>(e => opt.DestinationMember.Name);
                 });
        }
    }
}
