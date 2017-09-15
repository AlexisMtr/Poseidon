using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class PoolController : Controller
    {
        private readonly IRepository<Pool> Repository;

        public PoolController(IRepository<Pool> repository)
        {
            this.Repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolApi))]
        public IActionResult Get([FromRoute] string id)
        {
            Pool pool = this.Repository.GetById(id);

            if (pool == null)
                return NotFound();

            return Ok(new PoolApi
            {
                PoolId = pool.Id,
                Name = pool.Name,
                Location = pool.Location,
                LastUpdate = pool.Measures.OrderBy(m => m.Timestamp).Last().Timestamp,
                AlarmsCount = pool.Alarms.Where(a => !a.Ack).Count()
            });
        }

        [HttpGet("{id}/alarms")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Alarm>))]
        public IActionResult GetAlarms([FromRoute] string id, AlarmState filter = AlarmState.All)
        {
            IEnumerable<Alarm> alarms = (this.Repository as MongoDbAlarmsRepository).GetByPoolId(id, filter);

            return Ok(alarms.ToList());
        }

        [HttpGet("{id}/measures/current")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolMeasuresApi))]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            IQueryable<Measure> measures = (this.Repository as MongoDbMeasuresRepository).GetByPoolId(id)
                .OrderByDescending(m => m.Timestamp);

            return Ok(new PoolMeasuresApi
            {
                PoolId = id,
                Temperature = measures.FirstOrDefault(m => m.MeasureType.Equals(MeasureType.Temperature)),
                Ph = measures.FirstOrDefault(m => m.MeasureType.Equals(MeasureType.Ph)),
                Level = measures.FirstOrDefault(m => m.MeasureType.Equals(MeasureType.Level))
            });
        }
        
        [HttpGet("{id}/measures/forecast")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(NoContentResult))]
        public IActionResult GetForecastMeasures([FromRoute] string id)
        {
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkResult))]
        public IActionResult Put([FromRoute] string id, [FromBody] PoolApi model)
        {
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkResult))]
        public IActionResult Post([FromBody] PoolApi model)
        {
            Pool pool = new Pool
            {
                Id = model.PoolId,
                Name = model.Name,
                Alarms = new List<Alarm>(),
                Measures = new List<Measure>(),
                Location = model.Location,
                UsersId = new List<string>()
            };

            this.Repository.Add(pool);

            return Ok();
        }
    }
}
