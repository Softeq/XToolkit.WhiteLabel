// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Location
{
    public class LocationModel
    {
        public LocationModel() { }

        public LocationModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}