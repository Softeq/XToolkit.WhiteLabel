﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Location
{
    public class LocationModel : IEquatable<LocationModel>
    {
        public LocationModel()
        {
        }

        public LocationModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LocationModel);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        public bool Equals(LocationModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Latitude == other.Latitude &&
                   Longitude == other.Longitude;
        }
    }
}