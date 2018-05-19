using PoseidonFA.Models;
using System.Data.Entity;

namespace PoseidonFA.Configuration
{
    public class PoseidonContext : DbContext
    {
        public PoseidonContext()
            : base("name=DefaultConnection")
        {

        }

        public DbSet<Telemetry> Telemetries { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Pool> Pools { get; set; }
    }
}
