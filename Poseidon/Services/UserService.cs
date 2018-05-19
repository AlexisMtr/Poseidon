using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Poseidon.Configuration;
using Poseidon.Dtos;
using Poseidon.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Poseidon.Services
{
    public class UserService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IssuerSigningKeySettings signingKeySettings;

        public UserService( UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager, IOptions<IssuerSigningKeySettings> signingKeySettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.signingKeySettings = signingKeySettings.Value;
        }

        public async Task<string> Login(Credentials credentials)
        {
            User user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null) throw new UnauthorizedAccessException();

            IList<Claim> claims = await userManager.GetClaimsAsync(user);
            IList<string> userRoles = await userManager.GetRolesAsync(user);
            if(userRoles.Any())
            {
                Claim roleClaim = new Claim(ClaimTypes.Role, userRoles.First());
                claims.Add(roleClaim);
            }
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var checkPassword = await signInManager.CheckPasswordSignInAsync(user, credentials.Password, false);
            if (!checkPassword.Succeeded) throw new Exception();
            
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKeySettings.SigningKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddYears(1), signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> SignIn(UserCreationDto model)
        {
            IdentityResult userResult = await userManager.CreateAsync(new User
            {
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper()
            }, model.Password);

            if (!userResult.Succeeded) throw new Exception();

            User user = await userManager.FindByEmailAsync(model.Email);
            
            if(!await roleManager.RoleExistsAsync(Roles.SysAdmin))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = Roles.SysAdmin });
            }

            IdentityResult roleResult = await userManager.AddToRoleAsync(user, Roles.SysAdmin);

            if (!roleResult.Succeeded) throw new Exception();

            return await Login(new Credentials { Email = model.Email, Password = model.Password });
        }
    }
}
