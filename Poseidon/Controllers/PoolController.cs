using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class PoolController : Controller
    {
        [HttpGet("{id")]
        public IActionResult Get([FromRoute] string id)
        {
            return Ok();
        }

        [HttpGet("{id}/alarms")]
        public IActionResult GetAlarms([FromRoute] string id)
        {
            return Ok();
        }

        [HttpGet("{id}/measures/current")]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            return Ok();
        }
        
        [HttpGet("{id}/measures/forecast")]
        public IActionResult GetForecastMeasures([FromRoute] string id)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromRoute] string id, [FromBody] PoolApi pool)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] PoolApi pool)
        {
            return Ok();
        }
    }
}
