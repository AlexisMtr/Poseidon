using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;

namespace Poseidon.Configuration.MapperProfiles
{
    public class TelemetryProfile : Profile
    {
        public TelemetryProfile()
        {
            CreateMap<Telemetry, TelemetryDto>();
            CreateMap<PaginatedElement<Telemetry>, PaginatedDto<TelemetryDto>>();
        }
    }
}
