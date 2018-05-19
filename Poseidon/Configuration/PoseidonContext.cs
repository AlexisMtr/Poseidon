using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Poseidon.Models;

namespace Poseidon.Configuration
{
    public class PoseidonContext : IdentityDbContext<User, IdentityRole, string>
    {
        public PoseidonContext(DbContextOptions<PoseidonContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pool>()
                .HasIndex(e => e.DeviceId)
                .IsUnique();
            
            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Pool> Pools { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Telemetry> Telemetries { get; set; }
        public DbSet<Season> Seasons { get; set; }
    }
}
