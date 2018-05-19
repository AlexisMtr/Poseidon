using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Attributes;
using Poseidon.Dtos;
using Poseidon.Filters;
using Poseidon.Models;
using Poseidon.Services;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class PoolsController : Controller
    {
        private readonly PoolService poolService;
        private readonly TelemetryService telemetryService;
        private readonly AlarmService alarmService;
        private readonly UserManager<User> userManager;

        public PoolsController(PoolService poolService, TelemetryService telemetryService, AlarmService alarmService, UserManager<User> userManager)
        {
            this.poolService = poolService;
            this.telemetryService = telemetryService;
            this.alarmService = alarmService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = Roles.SysAdmin)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<PoolDto>))]
        public IActionResult GetAll([FromQuery]PoolFilter filter, [FromQuery]int rowsPerPage = 20, [FromQuery]int pageNumber = 1)
        {
            PaginatedElement<Pool> pools = poolService.Get(filter, rowsPerPage, pageNumber);
            return Ok(pools);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult Get([FromRoute]int id)
        {
            Pool pool = poolService.Get(id);
            return Ok(pool);
        }

        [HttpGet("{id}/telemetry/current")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TelemetryDto>))]
        public IActionResult GetCurrentTelemetry([FromRoute]int id)
        {
            IEnumerable<Telemetry> telemetries = telemetryService.GetAllCurrent(id);
            return Ok(telemetries);
        }

        [HttpGet("{id}/telemetry/history")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<TelemetryDto>))]
        public IActionResult GetTelemetryHistory([FromRoute]int id, [FromQuery]TelemetryFilter filter, int rowsPerPage = 20, int pageNumber = 1)
        {
            PaginatedElement<Telemetry> telemetries = telemetryService.GetByPool(id, filter, rowsPerPage, pageNumber);
            return Ok(telemetries);
        }

        [HttpGet("{id}/telemetry/forecast")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult GetForecastTelemetry([FromRoute]int id)
        {
            return NotFound();
        }

        [HttpGet("{id}/alarms")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<AlarmDto>))]
        public IActionResult GetAlarms([FromRoute]int id, [FromQuery]AlarmFilter filter, [FromQuery]int rowsPerPage = 20, [FromQuery]int pageNumber = 1)
        {
            PaginatedElement<Alarm> alarms = alarmService.GetByPool(id, filter, rowsPerPage, pageNumber);
            return Ok(alarms);
        }

        [HttpPost]
        [Authorize(Roles = Roles.SysAdmin)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public async Task<IActionResult> Post([FromBody]PoolCreationDto model)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            Pool pool = poolService.Add(model, user);
            return Ok(pool);
        }

        [HttpPut("{id}")]
        [MultipleAuthorize(new string[] { Roles.SysAdmin, Roles.Administrator })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult Put([FromRoute]int id, [FromBody]PoolCreationDto model)
        {
            Pool pool = poolService.Update(id, model);
            return Ok(pool);
        }
    }
}
