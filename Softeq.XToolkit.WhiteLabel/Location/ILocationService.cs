// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Location
{
    /// <summary>
    ///     Provides methods for getting current location.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        ///     Gets a value indicating whether location service is enabled.
        /// </summary>
        bool IsLocationServiceEnabled { get; }

        /// <summary>
        ///     Returns the current location of the device.
        /// </summary>
        /// <returns>Returns the location.</returns>
        Task<LocationModel?> GetCurrentLocation();
    }
}
