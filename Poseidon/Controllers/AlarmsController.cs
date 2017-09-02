using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class AlarmsController : Controller
    {
        [HttpPut("{id}/ack")]
        public IActionResult Ack([FromRoute] string id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] object alarm)
        {
            return Ok();
        }
    }
}
