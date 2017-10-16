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
    public class PoolsController : Controller
    {
        private readonly IPoolsRepository<Pool> PoolRepository;
        private readonly IMeasuresRepository<Measure> MeasureRepository;
        private readonly IAlarmsRepository<Alarm> AlarmRepository;
        private readonly IPoolConfigurationsRepository<PoolConfiguration> ConfigurationRepository;
        private readonly UserPermissionService PermissionService;

        public PoolsController(IPoolsRepository<Pool> poolRepository, IMeasuresRepository<Measure> measureRepository,
            IAlarmsRepository<Alarm> alarmRepository, IPoolConfigurationsRepository<PoolConfiguration> configurationRepository,
            UserPermissionService userPermissionService)
        {
            this.PoolRepository = poolRepository;
            this.AlarmRepository = alarmRepository;
            this.MeasureRepository = measureRepository;
            this.ConfigurationRepository = configurationRepository;
            this.PermissionService = userPermissionService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolOverviewPayload))]
        public IActionResult Get([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            Pool pool = this.PoolRepository.GetById(id);

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

            IEnumerable<Alarm> alarms = this.AlarmRepository.GetByPoolId(id, filter);

            return Ok(alarms.ToList());
        }

        [HttpGet("{id}/measures/current")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MeasuresPayload))]
        public IActionResult GetCurrentMeasures([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            IQueryable<Measure> measures = this.MeasureRepository.GetByPoolId(id)
                .OrderBy(m => m.Timestamp);

            return Ok(new MeasuresPayload
            {
                PoolId = id,
                Temperature = measures.LastOrDefault(m => m.MeasureType.Equals(MeasureType.Temperature)),
                Ph = measures.LastOrDefault(m => m.MeasureType.Equals(MeasureType.Ph)),
                Level = measures.LastOrDefault(m => m.MeasureType.Equals(MeasureType.Level)),
                Battery = measures.LastOrDefault(m => m.MeasureType.Equals(MeasureType.Battery))
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

        [HttpGet("{id}/measures")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Measure>))]
        public IActionResult GetMeasuresBetweenDate([FromRoute] string id, [FromBody] DateTimeFilter dates)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            if (dates.MinDateTimestamp == default(long) || dates.MaxDateTimestamp == default(long))
                return BadRequest();

            IEnumerable<Measure> measures = this.MeasureRepository.GetByPoolIdBetween(id, dates.MinDateTimestamp.ToDateTime(), dates.MaxDateTimestamp.ToDateTime());

            return Ok(measures.ToList());
        }

        [HttpGet("{id}/measures/{type}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Measure>))]
        public IActionResult GetMeasuresBetweenDate([FromRoute] string id, [FromRoute] string type, [FromBody] DateTimeFilter dates)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            if (!this.PermissionService.IsAllowed(user.Id, id))
                return Forbid();

            if(!Enum.TryParse(type, out MeasureType enumValue))
                return BadRequest();

            if (dates.MinDateTimestamp == default(long) || dates.MaxDateTimestamp == default(long))
                return BadRequest();

            IEnumerable<Measure> measures = this.MeasureRepository.GetByPoolIdBetween(id, dates.MinDateTimestamp.ToDateTime(), dates.MaxDateTimestamp.ToDateTime())
                .Where(m => m.MeasureType.Equals((int)enumValue));

            return Ok(measures.ToList());
        }

        [HttpGet("{id}/configuration")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolConfiguration))]
        public IActionResult GetConfiguration([FromRoute] string id)
        {
            return Ok(this.ConfigurationRepository.GetByPoolId(id));
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

        [HttpPut("{id}/configuration")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        public IActionResult PutConfiguration([FromRoute] string id, [FromBody] PoolConfiguration model)
        {
            var config = this.ConfigurationRepository.GetByPoolId(id);
            if(config == null)
            {
                this.ConfigurationRepository.Add(model);
            }
            else
            {
                this.ConfigurationRepository.Update(id, model);
            }

            return Ok(new ConfirmMessagePayload
            {
                ObjectIdentifier = this.ConfigurationRepository.GetByPoolId(id).Id,
                Code = HttpStatusCode.OK,
                Timestamp = DateTime.UtcNow.ToTimestamp(),
                Message = "Configuration updated"
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

            this.PoolRepository.Add(pool);

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
