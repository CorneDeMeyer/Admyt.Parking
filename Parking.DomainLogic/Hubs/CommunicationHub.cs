using Microsoft.AspNetCore.SignalR;

namespace Parking.DomainLogic.Hubs
{
    public class CommunicationHub : Hub
    {
        public async Task SendMessage(Guid gate, object message)
        {
            await Clients.All.SendAsync("Broadcast", gate, message);
        }
    }
}
