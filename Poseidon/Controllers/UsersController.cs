using Microsoft.AspNetCore.Mvc;
using Poseidon.APIModels;
using Poseidon.Models;
using Poseidon.Repositories;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private MongoDbService Service { get; set; }

        protected UsersController(MongoDbService service)
        {
            this.Service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] string id)
        {
            IRepository<User> repository = new MongoDbUsersRepository(this.Service);
            User user = repository.GetById(id);
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
            IRepository<User> repository = new MongoDbUsersRepository(this.Service);
            IEnumerable<Pool> pools = (repository as MongoDbUsersRepository).GetPools(id);

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
            return Ok();
        }
    }
}
