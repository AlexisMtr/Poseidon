using Poseidon.Models;
using Poseidon.Repositories;
using System;
using System.Linq;
using System.Text;

namespace Poseidon.Services
{
    public class UserPermissionService
    {
        private readonly IUsersRepository<User> UserRepository;
        private readonly IPoolsRepository<Pool> PoolRepository;

        public UserPermissionService(IUsersRepository<User> userRepository, IPoolsRepository<Pool> poolRepository)
        {
            this.UserRepository = userRepository;
            this.PoolRepository = poolRepository;
        }

        public bool IsAllowed(string userId, string poolId)
        {
            var usersId = PoolRepository.GetById(poolId).UsersId;
            var poolsId = UserRepository.GetById(userId).PoolsId;

            return usersId.ToList().Contains(userId) && poolsId.ToList().Contains(poolId);
        }
    }
}
