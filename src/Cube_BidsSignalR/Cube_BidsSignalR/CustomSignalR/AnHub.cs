using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
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

        //public async IAsyncEnumerable<int> Counter(
        //int count,
        //int delay,
        //[EnumeratorCancellation]
        //CancellationToken cancellationToken)
        //{
        //    for (var i = 0; i < count; i++)
        //    {
        //        // Check the cancellation token regularly so that the server will stop
        //        // producing items if the client disconnects.
        //        cancellationToken.ThrowIfCancellationRequested();

        //        yield return i;

        //        // Use the cancellationToken in other APIs that accept cancellation
        //        // tokens so the cancellation can flow down to them.
        //        await Task.Delay(delay, cancellationToken);
        //    }
        //}
    }
}

