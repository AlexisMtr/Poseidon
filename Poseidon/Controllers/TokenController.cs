using AlexisMtrTools.DateTime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Poseidon.Configuration;
using Poseidon.Helpers;
using Poseidon.Models;
using Poseidon.Payload;
using Poseidon.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Poseidon.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IRepository<User> UserRepository;
        private readonly IssuerSigningKeySettings Issuer;
        public TokenController(IRepository<User> userRepository, IOptions<IssuerSigningKeySettings> signinKey)
        {
            this.UserRepository = userRepository;
            this.Issuer = signinKey.Value;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GeneratedTokenPayload))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(UnauthorizedResult))]
        public IActionResult Get([FromBody] Credentials credentials)
        {
            var user = (this.UserRepository as MongoDbUsersRepository).GetByLoginAndPassword(credentials.Username, credentials.Password);

            if (user == null)
                return Unauthorized();

            var userData = new UserDataClaim
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                Login = user.Login,
                Role = user.Role
            };

            List<Claim> claims = new List<Claim>
            {
                { new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(userData)) },
                { new Claim(ClaimTypes.Role, user.Role) }
            };

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Issuer.SigningKey));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddYears(1), signingCredentials: signingCredentials);

            return Ok(new GeneratedTokenPayload
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserData = userData,
                Timestamp = DateTime.UtcNow.ToTimestamp()
            });
        }
    }
}
