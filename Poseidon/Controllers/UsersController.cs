using AlexisMtrTools.DateTime;
using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IRepository<User> Repository;

        public UsersController(IRepository<User> repository)
        {
            this.Repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            User user = this.Repository.GetById(id);
            return Ok(new UserApi
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                PoolsId = user.PoolsId
            });
        }

        [HttpGet("{id}/pools")]
        public IActionResult GetPools([FromRoute] string id)
        {
            IEnumerable<Pool> pools = (this.Repository as MongoDbUsersRepository).GetPools(id);

            List<PoolApi> poolsApi = new List<PoolApi>();
            foreach(Pool p in pools)
            {
                poolsApi.Add(new PoolApi
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
        public IActionResult Put([FromRoute] string id, [FromBody] object user)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] RegisterApi user)
        {
            User udbUser = new User
            {
                Id = "U" + DateTime.Now.ToTimestamp(),
                Login = user.Login,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Password = user.Password,
                PoolsId = new List<string>()
            };

            this.Repository.Add(udbUser);

            return Ok(new
            {
                Message = $"{user.FirstName} {user.LastName} created",
                Login = user.Login
            });
        }
    }
}
