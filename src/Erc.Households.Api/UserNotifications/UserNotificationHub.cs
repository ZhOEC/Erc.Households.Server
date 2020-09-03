using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erc.Households.Api.UserNotifications
{
    public class UserNotificationHub :Hub
    {
        readonly ConnectedClientsRepository _connectedClientsRepository;

        public UserNotificationHub(ConnectedClientsRepository connectedClientsRepository) => 
            _connectedClientsRepository = connectedClientsRepository;

        public override async Task OnConnectedAsync()
        {
            _connectedClientsRepository.Add(Context.ConnectionId, Context.UserIdentifier);
            foreach (var group in Context.User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value))
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
            
           await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectedClientsRepository.Remove(Context.ConnectionId);
            foreach (var group in Context.User.Claims.Where(c => string.Equals(c.Type, "Group", StringComparison.InvariantCultureIgnoreCase)).Select(c => c.Value))
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
