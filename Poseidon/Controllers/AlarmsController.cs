using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Poseidon.Services;
using Poseidon.Models;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AlarmsController : Controller
    {
        private readonly AlarmService alarmService;

        public AlarmsController(AlarmService alarmService)
        {
            this.alarmService = alarmService;
        }

        [HttpPut("{id}/ack")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Ack(int id)
        {
            Alarm alarm = alarmService.Ack(id);
            return Ok(alarm);
        }
    }
}
