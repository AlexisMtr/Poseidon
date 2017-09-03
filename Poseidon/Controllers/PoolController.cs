using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
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

        [HttpGet("{id")]
        public IActionResult Get([FromRoute] string id)
        {
            IRepository<Pool> repository = new MongoDbPoolRespository();
            Pool pool = repository.GetById(id);
            
            return Ok(new PoolApi
            {
                PoolId = pool.Id,
                Name = pool.Name,
                Location = pool.Location,
                LastUpdate = pool.Measures.OrderBy(m => m.Timestamp).Last().Timestamp
            });
        }

        [HttpGet("{id}/alarms")]
        public IActionResult GetAlarms([FromRoute] string id, AlarmState filter = AlarmState.All)
        {
            IRepository<Alarm> repository = new MongoDbAlarmsRespository(this.Service);
            IQueryable<Alarm> alarms = (repository as MongoDbAlarmsRespository).GetByPoolId(id, filter);

            return Ok(alarms.ToList());
        }

        [HttpGet("{id}/measures/current")]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            IRepository<Measure> repository = new MongoDbMeasuresRepository();
            IQueryable<Measure> measures = (repository as MongoDbMeasuresRepository).GetByPoolId(id)
                .OrderByDescending(m => m.Timestamp);

            return Ok(new PoolMeasuresApi
            {
                Temperature = measures.FirstOrDefault(m => m.Type.Equals(MeasureType.Temperature)),
                Ph = measures.FirstOrDefault(m => m.Type.Equals(MeasureType.Ph)),
                Level = measures.FirstOrDefault(m => m.Type.Equals(MeasureType.Level))
            });
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
