using Microsoft.AspNetCore.Authorization;

namespace Poseidon.Attributes
{
    public class MultipleAuthorizeAttribute : AuthorizeAttribute
    {
        public MultipleAuthorizeAttribute(string[] roles)
        {
            Roles = string.Join(',', roles);
        }
    }
}
