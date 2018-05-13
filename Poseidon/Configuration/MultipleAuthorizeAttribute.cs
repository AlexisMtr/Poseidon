using Microsoft.AspNetCore.Authorization;

namespace Poseidon.Configuration
{
    public class MultipleAuthorizeAttribute : AuthorizeAttribute
    {
        public MultipleAuthorizeAttribute(string[] roles)
        {
            Roles = string.Join(',', roles);
        }
    }
}
