using Poseidon.Helpers;

namespace Poseidon.Payload
{
    public class GeneratedTokenPayload
    {
        public string Token { get; set; }
        public UserDataClaim UserData { get; set; }
        public long Timestamp { get; set; }
    }
}
