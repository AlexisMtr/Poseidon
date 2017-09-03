using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class MeasuresController : Controller
    {
        private MongoDbService Service { get; set; }

        protected MeasuresController(MongoDbService service)
        {
            this.Service = service;
        }

        [HttpPost]
        public IActionResult Post([FromForm] PoolMeasuresApi measures)
        {
            return Ok();
        }
    }
}
