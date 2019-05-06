using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Attributes;
using Poseidon.Dtos;
using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper mapper;

        public PoolsController(PoolService poolService, TelemetryService telemetryService,
            AlarmService alarmService, UserManager<User> userManager, IMapper mapper)
        {
            this.poolService = poolService;
            this.telemetryService = telemetryService;
            this.alarmService = alarmService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<PoolDto>))]
        public async Task<IActionResult> GetAll([FromQuery]PoolFilter filter, [FromQuery]int rowsPerPage = 20, [FromQuery]int pageNumber = 1)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            PaginatedElement<Pool> pools = poolService.Get(filter, rowsPerPage, pageNumber, user);

            PaginatedDto<PoolDto> dto = mapper.Map<PaginatedDto<PoolDto>>(pools);
            dto.NextPageUrl = HttpContext.GetNextPageUrl(pageNumber < dto.PageCount);

            return Ok(dto);
        }

        [HttpGet("odata")]
        [EnableQuery]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Pool>))]
        public Task<IActionResult> GetOData()
        {
            var pools = new List<Pool>();
            for (int i = 0; i < 10; i++)
            {
                pools.Add(new Pool
                {
                    Name = $"pool{i}",
                    Id = i
                });
            }

            return Task.FromResult((IActionResult)Ok(pools.AsQueryable()));
        }

        [HttpGet("odata2")]
        [AllowAnonymous]
        //[EnableQuery]
        public Task<IActionResult> GetOData(ODataQueryOptions<Pool> queryOption)
        {
            var pools = new List<Pool>();
            for (int i = 0; i < 10; i++)
            {
                pools.Add(new Pool
                {
                    Name = $"pool{i}",
                    Id = i
                });
            }

            var result = queryOption.ApplyTo(pools.AsQueryable());
            return Task.FromResult((IActionResult)Ok(result));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult Get([FromRoute]int id)
        {
            Pool pool = poolService.Get(id);
            return Ok(mapper.Map<PoolDto>(pool));
        }

        [HttpGet("{id}/telemetry/current")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TelemetryDto>))]
        public IActionResult GetCurrentTelemetry([FromRoute]int id)
        {
            IEnumerable<Telemetry> telemetries = telemetryService.GetAllCurrent(id);
            return Ok(mapper.Map<IEnumerable<TelemetryDto>>(telemetries));
        }

        [HttpGet("{id}/telemetry/history")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<TelemetryDto>))]
        public IActionResult GetTelemetryHistory([FromRoute]int id, [FromQuery]TelemetryFilter filter, int rowsPerPage = 20, int pageNumber = 1)
        {
            PaginatedElement<Telemetry> telemetries = telemetryService.GetByPool(id, filter, rowsPerPage, pageNumber);

            PaginatedDto<TelemetryDto> dto = mapper.Map<PaginatedDto<TelemetryDto>>(telemetries);
            dto.NextPageUrl = HttpContext.GetNextPageUrl(pageNumber < dto.PageCount);

            return Ok(dto);
        }

        [HttpGet("{id}/telemetry/forecast")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public IActionResult GetForecastTelemetry([FromRoute]int id)
        {
            return NotFound();
        }

        [HttpGet("{id}/alarms")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PaginatedDto<AlarmDto>))]
        public async Task<IActionResult> GetAlarms([FromRoute]int id, [FromQuery]AlarmFilter filter, [FromQuery]int rowsPerPage = 20, [FromQuery]int pageNumber = 1)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            PaginatedElement<Alarm> alarms = alarmService.GetByPool(id, filter, rowsPerPage, pageNumber, user);

            PaginatedDto<AlarmDto> dto = mapper.Map<PaginatedDto<AlarmDto>>(alarms);

            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = Roles.SysAdmin)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public async Task<IActionResult> Post([FromBody]PoolCreationDto model)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            Pool pool = poolService.Add(model, user);
            return Ok(mapper.Map<PoolDto>(pool));
        }

        [HttpPut("{id}")]
        [MultipleAuthorize(new string[] { Roles.SysAdmin, Roles.Administrator })]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolDto))]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]PoolCreationDto model)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            Pool pool = poolService.Update(id, model, user);
            return Ok(mapper.Map<PoolDto>(pool));
        }

        [HttpPut("{poolId}/association/{deviceId}")]
        [MultipleAuthorize(new string[] { Roles.SysAdmin, Roles.Administrator })]
        public async Task<IActionResult> Associate(int poolId, string deviceId)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            poolService.Associate(poolId, deviceId, user);
            return Ok();
        }

        [HttpDelete("{poolId}/association")]
        [MultipleAuthorize(new string[] { Roles.SysAdmin, Roles.Administrator })]
        public async Task<IActionResult> Dissociate(int poolId)
        {
            string userEmail = User.FindFirst(ClaimTypes.Email).Value;
            User user = await userManager.FindByEmailAsync(userEmail);

            poolService.Dissociate(poolId, user);
            return Ok();
        }
    }
}
