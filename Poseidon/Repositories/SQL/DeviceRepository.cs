using Poseidon.Configuration;
using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;
using System.Linq;

namespace Poseidon.Repositories.SQL
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly PoseidonContext context;

        public DeviceRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public void Add(Device entity)
        {
            context.Devices.Add(entity);
        }

        public int Count(IFilter<Device> filter)
        {
            return context.Devices.Count(filter ?? new DeviceFilter());
        }

        public void Delete(Device entity)
        {
            context.Devices.Remove(entity);
        }

        public IQueryable<Device> Get(IFilter<Device> filter)
        {
            return context.Devices
                .Where(filter ?? new DeviceFilter());
        }

        public Device GetById(string id, IFilter<Device> filter)
        {
            return context.Devices
                .Where(filter ?? new DeviceFilter())
                .FirstOrDefault(e => e.DeviceId.Equals(id));
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
