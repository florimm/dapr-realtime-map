namespace RealtimeMap.API.Models
{
    public class Position
    {
        public bool IsWithinViewport(Viewport viewport)
        {
            // naive implementation, ignores edge cases
            if (viewport.SouthWest is null || viewport.NorthEast is null)
            {
                return false;
            }

            return this.Longitude >= viewport.SouthWest.Longitude &&
                   this.Latitude >= viewport.SouthWest.Longitude &&
                   this.Longitude <= viewport.NorthEast.Longitude &&
                   this.Latitude <= viewport.NorthEast.Latitude;
        }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
        public long Timestamp { get; set; }
        public int Heading { get; set; }
        public string VehicleId { get; set; }
        public bool DoorsOpen { get; set; }
        public string OrgId { get; set; }
        public double Speed { get; set; }
    }
}