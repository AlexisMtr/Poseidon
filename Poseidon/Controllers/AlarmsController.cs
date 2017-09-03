using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System;
using AlexisMtrTools.DateTime;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class AlarmsController : Controller
    {
        private MongoDbService Service { get; set; }

        public AlarmsController(MongoDbService service)
        {
            this.Service = service;
        }


        [HttpPut("{id}/ack")]
        public IActionResult Ack([FromRoute] string id)
        {
            IRepository<Alarm> repository = new MongoDbAlarmsRespository(this.Service);
            (repository as MongoDbAlarmsRespository).Ack(id);
            Alarm alarm = repository.GetById(id);

            return Ok(new PoolAlarmAcknowledgmentApi
            {
                PoolId = alarm.PoolId,
                AlarmId = id,
                AlarmTimestamp = alarm.Timestamp,
                AlarmAcknowledgmentTimestamp = DateTime.Now.ToTimestamp()
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] AlarmApi alarm)
        {
            IRepository<Alarm> repository = new MongoDbAlarmsRespository(this.Service);

            Alarm dbAlarm = new Alarm
            {
                Id = "A" + DateTime.Now.ToTimestamp(),
                Description = alarm.Description,
                PoolId = alarm.PoolId,
                Timestamp = DateTime.Now.ToTimestamp(),
                AlarmType = alarm.AlarmType
            };

            repository.Add(dbAlarm);

            return Ok(new
            {
                Message = "Alarm created",
                AlarmId = dbAlarm.Id
            });
        }
    }
}
