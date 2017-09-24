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

namespace PoseidonFA
{
    public static class PostMeasuresFunction
    {
        public static IContainer Container { get; set; }

        private static IContainer BuildDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MongoDbContext>();
            builder.RegisterType<MongoDbAlarmsRepository>().As<IRepository<Alarm>>();
            builder.RegisterType<MongoDbMeasuresRepository>().As<IRepository<Measure>>();
            builder.RegisterType<MongoDbPoolConfiguartionsRespository>().As<IConfigurationRepository<PoolConfiguration>>();

            var settings = new PoseidonSettings
            {
                BatteryLevelAlarm = int.Parse(ConfigurationManager.AppSettings["BatteryLevelAlarm"])
            };
            builder.RegisterInstance(settings);

            return builder.Build();
        }

        [FunctionName("PostMeasures")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");


            Container = BuildDIContainer();

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;

            // Get request body
            var payload = await req.Content.ReadAsAsync<string>();
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<IncomingMeasures>(payload);


            // Set name to query string or body data

            return name == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);
        }
    }
}
