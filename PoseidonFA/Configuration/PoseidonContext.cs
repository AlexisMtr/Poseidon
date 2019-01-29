using Microsoft.EntityFrameworkCore;
using PoseidonFA.Models;

namespace PoseidonFA.Configuration
{
    public class PoseidonContext : DbContext
    {
        public PoseidonContext(DbContextOptions<PoseidonContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Telemetry> Telemetries { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Pool> Pools { get; set; }
    }
}
