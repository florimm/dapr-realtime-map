using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealtimeMap.API.Models;
using RealtimeMap.API.MQQT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeMap.API.Controllers
{
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        // [Topic("mqtt-binding", "hfp")]
        [HttpPost("mqtt-binding")]
        public async Task<IActionResult> Subscribe(Root2 hrtPositionUpdate)
        {
            if (hrtPositionUpdate != null && hrtPositionUpdate.VP != null) {
                Console.WriteLine($"Data => {hrtPositionUpdate.VP.lat} {hrtPositionUpdate.VP.@long} {hrtPositionUpdate.VP.hdg}");
            }
            else
            {
                Console.WriteLine($"NoData => {hrtPositionUpdate}");
            }
            // await Task.CompletedTask;
            // var vehicleId = $"{hrtPositionUpdate.OperatorId}.{hrtPositionUpdate.VehicleNumber}";

            // var position = new Position
            // {
            //     OrgId = hrtPositionUpdate.OperatorId,
            //     Longitude = hrtPositionUpdate.VehiclePosition.Long.GetValueOrDefault(),
            //     Latitude = hrtPositionUpdate.VehiclePosition.Lat.GetValueOrDefault(),
            //     VehicleId = vehicleId,
            //     Heading = (int)hrtPositionUpdate.VehiclePosition.Hdg.GetValueOrDefault(),
            //     DoorsOpen = hrtPositionUpdate.VehiclePosition.Drst == 1,
            //     Timestamp = hrtPositionUpdate.VehiclePosition.Tst.GetValueOrDefault().Ticks,
            //     Speed = hrtPositionUpdate.VehiclePosition.Spd.GetValueOrDefault()
            // };

            // Console.WriteLine(position.VehicleId);
            return Ok();
        }
    }
}
