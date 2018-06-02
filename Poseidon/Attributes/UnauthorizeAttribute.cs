using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Poseidon.Attributes
{
    public class UnauthorizeAttribute : AuthorizeAttribute
    {
        public UnauthorizeAttribute(string role)
            : this(new string[] { role }) { }

        public UnauthorizeAttribute(string[] roles)
        {
            IEnumerable<string> authorizeRoles = typeof(Models.Roles)
                   .GetFields(BindingFlags.Static | BindingFlags.Public)
                   .Where(e => e.IsLiteral)
                   .Select(e => e.GetRawConstantValue() as string)
                   .Where(e => !roles.Contains(e));

            Roles = string.Join(',', authorizeRoles);
        }
    }
}
