// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;

namespace Softeq.XToolkit.WhiteLabel.Location
{
    public interface ILocationService
    {
        bool IsLocationServiceEnabled { get; }

        Task<LocationModel?> GetCurrentLocation();
    }
}