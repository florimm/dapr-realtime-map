using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using RealtimeMap.API.Models;

namespace RealtimeMap.API.Actors
{
    public interface IVehicleActor : IActor
    {
        Task PositionChanged(Position position);
    }

    public class VehicleActor : Actor, IVehicleActor
    {
        public VehicleActor(ActorHost host) : base(host)
        {
            
        }
        public async Task PositionChanged(Position position)
        {
            var vehiclePosition = await StateManager.GetStateAsync<Position>("position");
            if (vehiclePosition == null || (position.Latitude != vehiclePosition.Latitude || position.Longitude != vehiclePosition.Longitude))
            {
                await StateManager.SetStateAsync("position", position);
                await StateManager.SaveStateAsync();
                await this.ProxyFactory
                    .CreateActorProxy<ISignalRActor>(ActorId.CreateRandom(), nameof(SignalRActor))
                    .PositionChanged(position);
            }
        }
    }
}
