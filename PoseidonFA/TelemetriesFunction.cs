using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using PoseidonFA.Configuration;
using PoseidonFA.Dtos;
using PoseidonFA.Models;
using PoseidonFA.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PoseidonFA
{
    public static class TelemetriesFunction
    {
        [FunctionName("Telemetries")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Telemetries/{deviceId}")]HttpRequest req,
            string deviceId,
            ILogger log)
        {
            ProcessDataService service;
            DeviceConfigurationService deviceConfigurationService;
            try
            {
                MapperConfiguration.ConfigureMapper();
                DependencyInjection.InitializeContainer(log);

                service = DependencyInjection.ServiceProvider.GetRequiredService<ProcessDataService>();
                deviceConfigurationService = DependencyInjection.ServiceProvider.GetRequiredService<DeviceConfigurationService>();
            }
            catch (Exception e)
            {
                log.LogError(e.Message, e);
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }

            string requestBody = string.Empty;
            using (var sr = new StreamReader(req.Body))
            {
                requestBody = await sr.ReadToEndAsync();
            }
            TelemetriesSetDto payload = JsonConvert.DeserializeObject<TelemetriesSetDto>(requestBody);

            try
            {
                service.Process(deviceId, payload);
                DeviceConfiguration configuration = deviceConfigurationService.GetDeviceConfiguration(deviceId);

                IActionResult result  = configuration.IsPublished && !req.Query.ContainsKey("getConfiguration") ? 
                    new StatusCodeResult((int)HttpStatusCode.NotModified) as IActionResult :
                    new OkObjectResult(new { configuration.PublicationDelay }) as IActionResult;

                if(!deviceConfigurationService.SetAsPublished(configuration))
                {
                    log.LogWarning($"DeviceConfiguration {configuration.Id} is published but stay as 'unpublished' in the database");
                }

                log.LogInformation($"{DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm")} - Telemetries updated for device {deviceId}");

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
