using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;

namespace Poseidon.Configuration.MapperProfiles
{
    public class AlarmProfile : Profile
    {
        public AlarmProfile()
        {
            CreateMap<AlarmProfile, AlarmDto>();
            CreateMap<PaginatedElement<Alarm>, PaginatedDto<AlarmDto>>();
        }
    }
}
