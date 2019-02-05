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
using System.Net;
using PoseidonFA.Models;

namespace PoseidonFA
{
    public static class TelemetriesFunction
    {
        [FunctionName("Telemetries")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Telemetries/{deviceId}")]HttpRequest req,
            string deviceId,
            ILogger log)
        {
            ProcessDataService service = null;
            DeviceConfigurationService deviceConfigurationService = null;
            try
            {
                MapperConfiguration.ConfigureMapper();
                DependencyInjection.InitializeContainer(log);

                service = DependencyInjection.ServiceProvider.GetService<ProcessDataService>();
                deviceConfigurationService = DependencyInjection.ServiceProvider.GetService<DeviceConfigurationService>();
            }
            catch (Exception e)
            {
                log.LogError(e.Message, e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TelemetriesSetDto payload = JsonConvert.DeserializeObject<TelemetriesSetDto>(requestBody);

            try
            {
                service.Process(deviceId, payload);
                DeviceConfiguration configuration = deviceConfigurationService.GetDeviceConfiguration(deviceId);

                IActionResult result  = configuration.IsPublished ? 
                    new StatusCodeResult((int)HttpStatusCode.NotModified) as IActionResult :
                    new OkObjectResult(new { configuration.PublicationDelay }) as IActionResult;

                if(!deviceConfigurationService.SetAsPublished(configuration))
                {
                    log.LogWarning($"DeviceConfiguration {configuration.Id} is published but stay as 'unpublished' in the database");
                }

                return result;
            }
            catch (Exception e)
            {
                log.LogError(e.Message, e);
                return new BadRequestObjectResult(new
                {
                    Message = "Error while processing telemetries",
                    InnerMessage = e.Message
                });
            }
        }
    }
}
