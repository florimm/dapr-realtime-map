using Dapr;
using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealtimeMap.API.Actors;
using RealtimeMap.API.Models;
using RealtimeMap.API.MQQT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeMap.API.Controllers
{
    [ApiController]
    public class CarPositionController : ControllerBase
    {
        private IActorProxyFactory proxyFactory;

        public CarPositionController(IActorProxyFactory proxyFactory)
        {
            this.proxyFactory = proxyFactory;
        }


        [HttpPost("car-position-change-events")]
        public async Task<IActionResult> Subscribe(Root2 hrtPositionUpdate)
        {
            // await Task.Delay(1000);
            if (hrtPositionUpdate != null && hrtPositionUpdate.VP != null)
                {
                    var vehicleId = $"{hrtPositionUpdate.VP.oper}-{hrtPositionUpdate.VP.veh}";
                    var position = new Position()
                    {
                        OrgId = hrtPositionUpdate.VP.oper.GetValueOrDefault(0).ToString(),
                        Longitude = hrtPositionUpdate.VP.@long.GetValueOrDefault(),
                        Latitude = hrtPositionUpdate.VP.lat.GetValueOrDefault(),
                        VehicleId = vehicleId,
                        Heading = (int)hrtPositionUpdate.VP.hdg.GetValueOrDefault(),
                        DoorsOpen = hrtPositionUpdate.VP.drst == 1,
                        Timestamp = hrtPositionUpdate.VP.tst.GetValueOrDefault().Ticks,
                        Speed = hrtPositionUpdate.VP.spd.GetValueOrDefault()

                    };
                    var actorId = new ActorId(vehicleId.ToString());
                    var proxy = proxyFactory.CreateActorProxy<IVehicleActor>(actorId, nameof(VehicleActor));
                    await proxy.PositionChanged(position);
                }
                      
            return Ok();
        }
    }
}
