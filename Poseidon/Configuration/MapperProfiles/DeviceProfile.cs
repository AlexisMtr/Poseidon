using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;

namespace Poseidon.Configuration.MapperProfiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceConfigurationDto, DeviceConfiguration>()
                .ForMember(d => d.IsPublished, opt => opt.UseValue(false));
        }
    }
}
