using System.Collections.Generic;
using PoseidonFA.Configuration;
using PoseidonFA.Models;
using Microsoft.AspNet.SignalR.Client;
using Autofac.Core;

namespace PoseidonFA.Services
{
    public class SignalRNotificationHub : Service, INotificationHub
    {
        public NotificationHubSettings Settings { get; private set; }

        public override string Description => "Send notifications througt a WebSocket";

        private readonly HubConnection HubConnection;

        public SignalRNotificationHub(NotificationHubSettings settings)
        {
            this.Settings = settings;
            this.HubConnection = new HubConnection(this.Settings.FullRightConnectionString);
        }

        public void Send(IEnumerable<Alarm> alarms, IEnumerable<string> receiversIds)
        {
            var proxy = this.HubConnection.CreateHubProxy(this.Settings.HubName);
            this.HubConnection.Start();

            proxy.Invoke("SendAlarm", receiversIds, alarms);

            this.HubConnection.Stop();
        }
    }
}
