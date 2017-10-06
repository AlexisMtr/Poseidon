using Autofac.Core;
using Microsoft.AspNet.SignalR.Client;

namespace PoseidonFA.Services
{
    public class HubConnectionService : Service
    {
        public HubConnection Connection { get; set; }
        
        public HubConnectionService(HubConnection connection)
        {
            this.Connection = connection;
        }


        public override string Description => "Proxy for SignalR HubConnection";
    }
}
