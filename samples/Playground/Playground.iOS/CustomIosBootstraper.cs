// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.Connectivity.iOS;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Essentials.iOS.ImagePicker;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.iOS
{
    internal class CustomIosBootstrapper : IosBootstrapper
    {
        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            base.ConfigureIoc(builder);

            // core
            CustomBootstrapper.Configure(builder);

            builder.PerDependency<StoryboardDialogsService, IDialogsService>();

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
