// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Connectivity;
using System.Collections.Generic;
using System.Reflection;
using Playground.Droid.Extended;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.Droid;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.ImagePicker;
using Softeq.XToolkit.WhiteLabel.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Navigation;

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

            builder.Singleton<DroidExtendedDialogsService, IDialogsService>();
            builder.Singleton<DefaultAlertBuilder, IAlertBuilder>();
            builder.Singleton<ConnectivityService, IConnectivityService>();

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();
            builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();

            builder.Singleton<DroidImagePickerService, IImagePickerService>();
        }
    }
}
