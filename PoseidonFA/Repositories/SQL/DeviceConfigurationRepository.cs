using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PoseidonFA.Configuration;
using PoseidonFA.Models;

namespace PoseidonFA.Repositories.SQL
{
    public class DeviceConfigurationRepository : IDeviceConfigurationRepository
    {
        private readonly PoseidonContext context;

        public DeviceConfigurationRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public DeviceConfiguration Get(int configurationId)
        {
            return context.DeviceConfiguration.FirstOrDefault(e => e.Id.Equals(configurationId));
        }

        public DeviceConfiguration GetByDevice(string deviceId)
        {
            return context.Devices
                .Include(e => e.Configuration)
                .FirstOrDefault(e => e.DeviceId.Equals(deviceId)).Configuration;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
