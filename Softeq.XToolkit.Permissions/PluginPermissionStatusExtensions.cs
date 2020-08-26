// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.ComponentModel;
using PluginPermissionStatus = Xamarin.Essentials.PermissionStatus;

namespace Softeq.XToolkit.Permissions
{
    public static class PluginPermissionStatusExtensions
    {
        public static PermissionStatus ToPermissionStatus(this PluginPermissionStatus permissionStatus)
        {
            return permissionStatus switch
            {
                PluginPermissionStatus.Denied => PermissionStatus.Denied,
                PluginPermissionStatus.Disabled => PermissionStatus.Denied,
                PluginPermissionStatus.Granted => PermissionStatus.Granted,
                PluginPermissionStatus.Restricted => PermissionStatus.Denied,
                PluginPermissionStatus.Unknown => PermissionStatus.Unknown,
                _ => throw new InvalidEnumArgumentException(
                    nameof(permissionStatus),
                    (int) permissionStatus,
                    permissionStatus.GetType())
            };
        }
    }
}
