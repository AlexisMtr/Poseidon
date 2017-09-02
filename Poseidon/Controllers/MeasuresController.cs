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
    public class MeasuresController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromForm] object measures)
        {
            return Ok();
        }
    }
}
