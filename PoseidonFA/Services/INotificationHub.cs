using PoseidonFA.Configuration;
using PoseidonFA.Models;
using System.Collections.Generic;

namespace PoseidonFA.Services
{
    public interface INotificationHub
    {
        NotificationHubSettings Settings { get; }

        void Send(IEnumerable<Alarm> alarms, IEnumerable<string> receiversIds);
    }
}
