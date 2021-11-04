using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
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
            await Task.CompletedTask;
            //await Clients.All.SendAsync("positionChanged", position);
        }
    }
}
