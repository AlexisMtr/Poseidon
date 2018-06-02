using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Poseidon.Services;
using Poseidon.Models;
using AutoMapper;
using Poseidon.Dtos;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AlarmsController : Controller
    {
        private readonly AlarmService alarmService;
        private readonly IMapper mapper;

        public AlarmsController(AlarmService alarmService, IMapper mapper)
        {
            this.alarmService = alarmService;
            this.mapper = mapper;
        }

        [HttpPut("{id}/ack")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Ack(int id)
        {
            Alarm alarm = alarmService.Ack(id);
            return Ok(mapper.Map<AlarmDto>(alarm));
        }
    }
}
