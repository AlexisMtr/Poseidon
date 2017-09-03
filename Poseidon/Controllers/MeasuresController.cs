using AlexisMtrTools.DateTime;
using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System;

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
            IRepository<Measure> repository = new MongoDbMeasuresRepository(this.Service);

            try
            {
                repository.Add(measures.Temperature);
                repository.Add(measures.Ph);
                repository.Add(measures.Level);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok(new
            {
                Message = "Added",
                Timestamp = DateTime.Now.ToTimestamp()
            });
        }
    }
}
