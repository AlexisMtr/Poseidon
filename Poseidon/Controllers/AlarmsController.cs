using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using System;
using AlexisMtrTools.DateTime;
using System.Net;
using Poseidon.Services;
using Microsoft.AspNetCore.Authorization;
using Poseidon.Helpers;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AlarmsController : Controller
    {
        private readonly IRepository<Alarm> Repository;
        private readonly UserPermissionService PermissionService;

        public AlarmsController(IRepository<Alarm> repository, UserPermissionService userPermissionService)
        {
            this.Repository = repository;
            this.PermissionService = userPermissionService;
        }


        [HttpPut("{id}/ack")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PoolAlarmAcknowledgmentApi))]
        public IActionResult Ack([FromRoute] string id)
        {
            var user = UserDataClaim.GetUserDataClaim(HttpContext);
            var poolId = (this.Repository as MongoDbAlarmsRepository).GetById(id).PoolId;
            if (!this.PermissionService.IsAllowed(user.Id, poolId))
                return Forbid();

            (this.Repository as MongoDbAlarmsRepository).Ack(id);
            Alarm alarm = this.Repository.GetById(id);

            return Ok(new PoolAlarmAcknowledgmentApi
            {
                PoolId = alarm.PoolId,
                AlarmId = id,
                AlarmTimestamp = alarm.Timestamp,
                AlarmAcknowledgmentTimestamp = DateTime.Now.ToTimestamp()
            });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(OkResult))]
        public IActionResult Post([FromBody] AlarmApi alarm)
        {
            Alarm dbAlarm = new Alarm
            {
                Id = "A" + DateTime.Now.ToTimestamp(),
                Description = alarm.Description,
                PoolId = alarm.PoolId,
                Timestamp = DateTime.Now.ToTimestamp(),
                AlarmType = alarm.AlarmType
            };

            this.Repository.Add(dbAlarm);

            return Ok(new
            {
                Message = "Alarm created",
                AlarmId = dbAlarm.Id
            });
        }
    }
}
