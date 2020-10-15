// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Location;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Location.LocationModelTests
{
    public class LocationModelTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(3.14, -3.14)]
        [InlineData(-5.02, -4)]
        [InlineData(1.004, 8.2)]
        public void LocationModel_WhenInitializedWithValues_InitializesPropertiesCorrectly(double latitude, double longitude)
        {
            var locationModel = new LocationModel(latitude, longitude);

            Assert.Equal(latitude, locationModel.Latitude);
            Assert.Equal(longitude, locationModel.Longitude);
        }

        [Fact]
        public void LocationModel_WhenInitializedWithoutValues_InitializesPropertiesToZero()
        {
            var locationModel = new LocationModel();

            Assert.Equal(0, locationModel.Latitude);
            Assert.Equal(0, locationModel.Longitude);
        }
    }
}
