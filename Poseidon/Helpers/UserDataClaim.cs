using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace Poseidon.Helpers
{
    public class UserDataClaim
    {
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Login { get; set; }
        [JsonProperty]
        public string Role { get; set; }

        public static UserDataClaim GetUserDataClaim(HttpContext context)
        {
            var userDataString = context.User.Claims.First(x => x.Type.Equals(ClaimTypes.UserData)).Value;
            return JsonConvert.DeserializeObject<UserDataClaim>(userDataString);
        }
    }
}
