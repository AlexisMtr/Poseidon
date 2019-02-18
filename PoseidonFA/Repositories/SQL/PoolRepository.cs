using System.Linq;
using Microsoft.EntityFrameworkCore;
using PoseidonFA.Configuration;
using PoseidonFA.Models;

namespace PoseidonFA.Repositories.SQL
{
    public class PoolRepository : IPoolRepository
    {
        private readonly PoseidonContext context;

        public PoolRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public Pool Get(int id)
        {
            return context.Pools
                .Include(e => e.Device.Configuration)
                .FirstOrDefault(e => e.Id.Equals(id));
        }

        public Pool GetByDevice(string deviceId)
        {
            return context.Pools
                .Include(e => e.Device.Configuration)
                .FirstOrDefault(e => e.Device.DeviceId.Equals(deviceId));
        }
    }
}
