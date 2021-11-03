using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RealtimeMap.API.Models;
using Dapr.Actors;
using Dapr.Actors.Client;
using RealtimeMap.API.Actors;

namespace RealtimeMap.API.MQQT
{
    public class MqttIngress : IHostedService
    {
        private readonly IConfiguration _configuration;

        private readonly IActorProxyFactory _client;

        private HrtPositionsSubscription _hrtPositionsSubscription;

        public MqttIngress(IConfiguration configuration, IActorProxyFactory client)
        {
            _configuration = configuration;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _hrtPositionsSubscription = await HrtPositionsSubscription.Start(
                sharedSubscriptionGroupName: GetSharedSubscriptionGroupName(),
                onPositionUpdate: ProcessHrtPositionUpdate);
        }

        private string GetSharedSubscriptionGroupName()
        {
            var sharedSubscriptionGroupName = _configuration["RealtimeMap:SharedSubscriptionGroupName"];

            return string.IsNullOrEmpty(sharedSubscriptionGroupName)
                ? $"group-{Guid.NewGuid()}"
                : sharedSubscriptionGroupName;
        }

        private async Task ProcessHrtPositionUpdate(HrtPositionUpdate hrtPositionUpdate)
        {
            var vehicleId = $"{hrtPositionUpdate.OperatorId}.{hrtPositionUpdate.VehicleNumber}";

            var position = new Position
            {
                OrgId = hrtPositionUpdate.OperatorId,
                Longitude = hrtPositionUpdate.VehiclePosition.Long.GetValueOrDefault(),
                Latitude = hrtPositionUpdate.VehiclePosition.Lat.GetValueOrDefault(),
                VehicleId = vehicleId,
                Heading = (int)hrtPositionUpdate.VehiclePosition.Hdg.GetValueOrDefault(),
                DoorsOpen = hrtPositionUpdate.VehiclePosition.Drst == 1,
                Timestamp = hrtPositionUpdate.VehiclePosition.Tst.GetValueOrDefault().Ticks,
                Speed = hrtPositionUpdate.VehiclePosition.Spd.GetValueOrDefault()
            };

            Console.WriteLine(position.VehicleId);
            var actorId = new ActorId(position.VehicleId);
            var proxy = _client.CreateActorProxy<VehicleActor>(actorId, nameof(VehicleActor));
            await proxy.PositionChanged(position);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _hrtPositionsSubscription?.Dispose();

            return Task.CompletedTask;
        }
    }
}
