using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;

namespace Poseidon.Configuration.MapperProfiles
{
    public class PoolProfile : Profile
    {
        public PoolProfile()
        {
            CreateMap<Pool, PoolDto>();
            CreateMap<PaginatedElement<Pool>, PaginatedDto<PoolDto>>();
        }
    }
}
