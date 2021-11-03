﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace RealtimeMap.API.Models
{
    public class CircularGeofence
    {
        public string Name { get; }
        public double RadiusInMetres { get; }
        public GeoCoordinate Coordinate { get; }

        public CircularGeofence(string name, GeoPoint centralPoint, double radiusInMetres)
        {
            RadiusInMetres = radiusInMetres;
            Name = name;

            Coordinate = new GeoCoordinate(centralPoint.Latitude, centralPoint.Longitude);
        }

        public bool IncludesLocation(double latitude, double longitude)
        {
            return Coordinate.GetDistanceTo(new GeoCoordinate(latitude, longitude)) <= RadiusInMetres;
        }
    }
}
