using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Poseidon.Dtos;
using Poseidon.Exceptions;
using Poseidon.Filters;
using Poseidon.Models;
using Poseidon.Repositories;

namespace Poseidon.Services
{
    public class DeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IMapper mapper;
        private readonly string defaultVersion;

        public DeviceService(IDeviceRepository deviceRepository, IMapper mapper)
        {
            this.deviceRepository = deviceRepository;
            this.mapper = mapper;
            this.defaultVersion = "0";
        }

        public IEnumerable<Device> Get(IFilter<Device> filter)
        {
            return deviceRepository.Get(filter).AsEnumerable();
        }

        public Device Get(string id, User user = null)
        {
            IFilter<Device> filter = new DeviceFilter();
            if(user != null)
            {
                filter = new IdentityDeviceFilter(filter, user);
            }
            Device device = deviceRepository.GetById(id, filter);
            if (device == null) throw new NotFoundException(typeof(Device));

            return device;
        }

        public void UpdateDeviceConfiguration(string deviceId, DeviceConfigurationDto deviceConfiguration, User user)
        {
            Device device = Get(deviceId, user);
            
            device.Configuration = mapper.Map<DeviceConfiguration>(deviceConfiguration);
            deviceRepository.SaveChanges();
        }

        public Device CreateNewDevice(string version = null)
        {
            Device device = new Device
            {
                DeviceId = Guid.NewGuid().ToString(),
                Version = string.IsNullOrEmpty(version) ? defaultVersion : version,
                Configuration = new DeviceConfiguration
                {
                    IsPublished = false,
                    Version = $"{DateTime.Now.Year}.0",
                    SleepModeStart = DateTime.Now,
                    PublicationDelay = TimeSpan.FromMinutes(15),
                    ConfigurationUpdateCheckDelay = TimeSpan.FromDays(1)
                }
            };
            deviceRepository.Add(device);
            deviceRepository.SaveChanges();

            return device;
        }
    }
}
