using System.Linq;
using Microsoft.Maui.Networking;

namespace Softeq.XToolkit.Connectivity
{
    public static class Extensions
    {
        public static bool IsConnected(this ConnectivityChangedEventArgs args)
        {
            var profiles = args.ConnectionProfiles;
            var access = args.NetworkAccess;

            var hasAnyConnection = profiles.Any();
            var hasInternet = access == NetworkAccess.Internet;

            return hasAnyConnection && hasInternet;
        }
    }
}