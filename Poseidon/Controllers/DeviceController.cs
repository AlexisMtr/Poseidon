using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Dtos;
using Poseidon.Filters;
using Poseidon.Models;
using Poseidon.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DeviceController : Controller
    {
        private readonly DeviceService deviceService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public DeviceController(DeviceService deviceService, IMapper mapper, UserManager<User> userManager)
        {
            this.deviceService = deviceService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = Roles.SysAdmin)]
        public IActionResult Get([FromQuery]DeviceFilter filter)
        {
            IEnumerable<Device> devices = deviceService.Get(filter);
            return Ok(mapper.Map<IEnumerable<string>>(devices));
        }

        [HttpGet("{deviceId}/configuration")]
        public async Task<IActionResult> GetConfiguration(string deviceId)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            Device device = deviceService.Get(deviceId, user);
            return Ok(mapper.Map<DeviceConfigurationDto>(device.Configuration));
        }

        [HttpPost]
        [Authorize(Roles = Roles.SysAdmin)]
        public IActionResult Post(string version = null)
        {
            Device device = deviceService.CreateNewDevice(version);
            return Ok(new { device.DeviceId });
        }

        [HttpPut("{deviceId}")]
        [Authorize(Roles = Roles.SysAdmin)]
        public async Task<IActionResult> Put(string deviceId, [FromBody]DeviceConfigurationDto deviceConfiguration)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            deviceService.UpdateDeviceConfiguration(deviceId, deviceConfiguration, user);
            return Ok();
        }
    }
}
