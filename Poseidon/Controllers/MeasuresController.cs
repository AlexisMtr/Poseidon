using AlexisMtrTools.DateTime;
using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using System;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class MeasuresController : Controller
    {
        private readonly IRepository<Measure> Repository;

        public MeasuresController(IRepository<Measure> repository)
        {
            this.Repository = repository;
        }

        [HttpPost]
        public IActionResult Post([FromBody] PoolMeasuresApi measures)
        {
            try
            {
                this.Repository.Add(measures.Temperature);
                this.Repository.Add(measures.Ph);
                this.Repository.Add(measures.Level);
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
