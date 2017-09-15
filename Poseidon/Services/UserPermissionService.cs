using Poseidon.Models;
using Poseidon.Repositories;
using System.Linq;

namespace Poseidon.Services
{
    public class UserPermissionService
    {
        private readonly IRepository<User> UserRepository;
        private readonly IRepository<Pool> PoolRepository;

        public UserPermissionService(IRepository<User> userRepository, IRepository<Pool> poolRepository)
        {
            this.UserRepository = userRepository;
            this.PoolRepository = poolRepository;
        }

        public bool IsAllowed(string userId, string poolId)
        {
            var usersId = PoolRepository.GetById(poolId).UsersId;
            var poolsId = UserRepository.GetById(userId).PoolsId;

            return usersId.ToList().Contains(poolId) && poolsId.ToList().Contains(userId);
        }
    }
}
