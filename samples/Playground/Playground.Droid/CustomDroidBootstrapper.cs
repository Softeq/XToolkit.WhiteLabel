// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Connectivity;
using System.Collections.Generic;
using System.Reflection;
using Playground.Droid.Extended;
using Playground.Extended;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.ImagePicker;
using Softeq.XToolkit.WhiteLabel.ImagePicker;

namespace Playground.Droid
{
    internal class CustomDroidBootstrapper : DroidBootstrapperBase
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies()     // Softeq.XToolkit.WhiteLabel.Droid
                .AddItem(GetType().Assembly);  // Playground.Droid
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<DroidExtendedDialogsService, IExtendedDialogsService>();

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();
            builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();

            // image picker
            builder.Singleton<DroidImagePickerService, IImagePickerService>();

            // connectivity
            builder.Singleton<ConnectivityService, IConnectivityService>();
        }
    }
}
