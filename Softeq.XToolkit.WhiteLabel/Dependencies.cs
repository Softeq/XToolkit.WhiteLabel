// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Files;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel
{
    /// <summary>
    ///     Please, avoid extending this class.
    ///     Use it only if you don't have another options to add reference on service.
    /// </summary>
    public static class Dependencies
    {
        public static bool IsInitialized { get; private set; }

        public static void Initialize(IContainer iocContainer)
        {
            if (IsInitialized)
            {
                throw new ArgumentException($"{nameof(Dependencies)} already initialized");
            }

            Container = iocContainer;
            IsInitialized = true;
        }

        public static IContainer Container { get; private set; } = default!;

        public static IPageNavigationService PageNavigationService => Container.Resolve<IPageNavigationService>();

        public static IPermissionRequestHandler PermissionRequestHandler =>
            Container.Resolve<IPermissionRequestHandler>();

        public static IJsonSerializer JsonSerializer => Container.Resolve<IJsonSerializer>();
        public static IFilesProvider InternalStorageProvider => Container.Resolve<InternalStorageProvider>();
        public static IPermissionsManager PermissionsManager => Container.Resolve<IPermissionsManager>();
    }
}
