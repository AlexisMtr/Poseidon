using System.Net;

namespace Poseidon.Payload
{
    public class ConfirmMessagePayload
    {
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
        public string ObjectIdentifier { get; set; }
        public long Timestamp { get; set; }
    }
}
