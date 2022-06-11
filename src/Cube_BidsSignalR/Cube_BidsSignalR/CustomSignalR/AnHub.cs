using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Cube_BidsSignalR.CustomSignalR
{
    //TUTORIAL used -> https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-5.0&tabs=visual-studio
    //  The Hub class manages connections, groups, and messaging.
    public class AnHub : Hub
    {
        //The SendMessage method can be called by a connected client to send a message to all clients.
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

