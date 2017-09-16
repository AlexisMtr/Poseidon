using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Poseidon.Payload;
using Poseidon.Helpers;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System;
using AlexisMtrTools.DateTime;

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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolOverviewPayload))]
        public IActionResult Get([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            Pool pool = this.Repository.GetById(id);

            if (pool == null)
                return NotFound();

            return Ok(new PoolOverviewPayload
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
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MeasuresPayload))]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            IQueryable<Measure> measures = (this.Repository as MongoDbMeasuresRepository).GetByPoolId(id)
                .OrderByDescending(m => m.Timestamp);

            return Ok(new MeasuresPayload
            {
                PoolId = id,
                Temperature = measures.FirstOrDefault(m => m.MeasureType.Equals(MeasureType.Temperature)),
                Ph = measures.FirstOrDefault(m => m.MeasureType.Equals(MeasureType.Ph)),
                Level = measures.FirstOrDefault(m => m.MeasureType.Equals(MeasureType.Level))
            });
        }
        
        [HttpGet("{id}/measures/forecast")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        public IActionResult GetForecastMeasures([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            return Ok(new ConfirmMessagePayload
            {
                Code = HttpStatusCode.NotImplemented,
                Message = "Not implemented",
                ObjectIdentifier = "",
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        public IActionResult Put([FromRoute] string id, [FromBody] PoolPayload model)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            return Ok(new ConfirmMessagePayload
            {
                Code = HttpStatusCode.NotImplemented,
                Message = "Not implemented",
                ObjectIdentifier = "",
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        public IActionResult Post([FromBody] PoolPayload model)
        {
            Pool pool = new Pool
            {
                Id = "",
                Name = model.Name,
                Alarms = new List<Alarm>(),
                Measures = new List<Measure>(),
                Location = model.Location,
                UsersId = new List<string>()
            };

            this.Repository.Add(pool);

            return Ok(new ConfirmMessagePayload
            {
                Code = HttpStatusCode.Created,
                Message = "Pool added",
                ObjectIdentifier = pool.Id,
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }
    }
}
