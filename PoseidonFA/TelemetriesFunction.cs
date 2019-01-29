using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using PoseidonFA.Dtos;
using PoseidonFA.Services;
using PoseidonFA.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Newtonsoft.Json;
using System;

namespace PoseidonFA
{
    public static class TelemetriesFunction
    {
        [FunctionName("Telemetries")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            MapperConfiguration.ConfigureMapper();
            DependencyInjection.InitializeContainer(log);

            ProcessDataService service = DependencyInjection.ServiceProvider.GetService<ProcessDataService>();

            string poolId = req.Query
                .FirstOrDefault(q => string.Compare(q.Key, "poolid", true) == 0).Value;

            if (string.IsNullOrEmpty(poolId)) return new BadRequestObjectResult(new
            {
                Message = $"Parameter {nameof(poolId)} must be define"
            });

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TelemetriesSetDto payload = JsonConvert.DeserializeObject<TelemetriesSetDto>(requestBody);

            try
            {
                service.Process(int.Parse(poolId), payload);
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new
                {
                    Message = "Error while processing telemetries",
                    InnerMessage = e.Message
                });
            }
        }
    }
}
