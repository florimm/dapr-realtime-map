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
            Logger.LogInformation($"{Id} position changed to {position.Latitude}, {position.Longitude}");
            //var vehiclePosition = await StateManager.GetStateAsync<Position>("position");
            //if (vehiclePosition == null || (position.Latitude != vehiclePosition.Latitude || position.Longitude != vehiclePosition.Longitude))
            //{
                System.Console.WriteLine($"Vehicle position changed to {position.Latitude}, {position.Longitude}");
                // await StateManager.SetStateAsync("position", position);
                // await StateManager.SaveStateAsync();
                // await this.ProxyFactory
                //     .CreateActorProxy<ISignalRActor>(ActorId.CreateRandom(), nameof(SignalRActor))
                //     .PositionChanged(position);
            //}
            await Task.CompletedTask;
        }
    }
}
