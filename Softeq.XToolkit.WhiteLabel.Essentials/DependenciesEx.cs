// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Permissions;

namespace Softeq.XToolkit.WhiteLabel.Essentials
{
    public static class DependenciesEx
    {
        public static IPermissionsManager PermissionsManager => Dependencies.Container.Resolve<IPermissionsManager>();
    }
}
