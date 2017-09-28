using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Configuration;
using PoseidonFA.Payload;
using Autofac;
using PoseidonFA.Configuration;
using PoseidonFA.Repositories;
using PoseidonFA.Models;
using PoseidonFA.Services;
using System;

namespace PoseidonFA
{
    public static class PostMeasuresFunction
    {
        public static IContainer Container { get; set; }

        private static IContainer BuildDIContainer()
        {
            var builder = new ContainerBuilder();

            var settings = new PoseidonSettings
            {
                BatteryLevelAlarm = int.Parse(ConfigurationManager.AppSettings["BatteryLevelAlarm"])
            };
            var dbContext = new MongoDbContext(ConfigurationManager.AppSettings["DefaultConnectionString"],
                ConfigurationManager.AppSettings["DefaultDbName"]);

            builder.RegisterInstance(settings).SingleInstance();

            builder.Register(o => o.InjectProperties(dbContext)).InstancePerLifetimeScope();

            builder.RegisterType<MongoDbAlarmsRepository>().As<IRepository<Alarm>>().InstancePerLifetimeScope();
            builder.RegisterType<MongoDbMeasuresRepository>().As<IRepository<Measure>>().InstancePerLifetimeScope();
            builder.RegisterType<MongoDbPoolConfiguartionsRespository>().As<IConfigurationRepository<PoolConfiguration>>().InstancePerLifetimeScope();

            builder.RegisterType<ProcessDataService>().InstancePerLifetimeScope();


            return builder.Build();
        }

        [FunctionName("PostMeasures")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");


            Container = BuildDIContainer();

            using (var scope = Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<ProcessDataService>();

                try
                {
                    string poolId = req.GetQueryNameValuePairs()
                        .FirstOrDefault(q => string.Compare(q.Key, "poolid", true) == 0).Value;
                    
                    var payload = await req.Content.ReadAsAsync<IncomingMeasures>();

                    service.Process(poolId, payload);
                    return req.CreateErrorResponse(HttpStatusCode.OK, "");
                }
                catch(Exception e)
                {
                    log.Error("Error during processing", e);
                    return req.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                }
            }

        }
    }
}
