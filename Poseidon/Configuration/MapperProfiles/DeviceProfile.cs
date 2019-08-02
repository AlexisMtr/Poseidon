using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Models;
using System.Collections.Generic;

namespace Poseidon.Configuration.MapperProfiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceConfigurationDto, DeviceConfiguration>()
                .ForMember(d => d.IsPublished, opt => opt.UseValue(false));
            CreateMap<Device, string>()
                .ConvertUsing(e => e.DeviceId);

            CreateMap<DeviceConfiguration, DeviceConfigurationDto>();
        }
    }
}
