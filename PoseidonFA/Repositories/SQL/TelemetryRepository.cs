using PoseidonFA.Configuration;
using PoseidonFA.Models;
using System;

namespace PoseidonFA.Repositories.SQL
{
    public class TelemetryRepository : ITelemetryRepository
    {
        private readonly PoseidonContext context;

        public TelemetryRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public void Add(Telemetry telemetry)
        {
            context.Telemetries.Add(telemetry);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
