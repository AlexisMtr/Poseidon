using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Dtos;
using Poseidon.Models;
using Poseidon.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DeviceController : Controller
    {
        private readonly DeviceService deviceService;
        private readonly UserManager<User> userManager;

        public DeviceController(DeviceService deviceService, UserManager<User> userManager)
        {
            this.deviceService = deviceService;
            this.userManager = userManager;
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
