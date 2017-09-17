using AlexisMtrTools.DateTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Models;
using Poseidon.Payload;
using Poseidon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersRepository<User> Repository;

        public UsersController(IUsersRepository<User> repository)
        {
            this.Repository = repository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserPayload))]
        public IActionResult Get([FromRoute] string id)
        {
            User user = this.Repository.GetById(id);
            return Ok(new UserPayload
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                PoolsId = user.PoolsId
            });
        }

        [HttpGet("{id}/pools")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<PoolOverviewPayload>))]
        public IActionResult GetPools([FromRoute] string id)
        {
            IEnumerable<Pool> pools = this.Repository.GetPools(id);

            List<PoolOverviewPayload> poolsApi = new List<PoolOverviewPayload>();
            foreach(Pool p in pools)
            {
                poolsApi.Add(new PoolOverviewPayload
                {
                    PoolId = p.Id,
                    Location = p.Location,
                    Name = p.Name,
                    LastUpdate = p.Measures.OrderBy(m => m.Timestamp).Last().Timestamp,
                    AlarmsCount = p.Alarms.Where(a => !a.Ack).Count()
                });
            }

            return Ok(poolsApi);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        public IActionResult Put([FromRoute] string id, [FromBody] UserPayload user)
        {
            return Ok(new ConfirmMessagePayload
            {
                Code = HttpStatusCode.NotImplemented,
                Message = "Endpoint not implemented",
                ObjectIdentifier = "",
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ConfirmMessagePayload))]
        public IActionResult Post([FromBody] RegisterPayload user)
        {
            User dbUser = new User
            {
                Id = "U" + DateTime.Now.ToTimestamp(),
                Login = user.Login,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Password = user.Password,
                PoolsId = new List<string>()
            };

            this.Repository.Add(dbUser);

            return Ok(new ConfirmMessagePayload
            {
                Code = HttpStatusCode.Created,
                Message = $"{dbUser.LastName} {dbUser.FirstName} added",
                ObjectIdentifier = dbUser.Id,
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }
    }
}
