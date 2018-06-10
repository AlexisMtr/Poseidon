using Autofac;
using PoseidonFA.Repositories;
using PoseidonFA.Repositories.SQL;
using PoseidonFA.Services;

namespace PoseidonFA.Configuration
{
    public static class DependencyInjection
    {
        public static IContainer Container { get; set; }

        public static void InitializeContainer(ILogger logger)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterInstance(logger).As<ILogger>().SingleInstance();
            builder.RegisterType<PoseidonContext>().InstancePerLifetimeScope();

            builder.RegisterType<PoolRepository>().As<IPoolRepository, PoolRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AlarmRepository>().As<IAlarmRepository, AlarmRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TelemetryRepository>().As<ITelemetryRepository, TelemetryRepository>().InstancePerLifetimeScope();

            builder.RegisterType<PoolService>().InstancePerLifetimeScope();
            builder.RegisterType<AlarmService>().InstancePerLifetimeScope();
            builder.RegisterType<TelemetryService>().InstancePerLifetimeScope();
            builder.RegisterType<ProcessDataService>().InstancePerLifetimeScope();

            Container = builder.Build();
        }
    }
}
