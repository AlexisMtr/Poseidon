using PoseidonFA.Models;

namespace PoseidonFA.Repositories
{
    public interface ITelemetryRepository
    {
        void Add(Telemetry telemetry);
        void SaveChanges();
    }
}
