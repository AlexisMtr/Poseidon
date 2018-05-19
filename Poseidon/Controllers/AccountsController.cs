using Microsoft.AspNetCore.Mvc;
using Poseidon.Dtos;
using Poseidon.Services;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly UserService userService;

        public AccountsController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Credentials credentials)
        {
            return Ok(await userService.Login(credentials));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserCreationDto model)
        {
            return Ok(await userService.SignIn(model));
        }
    }
}
