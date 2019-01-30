﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Pool>()
                .HasIndex("DeviceId")
                .IsUnique();

            builder.Entity<DeviceConfiguration>()
                .Property(e => e.ConfigurationUpdateCheckDelay)
                .HasConversion<TimeSpanToTicksConverter>();
            builder.Entity<DeviceConfiguration>()
                .Property(e => e.PublicationDelay)
                .HasConversion<TimeSpanToTicksConverter>();
            
            base.OnModelCreating(builder);
        }
        
        public DbSet<Pool> Pools { get; set; }
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<Telemetry> Telemetries { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<UserPoolAssociation> UserPoolAssociations { get; set; }
    }
}
