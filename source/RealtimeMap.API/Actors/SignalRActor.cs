using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;
using RealtimeMap.API.Models;

namespace RealtimeMap.API.Actors
{
    public interface ISignalRActor : IActor
    {
        Task PositionChanged(Position position);
    }

    public class SignalRActor : Actor, ISignalRActor
    {
        public SignalRActor(ActorHost host) : base(host)
        {
            
        }
        public async Task PositionChanged(Position position)
        {
            Logger.LogInformation($"{Id} position changed to SignalR {position.Latitude}, {position.Longitude}");
            System.Console.WriteLine($"Vehicle position changed to from SignalR {position.Latitude}, {position.Longitude}");
            await Task.CompletedTask;
            //await Clients.All.SendAsync("positionChanged", position);
        }
    }
}
