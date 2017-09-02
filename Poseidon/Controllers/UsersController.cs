using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            return Ok();
        }

        [HttpGet("{id}/pool")]
        public IActionResult GetPool([FromRoute] string id)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] object user)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] object user)
        {
            return Ok();
        }
    }
}
