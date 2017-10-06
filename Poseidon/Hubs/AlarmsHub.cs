using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Poseidon.Helpers;
using System.Collections.Generic;
using Poseidon.Models;

namespace Poseidon.Hubs
{
    public class AlarmsHub : Hub
    {
        private readonly IConnectionMapper<string> Mapper;

        public AlarmsHub(IConnectionMapper<string> mapper)
        {
            Mapper = mapper;
        }

        public override Task OnConnectedAsync()
        {
            var claims = UserDataClaim.GetUserDataClaim(Context.User.Claims);
            Mapper.Add(claims.Id, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var claims = UserDataClaim.GetUserDataClaim(Context.User.Claims);
            Mapper.Remove(claims.Id, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async void SendAlarm(IEnumerable<string> usersId, List<Alarm> alarm)
        {
            var connectionsId = new List<string>();
            foreach(var id in usersId)
            {
                connectionsId.AddRange(Mapper.GetConnections(id));
            }

            foreach (var connection in connectionsId)
            {
                await Clients.Client(connection).InvokeAsync("alarm", alarm);
            }
        }
    }
}
