// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Playground.Extended;
using Playground.iOS.Extended;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.Connectivity.iOS;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Essentials.iOS.ImagePicker;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS
{
    internal class CustomIosBootstrapper : IosBootstrapperBase
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies()     // Softeq.XToolkit.WhiteLabel.iOS
                .AddItem(GetType().Assembly);  // Playground.iOS
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<IosExtendedDialogsService, IExtendedDialogsService>();

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();

            // image picker
            builder.Singleton<IosImagePickerService, IImagePickerService>();

            // connectivity
            builder.Singleton<IosConnectivityService, IConnectivityService>();
        }
    }
}
