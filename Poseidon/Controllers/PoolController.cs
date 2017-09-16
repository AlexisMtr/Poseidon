using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Poseidon.APIModels;
using Poseidon.Helpers;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PoolController : Controller
    {
        private readonly IRepository<Pool> Repository;
        private readonly UserPermissionService PermissionService;

        public PoolController(IRepository<Pool> repository, UserPermissionService userPermissionService)
        {
            this.Repository = repository;
            this.PermissionService = userPermissionService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolApi))]
        public IActionResult Get([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

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
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            IEnumerable<Alarm> alarms = (this.Repository as MongoDbAlarmsRepository).GetByPoolId(id, filter);

            return Ok(alarms.ToList());
        }

        [HttpGet("{id}/measures/current")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolMeasuresApi))]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

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
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkResult))]
        public IActionResult Put([FromRoute] string id, [FromBody] PoolApi model)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

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
