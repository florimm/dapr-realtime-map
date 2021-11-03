using System.Collections.Generic;

namespace RealtimeMap.API.DTO
{
    public class OrganizationDetailsDto : OrganizationDto
    {
        public IReadOnlyList<GeofenceDto> Geofences { get; set; }
    }
}