// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using Softeq.XToolkit.Common.Files;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel
{
    /// <summary>
    /// Please, avoid extending this class.
    /// Use it only if you don't have another options to add reference on service.
    /// </summary>
    public static class Dependencies
    {
        private static bool _isInitialized;
        private static IIocContainer _iocContainer;

        public static void Initialize(IIocContainer iocContainer)
        {
            if (_isInitialized)
            {
                throw new ArgumentException($"{nameof(Dependencies)} already initialized");
            }

            _iocContainer = iocContainer;
            _isInitialized = true;
        }

        public static IIocContainer IocContainer => _iocContainer;
        
        public static IPageNavigationService PageNavigationService => IocContainer.Resolve<IPageNavigationService>();

        public static IPermissionRequestHandler PermissionRequestHandler =>
            IocContainer.Resolve<IPermissionRequestHandler>();

        public static IJsonSerializer JsonSerializer => IocContainer.Resolve<IJsonSerializer>();
        public static IFilesProvider InternalStorageProvider => IocContainer.Resolve<InternalStorageProvider>();
        public static IPermissionsManager PermissionsManager => IocContainer.Resolve<IPermissionsManager>();
    }
}