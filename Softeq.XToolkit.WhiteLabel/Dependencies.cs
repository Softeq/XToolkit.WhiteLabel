// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Files;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel
{
    /// <summary>
    ///     Static class that contains references to application services.
    ///     <para/>
    ///     Please, avoid using this class.
    ///     Use it only if you don't have other options to add service reference.
    /// </summary>
    public static class Dependencies
    {
        /// <summary>
        ///     Gets a value indicating whether the <see cref="Dependencies"/> service has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        ///     Gets the <see cref="IContainer"/> instance that will be used for resolving dependencies.
        /// </summary>
        public static IContainer Container { get; private set; } = default!;

        public static IPageNavigationService PageNavigationService => Resolve<IPageNavigationService>();

        public static IJsonSerializer JsonSerializer => Resolve<IJsonSerializer>();

        public static IFileProvider InternalStorageProvider => Resolve<InternalStorageFileProvider>();

        private static T Resolve<T>() where T : notnull
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Dependencies service is not initialized!");
            }

            return Container.Resolve<T>();
        }

        /// <summary>
        ///     Initializes <see cref="Dependencies"/> service with the specified <see cref="IContainer"/>.
        /// </summary>
        /// <param name="iocContainer">IoC container that will be used for resolving dependencies.</param>
        public static void Initialize(IContainer iocContainer)
        {
            if (IsInitialized)
            {
                throw new InvalidOperationException($"{nameof(Dependencies)} class is already initialized");
            }

            if (iocContainer == null)
            {
                throw new ArgumentNullException(nameof(iocContainer));
            }

            Container = iocContainer;
            IsInitialized = true;
        }
    }
}
