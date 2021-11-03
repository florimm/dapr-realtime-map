using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Actors;
using RealtimeMap.API.Models;

namespace RealtimeMap.API.Actors
{
    public class VehicleActor : IActor
    {
        public async Task PositionChanged(Position position)
        {
            
        }
    }
}
