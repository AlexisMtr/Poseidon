using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class PoolController : Controller
    {
        private MongoDbService Service { get; set; }

        public PoolController(MongoDbService service)
        {
            this.Service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            IRepository<Pool> repository = new MongoDbPoolRespository(this.Service);
            Pool pool = repository.GetById(id);

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
        public IActionResult GetAlarms([FromRoute] string id, AlarmState filter = AlarmState.All)
        {
            IRepository<Alarm> repository = new MongoDbAlarmsRespository(this.Service);
            IEnumerable<Alarm> alarms = (repository as MongoDbAlarmsRespository).GetByPoolId(id, filter);

            return Ok(alarms.ToList());
        }

        [HttpGet("{id}/measures/current")]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            IRepository<Measure> repository = new MongoDbMeasuresRepository(this.Service);
            IQueryable<Measure> measures = (repository as MongoDbMeasuresRepository).GetByPoolId(id)
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
        public IActionResult GetForecastMeasures([FromRoute] string id)
        {
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put([FromRoute] string id, [FromBody] PoolApi model)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] PoolApi model)
        {
            IRepository<Pool> repository = new MongoDbPoolRespository(this.Service);
            Pool pool = new Pool
            {
                Id = model.PoolId,
                Name = model.Name,
                Alarms = new List<Alarm>(),
                Measures = new List<Measure>(),
                Location = model.Location,
                UsersId = new List<string>()
            };

            repository.Add(pool);

            return Ok();
        }
    }
}
