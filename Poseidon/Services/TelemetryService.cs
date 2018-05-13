using Poseidon.Repositories;

namespace Poseidon.Services
{
    public class TelemetryService
    {
        private readonly ITelemetryRepository measuresRepository;

        public TelemetryService(ITelemetryRepository measuresRepository)
        {
            this.measuresRepository = measuresRepository;
        }
    }
}
