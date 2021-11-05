using System.Threading.Tasks;
using Dapr.Actors;
using Dapr.Actors.Runtime;
using Microsoft.Extensions.Logging;
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
            var vehiclePosition = await StateManager.TryGetStateAsync<Position>("position");
            if (!vehiclePosition.HasValue || (position.Latitude != vehiclePosition.Value.Latitude || position.Longitude != vehiclePosition.Value.Longitude))
            {
                Logger.LogInformation($"{Id} position changed to {position.Latitude}, {position.Longitude}");
                await StateManager.SetStateAsync("position", position);
                await StateManager.SaveStateAsync();
            }
            await this.ProxyFactory
                    .CreateActorProxy<ISignalRActor>(new ActorId(position.VehicleId), nameof(SignalRActor))
                    .PositionChanged(position);
        }
    }
}
