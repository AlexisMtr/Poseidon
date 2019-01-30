using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PoseidonFA.Repositories;
using PoseidonFA.Repositories.SQL;
using PoseidonFA.Services;
using System;
using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;

namespace PoseidonFA.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void InitializeContainer(ILogger logger)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            string dbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");
            serviceCollection.AddDbContext<PoseidonContext>(opt => opt.UseSqlServer(dbConnectionString));

            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();

            builder.RegisterType<PoolRepository>().As<IPoolRepository, PoolRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AlarmRepository>().As<IAlarmRepository, AlarmRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TelemetryRepository>().As<ITelemetryRepository, TelemetryRepository>().InstancePerLifetimeScope();

            builder.RegisterType<PoolService>().InstancePerLifetimeScope();
            builder.RegisterType<AlarmService>().InstancePerLifetimeScope();
            builder.RegisterType<TelemetryService>().InstancePerLifetimeScope();
            builder.RegisterType<ProcessDataService>().InstancePerLifetimeScope();

            IContainer container = builder.Build();
            ServiceProvider = new AutofacServiceProvider(container);
        }
    }
}
