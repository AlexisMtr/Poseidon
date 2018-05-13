using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Configuration;
using Poseidon.Dtos;
using Poseidon.Models;
using Poseidon.Services;
using System.Net;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PoolsController : Controller
    {
        private readonly PoolService poolService;
        private readonly TelemetryService telemetryService;
        private readonly AlarmService alarmService;

        public PoolsController(PoolService poolService, TelemetryService telemetryService, AlarmService alarmService)
        {
            this.poolService = poolService;
            this.telemetryService = telemetryService;
            this.alarmService = alarmService;
        }

        [HttpGet]
        [Authorize(Roles = Roles.SysAdmin)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<PoolDto>))]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/telemetry/current")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult GetCurrentTelemetry(int id)
        {
            return Ok();
        }        

        [HttpGet("{id}/telemetry/history")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<TelemetryDto>))]
        public IActionResult GetTelemetryHistory(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/telemetry/forecast")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult GetForecastTelemetry(int id)
        {
            return Ok();
        }

        [HttpGet("{id}/alarms")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<AlarmDto>))]
        public IActionResult GetAlarms(int id)
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = Roles.SysAdmin)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult Post(object model)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        [MultipleAuthorize(new string[] { Roles.SysAdmin, Roles.Administrator })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult Put(int id, object model)
        {
            return Ok();
        }
    }
}
