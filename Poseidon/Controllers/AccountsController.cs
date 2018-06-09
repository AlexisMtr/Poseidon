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
            string token = await userService.Login(credentials);
            return Ok(new TokenDto
            {
                Token = token
            });
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(UserCreationDto model)
        {
            string token = await userService.SignIn(model);
            return Ok(new TokenDto
            {
                Token = token
            });
        }
    }
}
