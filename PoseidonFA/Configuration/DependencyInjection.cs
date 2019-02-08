using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PoseidonFA.Repositories;
using PoseidonFA.Repositories.SQL;
using PoseidonFA.Services;
using System;

namespace PoseidonFA.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void InitializeContainer(ILogger logger)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            serviceCollection.AddSingleton(typeof(ILogger), logger);

            string dbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");
            serviceCollection.AddDbContext<PoseidonContext>(opt => opt.UseSqlServer(dbConnectionString));

            serviceCollection.AddScoped<IPoolRepository, PoolRepository>();
            serviceCollection.AddScoped<IAlarmRepository, AlarmRepository>();
            serviceCollection.AddScoped<ITelemetryRepository, TelemetryRepository>();
            serviceCollection.AddScoped<IDeviceConfigurationRepository, DeviceConfigurationRepository>();

            serviceCollection.AddScoped<PoolService>();
            serviceCollection.AddScoped<AlarmService>();
            serviceCollection.AddScoped<TelemetryService>();
            serviceCollection.AddScoped<DeviceConfigurationService>();
            serviceCollection.AddScoped<ProcessDataService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
