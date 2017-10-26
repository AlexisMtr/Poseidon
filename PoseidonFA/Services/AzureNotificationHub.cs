using System.Collections.Generic;
using PoseidonFA.Models;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using PoseidonFA.Configuration;
using Autofac.Core;

namespace PoseidonFA.Services
{
    public class AzureNotificationHub : Service, INotificationHub
    {
        public NotificationHubSettings Settings { get; private set; }

        public override string Description => "Send notification througt the Azure Notification Hub";

        public AzureNotificationHub(NotificationHubSettings settings)
        {
            this.Settings = settings;
        }

        public void Send(IEnumerable<Alarm> alarms, IEnumerable<string> receiversIds)
        {
            Task.Run(async () =>
            {
                var clientHub = NotificationHubClient.CreateClientFromConnectionString(this.Settings.FullRightConnectionString, this.Settings.HubName);
                var windowsNotification = await clientHub.SendWindowsNativeNotificationAsync("", receiversIds);
                var androidNotification = await clientHub.SendGcmNativeNotificationAsync("", receiversIds);
            });
        }
    }
}
