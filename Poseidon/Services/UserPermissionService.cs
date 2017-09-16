using Poseidon.Models;
using Poseidon.Repositories;
using System;
using System.Linq;
using System.Text;

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

        public bool IsAuthenticated(string encodingCredentials, out string userId)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var decodedCrendentials = encoding.GetString(Convert.FromBase64String(encodingCredentials));
            var credentials = decodedCrendentials.Split(':');

            var user = UserRepository.GetById(credentials[0]);
            userId = user.Id;

            if (user == null)
                return false;

            return user.Password.Equals(credentials[1]);
        }

        public bool IsAllowed(string userId, string poolId)
        {
            var usersId = PoolRepository.GetById(poolId).UsersId;
            var poolsId = UserRepository.GetById(userId).PoolsId;

            return usersId.ToList().Contains(userId) && poolsId.ToList().Contains(poolId);
        }
    }
}
