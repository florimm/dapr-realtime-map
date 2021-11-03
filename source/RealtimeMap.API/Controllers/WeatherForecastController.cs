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
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Topic("mqtt-binding", "/hfp/v2/journey/ongoing/vp/bus/#")]
        [HttpPost()]
        public async Task<IActionResult> Subscribe(HrtPositionUpdate hrtPositionUpdate)
        {
            await Task.CompletedTask;
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
            return Ok();
        }
    }
}
