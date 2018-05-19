using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using PoseidonFA.Dtos;
using Autofac;
using PoseidonFA.Services;
using PoseidonFA.Configuration;

namespace PoseidonFA
{
    public static class PostMeasuresFunction
    {
        [FunctionName("PostMeasures")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            DependencyInjection.InitializeContainer(new TraceWriterWrapper(log));
            using (var scope = DependencyInjection.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<ProcessDataService>();

                string poolId = req.GetQueryNameValuePairs()
                    .FirstOrDefault(q => string.Compare(q.Key, "poolid", true) == 0).Value;

                TelemetriesSetDto payload = await req.Content.ReadAsAsync<TelemetriesSetDto>();

                service.Process(int.Parse(poolId), payload);
                return req.CreateErrorResponse(HttpStatusCode.OK, "");
            }

        }
    }
}
