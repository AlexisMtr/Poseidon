using AlexisMtrTools.DateTime;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Payload;
using Poseidon.Models;
using Poseidon.Repositories;
using System;
using System.Net;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class MeasuresController : Controller
    {
        private readonly IMeasuresRepository<Measure> Repository;

        public MeasuresController(IMeasuresRepository<Measure> repository)
        {
            this.Repository = repository;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(StatusCodeResult))]
        public IActionResult Post([FromBody] MeasuresPayload measures)
        {
            try
            {
                this.Repository.Add(measures.Temperature);
                this.Repository.Add(measures.Ph);
                this.Repository.Add(measures.Level);
            }
            catch(Exception e)
            {
                return StatusCode(500, new ConfirmMessagePayload
                {
                    Code = HttpStatusCode.InternalServerError,
                    Message = $"Error occured : {e.Message}",
                    ObjectIdentifier = "",
                    Timestamp = DateTime.UtcNow.ToTimestamp()
                });
            }

            return Ok(new ConfirmMessagePayload
            {
                Code = HttpStatusCode.Created,
                Message = "Measures added",
                ObjectIdentifier = "",
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }
    }
}
