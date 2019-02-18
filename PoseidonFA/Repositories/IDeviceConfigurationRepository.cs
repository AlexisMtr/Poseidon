using PoseidonFA.Models;

namespace PoseidonFA.Repositories
{
    public interface IDeviceConfigurationRepository
    {
        DeviceConfiguration GetByDevice(string deviceId);
        void SaveChanges();
    }
}
